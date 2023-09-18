using AudioPool.Models;

namespace Models.DTOs;
public class GenreDetailsDto : HyperMediaModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfArtists { get; set; }
    
    
}
