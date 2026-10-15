# RaceDay Backend API
 
## Project Overview
The RaceDay Backend is a RESTful API built with ASP.NET Core Web API and Entity Framework Core (Code-First). It serves as the data and authentication backbone for the RaceDay event management system, handling user accounts, race events, categories, enrolments, and results tracking.
 
## User Roles
The system enforces strict role-based access control using server-side session management:
*   **Organiser:** Has administrative access to create and manage race events, define distance/age categories, and capture final race results (times and medals) for participants.
*   **Participant:** Can view available upcoming events, enrol in specific race categories, and track their personal performance history and achievements.
 
## Prerequisites
*   Visual Studio 2022
*   .NET 8.0 SDK
*   Microsoft SQL Server (LocalDB or SSMS)
 
## Setup and Run Instructions
 
1. **Clone the repository:**
   ```bash
   git clone <your-repository-url>
   ```
 
2. **Open the solution:**
   Open the `RaceDaySystem.sln` file in Visual Studio 2022.
 
3. **Configure the Database Connection:**
   Open `appsettings.json` in the `RaceDayBackend` project and update the `DefaultConnection` string to point to your local SQL Server instance.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=RaceDayDB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
   }
   ```
 
4. **Apply Migrations (Code-First):**
   Open the Package Manager Console in Visual Studio (**Tools** > **NuGet Package Manager** > **Package Manager Console**). Ensure the Default Project is set to `RaceDayBackend` and run:
   ```powershell
   Update-Database
   ```
 
5. **Run the API:**
   Press `F5` or click the green **Play** button in Visual Studio. The API will launch and automatically open the Swagger UI in your browser, where you can test all endpoints.
 
## Running Unit Tests
The solution includes a comprehensive xUnit testing project (`RaceDayBackend.Tests`) utilizing Moq and an In-Memory database to verify role enforcement and authentication.
 
1. In Visual Studio, go to **Test** > **Test Explorer**.
2. Click the **Run All** button to execute the tests.
 
## CI/CD Pipeline
This project utilizes GitHub Actions for continuous integration. On every push to the repository, the pipeline automatically restores dependencies, builds the .NET solution, and executes all unit tests to ensure stability.
 
## Video Presentation
[Click here to view the API Demonstration and Code Walkthrough Video](#)
