using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Net.Http.Headers;
using WebAppML.Bll;
using WebAppML.Entity;

namespace WebAppML.Controllers
{
    public class ProdutoMLController : Controller
    {
        private readonly ILogger<ProdutoMLController> logger;
        private readonly AplicacaoBll aplicacaoBll;
        private readonly HttpClient client;

        public ProdutoMLController(ILogger<ProdutoMLController> logger, AplicacaoBll aplicacaoBll, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.aplicacaoBll = aplicacaoBll;
            this.client = httpClientFactory.CreateClient();
        }

        public IActionResult MenuProduto()
        {
            ViewBag.NomeAplicacao = HttpContext.Session.GetString("NomeAplicacao");
            return View("~/Views/ML/MenuProduto.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> PesquisarTodosProdutos() //Implementar revalidação de token
        {
            try
            {
                string appId = HttpContext.Session.GetString("AppId");
                Aplicacao app = aplicacaoBll.PesquisarAplicacao(new Aplicacao() { AppId = appId });
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", app.Token.AccessToken);

                HttpResponseMessage response = await client.GetAsync($"https://api.mercadolibre.com/users/{app.IdUsuario}/items/search");
                string conteudo = "";
                bool status = false;

                if (response.IsSuccessStatusCode)
                {
                    conteudo = await response.Content.ReadAsStringAsync();
                    status = true;
                }
                return Json(new { message = conteudo, success = status });
            }
            catch (Exception e)
            {
                throw new Exception("Erro: ", e);
            }
        }

        [HttpGet]
        public async Task<JsonResult> PesquisarProduto(string idItem) // Implementar revalidação de token
        {
            try
            {
                string appId = HttpContext.Session.GetString("AppId");
                Aplicacao app = aplicacaoBll.PesquisarAplicacao(new Aplicacao() { AppId = appId });
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", app.Token.AccessToken);

                HttpResponseMessage response = await client.GetAsync($"https://api.mercadolibre.com/items/{idItem}");
                string conteudo = "";
                bool status = false;

                if (response.IsSuccessStatusCode)
                {
                    conteudo = await response.Content.ReadAsStringAsync();
                    status = true;
                }
                return Json(new { success = status, message = conteudo });
            }
            catch (Exception e)
            {
                throw new Exception("Erro: ", e);
            }

        }
    }
}
