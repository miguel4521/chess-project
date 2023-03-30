using Microsoft.AspNetCore.Mvc;

namespace chess_project.Controllers;

[ApiController]
[Route("[controller]")]
public class GetAiMoveController : ControllerBase
{
    private readonly ILogger<GetAiMoveController> _logger;

    public GetAiMoveController(ILogger<GetAiMoveController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public Move GetAiMove([FromQuery] string guid)
    {
        // get the user's position class
        Position position = Position.Positions[guid];
        Move move = position.GetBestMove();
        position.MakeMove(move);
        return move;
    }
}