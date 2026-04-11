using App;
using Microsoft.EntityFrameworkCore;

await using var db = new DataContext();
await db.Database.MigrateAsync();

User unit = db.Users.Find(26);
CRUD.Delete(unit);