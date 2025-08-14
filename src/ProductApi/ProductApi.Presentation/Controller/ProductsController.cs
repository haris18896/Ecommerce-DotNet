using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using SharedLibrary.Responses;

namespace ProductApi.Presentation.Controller
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            // Get all products from service Repo
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No products detected in database");
            }

            // convert data from entity to DTO and return
            var (_, list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet("id:int")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            // get single product from repo
            var product = await productInterface.FindByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with id {id} not found");
            }
            // Convert from entity to Dto
            var (_product, _dto) = ProductConversion.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound($"Product with id {id} not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            // Validate the incoming product DTO
            if (!ModelState.IsValid)
                return BadRequest("Product data is null");

            // Convert DTO to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            // Validate the incoming product DTO
            if (!ModelState.IsValid)
                return BadRequest("Product data is null");

            // Convert DTO to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            // Validate the incoming product DTO
            if (!ModelState.IsValid)
                return BadRequest("Product data is null");

            // Convert DTO to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : NotFound(response);
        }
    }
}