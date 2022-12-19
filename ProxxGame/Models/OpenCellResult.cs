namespace ProxxGame.Models;

public class OpenCellResult
{
    public OpenCellResult(Cell openedCell, string openedCellsStatesString, bool isVictory)
    {
        Cell = openedCell;
        OpenedCellsStatesString = openedCellsStatesString;
        IsVictory = isVictory;
    }
    
    public Cell Cell { get; }
    
    public string OpenedCellsStatesString { get; }
    
    public bool IsVictory { get; }
}