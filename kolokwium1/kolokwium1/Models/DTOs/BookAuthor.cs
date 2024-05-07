namespace kolokwium1.Models.DTOs;

public class BookAuthor
{
    public int id { get; set; }
    public string title { get; set; }
    public AuthorDTO Author { get; set; }
    
}
public record AuthorDTO
{
    public string firstName { get; set; }
    public string lastName { get; set; }
}