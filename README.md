# WorkforceManagerAPI

 A web API designed to handle request through a plethora of clients like web or mobile applications.


### Description:
The application provides simple CRUD actions for the two models provided, Employees and Skills. 
The user is free to create and update those models along with their relative details. 
Each Employee can be associated with zero to many Skills, as long as they have been already registered from the Skills menu.
Along with any Employee Skillset change a History Entry is generated, both for added and removed Skills to track changes.

### Built with:
* [.NET Core 3.1](https://github.com/dotnet/core) - .NET Core is a cross-platform version of .NET
* [Entity Framework Core 3.1.8](https://github.com/dotnet/efcore) - EF Core is a modern object-database mapper for .NET
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2019) - Relational Database used for storage
