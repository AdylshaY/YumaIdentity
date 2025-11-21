# YumaIdentity

<div align="center">
    <img src="https://img.shields.io/badge/.NET%209.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
    <img src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" alt="MS SQL Server">
    <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker">
    <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" alt="JWT">
    <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger">
</div>
<br>

A modern, production-ready Identity and Access Management (IAM) service built with .NET 9 and Clean Architecture principles. YumaIdentity provides centralized authentication, authorization, and multi-tenant user management for your applications, backed by MS SQL Server and designed for seamless Docker deployment.

## ✨ Key Features

### Authentication & Authorization
- **User Registration & Login:** Email and password-based authentication with secure password hashing
- **JWT Token Management:** Access tokens and refresh tokens for secure, stateless authentication
- **Role-Based Access Control (RBAC):** Granular permission management with user roles
- **Multi-Tenant Architecture:** Built-in support for tenant isolation with `TenantId` and `IsIsolated` flags
- **Application Management:** Register multiple applications with unique `ClientId` and `ClientSecret`

### Security
- **Secure Password Hashing:** Industry-standard password hashing using ASP.NET Core Identity's password hasher
- **Token Security:** JWT tokens with configurable expiration times
- **Dynamic Audience Validation:** Enhanced security through token audience verification
- **Global Error Handling:** Custom middleware for consistent, secure error responses
- **Swagger Authorization:** Role-based access control for API documentation endpoints

### Architecture & Performance
- **Clean Architecture:** Separation of concerns across Domain, Application, Infrastructure, and Presentation layers
- **CQRS Pattern:** Command Query Responsibility Segregation using MediatR
- **Memory Caching:** Performance optimization through in-memory caching
- **Entity Framework Core:** Robust data access with migrations support
- **Async/Await:** Non-blocking I/O operations throughout the application

### DevOps & Deployment
- **Docker Support:** Complete containerization with multi-stage Dockerfile
- **Docker Compose:** One-command deployment with API and SQL Server
- **Health Checks:** Database health monitoring for reliable service startup
- **Environment Configuration:** Flexible configuration via appsettings and environment variables

## 🛠️ Tech Stack

- **Framework:** .NET 9.0 / C# 13
- **Architecture:** Clean Architecture with CQRS pattern
- **Database:** Microsoft SQL Server 2022
- **ORM:** Entity Framework Core 9.0
- **Mediator:** MediatR for CQRS implementation
- **Authentication:** JWT (JSON Web Tokens)
- **Caching:** In-Memory Caching
- **API Documentation:** Swagger / OpenAPI (Swashbuckle)
- **Containerization:** Docker with multi-stage builds
- **Orchestration:** Docker Compose

## 🏛️ Architecture

YumaIdentity follows **Clean Architecture** principles, ensuring maintainability, testability, and independence from external frameworks. The architecture promotes separation of concerns and dependency inversion, making the codebase resilient to change.

### Project Structure

```
YumaIdentity/
├── src/
│   ├── Core/
│   │   ├── YumaIdentity.Domain/          # Enterprise business rules
│   │   │   ├── Entities/                 # Domain entities (User, Application, Role, etc.)
│   │   │   └── Enums/                    # Domain enumerations
│   │   │
│   │   └── YumaIdentity.Application/     # Application business rules
│   │       ├── Features/                 # CQRS Commands and Queries
│   │       │   ├── Auth/                 # Authentication features
│   │       │   │   ├── Commands/         # Register, Login, RefreshToken
│   │       │   │   └── Shared/           # Shared DTOs
│   │       │   └── Admin/                # Administration features
│   │       │       ├── Commands/         # CreateApp, DeleteUser, AssignRole, etc.
│   │       │       └── Queries/          # GetUsers, GetApplications, etc.
│   │       ├── Interfaces/               # Abstraction contracts
│   │       └── Common/                   # Shared logic and exceptions
│   │
│   ├── Infrastructure/
│   │   └── YumaIdentity.Infrastructure/  # External dependencies
│   │       ├── Persistence/              # EF Core DbContext
│   │       ├── Services/                 # Service implementations
│   │       │   ├── JwtTokenGenerator.cs
│   │       │   ├── PasswordHasher.cs
│   │       │   ├── ClientValidator.cs
│   │       │   └── DatabaseSeeder.cs
│   │       └── Migrations/               # Database migrations
│   │
│   └── Presentation/
│       └── YumaIdentity.API/             # API entry point
│           ├── Controllers/              # REST API controllers
│           ├── Middleware/               # Exception handling
│           ├── Extensions/               # Service registration
│           └── Program.cs                # Application startup
│
├── docker-compose.yml                    # Container orchestration
├── Dockerfile                            # Multi-stage build configuration
└── YumaIdentity.slnx                     # Solution file
```

