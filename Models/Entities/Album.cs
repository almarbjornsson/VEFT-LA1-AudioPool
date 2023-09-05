using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class Album
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    public string CoverImageUrl { get; set; }

    public string Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public string ModifiedBy { get; set; }

    // Navigation Properties
    public ICollection<Artist> Artists { get; set; }
    public ICollection<Song> Songs { get; set; }
}