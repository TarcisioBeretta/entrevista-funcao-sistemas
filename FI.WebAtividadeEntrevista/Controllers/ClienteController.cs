using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private BoCliente _BoCliente;
        private BoBeneficiario _BoBeneficiario;

        public ClienteController()
        {
            _BoCliente = new BoCliente();
            _BoBeneficiario = new BoBeneficiario();
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                try
                {
                    IncluirOuAtualizarCliente(model);
                    return Json("Cadastro efetuado com sucesso");
                }
                catch (Exception e)
                {
                    Response.StatusCode = 400;
                    return Json(e.Message);
                }
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                try
                {
                    IncluirOuAtualizarCliente(model);
                    return Json("Cadastro alterado com sucesso");
                }
                catch (Exception e)
                {
                    Response.StatusCode = 400;
                    return Json(e.Message);
                }
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            Cliente cliente = _BoCliente.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = ConsultarListaBeneficiarios(cliente.Id)
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private void IncluirOuAtualizarCliente(ClienteModel clienteModel)
        {
            ValidarCliente(clienteModel);
            clienteModel.Id = SalvarCliente(clienteModel);
            AtualizarBeneficiarios(clienteModel);
        }

        private long SalvarCliente(ClienteModel clienteModel)
        {
            var cliente = new Cliente()
            {
                CEP = clienteModel.CEP,
                Cidade = clienteModel.Cidade,
                Email = clienteModel.Email,
                Estado = clienteModel.Estado,
                Logradouro = clienteModel.Logradouro,
                Nacionalidade = clienteModel.Nacionalidade,
                Nome = clienteModel.Nome,
                Sobrenome = clienteModel.Sobrenome,
                Telefone = clienteModel.Telefone,
                CPF = clienteModel.CPF
            };

            if (clienteModel.Id > 0)
            {
                cliente.Id = clienteModel.Id;
                _BoCliente.Alterar(cliente);
                return clienteModel.Id;
            }
            else
            {
                return _BoCliente.Incluir(cliente);
            }
        }

        private void ValidarCliente(ClienteModel clienteModel)
        {
            if (clienteModel.Id > 0)
            {
                var cliente = _BoCliente.Consultar(clienteModel.Id);
                if (cliente.CPF == clienteModel.CPF)
                {
                    return;
                }
            }

            if (_BoCliente.VerificarExistencia(clienteModel.CPF))
            {
                throw new Exception(string.Format("CPF: {0} já está em uso por outro cliente", clienteModel.CPF));
            }
        }

        private void AtualizarBeneficiarios(ClienteModel clienteModel)
        {
            RemoverBeneficiariosDoCliente(clienteModel);
            InserirBeneficiariosDoCliente(clienteModel);
        }

        private void InserirBeneficiariosDoCliente(ClienteModel clienteModel)
        {
            if(clienteModel.Beneficiarios == null)
            {
                return;
            }

            foreach (BeneficiarioModel beneficiarioModel in clienteModel.Beneficiarios)
            {
                if (_BoBeneficiario.VerificarExistencia(clienteModel.Id, beneficiarioModel.CPF))
                {
                    throw new Exception(string.Format("CPF: {0} já está em uso por outro beneficiário", beneficiarioModel.CPF));
                }
                else
                {
                    _BoBeneficiario.Incluir(new Beneficiario()
                    {
                        IdCliente = clienteModel.Id,
                        CPF = beneficiarioModel.CPF,
                        Nome = beneficiarioModel.Nome,
                    });
                }
            }
        }

        private void RemoverBeneficiariosDoCliente(ClienteModel clienteModel)
        {
            var beneficiarios = _BoBeneficiario.Listar(clienteModel.Id);
            foreach (Beneficiario beneficiario in beneficiarios)
            {
                _BoBeneficiario.Excluir(beneficiario.Id);
            }
        }


        private List<BeneficiarioModel> ConsultarListaBeneficiarios(long idCliente)
        {
            var beneficiarios = _BoBeneficiario.Listar(idCliente);
            var beneficiariosModel = new List<BeneficiarioModel>();

            foreach (Beneficiario beneficiario in beneficiarios)
            {
                beneficiariosModel.Add(new BeneficiarioModel()
                {
                    Id = beneficiario.Id,
                    CPF = beneficiario.CPF,
                    Nome = beneficiario.Nome
                });
            }

            return beneficiariosModel;
        }
    }
}