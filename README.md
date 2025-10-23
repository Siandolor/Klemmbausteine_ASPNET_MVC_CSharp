# Klemmbausteine Inventory Management System

## Overview
**Klemmbausteine** is a web-based inventory management system built with **ASP.NET Core MVC** and **Entity Framework Core**.  
It allows you to manage building block products, track stock, handle purchases and sales, and analyze profitability.

---

## Features
- Product management (create, edit, view, delete)
- Purchase and sales tracking with automatic stock updates
- Profit/loss calculation per product
- Category-based product filtering
- Responsive Bootstrap 5 layout
- SQL Server database integration

---

## Tech Stack
- **Backend:** ASP.NET Core 8.0 MVC
- **Database:** Microsoft SQL Server (Entity Framework Core)
- **Frontend:** Bootstrap 5, HTML5, CSS3, Razor Views
- **Language:** C#

---

## Project Structure
```
Klemmbausteine/
│
├── Controllers/
│   └── ProductsController.cs        # Handles product CRUD and business logic
│
├── Models/
│   ├── Product.cs                   # Product entity
│   ├── Purchase.cs                  # Purchase entity
│   └── Sale.cs                      # Sale entity
│
├── Data/
│   └── KlemmbausteineContext.cs     # EF Core database context
│
├── Views/
│   └── Products/                    # Razor views (Index, Create, Edit, Delete, Details)
│
├── wwwroot/
│   ├── css/                         # site.css, layout styles
│   ├── js/                          # site.js (scripts)
│   └── lib/                         # Bootstrap, jQuery
│
├── Program.cs                       # Application entry point
└── README.md                        # This file
```

---

## Getting Started

### Prerequisites
- .NET 9 SDK or later  
- SQL Server (LocalDB or full instance)  
- Visual Studio 2022 / VS Code with C# extension

### Setup Steps
1. Clone this repository:
   ```bash
     git clone https://github.com/yourusername/Klemmbausteine.git
     cd Klemmbausteine
   ```

2. Configure the connection string in **appsettings.json**:
   ```json
     "ConnectionStrings": {
       "KlemmbausteineContext": "Server=(localdb)\\mssqllocaldb;Database=KlemmbausteineDB;Trusted_Connection=True;"
     }
   ```

3. Apply migrations and update the database:
   ```bash  
      dotnet restore
      dotnet ef database update
   ```

4. Run the application:
   ```bash
      dotnet run
   ```

5. Open your browser and navigate to:
   ```text
      https://localhost:5001
   ```

---

## Author
**Daniel Fitz, MBA, MSc, BSc**  
Vienna, Austria  
Developer & Security Technologist — *Post-Quantum Cryptography, Blockchain/Digital Ledger & Simulation*  
C/C++ · C# · Java · Python · Visual Basic · ABAP · JavaScript/TypeScript

International Accounting · Macroeconomics & International Relations · Physiotherapy · Computer Sciences  
Former Officer of the German Federal Armed Forces

---

## License
**MIT License** — free for educational and research use.  
Attribution required for redistribution or commercial adaptation.

---

> “Klemmbausteine – minimal demo for buying and selling a product, maximal saleforce.”  
> — Daniel Fitz, 2025
