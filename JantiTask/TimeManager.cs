using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JantiTask
{
    public class TimeManager
    {
        private TimeManager() { }
        private static TimeManager? instance;
        public static TimeManager getInstance()
        {
            if (instance == null)
                instance = new TimeManager();
            return instance;
        }
        private TimeZoneInfo CurrentTimeZone { get; set; } = TimeZoneInfo.Utc;
        //Получение текущего времени в установленном часовом поясе
        public string GetTime()
        {
            return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, CurrentTimeZone)
                .ToString("dd.MM.yyyy HH:mm:ss zzz");
        }
        //Установка часового пояса по названию
        public bool SetTimeZone(string timeZoneId)
        {
            if (CheckTimeZoneId(timeZoneId))
            {
                CurrentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return true;
            }
            else
                return false;
        }
        //Установка часового пояса по датe
        public string ConvertDate(string date)
        {
            if (CanConvertToDate(date))
            {
                if (CheckDateFormat(date))
                {
                    DateTimeOffset.TryParse(date, out DateTimeOffset dt);
                    CurrentTimeZone = GetTimeZoneInfo(dt);
                    return GetTime();
                }
                else
                {
                    CurrentTimeZone = TimeZoneInfo.Utc;
                    return GetTime();
                }
            }
            else
                return "";
        }
        //Проверка введного часового пояса
        private bool CheckTimeZoneId(string timeZoneId)
        {
            if (TimeZoneInfo.TryConvertIanaIdToWindowsId(timeZoneId, out string? winTimeZoneId))
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                if (CurrentTimeZone.StandardName != timeZone.StandardName)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        //Проверка на соответсвие формату даты с часовым поясом
        private bool CheckDateFormat(string date)
        {
            string dateFormat = @"\w*[\+-]\d{2}\:\d{2}$";
            return Regex.IsMatch(date, dateFormat) ? true : false;
        }
        //Формирование переменной типа TimeZoneInfo на основе даты в запросе
        private TimeZoneInfo GetTimeZoneInfo(DateTimeOffset date)
        {
            return TimeZoneInfo.CreateCustomTimeZone("customId", date.Offset, "CustomTimeZoneName", "StandartCustomTimeZoneName");
        }
        //Проверка на возможность преобразования полученной строки в дату
        private bool CanConvertToDate(string date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTimeOffset dt;
            if (DateTimeOffset.TryParse(date, out dt) ||
                DateTimeOffset.TryParseExact(date, "dd/MM/yyyy HH-mm-ss", provider, DateTimeStyles.None, out dt))
                return true;
            else 
                return false;
        }
    }
}
