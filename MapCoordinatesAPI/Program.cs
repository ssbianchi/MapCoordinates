var builder = WebApplication.CreateBuilder(args);

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adicionar serviços para Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.WebHost.UseUrls("http://0.0.0.0:7086");


var app = builder.Build();

// Usar CORS
app.UseCors("AllowAll");

// Habilitar o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Mapear Controllers
app.MapControllers();

app.Run();
