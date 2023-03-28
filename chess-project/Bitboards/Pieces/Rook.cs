using System.Diagnostics;

namespace chess_project.Bitboards.Pieces;

public class Rook : SlidingPiece
{
    public Rook(bool isWhite)
    {
        IsWhite = isWhite;
    }

    public override Bitboard GetAttackMap(Position position)
    {
        Bitboard attackMap = new Bitboard();

        for (int square = 0; square < 64; square++)
        {
            if (this[square])
                attackMap |= GetRookAttacks(square, position.GetOccupiedSquares());
        }

        return attackMap;
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>();

        Bitboard friendlyPieces = IsWhite ? position.GetWhitePieces() : position.GetBlackPieces();

        for (int square = 0; square < 64; square++)
        {
            if (this[square])
            {
                Bitboard attacks = GetRookAttacks(square, position.GetOccupiedSquares());
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
        return IsWhite ? 'R' : 'r';
    }
}