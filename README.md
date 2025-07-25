
# 🌐 E-Commerce Website

An **ASP.NET Core MVC** based e-commerce application that enables users to browse products, manage a shopping cart, and complete purchases securely.

---

## 📑 Table of Contents

1. [📌 Project Overview](#project-overview)
2. [🛠 Technologies Used](#technologies-used)
3. [📋 Prerequisites](#prerequisites)
4. [📥 Installation](#installation)
5. [⚙️ Configuration](#configuration)
6. [🗄 Database Setup](#database-setup)
7. [🚀 Running the Application](#running-the-application)
8. [✨ Features](#features)
9. [📁 Project Structure](#project-structure)
10. [🤝 Contributing](#contributing)
11. [📝 License](#license)
12. [📫 Contact](#contact)

---

## 📌 Project Overview

This project is a full-featured **e-commerce website** built with ASP.NET Core MVC and Entity Framework Core.
It includes user management, product listings, a shopping cart, and a secure checkout system.

### 🔑 Key Functionalities

* 🔐 User registration and login (ASP.NET Identity)
* 🛍 Product catalog with categories and search
* 🛒 Shopping cart with persistent session storage
* 📦 Checkout process and order history
* 🧾 Invoice generation
* 🛠 Admin dashboard for managing inventory

---

## 🛠 Technologies Used

* **Framework:** ASP.NET Core MVC 6.0
* **ORM:** Entity Framework Core
* **Database:** SQL Server (LocalDB or full instance)
* **Authentication:** ASP.NET Core Identity
* **Front-end:** Razor Views + Bootstrap 5
* **DI Container:** Built-in ASP.NET Core Dependency Injection

---

## 📋 Prerequisites

Ensure the following are installed on your system:

* [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* [SQL Server or LocalDB](https://www.microsoft.com/sql-server/)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (with ASP.NET workload) or VS Code

---

## 📥 Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/ecommerce-aspnetcore-mvc.git
   cd ecommerce-aspnetcore-mvc
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Build the project**

   ```bash
   dotnet build
   ```

---

## ⚙️ Configuration

### 🔧 appsettings.json

Edit the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 🔐 User Secrets (optional)

```bash
dotnet user-secrets init

# Replace with your production connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_Production_Connection_String"
```

---

## 🗄 Database Setup

1. **Apply EF Core Migrations**

```bash
dotnet ef database update
```

2. **Seed initial data** *(optional)*

* Edit `Data/DbInitializer.cs` to add products, categories, and admin user

---

## 🚀 Running the Application

### Using CLI

```bash
 dotnet run --project ECommerce.Web
```

### Using Visual Studio

* Open `ECommerce.sln`
* Set `ECommerce.Web` as startup project
* Press `F5` to debug or `Ctrl+F5` to run

🌐 Navigate to: `https://localhost:5001` or `http://localhost:5000`

---

## ✨ Features

* 🔐 **Authentication & Roles** (User, Admin)
* 🛍 **Product Listings** with category filtering
* 🛒 **Cart Management** (add, remove, update)
* 💳 **Checkout Process** with order confirmation
* 📜 **Order History** per user
* 🛠 **Admin Panel** for CRUD operations

---

## 📁 Project Structure

```
ECommerce.sln
├── ECommerce.Web         # ASP.NET Core MVC app
├── ECommerce.Data        # EF Core DbContext and migrations
├── ECommerce.Core        # Domain models and interfaces
├── ECommerce.Services    # Business logic and services
└── README.md             # This file
```

---

## 🤝 Contributing

We welcome contributions! 🚀

1. Fork this repo
2. Create a branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

Distributed under the **MIT License**. See `LICENSE` file for more info.

---

## 📫 Contact

For questions or suggestions:



* **Name**: Bhimani Pranav
* **Email**: [pranavbhimani04@gmail.com](mailto:pranavbhimani04@gmail.com)
* **GitHub**: [PranavBhimani25](https://github.com/PranavBhimani25)

---

> © 2025 Your Name. All rights reserved. Made with ❤️ using ASP.NET Core MVC.
