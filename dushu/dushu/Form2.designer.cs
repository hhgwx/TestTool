namespace dushu
{
    partial class Form2
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.myMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.setting = new System.Windows.Forms.ToolStripMenuItem();
			this.exist = new System.Windows.Forms.ToolStripMenuItem();
			this.myMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "<Start>";
			this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label1_MouseClick);
			this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
			this.label1.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
			this.label1.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
			this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
			this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
			// 
			// myMenu
			// 
			this.myMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setting,
            this.exist});
			this.myMenu.Name = "myMenu";
			this.myMenu.Size = new System.Drawing.Size(107, 48);
			this.myMenu.Text = "Menu";
			// 
			// setting
			// 
			this.setting.Name = "setting";
			this.setting.Size = new System.Drawing.Size(152, 22);
			this.setting.Text = "Setting";
			this.setting.Click += new System.EventHandler(this.setting_Click);
			// 
			// exist
			// 
			this.exist.Name = "exist";
			this.exist.Size = new System.Drawing.Size(152, 22);
			this.exist.Text = "Exist";
			this.exist.Click += new System.EventHandler(this.exist_Click);
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(511, 20);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form2";
			this.Text = "Form2";
			this.TransparencyKey = System.Drawing.SystemColors.Control;
			this.myMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion
		public System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenuStrip myMenu;
		private System.Windows.Forms.ToolStripMenuItem setting;
		private System.Windows.Forms.ToolStripMenuItem exist;
	}
}