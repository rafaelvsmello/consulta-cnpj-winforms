using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ConsultaCnpj
{
    public partial class Form : System.Windows.Forms.Form
    {
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

                    var responseDeserialized = JsonConvert.DeserializeObject<Empresa.Root>(objResponse.ToString());

                    lstDados.Items.Add("CNPJ: " + cnpj);
                    lstDados.Items.Add("Nome Empresarial: " + responseDeserialized.nome);
                    lstDados.Items.Add(string.IsNullOrEmpty(responseDeserialized.fantasia) ? "SEM NOME FANTASIA" : "Nome Fantasia: " + responseDeserialized.fantasia);
                    lstDados.Items.Add("Endereço: " + responseDeserialized.logradouro + " Nº: " + responseDeserialized.numero);
                    lstDados.Items.Add(string.IsNullOrEmpty(responseDeserialized.complemento) ? "SEM COMPLEMENTO" : "Complemento: " + responseDeserialized.complemento);
                    lstDados.Items.Add("Bairro: " + responseDeserialized.bairro);
                    lstDados.Items.Add("Cidade: " + responseDeserialized.municipio);
                    lstDados.Items.Add("CEP: " + responseDeserialized.cep);
                    lstDados.Items.Add("Abertura: " + responseDeserialized.abertura);

                    streamDados.Close();
                    streamReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }            
        }

        private void GravarDados()
        { }

        public Form()
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
            txtCnpj.Text = string.Empty;
            lstDados.Items.Clear();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (lstDados.Items.Count > 0)
            {
                GravarDados();
            }
            else
            {
                MessageBox.Show("Informe um CNPJ para pesquisar e depois cadastrar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    }
}
