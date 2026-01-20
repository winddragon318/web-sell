# SellPhone — ASP.NET Core Blazor Project

## Project Overview

SellPhone is a Blazor web application targeting .NET 10. It includes server-side APIs, Blazor components for UI, SCSS-based styles, and client-side JavaScript under `wwwroot`. This README explains how to set up, run, and contribute to the project.

---

## Prerequisites

- .NET 10 SDK (install the latest patch for .NET 10)
- Git
- Visual Studio 2026 (recommended) or VS Code
- Optional: Dart Sass (`sass`) for compiling SCSS locally
- A database server supported by EF Core (e.g., SQL Server, PostgreSQL)

---

## Quick Start

1. Clone the repository

2. Restore .NET packages:

````````
dotnet restore
````````

3. Configure the database connection string

- Edit `appsettings.Development.json` or `appsettings.json`, or set an environment variable such as `ConnectionStrings__DefaultConnection` to point to your database.

4. Apply EF Core migrations:

From a terminal:

````````
dotnet ef database update
````````

Or from Visual Studio 2026 Package Manager Console:

````````
PM> Update-Database
````````

5. Run the application:

From a terminal:

````````
dotnet run
````````

Or in Visual Studio 2026 use the debug profile:

Open the app at the URL shown in the console (commonly `https://localhost:5001`).

---

## Project Structure

- **`/Data`**: Contains EF Core data models and DbContext.
- **`/Migrations`**: Contains EF Core migration files.
- **`/Pages`**: Contains Blazor components for each page.
- **`/Shared`**: Contains shared components and layout.
- **`/wwwroot`**: Contains static files like CSS, JS, and images.

---

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b my-feature`
3. Commit your changes: `git commit -m 'Add my feature'`
4. Push to the branch: `git push origin my-feature`
5. Submit a pull request

Please ensure your code adheres to the existing style and conventions used in the project. Run all tests before submitting a pull request.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Acknowledgements

- Thanks to the developers of .NET, Blazor, EF Core, and other open-source projects used in this application.
- Inspired by the need for a powerful, flexible platform for building web applications with C# and .NET.

---

## Contact

For questions or support, please create an issue on the GitHub repository or contact the project maintainer.

---

## Front-end assets (SCSS / JS)

- Main SCSS: `wwwroot/scss/site.scss` — canonical stylesheet.
- Compiling SCSS manually (optional):

````````

- Client JS files are under `wwwroot/lib/`. Static files are served from `wwwroot` — ensure component references match file paths.

---

## Project structure (important locations)

- `Program.cs` — app startup and DI registrations
- `Components/Pages/` — Blazor pages (e.g., `Home.razor`)
- `Components/Shared/` — shared UI components (e.g., `AccountDropdown.razor`)
- `wwwroot/scss/site.scss` — SCSS source
- `wwwroot/css/site.css` — compiled CSS (may be generated)
- `wwwroot/lib/` — client-side JS libraries

---

## Common tasks

- Add and apply EF migrations when models change:

````````

- Build and run:

````````

- Clean artifacts:

````````

- Compile SCSS (if needed): use `sass` as shown above or configure a build step in your IDE.

---

## Debugging & Visual Studio 2026 tips

- To set environment variables for a debug profile: open project Properties -> Debug and add them to the profile.
- If migrations fail: verify the connection string and DB access rights; confirm the EF Core provider package (e.g., `Microsoft.EntityFrameworkCore.SqlServer`).
- Static assets not updating: ensure `wwwroot/css/site.css` exists, verify component references, and clear the browser cache.
- For breakpoint debugging in Blazor components, set breakpoints inside `.razor` files. Use browser developer tools for client-side JS issues.

---

## Contributing

- Create feature branches from `main`.
- Open a pull request with a clear description and testing steps.
- Include EF migration files when the data model changes.

---

## Troubleshooting

- Port in use: change the launch profile or run with a different URL:

````````

- Migration ID conflicts: rebase/merge `main` before adding migrations.
- Missing NuGet packages: run `dotnet restore` or use Visual Studio's NuGet restore.

---

## Contact

Open an issue in the repository with logs and reproduction steps for help.
