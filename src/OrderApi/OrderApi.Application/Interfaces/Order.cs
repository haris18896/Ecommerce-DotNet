using System.Linq.Expressions;
using OrderApi.Domain.Entity;
using SharedLibrary.Interface;

namespace OrderApi.Application.interfaces
{
    public interface IOrder : IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicate);
    }
}