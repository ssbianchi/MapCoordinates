var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Servir arquivos est�ticos da pasta wwwroot
app.UseStaticFiles();

app.Run();
