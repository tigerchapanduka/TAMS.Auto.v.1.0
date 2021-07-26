using System;
using System.Collections.Generic;
namespace TAMS.BO
{
    public class LineItem
    {
        public LineItem()
        {
            tax_lines = new List<Tax>();
            properties = new List<Note>();
        }
        public bool Imported { get; set; }
        public string ImportException { get; set; }
        public List<Tax> tax_lines { get; set; }
        public Int64? id { get; set; }
        public string variant_id { get; set; }
        public string title { get; set; }
        
        public string quantity { get; set; }
        public string sku { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public string fulfillment_service { get; set; }
        
        public Int64? product_id { get; set; }
        public string requires_shipping { get; set; }
        public string taxable { get; set; }
        
        public string gift_card { get; set; }
        public string name { get; set; }
        public string? variant_inventory_management { get; set; }
        /*
         * must be an array current data coming through as empty array of undefined object
        public string properties { get; set; }
        */
        public Boolean product_exists { get; set; }
        
        public Int32? fulfillable_quantity { get; set; }
        
        public int grams { get; set; }
        
        public double price { get; set; }
        public double total_discount { get; set; }
        public string? fulfillment_status { get; set; }

        public TotalShippingPrice total_shipping_price_set { get; set; }
        public TotalDiscountSet total_discounts_set { get; set; }
        public TotalDiscountSet total_line_items_price_set { get; set; }
        public TotalDiscountSet price_set { get; set; }

        public List<Note> properties { get; set; }
    }
}
