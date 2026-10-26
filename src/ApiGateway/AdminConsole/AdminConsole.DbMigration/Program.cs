using AdminConsole.DbMigration;
using Microsoft.EntityFrameworkCore;

using var context = new TourManagementDbContextFactory().CreateDbContext(args);
context.Database.Migrate();
Console.WriteLine("Database migrated.");
