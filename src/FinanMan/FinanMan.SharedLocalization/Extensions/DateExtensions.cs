using System.Globalization;

namespace FinanMan.SharedLocalization.Extensions
{
    public static class DateExtensions
    {
        public static string ToLocalizedLongDate(this DateTime dateTime, string lang) =>
            dateTime.ToLocalizedLongDate(CultureInfo.CreateSpecificCulture(lang));

        public static string ToLocalizedLongDate(this DateTime dateTime, CultureInfo cultureInfo, bool removeDayOfWeek = false)
        {
            var format = cultureInfo.DateTimeFormat.LongDatePattern;
            if(removeDayOfWeek)
            {
                format = format.Replace("dddd,", "").Replace(", dddd", "").Replace("dddd", "").Trim();
            }
            return dateTime.ToString(format, cultureInfo);
        }
    }
}
