namespace WowShot2
{
	partial class FormSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
			listBoxProfile = new ListBox();
			buttonAdd = new Button();
			buttonDelete = new Button();
			groupBoxProfile = new GroupBox();
			buttonKeyCapture = new Button();
			comboBoxTarget = new ComboBox();
			label7 = new Label();
			label6 = new Label();
			textBoxDelay = new TextBox();
			checkBoxUseDelay = new CheckBox();
			buttonApply = new Button();
			textBox1 = new TextBox();
			comboBoxFormat = new ComboBox();
			label5 = new Label();
			textBoxFileName = new TextBox();
			label4 = new Label();
			buttonBrowse = new Button();
			label3 = new Label();
			textBoxSaveDir = new TextBox();
			checkBoxClipboard = new CheckBox();
			checkBoxSaveFile = new CheckBox();
			checkBoxAlt = new CheckBox();
			checkBoxShift = new CheckBox();
			checkBoxCtrl = new CheckBox();
			label2 = new Label();
			textBoxKey = new TextBox();
			textBoxProfileName = new TextBox();
			label1 = new Label();
			buttonClearCounter = new Button();
			checkBoxRememberNumber = new CheckBox();
			buttonCancel = new Button();
			buttonOk = new Button();
			buttonDuplicate = new Button();
			buttonMoveUp = new Button();
			buttonMoveDown = new Button();
			checkBoxShowNotification = new CheckBox();
			groupBoxProfile.SuspendLayout();
			SuspendLayout();
			// 
			// listBoxProfile
			// 
			listBoxProfile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			listBoxProfile.FormattingEnabled = true;
			listBoxProfile.ItemHeight = 25;
			listBoxProfile.Location = new Point(12, 12);
			listBoxProfile.Name = "listBoxProfile";
			listBoxProfile.Size = new Size(764, 179);
			listBoxProfile.TabIndex = 0;
			listBoxProfile.SelectedIndexChanged += listBoxProfile_SelectedIndexChanged;
			// 
			// buttonAdd
			// 
			buttonAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonAdd.Location = new Point(782, 12);
			buttonAdd.Name = "buttonAdd";
			buttonAdd.Size = new Size(112, 34);
			buttonAdd.TabIndex = 1;
			buttonAdd.Text = "追加";
			buttonAdd.UseVisualStyleBackColor = true;
			buttonAdd.Click += buttonAdd_Click;
			// 
			// buttonDelete
			// 
			buttonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonDelete.Location = new Point(782, 52);
			buttonDelete.Name = "buttonDelete";
			buttonDelete.Size = new Size(112, 34);
			buttonDelete.TabIndex = 2;
			buttonDelete.Text = "削除";
			buttonDelete.UseVisualStyleBackColor = true;
			buttonDelete.Click += buttonDelete_Click;
			// 
			// groupBoxProfile
			// 
			groupBoxProfile.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			groupBoxProfile.Controls.Add(buttonKeyCapture);
			groupBoxProfile.Controls.Add(comboBoxTarget);
			groupBoxProfile.Controls.Add(label7);
			groupBoxProfile.Controls.Add(label6);
			groupBoxProfile.Controls.Add(textBoxDelay);
			groupBoxProfile.Controls.Add(checkBoxUseDelay);
			groupBoxProfile.Controls.Add(buttonApply);
			groupBoxProfile.Controls.Add(textBox1);
			groupBoxProfile.Controls.Add(comboBoxFormat);
			groupBoxProfile.Controls.Add(label5);
			groupBoxProfile.Controls.Add(textBoxFileName);
			groupBoxProfile.Controls.Add(label4);
			groupBoxProfile.Controls.Add(buttonBrowse);
			groupBoxProfile.Controls.Add(label3);
			groupBoxProfile.Controls.Add(textBoxSaveDir);
			groupBoxProfile.Controls.Add(checkBoxClipboard);
			groupBoxProfile.Controls.Add(checkBoxSaveFile);
			groupBoxProfile.Controls.Add(checkBoxAlt);
			groupBoxProfile.Controls.Add(checkBoxShift);
			groupBoxProfile.Controls.Add(checkBoxCtrl);
			groupBoxProfile.Controls.Add(label2);
			groupBoxProfile.Controls.Add(textBoxKey);
			groupBoxProfile.Controls.Add(textBoxProfileName);
			groupBoxProfile.Controls.Add(label1);
			groupBoxProfile.Location = new Point(12, 197);
			groupBoxProfile.Name = "groupBoxProfile";
			groupBoxProfile.Size = new Size(882, 727);
			groupBoxProfile.TabIndex = 6;
			groupBoxProfile.TabStop = false;
			groupBoxProfile.Text = "キャプチャ設定";
			// 
			// buttonKeyCapture
			// 
			buttonKeyCapture.Location = new Point(395, 91);
			buttonKeyCapture.Name = "buttonKeyCapture";
			buttonKeyCapture.Size = new Size(125, 34);
			buttonKeyCapture.TabIndex = 4;
			buttonKeyCapture.Text = "キーキャプチャ";
			buttonKeyCapture.UseVisualStyleBackColor = true;
			buttonKeyCapture.Click += buttonKeyCapture_Click;
			// 
			// comboBoxTarget
			// 
			comboBoxTarget.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxTarget.FormattingEnabled = true;
			comboBoxTarget.Items.AddRange(new object[] { "選択範囲", "アクティブウィンドウ", "全ディスプレイ", "ディスプレイ1", "ディスプレイ2", "ディスプレイ3", "ディスプレイ4", "ディスプレイ5", "ディスプレイ6", "ディスプレイ7", "ディスプレイ8", "ディスプレイ9" });
			comboBoxTarget.Location = new Point(158, 185);
			comboBoxTarget.Name = "comboBoxTarget";
			comboBoxTarget.Size = new Size(281, 33);
			comboBoxTarget.TabIndex = 12;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(19, 188);
			label7.Name = "label7";
			label7.Size = new Size(123, 25);
			label7.TabIndex = 11;
			label7.Text = "キャプチャ対象 :";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(399, 140);
			label6.Name = "label6";
			label6.Size = new Size(121, 25);
			label6.TabIndex = 9;
			label6.Text = "遅延時間(秒) :";
			// 
			// textBoxDelay
			// 
			textBoxDelay.Location = new Point(528, 137);
			textBoxDelay.Name = "textBoxDelay";
			textBoxDelay.Size = new Size(93, 31);
			textBoxDelay.TabIndex = 10;
			// 
			// checkBoxUseDelay
			// 
			checkBoxUseDelay.AutoSize = true;
			checkBoxUseDelay.Location = new Point(158, 139);
			checkBoxUseDelay.Name = "checkBoxUseDelay";
			checkBoxUseDelay.Size = new Size(211, 29);
			checkBoxUseDelay.TabIndex = 8;
			checkBoxUseDelay.Text = "遅延キャプチャ ON/OFF";
			checkBoxUseDelay.UseVisualStyleBackColor = true;
			// 
			// buttonApply
			// 
			buttonApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonApply.Location = new Point(764, 680);
			buttonApply.Name = "buttonApply";
			buttonApply.Size = new Size(112, 34);
			buttonApply.TabIndex = 23;
			buttonApply.Text = "適用";
			buttonApply.UseVisualStyleBackColor = true;
			buttonApply.Click += buttonApply_Click;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(47, 439);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.ReadOnly = true;
			textBox1.Size = new Size(533, 212);
			textBox1.TabIndex = 21;
			textBox1.TabStop = false;
			textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// comboBoxFormat
			// 
			comboBoxFormat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxFormat.FormattingEnabled = true;
			comboBoxFormat.Items.AddRange(new object[] { "jpg", "png", "bmp" });
			comboBoxFormat.Location = new Point(700, 384);
			comboBoxFormat.Name = "comboBoxFormat";
			comboBoxFormat.Size = new Size(163, 33);
			comboBoxFormat.TabIndex = 20;
			// 
			// label5
			// 
			label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label5.AutoSize = true;
			label5.Location = new Point(581, 387);
			label5.Name = "label5";
			label5.Size = new Size(108, 25);
			label5.TabIndex = 19;
			label5.Text = "ファイル型式 :";
			// 
			// textBoxFileName
			// 
			textBoxFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxFileName.Location = new Point(221, 384);
			textBoxFileName.Name = "textBoxFileName";
			textBoxFileName.Size = new Size(354, 31);
			textBoxFileName.TabIndex = 18;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(47, 387);
			label4.Name = "label4";
			label4.Size = new Size(168, 25);
			label4.TabIndex = 17;
			label4.Text = "ファイル名テンプレート :";
			// 
			// buttonBrowse
			// 
			buttonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonBrowse.Location = new Point(770, 320);
			buttonBrowse.Name = "buttonBrowse";
			buttonBrowse.Size = new Size(93, 34);
			buttonBrowse.TabIndex = 16;
			buttonBrowse.Text = "参照...";
			buttonBrowse.UseVisualStyleBackColor = true;
			buttonBrowse.Click += buttonBrowse_Click;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(47, 325);
			label3.Name = "label3";
			label3.Size = new Size(152, 25);
			label3.TabIndex = 14;
			label3.Text = "保存先ディレクトリ :";
			// 
			// textBoxSaveDir
			// 
			textBoxSaveDir.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxSaveDir.Location = new Point(221, 322);
			textBoxSaveDir.Name = "textBoxSaveDir";
			textBoxSaveDir.Size = new Size(543, 31);
			textBoxSaveDir.TabIndex = 15;
			// 
			// checkBoxClipboard
			// 
			checkBoxClipboard.AutoSize = true;
			checkBoxClipboard.Location = new Point(19, 679);
			checkBoxClipboard.Name = "checkBoxClipboard";
			checkBoxClipboard.Size = new Size(270, 29);
			checkBoxClipboard.TabIndex = 22;
			checkBoxClipboard.Text = "クリップボードへのコピー ON/OFF";
			checkBoxClipboard.UseVisualStyleBackColor = true;
			// 
			// checkBoxSaveFile
			// 
			checkBoxSaveFile.AutoSize = true;
			checkBoxSaveFile.Location = new Point(19, 271);
			checkBoxSaveFile.Name = "checkBoxSaveFile";
			checkBoxSaveFile.Size = new Size(226, 29);
			checkBoxSaveFile.TabIndex = 13;
			checkBoxSaveFile.Text = "ファイルへの保存 ON/OFF";
			checkBoxSaveFile.UseVisualStyleBackColor = true;
			// 
			// checkBoxAlt
			// 
			checkBoxAlt.AutoSize = true;
			checkBoxAlt.Location = new Point(746, 95);
			checkBoxAlt.Name = "checkBoxAlt";
			checkBoxAlt.Size = new Size(60, 29);
			checkBoxAlt.TabIndex = 7;
			checkBoxAlt.Text = "Alt";
			checkBoxAlt.UseVisualStyleBackColor = true;
			// 
			// checkBoxShift
			// 
			checkBoxShift.AutoSize = true;
			checkBoxShift.Location = new Point(666, 95);
			checkBoxShift.Name = "checkBoxShift";
			checkBoxShift.Size = new Size(74, 29);
			checkBoxShift.TabIndex = 6;
			checkBoxShift.Text = "Shift";
			checkBoxShift.UseVisualStyleBackColor = true;
			// 
			// checkBoxCtrl
			// 
			checkBoxCtrl.AutoSize = true;
			checkBoxCtrl.Location = new Point(595, 94);
			checkBoxCtrl.Name = "checkBoxCtrl";
			checkBoxCtrl.Size = new Size(65, 29);
			checkBoxCtrl.TabIndex = 5;
			checkBoxCtrl.Text = "Ctrl";
			checkBoxCtrl.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(19, 46);
			label2.Name = "label2";
			label2.Size = new Size(117, 25);
			label2.TabIndex = 0;
			label2.Text = "プロファイル名 :";
			// 
			// textBoxKey
			// 
			textBoxKey.Location = new Point(158, 93);
			textBoxKey.Name = "textBoxKey";
			textBoxKey.Size = new Size(231, 31);
			textBoxKey.TabIndex = 3;
			// 
			// textBoxProfileName
			// 
			textBoxProfileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textBoxProfileName.Location = new Point(158, 43);
			textBoxProfileName.Name = "textBoxProfileName";
			textBoxProfileName.Size = new Size(606, 31);
			textBoxProfileName.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(19, 96);
			label1.Name = "label1";
			label1.Size = new Size(133, 25);
			label1.TabIndex = 2;
			label1.Text = "ショートカットキー :";
			// 
			// buttonClearCounter
			// 
			buttonClearCounter.Location = new Point(241, 930);
			buttonClearCounter.Name = "buttonClearCounter";
			buttonClearCounter.Size = new Size(112, 34);
			buttonClearCounter.TabIndex = 8;
			buttonClearCounter.Text = "連番リセット";
			buttonClearCounter.UseVisualStyleBackColor = true;
			buttonClearCounter.Click += buttonClearCounter_Click;
			// 
			// checkBoxRememberNumber
			// 
			checkBoxRememberNumber.AutoSize = true;
			checkBoxRememberNumber.Location = new Point(31, 934);
			checkBoxRememberNumber.Name = "checkBoxRememberNumber";
			checkBoxRememberNumber.Size = new Size(204, 29);
			checkBoxRememberNumber.TabIndex = 7;
			checkBoxRememberNumber.Text = "前回の連番を記憶する";
			checkBoxRememberNumber.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Location = new Point(782, 930);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(112, 34);
			buttonCancel.TabIndex = 11;
			buttonCancel.Text = "キャンセル";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += buttonCancel_Click;
			// 
			// buttonOk
			// 
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonOk.Location = new Point(651, 930);
			buttonOk.Name = "buttonOk";
			buttonOk.Size = new Size(112, 34);
			buttonOk.TabIndex = 10;
			buttonOk.Text = "OK";
			buttonOk.UseVisualStyleBackColor = true;
			buttonOk.Click += buttonOK_Click;
			// 
			// buttonDuplicate
			// 
			buttonDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonDuplicate.Location = new Point(782, 92);
			buttonDuplicate.Name = "buttonDuplicate";
			buttonDuplicate.Size = new Size(112, 34);
			buttonDuplicate.TabIndex = 3;
			buttonDuplicate.Text = "複製";
			buttonDuplicate.UseVisualStyleBackColor = true;
			buttonDuplicate.Click += buttonDuplicate_Click;
			// 
			// buttonMoveUp
			// 
			buttonMoveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonMoveUp.Location = new Point(782, 157);
			buttonMoveUp.Name = "buttonMoveUp";
			buttonMoveUp.Size = new Size(54, 34);
			buttonMoveUp.TabIndex = 4;
			buttonMoveUp.Text = "↑";
			buttonMoveUp.UseVisualStyleBackColor = true;
			buttonMoveUp.Click += buttonMoveUp_Click;
			// 
			// buttonMoveDown
			// 
			buttonMoveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonMoveDown.Location = new Point(840, 157);
			buttonMoveDown.Name = "buttonMoveDown";
			buttonMoveDown.Size = new Size(54, 34);
			buttonMoveDown.TabIndex = 5;
			buttonMoveDown.Text = "↓";
			buttonMoveDown.UseVisualStyleBackColor = true;
			buttonMoveDown.Click += buttonMoveDown_Click;
			// 
			// checkBoxShowNotification
			// 
			checkBoxShowNotification.AutoSize = true;
			checkBoxShowNotification.Location = new Point(386, 934);
			checkBoxShowNotification.Name = "checkBoxShowNotification";
			checkBoxShowNotification.Size = new Size(201, 29);
			checkBoxShowNotification.TabIndex = 9;
			checkBoxShowNotification.Text = "キャプチャ時に通知する";
			checkBoxShowNotification.UseVisualStyleBackColor = true;
			// 
			// FormSettings
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(906, 976);
			Controls.Add(checkBoxShowNotification);
			Controls.Add(buttonMoveDown);
			Controls.Add(buttonMoveUp);
			Controls.Add(buttonDuplicate);
			Controls.Add(buttonClearCounter);
			Controls.Add(checkBoxRememberNumber);
			Controls.Add(buttonOk);
			Controls.Add(buttonCancel);
			Controls.Add(groupBoxProfile);
			Controls.Add(buttonDelete);
			Controls.Add(buttonAdd);
			Controls.Add(listBoxProfile);
			Name = "FormSettings";
			Text = "WowShot2設定";
			Load += FormSetting_Load;
			KeyDown += FormSetting_KeyDown;
			groupBoxProfile.ResumeLayout(false);
			groupBoxProfile.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ListBox listBoxProfile;
		private Button buttonAdd;
		private Button buttonDelete;
		private GroupBox groupBoxProfile;
		private TextBox textBoxKey;
		private TextBox textBoxProfileName;
		private Label label1;
		private Label label2;
		private CheckBox checkBoxAlt;
		private CheckBox checkBoxShift;
		private CheckBox checkBoxCtrl;
		private CheckBox checkBoxClipboard;
		private CheckBox checkBoxSaveFile;
		private Button buttonBrowse;
		private Label label3;
		private TextBox textBoxSaveDir;
		private TextBox textBoxFileName;
		private Label label4;
		private Label label5;
		private ComboBox comboBoxFormat;
		private TextBox textBox1;
		private Button buttonApply;
		private Button buttonCancel;
		private Button buttonOk;
		private TextBox textBoxDelay;
		private CheckBox checkBoxUseDelay;
		private Label label6;
		private Button buttonClearCounter;
		private CheckBox checkBoxRememberNumber;
		private ComboBox comboBoxTarget;
		private Label label7;
		private Button buttonDuplicate;
		private Button buttonMoveUp;
		private Button buttonMoveDown;
		private Button buttonKeyCapture;
		private CheckBox checkBoxShowNotification;
	}
}