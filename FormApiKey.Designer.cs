namespace GT4Twitch
{
    partial class FormApiKey
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbAPIKey = new Label();
            tbAPIKey = new TextBox();
            btApply = new Button();
            btCancel = new Button();
            fileSystemWatcher1 = new FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // lbAPIKey
            // 
            lbAPIKey.AutoSize = true;
            lbAPIKey.Location = new Point(12, 9);
            lbAPIKey.Name = "lbAPIKey";
            lbAPIKey.Size = new Size(207, 15);
            lbAPIKey.TabIndex = 0;
            lbAPIKey.Text = "Twitch API Key (KEEP THIS A SECRET!):";
            // 
            // tbAPIKey
            // 
            tbAPIKey.Location = new Point(12, 27);
            tbAPIKey.Name = "tbAPIKey";
            tbAPIKey.Size = new Size(444, 23);
            tbAPIKey.TabIndex = 1;
            // 
            // btApply
            // 
            btApply.Location = new Point(381, 56);
            btApply.Name = "btApply";
            btApply.Size = new Size(75, 23);
            btApply.TabIndex = 2;
            btApply.Text = "Apply";
            btApply.UseVisualStyleBackColor = true;
            btApply.Click += btApply_Click;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(12, 56);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 3;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // FormApiKey
            // 
            AcceptButton = btApply;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btCancel;
            ClientSize = new Size(471, 89);
            ControlBox = false;
            Controls.Add(btCancel);
            Controls.Add(btApply);
            Controls.Add(tbAPIKey);
            Controls.Add(lbAPIKey);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormApiKey";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "API Key Setup";
            Load += FormApiKey_Load;
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbAPIKey;
        private TextBox tbAPIKey;
        private Button btApply;
        private Button btCancel;
        private FileSystemWatcher fileSystemWatcher1;
    }
}