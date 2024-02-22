namespace Domain;

public interface IVisitMetadataRepository
{
    /// <summary>
    /// Saves the visit metadata to the text file.
    /// </summary>
    /// <param name="visitMetaData">The information of the visitor</param>
    /// <returns></returns>
    Task SaveAsync(VisitMetaData visitMetaData);
}