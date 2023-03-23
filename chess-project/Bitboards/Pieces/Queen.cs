namespace chess_project.Bitboards.Pieces;

public class Queen : SlidingPiece
{
    public Queen(bool isWhite)
    {
        IsWhite = isWhite;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();
        
        Bitboard friendlyPieces = IsWhite ? position.GetWhitePieces() : position.GetBlackPieces();

        for (int square = 0; square < 64; square++)
        {
            if (this[square])
            {
                Bitboard bishopAttacks = GetBishopAttacks(square, position.GetOccupiedSquares());
                Bitboard rookAttacks = GetRookAttacks(square, position.GetOccupiedSquares());
                Bitboard attacks = bishopAttacks | rookAttacks; 
                Bitboard validMoves = attacks & ~friendlyPieces;

                // Add normal moves and captures
                while (validMoves != 0)
                {
                    moves.Add(new Move(square, validMoves.LSB(), position));
                    validMoves &= validMoves - 1;
                }
            }
        }

        return moves; // Return the list of moves generated
    }
    
    public override char GetSymbol()
    {
        return IsWhite ? 'q' : 'Q';
    }
}