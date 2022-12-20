using ProxxGame.Models;

namespace ProxxGame.GameDetails.RandomGeneration;

/// <summary>
/// The class which is responsible for random generation of Position objects
/// </summary>
public static class RandomPositionsGenerator
{
    /// <summary>
    /// Generates a certain amount of positions with random coordinates
    /// </summary>
    /// <param name="xNumberCells">The total amount of cells in X-axis</param>
    /// <param name="yNumberCells">The total amount of cells in Y-axis</param>
    /// <param name="numberOfPositions">Total number of positions to generate</param>
    /// <returns>An array of generated random positions</returns>
    public static Position[] Generate(int xNumberCells, int yNumberCells, int numberOfPositions)
    {
        var random = new Random();
        var positions = new List<Position>(numberOfPositions);

        for (int i = 0; i < numberOfPositions; i++)
        {
            bool positionExists;

            do
            {
                var xPosition = random.Next(0, xNumberCells);
                var yPosition = random.Next(0, yNumberCells);

                // If the Position object with given coordinates already exists, the flag is put to true to
                // regenerate the coordinates for this position
                if (positions.Any(position => position.X == xPosition && position.Y == yPosition))
                {
                    positionExists = true;
                }
                else
                {
                    positions.Add(new Position(xPosition, yPosition));
                    positionExists = false;
                }
                
            } while (positionExists);
        }

        return positions.ToArray();
    }
}