using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAMS.BO;
using TAMS.MAIL;

namespace TAMS
{
    
    public partial class MainFom : Form
    {
        Timer timer;
        public MainFom()
        {
            InitializeComponent();
            /*
            MainMenu mainmenu = new MainMenu();
            MenuItem importMenu = new MenuItem("Start Import File");
            importMenu.Click += ImportMenu_Click;
            ImportMenu_Click(null, null);
            mainmenu.MenuItems.Add(importMenu);
            this.Menu = mainmenu;
            */
            int freq = Convert.ToInt32( ConfigurationManager.AppSettings["frequency"]);
            timer = new Timer();
            timer.Interval = freq;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            importFileAsync();
            timer.Start();

        }

        private void importFileAsync()
        {
            try
            {
                DownloadManager downLoadManager = new DownloadManager();
                List<Order> ls = downLoadManager.Getorders();

                string qbFilePath = ConfigurationManager.AppSettings["QBFilePath"];

                //File.AppendAllText("log.txt", "\n Checking file exists");
                if (File.Exists(@qbFilePath))
                {
                    //  File.AppendAllText("log.txt", "\n File does exists");
                    QuickBooks qbOrder = new QuickBooks(qbFilePath, "QBTAMS", ls, "");

                    //List<Order> combinedImportList = new List<Order>();
                    List<Order> importedOrders = qbOrder.CreateOrder();
                 

                    Mailer.SendEmail(importedOrders);

                }
                else
                {

                    StringBuilder errMessage = new StringBuilder();
                    errMessage.AppendLine("Quickbooks file path is not valid");
                    Mailer.SendMessage(errMessage);

                }


            }
            catch (Exception ex)
            {
                //imporTimer.Start();

                // StringBuilder sb = new StringBuilder();
                //sb.Append(ex.Message);

                //File.AppendAllText("log.txt", "\n " + sb.ToString());
                //sb.Clear();
                string error = ex.Message;
                StringBuilder errMessage = new StringBuilder();
                errMessage.AppendLine(error);
                Mailer.SendMessage(errMessage);

            }

        }
        private void ImportMenu_Click(object sender, EventArgs e)
        {
            /*
            frmOrder order = new frmOrder();
            order.MdiParent = this;
            //order.ParentForm ;
            order.Show();
            */
        }
    }
}
