using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        public string? Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Lembrar de mim")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}