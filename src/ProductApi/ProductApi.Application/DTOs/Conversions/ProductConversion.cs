using ProductApi.Domain.Entities;
using ProductApi.Application.DTOs;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            // return Single
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDTO(
                    product!.Id,
                    product.Name!,
                    product.Price,
                    product.Quantity
                );

                return (singleProduct, null);
            }

            // return list
            if (products is not null || product is null)
            {
                var _products = products.Select(p => new ProductDTO(p.Id, p.Name, p.Price, p.Quantity)).ToList();
                return (null, _products);
            }
 
            return (null, null);

        }

    }
}