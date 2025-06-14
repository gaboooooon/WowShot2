﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WowShot2;

namespace WowShot2
{
	public class TrayAppContext : ApplicationContext
	{
		private NotifyIcon trayIcon;
		private Form dummyForm;

		private CaptureSettingsManager settingsManager;
		private List<HotKeyManager> hotKeyManagers = new();

		private Point? lastSettingFormLocation = null;

		public TrayAppContext()
		{
			trayIcon = new NotifyIcon()
			{
				Icon = Resource.TrayIcon,
				ContextMenuStrip = new ContextMenuStrip(),
				Text = "WowShot2",
				Visible = true
			};

			trayIcon.ContextMenuStrip.Items.Add("キャプチャ設定...", null, OnOpenSettings);
			trayIcon.ContextMenuStrip.Items.Add("バージョン情報...", null, OnAboutClicked);
			trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			trayIcon.ContextMenuStrip.Items.Add("終了", null, OnExit);

			// 非表示フォーム作成
			dummyForm = new Form();
			dummyForm.ShowInTaskbar = false;
			dummyForm.FormBorderStyle = FormBorderStyle.None;
			dummyForm.StartPosition = FormStartPosition.Manual;
			dummyForm.Size = new Size(0, 0);
			dummyForm.Location = new Point(-1000, -1000);
			dummyForm.Icon = Resource.TrayIcon;

			settingsManager = CaptureSettingsManager.Load();

			int hotKeyIdCounter = 1;

			foreach (var profile in settingsManager.Profiles)
			{
				if (profile.Key == Keys.None) continue;

				int modifiers = 0;
				if (profile.UseCtrl) modifiers |= (int)HotKeyManager.Modifiers.Control;
				if (profile.UseShift) modifiers |= (int)HotKeyManager.Modifiers.Shift;
				if (profile.UseAlt) modifiers |= (int)HotKeyManager.Modifiers.Alt;

				var manager = new HotKeyManager(dummyForm, profile.Key, (HotKeyManager.Modifiers)modifiers, id: hotKeyIdCounter++);

				var boundProfile = profile;
				manager.HotKeyPressed += (s, e) => PerformCapture(boundProfile);

				hotKeyManagers.Add(manager);
			}

			dummyForm.Load += (s, e) => dummyForm.Hide();
			dummyForm.Show();
		}

		private async void PerformCapture(CaptureShortcutProfile profile)
		{
			// 遅延キャプチャ
			if (profile.UseDelay && profile.DelaySeconds > 0)
			{
				await Task.Delay(profile.DelaySeconds * 1000);
			}

			// キャプチャ対象の分岐
			Bitmap? captured = profile.CaptureTarget switch
			{
				"全ディスプレイ" => CaptureHelper.CaptureAllScreens(),
				"アクティブウィンドウ" => CaptureHelper.CaptureActiveWindow(),
				"選択範囲" => CaptureHelper.CaptureSelectedRegion(),
				var target when target.StartsWith("ディスプレイ") =>
					TryCaptureDisplay(target, out var bmp) ? bmp : null,
				_ => null
			};

			if (captured == null)
			{
				MessageBox.Show("キャプチャに失敗しました。\n設定内容をご確認ください。",
								"キャプチャエラー",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
				return;
			}

			// ファイル名生成
			int number = settingsManager.GlobalLastUsedNumber;
			string fileName = ApplyFileNameTemplate(profile.FileNameTemplate, number, DateTime.Now);
			string ext = profile.FileFormat.ToLower();
			string savedFileName = fileName;

			// ファイル保存
			if (profile.SaveToFile)
			{
				string defaultSaveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "WowShot2");

				string saveDir = string.IsNullOrWhiteSpace(profile.SaveDirectory) ? defaultSaveDir : profile.SaveDirectory;
				Directory.CreateDirectory(saveDir); // 存在しなければ作成

				string baseFileName = $"{fileName}.{ext}";
				string fullPath = Path.Combine(saveDir, baseFileName);

				// 上書きを避けるためのファイル名補正処理
				int count = 1;
				while (File.Exists(fullPath))
				{
					string numberedFileName = $"{fileName}_{count}.{ext}";
					fullPath = Path.Combine(saveDir, numberedFileName);
					count++;
				}

				// 保存
				captured.Save(fullPath, ext switch
				{
					"jpg" => ImageFormat.Jpeg,
					"bmp" => ImageFormat.Bmp,
					_ => ImageFormat.Png
				});

				savedFileName = Path.GetFileName(fullPath); // 保存したファイル名を取得
			}

			// クリップボードにコピー
			if (profile.CopyToClipboard)
			{
				Clipboard.SetImage(captured);
			}

			// 連番更新
			if (settingsManager.RememberGlobalLastUsedNumber)
			{
				settingsManager.GlobalLastUsedNumber++;
				settingsManager.Save();
			}

			if (settingsManager.ShowCaptureNotification)
			{
				trayIcon.ShowBalloonTip(1000, "キャプチャ完了", $"{savedFileName} を保存しました", ToolTipIcon.Info);
			}
		}

