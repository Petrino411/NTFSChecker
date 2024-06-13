using System.ComponentModel;

namespace NTFSChecker;

partial class ColorSettingsForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSettingsForm));
        this.LabelMissinginMain = new System.Windows.Forms.Label();
        this.LabelMissinginCur = new System.Windows.Forms.Label();
        this.LabelWorngRights = new System.Windows.Forms.Label();
        this.ColorMain = new System.Windows.Forms.Label();
        this.ColorCur = new System.Windows.Forms.Label();
        this.ColorRight = new System.Windows.Forms.Label();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.BtnOK = new System.Windows.Forms.Button();
        this.tableLayoutPanel1.SuspendLayout();
        this.SuspendLayout();
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
        // LabelMissinginCur
        // 
        this.LabelMissinginCur.AutoSize = true;
        this.LabelMissinginCur.Location = new System.Drawing.Point(3, 29);
        this.LabelMissinginCur.Name = "LabelMissinginCur";
        this.LabelMissinginCur.Size = new System.Drawing.Size(256, 13);
        this.LabelMissinginCur.TabIndex = 1;
        this.LabelMissinginCur.Text = "Группы пользователей нет у дочернего каталога";
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
        // ColorMain
        // 
        this.ColorMain.BackColor = System.Drawing.Color.Red;
        this.ColorMain.Location = new System.Drawing.Point(359, 0);
        this.ColorMain.Name = "ColorMain";
        this.ColorMain.Size = new System.Drawing.Size(75, 20);
        this.ColorMain.TabIndex = 3;
        this.ColorMain.Click += new System.EventHandler(this.ColorMain_Click);
        // 
        // ColorCur
        // 
        this.ColorCur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
        this.ColorCur.Location = new System.Drawing.Point(359, 29);
        this.ColorCur.Name = "ColorCur";
        this.ColorCur.Size = new System.Drawing.Size(75, 20);
        this.ColorCur.TabIndex = 4;
        this.ColorCur.Click += new System.EventHandler(this.ColorCur_Click);
        // 
        // ColorRight
        // 
        this.ColorRight.BackColor = System.Drawing.Color.Purple;
        this.ColorRight.Location = new System.Drawing.Point(359, 58);
        this.ColorRight.Name = "ColorRight";
        this.ColorRight.Size = new System.Drawing.Size(75, 20);
        this.ColorRight.TabIndex = 5;
        this.ColorRight.Click += new System.EventHandler(this.ColorRight_Click);
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 2;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
        this.tableLayoutPanel1.Controls.Add(this.ColorRight, 1, 2);
        this.tableLayoutPanel1.Controls.Add(this.LabelWorngRights, 0, 2);
        this.tableLayoutPanel1.Controls.Add(this.ColorCur, 1, 1);
        this.tableLayoutPanel1.Controls.Add(this.LabelMissinginCur, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.LabelMissinginMain, 0, 0);
        this.tableLayoutPanel1.Controls.Add(this.ColorMain, 1, 0);
        this.tableLayoutPanel1.Controls.Add(this.BtnOK, 1, 3);
        this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 4;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(446, 118);
        this.tableLayoutPanel1.TabIndex = 6;
        // 
        // BtnOK
        // 
        this.BtnOK.Location = new System.Drawing.Point(359, 90);
        this.BtnOK.Name = "BtnOK";
        this.BtnOK.Size = new System.Drawing.Size(84, 23);
        this.BtnOK.TabIndex = 6;
        this.BtnOK.Text = "ОK";
        this.BtnOK.UseVisualStyleBackColor = true;
        this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
        // 
        // ColorSettingsForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(469, 140);
        this.Controls.Add(this.tableLayoutPanel1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "ColorSettingsForm";
        this.Text = "Настройка цветов";
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel1.PerformLayout();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Button BtnOK;

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    private System.Windows.Forms.Label LabelMissinginMain;
    private System.Windows.Forms.Label LabelMissinginCur;
    private System.Windows.Forms.Label LabelWorngRights;
    private System.Windows.Forms.Label ColorMain;
    private System.Windows.Forms.Label ColorCur;
    private System.Windows.Forms.Label ColorRight;

    #endregion
}