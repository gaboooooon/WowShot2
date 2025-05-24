using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WowShot2
{
	internal static class Program
	{
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private const int SW_RESTORE = 9;

		[STAThread]
		static void Main()
		{
			bool createdNew;
			using var mutex = new System.Threading.Mutex(true, "WowShot2_Mutex", out createdNew);

			if (!createdNew)
			{
				// 既に起動しているインスタンスを探す
				var current = Process.GetCurrentProcess();
				var processes = Process.GetProcessesByName(current.ProcessName);

				foreach (var p in processes)
				{
					if (p.Id != current.Id)
					{
						// メインウィンドウがある場合はアクティブにする（トレイアイコン型では不要かも）
						if (p.MainWindowHandle != IntPtr.Zero)
						{
							ShowWindow(p.MainWindowHandle, SW_RESTORE);
							SetForegroundWindow(p.MainWindowHandle);
						}
						break;
					}
				}

				MessageBox.Show("WowShot2 はすでに起動しています。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ApplicationConfiguration.Initialize();
			Application.Run(new TrayAppContext());
		}
	}
}
