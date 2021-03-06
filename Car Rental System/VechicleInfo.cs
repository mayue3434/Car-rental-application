﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Super_market
{
    public partial class Select : Form
    {
        private SqlConnectionStringBuilder b = new SqlConnectionStringBuilder();
        private SqlConnection con;
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da;
        private DataSet ds;
        private SqlCommandBuilder d;
        int x;
        int y;

        public Select()
        {
            InitializeComponent();
            b.DataSource = ".";
            b.InitialCatalog = "wkdl";
            b.IntegratedSecurity = true;
            con = new SqlConnection(b.ConnectionString);
            //this.skinengine1.skinfile = "macos.ssk";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                LoadDGV();
            if (this.Text == "Guest System")
            {

            }else if (this.Text =="Member System")
            {
                button4.Enabled = true;
            }
            else
            {
                button3.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
            }
        }
        private void LoadDGV()
        {
            //cmd.CommandText = "select id as 商品编号,warename as 商品名称,price as 单价,type as 已售出数量,zip as 保质期 from ware";
            cmd.CommandText = "select id,warename,price,zip,type,stocks from ware";
            cmd.Connection = con;
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[2].HeaderText = "Price(/mouth)";
            dataGridView1.Columns[3].HeaderText = "Zip";
            dataGridView1.Columns[4].HeaderText = "Type";
            dataGridView1.Columns[5].HeaderText = "Stocks";
            dataGridView1.RowHeadersVisible = false;

            setFridViewProperty();
        }

        private void setFridViewProperty()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.Rows[0].Selected = false;
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ds.Tables[0].Rows[x].Delete();
            d = new SqlCommandBuilder(da);
            da.Update(ds);
            MessageBox.Show("Delete successfully！");
            LoadDGV();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Insert ins = new Insert();
            ins.evt += new Insert.dele(LoadDGV);
            ins.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (IsNumeric(textBox1.Text.Trim()) || IsNumeric(textBox3.Text.Trim()) || textBox2.Text.Length != 0 || IsNumeric(textBox4.Text.Trim()) || textBox5.Text.Length != 0 || IsNumeric(textBox6.Text.Trim()))
            {

                    string str = "select id,warename,price,zip,type,stocks from ware where ";

                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Name";
                    dataGridView1.Columns[2].HeaderText = "Price(/mouth)";
                    dataGridView1.Columns[3].HeaderText = "Zip";
                    dataGridView1.Columns[4].HeaderText = "Type";
                    dataGridView1.Columns[5].HeaderText = "Stocks";

                    string oth = "";
                    string[] name = new string[6];
                    for (int j = 0; j < name.Length; j++)
                    {
                        name[j] = "";
                    }
                    if (textBox1.Text.Length != 0)
                    {
                        name[0] = " id ='" + textBox1.Text.Trim() + "'";
                    }

                    if (textBox2.Text.Length != 0)
                    {
                        //name[1] = " warename like'%" + textBox2.Text + "%'";
                        name[1] = " price >=" + textBox2.Text;
                    }

                    if (textBox3.Text.Length != 0)
                    {
                        name[2] = " price <=" + textBox3.Text;
                    }

                    if (textBox4.Text.Length != 0)
                    {
                        //name[3] = " zip ='" + textBox4.Text + "'";
                        name[3] = " warename like'%" + textBox4.Text + "%'";
                    }
                    if (textBox5.Text.Length != 0)
                    {
                        name[4] = " type ='" + textBox5.Text + "'";
                    }
                    if (textBox6.Text.Length != 0)
                    {
                        //name[5] = " stocks ='" + textBox6.Text + "'";
                        name[5] = " zip ='" + textBox6.Text + "'";
                    }
                    for (int i = 0; i < name.Length; i++)
                    {
                        if (name[i].Length != 0)
                        {
                            str += name[i] + "and";
                        }
                    }
                    if (str == "select id,warename,price,type,zip from ware where ")
                    {
                        oth = "select id,warename,price,type,zip from ware";
                        MessageBox.Show("不能为空!");
                        return;
                    }
                    else
                    {
                        oth = str.Substring(0, str.Length - 3);
                    }

                    //MessageBox.Show(oth);
                    da = new SqlDataAdapter(oth, con);
                    ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("没有查询结果", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    dataGridView1.DataSource = ds.Tables[0];
                    setFridViewProperty();
                }
                else
                {
                    MessageBox.Show("除名称外只能是数字");
                }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            x = e.RowIndex;
            y = e.ColumnIndex;

            DLbh bbhh = DLbh.getIns();
            string ssss = dataGridView1.Rows[x].Cells[0].Value.ToString();
            bbhh.Num = int.Parse(ssss);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Update u = new Update();
            u.evt += new Update.dele(LoadDGV);
            u.ShowDialog();
        }
       

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            LoadDGV();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            String id = dataGridView1.Rows[x].Cells[0].Value.ToString();
            String stock = dataGridView1.Rows[x].Cells[5].Value.ToString();
            if (int.Parse(stock) == 0)
            {
                MessageBox.Show("Inventory is empty. Please select others.");
            }
            else 
            {
                string name = DLname.getInstance().Str;
                string command = "select uReservation from userlogin where uName =" + name;

                DBTools db = new DBTools(".", "wkdl", true, "sa", "wdxg");
                SqlDataReader b = db.getResult("select * from userlogin where uName ='" + name + "' and uReservation != ''");
                //MessageBox.Show(b.Read().ToString());
                if (b.Read())
                {
                    MessageBox.Show("Sorry! Each member can only reserve one vechicle.", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    db.Disconnect();
                    return;
                }
                db.Disconnect();
                string update = "update ware set stocks = " + (int.Parse(stock) - 1) + " where [id]=" + id;
                string reserve = "; update userlogin set uReservation = 'Model: " + dataGridView1.Rows[x].Cells[1].Value + ", Price: $" + dataGridView1.Rows[x].Cells[2].Value + "/mouth, Zip: " + dataGridView1.Rows[x].Cells[3].Value + ", Reserve time: " + DateTime.Now.ToLocalTime() +"' where [uName] = '" + DLname.getInstance().Str + "'";
                da.InsertCommand = con.CreateCommand();
                da.InsertCommand.CommandText = update + reserve;
                //MessageBox.Show(update + reserve);
                con.Open();
                da.InsertCommand.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Reserve successfully, please wait for agent response.");
                LoadDGV();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}