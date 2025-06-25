using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text.Json;
using WebAppML.Bll;
using WebAppML.Entity;

namespace WebAppML.Controllers
{
    public class OAuthController : Controller
    {
        private readonly ILogger<OAuthController> logger;
        private readonly AplicacaoBll aplicacaoBll;
        private readonly HttpClient client;

        public OAuthController(ILogger<OAuthController> logger, AplicacaoBll aplicacaoBll, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.aplicacaoBll = aplicacaoBll;
            this.client = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult AutorizarMercadoLivre(Aplicacao app)
        {
            TempData["Aplicacao"] = Newtonsoft.Json.JsonConvert.SerializeObject(app);
            string authUrl = $"https://auth.mercadolivre.com.br/authorization" +
                             $"?response_type=code" +
                             $"&client_id={app.AppId}" +
                             $"&redirect_uri={app.RedirectURI}"; //"https://localhost:7050/OAuth/Callback"; // ou a URL de produção/homologação

            return Redirect(authUrl);
        }

        [HttpGet("/OAuth/Callback")]
        public async Task<IActionResult> Callback(string code) // Aqui obtém o code
        {
            var json = TempData["Aplicacao"] as string;
            var app = Newtonsoft.Json.JsonConvert.DeserializeObject<Aplicacao>(json);
            app = aplicacaoBll.PesquisarAplicacao(app);
            app.Codigo = code;

            if (string.IsNullOrEmpty(code))
                return BadRequest("Authorization code não fornecido.");

            aplicacaoBll.AtualizarAplicacao(app);
            HttpContext.Session.SetString("NomeAplicacao", app.Nome);
            HttpContext.Session.SetString("AppId", app.AppId);

            await ObterTokens(app);
            return RedirectToAction("MenuProduto", "ProdutoML");
        }

        private async Task ObterTokens(Aplicacao app)
        {
            try
            {
                Dictionary<string, string> headers;
                if (app.Token == null || app.Token.RefreshToken == null)
                {
                    headers = new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_code" },
                        { "client_id", app.AppId },
                        { "client_secret", app.ChaveSecreta },
                        { "code", app.Codigo },
                        { "redirect_uri", app.RedirectURI }
                    };
                    app.Token = new Token();
                }
                else
                {
                    headers = new Dictionary<string, string>
                    {
                        { "grant_type", "refresh_token" },
                        { "client_id", app.AppId },
                        { "client_secret", app.ChaveSecreta },
                        { "refresh_token", app.Token.RefreshToken }
                    };
                }

                var response = await client.PostAsync(URLsML.AutorizacaoTokenURL, new FormUrlEncodedContent(headers));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Token token = JsonSerializer.Deserialize<Token>(json);

                    app.Token = token;

                    // Validar se já tem UserID (com os novos Tokens) e atualizar o registro do usuário caso não
                    if(app.IdUsuario == 0)
                    {
                        var getIdUsuario = await client.GetAsync($"https://api.mercadolibre.com/users/me?access_token={app.Token.AccessToken}");
                        var resposta = await getIdUsuario.Content.ReadAsStringAsync();
                        // Converte para JSON e extrai apenas o campo "id"
                        app.IdUsuario = JObject.Parse(resposta).Value<long>("id");
                    }

                    aplicacaoBll.AtualizarAplicacao(app);
                }
                else
                {
                    Console.WriteLine($"Erro ao obter token: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
