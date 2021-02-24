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
    public partial class frmEmpresas : Form
    {
        private void CarregarDados()
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=cadastro;Trusted_Connection=True;"))
                {
                    string query = "SELECT * FROM EMPRESA;";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDados.DataSource = dt;

                    foreach (DataGridViewColumn item in dgvDados.Columns)
                    {
                        cmbFiltro.Items.Add(item.HeaderText);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }

        public frmEmpresas()
        {
            InitializeComponent();
        }

        private void frmEmpresas_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }
    }
}
