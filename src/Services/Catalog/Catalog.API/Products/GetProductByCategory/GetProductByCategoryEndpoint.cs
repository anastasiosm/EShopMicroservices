namespace Catalog.API.Products.GetProductByCategory;

// public record GetProductByCategoryRequest();

// Response record containing the list of products returned by the endpoint
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

// Endpoint class that configures the HTTP route for getting products by category
public class GetProductByCategoryEndpoint : ICarterModule
{
	// Method that registers the endpoint routes using Carter module
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		// Maps a GET request to the route "/products/category/{category}"
		app.MapGet("/products/category/{category}", 
			// Async handler that receives the category from route and ISender for MediatR
			async (string category, ISender sender) =>
		{
			// Sends the query to MediatR which will route it to the appropriate handler
			var result = await sender.Send(new GetProductByCategoryQuery(category));
			// Maps the result to the response type using Mapster
			var response = result.Adapt<GetProductByCategoryResponse>();
			// Returns HTTP 200 OK with the response
			return Results.Ok(response);
		})
			// Sets the endpoint name for URL generation
			.WithName("GetProductByCategory")
			// Configures OpenAPI to document 200 OK response
			.Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
			// Configures OpenAPI to document 400 Bad Request response
			.ProducesProblem(StatusCodes.Status400BadRequest)
			// Sets the summary for OpenAPI documentation
			.WithSummary("Get Product By Category")
			// Sets the description for OpenAPI documentation
			.WithDescription("Get Product By Category");
	}
}

