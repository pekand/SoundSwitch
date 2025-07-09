namespace SoundSwitch
{
    partial class FormSoundSwitch
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSoundSwitch));
            comboBox1 = new ComboBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            optionsToolStripMenuItem = new ToolStripMenuItem();
            mostTopToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            instantProgressBar1 = new InstantProgressBar();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.ContextMenuStrip = contextMenuStrip1;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 12);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(282, 29);
            comboBox1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, closeToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(128, 52);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mostTopToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(127, 24);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // mostTopToolStripMenuItem
            // 
            mostTopToolStripMenuItem.Name = "mostTopToolStripMenuItem";
            mostTopToolStripMenuItem.Size = new Size(135, 24);
            mostTopToolStripMenuItem.Text = "Most top";
            mostTopToolStripMenuItem.Click += mostTopToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(127, 24);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // instantProgressBar1
            // 
            instantProgressBar1.ContextMenuStrip = contextMenuStrip1;
            instantProgressBar1.Location = new Point(12, 47);
            instantProgressBar1.Minimum = 0;
            instantProgressBar1.Name = "instantProgressBar1";
            instantProgressBar1.Size = new Size(282, 28);
            instantProgressBar1.TabIndex = 1;
            instantProgressBar1.Text = "instantProgressBar1";
            instantProgressBar1.Value = 0;
            instantProgressBar1.MouseDown += instantProgressBar1_MouseDown;
            instantProgressBar1.MouseMove += instantProgressBar1_MouseMove;
            instantProgressBar1.MouseUp += instantProgressBar1_MouseUp;
            // 
            // FormSoundSwitch
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(305, 83);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(instantProgressBar1);
            Controls.Add(comboBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormSoundSwitch";
            StartPosition = FormStartPosition.Manual;
            Text = "SoundSwitch";
            Activated += FormSoundSwitch_Activated;
            FormClosing += FormSoundSwitch_FormClosing;
            Load += FormSoundSwitch_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBox1;
        private InstantProgressBar instantProgressBar1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem mostTopToolStripMenuItem;
    }
}
