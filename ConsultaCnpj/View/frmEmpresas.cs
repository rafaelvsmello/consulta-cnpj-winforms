﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

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
                    string query = "SELECT cnpj AS CNPJ," +
                        "nomeEmpresarial AS \"Nome Empresarial\"," +
                        "nomeFantasia AS \"Nome Fantasia\"," +
                        "endereco AS \"Endereço\"," +
                        "complemento AS Complemento," +
                        "bairro AS Bairro," +
                        "cidade AS Cidade," +
                        "cep AS CEP," +
                        "dataAbertura AS \"Data Abertura\" " +
                        "FROM EMPRESA;";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDados.DataSource = dt;                    

                    foreach (DataGridViewColumn item in dgvDados.Columns)
                    {
                        cmbFiltro.Items.Add(item.HeaderText);
                    }

                    cmbFiltro.SelectedIndex = 0;
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

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (dgvDados.Rows.Count > 0)
            {
                (dgvDados.DataSource as DataTable).DefaultView.RowFilter =
                string.Format("Convert([{0}], 'System.String') LIKE '%{1}%'", cmbFiltro.Text, txtFiltro.Text);
            }            
        }

    }
}
