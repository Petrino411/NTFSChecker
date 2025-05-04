using System.Windows.Forms;
using System;

namespace NTFSChecker.WinForms;

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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        this.BtnCheck = new System.Windows.Forms.Button();
        this.txtFolderPath = new System.Windows.Forms.TextBox();
        this.BtnOpen = new System.Windows.Forms.Button();
        this.ListLogs = new System.Windows.Forms.ListBox();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.labelInfo = new System.Windows.Forms.Label();
        this.labelTimer = new System.Windows.Forms.Label();
        this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.ExcelTool = new System.Windows.Forms.ToolStripMenuItem();
        this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.Timer1 = new System.Windows.Forms.Timer(this.components);
        this.tableLayoutPanel1.SuspendLayout();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // BtnCheck
        // 
        this.BtnCheck.Anchor = System.Windows.Forms.AnchorStyles.Top;
        this.tableLayoutPanel1.SetColumnSpan(this.BtnCheck, 3);
        this.BtnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.BtnCheck.Location = new System.Drawing.Point(448, 77);
        this.BtnCheck.Name = "BtnCheck";
        this.BtnCheck.Size = new System.Drawing.Size(109, 33);
        this.BtnCheck.TabIndex = 2;
        this.BtnCheck.Text = "Проверить";
        this.BtnCheck.UseVisualStyleBackColor = true;
        this.BtnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
        // 
        // txtFolderPath
        // 
        this.tableLayoutPanel1.SetColumnSpan(this.txtFolderPath, 3);
        this.txtFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.txtFolderPath.Location = new System.Drawing.Point(5, 45);
        this.txtFolderPath.Margin = new System.Windows.Forms.Padding(5);
        this.txtFolderPath.Name = "txtFolderPath";
        this.txtFolderPath.Size = new System.Drawing.Size(995, 24);
        this.txtFolderPath.TabIndex = 1;
        // 
        // BtnOpen
        // 
        this.BtnOpen.Anchor = System.Windows.Forms.AnchorStyles.Top;
        this.tableLayoutPanel1.SetColumnSpan(this.BtnOpen, 3);
        this.BtnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.BtnOpen.Location = new System.Drawing.Point(449, 3);
        this.BtnOpen.Name = "BtnOpen";
        this.BtnOpen.Size = new System.Drawing.Size(106, 34);
        this.BtnOpen.TabIndex = 0;
        this.BtnOpen.Text = "Открыть";
        this.BtnOpen.UseVisualStyleBackColor = true;
        this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
        // 
        // ListLogs
        // 
        this.tableLayoutPanel1.SetColumnSpan(this.ListLogs, 3);
        this.ListLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ListLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.ListLogs.FormattingEnabled = true;
        this.ListLogs.HorizontalScrollbar = true;
        this.ListLogs.ItemHeight = 18;
        this.ListLogs.Location = new System.Drawing.Point(5, 118);
        this.ListLogs.Margin = new System.Windows.Forms.Padding(5);
        this.ListLogs.Name = "ListLogs";
        this.ListLogs.Size = new System.Drawing.Size(995, 372);
        this.ListLogs.TabIndex = 3;
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 3;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
        this.tableLayoutPanel1.Controls.Add(this.progressBar, 1, 5);
        this.tableLayoutPanel1.Controls.Add(this.labelInfo, 1, 4);
        this.tableLayoutPanel1.Controls.Add(this.BtnOpen, 1, 0);
        this.tableLayoutPanel1.Controls.Add(this.ListLogs, 1, 3);
        this.tableLayoutPanel1.Controls.Add(this.txtFolderPath, 1, 1);
        this.tableLayoutPanel1.Controls.Add(this.BtnCheck, 1, 2);
        this.tableLayoutPanel1.Controls.Add(this.labelTimer, 2, 5);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 4;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(1005, 545);
        this.tableLayoutPanel1.TabIndex = 8;
        // 
        // progressBar
        // 
        this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.progressBar.Location = new System.Drawing.Point(204, 508);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(597, 23);
        this.progressBar.TabIndex = 10;
        // 
        // labelInfo
        // 
        this.labelInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.labelInfo.Location = new System.Drawing.Point(3, 495);
        this.labelInfo.Name = "labelInfo";
        this.labelInfo.Size = new System.Drawing.Size(176, 50);
        this.labelInfo.TabIndex = 10;
        this.labelInfo.Text = "Проверено:\nпапок:\nфайлов:";
        // 
        // labelTimer
        // 
        this.labelTimer.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.labelTimer.Location = new System.Drawing.Point(902, 508);
        this.labelTimer.Name = "labelTimer";
        this.labelTimer.Size = new System.Drawing.Size(100, 23);
        this.labelTimer.TabIndex = 11;
        this.labelTimer.Text = "00:00:00:000";
        // 
        // toolStripProgressBar1
        // 
        this.toolStripProgressBar1.Name = "toolStripProgressBar1";
        this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 15);
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.менюToolStripMenuItem, this.настройкиToolStripMenuItem });
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(1005, 27);
        this.menuStrip1.TabIndex = 9;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // менюToolStripMenuItem
        // 
        this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.ExcelTool });
        this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
        this.менюToolStripMenuItem.Size = new System.Drawing.Size(60, 23);
        this.менюToolStripMenuItem.Text = "Меню";
        // 
        // ExcelTool
        // 
        this.ExcelTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.ExcelTool.Name = "ExcelTool";
        this.ExcelTool.Size = new System.Drawing.Size(174, 24);
        this.ExcelTool.Text = "Экспорт в Excel";
        this.ExcelTool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.ExcelTool.Click += new System.EventHandler(this.ExportToExcelClick);
        // 
        // настройкиToolStripMenuItem
        // 
        this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
        this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(89, 23);
        this.настройкиToolStripMenuItem.Text = "Настройки";
        this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
        // 
        // Timer1
        // 
        this.Timer1.Tick += new System.EventHandler(this.TimerTick);
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.Control;
        this.ClientSize = new System.Drawing.Size(1005, 572);
        this.Controls.Add(this.tableLayoutPanel1);
        this.Controls.Add(this.menuStrip1);
        this.ForeColor = System.Drawing.SystemColors.Desktop;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Location = new System.Drawing.Point(15, 15);
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "MainForm";
        this.Text = "NTFSChecker";
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel1.PerformLayout();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Label labelTimer;

    private System.Windows.Forms.Label labelInfo;

    private System.Windows.Forms.ToolStripMenuItem ExcelTool;

    private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;

    private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;

    private System.Windows.Forms.MenuStrip menuStrip1;

    private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    private System.Windows.Forms.ListBox ListLogs;
        
    private System.Windows.Forms.Timer Timer1;

    #endregion


        

    private System.Windows.Forms.Button BtnOpen;
    private System.Windows.Forms.TextBox txtFolderPath;
    private System.Windows.Forms.Button BtnCheck;
}