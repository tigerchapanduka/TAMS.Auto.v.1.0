using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TAMS.BO;

namespace TAMS
{
    public class DownloadManager
    {
        public DownloadManager()
        {
        }

        public List<Order> getLatestOrders()
        {
            List<Order> ordersList;
            string shopifyUser = ConfigurationManager.AppSettings["ShopifyPath"];
            using (var wc = new System.Net.WebClient())
            using (var stream = wc.OpenRead(shopifyUser))
            using (var textReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(textReader))
            {
               
                ordersList = JsonSerializer.CreateDefault().Deserialize<List<Order>>(reader);
            }
            return ordersList;
        }
        public List<Order> Getorders()
        {

            Orders orders = new Orders();
            try
            {
               
    
                using (var webClient = new WebClient())
                {
                    string path  = ConfigurationManager.AppSettings["ShopifyPath"];
                    string user = ConfigurationManager.AppSettings["userid"];
                    string pswd = ConfigurationManager.AppSettings["pswd"];

                    // webClient.Credentials = new NetworkCredential("43c461c854e18cbb5897b9eceea38fe7", "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");
                    webClient.Credentials = new NetworkCredential(user, pswd);


                    //var json = webClient.DownloadString("https://43c461c854e18cbb5897b9eceea38fe7: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@thandanasa.myshopify.com/admin/api/2021-01/orders.json?limit=150");
                    var json = webClient.DownloadString(path);

                    orders =  JsonConvert.DeserializeObject<Orders>(json.ToString());
                    Console.WriteLine(json.ToString());

                    return orders.orders;
                
                    // Now parse with JSON.Net
                }
             

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return orders.orders;
        }

    }


}
