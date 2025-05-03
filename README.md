# 🛠️ MicroservicesSample3

A simple microservices-based architecture built using .NET 8, RabbitMQ, Ocelot API Gateway, and PostgreSQL — with messaging handled via MassTransit.

---

## ✅ Architecture Overview

**Tech Stack:**

- ✅ [.NET 8](https://dotnet.microsoft.com/en-us/)
- ✅ [Ocelot](https://ocelot.readthedocs.io/en/latest/) (API Gateway)
- ✅ [MassTransit](https://masstransit-project.com/) with [RabbitMQ](https://www.rabbitmq.com/)
- ✅ [PostgreSQL](https://www.postgresql.org/)
- ✅ Visual Studio (no Docker/container orchestration)
- ❌ No Docker

---

## 🔧 Solution Structure

Create a blank Visual Studio solution named `MicroservicesSolution`, then add the following projects:

| Project           | Type                        | Description                         |
|-------------------|-----------------------------|-------------------------------------|
| ProductService     | ASP.NET Core Web API         | Manages products                    |
| OrderService       | ASP.NET Core Web API         | Manages orders                      |
| ApiGateway         | ASP.NET Core Web Application (Empty) | Ocelot API Gateway          |
| Shared.Messages    | Class Library                | Message contracts shared between services |

---

## 📦 Microservices Involved

### 🛒 ProductService
- Handles product-related operations
- Publishes `ProductCreated` events via MassTransit

### 📦 OrderService
- Subscribes to `ProductCreated` events
- Consumes messages using `ProductCreatedConsumer` to update its own DB

### ✅ Add Shared.Messages as a project reference to both:
- ProductService.csproj
- OrderService.csproj

---

## 🔄 Communication Flow

```plaintext
[Client] → [API Gateway (Ocelot)]
                    |
                    ↓
          [ProductService] ---> Publishes ProductCreated → RabbitMQ
                    ↓
          [OrderService] <---- Consumes ProductCreated → Creates Order
```
---

## 📁 Folder Structure
```
MicroservicesSolution/
│
├── Shared.Messages/                 # Shared contracts for messaging      
│   └── ProductCreated.cs
│   └── Shared.Messages.csproj
│
├── ProductService/                  # Product microservice → Runs on port 5001
│   ├── Controllers/
│   │   └── ProductsController.cs
│   ├── Data/
│   │   └── ProductDbContext.cs
│   ├── Models/
│   │   └── Product.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── ProductService.csproj
│
├── OrderService/                    #  Order microservice → Runs on port 5002
│   ├── Consumers/
│   │   └── ProductCreatedConsumer.cs
│   ├── Controllers/
│   │   └── OrdersController.cs
│   ├── Data/
│   │   └── OrderDbContext.cs
│   ├── Models/
│   │   └── Order.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── OrderService.csproj
│
├── ApiGateway/                      # Ocelot API Gateway → Runs on port 9000
│   ├── ocelot.json
│   ├── Program.cs
│   └── ApiGateway.csproj
│
└── MicroservicesSolution.sln        # Solution file

```
---

## ✅ Component Responsibilities

| Folder/File        | Purpose                                                   |
| ------------------ | --------------------------------------------------------- |
| `Shared.Messages/` | Contains shared message DTOs like `ProductCreated`        |
| `Models/`          | Defines entity models for EF Core                         |
| `Data/`            | Contains `DbContext` for data access                      |
| `Consumers/`       | (OrderService) MassTransit consumers for message handling |
| `Controllers/`     | HTTP API controllers                                      |
| `ocelot.json`      | Route configuration for API Gateway                       |
| `Program.cs`       | Service configuration, DI, MassTransit, EF Core           |
| `appsettings.json` | Configuration for DB, RabbitMQ, etc.                      |

---

## 🔁 End-to-End Flow Summary
Step	Service	Action
- 1	Client	Sends POST /products to API Gateway
- 2	Ocelot	Forwards request to ProductService
- 3	ProductService	Saves product to DB and publishes ProductCreated event to RabbitMQ
MassTransit + RabbitMQ serialize the message and push it into the queue named "order-service" (because your cfg.ReceiveEndpoint("order-service", ...) targets that queue).
- 4	RabbitMQ	Delivers ProductCreated to order-service queue
- 5	OrderService	Consumes event and stores product data for orders
  
ProductCreated is a shared message defined in class library project

---

## 🧪 How to Run Locally
1) Start RabbitMQ (locally or using a cloud-hosted instance).
2) Update appsettings.json for each service with your PostgreSQL and RabbitMQ settings.
3) Set ApiGateway as the startup project in Visual Studio.
4) Run the solution.
5) Test the API by making the following request: POST http://localhost:9000/products

--- 

## 📌 Notes
- This project is designed to demonstrate basic microservices concepts using event-driven architecture.
- It is not production-ready; features like retries, observability, tracing, and security should be added before using it in a real-world system.


