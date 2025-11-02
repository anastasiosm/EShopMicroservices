namespace Catalog.API.Products.GetProductById;

// Query record to request a product by its unique identifier
public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResult>;

// Result record containing the retrieved product
public record GetProductByIdResult(Product Product);

// Handler class for processing GetProductByIdQuery requests
internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
	: IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
	// Main handler method that processes the query and returns the result
	public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
	{
		// Log the incoming query for debugging and monitoring purposes
		logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);

		// Load the product from the database using the provided ID asynchronously
		var product =  await session.LoadAsync<Product>(query.Id, cancellationToken);

		// Check if the product was not found in the database
		if (product is null)
		{
			// Throw a custom exception when the product doesn't exist
			throw new ProductNotFoundException();
		}

		// Return the result containing the found product
		return new GetProductByIdResult(product);
	}
}
