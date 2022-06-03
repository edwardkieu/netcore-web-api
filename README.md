# ASP.NET Core WebApi - Clean Architecture

An Implementation of Clean Architecture with ASP.NET Core 6 WebApi. With this Open-Source, you will get access to the world of Loosely-Coupled and Inverted-Dependency Architecture in ASP.NET Core 6 WebApi with a lot of best practices.

### Alternatively you can also clone the Repository.

1. Clone this Repository and Extract it to a Folder.
2. Change the Connection Strings for the Application and Identity in the WebApi/appsettings.json - (WebApi Project)
3. Run the following commands on Powershell in the WebApi Projecct's Directory.

- dotnet restore
- dotnet ef database update -Context ApplicationDbContext
- dotnet ef database update -Context IdentityContext
- dotnet run (OR) Run the Solution using Visual Studio 2022

## Technologies

- ASP.NET Core 6 WebApi
- REST Standards
- .NET Core 6

## Features

- [x] Clean Architecture
- [x] CQRS with MediatR Library
- [x] Entity Framework Core - Code First
- [x] Repository Pattern - Generic
- [x] Unit Of Work Pattern - Generic
- [x] MediatR Pipeline Logging & Validation
- [x] Serilog
- [x] xUnit test
- [x] Swagger UI
- [x] Response Wrappers
- [x] Healthchecks
- [x] Pagination
- [ ] In-Memory Caching
- [ ] Redis Caching
- [x] In-Memory Database
- [x] Microsoft Identity with JWT Authentication, RefreshToken
- [x] Role based Authorization
- [x] Identity Seeding
- [x] Database Seeding
- [x] Custom Exception Handling Middlewares
- [x] API Versioning
- [x] Fluent Validation
- [x] Automapper
- [x] SMTP / Mailkit / Sendgrid Email Service
- [x] Complete User Management Module (Register / Generate Token / Forgot Password / Confirmation Mail)
- [x] User Auditing
