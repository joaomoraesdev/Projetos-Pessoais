const base_path = window.location.origin;

function menu() {
    var dto = {
        // Tela de Login
        txtLogin: $('#txtLogin').val(),
        txtSenha: $('#txtSenha').val(),

        // Tela de Cadastro
        txtNomeAppCadastro: $("#txtNomeAppCadastro").val(),
        txtIdAppCadastro: $('#txtIdAppCadastro').val(),
        txtSenhaCadastro: $('#txtSenhaCadastro').val(),
        txtConfSenhaCadastro: $('#txtConfSenhaCadastro').val(),
        txtChaveSecreta: $('#txtChaveSecreta').val(),
        txtRedirectURI: $('#txtRedirectURI').val(),
    };

    function cadastroAplicacao(event) {
        event.preventDefault();
        if (validaCamposCadastro()) {
            $.ajax({
                url: base_path + "/Menu/CadastroAplicacao",
                type: 'POST',
                data: {
                    Nome: dto.txtNomeAppCadastro,
                    AppId: dto.txtIdAppCadastro,
                    Senha: dto.txtSenhaCadastro,
                    ChaveSecreta: dto.txtChaveSecreta,
                    RedirectURI: dto.txtRedirectURI
                },
                dataType: 'json',
                success: function (data) {
                    if (data.success)
                        window.location.href = base_path + "/Menu/Index";
                    alert(data.message);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });

        }
    }

    function validaCamposCadastro() {
        const senha = dto.txtSenhaCadastro;
        const confSenha = dto.txtConfSenhaCadastro;
        const redirectURI = dto.txtRedirectURI;

        // Regex para senha segura:
        // - Min. 8 caracteres
        // - Pelo menos 1 maiúscula
        // - Pelo menos 1 minúscula
        // - Pelo menos 1 número
        // - Pelo menos 1 caractere especial
        // Senha teste: @Teste123
        const senhaSeguraRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;

        // Regex básica de URL válida
        const urlRegex = /^(https?:\/\/)?([\w\-]+(\.[\w\-]+)+)([\w\-\._~:/?#[\]@!$&'()*+,;=]*)$/;

        // Verificações principais
        const camposPreenchidos = dto.txtIdAppCadastro && senha && confSenha && dto.txtChaveSecreta && redirectURI;

        if (!camposPreenchidos) {
            alert("Todos os campos são obrigatórios.");
            return false;
        }

        if (senha !== confSenha) {
            alert("As senhas não coincidem.");
            return false;
        }

        if (!senhaSeguraRegex.test(senha)) {
            alert("A senha deve conter ao menos 8 caracteres, com letra maiúscula, minúscula, número e caractere especial.");
            return false;
        }

        if (!urlRegex.test(redirectURI)) {
            alert("A URI de redirecionamento deve ser uma URL válida (ex: https://seusite.com/callback).");
            return false;
        }

        return true;
    }


    return {
        loginAplicacao: loginAplicacao,
        cadastroAplicacao: cadastroAplicacao,
        autenticacaoAplicacao: autenticacaoAplicacao
    }
}

// Garante que tudo está pronto para usar após o carregamento da página
$(document).ready(function () {
    $('#btnCadastrar').on('click', function (e) {
        menu().cadastroAplicacao(e);
    });
});
