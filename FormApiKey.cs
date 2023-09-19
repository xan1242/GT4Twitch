using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GT4Twitch
{
    public partial class FormApiKey : Form
    {
        public string ApiKey;
        public FormApiKey()
        {
            InitializeComponent();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            tbAPIKey.Text = "";
            this.Close();
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            ApiKey = tbAPIKey.Text;

            this.DialogResult = DialogResult.OK;
            tbAPIKey.Text = "";
            this.Close();
        }

        private void FormApiKey_Load(object sender, EventArgs e)
        {
            tbAPIKey.Text = "";
        }
    }
}
