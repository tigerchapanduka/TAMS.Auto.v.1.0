using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAMS.BO;

namespace TAMS.MAIL
{
    public static class Mailer
    {
        public  static void SendEmail(List<Order> importedOrders)
        {
            try
            {
                StringBuilder emailString = new StringBuilder();

                emailString.Append("<html><table>");
                emailString.Append("<tr><td>Order number</td><td>SKU</td></tr>");

                int count = 0;

                foreach (Order order in importedOrders)
                {
                    foreach (LineItem lineItem in order.line_items)
                    {
                        if (!string.IsNullOrEmpty(lineItem.ImportException))
                        {
                            emailString.Append("<tr><td>"+ order.order_number +"</td>");
                            emailString.Append("<td>").Append(lineItem.sku).Append("</td><tr>");
                            count = count + 1;
                        }
                    }
                }
                emailString.Append("</table></html>");


                if (count > 0)
                {
                    SendMessage(emailString);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                StringBuilder errMessage = new StringBuilder();
                errMessage.AppendLine(error);
                SendMessage(errMessage);
                //MessageBox.Show(errMessage);
            }
        
        }

        public static void SendMessage(StringBuilder emailString)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["gmailaddress"]);
                message.To.Add(ConfigurationManager.AppSettings["gmailaddress"]);
                message.IsBodyHtml = true;
                message.Subject = "TAMS Quickbooks Import Exception";
                message.Body = emailString.ToString();

                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationManager.AppSettings["smtphost"];
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpport"]);
                client.EnableSsl = true;
                string user = ConfigurationManager.AppSettings["gmailaddress"];
                string pswd = ConfigurationManager.AppSettings["gmailpassword"];
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(user, pswd);

                client.Send(message);

            } catch (Exception ex)
            { 
            
            
            }
            finally { 
            }
        }
    }
}
