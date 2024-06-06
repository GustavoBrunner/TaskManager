using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.DTO
{
    public class ProjectModelDto
    {
        public ProjectModelDto()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        [Display(Name = "Nome do Projeto")]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(100)]
        [MinLength(1, ErrorMessage = "O campo {0} precisa de preenchimento!")]
        [MaxLength(100, ErrorMessage = "Tamanho limite de {1} caracteres")]
        public string? ProjectName { get; set; }

        [Display(Name = "Descrição do projeto")]
        [Required(ErrorMessage = "O campo {0} é necessário!")]
        [StringLength(255)]
        [MinLength(1, ErrorMessage = "O campo Descrição precisa de preenchimento")]
        [MaxLength(255, ErrorMessage = "Tamanho limite de {1} caracteres")]
        public string? ProjectDescription { get; set; }

        public List<TaskModel>? Tasks { get; set; }    
        
    }
}