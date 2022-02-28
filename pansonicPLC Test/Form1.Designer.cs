namespace pansonicPLC_Test
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.device_plc_status = new System.Windows.Forms.Label();
            this.sendStr = new System.Windows.Forms.TextBox();
            this.receiveStr = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // device_plc_status
            // 
            this.device_plc_status.AutoSize = true;
            this.device_plc_status.Location = new System.Drawing.Point(12, 30);
            this.device_plc_status.Name = "device_plc_status";
            this.device_plc_status.Size = new System.Drawing.Size(101, 12);
            this.device_plc_status.TabIndex = 0;
            this.device_plc_status.Text = "提示串口是否打开";
            // 
            // sendStr
            // 
            this.sendStr.Location = new System.Drawing.Point(182, 33);
            this.sendStr.Multiline = true;
            this.sendStr.Name = "sendStr";
            this.sendStr.Size = new System.Drawing.Size(258, 374);
            this.sendStr.TabIndex = 1;
            // 
            // receiveStr
            // 
            this.receiveStr.Location = new System.Drawing.Point(511, 30);
            this.receiveStr.Multiline = true;
            this.receiveStr.Name = "receiveStr";
            this.receiveStr.Size = new System.Drawing.Size(250, 374);
            this.receiveStr.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "置位R300";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(23, 176);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 34);
            this.button2.TabIndex = 4;
            this.button2.Text = "读取Y30F";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(23, 114);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 34);
            this.button3.TabIndex = 5;
            this.button3.Text = "复位R300";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(23, 235);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 34);
            this.button4.TabIndex = 6;
            this.button4.Text = "读取Y30字";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 476);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.receiveStr);
            this.Controls.Add(this.sendStr);
            this.Controls.Add(this.device_plc_status);
            this.Name = "Form1";
            this.Text = "松下PLC通讯测试";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label device_plc_status;
        private System.Windows.Forms.TextBox sendStr;
        private System.Windows.Forms.TextBox receiveStr;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

