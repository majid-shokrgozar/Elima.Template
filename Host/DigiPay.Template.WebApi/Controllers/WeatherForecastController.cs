using Elima.Common.EntityFramework.Uow;
using DigiPay.Template.FirstModule.Domain.Samples;
using Microsoft.AspNetCore.Mvc;

namespace DigiPay.Template.WebApi.Controllers
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
        public async Task<IEnumerable<Sample>> Get()
        {
            var sample = await _sampleRepository.InsertAsync(new Sample()
            {
                Name = "Test1"
            });
            await _unitOfWork.SaveChangesAsync();

            sample.Name = "test" + DateTime.Now.ToString();
            await _sampleRepository.UpdateAsync(sample);
            await _unitOfWork.SaveChangesAsync();



            await _sampleRepository.DeleteAsync(sample);
            await _unitOfWork.SaveChangesAsync();



            var list = await _sampleRepository.GetListAsync();
            return list
            .ToArray();
        }
    }
}