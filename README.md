# EShopMicroservices 

A modern e-commerce platform built using microservices architecture with .NET 8, implementing best practices for cloud-native applications.

## 🏗️ Architecture

This project follows a **microservices architecture** with the following key principles:

- **CQRS (Command Query Responsibility Segregation)** - Separation of read and write operations
- **Vertical Slice Architecture** - Feature-based organization instead of technical layers
- **Clean Architecture** - Clear separation of concerns and dependencies
- **Docker Support** - Containerization for easy deployment and scalability

## 📂 Project Structure

## 🛠️ Technologies & Frameworks

### Core
- **.NET 8** - Latest .NET framework
- **C# 12** - Modern C# features

### Libraries & Packages
- **Carter** - Minimal API organization and routing
- **MediatR** - Mediator pattern implementation for CQRS
- **Mapster** - High-performance object mapping
- **Docker** - Containerization

## 📦 Microservices

### Catalog Service
Manages product catalog operations including creating, reading, updating, and deleting products.

**Features:**
- ✅ Create Product

**Planned Features:**
- ⏳ Get Products
- ⏳ Get Product by ID
- ⏳ Update Product
- ⏳ Delete Product

## 🔧 BuildingBlocks

The `BuildingBlocks` project contains shared infrastructure and abstractions used across all microservices:

### CQRS Infrastructure
- **ICommand / ICommand\<TResponse\>** - Command abstractions for write operations
- **IQuery\<TResponse\>** - Query abstractions for read operations
- **ICommandHandler** - Command handler contracts
- **IQueryHandler** - Query handler contracts

These interfaces integrate seamlessly with MediatR to provide a consistent CQRS implementation across all services.

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Docker (optional, for containerized deployment)
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**
2. **Restore dependencies** - `dotnet restore`
3. **Run the application** - `dotnet run`
4. **Using Docker** - docker build -t catalog-api -f Services/Catalog/Catalog.API/Dockerfile . docker run -p 8080:8080 catalog-api

## 📋 API Examples

### Create Product

**Endpoint:** `POST /products`

**Request Body:**



