namespace Catalog.API.Products.GetProductById;

// We don't have any request object because
// we will get the ID information from the request parameters.

// But, in order to follow best practices
// public record GetProductByIdRequest();

// Response record that wraps the Product entity for API response
public record GetProductByIdResponse(Product Product);


// Endpoint class that implements Carter module for registering HTTP routes
public class GetProductByIdEndpoint : ICarterModule
{
	// Method to configure and register the HTTP endpoint routes
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		// Map a GET HTTP endpoint with route parameter {id}
		app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
		{
			// Send the query through MediatR to retrieve product by ID
			var result = await sender.Send(new GetProductByIdQuery(id));

			// Map the query result to the response DTO using Mapster
			var response = result.Adapt<GetProductByIdResponse>();

			// Return HTTP 200 OK response with the product data
			return Results.Ok(response);
		})
			// Set the endpoint name for route generation and documentation
			.WithName("GetProductById")
			// Document that this endpoint produces a 200 OK response with GetProductByIdResponse
			.Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
			// Document that this endpoint can produce a 400 Bad Request problem details response
			.ProducesProblem(StatusCodes.Status400BadRequest)
			// Set the summary for OpenAPI/Swagger documentation
			.WithSummary("Get Product By Id")
			// Set the detailed description for OpenAPI/Swagger documentation
			.WithDescription("Get Product By Id");
	}
}