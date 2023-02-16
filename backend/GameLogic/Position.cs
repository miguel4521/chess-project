namespace backend.GameLogic;

public class Position
{
  public int y { get; set; }
  public int x { get; set; }

  public int ToBitboard()
  {
    // Convert the rank (y) to the corresponding bitboard index
    int index = (8 - y) * 8;

    // Add the file (x) to the index
    index += x - 1;

    return index;
  }
}