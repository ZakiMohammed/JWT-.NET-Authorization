using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woo.Model
{
    public class UserDTO
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class PaginationDTO<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
        [JsonProperty("filterCount")]
        public int FilterCount { get; set; }
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
