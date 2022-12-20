namespace ProxxGame.Models;

/// <summary>
/// The class which represents a cell position on the game board with X and Y coordinates
/// </summary>
public class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public int X { get; }
    
    public int Y { get; }
    
    // Needed in order to compare Position objects between each other
    public override bool Equals(object? obj)
    {
        //Check for null and compare run-time types.
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Position position = (Position) obj;
        return X == position.X && Y == position.Y;
    }

    // Needed in order to compare Position objects between each other
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Tries to parse x and y coordinates of string types into the Position object
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="inputPosition">The parsed Position object</param>
    /// <returns>Returns true if the parsing was successful - otherwise returns false</returns>
    public static bool TryParse(string? x, string? y, out Position? inputPosition)
    {
        inputPosition = null;
        
        var xSuccess = int.TryParse(x, out var convertedX);
        var ySuccess = int.TryParse(y, out var convertedY);

        if (!xSuccess || !ySuccess)
        {
            return false;
        }
        
        inputPosition = new Position(convertedX, convertedY);
        return true;
    }
}