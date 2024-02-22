using MediatR;
using Presentation.WebApi.Dtos;

namespace Presentation.WebApi.Commands.CollectBrowserInfoCommand;

public class CollectBrowserInfoCommand : IRequest<CollectBrowserInfoDtoOutput>
{
   public string Referrer { get; set; }

    public string UserAgent { get; set; }

    public string IpAddress { get; set; }

}