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
    public partial class MainFom : Form
    {
        public MainFom()
        {
            InitializeComponent();

            MainMenu mainmenu = new MainMenu();
            MenuItem importMenu = new MenuItem("Start Import File");
            importMenu.Click += ImportMenu_Click;
            ImportMenu_Click(null, null);
            mainmenu.MenuItems.Add(importMenu);
            
            this.Menu = mainmenu;
        }

        private void ImportMenu_Click(object sender, EventArgs e)
        {
            frmOrder order = new frmOrder();
            order.MdiParent = this;
            //order.ParentForm ;
            order.Show();
        }
    }
}
