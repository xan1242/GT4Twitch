using Microsoft.VisualBasic.ApplicationServices;
using Syroot.BinaryData;
using System.Configuration;

namespace GT4Twitch
{
    public partial class Form1 : Form
    {
        private string authToken = "";
        //FormApiKey apiKeyForm;

        private const string clientId = "CLIENTSECRETGOESHERE";
        private string channelId = "";

        private bool bAuthorized = false;
        private bool bGotChannelId = false;

        public Form1()
        {
            InitializeComponent();
            lbFmtDesc.Text = "Formatters: {0} = percentage, {1} = events won, {2} = total events, {3} = gold licenses\n{4} = ASpec Pts., {5} = total licenses, {6} = won missions, {7} = total missions\n{8} = gold coffee brakes, {9} total coffee brakes";

            //apiKeyForm = new FormApiKey();

            //OAuthForm = new FormOAuth();

            if (ConfigurationManager.AppSettings.AllKeys.Contains("ProcPath"))
                tbProcPath.Text = ConfigurationManager.AppSettings["ProcPath"];
            if (ConfigurationManager.AppSettings.AllKeys.Contains("McPath"))
                tbMcPath.Text = ConfigurationManager.AppSettings["McPath"];
            if (ConfigurationManager.AppSettings.AllKeys.Contains("TitleFormat"))
                tbTitleFormat.Text = ConfigurationManager.AppSettings["TitleFormat"];
            if (ConfigurationManager.AppSettings.AllKeys.Contains("authToken"))
            {
                authToken = ConfigurationManager.AppSettings["authToken"];
                if (!string.IsNullOrEmpty(authToken))
                {
                    bAuthorized = true;
                }

            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("channelId"))
            {
                channelId = ConfigurationManager.AppSettings["channelId"];
                if (!string.IsNullOrEmpty(channelId))
                    bGotChannelId = true;
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("bIsOnline"))
            {
                string strIsOnline = ConfigurationManager.AppSettings["bIsOnline"];
                bool bIsOnline = bool.Parse(strIsOnline);

                Core.bIsOnline = bIsOnline;
                rbGT4USOnline.Checked = bIsOnline;
                rbGT4US.Checked = !bIsOnline;
            }

            UpdateLoginStatusLabel();

        }
        private void UpdateLoginStatusLabel()
        {
            if (bAuthorized)
            {
                lbLoginStatus.Text = "Logged in. Channel ID: ";
                if (bGotChannelId)
                    lbLoginStatus.Text += channelId;
            }
            else
            {
                lbLoginStatus.Text = "Not logged in. Please log in first.";
            }
        }

