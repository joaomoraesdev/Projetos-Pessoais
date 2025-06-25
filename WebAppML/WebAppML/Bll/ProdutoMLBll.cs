using MongoDB.Driver;
using System.Net.Http.Headers;
using WebAppML.Entity;

namespace WebAppML.Bll
{
    public class ProdutoMLBll // DELETAR!!!!!!!!!!!
    {
        private readonly HttpClient client;

        public ProdutoMLBll()
        {
            client = new HttpClient();
        }

        public Aplicacao PesquisarAplicacao(Aplicacao app)
        {
            try
            {
                Aplicacao aplicacao = new Aplicacao();
                //aplicacao = database.Find(u => u.AppId == app.AppId).FirstOrDefault();
                return aplicacao;
            }
            catch
            {
                throw new ArgumentException("Aplicação não identificada");
            }
        }

        public RetornoAcao CriarAplicacao(Aplicacao app)
        {
            RetornoAcao retorno = new RetornoAcao();
            try
            {
                //var aplicacao = database.Find(u => u.AppId == app.AppId).FirstOrDefault();
                //if (aplicacao == null)
                //{
                //    database.InsertOne(app);
                //    retorno.Mensagem = "Aplicação criada com sucesso";
                //    retorno.Status = true;
                //}
                //else
                //{
                //    retorno.Mensagem = "Aplicação já existente";
                //    retorno.Status = false;
                //}
                return retorno;
            }
            catch
            {
                throw new ArgumentException("Não foi possível criar o perfil da aplicação");
            }

        }
    }
}
