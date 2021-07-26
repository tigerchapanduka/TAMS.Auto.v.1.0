using System;
namespace TAMS.BO
{
    public class TotalShippingPrice
    {

        public Money shop_money { get; set; }
        public override string ToString()
        {
            return shop_money.ToString();// base.ToString();
        }
    }

}

