using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ConsultaCnpj
{
    public partial class frmEmpresas : Form
    {
        string servidor, banco;

        // Método para carregar o DataGridView
        private void CarregarDados()
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection($"Server={servidor};Database={banco};Trusted_Connection=True;"))
                {
                    string query = "SELECT id AS ID, " +
                        "cnpj AS CNPJ," +
                        "nomeEmpresarial AS \"Nome Empresarial\"," +
                        "nomeFantasia AS \"Nome Fantasia\"," +
                        "CONCAT(endereco, ' Nº', numEndereco) AS \"Endereço\", " +
                        "complemento AS Complemento," +
                        "bairro AS Bairro," +
                        "cidade AS Cidade," +
                        "cep AS CEP," +
                        "dataAbertura AS \"Data Abertura\" " +
                        "FROM empresa;";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDados.DataSource = dt;
                    dgvDados.Columns["ID"].Visible = false;
                    // dgvDados.Columns["Nome Empresarial"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    
                    foreach (DataGridViewColumn item in dgvDados.Columns)
                    {
                        cmbFiltro.Items.Add(item.HeaderText);
                    }

                    cmbFiltro.Items.RemoveAt(0);
                    cmbFiltro.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para remover o registro do banco de dados
        private void ExcluirRegistro(int id)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection($"Server={servidor};Database={banco};Trusted_Connection=True;"))
                {
                    string query = "DELETE FROM empresa WHERE id=@id;";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@id", id);
                    conexao.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        CarregarDados();
                        MessageBox.Show("Registro excluído com sucesso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);                                                
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

        public frmEmpresas()
        {
            InitializeComponent();
        }

        private void frmEmpresas_Load(object sender, EventArgs e)
        {            
            servidor = Properties.Settings.Default.strServidor;
            banco = Properties.Settings.Default.strBancoDados;

            if (string.IsNullOrEmpty(servidor) || string.IsNullOrEmpty(banco))
            {
                MessageBox.Show("Banco de dados não configurado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            else
            {
                CarregarDados();
            }            
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Confirma a exlusão do registro selecionado?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ExcluirRegistro((int)dgvDados.SelectedCells[0].Value);
                }                
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            (dgvDados.DataSource as DataTable).DefaultView.RowFilter =
                string.Format("Convert([{0}], 'System.String') LIKE '%{1}%'", cmbFiltro.Text, txtFiltro.Text);                       
        }

    }
}
