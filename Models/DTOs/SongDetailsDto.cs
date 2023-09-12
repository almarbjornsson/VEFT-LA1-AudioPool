using AudioPool.Models;
namespace Models.DTOs;

public class SongDetailsDto : HyperMediaModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
    public AlbumDto Album { get; set; }
    
    // public int TrackNumberOnAlbum { get; set; }
    
}
