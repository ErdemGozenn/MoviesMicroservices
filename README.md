# Movies Microservices

An ASP.NET Core Web API project that provides CRUD operations for movies, directors, and genres. The application uses .NET 8, Entity Framework Core, SQLite, and MediatR.

## Project Structure

- **CORE**: shared base classes and services
- **Movies.APP**: entity classes, database context, and MediatR request/handler classes
- **Movies.API**: REST API controllers, Swagger, and application startup configuration

## Data Model

- A **Director** can direct multiple **Movies**.
- **Movies** and **Genres** have a many-to-many relationship through `MovieGenre`.

## Requirements

- .NET 8 SDK
- Rider or .NET CLI

## Run the Application

Open `MoviesMicroservices.sln` in Rider and run the **Movies.API: http** configuration. Swagger is available at:

`http://localhost:5079/swagger/index.html`

To run from a terminal, execute these commands from the solution directory:

```bash
dotnet restore MoviesMicroservices.sln
dotnet run --project Movies.API/Movies.API.csproj
```

## API Endpoints

The Swagger UI provides GET, POST, PUT, and DELETE operations for Movies, Directors, and Genres. An example HTTP request is available in `Movies.API/Movies.API.http`.

## Database

The SQLite database is stored in `Movies.API/MoviesDB.db`. The included database contains sample records; changes made through the API are saved to this local file.

## Scope

This version covers the CRUD and API endpoint creation/testing stages of the project requirements. JWT authentication, role-based authorization, and a Gateway are not included in this version.
