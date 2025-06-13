using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using WebAppML.Bll;
using WebAppML.Entity;
using Newtonsoft.Json;

namespace WebAppML.Controllers
{
    public class OAuthController : Controller
    {
        private readonly ILogger<OAuthController> _logger;
        private readonly AplicacaoBll aplicacaoBll;

        public OAuthController(ILogger<OAuthController> logger, AplicacaoBll _appBll)
        {
            _logger = logger;
            aplicacaoBll = _appBll;
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
        public IActionResult Callback(string code)
        {
            var json = TempData["Aplicacao"] as string;
            var app = Newtonsoft.Json.JsonConvert.DeserializeObject<Aplicacao>(json);
            app = aplicacaoBll.PesquisarAplicacao(app);
            app.Codigo = code;

            if (string.IsNullOrEmpty(code))
                return BadRequest("Authorization code não fornecido.");

            aplicacaoBll.AtualizarAplicacao(app);
            TempData["NomeAplicacao"] = app.Nome;

            // Implementar o restante do acesso ao mercado livre #access token e só ai logar na plataforma e redirecionar!s

            return RedirectToAction("MenuProduto", "ProdutoML");
        }
    }
}
