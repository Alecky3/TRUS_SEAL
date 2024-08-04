
# Trust Seal Application

<!-- Brief description of what the Trust Seal Application does -->
This application manages and verifies trust seals for websites and organizations.

## Prerequisites

<!-- List all the software and tools needed to run the project -->
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (2019 or later)
- [Git](https://git-scm.com/downloads)

## Getting Started

<!-- Step-by-step instructions to set up the project locally -->

1. Clone the repository
`git clone [https://github.com/TSAOrg/TrustSealApplication.git]`(https://github.com/TSAOrg/TrustSealApplication.git)
2. Navigate to the project directory
 `cd  trust-seal-application`
3. Restore dependencies
 ` dotnet restore`
 4. Update the database connection string
<!-- Explain where to find and how to update the connection string -->
- Open `appsettings.json` in the root directory
- Locate the `ConnectionStrings` section
- Update the `DefaultConnection` string with your local SQL Server details:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TrustSealDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  ```

5. Apply database migrations
  `dotnet ef database update`
 6. Run the Application
  ` dotnet run`
  
