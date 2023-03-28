using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace chess_project.Controllers;

[ApiController]
[Route("[controller]")]
public class GetLegalMovesController : ControllerBase
{
    private readonly ILogger<GetLegalMovesController> _logger;

    public GetLegalMovesController(ILogger<GetLegalMovesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Move> Get([FromQuery]string guid)
    {
        // get the user's position class
        Position position = Position.Positions[guid];
        // get the legal moves
        List<Move> moves = position.GenerateMoves();
        return moves;
    }
}