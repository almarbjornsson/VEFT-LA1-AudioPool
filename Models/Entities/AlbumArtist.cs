namespace Models.Entities;

public class AlbumArtist
{
    public int AlbumsId { get; set; }
    public Album Album { get; set; }
    public int ArtistsId { get; set; }
    public Artist Artist { get; set; }
}