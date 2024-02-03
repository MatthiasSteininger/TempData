using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await CreateWebHostBuilder().Build().RunAsync();
    }

    public static IWebHostBuilder CreateWebHostBuilder() =>
        new WebHostBuilder()
            .UseKestrel()
            .ConfigureServices(services => services.AddSingleton<Program>())
            .Configure(app => app.Run(context => context.RequestServices.GetRequiredService<Program>().HandleRequestAsync(context)));

    public async Task HandleRequestAsync(HttpContext context)
    {
        // Read the request body
        using (var reader = new StreamReader(context.Request.Body))
        {
            var body = await reader.ReadToEndAsync();

            // Print the received message to the console
            Console.WriteLine($"Received message: {body}");

            // Respond to the sender (optional)
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Message received successfully.");
        }
    }
}
