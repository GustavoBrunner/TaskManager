using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Settings;

namespace TaskManager.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public ProjectRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ProjectModel entity)
        {
            await _dbContext.Projects.AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(ProjectModel entity)
        {
            _dbContext.Projects.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(ProjectModel entity)
        {
            _dbContext.Projects.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ProjectModel>> GetAll()
        {
            var projects = await _dbContext.Projects
                .Include(p => p.Tasks)
                .OrderBy(p => p.Id).AsNoTracking()
                .ToListAsync();
            return projects;
        }

        public async Task<ProjectModel> GetById(string id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            return project;
        }

        public bool CheckIfExistsById(string id)
        {
            return _dbContext.Projects.Any(p => p.Id == id);
        }
    }
}