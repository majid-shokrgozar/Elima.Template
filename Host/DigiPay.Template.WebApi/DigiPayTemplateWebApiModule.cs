using Autofac;
using Elima.Common.DependencyInjection;
using Elima.Common.ExceptionHandling;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Elima.Common.Security.Authentication;
using DigiPay.Template.BuildingBlocks.Application;
using DigiPay.Template.BuildingBlocks.Persistence;
using DigiPay.Template.BuildingBlocks.Presentation;
using DigiPay.Template.BuildingBlocks.Presentation.ExceptionHandler;
using DigiPay.Template.BuildingBlocks.Presentation.Middleware;
using DigiPay.Template.WebApi.Authorization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.HttpsPolicy;

namespace DigiPay.Template.WebApi
{
    [DependsOn(typeof(DigiPayBuildingBlocksPersistenceModule))]
    [DependsOn(typeof(DigiPayBuildingBlocksPresentationModule))]
    [DependsOn(typeof(DigiPayBuildingBlocksApplicationModule))]
    public class DigiPayTemplateWebApiModule : ElimaAutofacModule
    {
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            context.Services.AddControllers();
            context.Services.AddHttpContextAccessor();

            context.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterType<AbpExceptionHandlingMiddleware>();
            });

            context.Services
                .AddFastEndpoints(options => options.DisableAutoDiscovery = false)
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


            app.UseHttpsRedirection();


            app.MapControllers();

            app.UseMiddleware<AbpExceptionHandlingMiddleware>();

            app.UseFastEndpoints()
               .UseSwaggerGen()
               .UseSwaggerUI();

            return base.OnApplicationInitializationAsync(context);
        }
    }

}
