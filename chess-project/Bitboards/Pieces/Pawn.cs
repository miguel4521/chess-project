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

            bitboard[square] = true;

            // white pawns
            if (!IsWhite)
            {
                // generate pawn attacks
                if (((bitboard >> 7) & NotAFile) != 0) attacks |= bitboard << 7;
                if (((bitboard >> 9) & NotHFile) != 0) attacks |= bitboard << 9;
            }
            // black pawns
            else
            {
                // generate pawn attacks
                if (((bitboard << 7) & NotHFile) != 0) attacks |= bitboard >> 7;
                if (((bitboard << 9) & NotAFile) != 0) attacks |= bitboard >> 9;
            }

            // return attack map
            return attacks;
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

                    while (attacks != 0)
                    {
                        int targetSquare = attacks.LSB();
                        moves.Add(new Move(square, targetSquare, position));
                        attacks &= attacks - 1;
                    }

                    int singlePush = IsWhite ? square - 8 : square + 8;
                    int doublePush = IsWhite ? square - 16 : square + 16;

                    if (!position.GetOccupiedSquares()[singlePush])
                    {
                        moves.Add(new Move(square, singlePush, position));

                        if ((!IsWhite && square is >= 8 and <= 15) || (IsWhite && square is >= 48 and <= 55))
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
            return IsWhite ? 'p' : 'P';
        }
    }
}