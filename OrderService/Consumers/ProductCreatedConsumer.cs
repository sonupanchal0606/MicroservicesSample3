using MassTransit;
using OrderService.Data;
using OrderService.Models;
using Shared.Messages;

namespace OrderService.Consumers
{
	public class ProductCreatedConsumer : IConsumer<ProductCreated> // Subscribes to ProductCreated messages.
	{
		private readonly OrderDbContext _dbContext;

		public ProductCreatedConsumer(OrderDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task Consume(ConsumeContext<ProductCreated> context)
		{
			var order = new Order
			{
				Id = Guid.NewGuid(),
				ProductName = context.Message.Name,
				Quantity = 1,
				TotalPrice = context.Message.Price
			};

			_dbContext.Orders.Add(order);
			await _dbContext.SaveChangesAsync();
		}
	}
}
