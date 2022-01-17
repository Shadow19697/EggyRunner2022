using Scripts.Enums;
using System;

namespace Scripts.WorldTimeAPI
{
    public class SpecialDate
    {
        private long newYear = new DateTime(DateTime.Now.Year, 1, 1).Date.Ticks;
        private long xmas2 = new DateTime(DateTime.Now.Year, 1, 6).Date.Ticks;
        private long valentine = new DateTime(DateTime.Now.Year, 2, 14).Date.Ticks;
        private long birthday = new DateTime(DateTime.Now.Year, 6, 19).Date.Ticks;
        private long independence = new DateTime(DateTime.Now.Year, 7, 9).Date.Ticks;
        private long halloween = new DateTime(DateTime.Now.Year, 10, 31).Date.Ticks;
        private long xmas1 = new DateTime(DateTime.Now.Year, 12, 8).Date.Ticks;
        private long end = new DateTime(DateTime.Now.Year, 12, 31).Date.Ticks;

        public SpecialDateEnum WichSpecialIs()
        {
            long c = WorldTimeAPI.Instance.GetCurrentDateTime().Date.Ticks;
            if (c == valentine) return SpecialDateEnum.Valentine;
            if (c == birthday) return SpecialDateEnum.Birthday;
            if (c == independence) return SpecialDateEnum.Independence;
            if (c == halloween) return SpecialDateEnum.Halloween;
            if ((c >= xmas1 && c <= end) || (c >= newYear && c <= xmas2)) return SpecialDateEnum.Christmas;
            return SpecialDateEnum.Regular;
        }
    }
}