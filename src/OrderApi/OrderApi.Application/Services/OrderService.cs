using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.interfaces;
using OrderApi.Domain.Entity;
using Polly;
using Polly.Registry;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        // Get Product
        public async Task<ProductDTO> GetProduct(int productId)
        {
            // call prodcut API using HttpClient
            // Redirect this call to the API Gateway since prodcut API is not response to outsiders
            var getProduct = await httpClient.GetAsync($"/api/prodcuts/${productId}");
            if (!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;

        }

        // GET User
        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"/api/prodcuts/${userId}");
            if (!getUser.IsSuccessStatusCode)
            {
                return null!;
            }

            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }


        // GET ORDER DETAILS BY ID
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is not null || order!.Id <= 0)
            {
                return null!;
            }

            // Get Retry Pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // Prepare Product
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            // Prepare client
            var appUserDto = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            // Populate Order Details
            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDto.Id,
                appUserDto.Name,
                appUserDto.Email,
                appUserDto.Address,
                appUserDto.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderDate
            );
        }

        // Get Orders By Client ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            // Get all client's orders
            var orders = await orderInterface.GetOrderAsync(o => o.ClientId == clientId);
            if (!orders.Any()) return null!;

            // convert from entity to DTO
            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}