using App;
using Microsoft.EntityFrameworkCore;

await using var db = new DataContext();
await db.Database.MigrateAsync();