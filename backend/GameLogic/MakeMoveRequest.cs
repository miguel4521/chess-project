namespace backend.GameLogic;

public class MakeMoveRequest
{
  public string loadNewFen(string fen, Move move)
  {
    // Load the gamestate
    var gameState = new GameState(fen);
    // Make the move
    // gameState.makeMove(move);
    
    // Save the new fen
    fen = "";
    
    return fen;
  }
}