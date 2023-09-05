namespace Models;

public class SongDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
    public AlbumDto Album { get; set; }
    public int TrackNumberOnAlbum { get; set; }
}
