namespace ProxxGame.Models;

public class OpenCellResult
{
    public OpenCellResult(Cell openedCell, bool isBlackHole, string openedCellsStatesString, bool isVictory)
    {
        Cell = openedCell;
        IsBlackHole = isBlackHole;
        OpenedCellsStatesString = openedCellsStatesString;
        IsVictory = isVictory;
    }
    
    public Cell Cell { get; }
    
    public bool IsBlackHole { get; }

    public string OpenedCellsStatesString { get; }
    
    public bool IsVictory { get; }
}