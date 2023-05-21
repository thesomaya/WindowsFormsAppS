﻿using System;
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
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate(); 
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\itsso\source\repos\LibraryProjectTeam\assets\BookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string query = "select * from BookTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int key = 0, stock=0;
        private void BookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BTitleTb.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            if (BTitleTb.Text == "")
            {
                key = 0;
                stock = 0;
            }
            else
            {
                key = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value.ToString());
                stock = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[4].Value.ToString());
            }
        }
        
        private void Reset()
        {
            BTitleTb.Text = "";
            QtyTb.Text = "";
            PriceTb.Text = "";
            ClientNameTb.Text = "";
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void UpdateBook()
        {
            stock = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[4].Value.ToString());
            int newQty = stock - Convert.ToInt32(QtyTb.Text);
            try
            {
                Con.Open();
                string query = "update BookTbl set BQty=" + newQty + " where BId=" + key + ";";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book Updated Successfully.");
                Con.Close();
                populate();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        
        int n = 0, Grdtotal = 0;
        private void SaveBtn_Click(object sender, EventArgs e)
        {        
            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > stock)
            {
                MessageBox.Show("No Enough Stock");
            } 
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n+1;
                newRow.Cells[1].Value = BTitleTb.Text;
                newRow.Cells[2].Value = PriceTb.Text;
                newRow.Cells[3].Value = QtyTb.Text;                
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
                n++;
                UpdateBook();
                Grdtotal += total;
                TotalLbl.Text = "Total " + Grdtotal + " TL";
                Con.Open();
                string query = "insert into BillTbl (BName, ClientName, Amount) values ('" + BTitleTb.Text + "','" + ClientNameTb.Text + "'," + total + ")";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
        }
        int prodid, prodqty, prodprice, tottal, pos = 60;

        private void logoutP_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void LogoutLbl_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            UsernameLbl.Text = Login.UserName;
        }

        string prodname;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Book Shop", new Font("Century Gothic", 12), Brushes.Black, new Point(80));
            e.Graphics.DrawString("ID PRODUCT PRICE QUANTITY TOTAL", new Font("Century Gothic", 10), Brushes.Black, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column1"].Value);
                prodname = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToInt32(row.Cells["Column3"].Value);
                prodqty = Convert.ToInt32(row.Cells["Column4"].Value);
                tottal = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Century Gothic", 8), Brushes.Blue, new Point(26, pos));
                e.Graphics.DrawString("" + prodname, new Font("Century Gothic", 8), Brushes.Blue, new Point(45, pos));
                e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString("" + prodqty, new Font("Century Gothic", 8), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 8), Brushes.Blue, new Point(235, pos));
                pos = pos + 20;
            }
            e.Graphics.DrawString("Total: TL " + Grdtotal, new Font("Century Gothic", 12), Brushes.Black, new Point(60, pos + 50));
            e.Graphics.DrawString("***********BookStore***********", new Font("Century Gothic", 10), Brushes.Black, new Point(40, pos + 85));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            Grdtotal = 0;
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            if (ClientNameTb.Text == "" || BTitleTb.Text == "")
            {
                MessageBox.Show("Type Client Name.");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into BillTbl values ('" + UsernameLbl.Text + "','" + ClientNameTb.Text + "'," + Grdtotal + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bill Saved Successfully.");
                    Con.Close();
                    //populate();
                    //Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
                
            }
        }
    }
}
