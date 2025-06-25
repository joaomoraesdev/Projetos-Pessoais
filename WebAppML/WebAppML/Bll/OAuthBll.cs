using System.Text.Json;
using WebAppML.Entity;

namespace WebAppML.Bll
{
    public class OAuthBll // DELETAR!!!!!!!
    {
        public async Task<RetornoAcao> AutenticacaoAplicacao(Aplicacao app)
        {
            RetornoAcao retorno = new RetornoAcao();
            await ObterCodigo(app);
            await ObterTokenAcesso(app);
            return retorno;
        }

        private async Task ObterCodigo(Aplicacao app)
        {

        }

        private async Task ObterTokenAcesso(Aplicacao app) // Aqui obtém-se o code
        {
            //var headers = new Dictionary<string, string>
            //{
            //    { "grant_type", "authorization_code" },
            //    { "client_id", app.AppId },
            //    { "client_secret", app.ChaveSecreta },
            //    { "code", app.Codigo },
            //    { "redirect_uri", app.RedirectURI }
            //};

            //var content = new FormUrlEncodedContent(headers);

            //var response = await client.PostAsync(URLsML.AutorizacaoTokenURL, content);

            //if (response.IsSuccessStatusCode)
            //{
            //    //para pegar os campos individualmente tem que desserilizar o JSON e atribuir em alguma propriedade
            //    var json = await response.Content.ReadAsStringAsync();
            //    tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
            //    Console.WriteLine("Access Token: " + tokenResponse.AccessToken);
            //    Console.WriteLine("Refresh Token: " + tokenResponse.RefreshToken);
            //    Console.WriteLine("Token obtido:\n" + json);
            //}
            //else
            //{
            //    Console.WriteLine($"Erro ao obter token: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
            //}
        }
    }
}
