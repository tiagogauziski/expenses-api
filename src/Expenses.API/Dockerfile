FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Expenses.API/Expenses.API.csproj", "Expenses.API/"]
RUN dotnet restore "Expenses.API/Expenses.API.csproj"
COPY . .
WORKDIR "/src/Expenses.API"
RUN dotnet build "Expenses.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Expenses.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Expenses.API.dll"]