### Architecture Layers

#### 1. **Domain Layer** (Core/YumaIdentity.Domain)
The innermost layer containing enterprise business logic and domain models:
- **Entities:** Core business objects (User, Application, AppRole, UserRole, RefreshToken, UserToken, TokenType)
- **Enums:** Domain-specific enumerations (TokenTypes)
- **No Dependencies:** Pure business logic with no external dependencies

#### 2. **Application Layer** (Core/YumaIdentity.Application)
Contains application-specific business rules and orchestration:
- **Features:** Organized by feature using CQRS pattern
  - **Auth:** User registration, login, and token refresh
  - **Admin:** Application management, user management, role assignment
- **Interfaces:** Abstractions for infrastructure services (ITokenGenerator, IPasswordHasher, IClientValidator, IEmailService, etc.)
- **Common:** Shared exceptions, validators, and utilities
- **Dependencies:** Only depends on Domain layer

#### 3. **Infrastructure Layer** (Infrastructure/YumaIdentity.Infrastructure)
Implements interfaces from Application layer and handles external concerns:
- **Persistence:** Entity Framework Core DbContext and configurations
- **Services:** Concrete implementations (JWT generation, password hashing, client validation)
- **Database Seeding:** Automated admin user and application setup
- **Migrations:** Database schema versioning
- **Dependencies:** Depends on Application and Domain layers

#### 4. **Presentation Layer** (Presentation/YumaIdentity.API)
The API entry point and user interface:
- **Controllers:** RESTful endpoints for Auth and Admin operations
- **Middleware:** Global exception handling
- **Extensions:** Service registration and configuration helpers
- **Swagger:** Interactive API documentation
- **Dependencies:** Depends on all other layers

### Design Patterns

- **CQRS (Command Query Responsibility Segregation):** Separate read and write operations using MediatR
- **Repository Pattern:** Data access abstraction through EF Core DbContext
- **Dependency Injection:** Constructor injection throughout the application
- **Options Pattern:** Strongly-typed configuration with IOptions<T>
- **Middleware Pattern:** Request pipeline customization for cross-cutting concerns

## 🚀 Getting Started

### Prerequisites

Choose one of the following deployment methods:

