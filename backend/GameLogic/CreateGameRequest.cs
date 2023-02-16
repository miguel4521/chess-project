using MySql.Data.MySqlClient;

namespace backend.GameLogic;

public class CreateGameRequest
{
  private readonly string startpos = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

  // Creates a new game in the database and returns the game id
  public string CreateGame()
  {
    string connectionString = "server=localhost;user=root;database=server_database;port=3306;password=1111";
    MySqlConnection conn = new MySqlConnection(connectionString);
    conn.Open();

    Guid guid = Guid.NewGuid();
    // Perform database operations here
    string sql = $"INSERT INTO games (GameId, FEN, DateCreated, DateModified) VALUES('{guid}', '{startpos}', NOW(), NOW())";

    MySqlCommand cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();

    sql = $"SELECT GameId FROM games WHERE GameId = {guid}";

    return guid.ToString();
  }
}