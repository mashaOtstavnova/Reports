namespace Reports
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.createExel = new System.Windows.Forms.Button();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // createExel
            // 
            this.createExel.Dock = System.Windows.Forms.DockStyle.Right;
            this.createExel.Location = new System.Drawing.Point(822, 3);
            this.createExel.Name = "createExel";
            this.createExel.Size = new System.Drawing.Size(75, 32);
            this.createExel.TabIndex = 1;
            this.createExel.Text = "Exel";
            this.createExel.UseVisualStyleBackColor = true;
            this.createExel.Click += new System.EventHandler(this.createExel_Click);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 41);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(894, 463);
            this.splitContainerControl1.SplitterPosition = 205;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainerControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.createExel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.675438F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.32456F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 507);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 507);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainView";
            this.Text = "MainView";
            this.Load += new System.EventHandler(this.MainView_Load);
            this.SizeChanged += new System.EventHandler(this.MainView_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button createExel;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}