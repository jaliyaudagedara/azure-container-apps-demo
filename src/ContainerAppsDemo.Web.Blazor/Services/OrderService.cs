using ContainerAppsDemo.Web.Blazor.Data;
using Dapr.Client;

namespace ContainerAppsDemo.Web.Blazor.Services
{
    public class OrderService : IOrderService
    {
        private readonly DaprClient _daprClient;

        public OrderService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken = default)
        {
            IEnumerable<Order>? orders = await _daprClient.InvokeMethodAsync<IEnumerable<Order>>(HttpMethod.Get,
                "orders-api",
                "orders",
                cancellationToken);

            return orders;
        }
    }
}