**Option 1: Docker (Recommended)**
- [Docker Desktop](https://www.docker.com/products/docker-desktop) or Docker Engine
- Docker Compose (included with Docker Desktop)

**Option 2: Local Development**
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Microsoft SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

### Installation

#### 1. Clone the Repository

```bash
git clone https://github.com/AdylshaY/YumaIdentity.git
cd YumaIdentity
```

#### 2. Environment Configuration

Create a `.env` file in the root directory by copying the example:

```bash
cp .env.example .env
```

Edit the `.env` file with your own values:

```env
# Database Settings
MSSQL_SA_PASSWORD=YourStrongPassword123!

# JWT Settings
Jwt__Key=YourSuperSecretKeyThatIsAtLeast32CharactersLong!
Jwt__Issuer=https://your-domain.com
Jwt__Audience=https://your-client-app.com
Jwt__AccessTokenExpirationMinutes=15
Jwt__RefreshTokenExpirationDays=30

# Admin Seeder Settings
AdminSeed__AdminClientId=admin-dashboard-client
AdminSeed__AdminClientName=YumaIdentity Admin Dashboard
AdminSeed__AdminClientSecret=YourAdminClientSecret123!
AdminSeed__SuperAdminEmail=admin@yourdomain.com
AdminSeed__SuperAdminPassword=Admin123!
```

> **⚠️ Security Warning:** Never commit your `.env` file to version control. Always use strong, unique passwords in production.

### Deployment Options

#### Option A: Docker Deployment (Recommended)

The easiest way to run YumaIdentity is using Docker Compose, which automatically sets up both the API and SQL Server:

```bash
docker-compose up --build
```

The application will be available at:
- **API:** http://localhost:8080
- **Swagger UI:** http://localhost:8080/swagger

To run in detached mode:
```bash
docker-compose up -d
```

To stop the services:
```bash
docker-compose down
```

To remove volumes (database data):
```bash
docker-compose down -v
```

#### Option B: Local Development

##### Step 1: Configure Database Connection

If using a local SQL Server instance, update the connection string in `src/Presentation/YumaIdentity.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=YumaIdentity;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

##### Step 2: Apply Database Migrations

Navigate to the API project directory and run migrations:

```bash
cd src/Presentation/YumaIdentity.API
dotnet ef database update
```

##### Step 3: Run the Application

```bash
dotnet run
```

Or use the Visual Studio debugger (F5).

The application will be available at:
- **HTTPS:** https://localhost:7001
- **HTTP:** http://localhost:5001
- **Swagger UI:** https://localhost:7001/swagger

### Initial Admin Setup

YumaIdentity automatically creates a default admin user and application on first launch using the **Database Seeder**. This setup is essential for accessing admin endpoints.

#### What Gets Created:

1. **Super Admin User**
   - Email: From `AdminSeed__SuperAdminEmail`
   - Password: From `AdminSeed__SuperAdminPassword`
   - Role: SuperAdmin
   - Email verified: Yes

2. **Admin Dashboard Application**
   - Name: From `AdminSeed__AdminClientName`
   - ClientId: From `AdminSeed__AdminClientId`
   - ClientSecret: From `AdminSeed__AdminClientSecret`
   - Has "SuperAdmin" role pre-configured

#### Configuration Source:

- **Docker Compose:** Credentials come from `.env` file
- **Local Development:** Credentials come from `appsettings.json` in the API project

Example `appsettings.json` section:
```json
{
  "AdminSeed": {
    "AdminClientId": "admin-dashboard-client",
    "AdminClientName": "YumaIdentity Admin Dashboard",
    "AdminClientSecret": "SUPER_SECRET_ADMIN_CLIENT_PASSWORD_123!",
    "SuperAdminEmail": "superadmin@yuma.com",
    "SuperAdminPassword": "Admin1234!"
  }
}
```

> **⚠️ Production Security:**
> - Change default credentials immediately after first login
> - Use strong, unique passwords (minimum 8 characters with uppercase, lowercase, numbers, and symbols)
> - Consider implementing password rotation policies
> - Store credentials securely using Azure Key Vault, AWS Secrets Manager, or similar services

## 📚 API Documentation

### Authentication Endpoints

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "clientId": "your-app-client-id",
  "clientSecret": "your-app-client-secret"
}
```

**Response:** `200 OK`
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "clientId": "your-app-client-id",
  "clientSecret": "your-app-client-secret"
}
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "550e8400-e29b-41d4-a716-446655440000",
  "expiresIn": 900
}
```

#### Refresh Token
```http
POST /api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "550e8400-e29b-41d4-a716-446655440000",
  "clientId": "your-app-client-id",
  "clientSecret": "your-app-client-secret"
}
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "650e8400-e29b-41d4-a716-446655440001",
  "expiresIn": 900
}
```

### Admin Endpoints (Requires SuperAdmin Role)

All admin endpoints require the `Authorization: Bearer <token>` header with a valid JWT token for a SuperAdmin user.

#### Application Management

##### Get All Applications
```http
GET /api/admin/applications
Authorization: Bearer <token>
```

##### Create Application
```http
POST /api/admin/applications
Authorization: Bearer <token>
Content-Type: application/json

