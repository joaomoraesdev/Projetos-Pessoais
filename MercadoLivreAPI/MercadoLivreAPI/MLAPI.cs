using MercadoLivreAPI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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
                "TG-6837949f53334100017c863e-209627840",
                "https://www.google.com.br");
            tokenResponse = new TokenResponse();
            tokenResponse.RefreshToken = "TG-683764e753334100017a4e89-209627840";
            tokenResponse.AccessToken = "APP_USR-7088454564644269-052815-da5b41c7383a7b9aaf8369c0289a8564-209627840";
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
                await SalvarToken();

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
                    new SaleTerm { id = "WARRANTY_TYPE", value_id = "2230279" }, // Garantia do vendedor
                    new SaleTerm { id = "WARRANTY_TIME", value_name = "90 dias" }
                },
                pictures = new List<Picture>
                {
                    new Picture { source = "http://mla-s2-p.mlstatic.com/968521-MLA20805195516_072016-O.jpg" }
                },
                attributes = new List<ProductAttribute>
                {
                    new ProductAttribute { id = "BRAND", value_name = "Marca do produto" },
                    new ProductAttribute { id = "EAN", value_name = "7898095297749" },
                    new ProductAttribute { id = "MODEL", value_name = "Modelo Teste 01" }
                }
            };


            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(product, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var response = await client.PostAsync("https://api.mercadolibre.com/items", content);
            var resultJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Produto criado com sucesso!");
                Console.WriteLine(resultJson);

                // Captura o ID do produto para poder usá-lo depois
                var createdItem = JsonSerializer.Deserialize<JsonElement>(resultJson);
                string itemId = createdItem.GetProperty("id").GetString();

                Console.WriteLine($"🔍 Consultando produto criado: {itemId}");
                await GetProduto(itemId);
            }
            else
            {
                Console.WriteLine($"❌ Erro ao criar produto: {response.StatusCode}\n{resultJson}");
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
                { "username", "TESTUSER1423718710" },
                { "password", "5uEiJUbgl6" }
            };

            var content = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync("https://api.mercadolibre.com/oauth/token", content);
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);
            var token = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> CarregarTokenSalvo()
        {
            string caminho = "token.json";
            if (File.Exists(caminho))
            {
                string json = await File.ReadAllTextAsync(caminho);
                tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
                return true;
            }
            return false;
        }

        public async Task SalvarToken()
        {
            string json = JsonSerializer.Serialize(tokenResponse);
            await File.WriteAllTextAsync("token.json", json);
        }

        public async Task InicializarTokenAutomatico()
        {
            bool carregado = await CarregarTokenSalvo();

            if (!carregado || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                Console.WriteLine("⚠️ Token não encontrado ou inválido. Tentando refresh...");

                try
                {
                    await ObterTokenAcesso();
                }
                catch
                {
                    Console.WriteLine("❌ Erro ao tentar usar refresh token. Obtendo novo código de autorização...");
                    await ObterCodigoAutorizacao();
                }
            }
        }

    }
}
