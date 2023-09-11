namespace Models.DTOs;

public class ArtistDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public string CoverImageUrl { get; set; }
    public DateTime DateOfStart { get; set; }
    public IEnumerable<AlbumDto> Albums { get; set; }
    public IEnumerable<GenreDto> Genres { get; set; }
}