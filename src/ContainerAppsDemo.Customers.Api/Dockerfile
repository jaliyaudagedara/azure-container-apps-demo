#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["src/ContainerAppsDemo.Customers.Api/ContainerAppsDemo.Customers.Api.csproj", "src/ContainerAppsDemo.Customers.Api/"]
RUN dotnet restore "src/ContainerAppsDemo.Customers.Api/ContainerAppsDemo.Customers.Api.csproj"
COPY . .
WORKDIR "/src/ContainerAppsDemo.Customers.Api"
RUN dotnet build "ContainerAppsDemo.Customers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContainerAppsDemo.Customers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContainerAppsDemo.Customers.Api.dll"]