using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DTO
{
    public class UserModelDto
    {
        public UserModelDto(){
            this.ConcurrencyStamp = Guid.NewGuid().ToString();
        }
        
        public string? Id { get; set; }
        [Display(Name = "Nome Completo")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(130)]
        public string? FullName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(255)]
        public string? Email { get; set; }

        [Display(Name = "CPF")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(11)]
        [MinLength(11, ErrorMessage = "Tamanho mínimo de 11 letras")]
        [MaxLength(11, ErrorMessage = "Tamanho máximo de 11 letras")]
        public string? Cpf { get; set; }

        [Display(Name = "CPF")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(11)]
        [MinLength(11, ErrorMessage = "Tamanho mínimo de 11 letras")]
        [MaxLength(11, ErrorMessage = "Tamanho máximo de 11 letras")]
        public string? PhoneNumber { get; set; }
        public string ConcurrencyStamp { get; set; }

        [Display(Name ="Data de Nascimento")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        public DateTime Birth { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(16)]
        [MinLength(6,ErrorMessage = "Tamanho mínimo de 6 caracteres")]
        [MaxLength(16, ErrorMessage = "Tamanho máximo de 16 caracteres")]
        public string? Password { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(16)]
        [MinLength(6, ErrorMessage = "Tamanho mínimo de 6 caracteres")]
        [MaxLength(16,ErrorMessage = "Tamanho máximo de 16 caracteres")]
        [Compare(nameof(Password), ErrorMessage = "Senha e confirmação diferentes")]
        public string? ConfPassword { get; set; }


  
    }
}