using System.ComponentModel.DataAnnotations;

namespace Models.InputModels;

public class AlbumInputModel
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public IEnumerable<int> ArtistIds { get; set; }

    [Url]
    public string CoverImageUrl { get; set; }

    [MinLength(10)]
    public string Description { get; set; }
}