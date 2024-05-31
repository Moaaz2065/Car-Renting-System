using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace Car_Renting_Project
{
    public partial class Form5 : Form
    {
        CrystalReport2 cr;
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = cr;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            cr = new CrystalReport2();
        }
    }
}
