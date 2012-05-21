using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using System.Configuration;



namespace PLogin
{
    
    
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbUser.Enabled = true;
            tbPassword.Enabled = true;
            cbAuthentication.SelectedIndex = 1;
        }

        private void cbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAuthentication.SelectedIndex == 1)
            {
                tbUser.Clear();
                tbUser.Enabled = true;
                tbPassword.Clear();
                tbPassword.Enabled = true;
            }
            else
            {
                tbUser.Text = WindowsIdentity.GetCurrent().Name;
                tbUser.Enabled = false;
                tbPassword.Clear();
                tbPassword.Enabled = false;
            }
        }

        private List<SqlConnection> CreateCon(int i)
        {
            List<SqlConnection> con = new List<SqlConnection>();
            
            if (i == 1)
            {
                string user = tbUser.Text.Trim();
                string password = tbPassword.Text.Trim();
                string server = ".\\SQLEXPRESS";

                string constr = "user id=" + user + ";" +
                    "password=" + password + "; server=" + server + ";" +
                    "Trusted_Connection=no;" +
                    "database=" + ConfigurationManager.AppSettings["BD1"] + "; " +
                    "connection timeout=30";

                SqlConnection conBD = new SqlConnection(constr);

                

                con.Add(conBD);

                constr = "user id=" + user + ";" +
                    "password=" + password + "; server=" + server + ";" +
                    "Trusted_Connection=no;" +
                    "database=" + ConfigurationManager.AppSettings["BD2"] + "; " +
                    "connection timeout=30";

                SqlConnection conLog = new SqlConnection(constr);

                con.Add(conLog);

                return con;
            }
            if (i == 0)
            {
                string server = ".\\SQLEXPRESS";

                string constr = "server=" + server + "; Trusted_Connection=yes; database=" + ConfigurationManager.AppSettings["BD1"] + "; connection timeout=30";

                SqlConnection conBD = new SqlConnection(constr);

                con.Add(conBD);

                constr = "server=" + server + "; Trusted_Connection=yes; database=" + ConfigurationManager.AppSettings["BD2"] + "; connection timeout=30";

                SqlConnection conLog = new SqlConnection(constr);

                con.Add(conLog);

                return con;
            }
            return null;

        }


        private void btLogin_Click(object sender, EventArgs e)
        {
            List<SqlConnection> con = this.CreateCon(cbAuthentication.SelectedIndex);
            
            try
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
                Integrador fdois = new Integrador(con);
                fdois.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
                
            }
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.InvokeOnClick(btLogin, EventArgs.Empty);
            }
        }


        

    }
}
