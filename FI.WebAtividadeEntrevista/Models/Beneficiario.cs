using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }
        
        [Required]
        [CpfValido(ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }

        [Required]
        public string Nome { get; set; }
    }    
}