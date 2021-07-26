using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAMS
{
    public partial class frmOrder : Form
    {
        public frmOrder()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            ctrlOrder orderImport = new ctrlOrder(this);
            TableLayoutPanel layoutPanel = new TableLayoutPanel();
            orderImport.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(orderImport);
            layoutPanel.Dock = DockStyle.Fill;
            this.Controls.Add(layoutPanel);
            
        }
    }
}
