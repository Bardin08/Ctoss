using System.Text.Json.Serialization;
using Ctoss.Models.Enums;

namespace Ctoss.Models;

public class Sorting
{
    [JsonPropertyName("colId")]
    public string Property { get; set; } = null!;

    [JsonPropertyName("sort")]
    public SortingOrder Order { get; set; }
}
