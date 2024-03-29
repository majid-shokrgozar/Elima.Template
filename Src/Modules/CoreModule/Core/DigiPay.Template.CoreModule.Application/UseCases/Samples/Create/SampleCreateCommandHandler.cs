﻿using Elima.Common.Application.MediatR.Commands;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Results;
using DigiPay.Template.CoreModule.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples.Create
{
    public class SampleCreateCommandHandler : ICommandHandler<SampleCreateCommand, SampleDto>
    {
        private readonly ICommandSampleRepository _sampleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SampleCreateCommandHandler(ICommandSampleRepository sampleRepository, IUnitOfWork unitOfWork)
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

            return SampleDto.MapFromEntity(sample);
        }
    }
}
