namespace Presentation.WebApi.Dtos;

public class CollectBrowserInfoInputDto
{
    public string Referrer { get; set; }

    public string UserAgent { get; set; }

    public string IpAddress { get; set; }

}