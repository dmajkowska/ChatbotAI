using ChatbotAI.API.Application.Command.GenerateAnswer;
using ChatbotAI.API.Application.Command.RateAnswer;
using ChatbotAI.API.Application.Command.TruncateAnswer;
using ChatbotAI.API.Contract.GenerateAnswer;
using ChatbotAI.API.Contract.RateAnswer;
using ChatbotAI.API.Contract.TruncateAnswer;
using ChatbotAI.API.Infrastructure.Hubs;
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

        public ChatbotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<GenerateAnswerResponse>> GenerateAnswer(GenerateAnswerRequest request)
        {
            var query = new GenerateAnswerCommand
            {
                Question = request.Question,
            };

            var result = await _mediator.Send(query);

            return result;
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
