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

        public abstract void SetSpecificProperties(Dictionary<string, object> values);
        public abstract Dictionary<string, object> GetSpecificProperties();
    }
} 