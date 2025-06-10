using ElectronicStore.DTOs;
using ElectronicStore.Models;
using ElectronicStore.Repositories;
using ElectronicStore.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;

    public ProductsController(IProductRepository productRepository, ProductMapper productMapper)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products.Select(_productMapper.MapModelToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productRepository.GetOneAsync(id);
        if (product == null)
            return NotFound();

        return Ok(_productMapper.MapModelToDto(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(ProductDto productDto)
    {
        try
        {
            var product = _productMapper.MapDtoToModel(productDto);
            await _productRepository.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.ID }, _productMapper.MapModelToDto(product));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDto productDto)
    {
        try
        {
            var existingProduct = await _productRepository.GetOneAsync(id);
            if (existingProduct == null)
                return NotFound();

            _productMapper.MapDtoToExistingModel(productDto, existingProduct);

            await _productRepository.UpdateAsync(existingProduct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 