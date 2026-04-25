using MathPillars.Api.Modulos.AlgebraLinear;
using MathPillars.Api.Modulos.CalculoMultivariavel;
using MathPillars.Api.Modulos.Probabilidade;

var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS para permitir acesso do Blazor WASM
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Injeção de Dependência - Módulo Álgebra Linear
builder.Services.AddScoped<ProdutoEscalarServico>();
builder.Services.AddScoped<SimilaridadeCossenoServico>();
builder.Services.AddScoped<SVDServico>();
builder.Services.AddScoped<PCAServico>();
builder.Services.AddScoped<AutovetoresServico>();

// Injeção de Dependência - Módulo Cálculo Multivariável
builder.Services.AddScoped<GradienteServico>();
builder.Services.AddScoped<JacobianaServico>();
builder.Services.AddScoped<LossLandscapeServico>();
builder.Services.AddScoped<ComparadorOtimizadoresServico>();

// Injeção de Dependência - Módulo Probabilidade
builder.Services.AddScoped<BayesServico>();
builder.Services.AddScoped<GaussianaServico>();
builder.Services.AddScoped<EntropiaCruzadaServico>();

var app = builder.Build();

app.UseCors("PermitirBlazor");
app.UseAuthorization();
app.MapControllers();

app.Run();
