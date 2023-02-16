using backend.GameLogic;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CreateGameController : ControllerBase
{
    [HttpGet]
    public IActionResult CreateGame()
    {
        var request = new CreateGameRequest();
        string gameId = request.CreateGame();
        return Ok(new { gameId });
    }
}