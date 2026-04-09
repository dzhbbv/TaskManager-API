# TaskManager API

A robust Task Management Backend built with **.NET 10**. This project implements modern security standards and clean architecture patterns.

---
## Tech Stack
* **Language:** C#
* **Framework:** .NET 10.0 (Web API)
* **Security:** **JWT (JSON Web Tokens)** & **BCrypt** password hashing
* **Design Patterns:** Repository Pattern, Dependency Injection, DTOs
* **Storage:** In-Memory (SQL/PostgreSQL with EF Core integration coming soon)
* **OS:** Developed on **Arch Linux** (Hyprland) using **JetBrains Rider**

---
## Security Features
* **JWT Authentication:** Custom `AuthService` implementation with claims (Name, Id).
* **Password Hashing:** Secure password storage using `BCrypt.Net`.
* **Token Management:** 30-minute expiration and custom Issuer/Audience validation.
---
## Architecture
* **Controllers:** Clean RESTful endpoints.
* **Services:** Business logic (Auth, Task, User).
* **Repositories:** Data abstraction layer.
* **Models:** Strict separation between `Entities` and `DTOs`.
---
## Getting Started
Follow these steps to get the project up and running on your local machine.

1. **Prerequisites**
Ensure you have the following installed:
- .NET 10 SDK (Check with dotnet --version)
- Git (Check with git --version)

1. **Installation & Setup**
Clone the repository and prepare the environment:
```bash
# Clone the repository
git clone https://github.com/dzhbbv/TaskManager-API.git

# Navigate to the project directory
cd "TaskManager-API"
  
# Restore NuGet packages
dotnet restore
```

3. **Running the Application**
To start the API server, run:
```bash
dotnet run --project "TaskManager API/TaskManager API.csproj"
```

4. **Accessing the API**
Once the application is running, it will be available at:
- Base URL: https://localhost:7196 (Default)
- Interactive Documentation (Swagger): https://localhost:7196/swagger

*Note: If the port is different, please check the settings in TaskManager API/Properties/launchSettings.json.*

#### Testing with Swagger
After running the app, open the Swagger UI in your browser. It allows you to:  
- Register/Login users and get your JWT.
- Test Endpoints (GET, POST, PUT, DELETE) without using external tools like Postman.
- Explore Models and see the required JSON structure for tasks and users.
---
## Roadmap
- [x] Initial Project Setup & Architecture
- [x] In-Memory Data Storage
- [x] **JWT Authentication System**
- [x] **Secure Password Hashing (BCrypt)**
- [x] Entity Framework Core integration
- [x] PostgreSQL Database connection
- [ ] Docker Containerization
---
## Author
**dzhbbv**
* Aspiring Software Engineer from KCHR 🇷🇺
* Linux Enthusiast (Arch/Hyprland)
* Backend specialist in training
