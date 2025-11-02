namespace Catalog.API.Products.GetProductByCategory;

// Query record representing the request to get products by category
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

// Result record containing the products returned from the query
public record GetProductByCategoryResult(IEnumerable<Product> Products);

// Internal handler class using primary constructor to inject dependencies
internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
	// Implements the query handler interface for CQRS pattern
	: IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
	// Async method that handles the query execution
	public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
	{
		// Logs information about the query being handled with structured logging
		logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}", query);

		// Queries the Marten document database for products
		var products = await session.Query<Product>()
			// Filters products where the Category list contains the requested category
			.Where(p => p.Category.Contains(query.Category))
			// Executes the query and returns a list asynchronously
			.ToListAsync();

		// Returns the result wrapped in the result record
		return new GetProductByCategoryResult(products);
	}
}
