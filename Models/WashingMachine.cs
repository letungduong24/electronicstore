using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementAPI.Models
{
    public class WashingMachine : ProductModel
    {
        public string Capacity { get; set; }

        public override void SetSpecificProperties(Dictionary<string, object> values)
        {
            if (values == null) return;
            
            if (values.ContainsKey("Capacity"))
            {
                var capacityValue = values["Capacity"];
                Console.WriteLine($"Capacity value type: {capacityValue?.GetType()}");
                Console.WriteLine($"Capacity value: {capacityValue}");
                Capacity = capacityValue?.ToString();
                Console.WriteLine($"Final Capacity: {Capacity}");
            }
        }

        public object GetSpecificProperties() => new { Capacity };
    }
} 