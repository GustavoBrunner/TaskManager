using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface ITaskRepository : IBaseRepository<TaskModel>
    {
        Task<TaskModel> FindTaskIncludeUser(string id);

        Task<TaskModel> FindTaskIncludeProject(string id);

        Task<TaskModel> TaskInformations(string id);

        Task<IEnumerable<TaskModel>> GetAllTasksIncludeProject();
    }
}