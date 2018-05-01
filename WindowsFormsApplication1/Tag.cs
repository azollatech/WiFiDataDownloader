using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Tag
    {
        public Tag(string id, string epc, string datetime, string uid, Boolean bib_no)
        {
            this.id = id;
            this.epc = epc;
            this.datetime = datetime;
            this.uid = uid;
            this.bib_no = bib_no;
        }

        [JsonProperty("id")]
        public string id
        { get; set; }

        [JsonProperty("epc")]
        public string epc
        { get; set; }

        [JsonProperty("datetime")]
        public string datetime
        { get; set; }

        [JsonProperty("uid")]
        public string uid
        { get; set; }

        [JsonProperty("bib_no")]
        public Boolean bib_no
        { get; set; }
    }
}
