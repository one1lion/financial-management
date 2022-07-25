namespace FinanMan.Database.Models.Shared;

[AttributeUsage(AttributeTargets.Field)]
internal class RecurrenceTypeInfoAttribute : Attribute
{
    public string DisplayText { get; set; } = default!;
}