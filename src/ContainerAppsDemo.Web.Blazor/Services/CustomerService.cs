using ContainerAppsDemo.Web.Blazor.Data;
using Dapr.Client;

namespace ContainerAppsDemo.Web.Blazor.Services;

public class CustomerService : ICustomerService
{
    private readonly DaprClient _daprClient;
    private readonly string _serviceId;

    public CustomerService(DaprClient daprClient, IConfiguration configuration)
    {
        _daprClient = daprClient;
        _serviceId = configuration.GetValue<string>("Services:Customers:ServiceId");
    }

    public async Task<IEnumerable<Customer>> GetCustomers(CancellationToken cancellationToken = default)
    {
        return await _daprClient.InvokeMethodAsync<List<Customer>>(HttpMethod.Get,
            _serviceId,
            "customers",
            cancellationToken);
    }
}
