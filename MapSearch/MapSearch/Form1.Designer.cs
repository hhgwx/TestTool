namespace MapSearch
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.RadioButton();
            this.endButton = new System.Windows.Forms.RadioButton();
            this.wallButton = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.AutoSize = true;
            this.startButton.Location = new System.Drawing.Point(50, 310);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(73, 16);
            this.startButton.TabIndex = 0;
            this.startButton.TabStop = true;
            this.startButton.Text = "startPoint";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // endButton
            // 
            this.endButton.AutoSize = true;
            this.endButton.Location = new System.Drawing.Point(50, 330);
            this.endButton.Name = "endButton";
            this.endButton.Size = new System.Drawing.Size(67, 16);
            this.endButton.TabIndex = 1;
            this.endButton.TabStop = true;
            this.endButton.Text = "endPoint";
            this.endButton.UseVisualStyleBackColor = true;
            // 
            // wallButton
            // 
            this.wallButton.AutoSize = true;
            this.wallButton.Location = new System.Drawing.Point(50, 350);
            this.wallButton.Name = "wallButton";
            this.wallButton.Size = new System.Drawing.Size(43, 16);
            this.wallButton.TabIndex = 2;
            this.wallButton.TabStop = true;
            this.wallButton.Text = "wall";
            this.wallButton.UseVisualStyleBackColor = true;
            this.wallButton.Select();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 32);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(303, 400);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.endButton);
            this.Controls.Add(this.wallButton);
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton startButton;
        private System.Windows.Forms.RadioButton endButton;
        private System.Windows.Forms.RadioButton wallButton;
        private System.Windows.Forms.Button button1;
    }
}

