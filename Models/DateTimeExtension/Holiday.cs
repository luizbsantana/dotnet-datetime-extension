using System;

namespace Extensions.Models.DateTimeExtension
{
    public class Holiday
    {
        public Holiday(DateTime date, string name)
        {
            Date = date;
            Name = name;
        }

        public DateTime Date { get; private set; }

        public string Name { get; private set; }
    }
}