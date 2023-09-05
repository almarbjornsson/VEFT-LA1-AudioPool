using System.ComponentModel.DataAnnotations;

namespace Models.InputModels;

public class ArtistInputModel
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    [MinLength(10)]
    public string Bio { get; set; }

    [Url]
    public string CoverImageUrl { get; set; }

    [Required]
    public DateTime DateOfStart { get; set; }
}