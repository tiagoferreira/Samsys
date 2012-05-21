using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace PLogin
{
    public partial class QAnalyzer : Form
    {
        private List<SqlConnection> con;

        public QAnalyzer(List<SqlConnection> conn)
        {
            InitializeComponent();
            con = conn;
        }

        private void btSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btExecutar_Click(object sender, EventArgs e)
        {
            string query = tbQuery.Text.ToLower().Trim();

            if (query == "")
            {
                lbInfo.ForeColor = System.Drawing.Color.White;
                lbInfo.Text = "Introduza uma instrução SQL!";
            }
            else
            {
                string[] instr = query.Split(' ');

                if (instr[0] != "select")
                {
                    lbInfo.ForeColor = System.Drawing.Color.Red;
                    lbInfo.Text = "Não permitido!";
                }
                else
                {
                    limparDataGridView();
                    executaQuery(query);
                }
            }
            
        }

        private void executaQuery(string query)
        {
            SqlCommand instrucao = new SqlCommand(query, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
            SqlDataReader myReader;

            try
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                myReader = instrucao.ExecuteReader();
                BindingSource bSource = new BindingSource();
                bSource.DataSource = myReader;

                if (myReader.HasRows == false)
                {
                    lbInfo.ForeColor = System.Drawing.Color.White;
                    lbInfo.Text = "Query sem resultados!";

                }
                else
                {
                    dataGridView1.DataSource = bSource;

                    lbInfo.ForeColor = System.Drawing.Color.Green;
                    lbInfo.Text = "Query executado com sucesso!";
                }
                
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

        private void btLimpar_Click(object sender, EventArgs e)
        {
            tbQuery.Clear();
            limparDataGridView();
            lbInfo.Text = "";
        }

        private void limparDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();  
        }

        private void tbQuery_MouseClick(object sender, MouseEventArgs e)
        {
            lbInfo.Text = "";
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("..\\log.txt");
            sw.WriteLine(tbQuery.Text);
            sw.Close();
        }

    }
}
