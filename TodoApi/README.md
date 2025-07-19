# Todo API

A simple .NET Web API for managing todo items with comprehensive Swagger documentation.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete todo items
- **Swagger Documentation**: Interactive API documentation
- **Data Validation**: Input validation using Data Annotations
- **In-Memory Storage**: Simple in-memory storage for development
- **RESTful Design**: Follows REST API conventions

## Todo Item Properties

Each todo item contains the following properties:

- **Id** (int): Unique identifier (auto-generated)
- **Order** (int): Priority/order of the todo item
- **CreatedByUserId** (int): ID of the user who created the todo
- **CreatedOn** (DateTime): Timestamp when the todo was created
- **Description** (string): Description of the todo task
- **PlannedDate** (DateTime?): Optional planned date for the todo
- **DueDate** (DateTime?): Optional due date for the todo

## User Properties

Each user contains the following properties:

- **Id** (int): Unique identifier (auto-generated)
- **Name** (string): Full name of the user
- **Email** (string): Email address of the user

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/todos` | Get all todo items |
| GET | `/api/todos/{id}` | Get a specific todo item |
| GET | `/api/todos/user/{userId}` | Get todos by user |
| GET | `/api/todos/overdue` | Get overdue todos |
| GET | `/api/todos/daterange` | Get todos by date range |
| POST | `/api/todos` | Create a new todo item |
| PUT | `/api/todos/{id}` | Update an existing todo item |
| DELETE | `/api/todos/{id}` | Delete a todo item |
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get a specific user |
| GET | `/api/users/email/{email}` | Get user by email |
| GET | `/api/users/search` | Search users by name |
| POST | `/api/users` | Create a new user |
| PUT | `/api/users/{id}` | Update an existing user |
| DELETE | `/api/users/{id}` | Delete a user |
| GET | `/api/data/status` | Get data store status |
| GET | `/api/data/summary` | Get data summary |
| POST | `/api/data/reset` | Reset data store |
| POST | `/api/data/initialize` | Initialize sample data |

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later

### Running the Application

1. Navigate to the project directory:
   ```bash
   cd TodoApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   - **Swagger UI**: `https://localhost:7001` or `http://localhost:5001`
   - **API Base URL**: `https://localhost:7001/api` or `http://localhost:5001/api`

## API Examples

### Create a User

```http
POST /api/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john.doe@example.com"
}
```

### Create a Todo Item

```http
POST /api/todos
Content-Type: application/json

{
  "order": 1,
  "createdByUserId": 1,
  "description": "Complete project documentation",
  "plannedDate": "2024-01-15T00:00:00Z",
  "dueDate": "2024-01-20T00:00:00Z"
}
```

### Update a Todo Item

```http
PUT /api/todos/1
Content-Type: application/json

{
  "order": 2,
  "description": "Updated description",
  "plannedDate": "2024-01-16T00:00:00Z",
  "dueDate": "2024-01-21T00:00:00Z"
}
```

## Project Structure

```
TodoApi/
├── Controllers/
│   ├── TodosController.cs      # Todo API endpoints
│   └── UsersController.cs      # User API endpoints
├── Models/
│   ├── Todo.cs                 # Todo entity model
│   ├── TodoDto.cs              # Todo data transfer objects
│   ├── User.cs                 # User entity model
│   └── UserDto.cs              # User data transfer objects
├── Services/
│   ├── ITodoService.cs         # Todo service interface
│   ├── TodoService.cs          # Todo service implementation
│   ├── IUserService.cs         # User service interface
│   └── UserService.cs          # User service implementation
├── Program.cs                  # Application configuration
├── TodoApi.csproj             # Project file
└── README.md                  # This file
```

## Development

The API uses:
- **ASP.NET Core 8.0** for the web framework
- **Swashbuckle.AspNetCore** for Swagger documentation
- **Data Annotations** for validation
- **Dependency Injection** for service management
- **Singleton Pattern** for persistent in-memory data storage

## Data Persistence

The application uses a singleton `InMemoryDataStore` that persists data across HTTP requests. This means:
- Data created in one request will be available in subsequent requests
- The data store maintains state throughout the application lifecycle
- You can use the `/api/data/status` endpoint to check the current data state
- Use `/api/data/reset` to reset the data store to initial sample data

## Configuration

The data store can be configured through `appsettings.json`:

```json
{
  "DataStore": {
    "InitializeSampleData": true
  }
}
```

- **InitializeSampleData**: When `true`, the data store will be populated with sample data on startup. When `false`, the data store starts empty.

You can also manually initialize sample data using the `/api/data/initialize` endpoint.

### Environment-Specific Configuration

You can create environment-specific configuration files:

- `appsettings.Development.json` - Development settings (sample data enabled)
- `appsettings.Test.json` - Test settings (sample data disabled)
- `appsettings.Production.json` - Production settings

To run with a specific configuration:
```bash
dotnet run --environment Test
```

## Sample Data

The application comes with sample todo items pre-loaded for testing purposes.

## License

This project is for demonstration purposes. 