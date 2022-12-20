using System.Text;

namespace ProxxGame.Models.Extensions;

/// <summary>
/// The extension class which extends Cell objects functionality
/// </summary>
public static class CellExtensions
{
    /// <summary>
    /// Outputs the state of all cells which were passed to this method
    /// </summary>
    public static string OutputCellStates(this IEnumerable<Cell> cells)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Cell states: ");

        foreach (var cell in cells)
        {
            stringBuilder.AppendLine($"\t{cell.ToString()}");
        }

        return stringBuilder.ToString();
    }
}