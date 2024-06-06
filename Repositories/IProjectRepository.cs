using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface IProjectRepository : IBaseRepository<ProjectModel>
    {
    }
}