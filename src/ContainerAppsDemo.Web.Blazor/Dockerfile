#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["src/ContainerAppsDemo.Web.Blazor/ContainerAppsDemo.Web.Blazor.csproj", "src/ContainerAppsDemo.Web.Blazor/"]
RUN dotnet restore "src/ContainerAppsDemo.Web.Blazor/ContainerAppsDemo.Web.Blazor.csproj"
COPY . .
WORKDIR "/src/ContainerAppsDemo.Web.Blazor"
RUN dotnet build "ContainerAppsDemo.Web.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContainerAppsDemo.Web.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContainerAppsDemo.Web.Blazor.dll"]