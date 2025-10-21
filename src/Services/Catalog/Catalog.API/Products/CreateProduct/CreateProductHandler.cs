using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

/// <summary>
/// Represents a command to create a new product with the specified details.
/// </summary>
/// <remarks>This command encapsulates the necessary information to create a product, including its name,
/// categories,  description, image file, and price. It is intended to be used in scenarios where product creation is
/// required  within the application.</remarks>
/// <param name="Name">The name of the product. This value cannot be null or empty.</param>
/// <param name="Category">A list of categories to which the product belongs. The list cannot be null, but it may be empty.</param>
/// <param name="Description">A description of the product. This value cannot be null or empty.</param>
/// <param name="ImageFile">The file name or path of the product's image. This value cannot be null or empty.</param>
/// <param name="Price">The price of the product. Must be a positive decimal value.</param>
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
	: ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);


/// <summary>
/// Handles the creation of a product based on the provided command.
/// </summary>
/// <remarks>This handler processes a <see cref="CreateProductCommand"/> and returns a <see
/// cref="CreateProductResult"/>  containing the outcome of the operation. It is typically used in a CQRS (Command Query
/// Responsibility Segregation)  pattern to encapsulate the logic for creating a product.</remarks>
internal class CreateProductCommandHandler
	: ICommandHandler<CreateProductCommand, CreateProductResult>
{
	public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
	{
		// create Product entity from command object
		// save to database
		// return CreateProductResult result

		var product = new Product
		{
			Name = command.Name,
			Category = command.Category,
			Description = command.Description,
			ImageFile = command.ImageFile,
			Price = command.Price
		};

		return new CreateProductResult(Guid.NewGuid());
	}
}

