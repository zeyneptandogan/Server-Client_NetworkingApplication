namespace SweetProjectClientSide
{
    partial class Form1
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
            this.label_ip = new System.Windows.Forms.Label();
            this.label_port = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.label_sweet = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.textBox_sweet = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.button_post = new System.Windows.Forms.Button();
            this.button_request = new System.Windows.Forms.Button();
            this.richTextBox_feed = new System.Windows.Forms.RichTextBox();
            this.richTextBox_notification = new System.Windows.Forms.RichTextBox();
            this.label_feed = new System.Windows.Forms.Label();
            this.label_notification = new System.Windows.Forms.Label();
            this.label_follow = new System.Windows.Forms.Label();
            this.textBox_follow = new System.Windows.Forms.TextBox();
            this.button_followedFeed = new System.Windows.Forms.Button();
            this.button_follow = new System.Windows.Forms.Button();
            this.button_listAll = new System.Windows.Forms.Button();
            this.label_block = new System.Windows.Forms.Label();
            this.textBox_block = new System.Windows.Forms.TextBox();
            this.button_block = new System.Windows.Forms.Button();
            this.label_delete = new System.Windows.Forms.Label();
            this.textBox_deleteSweet = new System.Windows.Forms.TextBox();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_followers = new System.Windows.Forms.Button();
            this.button_followed = new System.Windows.Forms.Button();
            this.button_mysweet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Location = new System.Drawing.Point(26, 43);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(24, 17);
            this.label_ip.TabIndex = 0;
            this.label_ip.Text = "IP:";
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(26, 81);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(42, 17);
            this.label_port.TabIndex = 1;
            this.label_port.Text = "Port: ";
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(26, 117);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(49, 17);
            this.label_name.TabIndex = 2;
            this.label_name.Text = "Name:";
            // 
            // label_sweet
            // 
            this.label_sweet.AutoSize = true;
            this.label_sweet.Location = new System.Drawing.Point(12, 355);
            this.label_sweet.Name = "label_sweet";
            this.label_sweet.Size = new System.Drawing.Size(50, 17);
            this.label_sweet.TabIndex = 3;
            this.label_sweet.Text = "Sweet:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(93, 43);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(118, 22);
            this.textBox_ip.TabIndex = 4;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(93, 78);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(118, 22);
            this.textBox_port.TabIndex = 5;
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(93, 117);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(118, 22);
            this.textBox_name.TabIndex = 6;
            // 
            // textBox_sweet
            // 
            this.textBox_sweet.Enabled = false;
            this.textBox_sweet.Location = new System.Drawing.Point(74, 355);
            this.textBox_sweet.Name = "textBox_sweet";
            this.textBox_sweet.Size = new System.Drawing.Size(221, 22);
            this.textBox_sweet.TabIndex = 7;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(44, 160);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(98, 23);
            this.button_connect.TabIndex = 8;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(170, 160);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(98, 23);
            this.button_disconnect.TabIndex = 9;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // button_post
            // 
            this.button_post.Enabled = false;
            this.button_post.Location = new System.Drawing.Point(235, 383);
            this.button_post.Name = "button_post";
            this.button_post.Size = new System.Drawing.Size(75, 23);
            this.button_post.TabIndex = 10;
            this.button_post.Text = "Post";
            this.button_post.UseVisualStyleBackColor = true;
            this.button_post.Click += new System.EventHandler(this.button_post_Click);
            // 
            // button_request
            // 
            this.button_request.Enabled = false;
            this.button_request.Location = new System.Drawing.Point(720, 297);
            this.button_request.Name = "button_request";
            this.button_request.Size = new System.Drawing.Size(156, 25);
            this.button_request.TabIndex = 11;
            this.button_request.Text = "Feed of all users";
            this.button_request.UseVisualStyleBackColor = true;
            this.button_request.Click += new System.EventHandler(this.button_request_Click);
            // 
            // richTextBox_feed
            // 
            this.richTextBox_feed.Location = new System.Drawing.Point(317, 40);
            this.richTextBox_feed.Name = "richTextBox_feed";
            this.richTextBox_feed.ReadOnly = true;
            this.richTextBox_feed.Size = new System.Drawing.Size(299, 254);
            this.richTextBox_feed.TabIndex = 12;
            this.richTextBox_feed.Text = "";
            // 
            // richTextBox_notification
            // 
            this.richTextBox_notification.Location = new System.Drawing.Point(348, 317);
            this.richTextBox_notification.Name = "richTextBox_notification";
            this.richTextBox_notification.ReadOnly = true;
            this.richTextBox_notification.Size = new System.Drawing.Size(268, 147);
            this.richTextBox_notification.TabIndex = 13;
            this.richTextBox_notification.Text = "";
            // 
            // label_feed
            // 
            this.label_feed.AutoSize = true;
            this.label_feed.Location = new System.Drawing.Point(331, 20);
            this.label_feed.Name = "label_feed";
            this.label_feed.Size = new System.Drawing.Size(44, 17);
            this.label_feed.TabIndex = 14;
            this.label_feed.Text = "Feed:";
            // 
            // label_notification
            // 
            this.label_notification.AutoSize = true;
            this.label_notification.Location = new System.Drawing.Point(345, 297);
            this.label_notification.Name = "label_notification";
            this.label_notification.Size = new System.Drawing.Size(93, 17);
            this.label_notification.TabIndex = 15;
            this.label_notification.Text = "Notifications: ";
            // 
            // label_follow
            // 
            this.label_follow.AutoSize = true;
            this.label_follow.Location = new System.Drawing.Point(622, 40);
            this.label_follow.Name = "label_follow";
            this.label_follow.Size = new System.Drawing.Size(90, 17);
            this.label_follow.TabIndex = 17;
            this.label_follow.Text = "Follow users:";
            // 
            // textBox_follow
            // 
            this.textBox_follow.Enabled = false;
            this.textBox_follow.Location = new System.Drawing.Point(639, 60);
            this.textBox_follow.Name = "textBox_follow";
            this.textBox_follow.Size = new System.Drawing.Size(237, 22);
            this.textBox_follow.TabIndex = 18;
            // 
            // button_followedFeed
            // 
            this.button_followedFeed.Enabled = false;
            this.button_followedFeed.Location = new System.Drawing.Point(720, 329);
            this.button_followedFeed.Name = "button_followedFeed";
            this.button_followedFeed.Size = new System.Drawing.Size(156, 27);
            this.button_followedFeed.TabIndex = 19;
            this.button_followedFeed.Text = "Feed of following";
            this.button_followedFeed.UseVisualStyleBackColor = true;
            this.button_followedFeed.Click += new System.EventHandler(this.button_followedFeed_Click);
            // 
            // button_follow
            // 
            this.button_follow.Enabled = false;
            this.button_follow.Location = new System.Drawing.Point(639, 88);
            this.button_follow.Name = "button_follow";
            this.button_follow.Size = new System.Drawing.Size(89, 23);
            this.button_follow.TabIndex = 20;
            this.button_follow.Text = "Follow";
            this.button_follow.UseVisualStyleBackColor = true;
            this.button_follow.Click += new System.EventHandler(this.button_follow_Click);
            // 
            // button_listAll
            // 
            this.button_listAll.Enabled = false;
            this.button_listAll.Location = new System.Drawing.Point(720, 362);
            this.button_listAll.Name = "button_listAll";
            this.button_listAll.Size = new System.Drawing.Size(156, 28);
            this.button_listAll.TabIndex = 21;
            this.button_listAll.Text = "List of all users";
            this.button_listAll.UseVisualStyleBackColor = true;
            this.button_listAll.Click += new System.EventHandler(this.button_listAll_Click);
            // 
            // label_block
            // 
            this.label_block.AutoSize = true;
            this.label_block.Location = new System.Drawing.Point(622, 127);
            this.label_block.Name = "label_block";
            this.label_block.Size = new System.Drawing.Size(85, 17);
            this.label_block.TabIndex = 22;
            this.label_block.Text = "Block users:";
            // 
            // textBox_block
            // 
            this.textBox_block.Enabled = false;
            this.textBox_block.Location = new System.Drawing.Point(639, 148);
            this.textBox_block.Name = "textBox_block";
            this.textBox_block.Size = new System.Drawing.Size(237, 22);
            this.textBox_block.TabIndex = 23;
            // 
            // button_block
            // 
            this.button_block.Enabled = false;
            this.button_block.Location = new System.Drawing.Point(639, 176);
            this.button_block.Name = "button_block";
            this.button_block.Size = new System.Drawing.Size(89, 23);
            this.button_block.TabIndex = 24;
            this.button_block.Text = "Block";
            this.button_block.UseVisualStyleBackColor = true;
            this.button_block.Click += new System.EventHandler(this.button_block_Click);
            // 
            // label_delete
            // 
            this.label_delete.AutoSize = true;
            this.label_delete.Location = new System.Drawing.Point(622, 217);
            this.label_delete.Name = "label_delete";
            this.label_delete.Size = new System.Drawing.Size(93, 17);
            this.label_delete.TabIndex = 25;
            this.label_delete.Text = "Delete sweet:";
            // 
            // textBox_deleteSweet
            // 
            this.textBox_deleteSweet.Enabled = false;
            this.textBox_deleteSweet.Location = new System.Drawing.Point(639, 237);
            this.textBox_deleteSweet.Name = "textBox_deleteSweet";
            this.textBox_deleteSweet.Size = new System.Drawing.Size(237, 22);
            this.textBox_deleteSweet.TabIndex = 26;
            // 
            // button_delete
            // 
            this.button_delete.Enabled = false;
            this.button_delete.Location = new System.Drawing.Point(639, 264);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(89, 23);
            this.button_delete.TabIndex = 27;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_followers
            // 
            this.button_followers.Enabled = false;
            this.button_followers.Location = new System.Drawing.Point(720, 397);
            this.button_followers.Name = "button_followers";
            this.button_followers.Size = new System.Drawing.Size(156, 28);
            this.button_followers.TabIndex = 28;
            this.button_followers.Text = "List of followers";
            this.button_followers.UseVisualStyleBackColor = true;
            this.button_followers.Click += new System.EventHandler(this.button_followers_Click);
            // 
            // button_followed
            // 
            this.button_followed.Enabled = false;
            this.button_followed.Location = new System.Drawing.Point(720, 432);
            this.button_followed.Name = "button_followed";
            this.button_followed.Size = new System.Drawing.Size(156, 28);
            this.button_followed.TabIndex = 29;
            this.button_followed.Text = "List of following";
            this.button_followed.UseVisualStyleBackColor = true;
            this.button_followed.Click += new System.EventHandler(this.button_followed_Click);
            // 
            // button_mysweet
            // 
            this.button_mysweet.Enabled = false;
            this.button_mysweet.Location = new System.Drawing.Point(720, 467);
            this.button_mysweet.Name = "button_mysweet";
            this.button_mysweet.Size = new System.Drawing.Size(156, 26);
            this.button_mysweet.TabIndex = 30;
            this.button_mysweet.Text = "My sweets";
            this.button_mysweet.UseVisualStyleBackColor = true;
            this.button_mysweet.Click += new System.EventHandler(this.button_mysweet_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 514);
            this.Controls.Add(this.button_mysweet);
            this.Controls.Add(this.button_followed);
            this.Controls.Add(this.button_followers);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.textBox_deleteSweet);
            this.Controls.Add(this.label_delete);
            this.Controls.Add(this.button_block);
            this.Controls.Add(this.textBox_block);
            this.Controls.Add(this.label_block);
            this.Controls.Add(this.button_listAll);
            this.Controls.Add(this.button_follow);
            this.Controls.Add(this.button_followedFeed);
            this.Controls.Add(this.textBox_follow);
            this.Controls.Add(this.label_follow);
            this.Controls.Add(this.label_notification);
            this.Controls.Add(this.label_feed);
            this.Controls.Add(this.richTextBox_notification);
            this.Controls.Add(this.richTextBox_feed);
            this.Controls.Add(this.button_request);
            this.Controls.Add(this.button_post);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_sweet);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label_sweet);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.label_ip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_sweet;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.TextBox textBox_sweet;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Button button_post;
        private System.Windows.Forms.Button button_request;
        private System.Windows.Forms.RichTextBox richTextBox_feed;
        private System.Windows.Forms.RichTextBox richTextBox_notification;
        private System.Windows.Forms.Label label_feed;
        private System.Windows.Forms.Label label_notification;
        private System.Windows.Forms.Label label_follow;
        private System.Windows.Forms.TextBox textBox_follow;
        private System.Windows.Forms.Button button_followedFeed;
        private System.Windows.Forms.Button button_follow;
        private System.Windows.Forms.Button button_listAll;
        private System.Windows.Forms.Label label_block;
        private System.Windows.Forms.TextBox textBox_block;
        private System.Windows.Forms.Button button_block;
        private System.Windows.Forms.Label label_delete;
        private System.Windows.Forms.TextBox textBox_deleteSweet;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_followers;
        private System.Windows.Forms.Button button_followed;
        private System.Windows.Forms.Button button_mysweet;
    }
}

