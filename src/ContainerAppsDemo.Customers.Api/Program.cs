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
	List<Customer> customers = new()
	{
		new(1, "John Doe"),
		new(2, "Jane Doe"),
		new(3, "John Smith"),
	};

	return TypedResults.Ok(customers);
})
.WithName("GetCustomers");

app.Run();

internal record Customer(int Id, string Name);