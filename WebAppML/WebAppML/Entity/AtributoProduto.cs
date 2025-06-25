using System.Text.Json.Serialization;

namespace WebAppML.Entity
{
    public class AtributoProduto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value_name")]
        public string NomeValor { get; set; }
    }
}
