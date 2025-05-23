using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MercadoLivreAPI;
using MercadoLivreAPI.Models;

namespace API_ML
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            MLAPI api = new MLAPI();
            //await api.ObterCodigoAutorizacao();
            Console.WriteLine("");
            //await api.ObterTokenAcesso();
            Console.WriteLine("");
            //await api.CriarUsuarioTeste();
            Console.WriteLine("");
            //await api.ObterTokenAcessoUsuario();
            Console.WriteLine("");
            //await api.GetProduto("MLB1828680414");
            Console.WriteLine("");
            await api.CriarProduto();
        }

    }
}
