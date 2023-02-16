namespace backend.Controllers;

using backend.GameLogic;
using backend.JavaScriptObjects;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

[ApiController]
[Route("[controller]")]
public class MoveRequestController : ControllerBase
{
  [HttpPost]
  public void MakeMove([FromBody] MoveObject moveObject)
  {
    Move move = moveObject.move;
    string gameId = moveObject.gameId;

    // The fen is retrieved from the database
    string connectionString = "server=localhost;user=root;database=server_database;port=3306;password=1111";
    MySqlConnection conn = new MySqlConnection(connectionString);
    conn.Open();

    string sql = $@"SELECT FEN FROM games WHERE GameId = ""{gameId}""";

    MySqlCommand cmd = new MySqlCommand(sql, conn);
    MySqlDataReader reader = cmd.ExecuteReader();

    string fen = "";

    while (reader.Read())
      fen = reader.GetString("FEN");
    // The move is requested
    var request = new MakeMoveRequest();
    fen = request.loadNewFen(fen, move);

    reader.Close();

    // Load the new fen into the database
    sql = $@"UPDATE games SET FEN = ""{fen}"" WHERE GameId = ""{gameId}""";
    MySqlCommand cmd2 = new MySqlCommand(sql, conn);
    cmd2.ExecuteNonQuery();

    conn.Close();
  }
}