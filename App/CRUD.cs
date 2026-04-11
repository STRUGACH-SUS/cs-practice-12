using Microsoft.EntityFrameworkCore;
namespace App;

public class CRUD
{
    // public static async Task<Note> Create(string name, string typeOfSqlite, string typeInCSharp, CancellationToken ct = default)
    // {
    //     await using var db = new DataContext();
    //     var record = new Note
    //     {
    //         Name = name,
    //         TypeOfSqlite = typeOfSqlite,
    //         TypeInCSharp = typeInCSharp
    //         
    //     };
    //     db.Notes.Add(record);
    //     await db.SaveChangesAsync(ct);
    //     return record;
    // }
    //
    // public static async Task<List<Note>> Read(string search, CancellationToken ct = default)
    // {
    //     await using var db = new DataContext();
    //     var result = await db.Notes
    //         .Where(x => EF.Functions.Like(x.Name, $"%{search}%"))
    //         .ToListAsync(ct);
    //     return result;
    // }
    //
    // public static async Task Update(Note record, string name, string typeOfSqlite, string typeInCSharp, CancellationToken ct = default)
    // {
    //     await using var db = new DataContext();
    //     record.Name = name;
    //     record.TypeOfSqlite =  typeOfSqlite;
    //     record.TypeInCSharp = typeInCSharp;
    //     db.Notes.Update(record);
    //     await db.SaveChangesAsync(ct);
    // }
    //
    // public static async Task Delete(Note record, CancellationToken ct = default)
    // {
    //     await using var db = new DataContext();
    //     db.Notes.Remove(record);
    //     await db.SaveChangesAsync(ct);
    // }
}