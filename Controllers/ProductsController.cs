using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductFactoryResolver _productFactoryResolver;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ProductFactoryResolver productFactoryResolver, IMapper mapper)
        {
            _productRepository = productRepository;
            _productFactoryResolver = productFactoryResolver;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(_mapper.Map<IEnumerable<ProductResponseDTO>>(products));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductResponseDTO>(product));
        }

        // POST: api/Products
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> AddProduct(ProductCreateDTO productCreateDTO)
        {
            try
            {
                var factory = _productFactoryResolver.ResolveFactory(productCreateDTO.Type);
                var product = factory.CreateProduct();
                _mapper.Map(productCreateDTO, product);
                await _productRepository.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ID }, _mapper.Map<ProductResponseDTO>(product));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Products/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDTO productUpdateDTO)
        {
            if (id != productUpdateDTO.ID)
            {
                return BadRequest("Product ID mismatch");
            }

            if (!await _productRepository.ProductExistsAsync(id))
            {
                return NotFound();
            }

            try
            {
                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                _mapper.Map(productUpdateDTO, existingProduct);
                await _productRepository.UpdateProductAsync(existingProduct);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!await _productRepository.ProductExistsAsync(id))
            {
                return NotFound();
            }

            await _productRepository.DeleteProductAsync(id);
            return NoContent();
        }
    }
} 