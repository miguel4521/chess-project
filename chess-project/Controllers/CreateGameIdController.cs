using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace chess_project.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateGameIdController : ControllerBase
{
    private readonly ILogger<CreateGameIdController> _logger;

    public CreateGameIdController(ILogger<CreateGameIdController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public CreateGameId Get()
    {
        string guid = Guid.NewGuid().ToString();
        Position.Positions.Add(guid, new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"));
        return new CreateGameId { Guid = guid };
    }
}