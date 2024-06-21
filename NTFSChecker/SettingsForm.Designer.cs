using System.ComponentModel;

namespace NTFSChecker;

partial class SettingsForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
        this.SettingsTabControl = new System.Windows.Forms.TabControl();
        this.ExportTabSettings = new System.Windows.Forms.TabPage();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.IgnoreUndefinedCheckBox = new System.Windows.Forms.CheckBox();
        this.AllExportCheckBox = new System.Windows.Forms.CheckBox();
        this.ColorTabSettings = new System.Windows.Forms.TabPage();
        this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        this.ColorRight = new System.Windows.Forms.Label();
        this.LabelWorngRights = new System.Windows.Forms.Label();
        this.ColorCur = new System.Windows.Forms.Label();
        this.LabelMissinginCur = new System.Windows.Forms.Label();
        this.LabelMissinginMain = new System.Windows.Forms.Label();
        this.ColorMain = new System.Windows.Forms.Label();
        this.BtnOK = new System.Windows.Forms.Button();
        this.DomainTabPage = new System.Windows.Forms.TabPage();
        this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        this.DomainACLabel = new System.Windows.Forms.Label();
        this.DomainTextBox = new System.Windows.Forms.TextBox();
        this.BtnDomainOk = new System.Windows.Forms.Button();
        this.SettingsTabControl.SuspendLayout();
        this.ExportTabSettings.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.ColorTabSettings.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.DomainTabPage.SuspendLayout();
        this.tableLayoutPanel3.SuspendLayout();
        this.SuspendLayout();
        // 
        // SettingsTabControl
        // 
        this.SettingsTabControl.Controls.Add(this.ExportTabSettings);
        this.SettingsTabControl.Controls.Add(this.ColorTabSettings);
        this.SettingsTabControl.Controls.Add(this.DomainTabPage);
        this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        this.SettingsTabControl.Location = new System.Drawing.Point(0, 0);
        this.SettingsTabControl.Name = "SettingsTabControl";
        this.SettingsTabControl.SelectedIndex = 0;
        this.SettingsTabControl.Size = new System.Drawing.Size(486, 156);
        this.SettingsTabControl.TabIndex = 0;
        // 
        // ExportTabSettings
        // 
        this.ExportTabSettings.Controls.Add(this.tableLayoutPanel1);
        this.ExportTabSettings.Location = new System.Drawing.Point(4, 22);
        this.ExportTabSettings.Name = "ExportTabSettings";
        this.ExportTabSettings.Padding = new System.Windows.Forms.Padding(3);
        this.ExportTabSettings.Size = new System.Drawing.Size(478, 130);
        this.ExportTabSettings.TabIndex = 0;
        this.ExportTabSettings.Text = "Экспорт";
        this.ExportTabSettings.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 1;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel1.Controls.Add(this.IgnoreUndefinedCheckBox, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.AllExportCheckBox, 0, 0);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 2;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 101);
        this.tableLayoutPanel1.TabIndex = 2;
        // 
        // IgnoreUndefinedCheckBox
        // 
        this.IgnoreUndefinedCheckBox.AutoSize = true;
        this.IgnoreUndefinedCheckBox.Location = new System.Drawing.Point(3, 53);
        this.IgnoreUndefinedCheckBox.Name = "IgnoreUndefinedCheckBox";
        this.IgnoreUndefinedCheckBox.Size = new System.Drawing.Size(220, 17);
        this.IgnoreUndefinedCheckBox.TabIndex = 1;
        this.IgnoreUndefinedCheckBox.Text = "Игнорировать неопределенные права";
        this.IgnoreUndefinedCheckBox.UseVisualStyleBackColor = true;
        this.IgnoreUndefinedCheckBox.CheckedChanged += new System.EventHandler(this.IgnoreUndefinedCheckedChanged);
        // 
        // AllExportCheckBox
        // 
        this.AllExportCheckBox.AutoSize = true;
        this.AllExportCheckBox.Location = new System.Drawing.Point(3, 3);
        this.AllExportCheckBox.Name = "AllExportCheckBox";
        this.AllExportCheckBox.Size = new System.Drawing.Size(130, 17);
        this.AllExportCheckBox.TabIndex = 0;
        this.AllExportCheckBox.Text = "Экспортировать все";
        this.AllExportCheckBox.UseVisualStyleBackColor = true;
        this.AllExportCheckBox.CheckedChanged += new System.EventHandler(this.ExportAllCheckedChanged);
        // 
        // ColorTabSettings
        // 
        this.ColorTabSettings.Controls.Add(this.tableLayoutPanel2);
        this.ColorTabSettings.Location = new System.Drawing.Point(4, 22);
        this.ColorTabSettings.Name = "ColorTabSettings";
        this.ColorTabSettings.Padding = new System.Windows.Forms.Padding(3);
        this.ColorTabSettings.Size = new System.Drawing.Size(478, 130);
        this.ColorTabSettings.TabIndex = 1;
        this.ColorTabSettings.Text = "Цвета";
        this.ColorTabSettings.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel2
        // 
        this.tableLayoutPanel2.ColumnCount = 2;
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
        this.tableLayoutPanel2.Controls.Add(this.ColorRight, 1, 2);
        this.tableLayoutPanel2.Controls.Add(this.LabelWorngRights, 0, 2);
        this.tableLayoutPanel2.Controls.Add(this.ColorCur, 1, 1);
        this.tableLayoutPanel2.Controls.Add(this.LabelMissinginCur, 0, 1);
        this.tableLayoutPanel2.Controls.Add(this.LabelMissinginMain, 0, 0);
        this.tableLayoutPanel2.Controls.Add(this.ColorMain, 1, 0);
        this.tableLayoutPanel2.Controls.Add(this.BtnOK, 1, 3);
        this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 4;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.Size = new System.Drawing.Size(472, 118);
        this.tableLayoutPanel2.TabIndex = 7;
        // 
        // ColorRight
        // 
        this.ColorRight.BackColor = System.Drawing.Color.Purple;
        this.ColorRight.Location = new System.Drawing.Point(380, 58);
        this.ColorRight.Name = "ColorRight";
        this.ColorRight.Size = new System.Drawing.Size(75, 20);
        this.ColorRight.TabIndex = 5;
        this.ColorRight.Click += new System.EventHandler(this.ColorRight_Click);
        // 
        // LabelWorngRights
        // 
        this.LabelWorngRights.AutoSize = true;
        this.LabelWorngRights.Location = new System.Drawing.Point(3, 58);
        this.LabelWorngRights.Name = "LabelWorngRights";
        this.LabelWorngRights.Size = new System.Drawing.Size(96, 26);
        this.LabelWorngRights.TabIndex = 2;
        this.LabelWorngRights.Text = "Отличия в правах\r\n\r\n";
        // 
        // ColorCur
        // 
        this.ColorCur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
        this.ColorCur.Location = new System.Drawing.Point(380, 29);
        this.ColorCur.Name = "ColorCur";
        this.ColorCur.Size = new System.Drawing.Size(75, 20);
        this.ColorCur.TabIndex = 4;
        this.ColorCur.Click += new System.EventHandler(this.ColorCur_Click);
        // 
        // LabelMissinginCur
        // 
        this.LabelMissinginCur.AutoSize = true;
        this.LabelMissinginCur.Location = new System.Drawing.Point(3, 29);
        this.LabelMissinginCur.Name = "LabelMissinginCur";
        this.LabelMissinginCur.Size = new System.Drawing.Size(256, 13);
        this.LabelMissinginCur.TabIndex = 1;
        this.LabelMissinginCur.Text = "Группы пользователей нет у дочернего каталога";
        // 
        // LabelMissinginMain
        // 
        this.LabelMissinginMain.AutoSize = true;
        this.LabelMissinginMain.Location = new System.Drawing.Point(3, 0);
        this.LabelMissinginMain.Name = "LabelMissinginMain";
        this.LabelMissinginMain.Size = new System.Drawing.Size(257, 13);
        this.LabelMissinginMain.TabIndex = 0;
        this.LabelMissinginMain.Text = "Группы пользователей нет у корневого каталога";
        // 
        // ColorMain
        // 
        this.ColorMain.BackColor = System.Drawing.Color.Red;
        this.ColorMain.Location = new System.Drawing.Point(380, 0);
        this.ColorMain.Name = "ColorMain";
        this.ColorMain.Size = new System.Drawing.Size(75, 20);
        this.ColorMain.TabIndex = 3;
        this.ColorMain.Click += new System.EventHandler(this.ColorMain_Click);
        // 
        // BtnOK
        // 
        this.BtnOK.Location = new System.Drawing.Point(380, 90);
        this.BtnOK.Name = "BtnOK";
        this.BtnOK.Size = new System.Drawing.Size(84, 23);
        this.BtnOK.TabIndex = 6;
        this.BtnOK.Text = "ОK";
        this.BtnOK.UseVisualStyleBackColor = true;
        this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
        // 
        // DomainTabPage
        // 
        this.DomainTabPage.Controls.Add(this.tableLayoutPanel3);
        this.DomainTabPage.Location = new System.Drawing.Point(4, 22);
        this.DomainTabPage.Name = "DomainTabPage";
        this.DomainTabPage.Padding = new System.Windows.Forms.Padding(3);
        this.DomainTabPage.Size = new System.Drawing.Size(478, 130);
        this.DomainTabPage.TabIndex = 2;
        this.DomainTabPage.Text = "Домен";
        this.DomainTabPage.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel3
        // 
        this.tableLayoutPanel3.ColumnCount = 2;
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.90678F));
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.09322F));
        this.tableLayoutPanel3.Controls.Add(this.DomainACLabel, 0, 0);
        this.tableLayoutPanel3.Controls.Add(this.DomainTextBox, 1, 0);
        this.tableLayoutPanel3.Controls.Add(this.BtnDomainOk, 1, 1);
        this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
        this.tableLayoutPanel3.Name = "tableLayoutPanel3";
        this.tableLayoutPanel3.RowCount = 2;
        this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
        this.tableLayoutPanel3.Size = new System.Drawing.Size(472, 124);
        this.tableLayoutPanel3.TabIndex = 2;
        // 
        // DomainACLabel
        // 
        this.DomainACLabel.AutoSize = true;
        this.DomainACLabel.Location = new System.Drawing.Point(3, 0);
        this.DomainACLabel.Name = "DomainACLabel";
        this.DomainACLabel.Size = new System.Drawing.Size(42, 13);
        this.DomainACLabel.TabIndex = 1;
        this.DomainACLabel.Text = "Домен";
        // 
        // DomainTextBox
        // 
        this.DomainTextBox.Location = new System.Drawing.Point(130, 3);
        this.DomainTextBox.Name = "DomainTextBox";
        this.DomainTextBox.Size = new System.Drawing.Size(339, 20);
        this.DomainTextBox.TabIndex = 0;
        // 
        // BtnDomainOk
        // 
        this.BtnDomainOk.Dock = System.Windows.Forms.DockStyle.Right;
        this.BtnDomainOk.Location = new System.Drawing.Point(394, 88);
        this.BtnDomainOk.Name = "BtnDomainOk";
        this.BtnDomainOk.Size = new System.Drawing.Size(75, 33);
        this.BtnDomainOk.TabIndex = 2;
        this.BtnDomainOk.Text = "OK";
        this.BtnDomainOk.UseVisualStyleBackColor = true;
        this.BtnDomainOk.Click += new System.EventHandler(this.BtnDomainOk_Click);
        // 
        // SettingsForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(486, 156);
        this.Controls.Add(this.SettingsTabControl);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "SettingsForm";
        this.Text = "Настройки";
        this.SettingsTabControl.ResumeLayout(false);
        this.ExportTabSettings.ResumeLayout(false);
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel1.PerformLayout();
        this.ColorTabSettings.ResumeLayout(false);
        this.tableLayoutPanel2.ResumeLayout(false);
        this.tableLayoutPanel2.PerformLayout();
        this.DomainTabPage.ResumeLayout(false);
        this.tableLayoutPanel3.ResumeLayout(false);
        this.tableLayoutPanel3.PerformLayout();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Button BtnDomainOk;

    private System.Windows.Forms.TextBox DomainTextBox;
    private System.Windows.Forms.Label DomainACLabel;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Label ColorRight;
    private System.Windows.Forms.Label LabelWorngRights;
    private System.Windows.Forms.Label ColorCur;
    private System.Windows.Forms.Label LabelMissinginCur;
    private System.Windows.Forms.Label LabelMissinginMain;
    private System.Windows.Forms.Label ColorMain;
    private System.Windows.Forms.Button BtnOK;

    public System.Windows.Forms.CheckBox AllExportCheckBox;
    private System.Windows.Forms.CheckBox IgnoreUndefinedCheckBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    private System.Windows.Forms.TabPage DomainTabPage;

    private System.Windows.Forms.TabControl SettingsTabControl;
    private System.Windows.Forms.TabPage ExportTabSettings;
    private System.Windows.Forms.TabPage ColorTabSettings;

    #endregion
}