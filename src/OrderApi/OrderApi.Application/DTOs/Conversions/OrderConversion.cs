using OrderApi.Domain.Entity;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderDate = order.OrderDate,
        };

        public static (OrderDTO? SingleOrder, IEnumerable<OrderDTO>? OrderList) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            // return single
            if (order is not null || orders is null)
            {
                var singleOrder = new OrderDTO(
                    order!.Id,
                    order.ClientId,
                    order.ProductId,
                    order.PurchaseQuantity,
                    order.OrderDate
                );

                return (singleOrder, null);
            }

            // return list
            if (orders is not null)
            {
                var _orders = orders.Select(o => new OrderDTO(
                    o.Id,
                    o.ClientId,
                    o.ProductId,
                    o.PurchaseQuantity,
                    o.OrderDate
                ));

                return (null, _orders);
            }

            return (null, null);
        }
    }
}