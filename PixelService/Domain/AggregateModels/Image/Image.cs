namespace Domain.AggregateModels.Image;

public class Image
{
    private string Content { get; set; } = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

    public string GetImageContent()
    {
        return Content;
    }
}