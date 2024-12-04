using HumbertoMVC.Controllers;
using HumbertoMVC.Controllers.systemControllers;
using HumbertoMVC.Models;
using HumbertoMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o Swagger ao projeto
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adiciona o Swagger

// Adiciona o HttpClient e registra o ApiService
builder.Services.AddHttpClient<ApiService>();

// Adiciona serviços ao contêiner (MVC e controle de visualizações)
builder.Services.AddControllersWithViews();

// Configura a sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Segurança extra
    options.Cookie.IsEssential = true; // Necessário para uso em cookies de sessão
});

// Configura o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        builder => { builder.WithOrigins("https://localhost:44336").AllowAnyHeader().AllowAnyMethod(); }
    );
});

// Configura os cookies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true; // Perguntar consentimento ao usuário
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});

// Adiciona o IHttpContextAccessor ao contêiner
builder.Services.AddHttpContextAccessor(); // Este é o registro correto

// Adiciona outros serviços ao contêiner
builder.Services.AddTransient<RoutesService>();
builder.Services.AddTransient<GeoCodingService>();
builder.Services.AddTransient<RouteController>();
builder.Services.AddTransient<BaseController>();
builder.Services.AddTransient<AdressController>();




var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Ativa o HSTS para ambientes de produção
}

// Configurações do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Humberto API v1");
        c.RoutePrefix = "swagger"; // Define o caminho para acessar a documentação em /swagger
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ativa o CORS
app.UseCors("AllowSpecificOrigin");

// Ativa o middleware de sessão
app.UseSession();

app.UseAuthorization();

// Define a rota padrão
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
