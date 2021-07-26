using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAMS.BO;
using TAMS.MAIL;
namespace TAMS.TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Order> ls = new List<Order>();
            Order order = new Order();
            LineItem item = new LineItem();
            order.order_number = "test";
            order.ImportException = "test exception";
            item.sku = "test value";
            item.ImportException = "test";
            List<LineItem> itemList = new List<LineItem>();
            itemList.Add(item);
            order.line_items = itemList;
            ls.Add(order);
            Mailer.SendEmail(ls);
        }
    }
}
