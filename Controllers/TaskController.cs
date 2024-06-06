using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.ViewModel;

namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public TaskController(ITaskRepository taskRepository, IMapper mapper, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(){
            var task = await _taskRepository.GetAllTasksIncludeProject();
            

            return View(task);
        }
        [HttpGet]
        public async Task<IActionResult> TaskInfo(string id){
            var task = await _taskRepository.TaskInformations(id);
            var taskMapped = _mapper.Map<TaskModelDto>(task);
            
            
            return View(taskMapped);
        }
        [HttpGet]
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> Register(string id){
            var projectList = await _projectRepository.GetAll();
            SelectList selectList = new SelectList(projectList, 
                nameof(ProjectModel.Id), nameof(ProjectModel.ProjectName));
            //params: coleção dos dados, o valor que será passado, do objeto escolhido, e o que será mostrado na lista, hora da escolha
            ViewBag.Projects = selectList;

            var employees = await _userRepository.GetAll();
            SelectList employeeList = new SelectList(
                employees, nameof(UserModel.Id), nameof(UserModel.FullName));
            ViewBag.Employees = employeeList;
            
            
            
            if(!string.IsNullOrEmpty(id)){
                var task = await _taskRepository.GetById(id);
                if(task is not null){
                    var taskMapped = _mapper.Map<TaskModelDto>(task);
                    return View(taskMapped);
                }
                else{
                    this.ShowInfoMessage("Tarefa não encontrada!", true);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(new TaskModelDto());
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] TaskModelDto modelDto){
            UserModel responsible = null;
            if(_taskRepository.CheckIfExistsById(modelDto.Id)){
                //atualização de tarefa
                var task = await _taskRepository.GetById(modelDto.Id);
                responsible = await _userRepository.GetById(modelDto.ResponsibleId);
                var taskMap = _mapper.Map<TaskModel>(task);
                task.Resposibles?.Add(responsible);
                task.IsActive = taskMap.IsActive;
                task.Resposibles = taskMap.Resposibles;
                task.IsFinished = taskMap.IsFinished; 
                task.TaskName = taskMap.TaskName;
                task.TaskDescription = taskMap.TaskDescription;

                if(ModelState.IsValid){
                    this.ShowInfoMessage("Tarefa atualizada com sucesso");
                }
                else{
                    this.ShowInfoMessage("Não foi possível atualizar tarefa", true);
                }
                return RedirectToAction(nameof(Index));
            }
            //inserção de nova tarefa
            ModelState.Remove(nameof(modelDto.IsActive));
            ModelState.Remove(nameof(modelDto.IsFinished));
            if(!ModelState.IsValid){
                this.ShowInfoMessage("Dados incorretos!", true);
                return View(modelDto);
            }
            var taskMapped = _mapper.Map<TaskModel>(modelDto);
            responsible = await _userRepository.GetById(modelDto.ResponsibleId);
            
            var project = await _projectRepository.GetById(taskMapped?.Project?.Id);
            taskMapped.Project = project;
            taskMapped.Resposibles = new List<UserModel>();
            taskMapped.Resposibles?.Add(responsible);
            responsible.Tasks?.Add(taskMapped);
            
            var result = await _taskRepository.Add(taskMapped);
            if (result){
                //adicionado com sucesso
                project.Tasks?.Add(taskMapped);
                await _userRepository.Update(responsible);
                this.ShowInfoMessage("Tarefa adicionda com sucesso");
            }
            else{
                //falha ao adicionar    
                this.ShowInfoMessage("Falha ao adicionar tarefa", true);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> RegisterOnProject(string pid){
            if(string.IsNullOrEmpty(pid))
            {
                this.ShowInfoMessage("Projeto não informado", true);
                return RedirectToAction(nameof(Index), "Project");
            }
            ViewBag.ProjectId = pid;
            var employees = await _userRepository.GetAll();
            SelectList employeesList = new SelectList(employees, 
                nameof(UserModel.Id), nameof(UserModel.FullName));
            ViewBag.EmployeeList = employeesList;

            return View(new TaskModelDto());
        }
        [HttpPost]
        public async Task<IActionResult> RegisterOnProject(
            [FromForm] TaskModelDto modelDto, string pid){
            if(string.IsNullOrEmpty(pid)) {
                this.ShowInfoMessage("Projeto não informado!", true);
                return RedirectToAction(nameof(Index));
            }
            modelDto.ProjectId = pid;
            ModelState.Remove(nameof(TaskModelDto.ProjectId));
            if(!ModelState.IsValid){
                this.ShowInfoMessage("Informações incorretas!", true);
                return View(modelDto);
            }
            var taskMapped = _mapper.Map<TaskModel>(modelDto);
            var responsible = await _userRepository.GetById(modelDto.ResponsibleId);
            taskMapped.Resposibles = new List<UserModel>
            {
                responsible,
            };
            responsible.Tasks?.Add(taskMapped);
            var project = await _projectRepository.GetById(pid);
            taskMapped.Project = project;
            var result = await _taskRepository.Add(taskMapped);
            if(result){
                project?.Tasks?.Add(taskMapped);
                var projectUpdate = await _projectRepository.Update(project);
                if(projectUpdate){
                    this.ShowInfoMessage($"Tarefa {taskMapped.TaskName} adicionada ao projeto {project.ProjectName} com sucesso");
                }
                else{
                    this.ShowInfoMessage("Falha ao adicionar tarefa", true);
                }
                return RedirectToAction(nameof(Index), "Project");
            }
            else{
                return RedirectToAction(nameof(Index));
            }
                
        }
        [Authorize(Roles = RolesConsts.ADMINISTRATOR)]
        public async Task<IActionResult> Delete(string id){
            if(string.IsNullOrEmpty(id)){
                //tarefa não informada
            }
            
            var task = await _taskRepository.GetById(id);
            if(task is null){
                //tarefa não existe
            }
            var project = await _projectRepository.GetById(task.Project?.Id);
            if(project is not null){
                project.Tasks?.Remove(task);
            }
            if(task.Resposibles != null){
                foreach(var r in task.Resposibles){
                    r.Tasks?.Remove(task);
                }
            }
            task.Resposibles?.Clear();
            var result = await _taskRepository.Delete(task);
            if(result){
                this.ShowInfoMessage("Tarefa excluída com sucesso!");
            }
            else{
                this.ShowInfoMessage("Não foi possível excluir tarefa!", true);
            }
            return RedirectToAction(nameof(Index));
        }







        
    }
}