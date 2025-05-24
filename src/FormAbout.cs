using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WowShot2
{
	public partial class FormAbout : Form
	{
		public FormAbout()
		{
			InitializeComponent();

			this.Icon = Resource.TrayIcon; // アイコンを設定

			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ControlBox = true; // 閉じるボタンは残す（任意）
			this.StartPosition = FormStartPosition.CenterParent; // 親フォーム中央に表示
			this.AcceptButton = buttonOK;
			this.Shown += FormAbout_Shown;

			labelVersion.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version}";
		}

		private void FormAbout_Shown(object? sender, EventArgs e)
		{
			buttonOK.Focus();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			// URLを指定
			string url = "https://github.com/gaboooooon/WowShot2";

			try
			{
				// 既定のブラウザで開く
				System.Diagnostics.Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true // 重要：これを true にしないと例外が出ることあり
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show("ブラウザを起動できませんでした。\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
