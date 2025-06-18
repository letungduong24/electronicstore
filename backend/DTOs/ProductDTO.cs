using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    public class ProductDTO
    {
         public int ID { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string ImageUrl { get; set; }

        // Dữ liệu đặc trưng của từng loại (TV, Điều hòa, Máy giặt)
        [Required]
        public Dictionary<string, object> Properties { get; set; }

        // Xác định loại sản phẩm để factory tạo đúng lớp con
        [Required]
        public string Type { get; set; }
    }
}