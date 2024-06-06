using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Settings;

namespace TaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _dbContext ;

        public TaskRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(TaskModel entity)
        {
            await _dbContext.Tasks.AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(TaskModel entity)
        {
            _dbContext.Tasks.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(TaskModel entity)
        {
            _dbContext.Tasks.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<TaskModel>> GetAll()
        {
            var tasks = await _dbContext.Tasks
                .Include(t => t.Resposibles)
                .OrderBy(t => t.TaskName)
                .AsNoTracking()
                .ToListAsync();
            return tasks;
        }

        public async Task<TaskModel> GetById(string id)
        {
            var task = await _dbContext.Tasks
                .FindAsync(id);
            return task;
        }

        public bool CheckIfExistsById(string id)
        {
            return _dbContext.Tasks.Any(t => t.Id == id);
        }

        public async Task<TaskModel> FindTaskIncludeUser(string id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Resposibles)
                .FirstOrDefaultAsync(t => t.Id == id);
            return task;
        }

        public async Task<TaskModel> FindTaskIncludeProject(string id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);
            return task;
        }

        public async Task<TaskModel> TaskInformations(string id){
            var infos = await _dbContext.Tasks
                .Include(t => t.Resposibles)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);
            return infos;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksIncludeProject()
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Project)
                .Include(t => t.Resposibles)
                .AsNoTracking().ToListAsync();
            return task;
        }
    }
}