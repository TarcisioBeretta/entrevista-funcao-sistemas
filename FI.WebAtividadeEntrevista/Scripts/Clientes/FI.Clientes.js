ListaBeneficiarios = [];

$(document).ready(function () {
    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "CPF": $(this).find("#CPF").val(),
                "Beneficiarios": ListaBeneficiarios
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });
    });

    $('#formBeneficiario').submit(function (e) {
        e.preventDefault();

        inserirBeneficiarios({
            "CPF": $(this).find("#CPFBeneficiario").val(),
            "Nome": $(this).find("#NomeBeneficiario").val()
        });

        $("#formBeneficiario")[0].reset();
    });

    $('#btnBeneficiarios').click(function () {
        $('#modalBeneficiario').modal('show');
    });
})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}


function atualizarListaBeneficiarios(beneficiarios) {
    ListaBeneficiarios = beneficiarios;
    renderizarBeneficiarios();
}

function inserirBeneficiarios(beneficiario) {
    ListaBeneficiarios.push(beneficiario);
    renderizarBeneficiarios();
}

function alterarBeneficiario(beneficiarioIdx) {
    var beneficiario = ListaBeneficiarios[beneficiarioIdx];
    $('#formBeneficiario').find("#CPFBeneficiario").val(beneficiario.CPF);
    $('#formBeneficiario').find("#NomeBeneficiario").val(beneficiario.Nome);
    excluirBeneficiario(beneficiarioIdx);
}

function excluirBeneficiario(beneficiarioIdx) {
    ListaBeneficiarios.splice(beneficiarioIdx, 1);
    renderizarBeneficiarios();
}

function renderizarBeneficiarios() {
    var html = '';

    for (idx = 0; idx < ListaBeneficiarios.length; idx++) {
        html += renderizarBeneficiario(ListaBeneficiarios[idx], idx);
    }

    $('#formBeneficiario #grid tbody').html('');
    $('#formBeneficiario #grid tbody').append(html);
}

function renderizarBeneficiario(beneficiario, idx) {
    return '' +
        '<tr>' +
        '   <td>' + beneficiario.CPF + '</td>' +
        '   <td>' + beneficiario.Nome + '</td>' +
        '   <td>' +
        '       <button type="button" class="btn btn-primary pull-right" data-dismiss="modal" onclick="excluirBeneficiario(' + idx + ')">Excluir</button>' +
        '       <button type="button" class="btn btn-primary pull-right" data-dismiss="modal" onclick="alterarBeneficiario(' + idx + ')" style="margin-right: 5px;">Alterar</button>' +
        '   </td>' +
        '</tr>';
}