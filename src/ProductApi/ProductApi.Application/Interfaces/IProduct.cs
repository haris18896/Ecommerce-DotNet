using SharedLibrary.Interface;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IProduct : IGenericInterface<Product>
    {
        // Define your product-related methods here
    }
}