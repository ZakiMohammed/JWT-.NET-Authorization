using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woo.Model
{
    public class Product
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("shortContent")]
        public string ShortContent { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("thumbImage")]
        public string ThumbImage { get; set; }
    }

    public class Service
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public class Contact
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class User
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
