namespace SpaceMate
{
    partial class SpaceMateClient
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbServerAddress = new System.Windows.Forms.TextBox();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.tbSessionID = new System.Windows.Forms.TextBox();
            this.cbHost = new System.Windows.Forms.CheckBox();
            this.gbServerInfo = new System.Windows.Forms.GroupBox();
            this.gbCreateRoom = new System.Windows.Forms.GroupBox();
            this.gbJoinRoom = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lvRooms = new System.Windows.Forms.ListView();
            this.gbCreateRoom.SuspendLayout();
            this.gbJoinRoom.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(652, 87);
            this.button1.TabIndex = 0;
            this.button1.Text = "Create Room";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.create_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "É Host?:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Room name:";
            // 
            // tbServerAddress
            // 
            this.tbServerAddress.Location = new System.Drawing.Point(175, 47);
            this.tbServerAddress.Name = "tbServerAddress";
            this.tbServerAddress.Size = new System.Drawing.Size(280, 31);
            this.tbServerAddress.TabIndex = 5;
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(175, 105);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(146, 31);
            this.tbServerPort.TabIndex = 6;
            // 
            // tbSessionID
            // 
            this.tbSessionID.Location = new System.Drawing.Point(159, 95);
            this.tbSessionID.Name = "tbSessionID";
            this.tbSessionID.Size = new System.Drawing.Size(146, 31);
            this.tbSessionID.TabIndex = 8;
            // 
            // cbHost
            // 
            this.cbHost.AutoSize = true;
            this.cbHost.Location = new System.Drawing.Point(159, 36);
            this.cbHost.Name = "cbHost";
            this.cbHost.Size = new System.Drawing.Size(88, 29);
            this.cbHost.TabIndex = 9;
            this.cbHost.Text = "Host";
            this.cbHost.UseVisualStyleBackColor = true;
            // 
            // gbServerInfo
            // 
            this.gbServerInfo.Location = new System.Drawing.Point(12, 13);
            this.gbServerInfo.Name = "gbServerInfo";
            this.gbServerInfo.Size = new System.Drawing.Size(680, 152);
            this.gbServerInfo.TabIndex = 13;
            this.gbServerInfo.TabStop = false;
            this.gbServerInfo.Text = "Server Info";
            // 
            // gbCreateRoom
            // 
            this.gbCreateRoom.Controls.Add(this.button1);
            this.gbCreateRoom.Controls.Add(this.label3);
            this.gbCreateRoom.Controls.Add(this.label4);
            this.gbCreateRoom.Controls.Add(this.cbHost);
            this.gbCreateRoom.Controls.Add(this.tbSessionID);
            this.gbCreateRoom.Location = new System.Drawing.Point(12, 187);
            this.gbCreateRoom.Name = "gbCreateRoom";
            this.gbCreateRoom.Size = new System.Drawing.Size(680, 253);
            this.gbCreateRoom.TabIndex = 14;
            this.gbCreateRoom.TabStop = false;
            this.gbCreateRoom.Text = "Create Room";
            // 
            // gbJoinRoom
            // 
            this.gbJoinRoom.Controls.Add(this.button3);
            this.gbJoinRoom.Controls.Add(this.button2);
            this.gbJoinRoom.Controls.Add(this.lvRooms);
            this.gbJoinRoom.Location = new System.Drawing.Point(12, 447);
            this.gbJoinRoom.Name = "gbJoinRoom";
            this.gbJoinRoom.Size = new System.Drawing.Size(680, 385);
            this.gbJoinRoom.TabIndex = 15;
            this.gbJoinRoom.TabStop = false;
            this.gbJoinRoom.Text = "Join Room";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 279);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(131, 77);
            this.button3.TabIndex = 15;
            this.button3.Text = "Refresh";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.refresh_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 279);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(499, 77);
            this.button2.TabIndex = 14;
            this.button2.Text = "Join Room";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.join_Click);
            // 
            // lvRooms
            // 
            this.lvRooms.Location = new System.Drawing.Point(14, 29);
            this.lvRooms.Name = "lvRooms";
            this.lvRooms.Size = new System.Drawing.Size(652, 226);
            this.lvRooms.TabIndex = 13;
            this.lvRooms.UseCompatibleStateImageBehavior = false;
            // 
            // SpaceMateClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 844);
            this.Controls.Add(this.tbServerPort);
            this.Controls.Add(this.tbServerAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbServerInfo);
            this.Controls.Add(this.gbCreateRoom);
            this.Controls.Add(this.gbJoinRoom);
            this.MaximizeBox = false;
            this.Name = "SpaceMateClient";
            this.Text = "SpaceMate";
            this.gbCreateRoom.ResumeLayout(false);
            this.gbCreateRoom.PerformLayout();
            this.gbJoinRoom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbServerAddress;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.TextBox tbSessionID;
        private System.Windows.Forms.CheckBox cbHost;
        private System.Windows.Forms.GroupBox gbServerInfo;
        private System.Windows.Forms.GroupBox gbCreateRoom;
        private System.Windows.Forms.GroupBox gbJoinRoom;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lvRooms;
    }
}