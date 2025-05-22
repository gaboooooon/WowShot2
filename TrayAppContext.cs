using System;
using System.Collections.Generic;
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
		//private HotKeyManager hotKeyManager;
		//private HotKeyManager activeWindowHotKeyManager;
		//private CaptureSettings settings;

		//private List<HotKeyManager> screenHotKeys = new();
		//private Dictionary<int, int> hotKeyIdToScreenIndex = new();
		//private int hotKeyIdCounter = 10; // ユニークなID

		private CaptureSettingsManager settingsManager;
		private List<HotKeyManager> hotKeyManagers = new();

		private Point? lastSettingFormLocation = null;

		public TrayAppContext()
		{
			//settings = CaptureSettings.Load();

			trayIcon = new NotifyIcon()
			{
				Icon = SystemIcons.Application,
				ContextMenuStrip = new ContextMenuStrip(),
				Text = "WowShot2",
				Visible = true
			};

			//trayIcon.ContextMenuStrip.Items.Add("今すぐキャプチャ", null, OnCaptureNow);
			//trayIcon.ContextMenuStrip.Items.Add("アクティブウィンドウをキャプチャ", null, OnCaptureActiveWindow);
			//trayIcon.ContextMenuStrip.Items.Add("範囲選択してキャプチャ", null, OnCaptureRegion);
			trayIcon.ContextMenuStrip.Items.Add("キャプチャ設定...", null, OnOpenSettings);
			trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			trayIcon.ContextMenuStrip.Items.Add("終了", null, OnExit);

			// 非表示フォーム作成
			dummyForm = new Form();
			dummyForm.ShowInTaskbar = false;
			dummyForm.FormBorderStyle = FormBorderStyle.None;
			dummyForm.StartPosition = FormStartPosition.Manual;
			dummyForm.Size = new Size(0, 0);
			dummyForm.Location = new Point(-1000, -1000);

			//dummyForm.Shown += (s, e) =>
			//{
			//	hotKeyManager = new HotKeyManager(dummyForm, Keys.A, HotKeyManager.Modifiers.Control | HotKeyManager.Modifiers.Shift);
			//	hotKeyManager.HotKeyPressed += (s2, e2) => OnCaptureNow(null, null);
			//};

			//dummyForm.Shown += (s, e) =>
			//{
			//	// 既存の全画面キャプチャ (Ctrl + Shift + A)
			//	hotKeyManager = new HotKeyManager(dummyForm, Keys.A, HotKeyManager.Modifiers.Control | HotKeyManager.Modifiers.Shift);
			//	hotKeyManager.HotKeyPressed += (s2, e2) => OnCaptureNow(null, null);

			//	// 新規追加：アクティブウィンドウキャプチャ (Ctrl + Shift + W)
			//	activeWindowHotKeyManager = new HotKeyManager(dummyForm, Keys.W, HotKeyManager.Modifiers.Control | HotKeyManager.Modifiers.Shift);
			//	activeWindowHotKeyManager.HotKeyPressed += (s3, e3) => OnCaptureActiveWindow(null, null);
			//};

			//dummyForm.Shown += (s, e) =>
			//{
			//	// 既存ホットキー（全画面・アクティブウィンドウ）もそのまま残す

			//	for (int i = 0; i < Screen.AllScreens.Length; i++)
			//	{
			//		// キー：Ctrl + Shift + (1〜9)
			//		Keys key = Keys.D1 + i; // D1 = '1' のキーコード

			//		var manager = new HotKeyManager(dummyForm, key, HotKeyManager.Modifiers.Control | HotKeyManager.Modifiers.Shift, hotKeyIdCounter);
			//		manager.HotKeyPressed += OnScreenHotKeyPressed;

			//		screenHotKeys.Add(manager);
			//		hotKeyIdToScreenIndex[hotKeyIdCounter] = i;

			//		hotKeyIdCounter++;
			//	}
			//};


			settingsManager = CaptureSettingsManager.Load();

			foreach (var profile in settingsManager.Profiles)
			{
				int modifiers = 0;
				if (profile.UseCtrl) modifiers |= (int)HotKeyManager.Modifiers.Control;
				if (profile.UseShift) modifiers |= (int)HotKeyManager.Modifiers.Shift;
				if (profile.UseAlt) modifiers |= (int)HotKeyManager.Modifiers.Alt;

				var manager = new HotKeyManager(dummyForm, profile.Key, (HotKeyManager.Modifiers)modifiers);
				manager.HotKeyPressed += (s, e) => PerformCapture(profile);
				hotKeyManagers.Add(manager);
			}

			dummyForm.Load += (s, e) => dummyForm.Hide();
			dummyForm.Show();
		}

		private async void PerformCapture(CaptureShortcutProfile profile)
		{
			if (profile.UseDelay)
			{
				await Task.Delay(profile.DelaySeconds * 1000);
			}

			Bitmap? captured = null;
			//Bitmap? captured = profile.Target switch
			//{
			//	"FullScreen" => CaptureHelper.CaptureAllScreens(),
			//	"ActiveWindow" => CaptureHelper.CaptureActiveWindow(),
			//	"Selection" => CaptureHelper.CaptureSelectedRegion(),
			//	"Display1" => CaptureHelper.CaptureScreenIndex(0),
			//	"Display2" => CaptureHelper.CaptureScreenIndex(1),
			//	_ => null
			//};

			if (captured == null) return;

			// ファイル名生成
			string fileName = ApplyFileNameTemplate(profile.FileNameTemplate, profile.LastUsedNumber, DateTime.Now);
			string ext = profile.FileFormat.ToLower();
			string fullPath = Path.Combine(profile.SaveDirectory, $"{fileName}.{ext}");

			if (profile.SaveToFile)
			{
				Directory.CreateDirectory(profile.SaveDirectory);
				captured.Save(fullPath, ext switch
				{
					"jpg" => ImageFormat.Jpeg,
					"bmp" => ImageFormat.Bmp,
					_ => ImageFormat.Png
				});
			}

			if (profile.CopyToClipboard)
			{
				Clipboard.SetImage(captured);
			}

			if (profile.RememberLastNumber)
			{
				profile.LastUsedNumber++;
				settingsManager.Save(); // 保存
			}

			trayIcon.ShowBalloonTip(1000, "キャプチャ完了", $"{fileName}.{ext} を保存しました", ToolTipIcon.Info);
		}

		//public class FlashOverlay : Form
		//{
		//	public FlashOverlay()
		//	{
		//		this.FormBorderStyle = FormBorderStyle.None;
		//		this.Bounds = SystemInformation.VirtualScreen;
		//		this.BackColor = Color.White;
		//		this.Opacity = 0.6;
		//		this.TopMost = true;
		//		this.ShowInTaskbar = false;
		//	}

		//	public void Flash(int durationMs = 100)
		//	{
		//		Show();
		//		Task.Delay(durationMs).ContinueWith(_ => Invoke(new Action(() => Close())));
		//	}
		//}

		//private void OnCaptureNow(object sender, EventArgs e)
		//{
		//	string saveDir = string.IsNullOrWhiteSpace(settings.SaveDirectory)
		//		? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ScreenCaptures")
		//		: settings.SaveDirectory;

		//	Directory.CreateDirectory(saveDir);

		//	try
		//	{
		//		// シャッター音
		//		System.Media.SystemSounds.Asterisk.Play(); // またはカスタム音

		//		// 実際のキャプチャ
		//		CaptureHelper.CaptureAllScreens(saveDir, settings.SaveFormat);
		//		trayIcon.ShowBalloonTip(1000, "キャプチャ成功", "画面を保存しました。", ToolTipIcon.Info);
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("キャプチャに失敗しました:\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		//private void OnCaptureActiveWindow(object sender, EventArgs e)
		//{
		//	string saveDir = string.IsNullOrWhiteSpace(settings.SaveDirectory)
		//		? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ScreenCaptures")
		//		: settings.SaveDirectory;

		//	Directory.CreateDirectory(saveDir);

		//	try
		//	{
		//		CaptureHelper.CaptureActiveWindow(saveDir, settings.SaveFormat);
		//		trayIcon.ShowBalloonTip(1000, "キャプチャ成功", "アクティブウィンドウを保存しました。", ToolTipIcon.Info);
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("アクティブウィンドウのキャプチャに失敗しました:\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		//private void OnScreenHotKeyPressed(object sender, EventArgs e)
		//{
		//	if (sender is HotKeyManager manager &&
		//		hotKeyIdToScreenIndex.TryGetValue(manager.HotKeyId, out int screenIndex))
		//	{
		//		string saveDir = string.IsNullOrWhiteSpace(settings.SaveDirectory)
		//			? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ScreenCaptures")
		//			: settings.SaveDirectory;

		//		Directory.CreateDirectory(saveDir);

		//		try
		//		{
		//			//CaptureHelper.CaptureSpecificScreen(screenIndex, saveDir, settings.SaveFormat);
		//			CaptureHelper.CapturePhysicalScreen(screenIndex, saveDir, settings.SaveFormat);

		//			trayIcon.ShowBalloonTip(1000, "キャプチャ成功", $"ディスプレイ {screenIndex + 1} を保存しました。", ToolTipIcon.Info);
		//		}
		//		catch (Exception ex)
		//		{
		//			MessageBox.Show($"ディスプレイ{screenIndex + 1}のキャプチャに失敗しました:\n" + ex.Message,
		//							"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//		}
		//	}
		//}

		//private void OnCaptureRegion(object sender, EventArgs e)
		//{
		//	string saveDir = string.IsNullOrWhiteSpace(settings.SaveDirectory)
		//		? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ScreenCaptures")
		//		: settings.SaveDirectory;

		//	Directory.CreateDirectory(saveDir);

		//	try
		//	{
		//		CaptureHelper.CaptureSelectedRegion(saveDir, settings.SaveFormat);
		//		trayIcon.ShowBalloonTip(1000, "キャプチャ成功", "指定範囲を保存しました。", ToolTipIcon.Info);
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("範囲選択キャプチャに失敗しました:\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
		//}

		private void OnOpenSettings(object sender, EventArgs e)
		{
			//using var form = new FormSettings();
			//form.ShowDialog();

			ShowSettingForm();
		}

		private void ShowSettingForm()
		{
			var settingForm = new FormSettings();

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

		private void OnExit(object sender, EventArgs e)
		{
			//hotKeyManager?.Dispose();
			//activeWindowHotKeyManager?.Dispose();
			//foreach (var hk in screenHotKeys)
			//{
			//	hk.Dispose();
			//}

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
	}
}