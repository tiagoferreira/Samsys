using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace PLogin
{
    public partial class Integrador : Form
    {
        private List<SqlConnection> con;

        public Integrador(List<SqlConnection> conn)
        {
            InitializeComponent();
            con = conn;
        }

        private void Integrador_Load(object sender, EventArgs e)
        {
            string nome = "SELECT SYSTEM_USER";
            con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();
            SqlCommand cmdNome = new SqlCommand(nome, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

            SqlDataReader myReader;
            myReader = cmdNome.ExecuteReader();
            
            while (myReader.Read())
            {
                label1.Text += myReader[""].ToString();
                nome = myReader[""].ToString();
            }
            con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
            con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Open();

            using (SqlCommand cmdLogIn = new SqlCommand("INSERT INTO Log VALUES (@data, '" + nome + "', 'Log In')", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
            {
                cmdLogIn.Parameters.Add(new SqlParameter("data", DateTime.Now));
                cmdLogIn.ExecuteNonQuery();
            }



            con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Close();

        }

        private void Integrador_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void btMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }

        #region Menu principal
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        #region Menu notify icon
        private void sairToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1_DoubleClick(sender, e);
        }
        #endregion

        private void queryAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QAnalyzer query = new QAnalyzer(con);
            query.ShowDialog();
        }

        private void btSair_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }

        private void gerarViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GerarView gv = new GerarView(con);
            gv.ShowDialog();
        }

    }
}
