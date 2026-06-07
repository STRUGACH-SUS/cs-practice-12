using System.ComponentModel.DataAnnotations;

namespace App;

public class Note
{
    public int Id { get; set; }
    public required  string  Title { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    
    public required int UserId {get; set;}
    public User? User {get; set;}
}