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

            Console.WriteLine("🔐 Inicializando autenticação...");
            await api.InicializarTokenAutomatico();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("== MERCADO LIVRE API ==");
                Console.WriteLine("1 - Criar Produto");
                Console.WriteLine("2 - Consultar Produto");
                Console.WriteLine("3 - Criar Usuário de Teste");
                Console.WriteLine("0 - Sair");
                Console.Write("\nEscolha uma opção: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await api.CriarProduto();
                        break;
                    case "2":
                        Console.Write("ID do produto: ");
                        await api.GetProduto(Console.ReadLine());
                        break;
                    case "3":
                        await api.CriarUsuarioTeste();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }
        }

    }
}
