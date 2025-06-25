using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace WebAppML.Entity
{
    public class ProdutoML
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("category_id")]
        public string IdCategoria { get; set; }

        [JsonPropertyName("price")]
        public decimal Preco { get; set; }

        [JsonPropertyName("currency_id")]
        public string IdMoeda { get; set; }

        [JsonPropertyName("available_quantity")]
        public int QtdDisponivel { get; set; }

        [JsonPropertyName("buying_mode")]
        public string ModoCompra { get; set; }

        [JsonPropertyName("condition")]
        public string Condicao { get; set; }

        [JsonPropertyName("listing_type_id")]
        public string IdTipoListagem { get; set; }

        [JsonPropertyName("sale_terms")]
        public List<TermoVenda> TermoVendas { get; set; }

        [JsonPropertyName("pictures")]
        public List<Foto> Fotos { get; set; }

        [JsonPropertyName("attributes")]
        public List<AtributoProduto> Atributos { get; set; }
    }
}
