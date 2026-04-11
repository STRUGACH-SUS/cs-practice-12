using System.Net.Mime;
using App;
using Bogus;
using Microsoft.EntityFrameworkCore;

await using var db = new DataContext();
await db.Database.MigrateAsync();

// var faker = new Faker("ru");
// for (int i = 0; i < 5; i++)
// {
//     var user = new User
//     {
//         NameOfUser = faker.Name.FullName(),
//     };
//     db.Users.Add(user);
//     List<Note> notes = [];
//     for (int j = 0; j < 2; j++)
//     {
//         notes.Add(new Note
//         {
//             Name = "TEXT",
//             TypeOfSqlite = "TEXT",
//             TypeInCSharp = "string",
//             UserId = 0,
//             User = user
//         });
//     }
//     user.Notes = notes;
// }
// await db.SaveChangesAsync();