# CQRS Pattern - Quick Reference Guide

## Overview

This project implements the CQRS (Command Query Responsibility Segregation) pattern using the MediatR library. The pattern separates operations into:
- Commands: modify data (Create, Update, Delete)
- Queries: read data (Get, Search, Filter)

The guide below shows recommended project structure, handler and endpoint examples, and quick checklists to follow when you add commands and queries.

---

## Project Structure

Use a feature-based folder layout. Example:

```
YourFeature/
  CreateFeature/
    CreateFeatureCommand.cs      (Command + DTOs + Handler)
    CreateFeatureEndpoint.cs     (API Endpoint)
  GetFeature/
    GetFeatureQuery.cs           (Query + DTOs + Handler)
    GetFeatureEndpoint.cs        (API Endpoint)
```

---

## Creating a New Command

### Step 1: Create Folder

```
Products/CreateProduct/
```

### Step 2: Create Handler File (`CreateProductHandler.cs`)

```csharp
namespace Catalog.API.Products.CreateProduct;

// 1. Define the Command record
public record CreateProductCommand(
    string Name, 
    List<string> Category, 
    string Description, 
    string ImageFile, 
    decimal Price
) : ICommand<CreateProductResult>;

// 2. Define the Result record
public record CreateProductResult(Guid Id);

// 3. Implement the Handler
internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken)
    {
        // Your business logic here
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
```

### Step 3: Create Endpoint File (`CreateProductEndpoint.cs`)

```csharp
namespace Catalog.API.Products.CreateProduct;

// 1. Define Request record
public record CreateProductRequest(
    string Name, 
    List<string> Category, 
    string Description, 
    string ImageFile, 
    decimal Price
);

// 2. Define Response record
public record CreateProductResponse(Guid Id);

// 3. Implement Carter Endpoint
public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", 
            async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}
```

---

## Creating a New Query

### Step 1: Create Folder

```
Products/GetProducts/
```

### Step 2: Create Handler File (`GetProductsHandler.cs`)

```csharp
namespace Catalog.API.Products.GetProducts;

// 1. Define the Query record
public record GetProductsQuery() : IQuery<GetProductsResult>;

// 2. Define the Result record
public record GetProductsResult(IEnumerable<Product> Products);

// 3. Implement the Handler
internal class GetProductsQueryHandler(
    IDocumentSession session, 
    ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(
        GetProductsQuery query, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

        var products = await session.Query<Product>().ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}
```

### Step 3: Create Endpoint File (`GetProductsEndpoint.cs`)

```csharp
namespace Catalog.API.Products.GetProducts;

// 1. Define Response record
public record GetProductsResponse(IEnumerable<Product> Products);

// 2. Implement Carter Endpoint
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var query = new GetProductsQuery();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
```

---

## Key Interfaces (BuildingBlocks)

### ICommand

```csharp
public interface ICommand : ICommand<Unit> { }
public interface ICommand<out TResponse> : IRequest<TResponse> { }
```

### ICommandHandler

```csharp
public interface ICommandHandler<in TCommand, TResponse> 
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{ }
```

### IQuery

```csharp
public interface IQuery<out TResponse> : IRequest<TResponse>
 where TResponse : notnull
{ }
```

### IQueryHandler

```csharp
public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{ }
```

---

## Quick Checklist

### For Commands:
- [ ] Create folder: `Feature/ActionName/`
- [ ] Define `Command` record inheriting `ICommand<TResult>`
- [ ] Define `Result` record
- [ ] Implement `Handler` class with `ICommandHandler<TCommand, TResult>`
- [ ] Define `Request` and `Response` records
- [ ] Implement `Endpoint` class with `ICarterModule`
- [ ] Use `MapPost`, `MapPut`, or `MapDelete` for the endpoint

### For Queries:
- [ ] Create folder: `Feature/QueryName/`
- [ ] Define `Query` record inheriting `IQuery<TResult>`
- [ ] Define `Result` record
- [ ] Implement `Handler` class with `IQueryHandler<TQuery, TResult>`
- [ ] Define `Response` record
- [ ] Implement `Endpoint` class with `ICarterModule`
- [ ] Use `MapGet` for the endpoint

---

## Tips

1. Naming Convention: Use `{Action}{Entity}Command/Query` (e.g., `CreateProductCommand`).
2. Dependency Injection: MediatR automatically discovers and registers handlers when configured.
3. Logging: Add `ILogger` to handlers for debug and traceability.
4. Mapping: Use Mapster (`Adapt<>()`) or similar to map between DTOs and domain models.
5. Carter Module: Endpoints are automatically registered via `ICarterModule` when Carter is added to the app.

---

## Example with Parameters

### Query with Route Parameter:
```csharp
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

// Endpoint:
app.MapGet("/products/category/{category}", 
    async (string category, ISender sender) =>
{
    var result = await sender.Send(new GetProductByCategoryQuery(category));
    return Results.Ok(result.Adapt<GetProductByCategoryResponse>());
});
```

---

## Dependencies

- MediatR: Mediator pattern implementation
- Carter: Lightweight library for defining HTTP endpoints
- Mapster: Object-to-object mapping

---

## How It Works

1. Client sends HTTP request to Endpoint
2. Endpoint creates a Command/Query object
3. Endpoint sends it via MediatR (`ISender.Send()`)
4. MediatR routes to the appropriate Handler
5. Handler executes business logic and returns Result
6. Endpoint maps Result to Response and returns to client

```
Client -> Endpoint -> Command/Query -> MediatR -> Handler -> Result -> Response -> Client
```

---

**Created for**: EShop Microservices Project  
**Pattern**: CQRS with MediatR  
**Framework**: .NET 8, C# 12  
**Location**: `BuildingBlocks\CQRS\CQRS-Pattern-README.md`
