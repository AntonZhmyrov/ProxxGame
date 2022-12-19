using ProxxGame.GameDetails;
using ProxxGame.GameDetails.RandomGeneration;
using ProxxGame.Models;
using ProxxGame.Models.Extensions;

namespace ProxxGame;

public class Game
{
    private readonly Cell[,] _gameBoard;
    private readonly GameSettings _gameSettings;
    private readonly SortedSet<Cell> _openedCells;
    private readonly int _totalCells;

    public Game(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _gameBoard = new Cell[_gameSettings.XNumberCells, _gameSettings.YNumberCells];
        _openedCells = new SortedSet<Cell>();
        _totalCells = _gameSettings.XNumberCells * _gameSettings.YNumberCells;
        
        Position[] blackHolesPositions = RandomPositionsGenerator.Generate(
            _gameSettings.XNumberCells, _gameSettings.YNumberCells, _gameSettings.NumberOfBlackHoles);

        InitializeCells(blackHolesPositions);
    }

    public OpenCellResult OpenCell(Position positionToOpen)
    {
        var cell = _gameBoard[positionToOpen.X, positionToOpen.Y];
        cell.Open(_openedCells);

        return new OpenCellResult(cell, _openedCells.OutputCellStates(), CheckIfVictory());
    }

    private void InitializeCells(Position[] blackHolesPositions)
    {
        // Initialize cells as cells or black holes with certain positions
        for (int x = 0; x < _gameSettings.XNumberCells; x++)
        {
            for (int y = 0; y < _gameSettings.YNumberCells; y++)
            {
                var cell = blackHolesPositions.Any(position => position.X == x && position.Y == y)
                    ? Cell.GenerateBlackHole(new Position(x, y))
                    : Cell.GenerateSimpleCell(new Position(x, y));

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

    private bool CheckIfVictory()
    {
        return _totalCells - _openedCells.Count == _gameSettings.NumberOfBlackHoles;
    }
}