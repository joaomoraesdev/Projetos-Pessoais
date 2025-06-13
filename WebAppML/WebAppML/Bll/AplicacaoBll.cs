using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Collections;
using System.Text.Json;
using System.Threading.Tasks;
using WebAppML.Entity;

namespace WebAppML.Bll
{
    public class AplicacaoBll
    {
        private readonly IMongoCollection<Aplicacao> database;
        private readonly HttpClient client;

        public AplicacaoBll(IMongoCollection<Aplicacao> _database)
        {
            database = _database;
            client = new HttpClient();
        }

        public Aplicacao PesquisarAplicacao(Aplicacao app)
        {
            try
            {
                Aplicacao aplicacao = new Aplicacao();
                aplicacao = database.Find(u => u.AppId == app.AppId).FirstOrDefault();
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
                var aplicacao = database.Find(u => u.AppId == app.AppId).FirstOrDefault();
                if (aplicacao == null)
                {
                    database.InsertOne(app);
                    retorno.Mensagem = "Aplicação criada com sucesso";
                    retorno.Status = true;
                }
                else
                {
                    retorno.Mensagem = "Aplicação já existente";
                    retorno.Status = false;
                }
                return retorno;
            }
            catch
            {
                throw new ArgumentException("Não foi possível criar o perfil da aplicação");
            }

        }

        public void AtualizarAplicacao(Aplicacao app)
        {
            try
            {
                database.ReplaceOne(u => u.Id == app.Id, app);
            }
            catch
            {
                throw new ArgumentException("Não foi possível atualizar a aplicação");
            }
        }

        public void DeletarAplicacao(string id)
        {
            try
            {
                database.DeleteOne(u => u.Id == id);
            }
            catch
            {
                throw new ArgumentException("Não foi possível atualizar a aplicação");
            }
        }
    }
}
