using DigiPay.Template.CoreModule.Domain.Samples;
using Elima.Common.Application.MediatR.Commands;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Results;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public class SampleUpdateByIdCommandHandler : ICommandHandler<SampleUpdateByIdCommand, SampleDto>
{
    private readonly ICommandSampleRepository _commandSampleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SampleUpdateByIdCommandHandler(ICommandSampleRepository commandSampleRepository, IUnitOfWork unitOfWork)
    {
        _commandSampleRepository = commandSampleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SampleDto>> Handle(SampleUpdateByIdCommand request, CancellationToken cancellationToken)
    {
        var sample = await _commandSampleRepository.GetAsync(new SampleId(request.Id), cancellationToken);

        if (sample == null) return Result<SampleDto>.NotFound(request.Id);

        sample.Name = request.Name;

        await _commandSampleRepository.UpdateAsync(sample);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SampleDto.MapFromEntity(sample);
    }
}