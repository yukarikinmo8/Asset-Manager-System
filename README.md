# Asset Manager System

A full-stack asset management platform built with a Vue.js frontend and C#/.NET backend. Designed to help organizations track, allocate, and manage assets efficiently.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Frontend Setup](#asset-manager-frontend-documentation)
- [Backend Setup](#asset-manager-backend-documentation)
- [Database Setup](#how-to-set-up-the-database)
- [Folder Structure](#folder-structure)
- [Packages Used (Backend)](#packages-used)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- Employee & Manager authentication and role management
- Asset inventory and lifecycle tracking
- Asset rental/return workflow
- RESTful API backend
- Modern, responsive UI

---

## Tech Stack

- **Frontend:** Vue.js, TypeScript, TailwindCSS, Vite
- **Backend:** C# (.NET), MongoDB
- **Authentication:** JWT, AspNetCore.Identity.MongoDbCore

---

## Asset Manager: Front-End Documentation

### How to Set Up the Frontend

1. [Download and install Node.js](https://nodejs.org/en/download)
2. Clone the frontend repository
3. Open a terminal in the project folder
4. Run the following command to install dependencies:
   ```bash
   npm install
   ```
5. After installing the packages, run:
   ```bash
   npm run dev
   ```
   to start the project.

### Folder Structure for Frontend

```
Asset-Manager-Frontend/
├── public/                 # Static files like images or icons accessible directly by the browser
├── src/
│   ├── assets/             # Images, fonts, and design-related files
│   ├── components/
│   │   ├── common/         # Shared components like navigation bars, footers, etc.
│   │   ├── employee/       # Components specific to the employee section (e.g., sign-in modal)
│   │   └── manager/        # Components specific to the manager section (e.g., sign-up form)
│   ├── router/             # Defines routes and maps URLs to components/pages
│   ├── views/
│   │   ├── employee/       # Pages/screens for employees
│   │   └── manager/        # Pages/screens for managers
│   ├── utils/              # Helper functions and reusable logic
│   ├── App.vue             # Main component that wraps the entire app
│   ├── main.ts             # Entry point of the app; mounts `App.vue`
│   ├── shims.d.ts          # TypeScript support for `.vue` files and Vite-specific types
│   ├── styles.css          # Global CSS or Tailwind styles
│   └── vite-env.d.ts       # Environment type definitions for Vite
├── .gitignore
├── index.html              # Base HTML template
├── package.json            # Project metadata and dependencies
├── package-lock.json       # Exact dependency versions
├── postcss.config.cjs      # PostCSS configuration (used with Tailwind)
├── tailwind.config.js      # Tailwind CSS configuration
├── tsconfig.app.json       # TypeScript config for the app
├── tsconfig.json           # Root TypeScript configuration
├── tsconfig.node.json      # Node-specific TypeScript configuration
└── vite.config.ts          # Vite build tool configuration
```

---

## Asset Manager: Back-End Documentation

### How to Set Up the Backend

1. Install Visual Studio and set up .NET
2. Clone the repository on your local machine
3. **Install MongoDB** (see database setup below)
4. Run the project. This will automatically seed the roles, a super admin account (for API testing), and populate assets in the database.

### How to Set Up the Database

1. [Install MongoDB Compass](https://downloads.mongodb.com/compass/mongodb-compass-1.46.5-win32-x64.exe)
2. [Install MongoDB Community Edition](https://www.mongodb.com/try/download/community)
3. Go back to step 3 of backend setup

### Packages Used

- AspNetCore.Identity.MongoDbCore (6.0.0)
- Microsoft.AspNetCore.Authentication.JwtBearer (8.013)
- MongoDb.Driver (2.28.0)
- Swashbuckle.AspNetCore (6.6.2)

### Sample Folder Structure for Backend

```
AssetManager/
├── Presentation/                # Handles HTTP requests
│   ├── AssetsController.cs
│   ├── RentalsController.cs
│   └── AuthController.cs
├── Application/                 # Business logic and functions
│   ├── AssetService.cs
│   ├── RentalService.cs
│   ├── AuthService.cs
│   └── Interfaces/
│       ├── IAssetService.cs
│       ├── IRentalService.cs
│       └── IAuthService.cs
├── Domain/                      # Models and structures (entities, DTOs)
│   ├── Entities/
│   │   ├── Asset.cs
│   │   ├── Rental.cs
│   │   └── ApplicationUser.cs
│   ├── DTO/
│       ├── AssetDto.cs
│       └── RentalDto.cs
├── Data/                        # DB context and seeding
│   ├── MongoDbContext.cs
│   ├── MongoSettings.cs
│   └── AssetSeeder.cs
├── Program.cs                   # Main app entry point (Startup config)
└── appsettings.json             # App configuration (DB connection, secrets, etc.)
```

---

## Contributing

Contributions, issues, and feature requests are welcome!  
To contribute:

1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes
4. Push to the branch (`git push origin feature/your-feature`)
5. Create a Pull Request

---

## License

This project is licensed under the MIT License.

---

## Contact

For questions or support, please open an issue on GitHub.
