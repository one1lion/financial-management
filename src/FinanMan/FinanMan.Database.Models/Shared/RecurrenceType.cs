using System.Reflection;

namespace FinanMan.Database.Models.Shared;

public enum RecurrenceType 
{
    [RecurrenceTypeInfo(DisplayText = "One-time")]
    OneTime = 1,
    [RecurrenceTypeInfo(DisplayText = "Daily")]
    Daily,
    [RecurrenceTypeInfo(DisplayText = "Weekly")]
    Weekly,
    [RecurrenceTypeInfo(DisplayText = "Bi-weekly")]
    BiWeekly,
    [RecurrenceTypeInfo(DisplayText = "Monthly")]
    Monthly,
    [RecurrenceTypeInfo(DisplayText = "Bi-monthly")]
    BiMonthly,
    [RecurrenceTypeInfo(DisplayText = "Quarterly")]
    Quarterly,
    [RecurrenceTypeInfo(DisplayText = "Semi-annually")]
    SemiAnnually,
    [RecurrenceTypeInfo(DisplayText = "Annually")]
    Annually,
    [RecurrenceTypeInfo(DisplayText = "Custom...")]
    Custom
}

public static class RecurrenceTypeExtensions
{
    public static string GetDisplayText(this RecurrenceType recurrenceType)
    {
        var type = recurrenceType.GetType();
        var field = type.GetField(recurrenceType.ToString());
        var attribute = field?.GetCustomAttribute<RecurrenceTypeInfoAttribute>(false);
        return attribute?.DisplayText ?? string.Empty;
    }
}