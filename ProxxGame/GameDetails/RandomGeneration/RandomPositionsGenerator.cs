using ProxxGame.Models;

namespace ProxxGame.GameDetails.RandomGeneration;

public class RandomPositionsGenerator
{
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