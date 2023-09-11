using AudioPool.Models;

namespace Models.DTOs;
public class SongDto : HyperMediaModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
}
