WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/customers", () =>
{
    var customers = new List<Customer>
    {
        new Customer(1, "John Doe"),
        new Customer(2, "Jane Doe")
    };
    return customers;
})
.WithName("GetCustomers");

app.Run();

internal record Customer(int Id, string Name);