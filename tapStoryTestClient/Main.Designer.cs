namespace tapStoryTestClient
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.UrlText = new System.Windows.Forms.TextBox();
            this.GroupNameText = new System.Windows.Forms.TextBox();
            this.File1Text = new System.Windows.Forms.TextBox();
            this.File2Text = new System.Windows.Forms.TextBox();
            this.File3Text = new System.Windows.Forms.TextBox();
            this.SendDataButton = new System.Windows.Forms.Button();
            this.TokenText = new System.Windows.Forms.TextBox();
            this.Browse1Button = new System.Windows.Forms.Button();
            this.Browse2Button = new System.Windows.Forms.Button();
            this.Browse3Button = new System.Windows.Forms.Button();
            this.RequestText = new System.Windows.Forms.TextBox();
            this.methodPost = new System.Windows.Forms.RadioButton();
            this.methodPut = new System.Windows.Forms.RadioButton();
            this.sendMethod = new System.Windows.Forms.GroupBox();
            this.methodGet = new System.Windows.Forms.RadioButton();
            this.methodDelete = new System.Windows.Forms.RadioButton();
            this.methodPatch = new System.Windows.Forms.RadioButton();
            this.UserIdText = new System.Windows.Forms.TextBox();
            this.FileGroupServerIdText = new System.Windows.Forms.TextBox();
            this.ResponseText = new System.Windows.Forms.TextBox();
            this.sendMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // UrlText
            // 
            this.UrlText.Location = new System.Drawing.Point(12, 12);
            this.UrlText.Name = "UrlText";
            this.UrlText.Size = new System.Drawing.Size(411, 22);
            this.UrlText.TabIndex = 0;
            this.UrlText.Text = "http://localhost:51915/api/UserStory";
            // 
            // GroupNameText
            // 
            this.GroupNameText.Location = new System.Drawing.Point(12, 50);
            this.GroupNameText.Multiline = true;
            this.GroupNameText.Name = "GroupNameText";
            this.GroupNameText.Size = new System.Drawing.Size(817, 27);
            this.GroupNameText.TabIndex = 1;
            this.GroupNameText.Text = "Test Group";
            // 
            // File1Text
            // 
            this.File1Text.Location = new System.Drawing.Point(13, 206);
            this.File1Text.Name = "File1Text";
            this.File1Text.Size = new System.Drawing.Size(688, 22);
            this.File1Text.TabIndex = 2;
            this.File1Text.Text = "C:\\Dev\\tapStory\\Text1.txt";
            // 
            // File2Text
            // 
            this.File2Text.Location = new System.Drawing.Point(13, 235);
            this.File2Text.Name = "File2Text";
            this.File2Text.Size = new System.Drawing.Size(688, 22);
            this.File2Text.TabIndex = 3;
            this.File2Text.Text = "C:\\Dev\\tapStory\\Text2.txt";
            // 
            // File3Text
            // 
            this.File3Text.Location = new System.Drawing.Point(13, 264);
            this.File3Text.Name = "File3Text";
            this.File3Text.Size = new System.Drawing.Size(688, 22);
            this.File3Text.TabIndex = 4;
            this.File3Text.Text = "C:\\Dev\\tapStory\\Text3.txt";
            // 
            // SendDataButton
            // 
            this.SendDataButton.Location = new System.Drawing.Point(640, 355);
            this.SendDataButton.Name = "SendDataButton";
            this.SendDataButton.Size = new System.Drawing.Size(189, 23);
            this.SendDataButton.TabIndex = 5;
            this.SendDataButton.Text = "Send Data";
            this.SendDataButton.UseVisualStyleBackColor = true;
            this.SendDataButton.Click += new System.EventHandler(this.SendDataButton_Click);
            // 
            // TokenText
            // 
            this.TokenText.Location = new System.Drawing.Point(13, 311);
            this.TokenText.Multiline = true;
            this.TokenText.Name = "TokenText";
            this.TokenText.Size = new System.Drawing.Size(621, 140);
            this.TokenText.TabIndex = 6;
            this.TokenText.Text = resources.GetString("TokenText.Text");
            this.TokenText.TextChanged += new System.EventHandler(this.TokenText_TextChanged);
            // 
            // Browse1Button
            // 
            this.Browse1Button.Location = new System.Drawing.Point(708, 206);
            this.Browse1Button.Name = "Browse1Button";
            this.Browse1Button.Size = new System.Drawing.Size(121, 23);
            this.Browse1Button.TabIndex = 7;
            this.Browse1Button.Text = "Browse";
            this.Browse1Button.UseVisualStyleBackColor = true;
            this.Browse1Button.Click += new System.EventHandler(this.Browse1Button_Click);
            // 
            // Browse2Button
            // 
            this.Browse2Button.Location = new System.Drawing.Point(708, 235);
            this.Browse2Button.Name = "Browse2Button";
            this.Browse2Button.Size = new System.Drawing.Size(121, 23);
            this.Browse2Button.TabIndex = 8;
            this.Browse2Button.Text = "Browse";
            this.Browse2Button.UseVisualStyleBackColor = true;
            this.Browse2Button.Click += new System.EventHandler(this.Browse2Button_Click);
            // 
            // Browse3Button
            // 
            this.Browse3Button.Location = new System.Drawing.Point(708, 263);
            this.Browse3Button.Name = "Browse3Button";
            this.Browse3Button.Size = new System.Drawing.Size(121, 23);
            this.Browse3Button.TabIndex = 9;
            this.Browse3Button.Text = "Browse";
            this.Browse3Button.UseVisualStyleBackColor = true;
            this.Browse3Button.Click += new System.EventHandler(this.Browse3Button_Click);
            // 
            // RequestText
            // 
            this.RequestText.Location = new System.Drawing.Point(13, 469);
            this.RequestText.Multiline = true;
            this.RequestText.Name = "RequestText";
            this.RequestText.ReadOnly = true;
            this.RequestText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RequestText.Size = new System.Drawing.Size(816, 72);
            this.RequestText.TabIndex = 10;
            // 
            // methodPost
            // 
            this.methodPost.AutoSize = true;
            this.methodPost.Checked = true;
            this.methodPost.Location = new System.Drawing.Point(20, 22);
            this.methodPost.Name = "methodPost";
            this.methodPost.Size = new System.Drawing.Size(67, 21);
            this.methodPost.TabIndex = 11;
            this.methodPost.TabStop = true;
            this.methodPost.Text = "POST";
            this.methodPost.UseVisualStyleBackColor = true;
            // 
            // methodPut
            // 
            this.methodPut.AutoSize = true;
            this.methodPut.Location = new System.Drawing.Point(109, 22);
            this.methodPut.Name = "methodPut";
            this.methodPut.Size = new System.Drawing.Size(57, 21);
            this.methodPut.TabIndex = 12;
            this.methodPut.Text = "PUT";
            this.methodPut.UseVisualStyleBackColor = true;
            // 
            // sendMethod
            // 
            this.sendMethod.Controls.Add(this.methodGet);
            this.sendMethod.Controls.Add(this.methodDelete);
            this.sendMethod.Controls.Add(this.methodPatch);
            this.sendMethod.Controls.Add(this.methodPut);
            this.sendMethod.Controls.Add(this.methodPost);
            this.sendMethod.Location = new System.Drawing.Point(13, 126);
            this.sendMethod.Name = "sendMethod";
            this.sendMethod.Size = new System.Drawing.Size(816, 57);
            this.sendMethod.TabIndex = 13;
            this.sendMethod.TabStop = false;
            this.sendMethod.Text = "Send Method";
            // 
            // methodGet
            // 
            this.methodGet.AutoSize = true;
            this.methodGet.Location = new System.Drawing.Point(368, 21);
            this.methodGet.Name = "methodGet";
            this.methodGet.Size = new System.Drawing.Size(58, 21);
            this.methodGet.TabIndex = 15;
            this.methodGet.Text = "GET";
            this.methodGet.UseVisualStyleBackColor = true;
            // 
            // methodDelete
            // 
            this.methodDelete.AutoSize = true;
            this.methodDelete.Location = new System.Drawing.Point(279, 21);
            this.methodDelete.Name = "methodDelete";
            this.methodDelete.Size = new System.Drawing.Size(83, 21);
            this.methodDelete.TabIndex = 14;
            this.methodDelete.Text = "DELETE";
            this.methodDelete.UseVisualStyleBackColor = true;
            // 
            // methodPatch
            // 
            this.methodPatch.AutoSize = true;
            this.methodPatch.Location = new System.Drawing.Point(188, 21);
            this.methodPatch.Name = "methodPatch";
            this.methodPatch.Size = new System.Drawing.Size(75, 21);
            this.methodPatch.TabIndex = 13;
            this.methodPatch.Text = "PATCH";
            this.methodPatch.UseVisualStyleBackColor = true;
            // 
            // UserIdText
            // 
            this.UserIdText.Location = new System.Drawing.Point(13, 84);
            this.UserIdText.Name = "UserIdText";
            this.UserIdText.Size = new System.Drawing.Size(100, 22);
            this.UserIdText.TabIndex = 14;
            this.UserIdText.Text = "2";
            // 
            // FileGroupServerIdText
            // 
            this.FileGroupServerIdText.Location = new System.Drawing.Point(122, 83);
            this.FileGroupServerIdText.Name = "FileGroupServerIdText";
            this.FileGroupServerIdText.Size = new System.Drawing.Size(327, 22);
            this.FileGroupServerIdText.TabIndex = 15;
            // 
            // ResponseText
            // 
            this.ResponseText.Location = new System.Drawing.Point(13, 547);
            this.ResponseText.Multiline = true;
            this.ResponseText.Name = "ResponseText";
            this.ResponseText.ReadOnly = true;
            this.ResponseText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResponseText.Size = new System.Drawing.Size(816, 72);
            this.ResponseText.TabIndex = 16;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 620);
            this.Controls.Add(this.ResponseText);
            this.Controls.Add(this.FileGroupServerIdText);
            this.Controls.Add(this.UserIdText);
            this.Controls.Add(this.sendMethod);
            this.Controls.Add(this.RequestText);
            this.Controls.Add(this.Browse3Button);
            this.Controls.Add(this.Browse2Button);
            this.Controls.Add(this.Browse1Button);
            this.Controls.Add(this.TokenText);
            this.Controls.Add(this.SendDataButton);
            this.Controls.Add(this.File3Text);
            this.Controls.Add(this.File2Text);
            this.Controls.Add(this.File1Text);
            this.Controls.Add(this.GroupNameText);
            this.Controls.Add(this.UrlText);
            this.Name = "Main";
            this.Text = "tapStory Test Client";
            this.sendMethod.ResumeLayout(false);
            this.sendMethod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UrlText;
        private System.Windows.Forms.TextBox GroupNameText;
        private System.Windows.Forms.TextBox File1Text;
        private System.Windows.Forms.TextBox File2Text;
        private System.Windows.Forms.TextBox File3Text;
        private System.Windows.Forms.Button SendDataButton;
        private System.Windows.Forms.TextBox TokenText;
        private System.Windows.Forms.Button Browse1Button;
        private System.Windows.Forms.Button Browse2Button;
        private System.Windows.Forms.Button Browse3Button;
        private System.Windows.Forms.TextBox RequestText;
        private System.Windows.Forms.RadioButton methodPost;
        private System.Windows.Forms.RadioButton methodPut;
        private System.Windows.Forms.GroupBox sendMethod;
        private System.Windows.Forms.RadioButton methodGet;
        private System.Windows.Forms.RadioButton methodDelete;
        private System.Windows.Forms.RadioButton methodPatch;
        private System.Windows.Forms.TextBox UserIdText;
        private System.Windows.Forms.TextBox FileGroupServerIdText;
        private System.Windows.Forms.TextBox ResponseText;
    }
}

