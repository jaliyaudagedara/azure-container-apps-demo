using ContainerAppsDemo.Web.Blazor.Data;
using Dapr.Client;

namespace ContainerAppsDemo.Web.Blazor.Services;

public class OrderService : IOrderService
{
    private readonly DaprClient _daprClient;
    private readonly string _serviceId;

    public OrderService(DaprClient daprClient, IConfiguration configuration)
    {
        _daprClient = daprClient;
        _serviceId = configuration.GetValue<string>("Services:Orders:ServiceId");
    }

    public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken = default)
    {
        return await _daprClient.InvokeMethodAsync<IEnumerable<Order>>(HttpMethod.Get,
            _serviceId,
            "orders",
            cancellationToken);
    }
}
