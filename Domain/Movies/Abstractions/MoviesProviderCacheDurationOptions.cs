namespace Domain.Movies.Abstractions;

public class MoviesProviderCacheDurationOptions
{
    /// <summary>
    /// Cache duration in minutes for the GetAll method
    /// </summary>
    public int GetAll { get; set; }

    /// <summary>
    /// Cache duration in minutes for the GetById method
    /// </summary>
    public int GetById { get; set; }

    /// <summary>
    /// Cache duration in minutes for the GetWithFilter method
    /// </summary>
    public int GetWithFilter { get; set; }
}
