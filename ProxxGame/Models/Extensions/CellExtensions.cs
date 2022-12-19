using System.Text;

namespace ProxxGame.Models.Extensions;

public static class CellExtensions
{
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