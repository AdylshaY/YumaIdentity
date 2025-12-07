# YumaIdentity

<div align="center">
    <img src="https://img.shields.io/badge/.NET%209.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 9">
    <img src="https://img.shields.io/badge/License-AGPL%20v3-blue?style=for-the-badge&logo=gnu" alt="License">
    <img src="https://img.shields.io/badge/Architecture-Clean-green?style=for-the-badge" alt="Clean Architecture">
</div>
<br>

**YumaIdentity** is a centralized Identity and Access Management (IAM) service built for modern applications. This project hosts both Backend (API) and Frontend (UI) components in a **Monorepo** structure.

It aims to solve enterprise needs with hybrid multi-tenancy support, secure JWT authentication, and Role-Based Access Control (RBAC).

## 📂 Project Structure

This repository is structured to isolate different layers of the application. Each folder contains its own detailed installation and usage documentation (README).

| Module | Description | Technology | Documentation |
| :--- | :--- | :--- | :--- |
| **[`backend/`](./backend)** | REST API containing authentication, database, and business logic. | .NET 9, EF Core, SQL Server | [Backend README](./backend/README.md) |
| **[`frontend/`](./frontend)** | *(Under Development)* Web application providing user and admin interfaces. | React / Next.js (Planned) | [Frontend README](./frontend/README.md) |

## 🚀 Getting Started

To start the project locally, navigate to the directory of the relevant module and follow the instructions there.

**For example, to run the Backend:**

```bash
cd backend
# Follow the steps in the Backend README
```

*Note: In future versions, docker-compose orchestration will be added to the root directory to launch the entire system (Backend + Frontend + DB) with a single command.*

## 🔮 Roadmap & Future Plans

YumaIdentity aktif olarak geliştirilmektedir. Aşağıda projenin ana kilometre taşları ve hedefleri yer almaktadır:

### [v1.0.1 - Monorepo Restructuring](https://github.com/AdylshaY/YumaIdentity/milestone/3)
Focuses on preparing the repository structure for future frontend integrations.
-   [X] **Monorepo Layout:** Moving the current .NET backend into a `backend/` directory.
-   [X] **Frontend Prep:** Establishing an empty `frontend/` directory structure.

### [v1.1.0 - Email Integration & Account Security](https://github.com/AdylshaY/YumaIdentity/milestone/1)
Focuses on completing the authentication cycle with email-based workflows.
-   [X] **Email Service:** Implementing `IEmailService` using **MailKit**.
-   [X] **Verification:** Implementing User Email Verification flow.
-   [X] **Recovery:** Implementing **Forgot Password** & **Reset Password** flows.

### [v1.2.0 - Observability with PLG Stack](https://github.com/AdylshaY/YumaIdentity/milestone/2)
Focuses on enterprise-grade logging and monitoring using the **Loki & Grafana** stack.
-   [X] **Infrastructure:** Setting up Loki & Grafana in Docker.
-   [X] **Structured Logging:** Integrating **Serilog** to push logs directly to Loki.
-   [X] **Auditing:** Implementing Request & Security Audit logging.

### [v1.3.0 - Architectural Refactoring: Remove MediatR](https://github.com/AdylshaY/YumaIdentity/milestone/4)
Focuses on ensuring the project remains 100% free and open-source by removing restrictive external dependencies.
-   [X] **Dependency Removal**: Removed MediatR NuGet package to avoid "Lucky Penny" license warnings.
-   [X] **Custom Mediator:** Implemented a lightweight, internal Mediator pattern in the Core layer.

### [v2.0.0 - Admin Dashboard UI]()
Focuses on building the visual interface for SuperAdmins and Tenant Admins.
-   [X] **Frontend Setup:** Initializing React + Vite project with TypeScript.
-   [X] **Auth Pages:** Implementing Login, Register, and Forgot Password screens.
-   [X] **Admin Panel:** Implementing CRUD screens for Users and Applications.

## 🤝 Contributing

Your contributions are valuable! Please make sure to review the README file of the relevant module (backend or frontend) before creating a "Feature Request" or "Bug Report".

1. Fork the Project

2. Create your Feature Branch (git checkout -b feature/AmazingFeature)

3. Commit your Changes (git commit -m 'Add some AmazingFeature')

4. Push to the Branch (git push origin feature/AmazingFeature)

5. Open a Pull Request

## 📄 License

This project is licensed under the **GNU Affero General Public License v3.0 (AGPL-3.0).**

See the [LICENSE](https://github.com/AdylshaY/YumaIdentity/blob/main/LICENSE) file for details. This license generally means that if you modify the source code and make it available over a network, you must also make your modified source code available to users.
