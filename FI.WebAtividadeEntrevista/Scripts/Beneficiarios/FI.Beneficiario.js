ListaBeneficiarios = [];

$(document).ready(function () {
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
});

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