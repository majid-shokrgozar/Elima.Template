using Autofac;
using Elima.Common.DependencyInjection;
using Elima.Common.ExceptionHandling;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Elima.Common.Security.Authentication;
using Elima.Template.BuildingBlocks.Application;
using Elima.Template.BuildingBlocks.Persistence;
using Elima.Template.BuildingBlocks.Presentation;
using Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;
using Elima.Template.BuildingBlocks.Presentation.Middleware;
using Elima.Template.WebApi.Authorization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.HttpsPolicy;

namespace Elima.Template.WebApi
{
    [DependsOn(typeof(ElimaBuildingBlocksPersistenceModule))]
    [DependsOn(typeof(ElimaBuildingBlocksPresentationModule))]
    [DependsOn(typeof(ElimaBuildingBlocksApplicationModule))]
    public class ElimaTemplateWebApiModule : ElimaAutofacModule
    {
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            context.Services.AddControllers();

            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen();

            context.Services.AddHttpContextAccessor();

            context.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterType<AbpExceptionHandlingMiddleware>();
            });

            context.Services
                .AddFastEndpoints()
                .SwaggerDocument();

            context.Services.Configure<ExceptionHandlingOptions>(option =>
            {
                option.SendStackTraceToClients = true;
                option.SendExceptionsDetailsToClients = true;
            });

            return Task.CompletedTask;
        }

        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var app = context.ApplicationBuilder;

            // Configure the HTTP request pipeline.
            if (context.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<AbpExceptionHandlingMiddleware>();

            app.UseFastEndpoints()
               .UseSwaggerGen();
            //.UseSwaggerUI();

            return base.OnApplicationInitializationAsync(context);
        }
    }

}
