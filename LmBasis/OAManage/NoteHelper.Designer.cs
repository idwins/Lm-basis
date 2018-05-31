namespace OAManage
{
    partial class NoteHelper
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtUName = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.labState = new System.Windows.Forms.Label();
            this.webBrow = new System.Windows.Forms.WebBrowser();
            this.dateB = new System.Windows.Forms.DateTimePicker();
            this.dateE = new System.Windows.Forms.DateTimePicker();
            this.btnWork = new System.Windows.Forms.Button();
            this.IsWebB = new System.Windows.Forms.Button();
            this.dGv = new System.Windows.Forms.DataGridView();
            this.content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnInWord = new System.Windows.Forms.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtWorkMsg = new System.Windows.Forms.TextBox();
            this.btnRemark = new System.Windows.Forms.Button();
            this.btnZj = new System.Windows.Forms.Button();
            this.btnSearchWeek = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dGv)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(10, 36);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(180, 21);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "http://oa.wangan.cn/index.aspx";
            // 
            // txtUName
            // 
            this.txtUName.Location = new System.Drawing.Point(206, 36);
            this.txtUName.Name = "txtUName";
            this.txtUName.Size = new System.Drawing.Size(59, 21);
            this.txtUName.TabIndex = 1;
            this.txtUName.Text = "lim";
            // 
            // btnLogin
            // 
            this.btnLogin.ForeColor = System.Drawing.Color.Maroon;
            this.btnLogin.Location = new System.Drawing.Point(639, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "登    陆";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // labState
            // 
            this.labState.AutoSize = true;
            this.labState.Location = new System.Drawing.Point(12, 13);
            this.labState.Name = "labState";
            this.labState.Size = new System.Drawing.Size(71, 12);
            this.labState.TabIndex = 4;
            this.labState.Text = "labState...";
            // 
            // webBrow
            // 
            this.webBrow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrow.Location = new System.Drawing.Point(8, 63);
            this.webBrow.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrow.Name = "webBrow";
            this.webBrow.Size = new System.Drawing.Size(975, 597);
            this.webBrow.TabIndex = 5;
            this.webBrow.Visible = false;
            this.webBrow.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrow_DocumentCompleted);
            // 
            // dateB
            // 
            this.dateB.Location = new System.Drawing.Point(406, 36);
            this.dateB.Name = "dateB";
            this.dateB.Size = new System.Drawing.Size(110, 21);
            this.dateB.TabIndex = 6;
            // 
            // dateE
            // 
            this.dateE.CalendarForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.dateE.CalendarTitleForeColor = System.Drawing.SystemColors.Highlight;
            this.dateE.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateE.Location = new System.Drawing.Point(538, 36);
            this.dateE.Name = "dateE";
            this.dateE.Size = new System.Drawing.Size(126, 21);
            this.dateE.TabIndex = 7;
            // 
            // btnWork
            // 
            this.btnWork.Location = new System.Drawing.Point(670, 34);
            this.btnWork.Name = "btnWork";
            this.btnWork.Size = new System.Drawing.Size(75, 23);
            this.btnWork.TabIndex = 8;
            this.btnWork.Text = "查    询";
            this.btnWork.UseVisualStyleBackColor = true;
            this.btnWork.Click += new System.EventHandler(this.btnWork_Click);
            // 
            // IsWebB
            // 
            this.IsWebB.Location = new System.Drawing.Point(725, 8);
            this.IsWebB.Name = "IsWebB";
            this.IsWebB.Size = new System.Drawing.Size(75, 23);
            this.IsWebB.TabIndex = 9;
            this.IsWebB.Text = "ShowPage";
            this.IsWebB.UseVisualStyleBackColor = true;
            this.IsWebB.Click += new System.EventHandler(this.IsWebB_Click);
            // 
            // dGv
            // 
            this.dGv.AllowUserToAddRows = false;
            this.dGv.AllowUserToDeleteRows = false;
            this.dGv.AllowUserToResizeColumns = false;
            this.dGv.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dGv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.content,
            this.Date,
            this.dataGridViewTextBoxColumn1});
            this.dGv.Location = new System.Drawing.Point(11, 63);
            this.dGv.Name = "dGv";
            this.dGv.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dGv.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dGv.RowHeadersVisible = false;
            this.dGv.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dGv.RowTemplate.Height = 23;
            this.dGv.Size = new System.Drawing.Size(968, 597);
            this.dGv.TabIndex = 10;
            // 
            // content
            // 
            this.content.DataPropertyName = "Content";
            this.content.HeaderText = "描述";
            this.content.Name = "content";
            this.content.ReadOnly = true;
            this.content.Width = 200;
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Date";
            this.Date.HeaderText = "时间";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 130;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "WeekDay";
            this.dataGridViewTextBoxColumn1.HeaderText = "星期";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // btnInWord
            // 
            this.btnInWord.Enabled = false;
            this.btnInWord.Location = new System.Drawing.Point(897, 34);
            this.btnInWord.Name = "btnInWord";
            this.btnInWord.Size = new System.Drawing.Size(75, 23);
            this.btnInWord.TabIndex = 11;
            this.btnInWord.Text = "填    充";
            this.btnInWord.UseVisualStyleBackColor = true;
            this.btnInWord.Click += new System.EventHandler(this.btnInWord_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(283, 36);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(108, 21);
            this.txtPwd.TabIndex = 2;
            this.txtPwd.Text = "123123";
            // 
            // txtWorkMsg
            // 
            this.txtWorkMsg.Location = new System.Drawing.Point(265, 236);
            this.txtWorkMsg.Multiline = true;
            this.txtWorkMsg.Name = "txtWorkMsg";
            this.txtWorkMsg.Size = new System.Drawing.Size(454, 252);
            this.txtWorkMsg.TabIndex = 12;
            this.txtWorkMsg.Visible = false;
            // 
            // btnRemark
            // 
            this.btnRemark.Enabled = false;
            this.btnRemark.Location = new System.Drawing.Point(816, 34);
            this.btnRemark.Name = "btnRemark";
            this.btnRemark.Size = new System.Drawing.Size(75, 23);
            this.btnRemark.TabIndex = 13;
            this.btnRemark.Text = "生成描述";
            this.btnRemark.UseVisualStyleBackColor = true;
            this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
            // 
            // btnZj
            // 
            this.btnZj.ForeColor = System.Drawing.Color.Maroon;
            this.btnZj.Location = new System.Drawing.Point(897, 8);
            this.btnZj.Name = "btnZj";
            this.btnZj.Size = new System.Drawing.Size(75, 23);
            this.btnZj.TabIndex = 14;
            this.btnZj.Text = "总结提交";
            this.btnZj.UseVisualStyleBackColor = true;
            this.btnZj.Click += new System.EventHandler(this.btnZj_Click);
            // 
            // btnSearchWeek
            // 
            this.btnSearchWeek.ForeColor = System.Drawing.Color.Maroon;
            this.btnSearchWeek.Location = new System.Drawing.Point(811, 8);
            this.btnSearchWeek.Name = "btnSearchWeek";
            this.btnSearchWeek.Size = new System.Drawing.Size(75, 23);
            this.btnSearchWeek.TabIndex = 15;
            this.btnSearchWeek.Text = "总结查询";
            this.btnSearchWeek.UseVisualStyleBackColor = true;
            this.btnSearchWeek.Click += new System.EventHandler(this.btnSearchWeek_Click);
            // 
            // NoteHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.btnSearchWeek);
            this.Controls.Add(this.btnZj);
            this.Controls.Add(this.btnRemark);
            this.Controls.Add(this.txtWorkMsg);
            this.Controls.Add(this.btnInWord);
            this.Controls.Add(this.IsWebB);
            this.Controls.Add(this.btnWork);
            this.Controls.Add(this.dateE);
            this.Controls.Add(this.dateB);
            this.Controls.Add(this.webBrow);
            this.Controls.Add(this.labState);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUName);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.dGv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NoteHelper";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.NoteHelper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dGv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtUName;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label labState;
        private System.Windows.Forms.WebBrowser webBrow;
        private System.Windows.Forms.DateTimePicker dateB;
        private System.Windows.Forms.DateTimePicker dateE;
        private System.Windows.Forms.Button btnWork;
        private System.Windows.Forms.Button IsWebB;
        private System.Windows.Forms.DataGridView dGv;
        private System.Windows.Forms.DataGridViewTextBoxColumn content;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button btnInWord;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtWorkMsg;
        private System.Windows.Forms.Button btnRemark;
        private System.Windows.Forms.Button btnZj;
        private System.Windows.Forms.Button btnSearchWeek;
    }
}