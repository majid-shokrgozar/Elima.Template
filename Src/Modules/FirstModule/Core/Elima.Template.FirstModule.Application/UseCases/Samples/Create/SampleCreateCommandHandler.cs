using Elima.Common.Application.MediatR.Commands;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Results;
using Elima.Template.FirstModule.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elima.Template.FirstModule.Application.UseCases.Samples.Create
{
    public class SampleCreateCommandHandler : ICommandHandler<SampleCreateCommand, SampleDto>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SampleCreateCommandHandler(ISampleRepository sampleRepository, IUnitOfWork unitOfWork)
        {
            _sampleRepository = sampleRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<SampleDto>> Handle(SampleCreateCommand request, CancellationToken cancellationToken)
        {
            var sample = await _sampleRepository.InsertAsync(new Sample()
            {
                Name = request.Name
            }, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SampleDto(sample.Name);
        }
    }
}
