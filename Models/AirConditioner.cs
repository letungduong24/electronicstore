using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementAPI.Models
{
    public class AirConditioner : ProductModel
    {
        public string Scope { get; set; }


        public override void SetSpecificProperties(Dictionary<string, object> values)
        {
            if (values == null) return;
            
            if (values.ContainsKey("Scope"))
                Scope = values["Scope"].ToString();

        }

        public override Dictionary<string, object> GetSpecificProperties()
        {
            return new Dictionary<string, object>
            {
                { "Scope", Scope }
            };
        }
    }
} 