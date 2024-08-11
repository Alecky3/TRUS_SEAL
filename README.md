
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

# Configure Tailwind & flowbite (a tailwind component library)

# Preliguisite
1. Make sure Nodejs is installed - installation instructions on https://nodejs.org/
2. Check NodeJs installation with this command; 'node -v' - should output something like <v20.10.0>
3. If Nodejs is installed correctly, create a package.json file.
   'npm init -y'
4. Install tailwind CLI
   'npm install -D tailwindcss@latest postcss@latest autoprefixer@latest'
5. On directory TrustSeal, create a file with name [postcss.config.js] with this content
    '// postcss.config.js
    module.exports = {
      plugins: {
        tailwindcss: {},
        autoprefixer: {},
      }
    }'
6. Create Tailwind config file - this creates a tailwind.config.js file on the directory TrustSeal
  'npx tailwindcss init'
7. Create a file under wwwroot/css/tailwind/ with filename [tailwind_main.css] - This will be the main css file tailwind will use to generate the output css under the name [tailwind_main_out.css]
8. Add this content on [tailwind_main.css]
    '@tailwind base;
     @tailwind components;
     @tailwind utilities;
    '
9. Tell tailwind CLI where your template files are
   - modify tailwind.config.js to look like this
   '@type {import('tailwindcss').Config} */
    module.exports = {
      content: ["./Pages/**/*.cshtml"],
      theme: {
        extend: {},
      },
      plugins: [],
    }'
10. Comment the css includes and js includes on _Layout.cshtml
11. include a new styleshett with href=["~/css/tailwind/tailwind_main_out.css"]


  
