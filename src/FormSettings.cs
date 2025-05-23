using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WowShot2
{
	public partial class FormSettings : Form
	{
		private CaptureSettingsManager settingsManager = new CaptureSettingsManager();
		private CaptureShortcutProfile? selectedProfile;

		private bool isCapturingKey = false;
		private Keys capturedKey = Keys.None;

		private TrayAppContext trayAppContext;

		public FormSettings(TrayAppContext context)
		{
			InitializeComponent();
			trayAppContext = context;
		}

		private void FormSetting_Load(object sender, EventArgs e)
		{
			this.Icon = Resource.TrayIcon; // アイコンを設定

			settingsManager = CaptureSettingsManager.Load();
			foreach (var profile in settingsManager.Profiles)
			{
				listBoxProfile.Items.Add(profile.ProfileName);
			}

			if (listBoxProfile.Items.Count > 0)
				listBoxProfile.SelectedIndex = 0;

			comboBoxFormat.Items.Clear();
			comboBoxFormat.Items.AddRange(new string[] { "jpg", "png", "bmp" });
			comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxFormat.SelectedIndex = 0; // デフォルト: jpg

			comboBoxTarget.Items.Clear();
			comboBoxTarget.Items.AddRange(new string[]
			{
				"選択範囲",
				"アクティブウィンドウ",
				"全ディスプレイ",
				"ディスプレイ1",
				"ディスプレイ2",
				"ディスプレイ3",
				"ディスプレイ4",
				"ディスプレイ5",
				"ディスプレイ6",
				"ディスプレイ7",
				"ディスプレイ8",
				"ディスプレイ9"
			});
			comboBoxTarget.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxTarget.SelectedIndex = 2; // デフォルト：全ディスプレイ

			checkBoxRememberNumber.Checked = settingsManager.RememberGlobalLastUsedNumber;
			checkBoxShowNotification.Checked = settingsManager.ShowCaptureNotification;
		}

		private void buttonKeyCapture_Click(object sender, EventArgs e)
		{
			isCapturingKey = true;
			textBoxKey.Text = "キーを押してください...";
			this.KeyPreview = true; // フォームでKeyDownを受け取る
		}

		private void FormSetting_KeyDown(object sender, KeyEventArgs e)
		{
			if (!isCapturingKey) return;

			// キャンセル：Escキー押下
			if (e.KeyCode == Keys.Escape)
			{
				isCapturingKey = false;
				this.KeyPreview = false;
				capturedKey = Keys.None;
				textBoxKey.Text = "None";
				return;
			}

			// 無効：Ctrl/Shift/Alt単独
			if (e.KeyCode == Keys.ControlKey ||
				e.KeyCode == Keys.ShiftKey ||
				e.KeyCode == Keys.Menu) // Altキー
			{
				//MessageBox.Show("Ctrl / Shift / Alt 単独では登録できません。", "無効なキー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//textBoxKey.Text = "キーを押してください...";
				return;
			}

			// 有効なキー入力
			isCapturingKey = false;
			this.KeyPreview = false;

			capturedKey = e.KeyCode;
			textBoxKey.Text = e.KeyCode.ToString();

			// ※キーキャプチャは修飾キーは対象にしない（※修飾キーはチェックボックスで設定してもらう）
			//checkBoxCtrl.Checked = e.Control;
			//checkBoxShift.Checked = e.Shift;
			//checkBoxAlt.Checked = e.Alt;

			e.SuppressKeyPress = true;
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			var newProfile = new CaptureShortcutProfile
			{
				ProfileName = "新しいプロファイル",
				Key = Keys.None
			};

			settingsManager.Profiles.Add(newProfile);
			listBoxProfile.Items.Add(newProfile.ProfileName);
			listBoxProfile.SelectedIndex = listBoxProfile.Items.Count - 1;
		}

		private bool HasUnsavedChanges()
		{
			if (selectedProfile == null) return false;

			return
				textBoxProfileName.Text != selectedProfile.ProfileName ||
				textBoxKey.Text != selectedProfile.Key.ToString() ||
				checkBoxCtrl.Checked != selectedProfile.UseCtrl ||
				checkBoxShift.Checked != selectedProfile.UseShift ||
				checkBoxAlt.Checked != selectedProfile.UseAlt ||
				checkBoxUseDelay.Checked != selectedProfile.UseDelay ||
				textBoxDelay.Text != selectedProfile.DelaySeconds.ToString() ||
				comboBoxTarget.Text != selectedProfile.CaptureTarget ||
				checkBoxSaveFile.Checked != selectedProfile.SaveToFile ||
				textBoxSaveDir.Text != selectedProfile.SaveDirectory ||
				textBoxFileName.Text != selectedProfile.FileNameTemplate ||
				comboBoxFormat.Text != selectedProfile.FileFormat ||
				checkBoxClipboard.Checked != selectedProfile.CopyToClipboard;
		}

		private void listBoxProfile_SelectedIndexChanged(object? sender, EventArgs e)
		{
			if (selectedProfile != null && HasUnsavedChanges())
			{
				var result = MessageBox.Show(
					"このプロファイルには未保存の変更があります。切り替えると失われます。\n続けますか？",
					"確認",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning
				);

				if (result == DialogResult.No)
				{
					// 強制的に元に戻す（選択変更しない）
					listBoxProfile.SelectedIndexChanged -= listBoxProfile_SelectedIndexChanged;
					listBoxProfile.SelectedItem = selectedProfile.ProfileName;
					listBoxProfile.SelectedIndexChanged += listBoxProfile_SelectedIndexChanged;
					return;
				}
			}

			if (listBoxProfile.SelectedIndex >= 0)
			{
				selectedProfile = settingsManager.Profiles[listBoxProfile.SelectedIndex];
				LoadProfileToUI(selectedProfile);
			}
		}

		private void LoadProfileToUI(CaptureShortcutProfile profile)
		{
			textBoxProfileName.Text = profile.ProfileName;
			textBoxKey.Text = profile.Key.ToString();
			checkBoxCtrl.Checked = profile.UseCtrl;
			checkBoxShift.Checked = profile.UseShift;
			checkBoxAlt.Checked = profile.UseAlt;

			checkBoxUseDelay.Checked = profile.UseDelay;
			textBoxDelay.Text = profile.DelaySeconds.ToString();

			comboBoxTarget.SelectedItem = profile.CaptureTarget;

			checkBoxSaveFile.Checked = profile.SaveToFile;
			textBoxSaveDir.Text = profile.SaveDirectory;
			textBoxFileName.Text = profile.FileNameTemplate;
			comboBoxFormat.Text = profile.FileFormat;

			checkBoxClipboard.Checked = profile.CopyToClipboard;
		}

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			int index = listBoxProfile.SelectedIndex;
			if (index < 0)
			{
				MessageBox.Show("削除するプロファイルを選択してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var result = MessageBox.Show("選択されたプロファイルを削除しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result != DialogResult.Yes) return;

			// プロファイル削除
			settingsManager.Profiles.RemoveAt(index);
			listBoxProfile.Items.RemoveAt(index);

			// 保存
			settingsManager.Save();

			// UIリセットまたは別プロファイルを再表示
			if (listBoxProfile.Items.Count > 0)
			{
				listBoxProfile.SelectedIndex = Math.Min(index, listBoxProfile.Items.Count - 1);
			}
			else
			{
				selectedProfile = null;
				ClearUIFields();
			}
		}

		private void ClearUIFields()
		{
			textBoxProfileName.Text = "";
			textBoxKey.Text = "";
			checkBoxCtrl.Checked = false;
			checkBoxShift.Checked = false;
			checkBoxAlt.Checked = false;

			checkBoxUseDelay.Checked = false;
			textBoxDelay.Text = "";

			comboBoxTarget.SelectedIndex = -1;

			checkBoxSaveFile.Checked = false;
			textBoxSaveDir.Text = "";
			textBoxFileName.Text = "";
			comboBoxFormat.SelectedIndex = -1;

			checkBoxClipboard.Checked = false;
		}

		private void buttonDuplicate_Click(object sender, EventArgs e)
		{
			int index = listBoxProfile.SelectedIndex;
			if (index < 0) return;

			var original = settingsManager.Profiles[index];
			var copy = JsonSerializer.Deserialize<CaptureShortcutProfile>(
				JsonSerializer.Serialize(original))!;

			copy.ProfileName += " - コピー";
			settingsManager.Profiles.Insert(index + 1, copy);
			listBoxProfile.Items.Insert(index + 1, copy.ProfileName);
			listBoxProfile.SelectedIndex = index + 1;

			settingsManager.Save();
		}

		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			int index = listBoxProfile.SelectedIndex;
			if (index > 0)
			{
				// プロファイルとリスト項目の入れ替え
				(settingsManager.Profiles[index - 1], settingsManager.Profiles[index]) =
					(settingsManager.Profiles[index], settingsManager.Profiles[index - 1]);

				object item = listBoxProfile.Items[index];
				listBoxProfile.Items[index] = listBoxProfile.Items[index - 1];
				listBoxProfile.Items[index - 1] = item;

				listBoxProfile.SelectedIndex = index - 1;
				settingsManager.Save();
			}
		}

		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			int index = listBoxProfile.SelectedIndex;
			if (index >= 0 && index < listBoxProfile.Items.Count - 1)
			{
				(settingsManager.Profiles[index + 1], settingsManager.Profiles[index]) =
					(settingsManager.Profiles[index], settingsManager.Profiles[index + 1]);

				object item = listBoxProfile.Items[index];
				listBoxProfile.Items[index] = listBoxProfile.Items[index + 1];
				listBoxProfile.Items[index + 1] = item;

				listBoxProfile.SelectedIndex = index + 1;
				settingsManager.Save();
			}
		}

		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog dialog = new FolderBrowserDialog())
			{
				dialog.Description = "保存先フォルダを選択してください。";
				dialog.SelectedPath = textBoxSaveDir.Text;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					textBoxSaveDir.Text = dialog.SelectedPath;
				}
			}
		}

		private void buttonApply_Click(object sender, EventArgs e)
		{
			string? captureTarget = comboBoxTarget.SelectedItem?.ToString();

			// ディスプレイ数チェック
			if (captureTarget != null && captureTarget.StartsWith("ディスプレイ"))
			{
				if (int.TryParse(captureTarget.Replace("ディスプレイ", ""), out int index))
				{
					int screenCount = Screen.AllScreens.Length;
					if (index < 1 || index > screenCount)
					{
						MessageBox.Show(
							$"選択された {captureTarget} は現在の環境に存在しません。\n現在のディスプレイ数は {screenCount} 台です。",
							"無効なディスプレイ",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning);

						return; // 保存処理を中断
					}
				}
			}

			if (selectedProfile == null) return;

			selectedProfile.ProfileName = textBoxProfileName.Text;
			selectedProfile.Key = Enum.TryParse(textBoxKey.Text, out Keys key) ? key : Keys.None;

			selectedProfile.UseCtrl = checkBoxCtrl.Checked;
			selectedProfile.UseShift = checkBoxShift.Checked;
			selectedProfile.UseAlt = checkBoxAlt.Checked;

			selectedProfile.UseDelay = checkBoxUseDelay.Checked;
			selectedProfile.DelaySeconds = int.TryParse(textBoxDelay.Text, out int delay) ? delay : 0;

			selectedProfile.CaptureTarget = captureTarget ?? "全ディスプレイ";

			selectedProfile.SaveToFile = checkBoxSaveFile.Checked;
			selectedProfile.SaveDirectory = textBoxSaveDir.Text;
			selectedProfile.FileNameTemplate = textBoxFileName.Text;
			selectedProfile.FileFormat = comboBoxFormat.Text;

			selectedProfile.CopyToClipboard = checkBoxClipboard.Checked;

			listBoxProfile.Items[listBoxProfile.SelectedIndex] = selectedProfile.ProfileName;

			if (IsHotKeyConflict(selectedProfile))
			{
				MessageBox.Show("このショートカットキーはすでに他のプロファイルで使用されています。",
								"ホットキーの重複", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			settingsManager.Save();
			trayAppContext.ReRegisterHotKeys(settingsManager); // ← 再登録を通知

			MessageBox.Show("設定を保存しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private bool IsHotKeyConflict(CaptureShortcutProfile targetProfile)
		{
			// キー未設定の場合はチェック不要
			if (targetProfile.Key == Keys.None)
				return false;

			return settingsManager.Profiles.Any(p =>
				p != targetProfile &&
				p.Key != Keys.None &&
				p.Key == targetProfile.Key &&
				p.UseCtrl == targetProfile.UseCtrl &&
				p.UseShift == targetProfile.UseShift &&
				p.UseAlt == targetProfile.UseAlt);
		}

		private void buttonClearCounter_Click(object sender, EventArgs e)
		{
			settingsManager.GlobalLastUsedNumber = 1;
			settingsManager.Save();

			MessageBox.Show("連番を初期化しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			settingsManager.RememberGlobalLastUsedNumber = checkBoxRememberNumber.Checked;
			settingsManager.ShowCaptureNotification = checkBoxShowNotification.Checked;

			buttonApply_Click(sender, e); // 保存処理
			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close(); // 変更は保存されない
		}

	}
}
