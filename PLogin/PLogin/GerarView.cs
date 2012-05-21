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
    public partial class GerarView : Form
    {
        private List<SqlConnection> con;

        public GerarView(List<SqlConnection> con)
        {
            InitializeComponent();
            this.con = con;

        }

        private void GerarView_Load(object sender, EventArgs e)
        {
            string gettabelas = "Select * from sys.Tables";
            SqlCommand instrucao = new SqlCommand(gettabelas, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
            SqlDataReader myReader;

            try
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                myReader = instrucao.ExecuteReader();

                while (myReader.Read())
                {
                    cbTabClientes.Items.Add(myReader.GetString(0));
                    cbTabArtigos.Items.Add(myReader.GetString(0));
                    cbTabVendas.Items.Add(myReader.GetString(0));
                }
                myReader.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
            }

            con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Open();
            SqlCommand cli = new SqlCommand("SELECT * FROM CLIENTES", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));
            SqlCommand art = new SqlCommand("SELECT * FROM ARTIGOS", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));
            SqlCommand vnd = new SqlCommand("SELECT * FROM VENDAS", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));

            SqlDataReader reader = cli.ExecuteReader();

            reader.Read();
            if (reader.HasRows == true)
            {
                cbTabClientes.SelectedItem = reader.GetString(14);

                cbTCIDCliente.SelectedItem = reader.GetString(0);
                cbTCNomeCli.SelectedItem = reader.GetString(1);
                cbTCAbrvCli.SelectedItem = reader.GetString(2);
                cbTCNivel1.SelectedItem = reader.GetString(3);
                cbTCNivel2.SelectedItem = reader.GetString(4);
                cbTCNivel3.SelectedItem = reader.GetString(5);
                cbTCNivel4.SelectedItem = reader.GetString(6);
                cbTCNivel5.SelectedItem = reader.GetString(7);
                tbTCDescr1.Text = reader.GetString(8);
                tbTCDescr2.Text = reader.GetString(9);
                tbTCDescr3.Text = reader.GetString(10);
                tbTCDescr4.Text = reader.GetString(11);
                tbTCDescr5.Text = reader.GetString(12);
                tbTCWhere.Text = reader.GetString(13);
            }
            reader.Close();
            reader = art.ExecuteReader();
            reader.Read();
            if (reader.HasRows == true)
            {
                cbTabArtigos.SelectedItem = reader.GetString(13);

                cbTARef.SelectedItem = reader.GetString(0);
                cbTAdesgn.SelectedItem = reader.GetString(1);
                cbTANivel1.SelectedItem = reader.GetString(2);
                cbTANivel2.SelectedItem = reader.GetString(3);
                cbTANivel3.SelectedItem = reader.GetString(4);
                cbTANivel4.SelectedItem = reader.GetString(5);
                cbTANivel5.SelectedItem = reader.GetString(6);
                tbTADescr1.Text = reader.GetString(7);
                tbTADescr2.Text = reader.GetString(8);
                tbTADescr3.Text = reader.GetString(9);
                tbTADescr4.Text = reader.GetString(10);
                tbTADescr5.Text = reader.GetString(11);
                tbTAWhere.Text = reader.GetString(12);
            }
            reader.Close();
            reader = vnd.ExecuteReader();
            reader.Read();
            if (reader.HasRows == true)
            {
                cbTabVendas.SelectedItem = reader.GetString(16);

                cbTVIDCli.SelectedItem = reader.GetString(0);
                cbTVData.SelectedItem = reader.GetString(1);
                cbTVRef.SelectedItem = reader.GetString(2);
                cbTVQtd.SelectedItem = reader.GetString(3);
                cbTVValLiq.SelectedItem = reader.GetString(4);
                cbTVNivel1.SelectedItem = reader.GetString(5);
                cbTVNivel2.SelectedItem = reader.GetString(6);
                cbTVNivel3.SelectedItem = reader.GetString(7);
                cbTVNivel4.SelectedItem = reader.GetString(8);
                cbTVNivel5.SelectedItem = reader.GetString(9);
                tbTVDescr1.Text = reader.GetString(10);
                tbTVDescr2.Text = reader.GetString(11);
                tbTVDescr3.Text = reader.GetString(12);
                tbTVDescr4.Text = reader.GetString(13);
                tbTVDescr5.Text = reader.GetString(14);
                tbTVWhere.Text = reader.GetString(15);
            }
            reader.Close();


            con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Close();

        }

        

        private void btSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btGerarView_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "Clientes")
            {
                vClientes();
            }

            if (tabControl1.SelectedTab.Text == "Artigos")
            {
                vArtigos();
            }

            if (tabControl1.SelectedTab.Text == "Vendas")
            {
                vVendas();
            }
                    
                    
           
              
        }


        private void btCopyClipb_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(tbVIEW.Text, TextDataFormat.Text);
            }
            catch { }
        }

        private void vClientes()
        {
            if ((cbTabClientes.SelectedItem == null) ||(cbTCIDCliente.SelectedItem == null) || (cbTCAbrvCli.SelectedItem == null) || (cbTCNomeCli.SelectedItem == null))
            {
                MessageBox.Show("Os campos Tabela, ID Cliente, Nome Cliente e Abreviatura são obrigatórios!");
                return;
            }

            
            string nivel1 = "";
            string nivel2 = "";
            string nivel3 = "";
            string nivel4 = "";
            string nivel5 = "";
            string final = "";

            try
            {
                
                nivel1 = (tbTCDescr1.Text.CompareTo("") != 0) ? tbTCDescr1.Text : cbTCNivel1.SelectedItem.ToString();
                nivel2 = (tbTCDescr2.Text.CompareTo("") != 0) ? tbTCDescr2.Text : cbTCNivel2.SelectedItem.ToString();
                nivel3 = (tbTCDescr3.Text.CompareTo("") != 0) ? tbTCDescr3.Text : cbTCNivel3.SelectedItem.ToString();
                nivel4 = (tbTCDescr4.Text.CompareTo("") != 0) ? tbTCDescr4.Text : cbTCNivel4.SelectedItem.ToString();
                nivel5 = (tbTCDescr5.Text.CompareTo("") != 0) ? tbTCDescr5.Text : cbTCNivel5.SelectedItem.ToString();
            }

            catch { }

            final += cbTCIDCliente.SelectedItem.ToString() + " AS 'Nr Cliente', ";
            final += cbTCNomeCli.SelectedItem.ToString() + " AS 'Nome Cliente', ";
            final += cbTCAbrvCli.SelectedItem.ToString() + " AS 'Abreviatura', ";

            if (nivel1.CompareTo("") != 0)
            {
                final += cbTCNivel1.SelectedItem.ToString() + " AS '" + nivel1 + "', ";
            }

            if (nivel2.CompareTo("") != 0)
            {
                final += cbTCNivel2.SelectedItem.ToString() + " AS '" + nivel2 + "', ";
            }

            if (nivel3.CompareTo("") != 0)
            {
                final += cbTCNivel3.SelectedItem.ToString() + " AS '" + nivel3 + "', ";
            }

            if (nivel4.CompareTo("") != 0)
            {
                final += cbTCNivel4.SelectedItem.ToString() + " AS '" + nivel4 + "', ";
            }

            if (nivel5.CompareTo("") != 0)
            {
                final += cbTCNivel5.SelectedItem.ToString() + " AS '" + nivel5 + "', ";
            }

            
            final = final.Substring(0, final.Length - 2);

            


            String strView = "CREATE VIEW SAM_" + cbTabClientes.SelectedItem.ToString().ToUpper() + " AS " +
                "SELECT " + final + " FROM " + cbTabClientes.SelectedItem.ToString();


            if (tbTCWhere.Text.CompareTo("") != 0)
            {
                strView += " WHERE " + tbTCWhere.Text;
            }

            tbVIEW.Text = strView;



            try
            {

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                SqlCommand setNoExecON = new SqlCommand("SET NOEXEC ON", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                setNoExecON.ExecuteNonQuery();

                SqlCommand instrucao = new SqlCommand(tbVIEW.Text, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

                instrucao.ExecuteNonQuery();

                SqlCommand setNoExecOFF = new SqlCommand("SET NOEXEC OFF", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                setNoExecON.ExecuteNonQuery();

                btGravar.Enabled = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
            }
          
        }

        private void vArtigos()
        {
            if((cbTabArtigos.SelectedItem == null) || (cbTARef.SelectedItem == null) || (cbTAdesgn.SelectedItem == null))
            {
                MessageBox.Show("Os campos Tabela, Referência e Designação são obrigatórios!");
                return;
            }

            string nivel1 = "";
            string nivel2 = "";
            string nivel3 = "";
            string nivel4 = "";
            string nivel5 = "";
            string final = "";

            try
            {
                nivel1 = (tbTADescr1.Text.CompareTo("") != 0) ? tbTADescr1.Text : cbTANivel1.SelectedItem.ToString();
                nivel2 = (tbTADescr2.Text.CompareTo("") != 0) ? tbTADescr2.Text : cbTANivel2.SelectedItem.ToString();
                nivel3 = (tbTADescr3.Text.CompareTo("") != 0) ? tbTADescr3.Text : cbTANivel3.SelectedItem.ToString();
                nivel4 = (tbTADescr4.Text.CompareTo("") != 0) ? tbTADescr4.Text : cbTANivel4.SelectedItem.ToString();
                nivel5 = (tbTADescr5.Text.CompareTo("") != 0) ? tbTADescr5.Text : cbTANivel5.SelectedItem.ToString();
            }

            catch { }

            final += cbTARef.SelectedItem.ToString() + " AS 'Referência', ";
            final += cbTAdesgn.SelectedItem.ToString() + " AS 'Designação', ";

            if (nivel1.CompareTo("") != 0)
            {
                final += cbTANivel1.SelectedItem.ToString() + " AS '" + nivel1 + "', ";
            }

            if (nivel2.CompareTo("") != 0)
            {
                final += cbTANivel2.SelectedItem.ToString() + " AS '" + nivel2 + "', ";
            }

            if (nivel3.CompareTo("") != 0)
            {
                final += cbTANivel3.SelectedItem.ToString() + " AS '" + nivel3 + "', ";
            }

            if (nivel4.CompareTo("") != 0)
            {
                final += cbTANivel4.SelectedItem.ToString() + " AS '" + nivel4 + "', ";
            }

            if (nivel5.CompareTo("") != 0)
            {
                final += cbTANivel5.SelectedItem.ToString() + " AS '" + nivel5 + "', ";
            }

            
            final = final.Substring(0, final.Length - 2);


            String strView = "CREATE VIEW SAM_" + cbTabArtigos.SelectedItem.ToString().ToUpper() + " AS " +
                "SELECT " + final + " FROM " + cbTabArtigos.SelectedItem.ToString();

            if (tbTAWhere.Text.CompareTo("") != 0)
            {
                strView += " WHERE " + tbTAWhere.Text;
            }


            tbVIEW.Text = strView;

            try
            {

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                SqlCommand setNoExecON = new SqlCommand("SET NOEXEC ON", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                setNoExecON.ExecuteNonQuery();

                SqlCommand instrucao = new SqlCommand(tbVIEW.Text, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

                instrucao.ExecuteNonQuery();

                SqlCommand setNoExecOFF = new SqlCommand("SET NOEXEC OFF", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                setNoExecON.ExecuteNonQuery();

                btGravar.Enabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
            }
            
        }

        private void vVendas()
        {
            if ((cbTabVendas.SelectedItem == null) || (cbTVRef.SelectedItem == null) || (cbTVData.SelectedItem == null) || (cbTVIDCli.SelectedItem == null) || (cbTVQtd.SelectedItem == null) || (cbTVValLiq.SelectedItem == null))
            {
                MessageBox.Show("Os campos Tabela, ID Cliente, Data, Referência, Quantidade e Valor Líquido são obrigatórios!");
                return;
            }

            string nivel1 = "";
            string nivel2 = "";
            string nivel3 = "";
            string nivel4 = "";
            string nivel5 = "";
            string final = "";

            try
            {
                nivel1 = (tbTVDescr1.Text.CompareTo("") != 0) ? tbTVDescr1.Text : cbTVNivel1.SelectedItem.ToString();
                nivel2 = (tbTVDescr2.Text.CompareTo("") != 0) ? tbTVDescr2.Text : cbTVNivel2.SelectedItem.ToString();
                nivel3 = (tbTVDescr3.Text.CompareTo("") != 0) ? tbTVDescr3.Text : cbTVNivel3.SelectedItem.ToString();
                nivel4 = (tbTVDescr4.Text.CompareTo("") != 0) ? tbTVDescr4.Text : cbTVNivel4.SelectedItem.ToString();
                nivel5 = (tbTVDescr5.Text.CompareTo("") != 0) ? tbTVDescr5.Text : cbTVNivel5.SelectedItem.ToString();
            }

            catch { }

            final += cbTVIDCli.SelectedItem.ToString() + " AS 'Nr Cliente', ";
            final += cbTVData.SelectedItem.ToString() + " AS 'Data', ";
            final += cbTVRef.SelectedItem.ToString() + " AS 'Referência', ";
            final += cbTVQtd.SelectedItem.ToString() + " AS 'Quantidade', ";
            final += cbTVValLiq.SelectedItem.ToString() + " AS 'Valor Líquido', ";
            

            if (nivel1.CompareTo("") != 0)
            {
                final += cbTVNivel1.SelectedItem.ToString() + " AS '" + nivel1 + "', ";
            }

            if (nivel2.CompareTo("") != 0)
            {
                final += cbTVNivel2.SelectedItem.ToString() + " AS '" + nivel2 + "', ";
            }

            if (nivel3.CompareTo("") != 0)
            {
                final += cbTVNivel3.SelectedItem.ToString() + " AS '" + nivel3 + "', ";
            }

            if (nivel4.CompareTo("") != 0)
            {
                final += cbTVNivel4.SelectedItem.ToString() + " AS '" + nivel4 + "', ";
            }

            if (nivel5.CompareTo("") != 0)
            {
                final += cbTVNivel5.SelectedItem.ToString() + " AS '" + nivel5 + "', ";
            }


            final = final.Substring(0, final.Length - 2);


            String strView = "CREATE VIEW SAM_" + cbTabVendas.SelectedItem.ToString().ToUpper() + " AS " +
                "SELECT " + final + " FROM " + cbTabVendas.SelectedItem.ToString();

            if (tbTVWhere.Text.CompareTo("") != 0)
            {
                strView += " WHERE " + tbTVWhere.Text;
            }

            tbVIEW.Text = strView;

            try
            {

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                SqlCommand setNoExecON = new SqlCommand("SET NOEXEC ON", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                setNoExecON.ExecuteNonQuery();

                SqlCommand instrucao = new SqlCommand(tbVIEW.Text, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

                instrucao.ExecuteNonQuery();

                SqlCommand setNoExecOFF = new SqlCommand("SET NOEXEC OFF", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                setNoExecON.ExecuteNonQuery();

                btGravar.Enabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();
            }
        }

        private void cbTabClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTCNivel1.Items.Clear();
            cbTCNivel2.Items.Clear();
            cbTCNivel3.Items.Clear();
            cbTCNivel4.Items.Clear();
            cbTCNivel5.Items.Clear();
            cbTCIDCliente.Items.Clear();
            cbTCAbrvCli.Items.Clear();
            cbTCNomeCli.Items.Clear();
            string getcampos = "SELECT TOP 0 * FROM " + cbTabClientes.Text;
            SqlCommand instrucao = new SqlCommand(getcampos, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
            SqlDataReader myReader;

            try
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();
                myReader = instrucao.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(myReader);


                foreach (DataColumn column in dt.Columns)
                {

                    cbTCNivel1.Items.Add(column.ColumnName);
                    cbTCNivel2.Items.Add(column.ColumnName);
                    cbTCNivel3.Items.Add(column.ColumnName);
                    cbTCNivel4.Items.Add(column.ColumnName);
                    cbTCNivel5.Items.Add(column.ColumnName);
                    cbTCIDCliente.Items.Add(column.ColumnName);
                    cbTCAbrvCli.Items.Add(column.ColumnName);
                    cbTCNomeCli.Items.Add(column.ColumnName);
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

        private void cbTabArtigos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTANivel1.Items.Clear();
            cbTANivel2.Items.Clear();
            cbTANivel3.Items.Clear();
            cbTANivel4.Items.Clear();
            cbTANivel5.Items.Clear();
            cbTAdesgn.Items.Clear();
            cbTARef.Items.Clear();
            string getcampos = "SELECT TOP 0 * FROM " + cbTabArtigos.Text;
            SqlCommand instrucao = new SqlCommand(getcampos, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
            SqlDataReader myReader;

            try
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();
                myReader = instrucao.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(myReader);


                foreach (DataColumn column in dt.Columns)
                {

                    cbTANivel1.Items.Add(column.ColumnName);
                    cbTANivel2.Items.Add(column.ColumnName);
                    cbTANivel3.Items.Add(column.ColumnName);
                    cbTANivel4.Items.Add(column.ColumnName);
                    cbTANivel5.Items.Add(column.ColumnName);
                    cbTARef.Items.Add(column.ColumnName);
                    cbTAdesgn.Items.Add(column.ColumnName);
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

        private void cbTabVendas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTVNivel1.Items.Clear();
            cbTVNivel2.Items.Clear();
            cbTVNivel3.Items.Clear();
            cbTVNivel4.Items.Clear();
            cbTVNivel5.Items.Clear();
            cbTVData.Items.Clear();
            cbTVIDCli.Items.Clear();
            cbTVQtd.Items.Clear();
            cbTVRef.Items.Clear();
            cbTVValLiq.Items.Clear();
            string getcampos = "SELECT TOP 0 * FROM " + cbTabVendas.Text;
            SqlCommand instrucao = new SqlCommand(getcampos, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
            SqlDataReader myReader;

            try
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();
                myReader = instrucao.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(myReader);


                foreach (DataColumn column in dt.Columns)
                {

                    cbTVNivel1.Items.Add(column.ColumnName);
                    cbTVNivel2.Items.Add(column.ColumnName);
                    cbTVNivel3.Items.Add(column.ColumnName);
                    cbTVNivel4.Items.Add(column.ColumnName);
                    cbTVNivel5.Items.Add(column.ColumnName);
                    cbTVData.Items.Add(column.ColumnName);
                    cbTVIDCli.Items.Add(column.ColumnName);
                    cbTVQtd.Items.Add(column.ColumnName);
                    cbTVRef.Items.Add(column.ColumnName);
                    cbTVValLiq.Items.Add(column.ColumnName);
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

        private void btGravar_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedTab.Text == "Clientes")
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                SqlCommand dropview = new SqlCommand("IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'SAM_" + cbTabClientes.Text.ToUpper() + "') DROP VIEW SAM_" + cbTabClientes.SelectedItem.ToString(), con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                dropview.ExecuteNonQuery();

                SqlCommand instrucao = new SqlCommand(tbVIEW.Text, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

                instrucao.ExecuteNonQuery();

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Open();

                SqlCommand apagar = new SqlCommand("DELETE FROM Clientes", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));
                apagar.ExecuteNonQuery();

                string strinserir = "INSERT INTO Clientes VALUES ('" + cbTCIDCliente.SelectedItem + "', '" + cbTCNomeCli.SelectedItem + "', '" + cbTCAbrvCli.SelectedItem + "', '" + cbTCNivel1.SelectedItem + "', '" + cbTCNivel2.SelectedItem + "', '" + cbTCNivel3.SelectedItem + "', '" + cbTCNivel4.SelectedItem + "', '" + cbTCNivel5.SelectedItem + "', @n1desc, @n2desc, @n3desc, @n4desc, @n5desc, @Where, '" + cbTabClientes.SelectedItem + "')";

                using (SqlCommand inserir = new SqlCommand(strinserir, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
                {
                    inserir.Parameters.Add(new SqlParameter("n1desc", tbTCDescr1.Text));
                    inserir.Parameters.Add(new SqlParameter("n2desc", tbTCDescr2.Text));
                    inserir.Parameters.Add(new SqlParameter("n3desc", tbTCDescr3.Text));
                    inserir.Parameters.Add(new SqlParameter("n4desc", tbTCDescr4.Text));
                    inserir.Parameters.Add(new SqlParameter("n5desc", tbTCDescr5.Text));
                    inserir.Parameters.Add(new SqlParameter("Where", tbTCWhere.Text));
                    inserir.ExecuteNonQuery();

                }
                string nome = "";

                SqlCommand cmdNome = new SqlCommand("SELECT SYSTEM_USER", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));

                SqlDataReader myReader;
                myReader = cmdNome.ExecuteReader();

                while (myReader.Read())
                {
                    nome = myReader[""].ToString();
                }

                myReader.Close();

                using (SqlCommand cmdLogIn = new SqlCommand("INSERT INTO Log VALUES (@data, '" + nome + "', 'View com o nome SAM_" + cbTabClientes.Text.ToUpper() + " criada')", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
                {
                    cmdLogIn.Parameters.Add(new SqlParameter("data", DateTime.Now));
                    cmdLogIn.ExecuteNonQuery();
                }

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Close();
            }

            if (tabControl1.SelectedTab.Text == "Artigos")
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                SqlCommand dropview = new SqlCommand("IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'SAM_" + cbTabArtigos.Text.ToUpper() + "') DROP VIEW SAM_" + cbTabArtigos.SelectedItem.ToString(), con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                dropview.ExecuteNonQuery();

                SqlCommand instrucao = new SqlCommand(tbVIEW.Text, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

                instrucao.ExecuteNonQuery();

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Open();

                SqlCommand apagar = new SqlCommand("DELETE FROM Artigos", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));
                apagar.ExecuteNonQuery();

                string strinserir = "INSERT INTO Artigos VALUES ('" + cbTARef.SelectedItem + "', '" + cbTAdesgn.SelectedItem + "', '" + cbTANivel1.SelectedItem + "', '" + cbTANivel2.SelectedItem + "', '" + cbTANivel3.SelectedItem + "', '" + cbTANivel4.SelectedItem + "', '" + cbTANivel5.SelectedItem + "', @n1desc, @n2desc, @n3desc, @n4desc, @n5desc, @Where, '" + cbTabArtigos.SelectedItem + "')";

                using (SqlCommand inserir = new SqlCommand(strinserir, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
                {
                    inserir.Parameters.Add(new SqlParameter("n1desc", tbTADescr1.Text));
                    inserir.Parameters.Add(new SqlParameter("n2desc", tbTADescr2.Text));
                    inserir.Parameters.Add(new SqlParameter("n3desc", tbTADescr3.Text));
                    inserir.Parameters.Add(new SqlParameter("n4desc", tbTADescr4.Text));
                    inserir.Parameters.Add(new SqlParameter("n5desc", tbTADescr5.Text));
                    inserir.Parameters.Add(new SqlParameter("Where", tbTAWhere.Text));
                    inserir.ExecuteNonQuery();

                }


                string nome = "";

                SqlCommand cmdNome = new SqlCommand("SELECT SYSTEM_USER", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));

                SqlDataReader myReader;
                myReader = cmdNome.ExecuteReader();

                while (myReader.Read())
                {
                    nome = myReader[""].ToString();
                }

                myReader.Close();

                using (SqlCommand cmdLogIn = new SqlCommand("INSERT INTO Log VALUES (@data, '" + nome + "', 'View com o nome SAM_" + cbTabArtigos.Text.ToUpper() + " criada')", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
                {
                    cmdLogIn.Parameters.Add(new SqlParameter("data", DateTime.Now));
                    cmdLogIn.ExecuteNonQuery();
                }

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Close();
            }

            if (tabControl1.SelectedTab.Text == "Vendas")
            {
                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Open();

                SqlCommand dropview = new SqlCommand("IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'SAM_" + cbTabVendas.Text.ToUpper() + "') DROP VIEW SAM_" + cbTabVendas.SelectedItem.ToString(), con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));
                dropview.ExecuteNonQuery();

                SqlCommand instrucao = new SqlCommand(tbVIEW.Text, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]));

                instrucao.ExecuteNonQuery();

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD1"]).Close();

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Open();

                SqlCommand apagar = new SqlCommand("DELETE FROM Vendas", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));
                apagar.ExecuteNonQuery();

                string strinserir = "INSERT INTO Vendas VALUES ('" + cbTVIDCli.SelectedItem + "', '" + cbTVData.SelectedItem + "', '" + cbTVRef.SelectedItem + "', '" + cbTVQtd.SelectedItem + "', '" + cbTVValLiq.SelectedItem + "', '" + cbTCNivel1.SelectedItem + "', '" + cbTCNivel2.SelectedItem + "', '" + cbTCNivel3.SelectedItem + "', '" + cbTCNivel4.SelectedItem + "', '" + cbTCNivel5.SelectedItem + "', @n1desc, @n2desc, @n3desc, @n4desc, @n5desc, @Where, '" + cbTabVendas.SelectedItem + "')";

                using (SqlCommand inserir = new SqlCommand(strinserir, con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
                {
                    inserir.Parameters.Add(new SqlParameter("n1desc", tbTVDescr1.Text));
                    inserir.Parameters.Add(new SqlParameter("n2desc", tbTVDescr2.Text));
                    inserir.Parameters.Add(new SqlParameter("n3desc", tbTVDescr3.Text));
                    inserir.Parameters.Add(new SqlParameter("n4desc", tbTVDescr4.Text));
                    inserir.Parameters.Add(new SqlParameter("n5desc", tbTVDescr5.Text));
                    inserir.Parameters.Add(new SqlParameter("Where", tbTVWhere.Text));
                    inserir.ExecuteNonQuery();

                }


                string nome = "";

                SqlCommand cmdNome = new SqlCommand("SELECT SYSTEM_USER", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]));

                SqlDataReader myReader;
                myReader = cmdNome.ExecuteReader();

                while (myReader.Read())
                {
                    nome = myReader[""].ToString();
                }

                myReader.Close();

                using (SqlCommand cmdLogIn = new SqlCommand("INSERT INTO Log VALUES (@data, '" + nome + "', 'View com o nome SAM_" + cbTabVendas.Text.ToUpper() + " criada')", con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"])))
                {
                    cmdLogIn.Parameters.Add(new SqlParameter("data", DateTime.Now));
                    cmdLogIn.ExecuteNonQuery();
                }

                con.Find(x => x.Database == ConfigurationManager.AppSettings["BD2"]).Close();
            }

            btGravar.Enabled = false;
            tbVIEW.Text = "";
        }
    }
}
