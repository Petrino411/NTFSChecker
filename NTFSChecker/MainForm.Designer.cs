using System.Windows.Forms;
using System;

namespace NTFSChecker
{
    partial class MainForm
    {
        
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnOpen = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.BtnCheck = new System.Windows.Forms.Button();
            this.ListLogs = new System.Windows.Forms.ListBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.ExportToExcel = new System.Windows.Forms.Button();
            this.ChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnOpen
            // 
            this.BtnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnOpen.Location = new System.Drawing.Point(469, 32);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(106, 34);
            this.BtnOpen.TabIndex = 0;
            this.BtnOpen.Text = "Открыть";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFolderPath.Location = new System.Drawing.Point(43, 85);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(983, 24);
            this.txtFolderPath.TabIndex = 1;
            // 
            // BtnCheck
            // 
            this.BtnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnCheck.Location = new System.Drawing.Point(469, 115);
            this.BtnCheck.Name = "BtnCheck";
            this.BtnCheck.Size = new System.Drawing.Size(106, 33);
            this.BtnCheck.TabIndex = 2;
            this.BtnCheck.Text = "Проверить";
            this.BtnCheck.UseVisualStyleBackColor = true;
            this.BtnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
            // 
            // ListLogs
            // 
            this.ListLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ListLogs.FormattingEnabled = true;
            this.ListLogs.ItemHeight = 18;
            this.ListLogs.Location = new System.Drawing.Point(43, 196);
            this.ListLogs.Name = "ListLogs";
            this.ListLogs.Size = new System.Drawing.Size(983, 418);
            this.ListLogs.TabIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(43, 167);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(983, 23);
            this.progressBar.TabIndex = 4;
            // 
            // ExportToExcel
            // 
            this.ExportToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExportToExcel.Location = new System.Drawing.Point(854, 639);
            this.ExportToExcel.Name = "ExportToExcel";
            this.ExportToExcel.Size = new System.Drawing.Size(144, 29);
            this.ExportToExcel.TabIndex = 5;
            this.ExportToExcel.Text = "Excel";
            this.ExportToExcel.UseVisualStyleBackColor = true;
            this.ExportToExcel.Click += new System.EventHandler(this.ExportToExcelClick);
            // 
            // ChangesCheckBox
            // 
            this.ChangesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChangesCheckBox.Location = new System.Drawing.Point(604, 639);
            this.ChangesCheckBox.Name = "ChangesCheckBox";
            this.ChangesCheckBox.Size = new System.Drawing.Size(244, 29);
            this.ChangesCheckBox.TabIndex = 6;
            this.ChangesCheckBox.Text = "Экспортировать все папки";
            this.ChangesCheckBox.UseVisualStyleBackColor = true;
            this.ChangesCheckBox.CheckedChanged += new System.EventHandler(this.ChangesCheckBox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 694);
            this.Controls.Add(this.ChangesCheckBox);
            this.Controls.Add(this.ExportToExcel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ListLogs);
            this.Controls.Add(this.BtnCheck);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.BtnOpen);
            this.Name = "MainForm";
            this.Text = "NTFSChecker";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox ChangesCheckBox;

        private System.Windows.Forms.Button ExportToExcel;

        private System.Windows.Forms.ProgressBar progressBar;

        private System.Windows.Forms.ListBox ListLogs;

        #endregion


        

        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button BtnCheck;
    }
}

