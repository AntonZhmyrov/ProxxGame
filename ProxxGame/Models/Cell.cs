namespace ProxxGame.Models;

/// <summary>
/// The class that represents a Cell object on the game board
/// </summary>
// I decided to keep black holes and simple cells as one object, because all of the behavior is technically the same
// apart of few properties, and inheriting the BlackHole from the Cell will not bring any advantages to the solution
// but might complicate it with additional type checking
public class Cell : IComparable<Cell>
{
    // Each cell knows all its neighbours, so it means that the game board is not just a two-dimensional array
    // but also a graph, where each cell has relations with all the neighbours
    // This is needed to make the operation of OpenCell() to be fast and performant, especially when the opened cell
    // has 0 of adjacent cell and somehow needs to signal to all surrounding cells to open up
    private readonly List<Cell> _neighbours;

    private Cell()
    {
        _neighbours = new List<Cell>();
        IsOpen = false;
    }

    /// <summary>
    /// The flag that indicates whether the cell is a black hole or not
    /// </summary>
    private bool IsBlackHole { get; init; }

    /// <summary>
    /// The flag that indicates whether the cell is open or not
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// This is a calculated property which calculates the number of adjacent holes for a simple cell
    /// </summary>
    public int AdjacentHoles => IsBlackHole ? -1 : _neighbours.Count(cell => cell.IsBlackHole);

    /// <summary>
    /// Position of the cell on the game board
    /// </summary>
    public Position Position { get; private init; } = null!;

    /// <summary>
    /// The method to add neighbouring cells to the cell
    /// </summary>
    /// <param name="neighbours">The collection of neighbours</param>
    public void AddNeighbours(IEnumerable<Cell> neighbours) => _neighbours.AddRange(neighbours);
    
    /// <summary>
    /// The main method of the Cell object. It opens up a cell modifying its internal state
    /// </summary>
    /// <param name="openedCells">The collection of already opened cells needed to track down the state of game</param>
    /// <returns>True if the opened cell is a black hole - otherwise it returns false</returns>
    public bool Open(SortedSet<Cell> openedCells)
    {
        // When a cell is already opened - we don't waste time and just short circuit here
        if (IsOpen)
        {
            return IsBlackHole;
        }

        IsOpen = true;

        // If the cell is not tracked as an opened cell - track it writing it to the collection
        if (!openedCells.Contains(this))
        {
            openedCells.Add(this);
        }

        // If the cell is black hole or the cell's adjacent black holes are not equal to 0 - short circuit here
        // For black hole situation, the Game class will identify the further actions.
        // The cell's responsibility is just to open up and tell what it is
        if (IsBlackHole || AdjacentHoles != 0)
        {
            return IsBlackHole;
        }

        // Also, the cell's responsibility is to notify all its neighbours to open up (except for holes)
        // when the adjacent holes for the cell is 0
        var neighboursToOpen = _neighbours.Where(neighbour => !neighbour.IsBlackHole && !neighbour.IsOpen);
        
        foreach (var neighbourToOpen in neighboursToOpen)
        {
            neighbourToOpen.Open(openedCells);
        }
        
        return IsBlackHole;
    }

    // Needed in order to compare Cell objects between each other
    public override bool Equals(object? obj)
    {
        //Check for null and compare run-time types.
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Cell cell = (Cell)obj;
        return Position.Equals(cell.Position);
    }

    // Needed in order to compare Cell objects between each other
    public override int GetHashCode() => Position.GetHashCode();

    /// <summary>
    /// Returns a string representation of cell's position, whether it is a hole and if not, how many
    /// adjacent holes exist around the cell
    /// </summary>
    public override string ToString() =>
        IsBlackHole 
            ? $"Cell: [{Position.X},{Position.Y}]. IT IS HOLE!"
            : $"Cell: [{Position.X},{Position.Y}]. Adjacent holes: {AdjacentHoles}";

    /// <summary>
    /// This is an implementation of IComparable interface, which is needed for automatic sorting of cells in our SortedSet
    /// Basically, it implements the behavior needed for the collection in order to sort our Cell objects
    /// </summary>
    public int CompareTo(Cell? other)
    {
        if (other == null)
        {
            return 1;
        }

        // The comparisons of X coordinate go first, as they are the first one to compare with 
        // If X coordinate of the current Cell is less than the X coordinate of other Cell
        // then the current cell precedes other cell in the sort order.
        if (Position.X < other.Position.X)
        {
            return -1;
        }

        // If X coordinate of the current Cell is greater than the X coordinate of other Cell
        // then the current cell follows other cell in the sort order.
        if (Position.X > other.Position.X)
        {
            return 1;
        }

        // We compare Y coordinates when X coordinates are equal
        // If Y coordinate of the current Cell is less than the Y coordinate of other Cell
        // then the current cell precedes other cell in the sort order.
        if (Position.Y < other.Position.Y)
        {
            return -1;
        }
        
        // If Y coordinate of the current Cell is greater than the Y coordinate of other Cell
        // then the current cell follows other cell in the sort order.
        if (Position.Y > other.Position.Y)
        {
            return 1;
        }

        return 0;
    }
    
    /// <summary>
    /// This is a static method needed to determine the positions of future neighbours of the cell
    /// It is static, as it is called at the point when the neighbours instances of the cell are not yet defined
    /// as objects, but we already know their future positions
    /// </summary>
    /// <param name="xPosition">The X coordinate of the cell for which to determine neighbours</param>
    /// <param name="yPosition">The Y coordinate of the cell for which to determine neighbours</param>
    /// <param name="xNumberCells">The number of cells in X axis of game board</param>
    /// <param name="yNumberCells">The number of cells in Y axis of game board</param>
    /// <returns></returns>
    public static Position[] GetNeighbouringCellsPositions(
        int xPosition, int yPosition, int xNumberCells, int yNumberCells)
    {
        var neighbouringPositions = new List<Position>();

        // These for loop and an inner for loop define the positions of neighbour cells
        // Technically, they implement the logic which allows to find all the neighbours like in some examples below
        // easy example --> [4,5] is our cell
        // Adjacent cells for it would be --> [3,4], [3,5], [3,6], [4,4], [4,6], [5,4], [5,5], [5,6]

        // Harder example
        // [0,2] is our cell
        // Adjacent cells for it would be --> [-1,1], [-1,2], [-1,3], [0,1], [0,3], [1,1], [1,2], [1,3]

        // Another Harder example
        // [9,0] is our cell
        // Adjacent cells for it would be --> [8,-1], [8,0], [8,1], [9,-1], [9,1], [10,-1], [10,0], [10,1]
        for (int x = xPosition - 1; x < xPosition + 2; x++)
        {
            if (x < 0 || x >= xNumberCells)
            {
                continue;
            }

            for (int y = yPosition - 1; y < yPosition + 2; y++)
            {
                if (y < 0 || y >= yNumberCells || (x == xPosition && y == yPosition))
                {
                    continue;
                }

                neighbouringPositions.Add(new Position(x, y));
            }
        }

        return neighbouringPositions.ToArray();
    }
    
    /// <summary>
    /// Creator method to instantiate a simple cell
    /// </summary>
    public static Cell CreateSimpleCell(Position position)
    {
        return new Cell
        {
            IsBlackHole = false,
            Position = position
        };
    }

    /// <summary>
    /// Creator method to instantiate a black hole
    /// </summary>
    public static Cell CreateBlackHole(Position position)
    {
        return new Cell
        {
            IsBlackHole = true,
            Position = position
        };
    }
}