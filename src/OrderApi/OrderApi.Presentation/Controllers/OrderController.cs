using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.interfaces;
using OrderApi.Application.Services;
using SharedLibrary.Responses;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        // Get all orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound("No orders found.");
            }

            var (_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound("No orders found.") : Ok(list);
        }

        // Get single Order
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order is null)
            {
                return NotFound(null);
            }

            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        // Get client orders
        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0)
            {
                return BadRequest("Invalid client ID.");
            }

            var orders = await orderInterface.GetOrderAsync(o => o.ClientId == clientId);
            return !orders.Any()
                ? NotFound("No orders found for the specified client.")
                : Ok(orders);
        }

        // Get order Details
        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("Invalid order ID.");
            }

            var orderDetails = await orderService.GetOrderDetails(orderId);
            if (orderDetails is null)
            {
                return NotFound("Order details not found.");
            }

            return Ok(orderDetails);
        }


        // Post Order
        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            // check model state if all data annotations are passed
            if (!ModelState.IsValid) return BadRequest("Incomplete data submitted");

            // convert to entity
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            /// convert from dto to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            // convert from DTO to Entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

    }
}