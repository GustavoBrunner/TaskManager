using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using TaskManager.DTO;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.ViewModel;

namespace TaskManager.Controllers
{
    
    public class EmployeeController : Controller
    {
        private readonly IUserRepository _repository;

        private readonly SignInManager<UserModel> _signInManager;
        private readonly IMapper _mapper;

        public EmployeeController(IUserRepository repository, 
            SignInManager<UserModel> signInManager, IMapper mapper)
        {
            _repository = repository;
            _signInManager = signInManager;
            this._mapper = mapper;
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(){
            var users = await _repository.GetAll();
            TempData["adms"] = await _repository.FindByRole(RolesConsts.ADMINISTRATOR.ToString());
            return View(users);
        }
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> Register(string id){
            if(string.IsNullOrEmpty(id)){
                return View(new UserModelDto());
            }
            var user = await _repository.GetById(id);
            if(user == null){
                return RedirectToAction(nameof(Index));
            }
            var userMapped = _mapper.Map<UserModelDto>(user);
            return View(userMapped);
        }

        [HttpPost]
        public async Task<IActionResult> Register (UserModelDto model){
            
            if(!string.IsNullOrEmpty(model.Id)){
                ModelState.Remove(nameof(model.Password));  
                ModelState.Remove(nameof(model.ConfPassword));
            }

            if(!ModelState.IsValid){
                this.ShowInfoMessage("Credenciais incorretas!",true);
                return View(model);
            }
            
            if(!_repository.CheckIfExistsById(model.Id)){
                //adição de novo usuários
                var userMapped = _mapper.Map<UserModel>(model);
                if(await _repository.CheckIfEmailExists(userMapped.Email)){
                    this.ShowInfoMessage("Email informado já existe", true);
                    return View(model);
                }
                var result = await _repository.AddUser(userMapped, model.Password);
                if(result.Succeeded){
                    this.ShowInfoMessage("Usuário adicionado com sucesso");
                }
                else{
                    foreach(var e in result.Errors){
                        ModelState.AddModelError(string.Empty, e.Description);
                    }
                    this.ShowInfoMessage("Não foi possível adicionar usuário", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else{
                //atualização de usuário existente
                var user = await _repository.GetById(model.Id);
                if((user.Email != model.Email) && 
                        (await _repository.CheckIfEmailExists(model.Email))){
                    this.ShowInfoMessage("Email já existente", true);
                    return View(model);
                }
                else{
                    
                    var userMapped = _mapper.Map<UserModel>(model);
                     
                    user.Email = userMapped.Email;
                    user.FullName = userMapped.FullName;
                    user.Cpf = userMapped.Cpf;
                    user.Birth = userMapped.Birth;
                    user.PhoneNumber = userMapped.PhoneNumber;
                     
                    var result = await _repository.UpdateUser(user);
                    if(result.Succeeded){
                        this.ShowInfoMessage("Usuário atualizado com sucesso");
                    }
                    else{
                        foreach(var e in result.Errors){
                            ModelState.AddModelError(string.Empty, e.Description);
                        }
                        this.ShowInfoMessage(
                            "Não foi possível atualizar usuário", true);
                        return View(model);
                    }
                    
                }
                
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public IActionResult Login(){

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel){
            ModelState.Remove(nameof(loginViewModel.ReturnUrl));
            if(!ModelState.IsValid){
                this.ShowInfoMessage("Credenciais incorretas!", true);
                return View(loginViewModel);
            }

            var result = await _signInManager
                .PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, 
                    loginViewModel.RememberMe, false);
            
            if(result.Succeeded){
                this.ShowInfoMessage("Login efetuado com sucesso!");
                loginViewModel.ReturnUrl = loginViewModel.ReturnUrl ?? "~/";
                return LocalRedirect(loginViewModel.ReturnUrl);
            }
            else{
                this.ShowInfoMessage("Não foi possível efetuar o login, credenciais erradas!", true);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout(string returnUrl = null){
            await _signInManager.SignOutAsync();
            if(returnUrl != null){
                return LocalRedirect(returnUrl);
            }
            else{
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> Delete(string id){
            if(string.IsNullOrEmpty(id)){
                this.ShowInfoMessage("Usuário não informado!", true);
                return RedirectToAction(nameof(Index));
            }
            if(!_repository.CheckIfExistsById(id)){
                this.ShowInfoMessage("Usuário não existe", true);
            }
            var user = await _repository.GetById(id);
            return View(user);
        }

        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        [HttpPost]
        public async Task<IActionResult> DeletePost(string id){
            var user = await _repository.GetById(id);
            if(user is null){
                this.ShowInfoMessage("Usuário não encontrado", true);
                return RedirectToAction(nameof(Index));
            }
            var result = await _repository.Delete(user);
            if(result){
                this.ShowInfoMessage("Usuário excluído com sucesso!");
            }
            else{
                this.ShowInfoMessage("Não foi possível excluir usuário!", true);
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> AddAdm(string id){
            if(!string.IsNullOrEmpty(id)){
                var user = await _repository.GetById(id);
                if(user is not null){
                    
                    var result = await _repository.AddAdm(user);
                    if(result){
                        this.ShowInfoMessage($"{StringExtensions.GetFirstWord(user.FullName)} adicionado aos administradores");
                    }
                    else{
                        this.ShowInfoMessage($"Não foi possível adicionar   {StringExtensions.GetFirstWord(user.FullName)} aos  administradores");
                    }
                }
                else{
                    this.ShowInfoMessage("Usuário não encontrado", true);
                }
            }
            else{
                this.ShowInfoMessage("Usuário não encontrado", true);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> RemoveAdm(string id){
            if(string.IsNullOrEmpty(id)){
                this.ShowInfoMessage("Usuário não informado", true);
                RedirectToAction(nameof(Index));
            }
            var user = await _repository.GetById(id);
            if(user is not null){
                var result = await _repository.RemoveAdm(user);
                if(result){
                    this.ShowInfoMessage($"{StringExtensions.GetFirstWord(user.FullName)} removido dos administradores");
                }
                else {
                    this.ShowInfoMessage($"Não foi possível remover {StringExtensions.GetFirstWord(user.FullName)} dos administradores", true);
                }
            }
            else{
                this.ShowInfoMessage("Funcionário não encontrado!", true);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowTasks(string id){
            var user = await _repository.FindUserIncludeTask(id);
            TempData["tasks"] = user?.Tasks?
                .OrderBy(t => t.TaskName)
                .AsEnumerable<TaskModel>();
            return View(user);
        }

        public async Task<IActionResult> ShowProject(string id){
            var user = await _repository.FindUserIncludeProject(id);
            TempData["projects"] = user?.Project;
            return View(user);
        }













        
    }
}