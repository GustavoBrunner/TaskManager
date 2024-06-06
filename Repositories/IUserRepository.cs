using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        public Task<IdentityResult> AddUser(UserModel userModel, string password);

        public Task<IdentityResult> UpdateUser(UserModel userModel);

        public Task<bool> CheckIfEmailExists(string email);

        public Task<bool> AddAdm(UserModel userModel);

        public Task<bool> RemoveAdm(UserModel userModel);

        public Task<IEnumerable<UserModel>> FindByRole(string roleName);

        public Task<UserModel> FindUserIncludeTask(string id);

        public Task<UserModel> FindUserIncludeProject(string id);
    }
}