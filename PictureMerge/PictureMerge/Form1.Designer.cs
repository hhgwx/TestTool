﻿namespace PictureMerge
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.picMergeWindow1 = new PictureMerge.PicMerge.PicMergeWindow();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.birthdayPicWindow1 = new PictureMerge.BirthdayPic.BirthdayPicWindow();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1000, 744);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.picMergeWindow1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(992, 718);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "相似照片查找";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // picMergeWindow1
            // 
            this.picMergeWindow1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.picMergeWindow1.Location = new System.Drawing.Point(0, 0);
            this.picMergeWindow1.Name = "picMergeWindow1";
            this.picMergeWindow1.Size = new System.Drawing.Size(993, 720);
            this.picMergeWindow1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.birthdayPicWindow1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(992, 718);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "时间顺照片整理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // birthdayPicWindow1
            // 
            this.birthdayPicWindow1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.birthdayPicWindow1.Location = new System.Drawing.Point(0, 0);
            this.birthdayPicWindow1.Name = "birthdayPicWindow1";
            this.birthdayPicWindow1.Size = new System.Drawing.Size(993, 720);
            this.birthdayPicWindow1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(999, 743);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingButton);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;

        private PictureMerge.PicMerge.PicMergeWindow picMergeWindow1;
        private BirthdayPic.BirthdayPicWindow birthdayPicWindow1;
    }
}

