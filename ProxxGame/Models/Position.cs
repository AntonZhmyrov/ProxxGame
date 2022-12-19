namespace ProxxGame.Models;

public class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public int X { get; }
    
    public int Y { get; }
    
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

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
    
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