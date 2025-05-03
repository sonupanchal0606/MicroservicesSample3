using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;

namespace OrderService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly OrderDbContext _context;

		public OrdersController(OrderDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetOrders()
		{
			var orders = await _context.Orders.ToListAsync();
			return Ok(orders);
		}
	}
}
