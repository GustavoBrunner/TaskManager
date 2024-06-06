using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TaskManager.Models;

namespace TaskManager.Settings
{
    public static class Initializer
    {
        public static void ProfileInitialize(RoleManager<IdentityRole> roleManager){
            if(!(roleManager.RoleExistsAsync(Roles.administrator.ToString()).Result)){
                var newRole = new IdentityRole(){
                    Name = Roles.administrator.ToString(),
                };
                roleManager.CreateAsync(newRole).Wait();
            }
            if(!(roleManager.RoleExistsAsync(Roles.employee.ToString()).Result)){
                var newRole = new IdentityRole(){
                    Name = Roles.employee.ToString()
                };
                roleManager.CreateAsync(newRole).Wait();
            }
        }

        public static void UserInitialize(UserManager<UserModel> userManager) {
            if(userManager.FindByNameAsync("admin@email.com").Result == null){
                var user = new UserModel() {
                    UserName = "admin@email.com",
                    Email = "admin@email.com",
                    Cpf = "00000000000",
                    PhoneNumber = "99999999999",
                    FullName = "System Adm",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Birth = new DateTime(1980, 1, 1)
                };
                var result = userManager.CreateAsync(user, "123@Aa").Result;
                if(result.Succeeded){
                    userManager.AddToRoleAsync(user, Roles.administrator.ToString()).Wait();
                }
            }
        }

        public static void InitializeUser(UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager){
            ProfileInitialize(roleManager);
            UserInitialize(userManager);    
        }
    }
}