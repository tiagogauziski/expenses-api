# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Expenses.API/Expenses.API.csproj", "Expenses.API/"]
COPY ["Expenses.Domain/Expenses.Domain.csproj", "Expenses.Domain/"]
COPY ["Expenses.Domain.Core/Expenses.Domain.Core.csproj", "Expenses.Domain.Core/"]
COPY ["Expenses.Infrastructure.SqlServer/Expenses.Infrastructure.SqlServer.csproj", "Expenses.Infrastructure.SqlServer/"]
COPY ["Expenses.Application/Expenses.Application.csproj", "Expenses.Application/"]
COPY ["Expenses.Application.IoC/Expenses.Application.IoC.csproj", "Expenses.Application.IoC/"]
COPY ["Expenses.Infrastructure.EventBus/Expenses.Infrastructure.EventBus.csproj", "Expenses.Infrastructure.EventBus/"]
RUN dotnet restore "Expenses.API/Expenses.API.csproj"

# copy everything else and build app
COPY . .
RUN dotnet publish -c release -o /app --no-restore /p:DebugType=None "/src/Expenses.API"

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Expenses.API.dll"]