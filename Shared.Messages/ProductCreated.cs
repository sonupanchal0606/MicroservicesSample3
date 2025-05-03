namespace Shared.Messages
{
	// Reference this project in both ProductService and OrderService.
	public class ProductCreated
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Stock { get; set; }
		public decimal Price { get; set; }
	}
}
