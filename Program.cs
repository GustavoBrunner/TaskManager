using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration
    .GetConnectionString(nameof(TaskManagerDbContext));


// builder.Services.AddDbContext<TaskManagerDbContext>(options =>
//     options.UseSqlite(connectionString));
builder.Services.AddDbContext<TaskManagerDbContext>( options =>
    options.UseMySql(connectionString,
    ServerVersion.AutoDetect(connectionString)));


builder.Services.AddIdentity<UserModel, IdentityRole>(options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})  
    .AddEntityFrameworkStores<TaskManagerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped(typeof(IProjectRepository), typeof(ProjectRepository));

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ConfigureApplicationCookie( options => {
    options.Cookie.Name = "AppTaskManager";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Employee/Login";
    options.LogoutPath = "/Home/Index";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.SlidingExpiration = true;
    options.ReturnUrlParameter = "returnUrl";
});

var app = builder.Build();



app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"
);

using (var scope = app.Services.CreateScope()){
    var service = scope.ServiceProvider;
    //ao inicializar o programa ele faz as migrations necessárias para atualizar o banco de dados da maneira que a aplicação necessita para sua execução
    // var context = service.GetRequiredService<TaskManagerDbContext>();
    // context.Database.Migrate();

    var roleManager = service
        .GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = service
        .GetRequiredService<UserManager<UserModel>>();
    Initializer.InitializeUser(userManager, roleManager);

}

app.Run();