		private bool TryCaptureDisplay(string target, out Bitmap? bitmap)
		{
			bitmap = null;

			if (!int.TryParse(target.Replace("ディスプレイ", ""), out int index)) return false;

			int screenCount = Screen.AllScreens.Length;
			if (index < 1 || index > screenCount) return false;

			bitmap = CaptureHelper.CaptureScreenIndex(index - 1); // 0-based index
			//bitmap = CaptureHelper.CapturePhysicalScreen(index - 1); // 0-based index
			return bitmap != null;
		}

		private void OnOpenSettings(object? sender, EventArgs e)
		{
			ShowSettingForm();
		}

		private void ShowSettingForm()
		{
			var settingForm = new FormSettings(this);

			if (lastSettingFormLocation == null)
			{
				// 初回表示：画面中央に表示（StartPosition設定で対応）
				settingForm.StartPosition = FormStartPosition.CenterScreen;
			}
			else
			{
				// 2回目以降：前回の位置
				settingForm.StartPosition = FormStartPosition.Manual;
				settingForm.Location = lastSettingFormLocation.Value;
			}

			settingForm.FormClosed += (s, e) =>
			{
				lastSettingFormLocation = settingForm.Location;
				settingForm.Dispose();
			};

			settingForm.Show();
		}

		private void OnAboutClicked(object? sender, EventArgs e)
		{
			using var dlg = new FormAbout();
			dlg.StartPosition = FormStartPosition.CenterScreen;
			dlg.ShowDialog();
		}

		private void OnExit(object? sender, EventArgs e)
		{
			foreach (var manager in hotKeyManagers)
			{
				manager.Dispose();
			}

			trayIcon.Visible = false;
			dummyForm.Close();
			Application.Exit();
		}

		public static string ApplyFileNameTemplate(string template, int number, DateTime now)
		{
			// 連番の %NNNNNN 部分を探して置換
			string fileName = Regex.Replace(template, @"%N+", match =>
			{
				int digitCount = match.Value.Length - 1; // % を除いた N の数
				return number.ToString().PadLeft(digitCount, '0');
			});

			// 日付・時刻の置換
			fileName = fileName
				.Replace("%YYYY", now.ToString("yyyy"))
				.Replace("%MM", now.ToString("MM"))
				.Replace("%DD", now.ToString("dd"))
				.Replace("%hh", now.ToString("HH"))
				.Replace("%mm", now.ToString("mm"))
				.Replace("%ss", now.ToString("ss"));

			return fileName;
		}

		public void ReRegisterHotKeys(CaptureSettingsManager newManager)
		{
			// 既存ホットキー解除
			foreach (var manager in hotKeyManagers)
			{
				manager.Dispose();
			}

			hotKeyManagers.Clear();

			settingsManager = newManager;

			int hotKeyIdCounter = 1;

			foreach (var profile in settingsManager.Profiles)
			{
				if (profile.Key == Keys.None) continue;

				int modifiers = 0;
				if (profile.UseCtrl) modifiers |= (int)HotKeyManager.Modifiers.Control;
				if (profile.UseShift) modifiers |= (int)HotKeyManager.Modifiers.Shift;
				if (profile.UseAlt) modifiers |= (int)HotKeyManager.Modifiers.Alt;

				var manager = new HotKeyManager(dummyForm, profile.Key, (HotKeyManager.Modifiers)modifiers, id: hotKeyIdCounter++);

				var boundProfile = profile;
				manager.HotKeyPressed += (s, e) => PerformCapture(boundProfile);

				hotKeyManagers.Add(manager);
			}
		}
	}
}