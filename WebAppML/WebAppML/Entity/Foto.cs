using System.Text.Json.Serialization;

namespace WebAppML.Entity
{
    public class Foto
    {
        [JsonPropertyName("source")]
        public string Fonte { get; set; }
    }
}
