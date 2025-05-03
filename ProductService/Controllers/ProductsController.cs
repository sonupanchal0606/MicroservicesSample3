using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Data;
using ProductService.Models;
using Shared.Messages;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ProductDbContext _context;
		private readonly IPublishEndpoint _publishEndpoint;

		public ProductsController(ProductDbContext context, IPublishEndpoint publishEndpoint)
		{
			_context = context;
			_publishEndpoint = publishEndpoint;
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct(Product product)
		{
			product.Id = Guid.NewGuid();
			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			// Publishes ProductCreated events.
			await _publishEndpoint.Publish(new ProductCreated
			{
				Id = product.Id,
				Name = product.Name,
				Stock = product.Stock,
				Price = product.Price
			});

			// ✅ This creates the product in PostgreSQL
			// ✅ Then publishes a ProductCreated event to RabbitMQ

			return Ok(product);
		}
	}
}
