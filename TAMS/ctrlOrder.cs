using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;
using Microsoft.Win32;
using System.Configuration;
using TAMS.BO;
using TAMS.MAIL;

namespace TAMS
{
    public partial class ctrlOrder : UserControl
    {

        TableLayoutPanel orderlpnl;
        DataGridView  dg;
        DataGridView dgLineItems;
        Timer imporTimer;
        Form parent;
        public ctrlOrder(Form itsparent)
        {

            parent = itsparent;
            InitializeComponent();

            TableLayoutPanel tblLayoutPnl = new TableLayoutPanel();
            tblLayoutPnl.Dock = DockStyle.Fill;
            dgLineItems = new DataGridView();
            dgLineItems.Dock = DockStyle.Fill;
            dg = new DataGridView();
            dg.SelectionChanged += Dg_SelectionChanged;
            dg.Dock = DockStyle.Fill;
            RowStyle rs = new RowStyle(SizeType.Percent, 50);
            tblLayoutPnl.RowStyles.Add(rs);
            tblLayoutPnl.Controls.Add(dg,0,0);
            RowStyle rs1 = new RowStyle(SizeType.Percent, 50);
            tblLayoutPnl.RowStyles.Add(rs1);
            tblLayoutPnl.Controls.Add(dgLineItems, 0, 1);
            RowStyle rs2 = new RowStyle(SizeType.Percent, 50);
            tblLayoutPnl.RowStyles.Add(rs2);
            
            this.Controls.Add(tblLayoutPnl);
            importFileAsync();
            //Environment.Exit(-1);
                    

        }

        private void Dg_SelectionChanged(object sender, EventArgs e)
        {
            if (dg.SelectedRows.Count > 0)
            {
                if (dg.SelectedRows[0] != null)
                {
                    Order order = (Order)dg.SelectedRows[0].DataBoundItem;
                    dgLineItems.DataSource = order.line_items;
                }
            }
        }


        private void ImporTimer_Tick(object sender, EventArgs e)
        {
            imporTimer.Stop();
            this.ParentForm.Text = "Order Import : " + System.DateTime.Now.ToString();
            Cursor currentCursor = Cursors.WaitCursor;
            this.Cursor  = currentCursor;

            importFileAsync();
            imporTimer.Start();

            this.Cursor = DefaultCursor;
        }

        private  void importFileAsync()
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
                    BindingSource bs = new BindingSource(importedOrders, "");
                    dg.DataSource = bs;

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



  
    }
}

