using System.ComponentModel;

namespace NTFSChecker.WinForms;

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
        this.DomainTabPage = new System.Windows.Forms.TabPage();
        this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        this.DomainACLabel = new System.Windows.Forms.Label();
        this.DomainTextBox = new System.Windows.Forms.TextBox();
        this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
        this.BtnOk = new System.Windows.Forms.Button();
        this.SettingsTabControl.SuspendLayout();
        this.ExportTabSettings.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.ColorTabSettings.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.DomainTabPage.SuspendLayout();
        this.tableLayoutPanel3.SuspendLayout();
        this.tableLayoutPanel4.SuspendLayout();
        this.SuspendLayout();
        // 
        // SettingsTabControl
        // 
        this.SettingsTabControl.Controls.Add(this.ExportTabSettings);
        this.SettingsTabControl.Controls.Add(this.ColorTabSettings);
        this.SettingsTabControl.Controls.Add(this.DomainTabPage);
        this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        this.SettingsTabControl.Location = new System.Drawing.Point(3, 3);
        this.SettingsTabControl.Name = "SettingsTabControl";
        this.SettingsTabControl.SelectedIndex = 0;
        this.SettingsTabControl.Size = new System.Drawing.Size(522, 208);
        this.SettingsTabControl.TabIndex = 0;
        // 
        // ExportTabSettings
        // 
        this.ExportTabSettings.Controls.Add(this.tableLayoutPanel1);
        this.ExportTabSettings.Location = new System.Drawing.Point(4, 22);
        this.ExportTabSettings.Name = "ExportTabSettings";
        this.ExportTabSettings.Padding = new System.Windows.Forms.Padding(3);
        this.ExportTabSettings.Size = new System.Drawing.Size(514, 182);
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
        this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 101);
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
        this.ColorTabSettings.Size = new System.Drawing.Size(514, 182);
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
        this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 3;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel2.Size = new System.Drawing.Size(508, 176);
        this.tableLayoutPanel2.TabIndex = 7;
        // 
        // ColorRight
        // 
        this.ColorRight.BackColor = System.Drawing.Color.Purple;
        this.ColorRight.Location = new System.Drawing.Point(409, 116);
        this.ColorRight.Name = "ColorRight";
        this.ColorRight.Size = new System.Drawing.Size(75, 20);
        this.ColorRight.TabIndex = 5;
        this.ColorRight.Click += new System.EventHandler(this.ColorRight_Click);
        // 
        // LabelWorngRights
        // 
        this.LabelWorngRights.AutoSize = true;
        this.LabelWorngRights.Location = new System.Drawing.Point(3, 116);
        this.LabelWorngRights.Name = "LabelWorngRights";
        this.LabelWorngRights.Size = new System.Drawing.Size(96, 26);
        this.LabelWorngRights.TabIndex = 2;
        this.LabelWorngRights.Text = "Отличия в правах\r\n\r\n";
        // 
        // ColorCur
        // 
        this.ColorCur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
        this.ColorCur.Location = new System.Drawing.Point(409, 58);
        this.ColorCur.Name = "ColorCur";
        this.ColorCur.Size = new System.Drawing.Size(75, 20);
        this.ColorCur.TabIndex = 4;
        this.ColorCur.Click += new System.EventHandler(this.ColorCur_Click);
        // 
        // LabelMissinginCur
        // 
        this.LabelMissinginCur.AutoSize = true;
        this.LabelMissinginCur.Location = new System.Drawing.Point(3, 58);
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
        this.ColorMain.Location = new System.Drawing.Point(409, 0);
        this.ColorMain.Name = "ColorMain";
        this.ColorMain.Size = new System.Drawing.Size(75, 20);
        this.ColorMain.TabIndex = 3;
        this.ColorMain.Click += new System.EventHandler(this.ColorMain_Click);
        // 
        // DomainTabPage
        // 
        this.DomainTabPage.Controls.Add(this.tableLayoutPanel3);
        this.DomainTabPage.Location = new System.Drawing.Point(4, 22);
        this.DomainTabPage.Name = "DomainTabPage";
        this.DomainTabPage.Padding = new System.Windows.Forms.Padding(3);
        this.DomainTabPage.Size = new System.Drawing.Size(514, 182);
        this.DomainTabPage.TabIndex = 2;
        this.DomainTabPage.Text = "Домен";
        this.DomainTabPage.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel3
        // 
        this.tableLayoutPanel3.ColumnCount = 2;
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.62992F));
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.37008F));
        this.tableLayoutPanel3.Controls.Add(this.DomainACLabel, 0, 0);
        this.tableLayoutPanel3.Controls.Add(this.DomainTextBox, 1, 0);
        this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
        this.tableLayoutPanel3.Name = "tableLayoutPanel3";
        this.tableLayoutPanel3.RowCount = 1;
        this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel3.Size = new System.Drawing.Size(508, 176);
        this.tableLayoutPanel3.TabIndex = 2;
        // 
        // DomainACLabel
        // 
        this.DomainACLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.DomainACLabel.AutoSize = true;
        this.DomainACLabel.Location = new System.Drawing.Point(5, 5);
        this.DomainACLabel.Margin = new System.Windows.Forms.Padding(5);
        this.DomainACLabel.Name = "DomainACLabel";
        this.DomainACLabel.Size = new System.Drawing.Size(43, 13);
        this.DomainACLabel.TabIndex = 1;
        this.DomainACLabel.Text = "Домен";
        // 
        // DomainTextBox
        // 
        this.DomainTextBox.Location = new System.Drawing.Point(56, 3);
        this.DomainTextBox.Name = "DomainTextBox";
        this.DomainTextBox.Size = new System.Drawing.Size(434, 20);
        this.DomainTextBox.TabIndex = 0;
        // 
        // tableLayoutPanel4
        // 
        this.tableLayoutPanel4.ColumnCount = 1;
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel4.Controls.Add(this.SettingsTabControl, 0, 0);
        this.tableLayoutPanel4.Controls.Add(this.BtnOk, 0, 1);
        this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel4.Name = "tableLayoutPanel4";
        this.tableLayoutPanel4.RowCount = 2;
        this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
        this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
        this.tableLayoutPanel4.Size = new System.Drawing.Size(528, 252);
        this.tableLayoutPanel4.TabIndex = 1;
        // 
        // BtnOk
        // 
        this.BtnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.BtnOk.Location = new System.Drawing.Point(450, 221);
        this.BtnOk.Name = "BtnOk";
        this.BtnOk.Size = new System.Drawing.Size(75, 23);
        this.BtnOk.TabIndex = 1;
        this.BtnOk.Text = "OK";
        this.BtnOk.UseVisualStyleBackColor = true;
        this.BtnOk.Click += new System.EventHandler(this.BtnOK_Click);
        // 
        // SettingsForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(528, 252);
        this.Controls.Add(this.tableLayoutPanel4);
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
        this.tableLayoutPanel4.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Button BtnOk;

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;

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

    public System.Windows.Forms.CheckBox AllExportCheckBox;
    private System.Windows.Forms.CheckBox IgnoreUndefinedCheckBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    private System.Windows.Forms.TabPage DomainTabPage;

    private System.Windows.Forms.TabControl SettingsTabControl;
    private System.Windows.Forms.TabPage ExportTabSettings;
    private System.Windows.Forms.TabPage ColorTabSettings;

    #endregion
}