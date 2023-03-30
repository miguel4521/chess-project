namespace chess_project.Bitboards.Pieces;

public abstract class Piece : Bitboard
{
    public bool IsWhite { get; set; }
    
    public abstract List<Move> GenerateMoves(Position position);
    
    public abstract Bitboard GetAttackMap(Position position);
    
    public abstract int Value { get; }
    
    public abstract char GetSymbol();
}