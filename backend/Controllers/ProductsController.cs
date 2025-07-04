using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Products/types
        [HttpGet("types")]
        public ActionResult<string[]> GetSupportedProductTypes()
        {
            var types = _productService.GetSupportedProductTypes();
            return Ok(types);
        }

        // GET: api/Products/types/display-names
        [HttpGet("types/display-names")]
        public ActionResult<Dictionary<string, string>> GetTypeDisplayNames()
        {
            var displayNames = _productService.GetTypeDisplayNames();
            return Ok(displayNames);
        }

        // GET: api/Products/types/{type}/properties
        [HttpGet("types/{type}/properties")]
        public ActionResult<Dictionary<string, string[]>> GetRequiredPropertiesForType(string type)
        {
            try
            {
                var properties = _productService.GetRequiredPropertiesForType(type);
                return Ok(properties);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Products/latest
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetLatestProducts()
        {
            var products = await _productService.GetLatestProductsAsync(4);
            return Ok(products);
        }

        // POST: api/Products
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                var product = await _productService.CreateProductAsync(productDto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ID }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Products/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
} 