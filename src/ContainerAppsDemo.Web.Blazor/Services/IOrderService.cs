using ContainerAppsDemo.Web.Blazor.Data;

namespace ContainerAppsDemo.Web.Blazor.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken = default);
}
