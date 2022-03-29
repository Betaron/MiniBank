using System.Collections.Generic;

namespace Minibank.Data.HttpClients.Models
{
    public class CourseResponse
    {
        private DateTime Date { get; set; }
        public  Dictionary<string, CurrencyInfo> Valute { get; set; }
    }

    public class CurrencyInfo
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public double Value { get; set; }
    }
}
