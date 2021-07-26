using System;
namespace TAMS.BO
{
    public class TotalDiscountSet
    {
        public Money shop_money { get; set; }
        public override string ToString()
        {
            return shop_money.ToString();// base.ToString();
        }
    }

}
