# YumaIdentity

<div align="center">
    <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
    <img src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" alt="MS SQL Server">
    <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker">
    <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" alt="JWT">
    <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger">
</div>
<br>


A centralized Identity and Access Management (IAM) service for your applications. Powered by .NET, structured with Clean Architecture, backed by MS SQL Server, and designed for easy Docker-based deployment.

## ✨ Key Features

- **Centralized Authentication & Authorization:** Manage users, roles, and application access from a single, secure point.
- **Client Credentials Flow:** Supports machine-to-machine (M2M) authentication using `ClientId` and `ClientSecret`.
- **JWT-Based Security:** Utilizes JSON Web Tokens (JWT) for secure API access, with support for refresh tokens.
- **Dynamic Audience Validation:** Enhances security by validating the token audience dynamically.
- **Secure Swagger UI:** Role-based access control for Swagger endpoints to protect your API documentation.
- **Clean Architecture:** A highly modular and maintainable codebase that separates concerns.
- **Dockerized:** Comes with a `Dockerfile` and `docker-compose.yml` for easy containerization and deployment.
- **Global Error Handling:** Custom middleware for consistent and secure error responses.
- **Caching Support:** Improves performance by caching frequently accessed data.

## 🛠️ Tech Stack

- **Backend:** .NET / C#
- **Database:** Microsoft SQL Server
- **Containerization:** Docker
- **Authentication:** JWT (JSON Web Tokens)
- **API Documentation:** Swagger / OpenAPI

## 🏛️ Architecture

This project is built using the principles of **Clean Architecture**. This design pattern ensures a clear separation of concerns, making the application:
- Independent of frameworks.
- Testable.
- Independent of UI.
- Independent of the database.

The main layers of the application are:
- **Domain:** Contains enterprise-wide logic, entities, and enums.
- **Application:** Contains application-specific logic, features, and interfaces.
- **Infrastructure:** Handles external concerns like database access, caching, and external APIs.
- **Presentation:** The entry point to the application (Web API), including controllers and middleware.

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/AdylshaY/YumaIdentity.git
    cd YumaIdentity
    ```

2.  **Configure Database:**
    -   Update the connection string in `appsettings.json` with your MS SQL Server details.
    -   Run database migrations to set up the schema.
    ```sh
    dotnet ef database update
    ```

### Initial Admin User Setup

The application is designed to create a default admin user upon its first launch. The source of the admin credentials depends on the environment you are running the application in.

#### Running with Visual Studio / .NET CLI

When you run the project directly from an IDE like Visual Studio or via the `dotnet run` command, the initial admin user's credentials are read from the `appsettings.json` file. Make sure you have a section like this:

```json
"AdminSeed": {
  "AdminClientId": "admin-dashboard-client",
  "AdminClientName": "YumaIdentity Admin Dashboard",
  "AdminClientSecret": "SUPER_SECRET_ADMIN_CLIENT_PASSWORD_123!",
  "SuperAdminEmail": "superadmin@yuma.com",
  "SuperAdminPassword": "Admin1234!"
}
```

#### Running with Docker Compose

When you run the project using `docker-compose`, the credentials for the initial admin user are sourced from a `.env` file located in the root directory of the project. Create a `.env` file with the following content:

```env
AdminSeed__AdminClientId=admin-dashboard-client
AdminSeed__AdminClientName=YumaIdentity Admin Dashboard
AdminSeed__AdminClientSecret=YumaIdentityAdminClientSecret
AdminSeed__SuperAdminEmail=superadmin@example.com
AdminSeed__SuperAdminPassword=SuperAdminPassword
```
The `docker-compose.yml` file is configured to pass these environment variables to the service.

> **⚠️ Security Warning:**
> It is strongly recommended to change the default admin password immediately after your first login, especially before deploying to a production environment.

### Running the Application

**Using .NET CLI:**
```sh
dotnet run
```

**Using Docker:**
```sh
docker-compose up --build
```

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

---