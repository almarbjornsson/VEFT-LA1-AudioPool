namespace Models.Entities;

public class ArtistGenre
{
    public int ArtistsId { get; set; }
    public Artist Artist { get; set; }
    public int GenresId { get; set; }
    public Genre Genre { get; set; }
}