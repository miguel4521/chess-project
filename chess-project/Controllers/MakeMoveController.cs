using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace chess_project.Controllers;

[ApiController]
[Route("[controller]")]
public class MakeMoveController : ControllerBase
{
    private readonly ILogger<MakeMoveController> _logger;

    public MakeMoveController(ILogger<MakeMoveController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get([FromQuery]string guid, [FromQuery]int from, [FromQuery]int to)
    {
        // get the user's position class
        Position position = Position.Positions[guid];
        // find the move from generated moves (in case of en passant)
        Move move = position.GenerateMoves().Find(m => m.From == from && m.To == to);
        // make the move
        position.MakeMove(move);
        return Ok(new { message = "data processed successfully" });
    }
}