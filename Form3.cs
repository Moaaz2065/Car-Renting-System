using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Renting_Project
{
    public partial class Form3 : Form
    {
        OracleDataAdapter adapter;
        OracleCommandBuilder commandBuilder;
        DataSet ds;
        string cmdstr, constr;
        public Form3()
        {
            InitializeComponent();
            constr = "Data Source = orcl; User ID = hr ; password = hr;";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                commandBuilder = new OracleCommandBuilder(adapter);
                adapter.Update(ds.Tables[0]);
            }
            catch
            {
                MessageBox.Show("There is Something Wrong");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            cmdstr = "Select * from car where state = 'R'";
            adapter = new OracleDataAdapter(cmdstr, constr);
            ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 f = new Form5();
            f.Show();
            this.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            cmdstr = "Select * from car where state = 'A'";
            adapter = new OracleDataAdapter(cmdstr, constr);
            ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
