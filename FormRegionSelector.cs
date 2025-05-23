using System;
using System.Drawing;
using System.Windows.Forms;

public class FormRegionSelector : Form
{
	public Rectangle SelectedRegion { get; private set; } = Rectangle.Empty;

	private Point startPoint;
	private Point endPoint;
	private bool dragging = false;

	public FormRegionSelector()
	{
		this.FormBorderStyle = FormBorderStyle.None;
		this.StartPosition = FormStartPosition.Manual;

		// ✅ 全ディスプレイをカバーする領域に表示
		this.Location = SystemInformation.VirtualScreen.Location;
		this.Size = SystemInformation.VirtualScreen.Size;

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
			SelectedRegion = GetRectangle(startPoint, endPoint);
			DialogResult = DialogResult.OK;
			Close();
		};
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
