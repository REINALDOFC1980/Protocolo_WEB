using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Protocolo_web_adm.DBContext.Protocolo_web_adm.DataContext;
using Protocolo_web_adm.Service.IRepository;
using Protocolo_web_adm.Service.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IDapperServices, DapperServices>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IAutenticacaoServices, AutenticacaoServices>();
builder.Services.AddTransient<IMenuService, MenuService>();

builder.Services.AddTransient<IApiService, ApiService>();


builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LogoutPath = "/Autenticacao/Login";
        option.LoginPath = "/Autenticacao/Login";
        option.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Erro/Erro_500");
    //app.UseStatusCodePagesWithReExecute("/Home/HttpError", "?statusCode={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
 name: "default",
//pattern: "{controller=Autenticacao}/{action=Login}/{id?}");
//pattern: "{controller=Home}/{action=TriagemProcesso}/{id?}");
pattern: "{controller=Home}/{action=index}/{id?}");
app.Run();
