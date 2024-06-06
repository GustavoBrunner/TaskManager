using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Settings;

namespace TaskManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<UserModel> _dbContext;

        public UserRepository(UserManager<UserModel> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(UserModel entity)
        {
            var result = await _dbContext.CreateAsync(entity);
            return result.Succeeded;
        }

        public async Task<bool> Delete(UserModel entity)
        {
            var result = await _dbContext.DeleteAsync(entity);
            return result.Succeeded;
        }
        public async Task<bool> Update(UserModel entity)
        {
            var result = await _dbContext.UpdateAsync(entity);
            return result.Succeeded;
        }


        public async Task<UserModel> GetById(string id)
        {
            return await _dbContext.FindByIdAsync(id);
        }

        public bool CheckIfExistsById(string id)
        {
            return _dbContext.Users.Any(u => u.Id == id);
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            return await _dbContext.Users
                .AsNoTracking().OrderBy(u => u.Id)
                .ToListAsync();
        }


        public async Task<bool> CheckIfEmailExists(string email)
        {
            var user = await _dbContext.FindByEmailAsync(email); 
            return  user != null;
        }

        public async Task<bool> AddAdm(UserModel userModel)
        {
            var result = await _dbContext.AddToRoleAsync(
                userModel, RolesConsts.ADMINISTRATOR.ToString());

            return result.Succeeded;
        }

        public async Task<bool> RemoveAdm(UserModel userModel)
        {
            var result = await _dbContext.RemoveFromRoleAsync(userModel, 
                RolesConsts.ADMINISTRATOR.ToString());
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserModel>> FindByRole(string roleName)
        {
            var employees = await _dbContext.GetUsersInRoleAsync(roleName);
            return employees;
        }

        public async Task<IdentityResult> UpdateUser(UserModel userModel)
        {
            var result = await _dbContext.UpdateAsync(userModel);
            return result;
        }

        public async Task<IdentityResult> AddUser(UserModel userModel, string password)
        {
            var result = await _dbContext.CreateAsync(userModel, password);
            return result;
        }

        public async Task<UserModel> FindUserIncludeTask(string id)
        {
            var userTask = await _dbContext.Users.Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.Id == id);
            return userTask;
        }

        public async Task<UserModel> FindUserIncludeProject(string id)
        {
            var userProject = await _dbContext.Users.Include(up => up.Tasks)
                .FirstOrDefaultAsync();
            return userProject;
        }
    }
}