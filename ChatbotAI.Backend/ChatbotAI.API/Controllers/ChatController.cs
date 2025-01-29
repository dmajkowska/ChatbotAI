using ChatbotAI.API.Application.Command.GenerateAnswer;
using ChatbotAI.API.Application.Command.RateAnswer;
using ChatbotAI.API.Contract.GenerateAnswer;
using ChatbotAI.API.Contract.RateAnswer;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ChatbotAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController: ControllerBase
    {

        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
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
    }
}
