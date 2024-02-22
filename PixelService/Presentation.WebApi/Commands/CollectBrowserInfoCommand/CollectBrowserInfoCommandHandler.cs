

namespace Presentation.WebApi.Commands.CollectBrowserInfoCommand;
using MediatR;
using Domain.AggregateModels.Image;
using Presentation.WebApi.Dtos;
using Presentation.WebApi.Services.BrowserInfoCollectedEventPublisher;

public class CollectBrowserInfoCommandHandler : IRequestHandler<CollectBrowserInfoCommand, CollectBrowserInfoDtoOutput>
{
    private readonly IBrowserInfoCollectedEventPublisher _browserInfoCollectedEventPublisher;
    private readonly IImageRepository _imageRepository;

    public CollectBrowserInfoCommandHandler(IBrowserInfoCollectedEventPublisher browserInfoCollectedEventPublisher,
        IImageRepository imageRepository)
    {
        _browserInfoCollectedEventPublisher = browserInfoCollectedEventPublisher;
        _imageRepository = imageRepository;
    }

    public async Task<CollectBrowserInfoDtoOutput> Handle(CollectBrowserInfoCommand request, CancellationToken cancellationToken)
    {
        var imageContent = _imageRepository.GetImageContent();

        var transparentPixel = Convert.FromBase64String(imageContent);

        await _browserInfoCollectedEventPublisher.PublishAsync(request.Referrer, request.UserAgent, request.IpAddress);

        return new CollectBrowserInfoDtoOutput()
        {
            ContentType = "image/gif",
            Content = transparentPixel
        };
    }
}