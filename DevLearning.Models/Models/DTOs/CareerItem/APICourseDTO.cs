using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevLearning.Models.Models.DTOs.CareerItem
{
    public class APICourseDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("durationInMinutes")]
        public int DurationInMinutes { get; set; }
    }
}
