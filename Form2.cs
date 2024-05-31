using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Renting_Project
{
    public partial class Form2 : Form
    {
        decimal balance = 0,id;
        string ordb = "Data Source=ORCL;User Id=hr;Password=hr;";
        OracleConnection conn;
        List<Tuple<int, int, string>> curr_items;
        public Form2(decimal id, decimal balance)
        {
            InitializeComponent();
            this.id = id;
            this.balance = balance;
            label2.Text = this.balance.ToString();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            curr_items = new List<Tuple<int, int, string>>(); 
            string s = textBox1.Text;
            if(s == "")
            {
                MessageBox.Show("Fill Space");
                return;
            }
            conn = new OracleConnection(ordb);
            conn.Open();

            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "select car_id,model_name,price,state from car where model_name = :tmp";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("tmp", s);
            OracleDataReader dr = c.ExecuteReader();
            while(dr.Read())
            {
                comboBox1.Items.Add($"Model : {dr[1]}  Price : {dr[2]}");
                curr_items.Add(new Tuple<int, int, string>(int.Parse(dr[0].ToString()), int.Parse(dr[2].ToString()), dr[3].ToString()));
            }
            dr.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                decimal newBalance = Convert.ToDecimal(textBox2.Text);
                balance += newBalance;
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand c = new OracleCommand();
                c.Connection = conn;
                c.CommandText = "update customer set balance = :bla where user_id = :id";

                c.Parameters.Add("bla", balance);
                c.Parameters.Add("id", id);
                c.ExecuteNonQuery();
                label2.Text = balance.ToString();
            }
            catch
            {
                MessageBox.Show("Something Wrong");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "My_RENTS";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("u_id", id);
            cmd.Parameters.Add("CID", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                listView1.Items.Add($"{dr[0].ToString()}  {dr[1].ToString()}  {dr[2].ToString()}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idx = comboBox1.SelectedIndex;
            if(curr_items[idx].Item3 == "R")
            {
                MessageBox.Show("Currently Rented", "Sorry!");
                return;
            }
            if(balance < curr_items[idx].Item2)
            {
                MessageBox.Show("Your Current Balance is less than the required", "Error!");
                return;
            }
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "update car set state = 'R' where car_id = :id";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("id", curr_items[idx].Item1.ToString());
            c.ExecuteNonQuery();

            c.CommandText = "update customer set balance = :bal where user_id = :id";
            c.Parameters.Clear();
            balance -= curr_items[idx].Item2;
            c.Parameters.Add("bal", balance.ToString());
            c.Parameters.Add("id", id);
            c.ExecuteNonQuery();
            label2.Text = balance.ToString();
            
            c.CommandText = "INSERT INTO reservation (car_id,user_id,start_date,end_date) VALUES (:c_id, :u_id, :st_day, :en_day)";
            c.Parameters.Clear();
            c.Parameters.Add("c_id", curr_items[idx].Item1);
            c.Parameters.Add("u_id", id);
            c.Parameters.Add("st_day", Convert.ToDateTime(DateTime.Now.Date.ToString()));
            c.Parameters.Add("en_day", Convert.ToDateTime(DateTime.Now.AddDays(30).Date.ToString()));
            c.ExecuteNonQuery();
        }
    }
}
