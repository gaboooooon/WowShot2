using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class KeyCaptureHook
{
	public event Action<Keys, Keys>? KeyCaptured;

	private IntPtr _hookId = IntPtr.Zero;
	private NativeMethods.LowLevelKeyboardProc? _proc;

	public void Start()
	{
		_proc = HookCallback;
		_hookId = NativeMethods.SetHook(_proc);
	}

	public void Stop()
	{
		NativeMethods.UnhookWindowsHookEx(_hookId);
	}

	private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode >= 0 && wParam == (IntPtr)NativeMethods.WM_KEYDOWN)
		{
			int vkCode = Marshal.ReadInt32(lParam);
			Keys key = (Keys)vkCode;
			Keys modifiers = Control.ModifierKeys;

			KeyCaptured?.Invoke(key, modifiers);
			Stop(); // 1回で解除
		}

		return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
	}

	private static class NativeMethods
	{
		public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		public const int WH_KEYBOARD_LL = 13;
		public const int WM_KEYDOWN = 0x0100;

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string? lpModuleName);

		public static IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using Process curProcess = Process.GetCurrentProcess();
			using ProcessModule curModule = curProcess.MainModule!;
			return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
		}
	}
}
