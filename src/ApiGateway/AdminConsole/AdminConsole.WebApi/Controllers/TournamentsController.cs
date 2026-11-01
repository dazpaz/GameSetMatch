using Microsoft.AspNetCore.Http.HttpResults;
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

			if (result.IsFailure) return BadRequest(result.Error);
			
			return Ok(result.Value);
		}

		[HttpGet("{id}", Name = "GetTournament")]
		public ActionResult<TournamentDto> GetTournament(Guid id)
		{
			var result = _queryTournament.GetTournamentById(id);

			if (result.IsFailure) return BadRequest(result.Error);
			if (result.Value == null) return NotFound();

			return Ok(result.Value);
		}

		[HttpPost(Name = "CreateTournament")]
		public ActionResult Create(AddTournamentDto addTournamentDto)
		{
			var command = AddTournamentCommand.CreateFromDto(addTournamentDto);
			if (command.IsFailure) return BadRequest(command.Error);

			var result = _tournamentCommandHandler.HandleAddTournamentCommand(command.Value);
			if (result.IsFailure) return BadRequest(result.Error);

			return result.IsSuccess
				? CreatedAtAction(nameof(GetTournament), new { id = result.Value }, null)
				: BadRequest(result.Error);
		}
	}
}
