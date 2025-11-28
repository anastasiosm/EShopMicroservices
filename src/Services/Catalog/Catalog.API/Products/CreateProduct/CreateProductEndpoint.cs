namespace Catalog.API.Products.CreateProduct;

/// <summary>
/// Represents a request to create a new product with the specified details.
/// </summary>
/// <remarks>This record encapsulates the necessary information for creating a product, including its name,
/// categories,  description, image file, and price. All fields are required to ensure the product is properly
/// defined.</remarks>
/// <param name="Name">The name of the product. This value cannot be null or empty.</param>
/// <param name="Category">A list of categories to which the product belongs. The list must contain at least one category.</param>
/// <param name="Description">A description of the product. This value cannot be null or empty.</param>
/// <param name="ImageFile">The file name or path of the product's image. This value cannot be null or empty.</param>
/// <param name="Price">The price of the product. Must be a positive decimal value.</param>
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

/// <summary>
/// Represents the response returned after successfully creating a product.
/// </summary>
/// <remarks>This response contains the unique identifier of the newly created product.</remarks>
/// <param name="Id"></param>
public record CreateProductResponse(Guid Id);

/// <summary>
/// Defines the endpoint for creating a new product in the catalog.
/// </summary>
/// <remarks>This class implements the <see cref="ICarterModule"/> interface to define the routing and handling of
public class CreateProductEndpoint : ICarterModule
{
	/// <summary>
	/// Configures and adds endpoint routes to the specified route builder.
	/// </summary>
	/// <remarks>This method is intended to be used during application startup to define the application's routing
	/// configuration.</remarks>
	/// <param name="app">The <see cref="IEndpointRouteBuilder"/> used to define and configure the application's endpoints. Cannot be <see
	/// langword="null"/>.</param>
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/products",
			// we are defining an asynchronous lambda function that handles HTTP POST requests to the "/products" endpoint.  
			async (CreateProductRequest request, ISender sender) =>
		{
			// Inside the lambda function, we are using the Mapster library to adapt (or map) the incoming CreateProductRequest object to a CreateProductCommand object. 
			var command = request.Adapt<CreateProductCommand>();

			// Send the command to the mediator for processing
			var result = await sender.Send(command);

			// Map the result to a response
			var response = result.Adapt<CreateProductResponse>();

			// Return a Created response with the location of the new resource
			return Results.Created($"/products/{response.Id}", response);
		})
		// Configure the endpoint metadata
		.WithName("CreateProduct")
		.Produces<CreateProductResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Create Product")
		.WithDescription("Create Product");
	}
}

