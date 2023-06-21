using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSNQ
{
    public static class Class1
    {

        

    }
    public class JS1
    {
        public JS1(string date, int temperatureCelsius)
        {
            Date = date;
            TemperatureCelsius = temperatureCelsius;
        }

        public string Date { get; set; }
        public int TemperatureCelsius { get; set; }
    }


    public class JS2:JS1

    {
        public JS2(string summary,string date, int temperatureCelsius) : base(date, temperatureCelsius)
        {
           Summary=summary;
        }
        public string? Summary { get; set; }
    }
}