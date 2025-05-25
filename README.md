# 🔐 Geno Auth System – Developer Guide

This document explains how authentication and authorization work in the Geno project using ASP.NET Core Identity and JWT tokens.

---

## ✅ Overview

### Modular Monolith with layered services

- **Auth Type:** JWT (Bearer Tokens)
- **Roles:** `ADMIN`, `CUSTOMER` (ASP.NET Identity)
- **Token Handling:** Access + Refresh Token system
- **DB:** InMemory (for development/testing)
- **Email:** MailKit integration (SMTP)

---

## 📦 Key Features

| Feature           | Description                                         |
| ----------------- | --------------------------------------------------- |
| Register/Login    | Available at `/api/auth`                            |
| Role-based Access | `[Authorize]`, `[Authorize(Roles = "ADMIN")]`, etc. |
| Token Generation  | JWT tokens with refresh token support               |
| Mail Service      | Configured with `IMailService` + MailKit            |
| Swagger UI        | Integrated with JWT auth support                    |

---

## 📍 Auth Flow

1. **Register**  
   `POST /api/auth/register`

   - Creates user with `CUSTOMER` role
   - Sends confirmation email (if configured)

2. **Login**  
   `POST /api/auth/login`

   - Returns a JWT and a refresh token

3. **Use Token**  
   Add the access token to the header:

4. **Refresh Token**  
   `POST /api/auth/refresh`

- Accepts `{ token, refreshToken }` to issue a new access token

---

## 🧪 Testing in Swagger

- Open Swagger UI at: `https://localhost:<port>/swagger`
- Click **Authorize** button
- Paste your token as:

- Now test secured endpoints like `/api/products`

---

## ⚙️ Dev Setup

1. Clone the project
2. Run the app:

```bash
dotnet run

Geno/
├── Controllers/
│   ├── AuthController.cs
│   └── ProductController.cs
├── Services/
│   ├── AuthService.cs
│   └── ProductService.cs
├── Models/
│   ├── User.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Program.cs
└── README.md
```
