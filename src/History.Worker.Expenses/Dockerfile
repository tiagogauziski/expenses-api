# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["src/History.Worker.Expenses/*.csproj", "History.Worker.Expenses/"]
COPY ["src/Expenses.Domain/*.csproj", "Expenses.Domain/"]
COPY ["src/Expenses.Infrastructure.EventBus/*.csproj", "Expenses.Infrastructure.EventBus/"]
COPY ["src/Expenses.Infrastructure.EventBus.RabbitMQ/*.csproj", "Expenses.Infrastructure.EventBus.RabbitMQ/"]
COPY ["src/Expenses.Infrastructure.EventBus.ServiceBus/*.csproj", "Expenses.Infrastructure.EventBus.ServiceBus/"]
COPY ["src/Expenses.OpenTelemetry/*.csproj", "Expenses.OpenTelemetry/"]
RUN dotnet restore "History.Worker.Expenses/History.Worker.Expenses.csproj"

# copy everything else and build app
COPY "./src" .
RUN dotnet publish -c release -o /app --no-restore /p:DebugType=None "/src/History.Worker.Expenses"

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "History.Worker.Expenses.dll"]