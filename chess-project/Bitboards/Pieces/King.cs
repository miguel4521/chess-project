﻿namespace chess_project.Bitboards.Pieces;

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

    public override List<Move> GenerateMoves(Position position)
    {
        List<Move> moves = new List<Move>(); // A list to store the generated moves

        Bitboard friendlyPieces = IsWhite ? position.GetWhitePieces() : position.GetBlackPieces();

        for (int square = 0; square < 64; square++)
        {
            if (this[square])
            {
                Bitboard attacks = MaskKingAttacks(square);
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
        return IsWhite ? 'k' : 'K';
    }
}