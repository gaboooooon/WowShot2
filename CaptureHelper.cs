using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WowShot2
{
	public static class CaptureHelper
	{
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

		[DllImport("dwmapi.dll")]
		private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

		private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport("Shcore.dll")]
		private static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

		private enum MonitorDpiType
		{
			MDT_EFFECTIVE_DPI = 0,
			MDT_ANGULAR_DPI = 1,
			MDT_RAW_DPI = 2,
			MDT_DEFAULT = MDT_EFFECTIVE_DPI
		}

		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromPoint(Point pt, uint dwFlags);


		public static Bitmap CaptureAllScreens()
		{
			Rectangle bounds = GetVirtualScreenBounds();

			Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
			}

			return bitmap;
		}

		private static Rectangle GetVirtualScreenBounds()
		{
			int left = int.MaxValue;
			int top = int.MaxValue;
			int right = int.MinValue;
			int bottom = int.MinValue;

			foreach (var screen in Screen.AllScreens)
			{
				if (screen.Bounds.Left < left) left = screen.Bounds.Left;
				if (screen.Bounds.Top < top) top = screen.Bounds.Top;
				if (screen.Bounds.Right > right) right = screen.Bounds.Right;
				if (screen.Bounds.Bottom > bottom) bottom = screen.Bounds.Bottom;
			}

			return new Rectangle(left, top, right - left, bottom - top);
		}

		public static Bitmap CaptureActiveWindow()
		{
			IntPtr hWnd = GetForegroundWindow();

			if (hWnd == IntPtr.Zero)
				throw new InvalidOperationException("アクティブウィンドウが取得できませんでした。");

			// DPI補正済みのウィンドウ矩形を取得
			RECT rect;
			int result = DwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, out rect, Marshal.SizeOf(typeof(RECT)));
			if (result != 0)
				throw new InvalidOperationException("DwmGetWindowAttribute による取得に失敗しました。");

			Rectangle bounds = new Rectangle(
				rect.Left,
				rect.Top,
				rect.Right - rect.Left,
				rect.Bottom - rect.Top
			);

			Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
			}

			return bitmap;
		}

		//public static Bitmap? CaptureSelectedRegion()
		//{
		//	using FormRegionSelector selector = new FormRegionSelector();
		//	if (selector.ShowDialog() == DialogResult.OK)
		//	{
		//		Rectangle region = selector.SelectedRegion;
		//		if (region.Width == 0 || region.Height == 0)
		//			throw new InvalidOperationException("範囲が無効です。");

		//		Bitmap bitmap = new Bitmap(region.Width, region.Height);

		//		using (Graphics g = Graphics.FromImage(bitmap))
		//		{
		//			g.CopyFromScreen(region.Location, Point.Empty, region.Size);
		//		}

		//		return bitmap;
		//	}

		//	return null;
		//}

		public static Bitmap? CaptureSelectedRegion()
		{
			using FormRegionSelector selector = new FormRegionSelector();
			if (selector.ShowDialog() == DialogResult.OK)
			{
				Rectangle region = selector.SelectedRegion;

				if (region.Width == 0 || region.Height == 0)
					throw new InvalidOperationException("範囲が無効です。");

				// ✅ 選択範囲の座標を「仮想スクリーン左上」基準に補正
				Rectangle screenRegion = new Rectangle(
					region.Left + SystemInformation.VirtualScreen.Left,
					region.Top + SystemInformation.VirtualScreen.Top,
					region.Width,
					region.Height
				);

				Bitmap bitmap = new Bitmap(screenRegion.Width, screenRegion.Height);

				using (Graphics g = Graphics.FromImage(bitmap))
				{
					g.CopyFromScreen(screenRegion.Location, Point.Empty, screenRegion.Size);
				}

				return bitmap;
			}

			return null;
		}

		public static Bitmap CaptureScreenIndex(int screenIndex)
		{
			var screens = Screen.AllScreens;
			if (screenIndex < 0 || screenIndex >= screens.Length)
				throw new ArgumentOutOfRangeException(nameof(screenIndex));

			var screen = screens[screenIndex];
			var bounds = screen.Bounds;

			Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
			}

			return bitmap;
		}

		// ※CapturePhysicalScreen()は、DWMを使用せずに物理的なスクリーンをキャプチャするためのものです。
		//   .Net 6以降では、DWMを使用せずに物理的なスクリーンをキャプチャする方法が提供されているため、
		//   CapturePhysicalScreen()は必要ないかもしれません。

		//public static Bitmap CapturePhysicalScreen(int screenIndex)
		//{
		//	var monitors = GetAllMonitors();

		//	if (screenIndex < 0 || screenIndex >= monitors.Count)
		//		throw new ArgumentOutOfRangeException(nameof(screenIndex));

		//	var bounds = monitors[screenIndex];

		//	Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

		//	using (Graphics g = Graphics.FromImage(bitmap))
		//	{
		//		g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
		//	}

		//	return bitmap;
		//}

		//public static List<Rectangle> GetAllMonitors()
		//{
		//	List<Rectangle> monitorRects = new List<Rectangle>();

		//	bool MonitorEnum(IntPtr hMonitor, IntPtr hdc, ref RECT lprcMonitor, IntPtr dwData)
		//	{
		//		Rectangle bounds = new Rectangle(
		//			lprcMonitor.Left,
		//			lprcMonitor.Top,
		//			lprcMonitor.Right - lprcMonitor.Left,
		//			lprcMonitor.Bottom - lprcMonitor.Top
		//		);
		//		monitorRects.Add(bounds);
		//		return true;
		//	}

		//	EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, IntPtr.Zero);
		//	return monitorRects;
		//}

		//private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

		//[DllImport("user32.dll")]
		//private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);
	}
}
