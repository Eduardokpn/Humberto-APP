using HumbertoMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o HttpClient e registra o ApiService
builder.Services.AddHttpClient<ApiService>(); // Injeção do HttpClient no ApiService

// Adiciona serviços ao contêiner (MVC e controle de visualizações)
builder.Services.AddControllersWithViews();

// Configura o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:44336").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Ativa o HSTS para ambientes de produção
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();