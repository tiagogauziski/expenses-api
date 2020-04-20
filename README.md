# Expenses API
Open source project written in .NET Core 3.1.

The goal of this project is to implement a sample API with commonly used technologies and different architecture choices.

[![Build Status](https://travis-ci.com/tiagogauziski/expenses-api.svg?branch=master)](https://travis-ci.com/tiagogauziski/expenses-api)

# How to test
Execute the following command on the project folder:
```
dotnet test
``` 

# Technology Stack
* .NET Core 3.1
* Entity Framework Core 3.1
* AutoMapper
* MediaR
* FluentValidation

# Architecture
* SOLID 
* Domain Driven Design (Layers and Domain Model pattern)
* Event Sourcing
* CQRS (Basic implementation)
* Repository Pattern	

# Few handy commands
## Docker MSSQL Linux
```
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Network@123' -p 1433:1433 -v mssql_volume:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
```

## Execute Migrations (make sure to have database created and update appsettings connection string)
```
dotnet ef database update --startup-project ..\Expenses.API\
```

## Preview SQL Server Migrations from dotnet ef tool
```
dotnet ef migrations script --startup-project ..\Expenses.API\
```

## Generate Migration
```
dotnet ef migrations add <MIGRATION_NAME> --startup-project ..\Expenses.API\
```