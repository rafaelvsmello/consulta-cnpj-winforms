using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ConsultaCnpj
{
    public partial class frmBancoDados : Form
    {
        private void ExisteDb()
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection($"Server={txtServidor.Text};Trusted_Connection=True;Initial Catalog=master"))
                {
                    conexao.Open();
                    string query = "SELECT * FROM master.dbo.sysdatabases WHERE name = 'cadastro'";                                    
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CriarDb();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriarDb()
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection($"Server={txtServidor.Text};Trusted_Connection=True;Initial Catalog=master"))
                {
                    conexao.Open();
                    string query = "CREATE DATABASE cadastro;";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    int result = cmd.ExecuteNonQuery();                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public frmBancoDados()
        {
            InitializeComponent();
        }

        private void frmBancoDados_Load(object sender, EventArgs e)
        {
            txtServidor.Text = Properties.Settings.Default.strServidor;
            txtBancoDados.Text = Properties.Settings.Default.strBancoDados;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.strServidor = txtServidor.Text;
            Properties.Settings.Default.strBancoDados = txtBancoDados.Text;
            Properties.Settings.Default.Save();
            ExisteDb();
        }
    }
}
