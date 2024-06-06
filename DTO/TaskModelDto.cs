using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.ViewModel
{
    public class TaskModelDto
    {
        public TaskModelDto(){
            this.Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        [Display(Name = "Nome da tarefa")]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(100)]
        [MinLength(1, ErrorMessage = "O campo {0} precisa de preenchimento!")]
        [MaxLength(100, ErrorMessage = "Tamanho limite de {1} caracteres")]
        public string? TaskName { get; set; }

        [Display(Name = "Descrição da tarefa")]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(255)]
        [MinLength(1, ErrorMessage = "O campo {0} precisa de preenchimento!")]
        [MaxLength(255, ErrorMessage = "Tamanho limite de {1} caracteres")]
        public string? TaskDescription { get; set; }

        [Required(ErrorMessage = "É necessário informar um projeto")]
        public string? ProjectId { get; set; }

        public string? ResponsibleId { get; set; }
        public bool IsActive { get; set; }
        
        public bool IsFinished { get; set; }
    }
}