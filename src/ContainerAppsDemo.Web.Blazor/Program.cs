using ContainerAppsDemo.Web.Blazor.Data;
using ContainerAppsDemo.Web.Blazor.Services;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);
//builder.Host.ConfigureAppConfiguration(config =>
// {
//     var daprClient = new DaprClientBuilder().Build();
//     var secretDescriptors = new List<DaprSecretDescriptor>
//        {
//            new DaprSecretDescriptor("secretstore")
//        };
//     config.AddDaprSecretStore("secretstore", secretDescriptors, daprClient);
// });

// Add services to the container.
builder.Services.AddControllers().AddDapr();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
