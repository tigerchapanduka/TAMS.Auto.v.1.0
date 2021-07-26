using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAMS.BO
{

    public class Order
    {
        public Order()
        {

            //ChildOrders = new List<Order>();
            
            note_attributes = new List<Note>();
            tax_lines = new List<Tax>();
            line_items = new List<LineItem>();

        }
        public bool AlreadyImported { get; set; }
        public Boolean Imported { get; set; }
        public string ImportException { get; set; }
        public string order_number { get; set; }
        public string financial_status { get; set; }
        public string fulfilemnt_status { get; set; }

        public TotalShippingPrice total_shipping_price_set { get; set; }
        public TotalDiscountSet total_discounts_set { get; set; }
        public TotalDiscountSet total_line_items_price_set { get; set; }
        public TotalDiscountSet price_set { get; set; }
     
        public List<Tax> tax_lines { get; set; }
        public List<Note> note_attributes { get; set; }
        public List<LineItem> line_items { get; set; }
        public CustomerDetails Customer { get; set; }
        public double LineItemPrice { get; set; }
        public double LineItemComparePrice { get; set; }
        public int    LineItemQuantity { get; set; }
        public Address shipping_address { get; set; }
        public Address billing_address { get; set; }
        public string BillingStreet { get; set; }
        /*
        public string ShippingStreet { get; set; }
        public string ShippingAddress1 { get; set; }
        public string ShippingAddress2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingZip { get; set; }
        public string ShippingProvince { get; set; }
        public string ShippingCountry { get; set; }

        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingZip { get; set; }
        public string BillingProvince { get; set; }
        public string BillingCountry { get; set; }
        */
        //public List<Order> ChildOrders { get; set; }


        /*
        public string ReceiptNumber { get; set; }
        
        public string LineItemName { get; set; }
        public string LineItemSKU { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string contact_email { get; set; }
        public double total_tax { get; set; }
        public double subtotal_price { get; set; }
        
        public double ShippingAmount { get; set; }
        public double Taxes { get; set; }
        public double Discount { get; set; }
        */
        
        /* New Fields */

        public string id { get; set; }
        public string email { get; set; }
        public string closed_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string number { get; set; }
        public string note { get; set; }
       // public string token { get; set; }
        //public string gateway { get; set; }
        //public string test { get; set; }
        public double total_price { get; set; }
        public string total_weight { get; set; }
        public double total_line_items_price { get; set; }
        //public string cart_token { get; set; }

        public string taxes_included { get; set; }
        public string currency { get; set; }
        public string confirmed { get; set; }
        public string total_discounts { get; set; }
        
        
        public string buyer_accepts_marketing { get; set; }
        public string name { get; set; }
        public string referring_site { get; set; }
        public string landing_site { get; set; }
        public string cancelled_at { get; set; }
        public string cancel_reason { get; set; }
        public string total_price_usd { get; set; }
        public string checkout_token { get; set; }
        public string reference { get; set; }
        //public string user_id { get; set; }
        //public string location_id { get; set; }
        //public string source_identifier { get; set; }
        public string source_url { get; set; }
        public string processed_at { get; set; }
        //public string device_id { get; set; }
        public string phone { get; set; }
        public string customer_locale { get; set; }
        //public string app_id { get; set; }
        //public string browser_ip { get; set; }
    }
}
