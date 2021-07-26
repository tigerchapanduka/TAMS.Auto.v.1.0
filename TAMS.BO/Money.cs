using System;
namespace TAMS.BO
{
    public class Money
    {
        public Money()
        {
            
        }
        public string amount { get; set; }
        public string currency_code { get; set; }
        public override string ToString()
        {
            return currency_code + " " + amount;// base.ToString();
        }
    }
}
