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
	List<Order> orders = new()
	{
		new Order(1, new DateTime(2022, 11, 01)),
		new Order(2, new DateTime(2022, 11, 02)),
	};

	return TypedResults.Ok(orders);
})
.WithName("GetOrders");

app.Run();

internal record Order(int Id, DateTime OrderDate);