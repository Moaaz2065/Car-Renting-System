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
    public partial class Form4 : Form
    {
        CrystalReport1 cr;
        public Form4()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            cr = new CrystalReport1();
            foreach(ParameterDiscreteValue i in cr.ParameterFields[0].DefaultValues)
            {
                comboBox1.Items.Add(i.Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr.SetParameterValue(0, Convert.ToDateTime(comboBox1.Text));
            crystalReportViewer1.ReportSource = cr;
        }
    }
}
