using API_ML;
using MercadoLivreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MercadoLivreAPI
{
    public class MLAPI
    {
        HttpClient client;
        DadosContaML dadosContaML;
        TokenResponse tokenResponse;
        public MLAPI()
        {
            client = new HttpClient();
            dadosContaML = new DadosContaML("7088454564644269",
                "6UwgH0x3oVS6vmNJE9ARzwGX0hCHNYQU",
                "TG-6830b393e4158c000193a82e-209627840",
                "https://www.google.com.br");
            tokenResponse = new TokenResponse();
            tokenResponse.RefreshToken = "TG-68309a22e47c3400010bef9f-209627840";
            tokenResponse.AccessToken = "APP_USR-7088454564644269-052313-69bf49aba79448ca6de7d7c6fcd5242c-209627840";
        }

        //const string app_id = "7088454564644269";
        //const string chave_secreta = "6UwgH0x3oVS6vmNJE9ARzwGX0hCHNYQU"; //chave secreta
        //const string codigo = "TG-68309a0872f4b800017146bf-209627840"; //code
        //const string uri_redirect = "https://www.google.com.br";

        // URL principal para obter token
        const string url_principal = "https://api.mercadolibre.com/oauth/token";

        public async Task ObterCodigoAutorizacao()
        {
            var headers = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", dadosContaML.AppId },
                { "client_secret", dadosContaML.ChaveSecreta },
                { "code", dadosContaML.Codigo },
                { "redirect_uri", dadosContaML.RedirecionamentoURI }
            };

            var content = new FormUrlEncodedContent(headers);

            var response = await client.PostAsync(url_principal, content);

            if (response.IsSuccessStatusCode)
            {
                //para pegar os campos individualmente tem que desserilizar o JSON e atribuir em alguma propriedade
                var json = await response.Content.ReadAsStringAsync();
                tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
                Console.WriteLine("Access Token: " + tokenResponse.AccessToken);
                Console.WriteLine("Refresh Token: " + tokenResponse.RefreshToken);
                Console.WriteLine("Token obtido:\n" + json);
            }
            else
            {
                Console.WriteLine($"Erro ao obter token: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task ObterTokenAcesso()
        {
            var values = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", dadosContaML.AppId },
                { "client_secret", dadosContaML.ChaveSecreta },
                { "refresh_token", tokenResponse.RefreshToken }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(url_principal, content);

            if (response.IsSuccessStatusCode)
            {
                //para pegar os campos individualmente tem que desserilizar o JSON e atribuir em alguma propriedade
                var json = await response.Content.ReadAsStringAsync();
                var respostaToken = JsonSerializer.Deserialize<TokenResponse>(json);

                tokenResponse.AccessToken = respostaToken.AccessToken;
                Console.WriteLine("Access Token: " + tokenResponse.AccessToken);
                Console.WriteLine("Refresh Token: " + tokenResponse.RefreshToken);

                Console.WriteLine("Token obtido:\n" + json);
            }
            else
            {
                Console.WriteLine($"Erro ao obter token: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task CriarProduto()
        {
            var product = new Product
            {
                title = "Item de test - No Ofertar",
                category_id = "MLB3530",
                price = 350,
                currency_id = "BRL",
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

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(product, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var response = await client.PostAsync("https://api.mercadolibre.com/items", content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("✅ Produto criado:\n" + result);
            }
            else
            {
                Console.WriteLine($"❌ Erro: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
            }
        }


        public async Task GetProduto(string itemId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
            var response = await client.GetAsync($"https://api.mercadolibre.com/items/{itemId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Produto:\n" + content);
            }
            else
            {
                Console.WriteLine($"Erro ao obter token: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task CriarUsuarioTeste()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mercadolibre.com/users/test_user");
            request.Content = new StringContent("{\"site_id\": \"MLB\"}", Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var usuarioTeste = JsonSerializer.Deserialize<UsuarioTeste>(json);
                Console.WriteLine($"Usuário de teste criado: {usuarioTeste.Usuario} - Senha: {usuarioTeste.Senha}\n{json}");
            }
            else
            {
                Console.WriteLine($"Erro ao criar usuário de teste: {response.StatusCode}\n{json}");
            }
        }

        public async Task ObterTokenAcessoUsuario()
        {
            //versão teste
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", dadosContaML.AppId },
                { "client_secret", dadosContaML.ChaveSecreta },
                { "username", "TESTUSER1294981658" },
                { "password", "Ollcfa7hkE" }
            };

            var content = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync("https://api.mercadolibre.com/oauth/token", content);
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);
            var token = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
