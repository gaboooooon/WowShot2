using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WowShot2
{
	public class HotKeyManager : NativeWindow, IDisposable
	{
		public event EventHandler HotKeyPressed;

		private const int WM_HOTKEY = 0x0312;

		public int HotKeyId { get; private set; }

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		[Flags]
		public enum Modifiers : uint
		{
			None = 0,
			Alt = 1,
			Control = 2,
			Shift = 4,
			Win = 8
		}

		public HotKeyManager(Form hiddenForm, Keys key, Modifiers modifiers, int id = 1)
		{
			HotKeyId = id;
			AssignHandle(hiddenForm.Handle);

			bool success = RegisterHotKey(this.Handle, HotKeyId, (uint)modifiers, key);
			if (!success)
			{
				MessageBox.Show("ホットキーの登録に失敗しました", "エラー");
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HotKeyId)
			{
				HotKeyPressed?.Invoke(this, EventArgs.Empty);
			}
			base.WndProc(ref m);
		}

		public void Dispose()
		{
			UnregisterHotKey(this.Handle, HotKeyId);
			ReleaseHandle();
		}
	}
}
