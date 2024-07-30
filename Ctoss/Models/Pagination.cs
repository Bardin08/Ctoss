namespace Ctoss.Models;

public record Pagination
{
    public int StartRow { get; init; }
    public int EndRow { get; init; }
}