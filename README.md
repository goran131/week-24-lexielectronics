# LexiElectronics - Project Overview

This README provides a high-level overview of the LexiElectronics ASP.NET Core MVC application, its architecture, and how to run and contribute.

## Contents
- docs/architecture.puml — PlantUML class diagram for core controllers and models.

## Project structure
- Controllers/ — MVC controllers (AdminController, etc.)
- Models/ — Domain models (Product, ProductCategory, ApplicationUser, Order, OrderItem, ViewModels)
- Views/ — Razor views for pages and partials
- Areas/Identity — Identity Razor Pages (Account management)
- wwwroot/ — static assets (images, js, css)
- Data/ — ApplicationDbContext and EF Core migrations

## Key concepts
- Products have categories and manufacturers. Product.VisibleInShop controls visibility.
- Users are ASP.NET Identity users (ApplicationUser) with a single role.
- AdminController handles product, category, order, and user management.

## Running the project
1. Ensure .NET SDK is installed (version used by project).
2. Update the database connection string in appsettings.json.
3. Apply migrations:
   - `dotnet ef database update`
4. Run the app:
   - `dotnet run`
5. Visit the site and log in with an admin account to access Admin pages.

## Diagram
A PlantUML file at `docs/architecture.puml` describes the main classes and relationships. Render with PlantUML or VS Code PlantUML extension.

## Contributing
- Follow the existing coding style.
- Ensure new features are covered by appropriate migrations and tested.

## Common tips
- When modifying Identity data, prefer UserManager/RoleManager over direct table edits.
- For file uploads, use `IWebHostEnvironment.WebRootPath` and `enctype="multipart/form-data"` in forms.

