using Microsoft.EntityFrameworkCore;
using Bogus;
namespace App;

public class CRUD
{
    public static async Task<User> Create(string? name, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var user = new User()
        {
            Name = name!=null? name:throw new NullReferenceException()
        };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user;
    }
    
    public static async Task<Note> Create(string title,int userId, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var record = new Note
        {
            Title = title, 
            CreatedAt = DateTimeOffset.Now,
            UserId = db.Users.Find(userId)!.Id
        };
        db.Notes.Add(record);
        await db.SaveChangesAsync(ct);
        return record;
    }
    
    public static async Task<List<User>> Read(int idSearch, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var result = await db.Users
            .Where(x => idSearch == x.Id)
            .ToListAsync(ct);
        return result;
    }
    public static async Task<List<Note>> Read(string titleSearch, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var result = await db.Notes
            .Where(x => EF.Functions.Like(x.Title, $"%{titleSearch}%"))
            .ToListAsync(ct);
        return result;
    }
    
    public static async Task<List<Note>> ReadAllNotes(int idSearch, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var result = await db.Notes
            .Where(x => idSearch == x.UserId)
            .ToListAsync(ct);
        return result;
    }
    
    public static async Task Update(User user, string changes, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        user.Name = changes;
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }
    
    public static async Task Update(Note note, string changes,int userId, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        note.Title = changes;
        note.CreatedAt =  DateTimeOffset.Now;
        note.UserId = userId;
        db.Notes.Update(note);
        await db.SaveChangesAsync(ct);
    }
    
    public static async Task Delete(User user, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        db.Users.Remove(user);
        var notes = db.Notes.Where(x => x.UserId == user.Id).ToList();
        foreach (var record in notes)
        {
            db.Notes.Remove(record);
        }
        await db.SaveChangesAsync(ct);
    }
    public static async Task Delete(Note record, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        db.Notes.Remove(record);
        await db.SaveChangesAsync(ct);
    }

    public static async Task GenerateData(int numberOfUsers, int numberOfNotes,CancellationToken ct = default)
    {
        await using var db = new DataContext();
        
        var faker = new Faker("ru");
        for (int i = 0; i < numberOfUsers; i++)
        {
            var user = new User
            {
                Name = faker.Name.FullName(),
            };
            db.Users.Add(user);
            List<Note> notes = [];
            for (int j = 0; j < numberOfNotes; j++)
            {
                notes.Add(new Note
                {
                    Title = faker.Lorem.Word(),
                    CreatedAt = DateTimeOffset.Now,
                    UserId = 0,
                    User = user
                });
            }
            user.Notes = notes;
        }
        await db.SaveChangesAsync(ct);
    }
}