# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

GlasAnketa ("Voice Survey") is a .NET 8 ASP.NET Core MVC application for employee satisfaction surveys. The application manages employee survey forms with multiple question types (scale and text) across various organizational departments.

## Architecture

This project follows a clean layered architecture pattern with clear separation of concerns:

### Layer Dependencies
```
GlasAnketa (Web/MVC) 
    ↓ references
GlasAnketa.Services (Business Logic)
    ↓ references  
GlasAnketa.DataAccess (Data Layer)
    ↓ references
GlasAnketa.Domain (Domain Models)
    ↑ referenced by
GlasAnketa.ViewModels (DTOs/ViewModels)
```

### Core Components

**Domain Layer (`GlasAnketa.Domain`)**
- Contains entity models: `User`, `QuestionForm`, `Question`, `Answer`, `QuestionType`, `Role`
- Defines the core business entities and their relationships

**Data Access Layer (`GlasAnketa.DataAccess`)**
- Entity Framework Core with SQL Server
- Repository pattern implementation with interfaces and implementations
- Database migrations and seeding with extensive predefined user data
- Connection string: Uses SQL Server with Trusted Connection

**Services Layer (`GlasAnketa.Services`)**
- Business logic implementation
- AutoMapper profiles for entity-to-viewmodel mapping
- Dependency injection configuration via `InjectionExtensions`

**ViewModels Layer (`GlasAnketa.ViewModels`)**
- DTOs for data transfer between layers
- View-specific models for MVC controllers

**Web Layer (`GlasAnketa`)**
- ASP.NET Core MVC with controllers: Account, Admin, Answer, Home, Questionnaire
- Session management enabled
- Default route: `{controller=Account}/{action=Login}/{id?}`

## Development Commands

### Build and Run
```powershell
# Build the entire solution
dotnet build UPdateDobraAnketa.sln

# Build specific project
dotnet build GlasAnketa/GlasAnketa.csproj

# Run the web application (from solution root)
dotnet run --project GlasAnketa/GlasAnketa.csproj

# Run in development mode
dotnet run --project GlasAnketa/GlasAnketa.csproj --environment Development
```

### Database Operations
```powershell
# Add new migration (from solution root)
dotnet ef migrations add <MigrationName> --project GlasAnketa.DataAccess --startup-project GlasAnketa

# Update database
dotnet ef database update --project GlasAnketa.DataAccess --startup-project GlasAnketa

# Drop database (if needed)
dotnet ef database drop --project GlasAnketa.DataAccess --startup-project GlasAnketa
```

### Testing and Development
```powershell
# Clean solution
dotnet clean UPdateDobraAnketa.sln

# Restore packages
dotnet restore UPdateDobraAnketa.sln

# Watch for changes (hot reload)
dotnet watch run --project GlasAnketa/GlasAnketa.csproj
```

## Key Patterns and Conventions

### Dependency Injection Setup
The application uses extension methods in `GlasAnketa.Services.Extensions.InjectionExtensions`:
- `InjectRepositories()` - Registers repository implementations
- `InjectServices()` - Registers service implementations  
- AutoMapper configuration in `Program.cs`

### Database Seeding
- Extensive predefined data in `DataSeedExtensions.cs`
- 412 users with company IDs, organizational units, and departments
- 12 predefined question forms covering different survey aspects
- 36 predefined questions across all forms

### Authentication & Authorization
- Custom authentication system (not Identity)
- Role-based access (Administrator, Employee)
- Session-based authentication
- Default login redirect from Home/Index

### Question Types
- **Scale**: 1-10 numerical ratings
- **Text**: Open-ended text responses

## Database Schema Notes

### Key Relationships
- Users have Roles (Administrator/Employee)
- Questions belong to QuestionForms and QuestionTypes  
- Questions are created by Users (seeded by user ID 1)
- Answers link Users to Questions with response values

### Organizational Structure
Users are organized by:
- `OU` (Organizational Unit): Production, Projects and IT, Supply chain, etc.
- `OU2` (Sub-unit): Rolling Unit, Coating Unit, Quality control, etc.
- `CompanyId`: Unique employee identifier used as password

## Development Guidelines

When working with this codebase:

1. **Adding new survey forms**: Update both `QuestionForm` and `Question` seed data in `DataSeedExtensions.cs`

2. **Adding new question types**: Extend `QuestionType` enum and update related mapping logic

3. **Modifying authentication**: Update `AccountController` and ensure session management remains consistent

4. **Database changes**: Always use migrations and update seed data if needed

5. **New controllers**: Follow the existing pattern of dependency injection for services and repositories

6. **AutoMapper profiles**: Add new profiles in `GlasAnketa.Services.AutoMappers` for entity-viewmodel mapping

The application is designed for internal company use with predefined users and focuses on collecting employee feedback across multiple organizational areas.