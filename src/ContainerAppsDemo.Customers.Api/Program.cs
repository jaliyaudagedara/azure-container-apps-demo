var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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