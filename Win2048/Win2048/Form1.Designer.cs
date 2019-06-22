using System;
using System.Windows.Forms;

namespace Win2048
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

            this.SuspendLayout();
            // 
            // textBox
            // 
            textBox = new System.Windows.Forms.RichTextBox[4, 4];
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++)
                {
                    this.textBox[x, y] = new System.Windows.Forms.RichTextBox();
                    this.textBox[x, y].BorderStyle = System.Windows.Forms.BorderStyle.None;
                    this.textBox[x, y].Enabled = false;
                    this.textBox[x, y].Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
                    this.textBox[x, y].Location = new System.Drawing.Point(y * 26, x * 26);
                    this.textBox[x, y].MaxLength = 5;
                    this.textBox[x, y].Name = "textBox[x][y]";
                    this.textBox[x, y].ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
                    this.textBox[x, y].Size = new System.Drawing.Size(26, 26);
                    this.textBox[x, y].TabIndex = 0;
                    //                    this.textBox[x,y].Text = "2048";
                    this.textBox[x, y].WordWrap = false;

                    this.Controls.Add(this.textBox[x, y]);
                }
            }

            // 
            // reStart
            // 
            this.reStart_button = new System.Windows.Forms.Button();
            this.reStart_button.Font = new System.Drawing.Font("MS UI Gothic", 6F);
            this.reStart_button.Location = new System.Drawing.Point(0, 104);
            this.reStart_button.Name = "reStart";
            this.reStart_button.Size = new System.Drawing.Size(48, 15);
            this.reStart_button.TabIndex = 0;
            this.reStart_button.Text = "R";
            this.reStart_button.UseVisualStyleBackColor = true;
            this.reStart_button.Click += new System.EventHandler(this.reStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(90, 125);
            this.Controls.Add(this.reStart_button);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
			this.TransparencyKey = System.Drawing.SystemColors.Control; //透明
			this.ResumeLayout(false);

        }

        #endregion

        #region event

        protected override bool ProcessDialogKey(Keys keyData)
        {
//            MessageBox.Show("222");
            RunPro runPro = new RunPro();
            Boolean movedFlg = false;
            if (keyData == Keys.Up)
            {
                movedFlg = runPro.move(Program.num44, 1);
            }
            else if (keyData == Keys.Down)
            {
                movedFlg = runPro.move(Program.num44, 3);
            }
            else if (keyData == Keys.Left)
            {
                movedFlg = runPro.move(Program.num44, 4);
            }
            else if (keyData == Keys.Right)
            {
                movedFlg = runPro.move(Program.num44, 2);
            }

            if (movedFlg)
            {
                //                System.Windows.Forms.MessageBox.Show("aaa", "aaa");
                runPro.lose1(Program.num44);
                runPro.newDataAppear(Program.num44);
                this.drawForm(Program.num44);
            }

            return base.ProcessDialogKey(keyData);
        }
        #endregion

        private System.Windows.Forms.RichTextBox[,] textBox;
        private Button reStart_button;
    }
}

