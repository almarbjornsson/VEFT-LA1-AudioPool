using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class Artist
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Bio { get; set; }

    public string CoverImageUrl { get; set; }

    [Required]
    public string DateStarted { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public string ModifiedBy { get; set; }

    // Navigation Properties
    public ICollection<Album> Albums { get; set; }
    public ICollection<Genre> Genres { get; set; }
}