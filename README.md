# Running the .NET Core Application in Development Mode

## Prerequisites

1. Install [Visual Studio](https://visualstudio.microsoft.com/) on your machine.

2. Ensure you have [.NET Core SDK](https://dotnet.microsoft.com/download) installed. (version 8.0 or later).

3. Install [MSSQL LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15) on your machine.

## Application Architecture Overview

### Entity Framework and Database Access

The application follows a layered architecture for organizing code:

- **DAL (Data Access Layer):** This layer is responsible for handling database interactions using Entity Framework. It includes DbContext for defining the database schema and data access operations.

- **BLL (Business Logic Layer):** The BLL contains business logic and services that process data before it reaches the database or after it's retrieved. This layer encapsulates the application's core functionality.

- **Controller Layer:** Controllers handle incoming HTTP requests, interact with services from the BLL, and return HTTP responses. They serve as the entry point for the application.

### Controller Pattern

- **Controllers:** Controllers in the application expose endpoints that handle incoming HTTP requests. They receive input from the client, invoke the necessary services from the BLL, and return the appropriate HTTP response.

### BLL (Business Logic Layer)

- **Services:** Services in the BLL contain business logic and handle complex operations. They interact with the DAL to perform database operations.

### DAL (Data Access Layer)

- **DbContext:** The DbContext is responsible for defining the database schema and provides a set of APIs for performing database operations using Entity Framework.

### Fluent Validation for DTOs

- **DTOs (Data Transfer Objects):** DTOs represent the data that is transferred between the client and the server. Fluent Validation is used to validate the incoming data from the endpoints. Validation rules are defined in a clear, fluent syntax.


## Setup MSSQL LocalDB

1. Open Visual Studio and the .NET Core project.

2. Open `appsettings.json` and ensure your connection string is set up like:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SuperComTask;Trusted_Connection=True;MultipleActiveResultSets=true"
      },
      // ...
    }
    ```
3. Create a database named SuperComTask in your localdb

## Run the Application with IIS Express

1. Set the "SuperComTask" as your startup project by right-clicking on your project in Solution Explorer and selecting `Set as StartUp Project`.

2. Press `F5` or click `Run` to start the application with IIS Express.

3. Your application should open in your default web browser.

## Run Migrations using Package Manager Console

1. Open `View > Other Windows > Package Manager Console`.

2. In the Package Manager Console, ensure your default project is set to your data access project:

    ```bash
    PM> Update-Database
    ```

    This command will apply any pending migrations and update your database.

## Testing

- **In-Memory Database:** Tests are performed using an in-memory database to ensure the application logic works correctly in an isolated environment.

## Additional Notes

- If you encounter any issues connecting to LocalDB, ensure that LocalDB is installed and running. You can check this using SQL Server Object Explorer in Visual Studio.
