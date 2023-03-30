using System.Diagnostics;
using System.Text;
using chess_project.Bitboards;
using chess_project.Bitboards.Pieces;

namespace chess_project;

public class Position
{
    public static Dictionary<string, Position> Positions = new();

    // An array of bitboards, one for each piece type and color
    public readonly Piece[] Pieces;
    public bool WhiteToMove = true;

    public const int MaxSearchDepth = 4;

    public Bitboard attackedSquares = new();

    // left white rook, right white, left black, right black
    public bool[] castlingRights = { true, true, true, true };

    public int? EnPassantTarget { get; set; }

    private readonly Dictionary<char, int> pieceIndexFromChar = new()
    {
        { 'P', 0 },
        { 'p', 1 },
        { 'N', 2 },
        { 'n', 3 },
        { 'B', 4 },
        { 'b', 5 },
        { 'R', 6 },
        { 'r', 7 },
        { 'Q', 8 },
        { 'q', 9 },
        { 'K', 10 },
        { 'k', 11 }
    };

    // The constructor that initializes the bitboards to the initial position
    public Position(string fen)
    {
        Pieces = new Piece[12];
        Pieces[0] = new Pawn(true); // white pawns
        Pieces[1] = new Pawn(false); // black pawns
        Pieces[2] = new Knight(true); // white knights
        Pieces[3] = new Knight(false); // black knights
        Pieces[4] = new Bishop(true); // white bishops
        Pieces[5] = new Bishop(false); // black bishops
        Pieces[6] = new Rook(true); // white rooks
        Pieces[7] = new Rook(false); // black rooks
        Pieces[8] = new Queen(true); // white queens
        Pieces[9] = new Queen(false); // black queens
        Pieces[10] = new King(true); // white king
        Pieces[11] = new King(false); // black king
        FromFen(fen);
    }

    public void MakeMove(Move move)
    {
        // Move the piece
        Pieces[move.PieceIndex][move.To] = true;
        Pieces[move.PieceIndex][move.From] = false;

        UpdateCastlingRights(move);

        if (move.IsEnPassant)
        {
            Pieces[move.CapturedPieceIndex][move.CapturedSquare.Value] = false;
            // Set the en passant target square to null after the move
            EnPassantTarget = null;
        }
        else
        {
            // If the move is a capture, remove the captured piece
            if (move.CapturedPieceIndex != -1)
                Pieces[move.CapturedPieceIndex][move.To] = false;
            // Update the en passant target square if it's a double pawn push
            if (Math.Abs(move.To - move.From) == 16 && Pieces[move.PieceIndex] is Pawn)
            {
                int offset = WhiteToMove ? 8 : -8;
                EnPassantTarget = move.From + offset;
            }
            else
                // Set the en passant target square to null for other moves
                EnPassantTarget = null;
        }

        // Switch sides to move
        WhiteToMove = !WhiteToMove;

        SetAttackedSquares();
    }

    public List<Move> GenerateMoves()
    {
        List<Move> moves = new List<Move>();
        List<Move> legalMoves = new List<Move>();

        for (int i = WhiteToMove ? 0 : 1; i < 12 && Pieces[i] != 0; i += 2)
            moves.AddRange(Pieces[i].GenerateMoves(this));

        // Check each move for legality
        foreach (Move move in moves)
        {
            MakeMove(move);
            WhiteToMove = !WhiteToMove;
            if (!InCheck())
                legalMoves.Add(move);

            WhiteToMove = !WhiteToMove;
            UndoMove(move);
        }

        return legalMoves;
    }

    public bool InCheck()
    {
        int kingIndex = WhiteToMove ? 10 : 11;
        int kingSquare = Pieces[kingIndex].LSB();
        return attackedSquares[kingSquare];
    }

    public void UndoMove(Move move)
    {
        // Switch sides to move
        WhiteToMove = !WhiteToMove;

        // Undo the move
        Pieces[move.PieceIndex][move.From] = true;
        Pieces[move.PieceIndex][move.To] = false;

        if (move.CapturedPieceIndex != -1)
            Pieces[move.CapturedPieceIndex][move.To] = true;

        if (move.IsCastling)
        {
            // Move the rook back
            int rookFrom = move.CastlingRookFrom.Value;
            int rookTo = move.CastlingRookTo.Value;
            int rookIndex = move.PieceIndex == 10 ? 6 : 7;
            Pieces[rookIndex][rookFrom] = true;
            Pieces[rookIndex][rookTo] = false;
        }
        else if (move.IsEnPassant)
        {
            int capturedSquare = move.CapturedSquare.Value;
            int offset = WhiteToMove ? 8 : -8;
            int pawnSquare = capturedSquare + offset;
            Pieces[move.CapturedPieceIndex][capturedSquare] = true;
            Pieces[move.PieceIndex][pawnSquare] = true;
        }

        // Update en passant target square
        EnPassantTarget = move.EnPassantTargetBeforeMove;

        // Update castling rights
        castlingRights = move.CastlingRightsBeforeMove;

        // Unset the attacked squares bitboard
        attackedSquares = move.AttackedSquares;
    }


    public void SetAttackedSquares()
    {
        attackedSquares = 0;

        for (int i = WhiteToMove ? 0 : 1; i < 12; i += 2)
        {
            if (Pieces[i] != 0)
            {
                Bitboard attackMap = Pieces[i].GetAttackMap(this);

                // OR the attack map with the attacked squares bitboard
                attackedSquares |= attackMap;
            }
        }
    }

    public int? GetEnPassantCaptureSquare()
    {
        if (!EnPassantTarget.HasValue)
            return null;

        int offset = WhiteToMove ? -8 : 8;
        return EnPassantTarget.Value + offset;
    }

