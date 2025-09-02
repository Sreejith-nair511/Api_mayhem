# UnauthorizedSWAPI - Clean Architecture Web API

A .NET 8 Web API project implementing clean architecture principles for user management.

## Project Structure

```
UnauthorizedSWAPI/
├── Controllers/           # API Controllers (request handling)
│   └── UsersController.cs
├── Services/             # Business logic layer
│   ├── Interfaces/
│   │   └── IUserService.cs
│   └── UserService.cs
├── Repositories/         # Data access layer
│   ├── Interfaces/
│   │   └── IUserRepository.cs
│   ├── MockUserRepository.cs    # In-memory implementation
│   └── DbUserRepository.cs      # Database implementation
├── Models/               # Data models and DTOs
│   ├── DTOs/
│   │   ├── UserDto.cs
│   │   └── CreateUserDto.cs
│   └── User.cs
└── Data/                 # Entity Framework context
    └── ApplicationDbContext.cs
```

## Features

- **Clean Architecture**: Separation of concerns with Controllers, Services, and Repositories
- **Dependency Injection**: Easy switching between Mock and Database implementations
- **Swagger UI**: Interactive API documentation at `/swagger`
- **Comprehensive Logging**: Structured logging throughout the application
- **Input Validation**: Data annotations and model validation
- **Error Handling**: Proper HTTP status codes and error responses

## API Endpoints

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update existing user
- `DELETE /api/users/{id}` - Delete user
- `GET /api/users/email-exists?email={email}` - Check if email exists

## Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server (for database implementation)

### Running the Application

1. **Clone and navigate to the project:**
   ```bash
   cd UnauthorizedSWAPI
   ```

2. **Restore packages:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI:**
   Open your browser and navigate to `https://localhost:7xxx/swagger` (port may vary)

## Configuration

### Using Mock Repository (Default)
The application is configured to use the `MockUserRepository` by default, which provides in-memory data for testing.

### Switching to Database Repository

1. **Update Program.cs:**
   ```csharp
   // Comment out this line:
   // builder.Services.AddScoped<IUserRepository, MockUserRepository>();
   
   // Uncomment these lines:
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   builder.Services.AddScoped<IUserRepository, DbUserRepository>();
   ```

2. **Update connection string in appsettings.json:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your-Database-Connection-String-Here"
     }
   }
   ```

3. **Create and run migrations:**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

## Sample Data

The Mock repository includes sample users:
- John Doe (john.doe@example.com)
- Jane Smith (jane.smith@example.com)
- Bob Johnson (bob.johnson@example.com) - Inactive

## Testing the API

### Using Swagger UI
1. Run the application
2. Navigate to `/swagger`
3. Try the different endpoints with the interactive interface

### Using curl or Postman

**Get all users:**
```bash
curl -X GET "https://localhost:7xxx/api/users"
```

**Create a new user:**
```bash
curl -X POST "https://localhost:7xxx/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Test",
    "lastName": "User",
    "email": "test@example.com"
  }'
```

## Architecture Benefits

1. **Testability**: Easy to unit test with mocked dependencies
2. **Maintainability**: Clear separation of concerns
3. **Flexibility**: Easy to switch between different data sources
4. **Scalability**: Clean structure supports future enhancements

## Next Steps

- Add authentication and authorization
- Implement additional entities (Orders, Products, etc.)
- Add caching layer
- Implement pagination for large datasets
- Add comprehensive unit and integration tests

And final notes for viresh<br>
switching to Database<br>
When ready to use a real database, simply:<br>
<br><br><br>
Uncomment the EF Core configuration in <br>
Program.cs<br>
Switch repository registration from <br>
MockUserRepository
 to 
DbUserRepository<br>
Update connection string in 
appsettings.json
Run migrations: dotnet ef migrations add InitialCreate && dotnet ef database update
