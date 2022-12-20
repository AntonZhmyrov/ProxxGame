using ProxxGame.Models;

namespace ProxxGame.GameDetails.Validation;

/// <summary>
/// An interface for position validator
/// </summary>
public interface IPositionValidator
{
    /// <summary>
    /// Validates the ranges of X and Y coordinates of the Position object by certain business rules
    /// defined in the methods implementation
    /// </summary>
    /// <param name="position">The position to validate</param>
    bool ValidateRanges(Position? position);
}