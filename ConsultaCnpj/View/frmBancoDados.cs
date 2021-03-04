using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultaCnpj
{
    public partial class frmBancoDados : Form
    {
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
        }
    }
}
