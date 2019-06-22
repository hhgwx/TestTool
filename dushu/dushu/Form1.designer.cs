namespace dushu
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
			this.components = new System.ComponentModel.Container();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.myMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.setting = new System.Windows.Forms.ToolStripMenuItem();
			this.exist = new System.Windows.Forms.ToolStripMenuItem();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.comboBox_fontSize = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.button_fontColor = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.next_key = new System.Windows.Forms.TextBox();
			this.pre_key = new System.Windows.Forms.TextBox();
			this.stop_key = new System.Windows.Forms.TextBox();
			this.hidden_key = new System.Windows.Forms.TextBox();
			this.next_shift = new System.Windows.Forms.CheckBox();
			this.pre_shift = new System.Windows.Forms.CheckBox();
			this.stop_shift = new System.Windows.Forms.CheckBox();
			this.next_alt = new System.Windows.Forms.CheckBox();
			this.pre_alt = new System.Windows.Forms.CheckBox();
			this.stop_alt = new System.Windows.Forms.CheckBox();
			this.next_ctrl = new System.Windows.Forms.CheckBox();
			this.pre_ctrl = new System.Windows.Forms.CheckBox();
			this.hidden_shift = new System.Windows.Forms.CheckBox();
			this.stop_ctrl = new System.Windows.Forms.CheckBox();
			this.hidden_alt = new System.Windows.Forms.CheckBox();
			this.hidden_ctrl = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.text_nowLine = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.myMenu.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "dushu";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
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
			this.setting.Size = new System.Drawing.Size(106, 22);
			this.setting.Text = "Setting";
			this.setting.Click += new System.EventHandler(this.setting_Click);
			// 
			// exist
			// 
			this.exist.Name = "exist";
			this.exist.Size = new System.Drawing.Size(106, 22);
			this.exist.Text = "Exist";
			this.exist.Click += new System.EventHandler(this.exist_Click);
			// 
			// radioButton4
			// 
			this.radioButton4.AutoSize = true;
			this.radioButton4.Location = new System.Drawing.Point(224, 6);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(29, 16);
			this.radioButton4.TabIndex = 1;
			this.radioButton4.Text = "4";
			this.radioButton4.UseVisualStyleBackColor = true;
			this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Checked = true;
			this.radioButton3.Location = new System.Drawing.Point(160, 6);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(29, 16);
			this.radioButton3.TabIndex = 1;
			this.radioButton3.TabStop = true;
			this.radioButton3.Text = "3";
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(88, 6);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(29, 16);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "2";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Location = new System.Drawing.Point(24, 6);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(29, 16);
			this.radioButton1.TabIndex = 1;
			this.radioButton1.Text = "1";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(72, 38);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(40, 19);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "4";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(112, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "秒";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(209, 67);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(56, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "Start";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "持续时间";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(136, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "一次显示字数";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(213, 38);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(40, 19);
			this.textBox2.TabIndex = 6;
			this.textBox2.Text = "4";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(139, 72);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(60, 16);
			this.checkBox1.TabIndex = 7;
			this.checkBox1.Text = "要解码";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.comboBox_fontSize);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.button_fontColor);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.next_key);
			this.panel1.Controls.Add(this.pre_key);
			this.panel1.Controls.Add(this.stop_key);
			this.panel1.Controls.Add(this.hidden_key);
			this.panel1.Controls.Add(this.next_shift);
			this.panel1.Controls.Add(this.pre_shift);
			this.panel1.Controls.Add(this.stop_shift);
			this.panel1.Controls.Add(this.next_alt);
			this.panel1.Controls.Add(this.pre_alt);
			this.panel1.Controls.Add(this.stop_alt);
			this.panel1.Controls.Add(this.next_ctrl);
			this.panel1.Controls.Add(this.pre_ctrl);
			this.panel1.Controls.Add(this.hidden_shift);
			this.panel1.Controls.Add(this.stop_ctrl);
			this.panel1.Controls.Add(this.hidden_alt);
			this.panel1.Controls.Add(this.hidden_ctrl);
			this.panel1.Location = new System.Drawing.Point(2, 111);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(275, 181);
			this.panel1.TabIndex = 9;
			this.panel1.Visible = false;
			// 
			// comboBox_fontSize
			// 
			this.comboBox_fontSize.FormattingEnabled = true;
			this.comboBox_fontSize.Items.AddRange(new object[] {
            "3",
            "5",
            "7",
            "8",
            "9",
            "10",
            "11",
            "13",
            "15",
            "17",
            "19",
            "21"});
			this.comboBox_fontSize.Location = new System.Drawing.Point(179, 153);
			this.comboBox_fontSize.Name = "comboBox_fontSize";
			this.comboBox_fontSize.Size = new System.Drawing.Size(59, 20);
			this.comboBox_fontSize.TabIndex = 13;
			this.comboBox_fontSize.Text = "9";
			// 
			// label10
			// 
			this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label10.Location = new System.Drawing.Point(8, 145);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(257, 2);
			this.label10.TabIndex = 12;
			// 
			// button_fontColor
			// 
			this.button_fontColor.Location = new System.Drawing.Point(20, 153);
			this.button_fontColor.Name = "button_fontColor";
			this.button_fontColor.Size = new System.Drawing.Size(68, 22);
			this.button_fontColor.TabIndex = 11;
			this.button_fontColor.Text = "字体颜色";
			this.button_fontColor.UseVisualStyleBackColor = true;
			this.button_fontColor.Click += new System.EventHandler(this.button_fontColor_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(125, 158);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(53, 12);
			this.label11.TabIndex = 10;
			this.label11.Text = "字体大小";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(18, 109);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(53, 12);
			this.label8.TabIndex = 10;
			this.label8.Text = "快速后翻";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(18, 77);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 10;
			this.label7.Text = "快速前翻";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(18, 42);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 12);
			this.label6.TabIndex = 10;
			this.label6.Text = "暂停/开始";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(18, 7);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(155, 12);
			this.label5.TabIndex = 10;
			this.label5.Text = "显示/隐藏 (隐藏时自动暂停)";
			// 
			// next_key
			// 
			this.next_key.Enabled = false;
			this.next_key.Location = new System.Drawing.Point(225, 123);
			this.next_key.Name = "next_key";
			this.next_key.Size = new System.Drawing.Size(26, 19);
			this.next_key.TabIndex = 9;
			this.next_key.Text = "→";
			// 
			// pre_key
			// 
			this.pre_key.Enabled = false;
			this.pre_key.Location = new System.Drawing.Point(225, 91);
			this.pre_key.Multiline = true;
			this.pre_key.Name = "pre_key";
			this.pre_key.Size = new System.Drawing.Size(26, 19);
			this.pre_key.TabIndex = 9;
			this.pre_key.Text = "←";
			// 
			// stop_key
			// 
			this.stop_key.Location = new System.Drawing.Point(225, 56);
			this.stop_key.Name = "stop_key";
			this.stop_key.Size = new System.Drawing.Size(26, 19);
			this.stop_key.TabIndex = 9;
			this.stop_key.Text = "S";
			// 
			// hidden_key
			// 
			this.hidden_key.Location = new System.Drawing.Point(225, 21);
			this.hidden_key.Name = "hidden_key";
			this.hidden_key.Size = new System.Drawing.Size(26, 19);
			this.hidden_key.TabIndex = 9;
			this.hidden_key.Text = "C";
			// 
			// next_shift
			// 
			this.next_shift.AutoSize = true;
			this.next_shift.Location = new System.Drawing.Point(158, 124);
			this.next_shift.Name = "next_shift";
			this.next_shift.Size = new System.Drawing.Size(70, 16);
			this.next_shift.TabIndex = 8;
			this.next_shift.Text = "SHIFT + ";
			this.next_shift.UseVisualStyleBackColor = true;
			// 
			// pre_shift
			// 
			this.pre_shift.AutoSize = true;
			this.pre_shift.Location = new System.Drawing.Point(158, 92);
			this.pre_shift.Name = "pre_shift";
			this.pre_shift.Size = new System.Drawing.Size(70, 16);
			this.pre_shift.TabIndex = 8;
			this.pre_shift.Text = "SHIFT + ";
			this.pre_shift.UseVisualStyleBackColor = true;
			// 
			// stop_shift
			// 
			this.stop_shift.AutoSize = true;
			this.stop_shift.Location = new System.Drawing.Point(158, 57);
			this.stop_shift.Name = "stop_shift";
			this.stop_shift.Size = new System.Drawing.Size(70, 16);
			this.stop_shift.TabIndex = 8;
			this.stop_shift.Text = "SHIFT + ";
			this.stop_shift.UseVisualStyleBackColor = true;
			// 
			// next_alt
			// 
			this.next_alt.AutoCheck = false;
			this.next_alt.AutoSize = true;
			this.next_alt.Checked = true;
			this.next_alt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.next_alt.Location = new System.Drawing.Point(104, 124);
			this.next_alt.Name = "next_alt";
			this.next_alt.Size = new System.Drawing.Size(59, 16);
			this.next_alt.TabIndex = 8;
			this.next_alt.Text = "ALT + ";
			this.next_alt.UseVisualStyleBackColor = true;
			// 
			// pre_alt
			// 
			this.pre_alt.AutoCheck = false;
			this.pre_alt.AutoSize = true;
			this.pre_alt.Checked = true;
			this.pre_alt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.pre_alt.Location = new System.Drawing.Point(104, 92);
			this.pre_alt.Name = "pre_alt";
			this.pre_alt.Size = new System.Drawing.Size(59, 16);
			this.pre_alt.TabIndex = 8;
			this.pre_alt.Text = "ALT + ";
			this.pre_alt.UseVisualStyleBackColor = true;
			// 
			// stop_alt
			// 
			this.stop_alt.AutoSize = true;
			this.stop_alt.Checked = true;
			this.stop_alt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.stop_alt.Location = new System.Drawing.Point(104, 57);
			this.stop_alt.Name = "stop_alt";
			this.stop_alt.Size = new System.Drawing.Size(59, 16);
			this.stop_alt.TabIndex = 8;
			this.stop_alt.Text = "ALT + ";
			this.stop_alt.UseVisualStyleBackColor = true;
			// 
			// next_ctrl
			// 
			this.next_ctrl.AutoSize = true;
			this.next_ctrl.Checked = true;
			this.next_ctrl.CheckState = System.Windows.Forms.CheckState.Checked;
			this.next_ctrl.Enabled = false;
			this.next_ctrl.Location = new System.Drawing.Point(39, 124);
			this.next_ctrl.Name = "next_ctrl";
			this.next_ctrl.Size = new System.Drawing.Size(67, 16);
			this.next_ctrl.TabIndex = 8;
			this.next_ctrl.Text = "CTRL + ";
			this.next_ctrl.UseVisualStyleBackColor = true;
			// 
			// pre_ctrl
			// 
			this.pre_ctrl.AutoSize = true;
			this.pre_ctrl.Checked = true;
			this.pre_ctrl.CheckState = System.Windows.Forms.CheckState.Checked;
			this.pre_ctrl.Enabled = false;
			this.pre_ctrl.Location = new System.Drawing.Point(39, 92);
			this.pre_ctrl.Name = "pre_ctrl";
			this.pre_ctrl.Size = new System.Drawing.Size(67, 16);
			this.pre_ctrl.TabIndex = 8;
			this.pre_ctrl.Text = "CTRL + ";
			this.pre_ctrl.UseVisualStyleBackColor = true;
			// 
			// hidden_shift
			// 
			this.hidden_shift.AutoSize = true;
			this.hidden_shift.Location = new System.Drawing.Point(158, 22);
			this.hidden_shift.Name = "hidden_shift";
			this.hidden_shift.Size = new System.Drawing.Size(70, 16);
			this.hidden_shift.TabIndex = 8;
			this.hidden_shift.Text = "SHIFT + ";
			this.hidden_shift.UseVisualStyleBackColor = true;
			// 
			// stop_ctrl
			// 
			this.stop_ctrl.AutoSize = true;
			this.stop_ctrl.Checked = true;
			this.stop_ctrl.CheckState = System.Windows.Forms.CheckState.Checked;
			this.stop_ctrl.Enabled = false;
			this.stop_ctrl.Location = new System.Drawing.Point(39, 57);
			this.stop_ctrl.Name = "stop_ctrl";
			this.stop_ctrl.Size = new System.Drawing.Size(67, 16);
			this.stop_ctrl.TabIndex = 8;
			this.stop_ctrl.Text = "CTRL + ";
			this.stop_ctrl.UseVisualStyleBackColor = true;
			// 
			// hidden_alt
			// 
			this.hidden_alt.AutoSize = true;
			this.hidden_alt.Checked = true;
			this.hidden_alt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.hidden_alt.Location = new System.Drawing.Point(104, 22);
			this.hidden_alt.Name = "hidden_alt";
			this.hidden_alt.Size = new System.Drawing.Size(59, 16);
			this.hidden_alt.TabIndex = 8;
			this.hidden_alt.Text = "ALT + ";
			this.hidden_alt.UseVisualStyleBackColor = true;
			// 
			// hidden_ctrl
			// 
			this.hidden_ctrl.AutoSize = true;
			this.hidden_ctrl.Checked = true;
			this.hidden_ctrl.CheckState = System.Windows.Forms.CheckState.Checked;
			this.hidden_ctrl.Enabled = false;
			this.hidden_ctrl.Location = new System.Drawing.Point(39, 22);
			this.hidden_ctrl.Name = "hidden_ctrl";
			this.hidden_ctrl.Size = new System.Drawing.Size(67, 16);
			this.hidden_ctrl.TabIndex = 8;
			this.hidden_ctrl.Text = "CTRL + ";
			this.hidden_ctrl.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(8, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 12);
			this.label4.TabIndex = 10;
			this.label4.Text = "其他设置";
			this.label4.Click += new System.EventHandler(this.label4_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(17, 72);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(53, 12);
			this.label9.TabIndex = 3;
			this.label9.Text = "当前行数";
			// 
			// text_nowLine
			// 
			this.text_nowLine.Location = new System.Drawing.Point(72, 69);
			this.text_nowLine.Name = "text_nowLine";
			this.text_nowLine.Size = new System.Drawing.Size(40, 19);
			this.text_nowLine.TabIndex = 2;
			this.text_nowLine.Text = "1";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label12.Location = new System.Drawing.Point(81, 96);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(53, 12);
			this.label12.TabIndex = 10;
			this.label12.Text = "打赏作者";
			this.label12.Click += new System.EventHandler(this.label12_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(278, 294);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.text_nowLine);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton3);
			this.Controls.Add(this.radioButton4);
			this.Name = "Form1";
			this.Text = "study";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
			this.myMenu.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip myMenu;
        private System.Windows.Forms.ToolStripMenuItem setting;
        private System.Windows.Forms.ToolStripMenuItem exist;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox pre_key;
		private System.Windows.Forms.TextBox stop_key;
		private System.Windows.Forms.TextBox hidden_key;
		private System.Windows.Forms.CheckBox pre_shift;
		private System.Windows.Forms.CheckBox stop_shift;
		private System.Windows.Forms.CheckBox pre_alt;
		private System.Windows.Forms.CheckBox stop_alt;
		private System.Windows.Forms.CheckBox pre_ctrl;
		private System.Windows.Forms.CheckBox hidden_shift;
		private System.Windows.Forms.CheckBox stop_ctrl;
		private System.Windows.Forms.CheckBox hidden_alt;
		private System.Windows.Forms.CheckBox hidden_ctrl;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox next_key;
		private System.Windows.Forms.CheckBox next_shift;
		private System.Windows.Forms.CheckBox next_alt;
		private System.Windows.Forms.CheckBox next_ctrl;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox text_nowLine;
		private System.Windows.Forms.Button button_fontColor;
		private System.Windows.Forms.ComboBox comboBox_fontSize;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
	}
}

