using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoLivreAPI.Models
{
    public class DadosContaML
    {
        public string AppId { get; set; }
        public string ChaveSecreta { get; set; }
        public string Codigo { get; set; }
        public string RedirecionamentoURI { get; set; } 

        public DadosContaML(string appId, string chaveSecreta, string codigo, string redirecionamentoURI)
        {
            AppId = appId;
            ChaveSecreta = chaveSecreta;
            Codigo = codigo;
            RedirecionamentoURI = redirecionamentoURI;
        }
    }
}
