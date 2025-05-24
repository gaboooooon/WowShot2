using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WowShot2
{
	public partial class FormKeyCapture : Form
	{
		private readonly KeyCaptureHook keyHook = new();

		public Keys CapturedKey { get; private set; } = Keys.None;
		public Keys Modifiers { get; private set; } = Keys.None;

		public FormKeyCapture()
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "キーを押してください";
			this.Size = new Size(300, 300); 
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.ControlBox = false;
			this.KeyPreview = true;
			this.Shown += FormKeyCapture_Shown;
		}

		private void FormKeyCapture_Shown(object? sender, EventArgs e)
		{
			keyHook.KeyCaptured += (key, mods) =>
			{
				this.CapturedKey = key;
				this.Modifiers = mods;
				this.DialogResult = DialogResult.OK;
				this.Close();
			};
			keyHook.Start();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			keyHook.Stop();
			base.OnFormClosing(e);
		}
	}
}
