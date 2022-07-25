using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace FinanMan.Database.Data;

public static class DataHelper
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var sortOrder = 1;
        modelBuilder.Entity<LuAccountType>().HasData(
            new List<LuAccountType>() { 
                new() { Id = sortOrder, Name = "Checking", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Savings", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Credit Card", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Cash", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) }
            }
        );

        sortOrder = 1;
        modelBuilder.Entity<LuCategory>().HasData(
            new List<LuCategory>() {
                new() { Id = sortOrder, Name = "Grocery Store", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "General Goods", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Fast Food", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Restaraunt", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Streaming Serivce", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Apothecary", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Clothing", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) }
            }
        );

        sortOrder = 1;
        modelBuilder.Entity<LuLineItemType>().HasData(
            new List<LuLineItemType>() {
                new() { Id = sortOrder, Name = "Sub Total", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Tax", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Fee", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Surcharge", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Shipping", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Tip", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Refund", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) },
                new() { Id = sortOrder, Name = "Discount", SortOrder = sortOrder++, LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00) }
            }
        );
        
        sortOrder = 1;
        modelBuilder.Entity<LuRecurrenceType>().HasData(
            Enum.GetValues<RecurrenceType>().Select(x => new LuRecurrenceType() { 
                Id = x,
                Name = x.ToString(),
                DisplayText = x.GetDisplayText(),
                SortOrder = sortOrder++,
                LastUpdated = new DateTime(2022, 07, 25, 17, 41, 00)
            })
        );
    }
}
