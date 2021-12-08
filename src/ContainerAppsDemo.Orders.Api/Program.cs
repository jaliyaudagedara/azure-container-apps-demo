WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/orders", () =>
{
    var orders = new List<Order>
    {
        new Order(1, new DateTime(2021, 11, 01)),
        new Order(2, new DateTime(2021, 11, 02)),
    };
    return orders;
})
.WithName("GetOrders");

app.Run();

internal record Order(int Id, DateTime OrderDate);