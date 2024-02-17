using DigiPay.Template.CoreModule.Domain.Samples;
using Elima.Common.Application.MediatR.Commands;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Results;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public class SampleDeletedByIdCommandHandler : ICommandHandler<SampleDeleteByIdCommand>
{
    private readonly ICommandSampleRepository _sampleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SampleDeletedByIdCommandHandler(ICommandSampleRepository sampleRepository, IUnitOfWork unitOfWork)
    {
        _sampleRepository = sampleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SampleDeleteByIdCommand request, CancellationToken cancellationToken)
    {
        await _sampleRepository.DeleteAsync(new SampleId(request.Id), cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}