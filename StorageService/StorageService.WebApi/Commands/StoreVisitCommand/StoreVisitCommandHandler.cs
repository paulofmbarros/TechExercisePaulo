using Domain;
using MediatR;

namespace Presentation.WebApi.Commands;

public class StoreVisitCommandHandler : IRequestHandler<StoreVisitCommand, Unit>
{

    private readonly IVisitMetadataRepository _visitMetadataRepository;
    private readonly ILogger<StoreVisitCommandHandler> _logger;

    public StoreVisitCommandHandler(IVisitMetadataRepository visitMetadataRepository,
        ILogger<StoreVisitCommandHandler> logger)
    {
        _visitMetadataRepository = visitMetadataRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(StoreVisitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var visitMetadata = new VisitMetaData(
                request.CollectBrowserInfoInputDto.Referrer,
                request.CollectBrowserInfoInputDto.UserAgent,
                request.CollectBrowserInfoInputDto.IpAddress,
                request.TimeStamp);


            this._logger.LogInformation("Storing visit metadata.");
            await this._visitMetadataRepository.SaveAsync(visitMetadata);

            this._logger.LogInformation("Visit metadata stored with success.");
        }
        catch (Exception e)
        {
            this._logger.LogError("Error storing visit metadata.");
            throw;
        }

        return Unit.Value;


    }
}