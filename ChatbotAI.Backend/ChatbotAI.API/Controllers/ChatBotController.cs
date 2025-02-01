using ChatbotAI.API.Application.Command.AddChatbotInteraction;
using ChatbotAI.API.Application.Command.RateAnswer;
using ChatbotAI.API.Application.Command.SaveAnswer;
using ChatbotAI.API.Contract.InteractWithChatbot;
using ChatbotAI.API.Contract.RateAnswer;
using ChatbotAI.API.Domain.Interfaces.Services;
using ChatbotAI.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text;

namespace ChatbotAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatbotHub> _hubContext;
        private readonly IChatbotService _chatbotService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ChatbotController(IChatbotService chatbotService, IMediator mediator, IHubContext<ChatbotHub> hubContext, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _chatbotService = chatbotService ?? throw new ArgumentNullException(nameof(chatbotService));
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpPost]
        public async Task<ActionResult<InteractWithChatbotResponse>> InteractWithChatbot(InteractWithChatbotRequest request)
        {
            var addChatbotCommand = new AddChatbotInteractionCommand
            {
                Question = request.Question,
            };

            var saveInteractionResult = await _mediator.Send(addChatbotCommand);

            _ = Task.Run(async () =>
            {
                var answerBuilder = new StringBuilder();
                var saveAnswerCommand = new SaveAnswerCommand()
                {
                    Id = saveInteractionResult.Id,
                };

                for (int i = 0; i <= saveInteractionResult.SectionList.Count - 1; i++)
                {
                    if (_chatbotService.IsAnswerStopped(saveInteractionResult.Id))
                    {
                        break;
                    }
                    foreach (var character in saveInteractionResult.SectionList[i])
                    {
                        if (_chatbotService.IsAnswerStopped(saveInteractionResult.Id))
                        {
                            break;
                        }
                        answerBuilder.Append(character);
                        await _hubContext.Clients.All.SendAsync("ReceiveAnswerPiece", saveInteractionResult.Id, character.ToString());
                        await Task.Delay(10);
                    }

                    if (i != saveInteractionResult.SectionList.Count - 1)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveAnswerPiece", saveInteractionResult.Id, "\n\n");
                        answerBuilder.Append("\n\n");
                        await Task.Delay(10);
                    }

                }

                if (_chatbotService.IsAnswerStopped(saveInteractionResult.Id))
                {
                    saveAnswerCommand.IsInterupted = true;

                    await _hubContext.Clients.All.SendAsync("ReceiveAnswerInterrupted", saveInteractionResult.Id);


                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveAnswerComplete", saveInteractionResult.Id);
                }

                saveAnswerCommand.AnswerContent = answerBuilder.ToString();
                Console.WriteLine("Sending command to Mediator.");


                using var scope = _serviceScopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(saveAnswerCommand);


                _chatbotService.RemoveStoppedAnswer(saveInteractionResult.Id);
            });

            var response = new InteractWithChatbotResponse()
            {
                Id = saveInteractionResult.Id,
                Question = request.Question,
                SectionList = new List<string>()
            };

            return Ok(response);
        }

     
        [HttpPost]
        [Route("Rate")]
       
        public async Task<IActionResult> RateAnswer([FromBody] RateAnswerRequest request)
        {
            var query = new RateAnswerCommand
            {
                Id = request.Id,
                Rating = request.Rating
            };

            await _mediator.Send(query);

            return Ok();
        }
    }
}
