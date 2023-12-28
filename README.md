# Running a .NET Core Application in Development Mode

## Prerequisites

1. Install [Visual Studio](https://visualstudio.microsoft.com/) on your machine.

2. Ensure you have [.NET Core SDK](https://dotnet.microsoft.com/download) installed.

3. Install [MSSQL LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15) on your machine.

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

## Additional Notes

- If you encounter any issues connecting to LocalDB, ensure that LocalDB is installed and running. You can check this using SQL Server Object Explorer in Visual Studio.
