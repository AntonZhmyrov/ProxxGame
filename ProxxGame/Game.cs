using ProxxGame.GameDetails;
using ProxxGame.GameDetails.RandomGeneration;
using ProxxGame.Models;
using ProxxGame.Models.Extensions;

namespace ProxxGame;

/// <summary>
/// This is a class which is technically responsible for all the game play process
/// </summary>
public class Game
{
    // The board data structure is a two-dimensional array. It is a best choice of data structure in this case as 
    // it completely represents the board
    private readonly Cell[,] _gameBoard;
    private readonly GameSettings _gameSettings;
    
    // The opened cells are kept in a SortedSet collection.
    // This is done because I needed to somehow store already opened cells and not do the traverse of _gameBoard
    // data structure every time a new cell is opened + it arranges all the cells in the sorted by position ascending
    // order. That way the opened cells are well structured and are easier to output for the end user
    private readonly SortedSet<Cell> _openedCells;
    private readonly int _totalCells;
    private readonly Position[] _blackHolesPositions;

    public Game(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _gameBoard = new Cell[_gameSettings.XNumberCells, _gameSettings.YNumberCells];
        _openedCells = new SortedSet<Cell>();
        _totalCells = _gameSettings.XNumberCells * _gameSettings.YNumberCells;
        
        // Random generation of black holes positions
        _blackHolesPositions = RandomPositionsGenerator.Generate(
            _gameSettings.XNumberCells, _gameSettings.YNumberCells, _gameSettings.NumberOfBlackHoles);

        // Initialize the game board with cells and black holes given their positions, which were generated on the previous step
        InitializeCells(_blackHolesPositions);
    }

    /// <summary>
    /// This is the main method of the game which opens up a new cell with the specific position provided by user
    /// </summary>
    public OpenCellResult OpenCell(Position positionToOpen)
    {
        var cell = _gameBoard[positionToOpen.X, positionToOpen.Y];
        var isBlackHole = cell.Open(_openedCells);

        // If the user fails and opens up a black hole, we open all the black holes in the game
        // That's how the classic Proxx game works
        if (isBlackHole)
        {
            OpenUpAllBlackHoles();
        }

        return new OpenCellResult(cell, isBlackHole, _openedCells.OutputCellStates(), CheckIfVictory());
    }

    private void InitializeCells(Position[] blackHolesPositions)
    {
        // In this method 2 generic loops exist
        // The first is used to generate a Cell object making it either a simple cell or
        // a black hole and to assign it to the position on game board
        // The second is used to fulfill each cell object on game board with all the neighbouring cells
        
        // Initialize cells as cells or black holes with certain positions
        for (int x = 0; x < _gameSettings.XNumberCells; x++)
        {
            for (int y = 0; y < _gameSettings.YNumberCells; y++)
            {
                // In here, the check is made, whether our black holes positions array contains the position we are currently in
                // If yes - then we create a black hole, otherwise - it is a simple cell
                var cell = blackHolesPositions.Any(position => position.X == x && position.Y == y)
                    ? Cell.CreateBlackHole(new Position(x, y))
                    : Cell.CreateSimpleCell(new Position(x, y));

                _gameBoard[x, y] = cell;
            }
        }

        // Initialize cells with their neighbouring cells
        for (int x = 0; x < _gameSettings.XNumberCells; x++)
        {
            for (int y = 0; y < _gameSettings.YNumberCells; y++)
            {
                Position[] positions = 
                    Cell.GetNeighbouringCellsPositions(x, y, _gameSettings.XNumberCells, _gameSettings.YNumberCells);
                
                Cell cell = _gameBoard[x, y];
                
                cell.AddNeighbours(positions.Select(position => _gameBoard[position.X, position.Y]));
            }
        }
    }

    /// <summary>
    /// This method checks whether the game is in the state of Victory
    /// We subtract from total cell the number of opened cells and if it equals to the number of
    /// black holes in the game - than it returns true as for victory
    /// </summary>
    private bool CheckIfVictory() => _totalCells - _openedCells.Count == _gameSettings.NumberOfBlackHoles;
    
    /// <summary>
    /// This method opens up all the black holes on the game board
    /// </summary>
    private void OpenUpAllBlackHoles()
    {
        var allBlackHoles = new List<Cell>();

        // All the black holes' positions are clear and we just take them and pick the cells which are black holes 
        // from the game board and then open them up
        foreach (Position blackHolePosition in _blackHolesPositions)
        {
            allBlackHoles.Add(_gameBoard[blackHolePosition.X, blackHolePosition.Y]);
        }
        
        allBlackHoles.ForEach(blackHole => blackHole.Open(_openedCells));
    }
}