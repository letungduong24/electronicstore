using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public abstract class ProductModel
    {
        [Required]
        public int ID { get; set; }

        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public string Type { get; set; }

        // Abstract methods that truly need polymorphism
        public abstract void SetSpecificProperties(Dictionary<string, object> values);
        public abstract Dictionary<string, object> GetSpecificProperties();
        
        // Business logic methods that can be overridden
        public virtual bool IsAvailableForPurchase()
        {
            return Stock > 0;
        }

        public virtual decimal CalculateDiscountPrice(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100");
            
            return Price * (1 - discountPercentage / 100);
        }

        public virtual string GetProductSummary()
        {
            return $"{Brand} {Model} - {Name}";
        }

        public virtual bool ValidateProduct()
        {
            return !string.IsNullOrEmpty(Name) && 
                   !string.IsNullOrEmpty(Description) && 
                   Price > 0 && 
                   Stock >= 0;
        }

        // Template method pattern
        public string GetFullProductInfo()
        {
            var summary = GetProductSummary();
            var availability = IsAvailableForPurchase() ? "Available" : "Out of Stock";
            var specificProps = GetSpecificProperties();
            
            var specificInfo = string.Join(", ", specificProps.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
            
            return $"{summary} - {availability} - {specificInfo}";
        }
    }
} 