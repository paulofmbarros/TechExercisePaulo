using MediatR;
using Presentation.WebApi.Dtos;

namespace Presentation.WebApi.Commands;

public class StoreVisitCommand : IRequest<Unit>
{
    public CollectBrowserInfoInputDto CollectBrowserInfoInputDto { get; set; }

    public string TimeStamp { get; set; }
}