using System;
using System.Collections.Generic;
using System.Linq;

namespace SRC.DiscordBot.Common;

public sealed class DiscordCodeBlockTable(IEnumerable<int?> columnWidths, string separator = " ")
{
    private readonly List<int?> _columnWidths = columnWidths.ToList();
    private readonly List<List<string>> _rows = [];

    public void AddRows(IEnumerable<IEnumerable<string>> rows)
    {
        foreach (var row in rows) AddRow(row);
    }

    public DiscordCodeBlockTable AddRows<T>(IEnumerable<T> rows, Func<T, IEnumerable<string>> factory)
    {
        foreach (var row in rows) AddRow(factory(row));
        
        return this;
    }

    public DiscordCodeBlockTable AddRow(IEnumerable<string> row)
    {
        _rows.Add(ValidateNewRow([.. row]));
        return this;
    }

    public DiscordCodeBlockTable AddRowParams(params ReadOnlySpan<string> row)
    {
        _rows.Add(ValidateNewRow(row.ToArray()));
        return this;
    }

    public override string ToString()
    {
        var actualWidths = Enumerable.Range(0, _columnWidths.Count)
            .Select(GetActualColumnWidth)
            .ToList();

        var ls = new List<string>();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var row in _rows)
        {
            var row1 = row;
            var paddedRow = row
                .Select((col, idx) =>
                {
                    var width = actualWidths[idx];

                    // Do not pad the right-most column
                    if (idx + 1 < row1.Count) col = col.PadRight(width);

                    return col[..Math.Min(col.Length, width)];
                });

            ls.Add(string.Join(separator, paddedRow));
        }

        return $"```{string.Join("\n", ls)}```";
    }

    private int GetActualColumnWidth(int columnIndex)
    {
        var colMax = _columnWidths[columnIndex];

        var actualColWidth = _rows.Max(r => r[columnIndex].Length);

        return colMax.HasValue ? Math.Min(actualColWidth, colMax.Value) : actualColWidth;
    }

    private List<string> ValidateNewRow(string[] row)
    {
        var result = row.ToList();

        while (row.Length < _columnWidths.Count) result.Add("-");

        return result.Count > _columnWidths.Count ? 
            throw new Exception("DiscordCodeBlockTable instance had too many columns") : 
            result;
    }
}