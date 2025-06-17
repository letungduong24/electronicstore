using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Services.Factories;

namespace UserManagementAPI.Mapper
{
    public class ProductMapper
    {
        private readonly IProductFactory _productFactory;

        public ProductMapper(IProductFactory productFactory)
        {
            _productFactory = productFactory;
        }

        public ProductDTO ToDTO(ProductModel model)
        {
            if (model == null)
                return null;

            var dto = new ProductDTO
            {
                ID = model.ID,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                Brand = model.Brand,
                Model = model.Model,
                Category = model.Category,
                ImageUrl = model.ImageUrl,
                Type = model.Type,
                Properties = model.GetSpecificProperties()
            };


            return dto;
        }

        public ProductModel ToModel(ProductDTO dto)
        {
            if (dto == null)
                return null;

            // Create the appropriate product model using factory
            ProductModel model = _productFactory.CreateProduct(dto.Type);
            
            // Map common properties
            model.ID = dto.ID;
            model.Name = dto.Name;
            model.Description = dto.Description;
            model.Price = dto.Price;
            model.Stock = dto.Stock;
            model.Brand = dto.Brand;
            model.Model = dto.Model;
            model.Category = dto.Category;
            model.ImageUrl = dto.ImageUrl;
            model.Type = dto.Type;

            // Set specific properties
            if (dto.Properties != null)
            {
                model.SetSpecificProperties(dto.Properties);
            }

            return model;
        }
    }
}