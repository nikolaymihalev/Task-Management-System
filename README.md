# TaskMaster
- TaskMaster is an ASP.NET Core application for task management, designed with a layered architecture that separates business logic, database, and web layers. The project is modular, scalable, and includes unit and integration tests.

## Technologies
- ASP.NET Core
- Entity Framework Core
- xUnit

## Programming Languages
- .Net Version 8.0
- HTML5/CSS 
- Javascript

## CI/CD Pipeline
This project includes a CI/CD pipeline using GitHub Actions for automated build and testing. The pipeline is configured to automatically:
- Checkout code
- Setup .NET
- Restore dependencies
- Build
- Test
These steps help maintain continuous integration, ensuring the code is always in a deployable state. With each push or pull request, the GitHub Action workflow is triggered, allowing for consistent and reliable testing and deployment processes.

## Project Structure
The solution is organized into five primary projects:
- [TaskMaster](#taskmaster-web-application)
- [TaskMaster.Core](#taskmastercore-business-logic)
- [TaskMaster.Infrastructure](#taskmasterinfrastructure-data-access-layer)
- [TaskMaster.UnitTests](#taskmasterunittests-unit-tests)
- [TaskMaster.IntegrationTests](#taskmasterintegrationtests-integration-tests)

## TaskMaster (Web Application)
The TaskMaster project includes two controllers and several extension files:

### Controllers
1. HomeController with methods:
- Index - [HttpGet] Displays home page / Redirects to the UserController's Dashboard method when the user is logged in.
- Privacy - [HttpGet] Displays the privacy policy.
- Contact - [HttpGet] Displays contact information.
2. UserController (Methods marked with [Authorize] unless specified otherwise)
- Dashboard - [HttpGet] Fetches and displays user-related task statistics.
- MyTasks - [HttpGet] Returns a view with the user's tasks.
- NewTask - Contains HttpGet and HttpPost methods:
[HttpGet] - Returns a view where users can fill in task details.
[HttpPost] - Creates a new task using a service, then redirects to MyTasks.
3. Notifications - [HttpGet] Displays all notifications for the user.
4. Task - [HttpGet] Shows detailed information for a specific task.
5. Remove - [HttpGet] method that deletes a task and redirects to MyTasks.
6. RemoveNotification - [HttpGet] method that deletes a notification and redirects to Notifications.
7. Edit - Contains HttpGet and HttpPost methods for editing tasks:
[HttpGet] - Returns a view with a form populated with task data.
[HttpPost] - Updates the task data and redirects to MyTasks.
8. Update - Contains HttpGet and HttpPost methods for updating tasks priority or status:
[HttpGet] - Returns a view with a form populated with task data.
[HttpPost] - Updates the task priority and status and redirects to MyTasks.
9. Logout - Logs the user out.

[AllowAnonymous] methods:

10. Register - Allows user registration.
11. Login - Allows user login.

### Extensions
1. ServiceCollectionExtensions
- AddApplicationServices - Registers all services from the Core project.
- AddApplicationDbContext - Registers the connection string and DbContext using a repository pattern.
- AddApplicationIdentity - Registers IdentityUser with IdentityRole.
2. ClaimsPrincipalExtensions - Extends user claims (System.Security.Claims) with a method to retrieve the user's ID.

## TaskMaster.Core (Business Logic)
The Core project contains the following folders:

### Constants - Defines two files for:
1. Variables - Constants for models
2. Messages - Output messages
### Contracts - Contains interfaces for services:
1. INotificationService
2. ITaskService
3. IStatisticsService
### Enums - Defines enums 
1. TaskPriority 
2. TaskStatus
### Models - Contains DTO models for:
1. Notification - Models for Form, Info, and Page
2. Task - Models for Form, Info, and Page
3. User - Models for Register, Login, and Statistics
### Services - Contains service implementations:
1. NotificationService
2. TaskService
3. StatisticsService

## TaskMaster.Infrastructure (Data Access Layer)
The Infrastructure project has three main folders:

### Common - Contains:
1. IRepository interface
2. Repository class - Handles the connection with DbContext
### Data - Contains:
1. Configurations
2. Migrations
4. ApplicationDbContext
5. SeedData
### Models - Contains:
1. Task
2. Notification

## TaskMaster.UnitTests (Unit Tests)
The UnitTests project includes tests for each service in the Core project, including:
### NotificationService, TaskService, StatisticsService
Each test suite covers all methods in the corresponding service, ensuring reliable business logic.

## TaskMaster.IntegrationTests (Integration Tests)
The IntegrationTests project includes integration tests for all methods in the UserController. These tests ensure that the web application’s controllers work as expected with real data.