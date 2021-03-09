using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ConsultaCnpj
{
    public partial class frmPrincipal : System.Windows.Forms.Form
    {
        Empresa.Root Dados = new Empresa.Root();
        string servidor, banco;

        // Método para carregar os dados da API
        private void CarregarDados(string cnpj, string cnpjTratado)
        {
            try
            {
                var requisicao = WebRequest.CreateHttp("https://www.receitaws.com.br/v1/cnpj/" + cnpjTratado);
                requisicao.Method = "GET";
                requisicao.UserAgent = "RequisicaoEmpresa";

                using (var response = requisicao.GetResponse())
                {
                    var streamDados = response.GetResponseStream();
                    var streamReader = new StreamReader(streamDados);
                    var objResponse = streamReader.ReadToEnd();

                    var responseDeserialized = JsonConvert.DeserializeObject<Empresa.Root>(objResponse.ToString()); // Desserialização do objeto

                    Dados.cnpj = cnpjTratado;
                    Dados.situacao = responseDeserialized.situacao;
                    Dados.data_situacao = responseDeserialized.data_situacao;
                    Dados.nome = responseDeserialized.nome;
                    Dados.fantasia = responseDeserialized.fantasia;
                    Dados.logradouro = responseDeserialized.logradouro;
                    Dados.numero = responseDeserialized.numero;
                    Dados.complemento = responseDeserialized.complemento;
                    Dados.bairro = responseDeserialized.bairro;
                    Dados.municipio = responseDeserialized.municipio;
                    Dados.cep = responseDeserialized.cep;
                    Dados.abertura = responseDeserialized.abertura;

                    // Adiciona os dados no ListBox
                    lstDados.Items.Add("CNPJ: " + cnpj);
                    lstDados.Items.Add("Situação: " + Dados.situacao);
                    lstDados.Items.Add("Data situação: " + Dados.data_situacao);
                    lstDados.Items.Add("Nome Empresarial: " + Dados.nome);
                    lstDados.Items.Add(string.IsNullOrEmpty(Dados.fantasia) ? "SEM NOME FANTASIA" : "Nome Fantasia: " + Dados.fantasia);
                    lstDados.Items.Add("Endereço: " + Dados.logradouro + " Nº: " + Dados.numero);
                    lstDados.Items.Add(string.IsNullOrEmpty(Dados.complemento) ? "SEM COMPLEMENTO" : "Complemento: " + Dados.complemento);
                    lstDados.Items.Add("Bairro: " + Dados.bairro);
                    lstDados.Items.Add("Cidade: " + Dados.municipio);
                    lstDados.Items.Add("CEP: " + Dados.cep);
                    lstDados.Items.Add("Abertura: " + Dados.abertura);

                    streamDados.Close();
                    streamReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para gravar os dados no SQL Server
        private void GravarDados()
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection($"Server={servidor};Database={banco};Trusted_Connection=True;"))
                {
                    string query = "INSERT INTO empresa (cnpj, situacao, dataSituacao, nomeEmpresarial, nomeFantasia, endereco, complemento, bairro, cidade, cep, dataAbertura) VALUES (" +
                        "@cnpj, " +
                        "@situacao, " +
                        "@dataSituacao, " +
                        "@nomeEmpresarial, " +
                        "@nomeFantasia, " +
                        "@endereco, " +
                        "@complemento, " +
                        "@bairro, " +
                        "@cidade, " +
                        "@cep, " +
                        "@dataAbertura);";

                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@cnpj", Dados.cnpj);
                    cmd.Parameters.AddWithValue("@situacao", Dados.situacao);
                    cmd.Parameters.AddWithValue("@dataSituacao", Dados.data_situacao);
                    cmd.Parameters.AddWithValue("@nomeEmpresarial", Dados.nome);
                    cmd.Parameters.AddWithValue("@nomeFantasia", Dados.fantasia);
                    cmd.Parameters.AddWithValue("@endereco", Dados.logradouro);
                    cmd.Parameters.AddWithValue("@complemento", Dados.complemento);
                    cmd.Parameters.AddWithValue("@bairro", Dados.bairro);
                    cmd.Parameters.AddWithValue("@cidade", Dados.municipio);
                    cmd.Parameters.AddWithValue("@cep", Dados.cep);
                    cmd.Parameters.AddWithValue("@dataAbertura", Dados.abertura);

                    conexao.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Cadastro realizado com sucesso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpar();
                        txtCnpj.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpar()
        {
            txtCnpj.Text = string.Empty;
            lstDados.Items.Clear();
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCnpj.Text))
            {
                MessageBox.Show("Informe um CNPJ para pesquisar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                CarregarDados(txtCnpj.Text, txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", ""));
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(servidor) || string.IsNullOrEmpty(banco))
            {
                MessageBox.Show("Banco de dados não configurado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (lstDados.Items.Count == 0)
            {
                MessageBox.Show("Informe um CNPJ para pesquisar e depois cadastrar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                GravarDados();
            }
        }

        private void btnVerEmpresas_Click(object sender, EventArgs e)
        {
            frmEmpresas formEmpresas = new frmEmpresas();
            formEmpresas.ShowDialog();
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBancoDados formBancoDados = new frmBancoDados();
            formBancoDados.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            servidor = Properties.Settings.Default.strServidor;
            banco = Properties.Settings.Default.strBancoDados;            
        }

    }
}
