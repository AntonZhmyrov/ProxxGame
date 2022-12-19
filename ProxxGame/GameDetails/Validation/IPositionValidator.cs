using ProxxGame.Models;

namespace ProxxGame.GameDetails.Validation;

public interface IPositionValidator
{
    bool ValidateRanges(Position? position);
}