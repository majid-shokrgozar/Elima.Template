using Elima.Common.EntityFramework.Uow;
using Elima.Template.FirstModule.Domain.Samples;
using Microsoft.AspNetCore.Mvc;

namespace Elima.Template.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISampleRepository _sampleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ISampleRepository sampleRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _sampleRepository = sampleRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await _sampleRepository.InsertAsync(new Sample()
            {
                Name = "Test1",
                CreateDate = DateOnly.FromDateTime(DateTime.Now),
                CreateTime = TimeOnly.FromDateTime(DateTime.Now)
            });
            await _unitOfWork.SaveChangesAsync();
            var list = await _sampleRepository.GetListAsync();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}