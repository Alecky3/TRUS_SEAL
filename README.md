
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
# This for when you need to configure from scratch, otherwise skip to the end
# Configure Tailwind & flowbite (a tailwind component library)

# Preliguisite
1. Make sure Nodejs is installed - installation instructions on https://nodejs.org/
2. Check NodeJs installation with this command; 'node -v' - should output something like <v20.10.0>
3. If Nodejs is installed correctly, create a package.json file under TRUSTSEALAPPLICATION folder.
   'npm init -y'
4. Install tailwind CLI
   'npm install -D tailwindcss@latest postcss@latest autoprefixer@latest'
5. On directory TRUSTSEALAPPLICATION, create a file with name [postcss.config.js] with this content
    '// postcss.config.js
    module.exports = {
      plugins: {
        tailwindcss: {},
        autoprefixer: {},
      }
    }'
6. Create Tailwind config file - this creates a tailwind.config.js file on the directory TrustSeal
  'npx tailwindcss init'
7. Create a file under TrustSeal/wwwroot/css/tailwind/ with filename [tailwind_main.css] - This will be the main css file tailwind will use to generate the output css under the name [tailwind_main_out.css]
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
12. Update package.json file, under Scripts, after "test" add a comma and add 
    'tailwind-dev":"npx tailwindcss -i ./TrustSeal/wwwroot/css/tailwind/tailwind_main.css -o ./wwwroot/css/tailwind/tailwind_main_out.css --watch'
    This allow you initiate the command a simple as 
    'npm run tailwind-dev'
13. Test whether tailwind command is working, run 'npm run tailwind-dev'

# Configure flowbite (Tailwind component library)
1. Install flowbite
    'npm install flowbite'
2. Add flowbite as a plugin on tailwind.config.js under plugins
    'plugins: [
        require('flowbite/plugin')
    ]'
3. Update tailwind.config.js under content
'content: [
        "..other include from before",
        "./node_modules/flowbite/**/*.js"
    ]'
4. Copy flowbite.min.js file under node_modules/flowbite/dist/flowbite.min.css to TrustSeal/wwwwroot/js/
5. Add flowbite js include under _Layout.cshtml

# configure tailwind and flowbite the easy way
1. Ensure NodeJs is installed
2. under TRUSTSEALAPPLICATION run
  'npm run install'
3. To start tailwind CLI run
  'npm run tailwind-dev'
4. Good to go.



  
