using AudioPool.Models;

namespace Models.DTOs;
public class ArtistDto : HyperMediaModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public string CoverImageUrl { get; set; }
    public DateTime DateOfStart { get; set; }
}
