// File: Interfaces/IPersianCalendarService.cs
using System.Runtime.Serialization;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for working with Persian calendar dates.
    /// </summary>
    public interface IPersianCalendarService
    {
        int Day { get; }
        DayOfWeek DayOfWeek { get; }
        int GetDayOfYear { get; }
        string GetLongDayOfWeekName { get; }
        string GetLongMonthName { get; }
        int GetMonthDays { get; }
        string GetPersianAmPm { get; }
        string GetShortDayOfWeekName { get; }
        int GetShortYear { get; }
        int GetWeekOfMonth { get; }
        int GetWeekOfYear { get; }
        int Hour { get; }
        bool IsLeapYear { get; }
        string LongTimeOfDay { get; }
        int Millisecond { get; }
        int Minute { get; }
        int Month { get; }
        string MonthName { get; }
        bool PersianNumber { get; set; }
        int Second { get; }
        int ShortHour { get; }
        string ShortTimeOfDay { get; }
        string TimeOfDay { get; }
        int Year { get; }
        int CompareTo(DateTime otherDateTime);
        string ElapsedTime();
        bool Equals(DateTime other);
        bool Equals(object obj);
        int GetDifferenceQuarter(DateTime targetDateTime);
        int GetHashCode();
        int GetMonthEnum(string longMonthName);
        void GetObjectData(SerializationInfo info, StreamingContext context);
        static IPersianCalendarService Now { get; }
        TimeSpan GetTime();
        TypeCode GetTypeCode();
        int MonthDifference(DateTime dateTime);
        bool ToBoolean(IFormatProvider provider);
        byte ToByte(IFormatProvider provider);
        char ToChar(IFormatProvider provider);
        DateTime ToDateTime();
        DateTime ToDateTime(IFormatProvider provider);
        decimal ToDecimal(IFormatProvider provider);
        double ToDouble(IFormatProvider provider);
        int ToEpochTime();
        short ToInt16(IFormatProvider provider);
        int ToInt32(IFormatProvider provider);
        long ToInt64(IFormatProvider provider);
        string ToLongDateString();
        long ToLongDateTimeInt();
        string ToLongDateTimeString();
        string ToLongTimeString();
        sbyte ToSByte(IFormatProvider provider);
        string ToShortDate1String();
        int ToShortDateInt();
        string ToShortDateString();
        string ToShortDateTimeString();
        string ToShortTimeString();
        float ToSingle(IFormatProvider provider);
        string ToString();
        string ToString(IFormatProvider provider);
        string ToString(string format, IFormatProvider? fp = null);
        int ToTimeInt();
        short ToTimeShort();
        object ToType(Type conversionType, IFormatProvider provider);
        ushort ToUInt16(IFormatProvider provider);
        uint ToUInt32(IFormatProvider provider);
        ulong ToUInt64(IFormatProvider provider);
    }
}