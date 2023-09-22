namespace GT4Twitch
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbProcPath = new TextBox();
            lbProcPath = new Label();
            lbMcPath = new Label();
            tbMcPath = new TextBox();
            btBrowseProc = new Button();
            btBrowseMc = new Button();
            groupBox1 = new GroupBox();
            rbGT4USOnline = new RadioButton();
            rbGT4US = new RadioButton();
            lbTitleFormat = new Label();
            tbTitleFormat = new TextBox();
            btLaunch = new Button();
            btAPIKey = new Button();
            ofdProcPath = new OpenFileDialog();
            ofdMcPath = new OpenFileDialog();
            lbFmtDesc = new Label();
            lbLoginStatus = new Label();
            rbSpec2 = new RadioButton();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tbProcPath
            // 
            tbProcPath.Location = new Point(83, 6);
            tbProcPath.Name = "tbProcPath";
            tbProcPath.Size = new Size(500, 23);
            tbProcPath.TabIndex = 0;
            tbProcPath.Validated += tbProcPath_Validated;
            // 
            // lbProcPath
            // 
            lbProcPath.AutoSize = true;
            lbProcPath.Location = new Point(12, 9);
            lbProcPath.Name = "lbProcPath";
            lbProcPath.Size = new Size(65, 15);
            lbProcPath.TabIndex = 1;
            lbProcPath.Text = "PCSX2 exe:";
            // 
            // lbMcPath
            // 
            lbMcPath.AutoSize = true;
            lbMcPath.Location = new Point(12, 38);
            lbMcPath.Name = "lbMcPath";
            lbMcPath.Size = new Size(56, 15);
            lbMcPath.TabIndex = 2;
            lbMcPath.Text = "MC path:";
            // 
            // tbMcPath
            // 
            tbMcPath.Location = new Point(83, 35);
            tbMcPath.Name = "tbMcPath";
            tbMcPath.Size = new Size(500, 23);
            tbMcPath.TabIndex = 3;
            tbMcPath.Validated += tbMcPath_Validated;
            // 
            // btBrowseProc
            // 
            btBrowseProc.Location = new Point(589, 9);
            btBrowseProc.Name = "btBrowseProc";
            btBrowseProc.Size = new Size(75, 23);
            btBrowseProc.TabIndex = 4;
            btBrowseProc.Text = "Browse...";
            btBrowseProc.UseVisualStyleBackColor = true;
            btBrowseProc.Click += btBrowseProc_Click;
            // 
            // btBrowseMc
            // 
            btBrowseMc.Location = new Point(589, 35);
            btBrowseMc.Name = "btBrowseMc";
            btBrowseMc.Size = new Size(75, 23);
            btBrowseMc.TabIndex = 5;
            btBrowseMc.Text = "Browse...";
            btBrowseMc.UseVisualStyleBackColor = true;
            btBrowseMc.Click += btBrowseMc_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rbSpec2);
            groupBox1.Controls.Add(rbGT4USOnline);
            groupBox1.Controls.Add(rbGT4US);
            groupBox1.Location = new Point(12, 93);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(176, 108);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Game Version";
            // 
            // rbGT4USOnline
            // 
            rbGT4USOnline.AutoSize = true;
            rbGT4USOnline.Location = new Point(6, 47);
            rbGT4USOnline.Name = "rbGT4USOnline";
            rbGT4USOnline.Size = new Size(160, 19);
            rbGT4USOnline.TabIndex = 7;
            rbGT4USOnline.Text = "Gran Turismo 4 US Online";
            rbGT4USOnline.UseVisualStyleBackColor = true;
            rbGT4USOnline.CheckedChanged += rbGT4USOnline_CheckedChanged;
            // 
            // rbGT4US
            // 
            rbGT4US.AutoSize = true;
            rbGT4US.Checked = true;
            rbGT4US.Location = new Point(6, 22);
            rbGT4US.Name = "rbGT4US";
            rbGT4US.Size = new Size(122, 19);
            rbGT4US.TabIndex = 0;
            rbGT4US.TabStop = true;
            rbGT4US.Text = "Gran Turismo 4 US";
            rbGT4US.UseVisualStyleBackColor = true;
            // 
            // lbTitleFormat
            // 
            lbTitleFormat.AutoSize = true;
            lbTitleFormat.Location = new Point(12, 67);
            lbTitleFormat.Name = "lbTitleFormat";
            lbTitleFormat.Size = new Size(71, 15);
            lbTitleFormat.TabIndex = 7;
            lbTitleFormat.Text = "Title format:";
            // 
            // tbTitleFormat
            // 
            tbTitleFormat.Location = new Point(83, 64);
            tbTitleFormat.Name = "tbTitleFormat";
            tbTitleFormat.Size = new Size(580, 23);
            tbTitleFormat.TabIndex = 8;
            tbTitleFormat.Text = "Gran Turismo 4: {0}% complete | {1}/{2} events complete | {3} gold licenses | {4} ASpec Points";
            tbTitleFormat.Validated += tbTitleFormat_Validated;
            // 
            // btLaunch
            // 
            btLaunch.Location = new Point(589, 178);
            btLaunch.Name = "btLaunch";
            btLaunch.Size = new Size(75, 23);
            btLaunch.TabIndex = 9;
            btLaunch.Text = "Launch";
            btLaunch.UseVisualStyleBackColor = true;
            btLaunch.Click += btLaunch_Click;
            // 
            // btAPIKey
            // 
            btAPIKey.Location = new Point(194, 178);
            btAPIKey.Name = "btAPIKey";
            btAPIKey.Size = new Size(117, 23);
            btAPIKey.TabIndex = 10;
            btAPIKey.Text = "Log in to Twitch";
            btAPIKey.UseVisualStyleBackColor = true;
            btAPIKey.Click += btAPIKey_Click;
            // 
            // ofdProcPath
            // 
            ofdProcPath.FileName = "PCSX2.exe";
            ofdProcPath.Filter = "Executables|*.exe|All files|*.*";
            // 
            // ofdMcPath
            // 
            ofdMcPath.FileName = "Mcd001.ps2";
            ofdMcPath.Filter = "PS2 memcard|*.ps2|All files|*.*";
            // 
            // lbFmtDesc
            // 
            lbFmtDesc.AutoSize = true;
            lbFmtDesc.Location = new Point(194, 93);
            lbFmtDesc.Name = "lbFmtDesc";
            lbFmtDesc.Size = new Size(95, 15);
            lbFmtDesc.TabIndex = 11;
            lbFmtDesc.Text = "FormatDescHere";
            // 
            // lbLoginStatus
            // 
            lbLoginStatus.AutoSize = true;
            lbLoginStatus.Location = new Point(317, 182);
            lbLoginStatus.Name = "lbLoginStatus";
            lbLoginStatus.Size = new Size(178, 15);
            lbLoginStatus.TabIndex = 12;
            lbLoginStatus.Text = "Not logged in. Please log in first.";
            // 
            // rbSpec2
            // 
            rbSpec2.AutoSize = true;
            rbSpec2.Location = new Point(6, 72);
            rbSpec2.Name = "rbSpec2";
            rbSpec2.Size = new Size(82, 19);
            rbSpec2.TabIndex = 8;
            rbSpec2.TabStop = true;
            rbSpec2.Text = "GT4 Spec II";
            rbSpec2.UseVisualStyleBackColor = true;
            rbSpec2.CheckedChanged += rbSpec2_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(674, 213);
            Controls.Add(lbLoginStatus);
            Controls.Add(lbFmtDesc);
            Controls.Add(btAPIKey);
            Controls.Add(btLaunch);
            Controls.Add(tbTitleFormat);
            Controls.Add(lbTitleFormat);
            Controls.Add(groupBox1);
            Controls.Add(btBrowseMc);
            Controls.Add(btBrowseProc);
            Controls.Add(tbMcPath);
            Controls.Add(lbMcPath);
            Controls.Add(lbProcPath);
            Controls.Add(tbProcPath);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "GT4Twitch";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbProcPath;
        private Label lbProcPath;
        private Label lbMcPath;
        private TextBox tbMcPath;
        private Button btBrowseProc;
        private Button btBrowseMc;
        private GroupBox groupBox1;
        private RadioButton rbGT4USOnline;
        private RadioButton rbGT4US;
        private Label lbTitleFormat;
        private TextBox tbTitleFormat;
        private Button btLaunch;
        private Button btAPIKey;
        private OpenFileDialog ofdProcPath;
        private OpenFileDialog ofdMcPath;
        private Label lbFmtDesc;
        private Label lbLoginStatus;
        private RadioButton rbSpec2;
    }
}