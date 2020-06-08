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
```powershell
docker run `
	 -e 'ACCEPT_EULA=Y' ` 
	 -e 'MSSQL_SA_PASSWORD=Network@123' ` 
	 -p 1433:1433 `
	 -v mssql_volume:/var/opt/mssql `
	 -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04 `
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

## Run Jaeger docker image to collect telemetry
```powershell
docker run -d --name jaeger `
	-e COLLECTOR_ZIPKIN_HTTP_PORT=9411 `
	-p 5775:5775/udp `
	-p 6831:6831/udp `
	-p 6832:6832/udp `
	-p 5778:5778 `
	-p 16686:16686 `
	-p 14268:14268 `
	-p 9411:9411 `
	jaegertracing/all-in-one
``

## Run RabbitMQ docker image
```poweshell
docker run --name rabbitmq `
	--hostname rabbitmq `
	-d `
	-p 15672:15672 `
	-p 5672:5672 `
	rabbitmq:3-management 
```