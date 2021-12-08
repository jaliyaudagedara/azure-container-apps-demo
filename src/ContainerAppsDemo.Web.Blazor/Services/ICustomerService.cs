using ContainerAppsDemo.Web.Blazor.Data;

namespace ContainerAppsDemo.Web.Blazor.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetCustomers(CancellationToken cancellationToken = default);
}
