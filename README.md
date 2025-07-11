# User Management Technical Exercise

The exercise is an ASP.NET Core web application backed by Entity Framework Core, which faciliates management of some fictional users.
We recommend that you use [Visual Studio (Community Edition)](https://visualstudio.microsoft.com/downloads) or [Visual Studio Code](https://code.visualstudio.com/Download) to run and modify the application. 

**The application uses an in-memory database, so changes will not be persisted between executions.**

## Please scroll to the bottom to see my notes!

## The Exercise
Complete as many of the tasks below as you feel comfortable with. These are split into 4 levels of difficulty 
* **Standard** - Functionality that is common when working as a web developer
* **Advanced** - Slightly more technical tasks and problem solving
* **Expert** - Tasks with a higher level of problem solving and architecture needed
* **Platform** - Tasks with a focus on infrastructure and scaleability, rather than application development.

### 1. Filters Section (Standard)

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true`
* Non Active – This should show only users where their `IsActive` property is set to `false`

### 2. User Model Properties (Standard)

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app.

### 3. Actions Section (Standard)

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list
* **View** - A screen that displays the information about a user
* **Edit** – A screen that allows you to edit a selected user from the list  
* **Delete** – A screen that allows you to delete a selected user from the list

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced)

Extend the system to capture log information regarding primary actions performed on each user in the app.
* In the **View** screen there should be a list of all actions that have been performed against that user. 
* There should be a new **Logs** page, containing a list of log entries across the application.
* In the Logs page, the user should be able to click into each entry to see more detail about it.
* In the Logs page, think about how you can provide a good user experience - even when there are many log entries.

### 5. Extend the Application (Expert)

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations.
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations.

### 6. Future-Proof the Application (Platform)

Add additional layers to the application that will ensure that it is scaleable with many users or developers. For example:
* Add CI pipelines to run tests and build the application.
* Add CD pipelines to deploy the application to cloud infrastructure.
* Add IaC to support easy deployment to new environments.
* Introduce a message bus and/or worker to handle long-running operations.

## Additional Notes

* Please feel free to change or refactor any code that has been supplied within the solution and think about clean maintainable code and architecture when extending the project.
* If any additional packages, tools or setup are required to run your completed version, please document these thoroughly.

## Rohaan's notes below
### Completed Tasks

**Tasks 1-3 (Standard):** All standard requirements completed with validation.

**Task 5 (Expert):** Rewrote the existing MVC application to use Blazor WebAssembly (front-end) and ASP.NET Core Web API (back-end) using best practices. Converted existing service (domain) & data layers to be asynchronous.

**Task 6 (Platform):** Implemented a pipeline for CI (building and testing) using GitHub Actions.

### Architecture Overview

The projects in the solution remain relatively the same but with a few additions:

- **UserManagement.Web.API** - REST API with Swagger documentation
- **UserManagement.Web.Client** - Blazor WebAssembly application (Includes Blazored.Toast nuget package)
- **UserManagement.Web.Shared** - Shared classes (i.e. DTOs) between client/server
- **UserManagement.Services** - Same (Server-side Business logic layer)
- **UserManagement.Data** - Same (Server-side Data access layer)

### Technical Implementation Details

**Validation:** Implemented both client-side and server-side validation using DataAnnotations on form models.

**UX:** Added toast notifications for CRUD operations and loading states throughout the client. Form validation displays clear error messages.

**Testing:** Focused on unit testing the API and Service layer. All my tests follow the existing AAA pattern and use the same conventions as the existing ones.

### Design Decisions

**.Shared class library:** The .Shared project contains the models used by both client and server for type safety and reducing duplication.

**Email Uniqueness:** Intentionally allowed duplicate emails since this is a user management system without an auth requirement.

**Task Prioritisation:** My main focus was delivering a high quality solution focusing on core functionality, architecture, and comprehensive testing to demonstrate my existing skills. 

### Running the Application

1. Run as multi-startup project (UserManagement.Web.API and UserManagement.Web.Client)
