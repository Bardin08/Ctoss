using Ctoss.Models.AgGrid;

namespace Ctoss;

public interface ICtossService
{
    Task<AgGridQueryResult<T>> ApplyAsync<T>(AgGridQuery query, IEnumerable<T> enumerable);
}