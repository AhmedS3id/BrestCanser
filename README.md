# Sakeena — Breast Cancer Early Detection API

A **ASP.NET Core** backend for the Sakeena mobile application, designed to support early detection and monitoring of breast cancer. The system provides user authentication, ML-powered image analysis, prediction history tracking, risk assessment, real-time notifications, and an AI chatbot.

> 🌐 **Live API:** `http://sakeena.runasp.net`

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)

---

## Overview

Sakeena's backend exposes a RESTful API covering the following modules:

- **Auth** — Register, login, token refresh, and full password recovery flow.
- **Account** — View and update user profile, change password.
- **ML (Image Scan)** — Upload a scan image and receive an AI-powered prediction result.
- **Prediction History** — Retrieve past scan results, filter by status, view statistics, and generate reports.
- **Risk Assessment** — Evaluate breast cancer risk based on user-provided health data.
- **Notifications** — Fetch notifications and mark them as read (individually or all at once).
- **Chat** — Send questions to an AI chatbot for health-related guidance.

---

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core Web API | Core framework |
| Entity Framework Core | ORM & database access |
| SQL Server | Relational database |
| JWT (Access + Refresh Tokens) | Authentication & authorization |
| SignalR | Real-time notifications |
| AutoMapper | Object mapping (Entities ↔ DTOs) |
| ML Integration | Breast cancer image analysis |
| Swagger / OpenAPI | API documentation |

---

## Project Structure

```
BrestCanser.Api/
│
├── Abstractions/                   # Shared contracts and base types
│   └── Consts/                     # Application-wide constants
│
├── Authentication/                 # JWT configuration and auth handlers
│
├── Clients/
│   └── MLModel/                    # HTTP client for the ML image analysis service
│
├── Contracts/                      # Request / Response DTOs per module
│   ├── Authentication/
│   ├── Chat/
│   ├── History/
│   ├── Notifications/
│   ├── RiskAssessment/
│   └── Users/
│
├── Controllers/                    # API endpoint controllers
├── Documents/                      # PDF report generation
├── Engine/                         # Core business logic
├── Entites/                        # EF Core domain models
├── Enum/                           # Enumerations (e.g. prediction status)
├── Errors/                         # Domain-specific error definitions
├── Extensions/                     # Extension methods
├── Helpers/                        # General-purpose utility classes
├── Hubs/                           # SignalR hubs for real-time notifications
├── Mapping/                        # AutoMapper profiles
├── Options/                        # Strongly-typed options classes
│
├── Persistance/                    # Data access layer
│   ├── EntitiesConfigurations/     # EF Core Fluent API configurations
│   └── Migrations/                 # EF Core database migrations
│
├── Services/                       # Service implementations
├── Settings/                       # Settings classes (JWT, Mail, ML, etc.)
└── Templates/                      # Email HTML templates
```

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio / VS Code

### Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd BrestCanser.Api
   ```

2. **Configure the database connection**
   Update `ConnectionStrings:DefaultConnection` in `appsettings.json` to point to your SQL Server instance.

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the project**
   ```bash
   dotnet run
   ```
   Or press **F5** in Visual Studio.

The API will be available at `https://localhost:{port}`. Use Postman or Swagger UI to explore the endpoints.

---

## Configuration

Before running, make sure the following keys are set in `appsettings.json`:

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `JWT:Key` | Secret key for signing JWT tokens |
| `JWT:Issuer` / `JWT:Audience` | Token issuer and audience |
| `JWT:ExpiryMinutes` | Access token lifetime |
| `MailSettings:*` | SMTP settings for sending emails (password reset codes) |
| ML service config | URL / API key for the image analysis service |

---

## API Endpoints

### Auth — `/Auth`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/Auth` | Login |
| POST | `/Auth/register` | Register a new account |
| POST | `/Auth/refresh` | Refresh access token |
| POST | `/Auth/revoke-refresh-token` | Revoke refresh token (logout) |
| POST | `/Auth/forget-password` | Request a password reset code |
| POST | `/Auth/verify-code` | Verify the reset code |
| POST | `/Auth/reset-password` | Set a new password |

### Account — `/account`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/account/profile` | Get current user profile |
| PUT | `/account/update-profile` | Update user profile |
| PUT | `/account/change-password` | Change password |

### ML — Image Scan `/api/ML`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/ML` | Upload a scan image and get a prediction result |

### Prediction History — `/api/PredictionHistory`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/PredictionHistory` | Get all prediction history |
| GET | `/api/PredictionHistory/with-status?status={status}` | Filter history by status (e.g. `Benign`) |
| GET | `/api/PredictionHistory/statistics` | Get prediction statistics |
| GET | `/api/PredictionHistory/report` | Get a full scan report |

### Risk Assessment — `/api/RiskAssessment`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/RiskAssessment/assess` | Assess breast cancer risk from user health data |

### Notifications — `/api/notifications`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/notifications` | Get all notifications |
| PUT | `/api/notifications/{id}/mark-read` | Mark a single notification as read |
| PUT | `/api/notifications/mark-all-read` | Mark all notifications as read |

### Chat — `/api/Chat`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Chat/ask` | Send a message to the AI chatbot |
