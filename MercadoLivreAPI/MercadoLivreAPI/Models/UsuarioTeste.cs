using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MercadoLivreAPI.Models
{
    public class UsuarioTeste
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("nickname")]
        public string Usuario { get; set; }

        [JsonPropertyName("password")]
        public string Senha { get; set; }

        [JsonPropertyName("site_status")]
        public string SiteStatus { get; set; }
    }
}
