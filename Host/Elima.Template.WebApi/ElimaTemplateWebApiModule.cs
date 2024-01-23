using Elima.Common.Modularity;

namespace Elima.Template.WebApi
{
    public class ElimaTemplateWebApiModule:ElimaModule
    {
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            context.Services.AddControllers();

            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen();

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
