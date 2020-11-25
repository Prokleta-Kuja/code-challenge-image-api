using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core.Models;
using ImageApi.Core.Requests;
using ImageApi.Core.Responses;
using ImageApi.Core.Validators;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResults<ImageVM>>> Get([FromQuery] SearchImagesRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Results.Any())
                return NoContent();

            return Ok(response);
        }
        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ImageVM>> GetById(int id)
        {
            var request = new GetImageRequest { Id = id };
            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(ProblemDetails))]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ImageVM>> Post(IFormFile file, [FromForm] string description)
        {
            using var imageStream = file.OpenReadStream();
            var request = new UploadImageRequest
            {
                Description = description,
                Image = imageStream,
                Size = (uint)file.Length,
                Type = file.ContentType
            };
            var validator = new UploadImageValidator();
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    if (error.PropertyName == nameof(UploadImageRequest.Description))
                        ModelState.AddModelError(nameof(description), error.ErrorMessage);
                    else
                        ModelState.AddModelError(nameof(file), error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
    }
}
