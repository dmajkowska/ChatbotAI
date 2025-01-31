using ChatbotAI.API.Application.Command.GenerateAnswer;
using ChatbotAI.API.Application.Command.RateAnswer;
using ChatbotAI.API.Application.Command.TruncateAnswer;
using ChatbotAI.API.Contract.GenerateAnswer;
using ChatbotAI.API.Contract.RateAnswer;
using ChatbotAI.API.Contract.TruncateAnswer;
using ChatbotAI.API.Domain.Interfaces.Services;
using ChatbotAI.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatbotAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatbotHub> _hubContext;
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService, IMediator mediator, IHubContext<ChatbotHub> hubContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _chatbotService = chatbotService ?? throw new ArgumentNullException(nameof(chatbotService));
        }

        [HttpPost]
        public async Task<ActionResult<GenerateAnswerResponse>> GenerateAnswer(GenerateAnswerRequest request)
        {
            var query = new GenerateAnswerCommand
            {
                Question = request.Question,
            };

            var result = await _mediator.Send(query);

            _ = Task.Run(async () =>
            {
                for (int i = 0; i <= result.SectionList.Count - 1; i++)
                {
                    if (_chatbotService.IsAnswerStopped(result.Id))
                    {
                        break;
                    }
                    foreach (var character in result.SectionList[i])
                    {
                        if (_chatbotService.IsAnswerStopped(result.Id))
                        {
                            break;
                        }
                        await _hubContext.Clients.All.SendAsync("ReceiveAnswerPiece", result.Id, character.ToString());
                        await Task.Delay(10);
                    }
                    
                    if(i != result.SectionList.Count - 1)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveAnswerPiece", result.Id, "\n\n");
                        await Task.Delay(10);
                    }
                    
                }

                if (_chatbotService.IsAnswerStopped(result.Id))
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveAnswerTruncated", result.Id);
                } else
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveAnswerComplete", result.Id);
                }
                
                _chatbotService.RemoveStoppedAnswer(result.Id);
            });

            var response = new GenerateAnswerResponse()
            {
                Id = result.Id,
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

        [HttpPost]
        [Route("Truncate")]

        public async Task<IActionResult> TruncateAnswer([FromBody] TruncateAnswerRequest request)
        {
            var query = new TruncateAnswerCommand
            {
                Id = request.Id,
                DisplayedCharactersCount = request.DisplayedCharactersCount
            };

            await _mediator.Send(query);

            return Ok();
        }
    }
}
