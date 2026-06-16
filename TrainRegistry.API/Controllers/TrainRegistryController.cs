using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainRegistry.Application.Trains.Commands.CreateTrain;
using TrainRegistry.Application.Trains.Queries.GetAllTrains;
using TrainRegistry.Application.Trains.Queries.GetTrainById;
using TrainRegistry.Api.Mappers;
using TrainRegistry.Domain;
using TrainRegistry.API.DTOs.Requests;

namespace TrainRegistry.src.TrainRegistry.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TrainRegistryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrainRegistryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> GetTrainById(Guid guid, CancellationToken cancellationToken)
        {
            var train = await _mediator.Send(new GetTrainByIdQuery(guid), cancellationToken);

            if (train == null)
            {
                return NotFound($"Train with id {guid} was not found.");
            }

            return Ok(Mapper.ToDTO(train));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrains(CancellationToken cancellationToken)
        {
            var trains = await _mediator.Send(new GetAllTrainsQuery(), cancellationToken);

            if (!trains.Any())
            {
                return NotFound("No train was found!");
            }

            return Ok(trains.Select(Mapper.ToDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrain([FromBody]CreateTrainRequest train, CancellationToken cancellationToken)
        {
            var createTrainCommand = new CreateTrainCommand(train.Name, train.Length, train.Speed);

            var guid = await _mediator.Send(createTrainCommand, cancellationToken);
            return CreatedAtAction(nameof(GetTrainById), new { guid}, null);
        }
    }
}
