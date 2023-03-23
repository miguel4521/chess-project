namespace chess_project;

public struct Move
{
    public int From { get; }  
    public int To { get; }
    public int PieceIndex { get; }
    public int CapturedPieceIndex { get; }
    public bool IsEnPassant { get; }
    public int? CapturedSquare { get; }

    public Move(int from,int to, Position position, bool isEnPassant = false, int? capturedSquare = null )  
    {  
        From=from ;  
        To=to;
        PieceIndex = position.GetPieceIndexAt(from);
        CapturedPieceIndex = isEnPassant ? position.GetPieceIndexAt(capturedSquare.Value) : position.GetPieceIndexAt(to);
        IsEnPassant = isEnPassant;
        CapturedSquare = capturedSquare;
    }
    
    public override string ToString()
    {
        return  From + " " + To;
    }
}