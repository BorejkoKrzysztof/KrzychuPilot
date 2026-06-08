using KrzychuPilot.Application.Common.Models.CreatePromptGroupDto;
using KrzychuPilot.Application.Prompts.Commands.CreatePrompt;
using KrzychuPilot.Application.Prompts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KrzychuPilot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PromptsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePromptGroupDto dto)
        {
            var result = await _mediator.Send(new CreatePromptGroupCommand(dto.Prompts));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetPromptsQuery());


            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