{
  "appName": "My Application",
  "clientId": "my-app-client",
  "clientSecret": "MyAppSecret123!",
  "allowedCallbackUrls": "http://localhost:3000/callback,https://myapp.com/callback",
  "isIsolated": false
}
```

##### Delete Application
```http
DELETE /api/admin/applications/{applicationId}
Authorization: Bearer <token>
```

##### Get Roles by Application
```http
GET /api/admin/applications/{applicationId}/roles
Authorization: Bearer <token>
```

#### User Management

##### Get All Users
```http
GET /api/admin/users
Authorization: Bearer <token>
```

##### Get User by ID
```http
GET /api/admin/users/{userId}
Authorization: Bearer <token>
```

##### Delete User
```http
DELETE /api/admin/users/{userId}
Authorization: Bearer <token>
```

##### Assign Role to User
```http
POST /api/admin/users/{userId}/roles
Authorization: Bearer <token>
Content-Type: application/json

{
  "roleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

##### Remove Role from User
```http
DELETE /api/admin/users/{userId}/roles/{roleId}
Authorization: Bearer <token>
```

### Interactive API Documentation

For a complete, interactive API documentation experience, navigate to the **Swagger UI**:
- **Docker:** http://localhost:8080/swagger
- **Local Development:** https://localhost:7001/swagger

Swagger UI allows you to:
- View all available endpoints
- Test API requests directly from the browser
- See request/response schemas
- Authorize using JWT tokens

## 🗺️ Roadmap & Future Features

Based on the current development backlog and GitHub issues, here are the planned enhancements:

### 🔐 Email & Authentication (In Progress)
- [ ] **Email Service Implementation** - Integration with MailKit for transactional emails
- [ ] **Email Verification Flow** - Verify user email addresses on registration
- [ ] **Forgot Password** - Send password reset links via email
- [ ] **Reset Password** - Secure password recovery mechanism
- [ ] **Mailpit Integration** - Email testing in development environment

### 📊 Observability & Logging (Planned)
- [ ] **Serilog Integration** - Structured logging with configurable sinks
- [ ] **Grafana Loki** - Centralized log aggregation
- [ ] **Grafana Dashboard** - Log visualization and monitoring
- [ ] **Request/Response Logging** - HTTP traffic performance tracking
- [ ] **Security Audit Logs** - Track critical security events (login attempts, role changes)

### 🏗️ Project Organization (Planned)
- [ ] **Repository Restructuring** - Separate backend and frontend directories
- [ ] **React Admin Dashboard** - Modern web UI for administration
- [ ] **Frontend Deployment** - Docker configuration for dashboard

### 🚀 Performance & Scalability
- [ ] **Distributed Caching** - Redis integration for multi-instance deployments
- [ ] **Rate Limiting** - API throttling and abuse prevention
- [ ] **Background Jobs** - Hangfire or similar for async processing

### 🔒 Advanced Security
- [ ] **Two-Factor Authentication (2FA)** - TOTP or SMS-based 2FA
- [ ] **OAuth 2.0 Providers** - Social login (Google, Microsoft, GitHub)
- [ ] **Account Lockout** - Brute force protection
- [ ] **Password Policies** - Configurable complexity requirements

### 📈 Enterprise Features
- [ ] **Audit Trail** - Complete history of all system changes
- [ ] **User Sessions Management** - View and revoke active sessions
- [ ] **API Keys** - Alternative authentication for service accounts
- [ ] **Webhook Support** - Event notifications for integrations

## 🛠️ Development

### Building from Source

```bash
# Restore dependencies
dotnet restore YumaIdentity.slnx

# Build the solution
dotnet build YumaIdentity.slnx

# Run tests (when available)
dotnet test YumaIdentity.slnx
```

### Database Migrations

Create a new migration:
```bash
cd src/Presentation/YumaIdentity.API
dotnet ef migrations add MigrationName
```

Apply migrations:
```bash
dotnet ef database update
```

Remove last migration:
```bash
dotnet ef migrations remove
```

### Docker Build

Build the image manually:
```bash
docker build -t yumaidentity:latest .
```

Run the container:
```bash
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal,1433;..." \
  -e Jwt__Key="YourSecretKey" \
  yumaidentity:latest
```

## 🐛 Troubleshooting

### Common Issues

#### Database Connection Failures

**Problem:** API cannot connect to SQL Server
```
SqlException: A network-related or instance-specific error occurred
```

**Solutions:**
- Ensure SQL Server is running and accessible
- Check connection string in `.env` or `appsettings.json`
- Verify firewall rules allow connection on port 1433
- For Docker: Use `host.docker.internal` instead of `localhost`
- Check SQL Server authentication mode (SQL Server and Windows Authentication)

#### Migration Errors

**Problem:** `dotnet ef` commands fail
```
Your startup project doesn't reference Microsoft.EntityFrameworkCore.Design
```

**Solution:**
```bash
dotnet tool install --global dotnet-ef
cd src/Presentation/YumaIdentity.API
dotnet ef database update
```

#### JWT Token Validation Errors

**Problem:** API returns 401 Unauthorized despite valid credentials

**Solutions:**
- Verify `Jwt__Key` is at least 32 characters
- Check `Jwt__Issuer` and `Jwt__Audience` match configuration
- Ensure token hasn't expired
- Verify Bearer token format: `Authorization: Bearer <token>`

#### Docker Compose Issues

**Problem:** API container exits immediately

**Solutions:**
- Check logs: `docker-compose logs auth-api`
- Verify `.env` file exists and has correct values
- Ensure SQL Server container is healthy: `docker-compose ps`
- Check port 8080 isn't already in use

### Enable Detailed Logging

For troubleshooting, increase logging verbosity in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

## 📄 License

This project is licensed under the **Apache License 2.0** - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**!

### How to Contribute

1. **Fork the Project**
   ```bash
   git clone https://github.com/YourUsername/YumaIdentity.git
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/AmazingFeature
   ```

3. **Make Your Changes**
   - Follow the existing code style and architecture patterns
   - Write clear, descriptive commit messages
   - Add tests if applicable
   - Update documentation as needed

4. **Commit Your Changes**
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```

5. **Push to Your Branch**
   ```bash
   git push origin feature/AmazingFeature
   ```

6. **Open a Pull Request**
   - Provide a clear description of the changes
   - Reference any related issues
   - Wait for review and address feedback

### Development Guidelines

- Follow Clean Architecture principles
- Use CQRS pattern for new features (Commands for writes, Queries for reads)
- Implement proper error handling and validation
- Write meaningful commit messages
- Keep pull requests focused and atomic
- Update README.md if adding new features

### Reporting Issues

Found a bug or have a feature request? Please [open an issue](https://github.com/AdylshaY/YumaIdentity/issues) with:
- Clear, descriptive title
- Detailed description of the problem or suggestion
- Steps to reproduce (for bugs)
- Expected vs actual behavior
- Environment details (OS, .NET version, Docker version)

## 👨‍💻 Author

**Adylsha Yumayev**
- GitHub: [@AdylshaY](https://github.com/AdylshaY)
- Email: adylshay@gmail.com

## 🙏 Acknowledgments

Special thanks to:
- The .NET community for excellent frameworks and tools
- Clean Architecture by Robert C. Martin (Uncle Bob)
- Contributors and users of this project

## ⭐ Support

If you find this project helpful, please consider:
- Giving it a ⭐ on GitHub
- Sharing it with others who might benefit
- Contributing improvements
- Reporting issues

---

<div align="center">
    <p>Built with ❤️ using .NET 9 and Clean Architecture</p>
    <p>© 2025 YumaIdentity. Licensed under Apache 2.0</p>
</div>