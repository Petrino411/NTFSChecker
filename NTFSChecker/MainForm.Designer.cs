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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ExportToExcel = new System.Windows.Forms.Button();
            this.ChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnCheck = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.ListLogs = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExportToExcel
            // 
            this.ExportToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExportToExcel.Location = new System.Drawing.Point(927, 3);
            this.ExportToExcel.Name = "ExportToExcel";
            this.ExportToExcel.Size = new System.Drawing.Size(144, 29);
            this.ExportToExcel.TabIndex = 5;
            this.ExportToExcel.Text = "Excel";
            this.ExportToExcel.UseVisualStyleBackColor = true;
            this.ExportToExcel.Click += new System.EventHandler(this.ExportToExcelClick);
            // 
            // ChangesCheckBox
            // 
            this.ChangesCheckBox.AutoSize = true;
            this.ChangesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChangesCheckBox.Location = new System.Drawing.Point(736, 3);
            this.ChangesCheckBox.Name = "ChangesCheckBox";
            this.ChangesCheckBox.Size = new System.Drawing.Size(185, 24);
            this.ChangesCheckBox.TabIndex = 6;
            this.ChangesCheckBox.Text = "Экспортировать все\r\n";
            this.ChangesCheckBox.UseVisualStyleBackColor = true;
            this.ChangesCheckBox.CheckedChanged += new System.EventHandler(this.ChangesCheckBox_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ExportToExcel);
            this.flowLayoutPanel1.Controls.Add(this.ChangesCheckBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 588);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1074, 42);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // BtnCheck
            // 
            this.BtnCheck.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnCheck.Location = new System.Drawing.Point(484, 73);
            this.BtnCheck.Name = "BtnCheck";
            this.BtnCheck.Size = new System.Drawing.Size(106, 33);
            this.BtnCheck.TabIndex = 2;
            this.BtnCheck.Text = "Проверить";
            this.BtnCheck.UseVisualStyleBackColor = true;
            this.BtnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFolderPath.Location = new System.Drawing.Point(3, 43);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(1068, 24);
            this.txtFolderPath.TabIndex = 1;
            // 
            // BtnOpen
            // 
            this.BtnOpen.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnOpen.Location = new System.Drawing.Point(484, 3);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(106, 34);
            this.BtnOpen.TabIndex = 0;
            this.BtnOpen.Text = "Открыть";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 112);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1068, 30);
            this.progressBar.TabIndex = 4;
            // 
            // ListLogs
            // 
            this.ListLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ListLogs.FormattingEnabled = true;
            this.ListLogs.ItemHeight = 18;
            this.ListLogs.Location = new System.Drawing.Point(3, 148);
            this.ListLogs.Name = "ListLogs";
            this.ListLogs.Size = new System.Drawing.Size(1068, 437);
            this.ListLogs.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.BtnOpen, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ListLogs, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtFolderPath, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.BtnCheck, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1074, 588);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1074, 630);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "MainForm";
            this.Text = "NTFSChecker";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

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

