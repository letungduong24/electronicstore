using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementAPI.Models
{
    public class Television : ProductModel
    {
        public string ScreenSize { get; set; }


        public override void SetSpecificProperties(Dictionary<string, object> values)
        {
            if (values == null) return;
            
            if (values.ContainsKey("ScreenSize"))
                ScreenSize = values["ScreenSize"].ToString();

        }

        public override Dictionary<string, object> GetSpecificProperties()
        {
            return new Dictionary<string, object>
            {
                { "ScreenSize", ScreenSize }
            };
        }
    }
} 