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
    private bool WhiteToMove = true;

    public int? EnPassantTarget { get; set; }

    private readonly Dictionary<char, int> pieceIndexFromChar = new()
    {
        { 'p', 0 },
        { 'P', 1 },
        { 'n', 2 },
        { 'N', 3 },
        { 'b', 4 },
        { 'B', 5 },
        { 'r', 6 },
        { 'R', 7 },
        { 'q', 8 },
        { 'Q', 9 },
        { 'k', 10 },
        { 'K', 11 }
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
                int offset = WhiteToMove ? -8 : 8;
                EnPassantTarget = move.From + offset;
            }
            else
                // Set the en passant target square to null for other moves
                EnPassantTarget = null;
        }

        // Switch sides to move
        WhiteToMove = !WhiteToMove;
    }

    public List<Move> GenerateMoves()
    {
        List<Move> moves = new List<Move>();
        // Generate moves for all pieces, depending on the side to move
        for (int i = WhiteToMove ? 0 : 1; i < 12; i += 2)
        {
            if (Pieces[i] != 0)
                moves.AddRange(Pieces[i].GenerateMoves(this));
        }

        return moves;
    }

    public int? GetEnPassantCaptureSquare()
    {
        if (!EnPassantTarget.HasValue)
            return null;

        int offset = WhiteToMove ? 8 : -8;
        return EnPassantTarget.Value + offset;
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
                bool isWhite = !char.IsUpper(c);

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