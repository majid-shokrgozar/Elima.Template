using Autofac;
using Autofac.Extensions.DependencyInjection;
using Elima.Common.Modularity;
namespace Elima.Template.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(builder=> { 
            
                builder.RegisterAssemblyModules();
            });

            await builder.AddApplicationAsync<ElimaTemplateWebApiModule>();

            //builder.Services.AddControllers();

            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.InitializeApplicationAsync<ElimaTemplateWebApiModule>();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            //app.MapControllers();

            app.Run();
        }
    }
}
