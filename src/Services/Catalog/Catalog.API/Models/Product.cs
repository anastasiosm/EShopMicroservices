namespace Catalog.API.Models;

public class Product
{
	public Guid Id { get; set; }
	// default! to suppress nullable warning for non-nullable property without initializer
	public string Name { get; set; } = default!;
	// initialize to empty list to avoid null reference issues
	public List<string> Category { get; set; } = new();
	public string Description { get; set; } = default!;
	public string ImageFile { get; set; } = default!;
	public decimal Price { get; set; }
}
