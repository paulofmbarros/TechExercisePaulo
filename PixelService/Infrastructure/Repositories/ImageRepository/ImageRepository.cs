
namespace Infrastructure.Repositories.ImageRepository;
using Domain.AggregateModels.Image;


public class ImageRepository : IImageRepository
{
    public string GetImageContent()
    {
        return new Image().GetImageContent();
    }
}