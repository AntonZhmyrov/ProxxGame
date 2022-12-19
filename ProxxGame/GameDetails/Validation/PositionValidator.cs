using ProxxGame.Models;

namespace ProxxGame.GameDetails.Validation;

public class PositionValidator : IPositionValidator
{
    private readonly GameSettings _gameSettings;

    public PositionValidator(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }

    public bool ValidateRanges(Position? position) =>
        position != null &&
        position.X > 0 && 
        position.X < _gameSettings.XNumberCells && 
        position.Y > 0 &&
        position.Y < _gameSettings.YNumberCells;
}