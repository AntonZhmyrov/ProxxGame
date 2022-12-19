namespace ProxxGame.Models;

public class Cell : IComparable<Cell>
{
    private readonly List<Cell> _neighbours;

    private Cell()
    {
        _neighbours = new List<Cell>();
        IsOpen = false;
    }

    public bool IsHole { get; private init; }

    public bool IsOpen { get; private set; }

    public int AdjacentHoles => IsHole ? -1 : _neighbours.Count(cell => cell.IsHole);

    public Position Position { get; private init; } = null!;

    public void AddNeighbours(IEnumerable<Cell> neighbours)
    {
        _neighbours.AddRange(neighbours);
    }

    public static Position[] GetNeighbouringCellsPositions(int xPosition, int yPosition, int xNumberCells,
        int yNumberCells)
    {
        // Find all the adjacent cells for the given cell
        // easy example --> [4,5] is our cell
        // Adjacent cells for it would be --> [3,4], [3,5], [3,6], [4,4], [4,6], [5,4], [5,5], [5,6]

        // Harder example
        // [0,2] is our cell
        // Adjacent cells for it would be --> [-1,1], [-1,2], [-1,3], [0,1], [0,3], [1,1], [1,2], [1,3]

        // Another Harder example
        // [9,0] is our cell
        // Adjacent cells for it would be --> [8,-1], [8,0], [8,1], [9,-1], [9,1], [10,-1], [10,0], [10,1]

        var neighbouringPositions = new List<Position>();

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

    public void Open(SortedSet<Cell> openedCells, bool forceOpenAll = false)
    {
        if (IsOpen)
        {
            return;
        }

        IsOpen = true;

        if (!openedCells.Contains(this))
        {
            openedCells.Add(this);
        }

        if (forceOpenAll || IsHole)
        {
            BroadcastOpenCellToNeighbours(neighbour => !neighbour.IsOpen, openedCells, forceOpenAll: true);
            
            return;
        }

        if (AdjacentHoles != 0)
        {
            return;
        }

        BroadcastOpenCellToNeighbours(neighbour => !neighbour.IsHole && !neighbour.IsOpen, openedCells, forceOpenAll);
    }

    public static Cell GenerateSimpleCell(Position position)
    {
        return new Cell
        {
            IsHole = false,
            Position = position
        };
    }

    public static Cell GenerateBlackHole(Position position)
    {
        return new Cell
        {
            IsHole = true,
            Position = position
        };
    }

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

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public override string ToString()
    {
        return IsHole 
            ? $"Cell: [{Position.X},{Position.Y}]. IT IS HOLE!"
            : $"Cell: [{Position.X},{Position.Y}]. Adjacent holes: {AdjacentHoles}";
    }

    public int CompareTo(Cell? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (Position.X < other.Position.X)
        {
            return -1;
        }

        if (Position.X > other.Position.X)
        {
            return 1;
        }

        if (Position.Y < other.Position.Y)
        {
            return -1;
        }
        
        if (Position.Y > other.Position.Y)
        {
            return 1;
        }

        return 0;
    }

    private void BroadcastOpenCellToNeighbours(Func<Cell, bool> filterPredicate, SortedSet<Cell> openedCells, bool forceOpenAll)
    {
        var neighboursToOpen = _neighbours.Where(filterPredicate);
        
        foreach (var neighbourToOpen in neighboursToOpen)
        {
            neighbourToOpen.Open(openedCells, forceOpenAll);
        }
    }
}