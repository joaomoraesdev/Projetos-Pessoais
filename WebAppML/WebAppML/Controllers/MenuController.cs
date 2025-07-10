using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppML.Bll;
using WebAppML.Entity;
using WebAppML.Models;

namespace WebAppML.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly AplicacaoBll aplicacaoBll;

        public MenuController(ILogger<MenuController> logger, AplicacaoBll _appBll)
        {
            _logger = logger;
            aplicacaoBll = _appBll;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult LoginAplicacao(string login, string senha)
        {
            try
            {
                var app = aplicacaoBll.PesquisarAplicacao(new Aplicacao { AppId = login });

                if (app.Senha == senha)
                {
                    // Redireciona para o método na controller de OAuth com os dados
                    return RedirectToAction("AutorizarMercadoLivre", "OAuth", new Aplicacao { Id = app.Id, AppId = app.AppId, RedirectURI = app.RedirectURI });
                }
                else
                {
                    ViewBag.Mensagem = "Acesso negado.";
                    return View("Login"); // ou a view que você estiver usando
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IActionResult Cadastro()
        {
            return View("Cadastro");
        }

        [HttpPost]
        public JsonResult CadastroAplicacao(Aplicacao app)
        {
            try
            {
                RetornoAcao retorno = aplicacaoBll.CriarAplicacao(app);
                return Json(new { success = retorno.Status, message = retorno.Mensagem });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
