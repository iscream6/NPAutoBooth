namespace NPConfig
{
    partial class AdminHTTP
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
            this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
            this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubject = new FadeFox.UI.Text3DLabel();
            this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new FadeFox.UI.GlassButton();
            this.btnOk = new FadeFox.UI.GlassButton();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.txtRESTfulLocalPort = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRESTfulServerPort = new System.Windows.Forms.TextBox();
            this.txtRESTfulServerIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtRESTfulVersion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlLayoutTop.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayoutTop
            // 
            this.pnlLayoutTop.BackColor = System.Drawing.SystemColors.Window;
            this.pnlLayoutTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
            this.pnlLayoutTop.Controls.Add(this.pnlCondition);
            this.pnlLayoutTop.Controls.Add(this.lblSubject);
            this.pnlLayoutTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLayoutTop.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlLayoutTop.GradientEnd = System.Drawing.SystemColors.Window;
            this.pnlLayoutTop.GradientStart = System.Drawing.SystemColors.Window;
            this.pnlLayoutTop.Location = new System.Drawing.Point(0, 0);
            this.pnlLayoutTop.Name = "pnlLayoutTop";
            this.pnlLayoutTop.Size = new System.Drawing.Size(687, 46);
            this.pnlLayoutTop.TabIndex = 73;
            // 
            // pnlCondition
            // 
            this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
            this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlCondition.Location = new System.Drawing.Point(584, 8);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(99, 26);
            this.pnlCondition.TabIndex = 66;
            // 
            // lblSubject
            // 
            this.lblSubject.BackColor = System.Drawing.Color.Transparent;
            this.lblSubject.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSubject.Location = new System.Drawing.Point(10, 10);
            this.lblSubject.Margin = new System.Windows.Forms.Padding(4);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(158, 24);
            this.lblSubject.TabIndex = 70;
            this.lblSubject.Text = "HTTP설정";
            this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
            this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.lblSubject.TextColor = System.Drawing.Color.Black;
            this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
            this.lblSubject.TextTopMargin = 2;
            // 
            // pnlControl
            // 
            this.pnlControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlControl.Controls.Add(this.btnCancel);
            this.pnlControl.Controls.Add(this.btnOk);
            this.pnlControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlControl.Location = new System.Drawing.Point(494, 547);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(181, 30);
            this.pnlControl.TabIndex = 289;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
            this.btnCancel.EnableFadeInOut = true;
            this.btnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.GlowColor = System.Drawing.Color.White;
            this.btnCancel.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnCancel.Location = new System.Drawing.Point(93, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "닫기";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.DarkGray;
            this.btnOk.EnableFadeInOut = true;
            this.btnOk.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.GlowColor = System.Drawing.Color.White;
            this.btnOk.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnOk.Location = new System.Drawing.Point(5, 3);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 24);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "확인(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.BackColor = System.Drawing.Color.Beige;
            this.groupBox11.Controls.Add(this.txtRESTfulLocalPort);
            this.groupBox11.Controls.Add(this.label31);
            this.groupBox11.Location = new System.Drawing.Point(10, 52);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(663, 82);
            this.groupBox11.TabIndex = 382;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "레스트풀 내부 PORT설정";
            // 
            // txtRESTfulLocalPort
            // 
            this.txtRESTfulLocalPort.Location = new System.Drawing.Point(87, 16);
            this.txtRESTfulLocalPort.Name = "txtRESTfulLocalPort";
            this.txtRESTfulLocalPort.Size = new System.Drawing.Size(107, 21);
            this.txtRESTfulLocalPort.TabIndex = 386;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Font = new System.Drawing.Font("굴림", 9F);
            this.label31.Location = new System.Drawing.Point(13, 19);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(78, 12);
            this.label31.TabIndex = 385;
            this.label31.Text = "PORT 설정 : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Beige;
            this.groupBox1.Controls.Add(this.txtRESTfulServerPort);
            this.groupBox1.Controls.Add(this.txtRESTfulServerIp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(10, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(663, 82);
            this.groupBox1.TabIndex = 386;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "레스트풀 SERVER정보";
            // 
            // txtRESTfulServerPort
            // 
            this.txtRESTfulServerPort.Location = new System.Drawing.Point(87, 55);
            this.txtRESTfulServerPort.Name = "txtRESTfulServerPort";
            this.txtRESTfulServerPort.Size = new System.Drawing.Size(107, 21);
            this.txtRESTfulServerPort.TabIndex = 388;
            // 
            // txtRESTfulServerIp
            // 
            this.txtRESTfulServerIp.Location = new System.Drawing.Point(87, 22);
            this.txtRESTfulServerIp.Name = "txtRESTfulServerIp";
            this.txtRESTfulServerIp.Size = new System.Drawing.Size(107, 21);
            this.txtRESTfulServerIp.TabIndex = 387;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("굴림", 9F);
            this.label2.Location = new System.Drawing.Point(13, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 12);
            this.label2.TabIndex = 386;
            this.label2.Text = "IP      설정 : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 9F);
            this.label1.Location = new System.Drawing.Point(13, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 12);
            this.label1.TabIndex = 385;
            this.label1.Text = "PORT 설정 : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Beige;
            this.groupBox2.Controls.Add(this.txtRESTfulVersion);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(10, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(663, 60);
            this.groupBox2.TabIndex = 389;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "레스트풀 VERSION";
            // 
            // txtRESTfulVersion
            // 
            this.txtRESTfulVersion.Location = new System.Drawing.Point(90, 23);
            this.txtRESTfulVersion.Name = "txtRESTfulVersion";
            this.txtRESTfulVersion.Size = new System.Drawing.Size(107, 21);
            this.txtRESTfulVersion.TabIndex = 387;
            this.txtRESTfulVersion.Text = "v2.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 9F);
            this.label3.Location = new System.Drawing.Point(13, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 12);
            this.label3.TabIndex = 386;
            this.label3.Text = "버젼  설정 : ";
            // 
            // AdminHTTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 589);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.pnlControl);
            this.Controls.Add(this.pnlLayoutTop);
            this.Name = "AdminHTTP";
            this.Tag = "RES";
            this.Text = "AdminHTTP";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdminSaleTIcket_FormClosed);
            this.Load += new System.EventHandler(this.AdminSaleTIcket_Load);
            this.pnlLayoutTop.ResumeLayout(false);
            this.pnlControl.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FadeFox.UI.GradientPanelEx pnlLayoutTop;
        private System.Windows.Forms.FlowLayoutPanel pnlCondition;
        private FadeFox.UI.Text3DLabel lblSubject;
        private System.Windows.Forms.FlowLayoutPanel pnlControl;
        private FadeFox.UI.GlassButton btnCancel;
        private FadeFox.UI.GlassButton btnOk;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtRESTfulLocalPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRESTfulServerPort;
        private System.Windows.Forms.TextBox txtRESTfulServerIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtRESTfulVersion;
        private System.Windows.Forms.Label label3;
    }
}