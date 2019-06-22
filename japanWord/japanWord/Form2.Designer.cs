namespace japanWord
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.addBt = new System.Windows.Forms.Button();
            this.delBt = new System.Windows.Forms.Button();
            this.stopBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(222, 100);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "reday go";
            // 
            // addBt
            // 
            this.addBt.Location = new System.Drawing.Point(224, 0);
            this.addBt.Name = "addBt";
            this.addBt.Size = new System.Drawing.Size(16, 32);
            this.addBt.TabIndex = 1;
            this.addBt.Text = "＋";
            this.addBt.UseVisualStyleBackColor = true;
            this.addBt.Click += new System.EventHandler(this.addBt_Click);
            // 
            // delBt
            // 
            this.delBt.Location = new System.Drawing.Point(224, 40);
            this.delBt.Name = "delBt";
            this.delBt.Size = new System.Drawing.Size(16, 32);
            this.delBt.TabIndex = 2;
            this.delBt.Text = "－";
            this.delBt.UseVisualStyleBackColor = true;
            this.delBt.Click += new System.EventHandler(this.delBt_Click);
            // 
            // stopBt
            // 
            this.stopBt.Location = new System.Drawing.Point(224, 80);
            this.stopBt.Name = "stopBt";
            this.stopBt.Size = new System.Drawing.Size(16, 16);
            this.stopBt.TabIndex = 3;
            this.stopBt.Text = "||";
            this.stopBt.UseVisualStyleBackColor = true;
            this.stopBt.Click += new System.EventHandler(this.stopBt_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(240, 100);
            this.Controls.Add(this.stopBt);
            this.Controls.Add(this.delBt);
            this.Controls.Add(this.addBt);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.Text = "Form2";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox1;
        public System.Windows.Forms.Button addBt;
        public System.Windows.Forms.Button delBt;
        public System.Windows.Forms.Button stopBt;
    }
}