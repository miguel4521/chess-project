using System.Diagnostics;

namespace chess_project.Bitboards.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(bool isWhite)
        {
            IsWhite = isWhite;
        }

        private Bitboard MaskPawnAttacks(int square)
        {
            // result attacks bitboard
            Bitboard attacks = 0UL;

            // piece bitboard
            Bitboard bitboard = 0UL;

            // set piece on board
            bitboard[square] = true;

            // white pawns
            if (!IsWhite)
            {
                // generate pawn attacks
                if (((bitboard >> 7) & NotAFile) != 0) attacks |= bitboard >> 7;
                if (((bitboard >> 9) & NotHFile) != 0) attacks |= bitboard >> 9;
            }
            // black pawns
            else
            {
                // generate pawn attacks
                if (((bitboard << 7) & NotHFile) != 0) attacks |= bitboard << 7;
                if (((bitboard << 9) & NotAFile) != 0) attacks |= bitboard << 9;
            }

            // return attack map
            return attacks;
        }

        public override Bitboard GetAttackMap(Position position)
        {
            Bitboard attackMap = new Bitboard();

            for (int square = 0; square < 64; square++)
            {
                if (this[square])
                    attackMap |= MaskPawnAttacks(square);
            }

            return attackMap;
        }

        public override List<Move> GenerateMoves(Position position)
        {
            List<Move> moves = new List<Move>();

            Bitboard enemyPieces = IsWhite ? position.GetBlackPieces() : position.GetWhitePieces();
            Bitboard ourPawns = this;

            for (int square = 0; square < 64; square++)
            {
                if (ourPawns[square])
                {
                    Bitboard attacks = MaskPawnAttacks(square) & enemyPieces;

                    if (position.EnPassantTarget.HasValue)
                    {
                        Bitboard enPassantTarget = 0UL;
                        enPassantTarget[position.EnPassantTarget.Value] = true;
                        Bitboard enPassantAttacks = MaskPawnAttacks(square) & enPassantTarget;

                        while (enPassantAttacks != 0)
                        {
                            int targetSquare = enPassantAttacks.LSB();
                            int capturedSquare = position.GetEnPassantCaptureSquare().Value;
                            moves.Add(new Move(square, targetSquare, position, true, capturedSquare));
                            enPassantAttacks &= enPassantAttacks - 1;
                        }
                    }

                    while (attacks != 0)
                    {
                        int targetSquare = attacks.LSB();
                        moves.Add(new Move(square, targetSquare, position));
                        attacks &= attacks - 1;
                    }

                    int singlePush = IsWhite ? square + 8 : square - 8;
                    int doublePush = IsWhite ? square + 16 : square - 16;

                    if (!position.GetOccupiedSquares()[singlePush])
                    {
                        moves.Add(new Move(square, singlePush, position));

                        if ((IsWhite && square is >= 8 and <= 15) || (!IsWhite && square is >= 48 and <= 55))
                        {
                            if (!position.GetOccupiedSquares()[doublePush])
                                moves.Add(new Move(square, doublePush, position));
                        }
                    }
                }
            }

            return moves;
        }

        public override char GetSymbol()
        {
            return IsWhite ? 'P' : 'p';
        }
    }
}