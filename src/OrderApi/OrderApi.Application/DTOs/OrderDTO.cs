using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDTO(
        int Id,
        [Required, Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")] int ProductId,
        [Required, Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0")] int ClientId,
        [Required, Range(1, int.MaxValue, ErrorMessage = "PurchaseQuantity must be greater than 0")] int PurchaseQuantity,
        DateTime OrderDate = default
    );
}