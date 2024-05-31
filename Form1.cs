using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Car_Renting_Project
{
    public partial class Form1 : Form
    {
        string ordb = "Data Source=ORCL;User Id=hr;Password=hr;";
        OracleConnection conn;
        public Form1()
        {
            InitializeComponent();
            conn = new OracleConnection(ordb);
            conn.Open();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username == "")
            {
                MessageBox.Show("Please enter username", "Wrong");
                return;
            }
            if (password == "")
            {
                MessageBox.Show("Please enter password", "Wrong");
                return;
            }
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "select password,user_id from programuser where username=:tmp";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("tmp", username);
            OracleDataReader dr = c.ExecuteReader();
            if(dr.Read())
            {
                string dbPassword = dr.GetString(0);
                decimal id = dr.GetDecimal(1);
                if(dbPassword != password)
                {
                    MessageBox.Show("Wrong Password");
                    return;
                }
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select user_id from admin where user_id = :tmp";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("tmp", id);
                dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    Form3 f = new Form3();
                    f.Show();
                    this.Hide();
                    return;
                }
                cmd.CommandText = "select balance from customer where user_id = :tmp";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cmd.Parameters.Clear();
                    Form2 f = new Form2(id, dr.GetDecimal(0));
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Excepetion");
                }
                dr.Close();
            }
            else
            {
                MessageBox.Show("Wrong Username");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = textBox4.Text;
            string password = textBox3.Text;
            string confPassword = textBox5.Text;
            if (username == "" || password == "" || confPassword == "")
            {
                MessageBox.Show("Please Fill the missing spaces", "Wrong");
                return;
            }
            if(password != confPassword)
            {
                MessageBox.Show("Password mismatch");
                return;
            }
            if(password.Count() < 8)
            {
                MessageBox.Show("Password size must be 8 or more");
                return;
            }
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "select username from programuser where username=:tmp";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("tmp", username);
            OracleDataReader dr = c.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Username Already Exists");
                return;
            }
            else
            {
                //confirm registrition
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getmaxid";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", OracleDbType.Int32, ParameterDirection.Output);
                cmd.ExecuteNonQuery();
                int mxID;
                try
                {
                    mxID = Convert.ToInt32(cmd.Parameters["id"].Value.ToString());
                }
                catch
                {
                    mxID = 0;
                }
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO programuser VALUES (:id, :name, :password)";
                cmd.Parameters.Add("id", mxID + 1);
                cmd.Parameters.Add("name", username);
                cmd.Parameters.Add("password", password);
                int r = cmd.ExecuteNonQuery();

                if (r != -1)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO customer VALUES (:id, :balance)";
                    cmd.Parameters.Add("id", mxID + 1);
                    cmd.Parameters.Add(new OracleParameter("balance", OracleDbType.Decimal)).Value = 0;
                    r = cmd.ExecuteNonQuery();

                    if (r != -1)
                    {
                        MessageBox.Show("Successfully Registered");
                        panel1.BringToFront();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }
    }
}
