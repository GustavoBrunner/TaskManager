using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
    public class UserModel : IdentityUser
    {
        public string? Cpf { get; set; }  

        public string? FullName { get; set; }

        public DateTime Birth { get; set; }

        public ProjectModel? Project { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public string? ActiveTaskId { get; set; }

        public List<TaskModel>? Tasks { get; set; }

        [NotMapped]
        public TaskModel? ActiveTask { get; set; }

        [NotMapped]
        public int Age { get => (int)Math.Floor((DateTime.Now - Birth).TotalDays / 365.25); }
    }
}