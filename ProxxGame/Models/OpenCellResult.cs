namespace ProxxGame.Models;

/// <summary>
/// This class is a result of operation of opening a cell by user
/// </summary>
public class OpenCellResult
{
    public OpenCellResult(Cell openedCell, bool isBlackHole, string openedCellsStatesString, bool isVictory)
    {
        Cell = openedCell;
        IsBlackHole = isBlackHole;
        OpenedCellsStatesString = openedCellsStatesString;
        IsVictory = isVictory;
    }
    
    /// <summary>
    /// An opened cell
    /// </summary>
    public Cell Cell { get; }
    
    /// <summary>
    /// Is the opened cell a black hole
    /// </summary>
    public bool IsBlackHole { get; }

    /// <summary>
    /// The string representation of current state of the game after the newly opened cell
    /// </summary>
    public string OpenedCellsStatesString { get; }
    
    /// <summary>
    /// Indicates whether the user won the game or not
    /// </summary>
    public bool IsVictory { get; }
}