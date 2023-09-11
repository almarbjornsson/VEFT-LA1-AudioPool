using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.Entities;
public class Genre
{
    
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public string? ModifiedBy { get; set; } = "AudioPoolAdmin";

    public ICollection<ArtistGenre> Artists { get; set; }
}