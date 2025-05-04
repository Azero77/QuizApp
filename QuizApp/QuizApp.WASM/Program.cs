using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QuizApp.BlazorWASM.Services;
using QuizApp.BlazorWASM.Services.ExamServices;
using QuizApp.BlazorWASM.Services.HttpClientHandlers;

namespace QuizApp.BlazorWASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddMudServices();
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            AddClientServices(builder);
            builder.Services.AddScoped<ExamService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<AuthenticationStateProvider,BffAuthenticationStateProvider>();
            var app = builder.Build();
            
            await app.RunAsync();
        }

        private static void AddClientServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddHttpClient<ExamsClient>(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:5002/api/");
                })
                .AddHttpMessageHandler<CookieHandler>()
                .AddHttpMessageHandler<AntiforgeryHandler>()
                .AddHttpMessageHandler<HttpClientErrorDelegationHandler>();
            builder.Services.AddSingleton<HttpClientErrorDelegationHandler>();
            builder.Services.AddSingleton<AntiforgeryHandler>();
            builder.Services.AddSingleton<CookieHandler>();
        }
    }
}
