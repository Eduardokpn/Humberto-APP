using HumbertoMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o HttpClient e registra o ApiService
builder.Services.AddHttpClient<ApiService>();

// Add services to the container.
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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