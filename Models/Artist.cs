namespace Models;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Followers { get; set; }
    public string Biography { get; set; }
    public DateTime CreateDate { get; set; }
    public bool SoftDelete { get; set; }
    public List<Album> Albums { get; set; } 

}
