using ContainerAppsDemo.Web.Blazor.Data;
using Dapr.Client;

namespace ContainerAppsDemo.Web.Blazor.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DaprClient _daprClient;

        public CustomerService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<IEnumerable<Customer>> GetCustomers(CancellationToken cancellationToken = default)
        {
            IEnumerable<Customer>? customers = null;
            try
            {
                customers = await _daprClient.InvokeMethodAsync<List<Customer>>(HttpMethod.Get,
                    "customers-api",
                    "customers",
                    cancellationToken);
            }
            catch (Exception ex)
            {

            }

            return customers;
        }
    }
}
