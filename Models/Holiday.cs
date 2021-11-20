using System;

namespace Brazil.Datetime.Models
{
    public class Holiday
    {
        public Holiday(DateTime date, string name)
        {
            this.Date = date;
            this.Name = name;

        }
        public DateTime Date { get; private set; }

        public string Name { get; private set; }
    }
}