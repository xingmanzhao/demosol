using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace DemoWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            Saler s = new Saler();
            s.Price = 10.1m;
            s.Goods = "Apple";

            MessageBox.Show(string.Format("the price of {0} is {1}.",s.Goods,s.Price));
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            Task t = new Task(()=>this.DoAction());
            t.Start();
        }

        private void DoAction()
        {
            
            this.txtAction.Text = "task action";
        }



    }
}
