using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRegistry.Api.Mappers;
using TrainRegistry.API.Controllers;
using TrainRegistry.API.DTOs.Requests;
using TrainRegistry.Application.Trains.Commands.CreateTrain;
using TrainRegistry.Application.Trains.Commands.UpdateTrainStatus;
using TrainRegistry.Application.Trains.Queries.GetAllTrains;
using TrainRegistry.Application.Trains.Queries.GetTrainById;

namespace TrainRegistry.src.TrainRegistry.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TrainRegistryController : ApiController
    {
        private readonly IMediator _mediator;

        public TrainRegistryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{guid:guid}")]
        [Authorize]
        public async Task<IActionResult> GetTrainById(Guid guid, CancellationToken cancellationToken)
        {
            var trainResponse = await _mediator.Send(new GetTrainByIdQuery(guid), cancellationToken);

            return trainResponse.Match
                (
                    Ok,
                    errors => Problem(errors)                 
                );
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTrains(CancellationToken cancellationToken)
        {
            var trains = await _mediator.Send(new GetAllTrainsQuery(), cancellationToken);

            return trains.Match(
              Ok,
                errors => Problem(errors)
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrain([FromBody]CreateTrainRequest train, CancellationToken cancellationToken)
        {
            var createTrainCommand = new CreateTrainCommand(train.Name, train.Length, train.Speed);

            var guid = await _mediator.Send(createTrainCommand, cancellationToken);
            return CreatedAtAction(nameof(GetTrainById), new { guid}, null);
        }

        [HttpPut("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrainStatus([FromBody] UpdateTrainStatusRequest updateTrainStatusRequest, CancellationToken cancellationToken)
        {
            var updateTrainCommand = new UpdateTrainStatusCommand(updateTrainStatusRequest.TrainId, updateTrainStatusRequest.TrainStatus);

            var result = await _mediator.Send(updateTrainCommand, cancellationToken);

            return result.Match(
                updated => NoContent(),
                error => Problem(error));
        }
    }
}
