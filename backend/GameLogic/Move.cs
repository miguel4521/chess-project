namespace backend.GameLogic;

public class Move
{
    public Move(Position startPosition, Position endPosition)
    {
        start = startPosition;
        end = endPosition;
    }

    public Position start { get; set; }
    public Position end { get; set; }
}