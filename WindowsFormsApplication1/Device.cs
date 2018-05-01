using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Device
    {
        [JsonProperty("name")]
        public string name
        { get; set; }
    }
}
