# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["src/Expenses.Worker.StatementCreator/Expenses.Worker.StatementCreator.csproj", "Expenses.Worker.StatementCreator/"]
COPY ["src/Expenses.Domain/Expenses.Domain.csproj", "Expenses.Domain/"]
COPY ["src/Expenses.Domain.Core/Expenses.Domain.Core.csproj", "Expenses.Domain.Core/"]
COPY ["src/Expenses.Infrastructure.SqlServer/Expenses.Infrastructure.SqlServer.csproj", "Expenses.Infrastructure.SqlServer/"]
COPY ["src/Expenses.Application/Expenses.Application.csproj", "Expenses.Application/"]
COPY ["src/Expenses.Application.IoC/Expenses.Application.IoC.csproj", "Expenses.Application.IoC/"]
COPY ["src/Expenses.Infrastructure.EventBus.InMemory/Expenses.Infrastructure.EventBus.InMemory.csproj", "Expenses.Infrastructure.EventBus.InMemory/"]
COPY ["nuget.config", "Expenses.Worker.StatementCreator/"]
RUN dotnet restore "Expenses.Worker.StatementCreator/Expenses.Worker.StatementCreator.csproj"

# copy everything else and build app
COPY "./src" .
RUN dotnet publish -c release -o /app --no-restore /p:DebugType=None "/src/Expenses.Worker.StatementCreator"

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Expenses.Worker.StatementCreator.dll"]