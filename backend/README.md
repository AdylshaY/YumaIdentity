# YumaIdentity

<div align="center">
    <img src="https://img.shields.io/badge/.NET%209.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 9">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
    <img src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" alt="MS SQL Server">
    <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker">
    <img src="https://img.shields.io/badge/License-AGPL%20v3-blue?style=for-the-badge&logo=gnu" alt="License">
</div>
<br>

**YumaIdentity** is a centralized Identity and Access Management (IAM) service built for modern applications. Powered by **.NET 9** and designed with **Clean Architecture**, it supports hybrid multi-tenancy, secure JWT authentication, and robust role-based authorization.

## 🚀 Key Features

-   **🔐 Centralized Authentication:** Single entry point for identity management across multiple client applications.
-   **🏢 Hybrid Multi-Tenancy:** Configure client applications as **Global** (shared user pool) or **Isolated** (app-specific user pool) depending on your business needs.
-   **🛡️ High Security:**
    -   Client Secrets and User Passwords are securely hashed using **BCrypt**.
    -   Robust Access Token & Refresh Token rotation mechanism.
    -   Dynamic Audience validation.
-   **🏗️ Clean Architecture:** Built using the CQRS pattern with **MediatR**, ensuring a decoupled and maintainable codebase.
-   **🐳 Docker Ready:** Fully containerized with `docker-compose` for instant deployment including SQL Server.
-   **⚙️ Automatic Seeding:** Automatically initializes the SuperAdmin account and Admin Dashboard client upon first launch.

## 🛠️ Tech Stack

-   **Framework:** .NET 9.0.
-   **Data Access:** Entity Framework Core 9.0 (Code-First).
-   **Database:** Microsoft SQL Server 2022.
-   **Pattern:** CQRS (MediatR), Repository Pattern.
-   **Auth:** JWT Bearer Authentication.
-   **Documentation:** Swagger / OpenAPI.

## 🏛️ Architecture

The project follows the **Onion Architecture** (Clean Architecture) principles to ensure separation of concerns:

1.  **Core (Domain & Application):** Contains enterprise logic, entities, enums, and use cases. No external dependencies.
2.  **Infrastructure:** Implements interfaces for database access, JWT generation, password hashing, and external services.
3.  **Presentation (API):** The entry point (REST API), handling HTTP requests, middleware, and controllers.

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### Prerequisites

-   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
-   [Docker Desktop](https://www.docker.com/products/docker-desktop) (Optional, for containerized run).

### Option 1: Running with Docker (Recommended)

This will start both the API and the SQL Server database.

1.  Clone the repository:
    ```bash
    git clone [https://github.com/AdylshaY/YumaIdentity.git](https://github.com/AdylshaY/YumaIdentity.git)
    cd YumaIdentity
    ```

2.  Configure the environment:
    -   Rename `.env.example` to `.env`.
    -   Update the variables (especially `MSSQL_SA_PASSWORD` and `Jwt__Key`) with your own secure values.

3.  Start the services:
    ```bash
    docker-compose up --build
    ```

### Option 2: Running Locally

1.  Update `appsettings.Development.json` with your local SQL Server connection string.
2.  Apply database migrations:
    ```bash
    dotnet ef database update --project src/Infrastructure/YumaIdentity.Infrastructure --startup-project src/Presentation/YumaIdentity.API
    ```
3.  Run the application:
    ```bash
    dotnet run --project src/Presentation/YumaIdentity.API
    ```

## 🔌 API Documentation

This project uses **Swagger/OpenAPI** for live API documentation.

Once the application is running, navigate to:
**`http://localhost:8080/swagger`** (or the port configured in your launch settings).

Here you can:
-   Explore all available endpoints.
-   Test API calls directly.
-   See the required request/response schemas.
-   Authorize using your JWT token.

> **Note:** Swagger is the single source of truth for the most up-to-date API contracts.

## 🔐 Default Admin Credentials

When the application starts for the first time, it seeds the database with a **SuperAdmin** user and a default **Admin Client**.

You can configure these initial credentials via the `.env` file (for Docker) or `appsettings.json` (for local dev).

**Default Credentials**

- **Docker (`.env.example`):**
    - **Email:** `superadmin@example.com`
    - **Password:** `SuperAdminPassword`
    - **Admin Client ID:** `admin-dashboard-client`
    - **Admin Client Secret:** `YumaIdentityAdminClientSecret`

- **Local Development (`appsettings.Development.json`):**
    - **Email:** `superadmin@yuma.com`
    - **Password:** `SuperAdminPassword`
    - **Admin Client ID:** `admin-dashboard-client`
    - **Admin Client Secret:** `YumaIdentityAdminClientSecret`

> ⚠️ **Security Warning:** Please change these credentials immediately after deployment or in your production configuration.

> **Note:** The default SuperAdmin email differs between Docker and local development environments. You can configure these credentials in your `.env` or `appsettings.json` files as needed.
