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

namespace WindowsFormsAppS
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void logoutP_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void logoutLbl_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void BooksLbl_Click(object sender, EventArgs e)
        {
            Books Obj = new Books();
            Obj.Show();
            this.Hide();
        }

        private void UsersLbl_Click(object sender, EventArgs e)
        {
            Users Obj = new Users();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\itsso\source\repos\LibraryProjectTeam\assets\BookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void Dashboard_Load(object sender, EventArgs e)
        {
            Con.Open();

            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(BQty) from BookTbl", Con); 
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            BookStockLbl.Text = "Stock: " + dt1.Rows[0][0].ToString();

            SqlDataAdapter sda2 = new SqlDataAdapter("select sum(Amount) from BillTbl", Con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            AmountLbl.Text = dt2.Rows[0][0].ToString() + " TL";

            SqlDataAdapter sda3 = new SqlDataAdapter("select count(*) from UserTbl", Con);
            DataTable dt3 = new DataTable();
            sda3.Fill(dt3);
            UsrLbl.Text = dt3.Rows[0][0].ToString() + " users";

            Con.Close();
        }

        private void BooksLabel_Click(object sender, EventArgs e)
        {
            Books Obj = new Books();
            Obj.Show();
            this.Hide();
        }

        private void BillsLabel_Click(object sender, EventArgs e)
        {
            Billing Obj = new Billing();
            Obj.Show();
            this.Hide();
        }

        private void UsersLabel_Click(object sender, EventArgs e)
        {
            Users Obj = new Users();
            Obj.Show();
            this.Hide();
        }
    }
}
