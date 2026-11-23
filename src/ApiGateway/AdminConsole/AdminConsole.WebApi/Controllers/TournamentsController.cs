using Microsoft.AspNetCore.Mvc;
using TourManagement.Application.Commands;
using TourManagement.Application.Handlers;
using TourManagement.Application.Queries;
using TourManagement.Contract;

namespace AdminConsole.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TournamentsController(ITournamentCommandHandler tournamentCommandHandler,
		IQueryTournament queryTournament) : ControllerBase
	{
		private readonly ITournamentCommandHandler _tournamentCommandHandler = tournamentCommandHandler;
		private readonly IQueryTournament _queryTournament = queryTournament;

		[HttpGet(Name = "GetTournaments")]
		public ActionResult<IEnumerable<TournamentDto>> GetTournaments()
		{
			var result = _queryTournament.GetAllTournaments();

			if (result.IsFailure) return StatusCode(StatusCodes.Status500InternalServerError, result.Error);

			return Ok(result.Value);
		}

		[HttpGet("{id}", Name = "GetTournament")]
		public ActionResult<TournamentDto> GetTournament(Guid id)
		{
			var result = _queryTournament.GetTournamentById(id);

			if (result.IsFailure) return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
			if (result.Value == null) return NotFound();

			return Ok(result.Value);
		}

		[HttpPost(Name = "AddTournament")]
		public IActionResult AddTournament(AddTournamentDto addTournamentDto)
		{
			var command = AddTournamentCommand.CreateFromDto(addTournamentDto);
			if (command.IsFailure) return BadRequest(command.Error);

			var result = _tournamentCommandHandler.HandleAddTournamentCommand(command.Value);
			if (result.IsFailure)
				return StatusCode(StatusCodes.Status500InternalServerError, result.Error);

			return CreatedAtAction(nameof(GetTournament), new { id = result.Value }, null);
		}

		[HttpPut("{id}", Name = "UpdateTournament")]
		public IActionResult UpdateTournament(Guid id, UpdateTournamentDto updateTournamentDto)
		{
			var command = UpdateTournamentCommand.CreateFromDto(id, updateTournamentDto);
			if (command.IsFailure) return BadRequest(command.Error);

			var result = _tournamentCommandHandler.HandleUpdateTournamentCommand(command.Value);
			if (result.IsFailure)
				return StatusCode(StatusCodes.Status500InternalServerError, result.Error);

			return NoContent();
		}

		[HttpPatch("{id}/active", Name = "SetTournamentActive")]
		public IActionResult SetTournamentActive(Guid id, SetTournamentActiveDto setTournamentActiveDto)
		{
			var command = SetTournamentActiveCommand.CreateFromDto(id, setTournamentActiveDto);
			if (command.IsFailure) return BadRequest(command.Error);

			var result = _tournamentCommandHandler.HandleSetTournamentActiveCommand(command.Value);
			if (result.IsFailure)
				return StatusCode(StatusCodes.Status500InternalServerError, result.Error);

			return NoContent();
		}
	}
}
