using Ctoss.Models.AgGrid;

namespace Ctoss;

public interface ICtossService
{
    AgGridQueryResult<T> Apply<T>(AgGridQuery query, IEnumerable<T> enumerable);
}