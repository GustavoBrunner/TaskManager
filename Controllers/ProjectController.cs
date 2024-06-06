using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskManager.DTO;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(){
            var projects = await _projectRepository.GetAll();
            
            return View(projects);
        }
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        [HttpGet]
        public async Task<IActionResult> Register(string id){
            if(string.IsNullOrEmpty(id)){
                //adição de um novo projeto
                return View(new ProjectModelDto());
            }
            //alteração de um projeto existente
            var project = _projectRepository.GetById(id);
            if(project is null){
                //nenhum projeto com esse id encontrado
            }
            var projectMapped = _mapper.Map<ProjectModelDto>(project);
            return View(projectMapped);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] ProjectModelDto modelDto){
            ProjectModel projectMapped = null;
            ModelState.Remove(nameof(ProjectModelDto.Tasks));
            if(!ModelState.IsValid){
                this.ShowInfoMessage("Informações incorretas.", true);
                return View(modelDto);
            }
            if(_projectRepository.CheckIfExistsById(modelDto.Id)){
                //alteração de projeto
                var project = await _projectRepository.GetById(modelDto.Id);
                project.ProjectDescription = modelDto.ProjectDescription;
                project.ProjectName = modelDto.ProjectName;
                project?.Tasks?.AddRange(modelDto?.Tasks);
                var updateResult = await _projectRepository.Update(project);
                if(updateResult){
                    this.ShowInfoMessage("Projeto alterado com sucesso!");
                }
                else{
                    this.ShowInfoMessage("Não foi possível adicionar o projeto!", true);
                }
                return RedirectToAction(nameof(Index));
            }
            //adição de novo projeto
            projectMapped = _mapper.Map<ProjectModel>(modelDto);
            var result = await _projectRepository.Add(projectMapped);
            if(result){
                this.ShowInfoMessage("Projeto adicionado com sucesso!");
            }
            else{
                this.ShowInfoMessage("Não foi possível adicionar o projeto", true);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProjectInfo(string id){
            var project = await _projectRepository.GetById(id);
            var projectMapped = _mapper.Map<ProjectModelDto>(project);
            return View(projectMapped);
        }






    }
}