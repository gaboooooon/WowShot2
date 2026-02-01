namespace WowShot2
{
	partial class FormAbout
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
			pictureBox1 = new PictureBox();
			labelProduct = new Label();
			labelVersion = new Label();
			labelCopyright = new Label();
			linkLabelGitHub = new LinkLabel();
			buttonOK = new Button();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.Image = Resource.IconImage;
			pictureBox1.Location = new Point(12, 12);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(150, 140);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			// 
			// labelProduct
			// 
			labelProduct.AutoSize = true;
			labelProduct.Font = new Font("Yu Gothic UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 128);
			labelProduct.Location = new Point(168, 12);
			labelProduct.Name = "labelProduct";
			labelProduct.Size = new Size(220, 54);
			labelProduct.TabIndex = 1;
			labelProduct.Text = "WowShot2";
			// 
			// labelVersion
			// 
			labelVersion.AutoSize = true;
			labelVersion.Font = new Font("Yu Gothic UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 128);
			labelVersion.Location = new Point(382, 12);
			labelVersion.Name = "labelVersion";
			labelVersion.Size = new Size(269, 54);
			labelVersion.TabIndex = 2;
			labelVersion.Text = "Version1.0.3.0";
			// 
			// labelCopyright
			// 
			labelCopyright.AutoSize = true;
			labelCopyright.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 128);
			labelCopyright.Location = new Point(248, 77);
			labelCopyright.Name = "labelCopyright";
			labelCopyright.Size = new Size(353, 32);
			labelCopyright.TabIndex = 3;
			labelCopyright.Text = "Copyright(c) 2026 gaboooooon";
			// 
			// linkLabelGitHub
			// 
			linkLabelGitHub.AutoSize = true;
			linkLabelGitHub.Location = new Point(239, 123);
			linkLabelGitHub.Name = "linkLabelGitHub";
			linkLabelGitHub.Size = new Size(370, 25);
			linkLabelGitHub.TabIndex = 4;
			linkLabelGitHub.TabStop = true;
			linkLabelGitHub.Text = "https://github.com/gaboooooon/WowShot2";
			linkLabelGitHub.VisitedLinkColor = Color.Blue;
			linkLabelGitHub.LinkClicked += linkLabelGitHub_LinkClicked;
			// 
			// buttonOK
			// 
			buttonOK.Location = new Point(339, 170);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(141, 34);
			buttonOK.TabIndex = 0;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += buttonOK_Click;
			// 
			// FormAbout
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(706, 216);
			Controls.Add(buttonOK);
			Controls.Add(linkLabelGitHub);
			Controls.Add(labelCopyright);
			Controls.Add(labelVersion);
			Controls.Add(labelProduct);
			Controls.Add(pictureBox1);
			Name = "FormAbout";
			Text = "バージョン情報";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PictureBox pictureBox1;
		private Label labelProduct;
		private Label labelVersion;
		private Label labelCopyright;
		private LinkLabel linkLabelGitHub;
		private Button buttonOK;
	}
}