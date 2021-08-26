namespace ContentQuery
{
    partial class QueryForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtpath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtnr = new System.Windows.Forms.TextBox();
            this.but_search = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbo_xls = new System.Windows.Forms.CheckBox();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.txthz = new System.Windows.Forms.TextBox();
            this.cbo_md = new System.Windows.Forms.CheckBox();
            this.cbo_doc = new System.Windows.Forms.CheckBox();
            this.cbotxt = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pNr = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labjg = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labmess = new System.Windows.Forms.Label();
            this.cbo_type = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件夹：";
            // 
            // txtpath
            // 
            this.txtpath.BackColor = System.Drawing.SystemColors.Control;
            this.txtpath.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtpath.Location = new System.Drawing.Point(88, 13);
            this.txtpath.Name = "txtpath";
            this.txtpath.Size = new System.Drawing.Size(157, 22);
            this.txtpath.TabIndex = 1;
            this.txtpath.Text = "C:\\";
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Location = new System.Drawing.Point(262, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtnr
            // 
            this.txtnr.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtnr.Location = new System.Drawing.Point(38, 49);
            this.txtnr.Name = "txtnr";
            this.txtnr.Size = new System.Drawing.Size(142, 23);
            this.txtnr.TabIndex = 0;
            // 
            // but_search
            // 
            this.but_search.Cursor = System.Windows.Forms.Cursors.Hand;
            this.but_search.Location = new System.Drawing.Point(276, 45);
            this.but_search.Name = "but_search";
            this.but_search.Size = new System.Drawing.Size(59, 31);
            this.but_search.TabIndex = 2;
            this.but_search.Text = "搜索";
            this.but_search.UseVisualStyleBackColor = true;
            this.but_search.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbo_xls);
            this.groupBox1.Controls.Add(this.linkLabel3);
            this.groupBox1.Controls.Add(this.txthz);
            this.groupBox1.Controls.Add(this.cbo_md);
            this.groupBox1.Controls.Add(this.cbo_doc);
            this.groupBox1.Controls.Add(this.cbotxt);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Location = new System.Drawing.Point(14, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 47);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "内容文件后缀";
            // 
            // cbo_xls
            // 
            this.cbo_xls.AutoSize = true;
            this.cbo_xls.Checked = true;
            this.cbo_xls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbo_xls.Location = new System.Drawing.Point(114, 19);
            this.cbo_xls.Name = "cbo_xls";
            this.cbo_xls.Size = new System.Drawing.Size(48, 21);
            this.cbo_xls.TabIndex = 3;
            this.cbo_xls.Text = "xlsx";
            this.cbo_xls.UseVisualStyleBackColor = true;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Cursor = System.Windows.Forms.Cursors.Help;
            this.linkLabel3.LinkColor = System.Drawing.Color.Maroon;
            this.linkLabel3.Location = new System.Drawing.Point(299, 21);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(32, 17);
            this.linkLabel3.TabIndex = 2;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "帮助";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // txthz
            // 
            this.txthz.BackColor = System.Drawing.SystemColors.Control;
            this.txthz.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txthz.Location = new System.Drawing.Point(214, 18);
            this.txthz.Name = "txthz";
            this.txthz.Size = new System.Drawing.Size(81, 23);
            this.txthz.TabIndex = 1;
            this.txthz.Text = "java|vue|sql";
            // 
            // cbo_md
            // 
            this.cbo_md.AutoSize = true;
            this.cbo_md.Checked = true;
            this.cbo_md.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbo_md.Location = new System.Drawing.Point(162, 19);
            this.cbo_md.Name = "cbo_md";
            this.cbo_md.Size = new System.Drawing.Size(46, 21);
            this.cbo_md.TabIndex = 0;
            this.cbo_md.Text = "md";
            this.cbo_md.UseVisualStyleBackColor = true;
            // 
            // cbo_doc
            // 
            this.cbo_doc.AutoSize = true;
            this.cbo_doc.Checked = true;
            this.cbo_doc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbo_doc.Location = new System.Drawing.Point(59, 19);
            this.cbo_doc.Name = "cbo_doc";
            this.cbo_doc.Size = new System.Drawing.Size(55, 21);
            this.cbo_doc.TabIndex = 0;
            this.cbo_doc.Text = "docx";
            this.cbo_doc.UseVisualStyleBackColor = true;
            // 
            // cbotxt
            // 
            this.cbotxt.AutoSize = true;
            this.cbotxt.Checked = true;
            this.cbotxt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbotxt.Location = new System.Drawing.Point(15, 19);
            this.cbotxt.Name = "cbotxt";
            this.cbotxt.Size = new System.Drawing.Size(41, 21);
            this.cbotxt.TabIndex = 0;
            this.cbotxt.Text = "txt";
            this.cbotxt.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pNr);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(12, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 339);
            this.panel1.TabIndex = 5;
            // 
            // pNr
            // 
            this.pNr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pNr.Location = new System.Drawing.Point(0, 35);
            this.pNr.Name = "pNr";
            this.pNr.Size = new System.Drawing.Size(346, 302);
            this.pNr.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.linkLabel2);
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Controls.Add(this.labjg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(346, 35);
            this.panel2.TabIndex = 0;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel2.LinkColor = System.Drawing.Color.Navy;
            this.linkLabel2.Location = new System.Drawing.Point(283, 10);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(44, 17);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "下一页";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
            this.linkLabel1.Location = new System.Drawing.Point(223, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(44, 17);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "上一页";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labjg
            // 
            this.labjg.AutoSize = true;
            this.labjg.Location = new System.Drawing.Point(9, 9);
            this.labjg.Name = "labjg";
            this.labjg.Size = new System.Drawing.Size(106, 17);
            this.labjg.TabIndex = 0;
            this.labjg.Text = "查找结果(0)： 1/1";
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labmess
            // 
            this.labmess.AutoSize = true;
            this.labmess.Font = new System.Drawing.Font("楷体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labmess.ForeColor = System.Drawing.Color.Maroon;
            this.labmess.Location = new System.Drawing.Point(8, 105);
            this.labmess.Name = "labmess";
            this.labmess.Size = new System.Drawing.Size(0, 21);
            this.labmess.TabIndex = 6;
            // 
            // cbo_type
            // 
            this.cbo_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_type.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_type.FormattingEnabled = true;
            this.cbo_type.Items.AddRange(new object[] {
            "文件名/内容",
            "内容",
            "文件名"});
            this.cbo_type.Location = new System.Drawing.Point(186, 48);
            this.cbo_type.Name = "cbo_type";
            this.cbo_type.Size = new System.Drawing.Size(84, 24);
            this.cbo_type.TabIndex = 7;
            // 
            // QueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(372, 487);
            this.Controls.Add(this.cbo_type);
            this.Controls.Add(this.labmess);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtnr);
            this.Controls.Add(this.but_search);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtpath);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "QueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找相关内容的文件";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtpath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtnr;
        private System.Windows.Forms.Button but_search;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txthz;
        private System.Windows.Forms.CheckBox cbo_md;
        private System.Windows.Forms.CheckBox cbo_doc;
        private System.Windows.Forms.CheckBox cbotxt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pNr;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label labjg;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.CheckBox cbo_xls;
        private System.Windows.Forms.Label labmess;
        private System.Windows.Forms.ComboBox cbo_type;
    }
}

