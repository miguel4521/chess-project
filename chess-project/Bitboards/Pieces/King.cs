using System.Collections;
using System.Diagnostics;

namespace chess_project.Bitboards.Pieces;

public class King : Piece
{
    public King(bool isWhite)
    {
        IsWhite = isWhite;
    }

    // generate king attacks
    private Bitboard MaskKingAttacks(int square)
    {
        // result attacks bitboard
        Bitboard attacks = 0UL;

        // piece bitboard
        Bitboard bitboard = 0UL;
        bitboard[square] = true;

        // generate king attacks
        if (bitboard >> 8 != 0) attacks |= bitboard >> 8;
        if (((bitboard >> 9) & NotHFile) != 0) attacks |= bitboard >> 9;
        if (((bitboard >> 7) & NotAFile) != 0) attacks |= bitboard >> 7;
        if (((bitboard >> 1) & NotHFile) != 0) attacks |= bitboard >> 1;
        if (bitboard << 8 != 0) attacks |= bitboard << 8;
        if (((bitboard << 9) & NotAFile) != 0) attacks |= bitboard << 9;
        if (((bitboard << 7) & NotHFile) != 0) attacks |= bitboard << 7;
        if (((bitboard << 1) & NotAFile) != 0) attacks |= bitboard << 1;

        // return attack map
        return attacks;
    }

    public override Bitboard GetAttackMap(Position position)
    {
        return MaskKingAttacks(LSB());
    }

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>(); // A list to store the generated moves

        Bitboard friendlyPieces = IsWhite ? position.GetWhitePieces() : position.GetBlackPieces();

        int square = LSB(); // Get the square of the king
        
        Bitboard attacks = MaskKingAttacks(square);
        Bitboard validMoves = attacks & ~friendlyPieces;

        // Add normal moves and captures
        while (validMoves != 0)
        {
            moves.Add(new Move(square, validMoves.LSB(), position));
            validMoves &= validMoves - 1;
        }

        // Add castling moves
        if (IsWhite && position.WhiteToMove)
        {
            if (position.castlingRights[1] && position.GetEmptySquares()[5] && position.GetEmptySquares()[6])
                moves.Add(new Move(square, square + 2, position, isCastling: true, castlingRookFrom: 7,
                    castlingRookTo: 5));
            if (position.castlingRights[0] && position.GetEmptySquares()[1] && position.GetEmptySquares()[2] &&
                position.GetEmptySquares()[3])
                moves.Add(new Move(square, square - 2, position, isCastling: true, castlingRookFrom: 0,
                    castlingRookTo: 3));
        }
        else if (!IsWhite && !position.WhiteToMove)
        {
            if (position.castlingRights[3] && position.GetEmptySquares()[61] && position.GetEmptySquares()[62])
                moves.Add(new Move(square, square + 2, position, isCastling: true, castlingRookFrom: 63,
                    castlingRookTo: 61));
            if (position.castlingRights[2] && position.GetEmptySquares()[59] && position.GetEmptySquares()[58] &&
                position.GetEmptySquares()[57])
                moves.Add(new Move(square, square - 2, position, isCastling: true, castlingRookFrom: 56,
                    castlingRookTo: 59));
        }


        return moves; // Return the list of moves generated
    }

    public override char GetSymbol()
    {
        return IsWhite ? 'K' : 'k';
    }
}