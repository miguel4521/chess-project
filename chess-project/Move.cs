using System.Diagnostics;
using chess_project.Bitboards;

namespace chess_project;

public struct Move
{
    public int From { get; }
    public int To { get; }
    public int PieceIndex { get; }
    public int CapturedPieceIndex { get; }
    public bool IsEnPassant { get; }
    public int? CapturedSquare { get; }
    public bool IsCastling { get; }
    public int? CastlingRookFrom { get; }
    public int? CastlingRookTo { get; }
    public bool[] CastlingRightsBeforeMove { get; set; }
    public int? EnPassantTargetBeforeMove { get; set; }
    public Bitboard AttackedSquares { get; }

    public Move(int from, int to, Position position, bool isEnPassant = false, int? capturedSquare = null,
        bool isCastling = false, int? castlingRookFrom = null, int? castlingRookTo = null)
    {
        From = from;
        To = to;
        PieceIndex = position.GetPieceIndexAt(from);
        CapturedPieceIndex =
            isEnPassant ? position.GetPieceIndexAt(capturedSquare.Value) : position.GetPieceIndexAt(to);
        IsEnPassant = isEnPassant;
        CapturedSquare = capturedSquare;
        IsCastling = isCastling;
        CastlingRookFrom = castlingRookFrom;
        CastlingRookTo = castlingRookTo;
        
        // Apply the move to the position and store the previous castling rights and en passant target square
        CastlingRightsBeforeMove = position.castlingRights.ToArray();
        EnPassantTargetBeforeMove = position.EnPassantTarget;

        AttackedSquares = position.attackedSquares;
    }

    public override string ToString()
    {
        return From + " " + To;
    }
}