using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QuizApp.BlazorWASM.Services;

namespace QuizApp.BlazorWASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            AddClientServices(builder);
            var app = builder.Build();
            
            await app.RunAsync();
        }

        private static void AddClientServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddHttpClient<ExamsClient>(client => 
                {
                    client.BaseAddress = new Uri("http://localhost:5000/api/");
                });
        }
    }
}