        private void UpdateConfig()
        {
            // Open the configuration file
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Update the value
            config.AppSettings.Settings["ProcPath"].Value = tbProcPath.Text;
            config.AppSettings.Settings["McPath"].Value = tbMcPath.Text;
            config.AppSettings.Settings["TitleFormat"].Value = tbTitleFormat.Text;

            config.AppSettings.Settings["bIsOnline"].Value = rbGT4USOnline.Checked.ToString();

            config.AppSettings.Settings["authToken"].Value = authToken;
            config.AppSettings.Settings["channelId"].Value = channelId;

            // Save the changes
            config.Save(ConfigurationSaveMode.Modified);

            // Refresh the ConfigurationManager
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void rbGT4USOnline_CheckedChanged(object sender, EventArgs e)
        {
            Core.bIsOnline = rbGT4USOnline.Checked;
            UpdateConfig();
        }

        private void btBrowseProc_Click(object sender, EventArgs e)
        {
            if (ofdProcPath.ShowDialog() == DialogResult.OK)
                tbProcPath.Text = ofdProcPath.FileName;
            if (tbMcPath.Text.Length <= 0)
                tbMcPath.Text = Launcher.GetRootPath(tbProcPath.Text) + "\\memcards\\Mcd001.ps2";
            UpdateConfig();
        }

        private void btBrowseMc_Click(object sender, EventArgs e)
        {
            if (ofdMcPath.ShowDialog() == DialogResult.OK)
                tbMcPath.Text = ofdMcPath.FileName;
            UpdateConfig();
        }

        private void btLaunch_Click(object sender, EventArgs e)
        {
            if (!bAuthorized)
            {
                if (MessageBox.Show("WARNING: You either haven't logged in to Twitch or have authorization.\n\nDo you wish to continue?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }

            Console.WriteLine("GT4Twitch: Launching: " + tbProcPath.Text);
            IntPtr handle = Launcher.OpenApp(tbProcPath.Text, Launcher.GetRootPath(tbProcPath.Text));
            if (handle != IntPtr.Zero)
            {
                //tbProcPath.Visible = false;
                //tbMcPath.Visible = false;
                //tbTitleFormat.Visible = false;
                //btAPIKey.Visible = false;
                //btBrowseMc.Visible = false;
                //btLaunch.Visible = false;
                //btBrowseProc.Visible = false;
                //groupBox1.Visible = false;
                //rbGT4US.Visible = false;
                //rbGT4USOnline.Visible = false;
                //lbMcPath.Visible = false;
                //lbTitleFormat.Visible = false;
                //lbProcPath.Visible = false;
                //
                //lbActiveNote.Visible = true;

                this.Visible = false;

                this.WindowState = FormWindowState.Minimized;
                Thread runThread = new Thread(() => Core.Run(handle, tbMcPath.Text, tbTitleFormat.Text, authToken, channelId, clientId, bAuthorized));
                runThread.Start();
            }
        }

        private void tbMcPath_Validated(object sender, EventArgs e)
        {
            UpdateConfig();
        }

        private void tbProcPath_Validated(object sender, EventArgs e)
        {
            UpdateConfig();
        }

        private void tbTitleFormat_Validated(object sender, EventArgs e)
        {
            UpdateConfig();
        }

        private void btAPIKey_Click(object sender, EventArgs e)
        {
            FormOAuth OAuthForm = new FormOAuth();

            if (OAuthForm.ShowDialog(this) == DialogResult.OK)
            {
                Task.Run(async () =>
                {
                    while (!OAuthForm.bReady)
                    {
                        Thread.Sleep(1);
                    }

                    if (!OAuthForm.bSuccess)
                    {
                        MessageBox.Show("Failed to obtain authorization.", "Error");
                        bAuthorized = false;
                        bGotChannelId = false;
                        lbLoginStatus.Invoke((MethodInvoker)delegate
                        {
                            UpdateLoginStatusLabel();
                        });
                        return;
                    }

                    authToken = OAuthForm.authToken;
                    channelId = OAuthForm.channelId;
                    if (!string.IsNullOrEmpty(authToken) || !authToken.Contains("error_no_token"))
                    {
                        bAuthorized = true;
                        Console.WriteLine("Authorization token obtained!");
                        UpdateConfig();
                        if (!string.IsNullOrEmpty(channelId) || !channelId.Contains("error_no_channelid"))
                        {
                            bGotChannelId = true;
                            Console.WriteLine("Channel ID obtained! Channel ID: " + channelId);
                            UpdateConfig();
                        }
                        else
                        {
                            bAuthorized = false;
                            bGotChannelId = false;
                        }
                    }
                    else
                    {
                        bAuthorized = false;
                        bGotChannelId = false;
                    }

                    lbLoginStatus.Invoke((MethodInvoker)delegate
                    {
                        UpdateLoginStatusLabel();
                    });
                });
            }
            else
            {
                MessageBox.Show("Failed to obtain authorization.", "Error");
                authToken = "error_no_token";
                channelId = "error_no_channelid";
                bAuthorized = false;
            }

            //OAuthForm.authToken = "";
            //OAuthForm.channelId = "";

            //if (apiKeyForm.ShowDialog(this) == DialogResult.OK)
            //    ApiKey = apiKeyForm.ApiKey;
            //else
            //    ApiKey = "";
        }
    }
}