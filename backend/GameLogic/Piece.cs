namespace backend.GameLogic;

public class Piece
{
    public Piece(Position position, char color, char type)
    {
        this.position = position;
        this.color = color;
        this.type = type;
    }
    public Position position { get; set; }
    public char color { get; set; }
    public char type { get; set; }
}