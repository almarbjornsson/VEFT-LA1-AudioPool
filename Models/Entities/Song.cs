using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;
public class Song
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public string? ModifiedBy { get; set; }

    // Foreign Key
    public int AlbumId { get; set; }

    // Navigation Property
    public Album Album { get; set; }
}