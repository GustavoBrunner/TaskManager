using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    [Table("Task")]
    public class TaskModel
    {
        [Key]
        [Column(TypeName ="varchar(255)")]
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

        public bool IsActive { get; set; }
        public bool IsFinished { get; set; }
        public ProjectModel? Project { get; set; }

        public List<UserModel>? Resposibles { get; set; }
        
    }
}