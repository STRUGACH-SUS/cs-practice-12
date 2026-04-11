namespace App;

public class User
{
    public int Id { get; set; }
    public required  string  NameOfUser { get; set; }
    
    public ICollection<Note>? Notes { get; set; }
}