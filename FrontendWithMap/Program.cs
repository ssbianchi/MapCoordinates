var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Servir arquivos estáticos da pasta wwwroot
app.UseStaticFiles();

app.Run();
