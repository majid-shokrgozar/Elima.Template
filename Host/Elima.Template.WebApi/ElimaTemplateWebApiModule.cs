using Elima.Common.Modularity;
using Elima.Common.Security.Authentication;
using Elima.Template.BuildingBlocks.Persistence;
using Elima.Template.WebApi.Authorization;

namespace Elima.Template.WebApi
{
    [DependsOn(typeof(ElimaBuildingBlocksPersistenceModule))]
    public class ElimaTemplateWebApiModule:ElimaModule
    {
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            context.Services.AddControllers();

            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen();

            context.Services.AddHttpContextAccessor();

            context.Services.AddScoped<ICurrentUser, CurrentUser>();

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

            app.UseAuthorization();

            app.MapControllers();

            return base.OnApplicationInitializationAsync(context);
        }
    }
}
