using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MercadoLivreAPI.Models;

namespace API_ML
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        const string token = "SEU_ACCESS_TOKEN_AQUI";

        static async Task Main(string[] args)
        {
            await CriarProdutoAsync();
            // await GetProduto("MLA123456789");
        }

        static async Task CriarProdutoAsync()
        {
            Product product = new Product
            {
                title = "Item de test - No Ofertar",
                category_id = "MLA3530",
                price = 350,
                currency_id = "ARS",
                available_quantity = 10,
                buying_mode = "buy_it_now",
                condition = "new",
                listing_type_id = "gold_special",
                sale_terms = new List<SaleTerm>
                {
                    new SaleTerm { id = "WARRANTY_TYPE", value_name = "Garantía del vendedor" },
                    new SaleTerm { id = "WARRANTY_TIME", value_name = "90 días" }
                },
                pictures = new List<Picture>
                {
                    new Picture { source = "http://mla-s2-p.mlstatic.com/968521-MLA20805195516_072016-O.jpg" }
                },
                attributes = new List<ProductAttribute>
                {
                    new ProductAttribute { id = "BRAND", value_name = "Marca del producto" },
                    new ProductAttribute { id = "EAN", value_name = "7898095297749" }
                }
            };

            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("https://api.mercadolibre.com/items", content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Produto criado:\n" + result);
            }
            else
            {
                Console.WriteLine($"Erro: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
            }
        }

        static async Task GetProduto(string itemId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"https://api.mercadolibre.com/items/{itemId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Produto:\n" + content);
            }
            else
            {
                Console.WriteLine($"Erro GET: {response.StatusCode}");
            }
        }
    }
}
