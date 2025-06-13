using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using WebAppML.Bll;

namespace WebAppML.Controllers
{
    public class ProdutoMLController : Controller
    {
        private readonly ILogger<ProdutoMLController> _logger;
        private readonly AplicacaoBll aplicacaoBll;

        public ProdutoMLController(ILogger<ProdutoMLController> logger, AplicacaoBll _appBll)
        {
            _logger = logger;
            aplicacaoBll = _appBll;
        }

        public IActionResult MenuProduto()
        {
            ViewBag.NomeAplicacao = TempData["NomeAplicacao"];
            return View("~/Views/ML/MenuProduto.cshtml");
        }
    }
}
