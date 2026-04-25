using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MathPillars.Web;
using MathPillars.Web.Servicos;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuracao do HttpClient para apontar para a API (ajustar porta conforme necessario no ambiente)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5296") });

// Registro dos servicos customizados do MathPillars
builder.Services.AddScoped<LeitorSSE>();
builder.Services.AddScoped<ClienteMathCore>();

await builder.Build().RunAsync();