    private void UpdateCastlingRights(Move move)
    {
        if (move.IsCastling)
        {
            // Move the rook involved in castling
            int rookFrom = move.CastlingRookFrom.Value;
            int rookTo = move.CastlingRookTo.Value;
            int rookIndex = move.PieceIndex == 10 ? 6 : 7;
            Pieces[rookIndex][rookFrom] = false;
            Pieces[rookIndex][rookTo] = true;
        }
        else
        {
            switch (move.PieceIndex)
            {
                // Update castling rights
                // White king
                case 10:
                    castlingRights[0] = false;
                    castlingRights[1] = false;
                    break;
                // Black king
                case 11:
                    castlingRights[2] = false;
                    castlingRights[3] = false;
                    break;
                // White rook
                case 6 when move.From == 56:
                    castlingRights[0] = false;
                    break;
                case 6:
                {
                    if (move.From == 63)
                        castlingRights[1] = false;
                    break;
                }
                // Black rook
                case 7 when move.From == 0:
                    castlingRights[2] = false;
                    break;
                case 7:
                {
                    if (move.From == 7)
                        castlingRights[3] = false;
                    break;
                }
            }
        }
    }

    public Move GetBestMove()
    {
        Move bestMove = default;
        int bestScore = int.MinValue;
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int depth = MaxSearchDepth;

        List<Move> legalMoves = GenerateMoves();

        foreach (Move move in legalMoves)
        {
            MakeMove(move);
            int score = -Negamax(depth - 1, -beta, -alpha);
            UndoMove(move);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }

            alpha = Math.Max(alpha, score);
            if (beta <= alpha)
                break;
        }

        return bestMove;
    }

    private int Negamax(int depth, int alpha, int beta)
    {
        if (depth == 0 || GenerateMoves().Count == 0)
        {
            // For simplicity, we'll use the material balance as the evaluation function.
            return EvaluatePosition();
        }

        int bestScore = int.MinValue;

        List<Move> legalMoves = GenerateMoves();
        foreach (Move move in legalMoves)
        {
            MakeMove(move);
            int score = -Negamax(depth - 1, -beta, -alpha);
            UndoMove(move);

            bestScore = Math.Max(bestScore, score);
            alpha = Math.Max(alpha, score);

            if (beta <= alpha)
            {
                break;
            }
        }

        return bestScore;
    }

    public int EvaluatePosition()
    {
        int score = 0;

        for (int i = 0; i < Pieces.Length; i++)
        {
            int pieceCount = Pieces[i].Count();
            int pieceValue = Pieces[i].Value;
            score += (i % 2 == 0 ? 1 : -1) * pieceCount * pieceValue;
        }

        return WhiteToMove ? score : -score;
    }


    public int GetPieceIndexAt(int square)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece[square])
                return pieceIndexFromChar[piece.GetSymbol()];
        }

        return -1;
    }

    public Bitboard GetWhitePieces()
    {
        Bitboard whitePieces = new Bitboard();
        for (int i = 0; i < 12; i += 2)
            whitePieces |= Pieces[i];
        return whitePieces;
    }

    public Bitboard GetBlackPieces()
    {
        Bitboard blackPieces = new Bitboard();
        for (int i = 1; i < 12; i += 2)
            blackPieces |= Pieces[i];
        return blackPieces;
    }

    public Bitboard GetOccupiedSquares()
    {
        Bitboard occupied = new Bitboard();
        foreach (Piece piece in Pieces)
            occupied |= piece;
        return occupied;
    }

    public Bitboard GetEmptySquares()
    {
        return GetOccupiedSquares().GetEmptySquares();
    }

    public string ToFen()
    {
        StringBuilder fen = new StringBuilder();

        // Loop through ranks from top to bottom
        for (int rank = 7; rank >= 0; rank--)
        {
            int emptyCount = 0;

            // Loop through files from left to right
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                int pieceIndex = GetPieceIndexAt(square);

                if (pieceIndex == -1)
                    emptyCount++;
                else
                {
                    if (emptyCount != 0)
                    {
                        fen.Append(emptyCount);
                        emptyCount = 0;
                    }

                    char pieceSymbol = Pieces[pieceIndex].GetSymbol();
                    fen.Append(pieceSymbol);
                }
            }

            if (emptyCount != 0)
                fen.Append(emptyCount);

            if (rank > 0)
                fen.Append('/');
        }

        fen.Append(' ');

        // Side to move
        fen.Append(WhiteToMove ? 'w' : 'b');

        // TODO: Add castling rights, en passant square, half-move clock, and full-move number information.
        // For simplicity, assuming no castling rights, no en passant square, and starting from move 1.
        fen.Append(" - - 0 1");

        return fen.ToString();
    }

    public void FromFen(string fen)
    {
        string[] fenParts = fen.Trim().Split(' ');

        // Clear the board
        foreach (var piece in Pieces)
            piece.Clear();

        int rank = 7;
        int file = 0;

        // Loop through the piece placement section of the FEN string
        foreach (char c in fenParts[0])
        {
            if (char.IsDigit(c))
            {
                file += int.Parse(c.ToString());
            }
            else if (c == '/')
            {
                rank--;
                file = 0;
            }
            else
            {
                int pieceIndex = pieceIndexFromChar[c];
                bool isWhite = char.IsUpper(c);

                Pieces[pieceIndex][rank * 8 + file] = true;
                Pieces[pieceIndex].IsWhite = isWhite;

                file++;
            }
        }

        // Set side to move
        WhiteToMove = fenParts[1] == "w";

        // TODO: Set castling rights, en passant square, halfmove clock, and fullmove number from the FEN string.
        // For simplicity, assuming no castling rights, no en passant square, and starting from move 1.
    }
}