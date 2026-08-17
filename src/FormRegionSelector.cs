using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WowShot2;

public class FormRegionSelector : Form
{
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	private const uint SWP_NOZORDER = 0x0004;
	private const uint SWP_NOACTIVATE = 0x0010;

	/// <summary>選択範囲。スクリーン座標（物理ピクセル）で返す。</summary>
	public Rectangle SelectedRegion { get; private set; } = Rectangle.Empty;

	private Point startPoint;
	private Point endPoint;
	private bool dragging = false;

	public FormRegionSelector()
	{
		this.Icon = Resource.TrayIcon; // アイコンを設定
		this.FormBorderStyle = FormBorderStyle.None;
		this.StartPosition = FormStartPosition.Manual;

		// PerMonitorV2 では 1 枚のフォームが異なる DPI のモニタにまたがるため、
		// WinForms の自動スケーリングを無効化し、物理ピクセルのまま扱う。
		this.AutoScaleMode = AutoScaleMode.None;

		// ✅ 全ディスプレイをカバーする領域に表示
		this.Bounds = SystemInformation.VirtualScreen;

		this.DoubleBuffered = true;
		this.TopMost = true;
		this.Opacity = 0.3;
		this.BackColor = Color.Black;
		this.ShowInTaskbar = false;
		this.Cursor = Cursors.Cross;

		this.MouseDown += (s, e) =>
		{
			dragging = true;
			startPoint = e.Location;
			endPoint = e.Location;
			Invalidate();
		};

		this.MouseMove += (s, e) =>
		{
			if (dragging)
			{
				endPoint = e.Location;
				Invalidate();
			}
		};

		this.MouseUp += (s, e) =>
		{
			dragging = false;
			Rectangle clientRegion = GetRectangle(startPoint, endPoint);
			// クライアント座標 → スクリーン座標（物理ピクセル）に変換して返す。
			// 呼び出し側で VirtualScreen のオフセットを足す必要はない。
			SelectedRegion = new Rectangle(this.PointToScreen(clientRegion.Location), clientRegion.Size);
			DialogResult = DialogResult.OK;
			Close();
		};
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		ApplyVirtualScreenBounds();
	}

	protected override void OnShown(EventArgs e)
	{
		base.OnShown(e);
		ApplyVirtualScreenBounds();
	}

	// 複数 DPI にまたがるオーバーレイなので、DPI 変更に伴う自動リサイズ・再スケールを抑止する。
	protected override void OnDpiChanged(DpiChangedEventArgs e)
	{
		e.Cancel = true;
		base.OnDpiChanged(e);
		ApplyVirtualScreenBounds();
	}

	/// <summary>
	/// WinForms の DPI スケーリングを経由せず、仮想スクリーン全体を物理ピクセルで直接指定する。
	/// </summary>
	private void ApplyVirtualScreenBounds()
	{
		Rectangle vs = SystemInformation.VirtualScreen;
		SetWindowPos(this.Handle, IntPtr.Zero, vs.Left, vs.Top, vs.Width, vs.Height, SWP_NOZORDER | SWP_NOACTIVATE);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (dragging)
		{
			using Pen pen = new Pen(Color.Green, 4);
			e.Graphics.DrawRectangle(pen, GetRectangle(startPoint, endPoint));
		}
	}

	private Rectangle GetRectangle(Point p1, Point p2)
	{
		return new Rectangle(
			Math.Min(p1.X, p2.X),
			Math.Min(p1.Y, p2.Y),
			Math.Abs(p1.X - p2.X),
			Math.Abs(p1.Y - p2.Y)
		);
	}
}
