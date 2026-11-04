# Contoso University - .NET 10

This project is an ASP.NET Core MVC application targeting .NET 10 (`net10.0`).

## Project Overview

### Framework
- ASP.NET Core MVC (.NET 10)

### Database Access: Entity Framework
- Entity Framework Core 10

### Project Structure
```
ContosoUniversity/
├── Controllers/            # MVC Controllers
├── Data/                   # Entity Framework context and initializer
├── Models/                 # Data models and view models
├── Services/               # Application services (in-memory notifications)
├── Views/                  # Razor views
├── wwwroot/                # Static files (Content, Scripts)
├── Program.cs              # Startup and dependency injection
└── appsettings.json        # Configuration
```

## Database Configuration

The application uses SQL Server LocalDB with the following connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=ContosoUniversityNoAuthEFCore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

## Running the Application

1. **Prerequisites**:
   - .NET 10 SDK
   - SQL Server LocalDB

2. **Setup**:
   - `dotnet build`
   - `dotnet run` (Kestrel)

## Features

- **Student Management**: CRUD operations for students with pagination and search
- **Course Management**: Manage courses and their assignments to departments
- **Instructor Management**: Handle instructor assignments and office locations
- **Department Management**: Manage departments and their administrators
- **Statistics**: View enrollment statistics by date

## Database Initialization

The application uses Entity Framework Core Code First with a database initializer that:
- Creates the database if it doesn't exist
- Seeds sample data including students, instructors, courses, and departments
- Handles model changes by recreating the database
