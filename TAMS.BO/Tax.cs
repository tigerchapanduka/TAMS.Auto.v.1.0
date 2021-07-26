using System;
namespace TAMS.BO
{
    public class Tax
    {
        public Tax()
        {
            
        }
        public string price { get; set; }
        public string rate { get; set; }
        public string title { get; set; }

        public override string ToString()
        {
            return price + " " + title + " " + rate;// base.ToString();
        }
    }
}
