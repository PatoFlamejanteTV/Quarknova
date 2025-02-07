using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Quarknova.Utilities
{
	// Token: 0x02000005 RID: 5
	public static class Imports
	{
		// Token: 0x06000025 RID: 37
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

		// Token: 0x06000026 RID: 38
		[DllImport("ntdll.dll", SetLastError = true)]
		public static extern uint RtlAdjustPrivilege(int Privilege, bool Enable, bool CurrentThread, out bool Enabled);

		// Token: 0x06000027 RID: 39
		[DllImport("ntdll.dll", SetLastError = true)]
		public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ResponseOption, out uint Response);

		// Token: 0x06000028 RID: 40
		[DllImport("ntdll.dll", SetLastError = true)]
		public static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

		// Token: 0x06000029 RID: 41
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetConsoleWindow();

		// Token: 0x0600002A RID: 42
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetCursorPos(out Imports.POINT lpPoint);

		// Token: 0x0600002B RID: 43
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetCursorPos(int X, int Y);

		// Token: 0x0600002C RID: 44
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x0600002D RID: 45
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

		// Token: 0x0600002E RID: 46
		[DllImport("kernel32.dll", SetLastError = true)]
		public unsafe static extern uint CreateThread(uint* lpThreadAttributes, uint dwStackSize, ThreadStart lpStartAddress, uint* lpParameter, uint dwCreationFlags, out uint lpThreadId);

		// Token: 0x0600002F RID: 47
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

		// Token: 0x06000030 RID: 48
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetSystemMetrics(int nIndex);

		// Token: 0x06000031 RID: 49
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

		// Token: 0x06000032 RID: 50
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);

		// Token: 0x06000033 RID: 51
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out Imports.RECT lpRect);

		// Token: 0x06000034 RID: 52
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06000035 RID: 53
		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern bool BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, Imports.TernaryRasterOperations rop);

		// Token: 0x06000036 RID: 54
		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, Imports.TernaryRasterOperations rop);

		// Token: 0x06000037 RID: 55
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

		// Token: 0x06000038 RID: 56
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		// Token: 0x06000039 RID: 57
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, StringBuilder lParam);

		// Token: 0x0600003A RID: 58
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

		// Token: 0x0600003B RID: 59
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x0600003C RID: 60
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool SetWindowText(IntPtr hWnd, string lpString);

		// Token: 0x0600003D RID: 61
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x0600003E RID: 62
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		// Token: 0x0600003F RID: 63
		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		// Token: 0x06000040 RID: 64
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern void GetSystemTime(out Imports.SYSTEMTIME lpSystemTime);

		// Token: 0x06000041 RID: 65
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetSystemTime(ref Imports.SYSTEMTIME time);

		// Token: 0x06000042 RID: 66
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool EnumChildWindows(IntPtr window, Imports.EnumWindowProc callback, IntPtr lParam);

		// Token: 0x06000043 RID: 67
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, Imports.SetWindowPosFlags uFlags);

		// Token: 0x06000044 RID: 68
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool ShowWindow(IntPtr hWnd, Imports.ShowWindowFlags nCmdShow);

		// Token: 0x06000045 RID: 69
		[DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		// Token: 0x06000046 RID: 70
		[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
		private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x06000047 RID: 71
		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x06000048 RID: 72
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetForegroundWindow();

		// Token: 0x06000049 RID: 73
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x0600004A RID: 74
		[DllImport("user32.dll")]
		public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

		// Token: 0x0600004B RID: 75
		[DllImport("kernel32.dll")]
		public static extern uint GetLastError();

		// Token: 0x0600004C RID: 76 RVA: 0x00003480 File Offset: 0x00001680
		public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 8)
			{
				return Imports.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			}
			return new IntPtr(Imports.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
		}

		// Token: 0x04000003 RID: 3
		public const int SM_CXICON = 11;

		// Token: 0x04000004 RID: 4
		public const int SM_CYICON = 12;

		// Token: 0x04000005 RID: 5
		public const uint GENERIC_ALL = 268435456U;

		// Token: 0x04000006 RID: 6
		public const uint FILE_SHARE_READ = 1U;

		// Token: 0x04000007 RID: 7
		public const uint FILE_SHARE_WRITE = 2U;

		// Token: 0x04000008 RID: 8
		public const uint OPEN_EXISTING = 3U;

		// Token: 0x04000009 RID: 9
		public static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

		// Token: 0x0400000A RID: 10
		public const int LVM_FIRST = 4096;

		// Token: 0x0400000B RID: 11
		public const int LVM_GETITEMCOUNT = 4100;

		// Token: 0x0400000C RID: 12
		public const int LVM_SETITEMPOSITION = 4111;

		// Token: 0x0400000D RID: 13
		public const int GWL_EXSTYLE = -20;

		// Token: 0x0400000E RID: 14
		public const int WS_EX_LAYERED = 524288;

		// Token: 0x0400000F RID: 15
		public const int LWA_ALPHA = 2;

		// Token: 0x04000010 RID: 16
		public const int LWA_COLORKEY = 1;

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x060000AA RID: 170
		public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

		// Token: 0x02000012 RID: 18
		public struct POINT
		{
			// Token: 0x060000AD RID: 173 RVA: 0x000061CB File Offset: 0x000043CB
			public static implicit operator Point(Imports.POINT point)
			{
				return new Point(point.X, point.Y);
			}

			// Token: 0x04000043 RID: 67
			public int X;

			// Token: 0x04000044 RID: 68
			public int Y;
		}

		// Token: 0x02000013 RID: 19
		public struct RECT
		{
			// Token: 0x060000AE RID: 174 RVA: 0x000061DE File Offset: 0x000043DE
			public static implicit operator Rectangle(Imports.RECT rect)
			{
				return new Rectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
			}

			// Token: 0x04000045 RID: 69
			public int Left;

			// Token: 0x04000046 RID: 70
			public int Top;

			// Token: 0x04000047 RID: 71
			public int Right;

			// Token: 0x04000048 RID: 72
			public int Bottom;
		}

		// Token: 0x02000014 RID: 20
		public struct SYSTEMTIME
		{
			// Token: 0x060000AF RID: 175 RVA: 0x00006200 File Offset: 0x00004400
			public static Imports.SYSTEMTIME FromDateTime(DateTime time)
			{
				Imports.SYSTEMTIME result;
				result.wYear = (ushort)time.Year;
				result.wMonth = (ushort)time.Month;
				result.wDayOfWeek = (ushort)time.DayOfWeek;
				result.wDay = (ushort)time.Day;
				result.wHour = (ushort)time.Hour;
				result.wMinute = (ushort)time.Minute;
				result.wSecond = (ushort)time.Second;
				result.wMilliseconds = (ushort)time.Millisecond;
				return result;
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00006286 File Offset: 0x00004486
			public DateTime ToDateTime()
			{
				return new DateTime((int)this.wYear, (int)this.wMonth, (int)this.wDay, (int)this.wHour, (int)this.wMinute, (int)this.wSecond, (int)this.wMilliseconds);
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x000062B7 File Offset: 0x000044B7
			public static DateTime ToDateTime(Imports.SYSTEMTIME time)
			{
				return time.ToDateTime();
			}

			// Token: 0x04000049 RID: 73
			public ushort wYear;

			// Token: 0x0400004A RID: 74
			public ushort wMonth;

			// Token: 0x0400004B RID: 75
			public ushort wDayOfWeek;

			// Token: 0x0400004C RID: 76
			public ushort wDay;

			// Token: 0x0400004D RID: 77
			public ushort wHour;

			// Token: 0x0400004E RID: 78
			public ushort wMinute;

			// Token: 0x0400004F RID: 79
			public ushort wSecond;

			// Token: 0x04000050 RID: 80
			public ushort wMilliseconds;
		}

		// Token: 0x02000015 RID: 21
		public enum TernaryRasterOperations : uint
		{
			// Token: 0x04000052 RID: 82
			SRCCOPY = 13369376U,
			// Token: 0x04000053 RID: 83
			SRCPAINT = 15597702U,
			// Token: 0x04000054 RID: 84
			SRCAND = 8913094U,
			// Token: 0x04000055 RID: 85
			SRCINVERT = 6684742U,
			// Token: 0x04000056 RID: 86
			SRCERASE = 4457256U,
			// Token: 0x04000057 RID: 87
			NOTSRCCOPY = 3342344U,
			// Token: 0x04000058 RID: 88
			NOTSRCERASE = 1114278U,
			// Token: 0x04000059 RID: 89
			MERGECOPY = 12583114U,
			// Token: 0x0400005A RID: 90
			MERGEPAINT = 12255782U,
			// Token: 0x0400005B RID: 91
			DSTINVERT = 5570569U
		}

		// Token: 0x02000016 RID: 22
		[Flags]
		public enum ShowWindowFlags
		{
			// Token: 0x0400005D RID: 93
			SW_FORCEMINIMIZE = 11,
			// Token: 0x0400005E RID: 94
			SW_HIDE = 0,
			// Token: 0x0400005F RID: 95
			SW_MAXIMIZE = 3,
			// Token: 0x04000060 RID: 96
			SW_MINIMIZE = 6,
			// Token: 0x04000061 RID: 97
			SW_RESTORE = 9,
			// Token: 0x04000062 RID: 98
			SW_SHOW = 5,
			// Token: 0x04000063 RID: 99
			SW_SHOWDEFAULT = 10,
			// Token: 0x04000064 RID: 100
			SW_SHOWMAXIMIZED = 3,
			// Token: 0x04000065 RID: 101
			SW_SHOWMINIMIZED = 2,
			// Token: 0x04000066 RID: 102
			SW_SHOWMINNOACTIVE = 7,
			// Token: 0x04000067 RID: 103
			SW_SHOWNA = 8,
			// Token: 0x04000068 RID: 104
			SW_SHOWNOACTIVATE = 4,
			// Token: 0x04000069 RID: 105
			SW_SHOWNORMAL = 1
		}

		// Token: 0x02000017 RID: 23
		public enum SpecialWindowHandles
		{
			// Token: 0x0400006B RID: 107
			HWND_TOP,
			// Token: 0x0400006C RID: 108
			HWND_BOTTOM,
			// Token: 0x0400006D RID: 109
			HWND_TOPMOST = -1,
			// Token: 0x0400006E RID: 110
			HWND_NOTOPMOST = -2
		}

		// Token: 0x02000018 RID: 24
		[Flags]
		public enum SetWindowPosFlags : uint
		{
			// Token: 0x04000070 RID: 112
			SWP_ASYNCWINDOWPOS = 16384U,
			// Token: 0x04000071 RID: 113
			SWP_DEFERERASE = 8192U,
			// Token: 0x04000072 RID: 114
			SWP_DRAWFRAME = 32U,
			// Token: 0x04000073 RID: 115
			SWP_FRAMECHANGED = 32U,
			// Token: 0x04000074 RID: 116
			SWP_HIDEWINDOW = 128U,
			// Token: 0x04000075 RID: 117
			SWP_NOACTIVATE = 16U,
			// Token: 0x04000076 RID: 118
			SWP_NOCOPYBITS = 256U,
			// Token: 0x04000077 RID: 119
			SWP_NOMOVE = 2U,
			// Token: 0x04000078 RID: 120
			SWP_NOOWNERZORDER = 512U,
			// Token: 0x04000079 RID: 121
			SWP_NOREDRAW = 8U,
			// Token: 0x0400007A RID: 122
			SWP_NOREPOSITION = 512U,
			// Token: 0x0400007B RID: 123
			SWP_NOSENDCHANGING = 1024U,
			// Token: 0x0400007C RID: 124
			SWP_NOSIZE = 1U,
			// Token: 0x0400007D RID: 125
			SWP_NOZORDER = 4U,
			// Token: 0x0400007E RID: 126
			SWP_SHOWWINDOW = 64U
		}

		// Token: 0x02000019 RID: 25
		public enum WindowMessages
		{
			// Token: 0x04000080 RID: 128
			NULL,
			// Token: 0x04000081 RID: 129
			CREATE,
			// Token: 0x04000082 RID: 130
			DESTROY,
			// Token: 0x04000083 RID: 131
			MOVE,
			// Token: 0x04000084 RID: 132
			SIZE = 5,
			// Token: 0x04000085 RID: 133
			ACTIVATE,
			// Token: 0x04000086 RID: 134
			SETFOCUS,
			// Token: 0x04000087 RID: 135
			KILLFOCUS,
			// Token: 0x04000088 RID: 136
			ENABLE = 10,
			// Token: 0x04000089 RID: 137
			SETREDRAW,
			// Token: 0x0400008A RID: 138
			SETTEXT,
			// Token: 0x0400008B RID: 139
			GETTEXT,
			// Token: 0x0400008C RID: 140
			GETTEXTLENGTH,
			// Token: 0x0400008D RID: 141
			PAINT,
			// Token: 0x0400008E RID: 142
			CLOSE,
			// Token: 0x0400008F RID: 143
			QUERYENDSESSION,
			// Token: 0x04000090 RID: 144
			QUERYOPEN = 19,
			// Token: 0x04000091 RID: 145
			ENDSESSION = 22,
			// Token: 0x04000092 RID: 146
			QUIT = 18,
			// Token: 0x04000093 RID: 147
			ERASEBKGND = 20,
			// Token: 0x04000094 RID: 148
			SYSCOLORCHANGE,
			// Token: 0x04000095 RID: 149
			SHOWWINDOW = 24,
			// Token: 0x04000096 RID: 150
			WININICHANGE = 26,
			// Token: 0x04000097 RID: 151
			SETTINGCHANGE = 26,
			// Token: 0x04000098 RID: 152
			DEVMODECHANGE,
			// Token: 0x04000099 RID: 153
			ACTIVATEAPP,
			// Token: 0x0400009A RID: 154
			FONTCHANGE,
			// Token: 0x0400009B RID: 155
			TIMECHANGE,
			// Token: 0x0400009C RID: 156
			CANCELMODE,
			// Token: 0x0400009D RID: 157
			SETCURSOR,
			// Token: 0x0400009E RID: 158
			MOUSEACTIVATE,
			// Token: 0x0400009F RID: 159
			CHILDACTIVATE,
			// Token: 0x040000A0 RID: 160
			QUEUESYNC,
			// Token: 0x040000A1 RID: 161
			GETMINMAXINFO,
			// Token: 0x040000A2 RID: 162
			PAINTICON = 38,
			// Token: 0x040000A3 RID: 163
			ICONERASEBKGND,
			// Token: 0x040000A4 RID: 164
			NEXTDLGCTL,
			// Token: 0x040000A5 RID: 165
			SPOOLERSTATUS = 42,
			// Token: 0x040000A6 RID: 166
			DRAWITEM,
			// Token: 0x040000A7 RID: 167
			MEASUREITEM,
			// Token: 0x040000A8 RID: 168
			DELETEITEM,
			// Token: 0x040000A9 RID: 169
			VKEYTOITEM,
			// Token: 0x040000AA RID: 170
			CHARTOITEM,
			// Token: 0x040000AB RID: 171
			SETFONT,
			// Token: 0x040000AC RID: 172
			GETFONT,
			// Token: 0x040000AD RID: 173
			SETHOTKEY,
			// Token: 0x040000AE RID: 174
			GETHOTKEY,
			// Token: 0x040000AF RID: 175
			QUERYDRAGICON = 55,
			// Token: 0x040000B0 RID: 176
			COMPAREITEM = 57,
			// Token: 0x040000B1 RID: 177
			GETOBJECT = 61,
			// Token: 0x040000B2 RID: 178
			COMPACTING = 65,
			// Token: 0x040000B3 RID: 179
			[Obsolete]
			COMMNOTIFY = 68,
			// Token: 0x040000B4 RID: 180
			WINDOWPOSCHANGING = 70,
			// Token: 0x040000B5 RID: 181
			WINDOWPOSCHANGED,
			// Token: 0x040000B6 RID: 182
			[Obsolete]
			POWER,
			// Token: 0x040000B7 RID: 183
			COPYDATA = 74,
			// Token: 0x040000B8 RID: 184
			CANCELJOURNAL,
			// Token: 0x040000B9 RID: 185
			NOTIFY = 78,
			// Token: 0x040000BA RID: 186
			INPUTLANGCHANGEREQUEST = 80,
			// Token: 0x040000BB RID: 187
			INPUTLANGCHANGE,
			// Token: 0x040000BC RID: 188
			TCARD,
			// Token: 0x040000BD RID: 189
			HELP,
			// Token: 0x040000BE RID: 190
			USERCHANGED,
			// Token: 0x040000BF RID: 191
			NOTIFYFORMAT,
			// Token: 0x040000C0 RID: 192
			CONTEXTMENU = 123,
			// Token: 0x040000C1 RID: 193
			STYLECHANGING,
			// Token: 0x040000C2 RID: 194
			STYLECHANGED,
			// Token: 0x040000C3 RID: 195
			DISPLAYCHANGE,
			// Token: 0x040000C4 RID: 196
			GETICON,
			// Token: 0x040000C5 RID: 197
			SETICON,
			// Token: 0x040000C6 RID: 198
			NCCREATE,
			// Token: 0x040000C7 RID: 199
			NCDESTROY,
			// Token: 0x040000C8 RID: 200
			NCCALCSIZE,
			// Token: 0x040000C9 RID: 201
			NCHITTEST,
			// Token: 0x040000CA RID: 202
			NCPAINT,
			// Token: 0x040000CB RID: 203
			NCACTIVATE,
			// Token: 0x040000CC RID: 204
			GETDLGCODE,
			// Token: 0x040000CD RID: 205
			SYNCPAINT,
			// Token: 0x040000CE RID: 206
			NCMOUSEMOVE = 160,
			// Token: 0x040000CF RID: 207
			NCLBUTTONDOWN,
			// Token: 0x040000D0 RID: 208
			NCLBUTTONUP,
			// Token: 0x040000D1 RID: 209
			NCLBUTTONDBLCLK,
			// Token: 0x040000D2 RID: 210
			NCRBUTTONDOWN,
			// Token: 0x040000D3 RID: 211
			NCRBUTTONUP,
			// Token: 0x040000D4 RID: 212
			NCRBUTTONDBLCLK,
			// Token: 0x040000D5 RID: 213
			NCMBUTTONDOWN,
			// Token: 0x040000D6 RID: 214
			NCMBUTTONUP,
			// Token: 0x040000D7 RID: 215
			NCMBUTTONDBLCLK,
			// Token: 0x040000D8 RID: 216
			NCXBUTTONDOWN = 171,
			// Token: 0x040000D9 RID: 217
			NCXBUTTONUP,
			// Token: 0x040000DA RID: 218
			NCXBUTTONDBLCLK,
			// Token: 0x040000DB RID: 219
			INPUT_DEVICE_CHANGE = 254,
			// Token: 0x040000DC RID: 220
			INPUT,
			// Token: 0x040000DD RID: 221
			KEYFIRST,
			// Token: 0x040000DE RID: 222
			KEYDOWN = 256,
			// Token: 0x040000DF RID: 223
			KEYUP,
			// Token: 0x040000E0 RID: 224
			CHAR,
			// Token: 0x040000E1 RID: 225
			DEADCHAR,
			// Token: 0x040000E2 RID: 226
			SYSKEYDOWN,
			// Token: 0x040000E3 RID: 227
			SYSKEYUP,
			// Token: 0x040000E4 RID: 228
			SYSCHAR,
			// Token: 0x040000E5 RID: 229
			SYSDEADCHAR,
			// Token: 0x040000E6 RID: 230
			KEYLAST,
			// Token: 0x040000E7 RID: 231
			UNICHAR,
			// Token: 0x040000E8 RID: 232
			IME_STARTCOMPOSITION = 269,
			// Token: 0x040000E9 RID: 233
			IME_ENDCOMPOSITION,
			// Token: 0x040000EA RID: 234
			IME_COMPOSITION,
			// Token: 0x040000EB RID: 235
			IME_KEYLAST = 271,
			// Token: 0x040000EC RID: 236
			INITDIALOG,
			// Token: 0x040000ED RID: 237
			COMMAND,
			// Token: 0x040000EE RID: 238
			SYSCOMMAND,
			// Token: 0x040000EF RID: 239
			TIMER,
			// Token: 0x040000F0 RID: 240
			HSCROLL,
			// Token: 0x040000F1 RID: 241
			VSCROLL,
			// Token: 0x040000F2 RID: 242
			INITMENU,
			// Token: 0x040000F3 RID: 243
			INITMENUPOPUP,
			// Token: 0x040000F4 RID: 244
			MENUSELECT = 287,
			// Token: 0x040000F5 RID: 245
			MENUCHAR,
			// Token: 0x040000F6 RID: 246
			ENTERIDLE,
			// Token: 0x040000F7 RID: 247
			MENURBUTTONUP,
			// Token: 0x040000F8 RID: 248
			MENUDRAG,
			// Token: 0x040000F9 RID: 249
			MENUGETOBJECT,
			// Token: 0x040000FA RID: 250
			UNINITMENUPOPUP,
			// Token: 0x040000FB RID: 251
			MENUCOMMAND,
			// Token: 0x040000FC RID: 252
			CHANGEUISTATE,
			// Token: 0x040000FD RID: 253
			UPDATEUISTATE,
			// Token: 0x040000FE RID: 254
			QUERYUISTATE,
			// Token: 0x040000FF RID: 255
			CTLCOLORMSGBOX = 306,
			// Token: 0x04000100 RID: 256
			CTLCOLOREDIT,
			// Token: 0x04000101 RID: 257
			CTLCOLORLISTBOX,
			// Token: 0x04000102 RID: 258
			CTLCOLORBTN,
			// Token: 0x04000103 RID: 259
			CTLCOLORDLG,
			// Token: 0x04000104 RID: 260
			CTLCOLORSCROLLBAR,
			// Token: 0x04000105 RID: 261
			CTLCOLORSTATIC,
			// Token: 0x04000106 RID: 262
			MOUSEFIRST = 512,
			// Token: 0x04000107 RID: 263
			MOUSEMOVE = 512,
			// Token: 0x04000108 RID: 264
			LBUTTONDOWN,
			// Token: 0x04000109 RID: 265
			LBUTTONUP,
			// Token: 0x0400010A RID: 266
			LBUTTONDBLCLK,
			// Token: 0x0400010B RID: 267
			RBUTTONDOWN,
			// Token: 0x0400010C RID: 268
			RBUTTONUP,
			// Token: 0x0400010D RID: 269
			RBUTTONDBLCLK,
			// Token: 0x0400010E RID: 270
			MBUTTONDOWN,
			// Token: 0x0400010F RID: 271
			MBUTTONUP,
			// Token: 0x04000110 RID: 272
			MBUTTONDBLCLK,
			// Token: 0x04000111 RID: 273
			MOUSEWHEEL,
			// Token: 0x04000112 RID: 274
			XBUTTONDOWN,
			// Token: 0x04000113 RID: 275
			XBUTTONUP,
			// Token: 0x04000114 RID: 276
			XBUTTONDBLCLK,
			// Token: 0x04000115 RID: 277
			MOUSEHWHEEL,
			// Token: 0x04000116 RID: 278
			MOUSELAST = 526,
			// Token: 0x04000117 RID: 279
			PARENTNOTIFY = 528,
			// Token: 0x04000118 RID: 280
			ENTERMENULOOP,
			// Token: 0x04000119 RID: 281
			EXITMENULOOP,
			// Token: 0x0400011A RID: 282
			NEXTMENU,
			// Token: 0x0400011B RID: 283
			SIZING,
			// Token: 0x0400011C RID: 284
			CAPTURECHANGED,
			// Token: 0x0400011D RID: 285
			MOVING,
			// Token: 0x0400011E RID: 286
			POWERBROADCAST = 536,
			// Token: 0x0400011F RID: 287
			DEVICECHANGE,
			// Token: 0x04000120 RID: 288
			MDICREATE = 544,
			// Token: 0x04000121 RID: 289
			MDIDESTROY,
			// Token: 0x04000122 RID: 290
			MDIACTIVATE,
			// Token: 0x04000123 RID: 291
			MDIRESTORE,
			// Token: 0x04000124 RID: 292
			MDINEXT,
			// Token: 0x04000125 RID: 293
			MDIMAXIMIZE,
			// Token: 0x04000126 RID: 294
			MDITILE,
			// Token: 0x04000127 RID: 295
			MDICASCADE,
			// Token: 0x04000128 RID: 296
			MDIICONARRANGE,
			// Token: 0x04000129 RID: 297
			MDIGETACTIVE,
			// Token: 0x0400012A RID: 298
			MDISETMENU = 560,
			// Token: 0x0400012B RID: 299
			ENTERSIZEMOVE,
			// Token: 0x0400012C RID: 300
			EXITSIZEMOVE,
			// Token: 0x0400012D RID: 301
			DROPFILES,
			// Token: 0x0400012E RID: 302
			MDIREFRESHMENU,
			// Token: 0x0400012F RID: 303
			IME_SETCONTEXT = 641,
			// Token: 0x04000130 RID: 304
			IME_NOTIFY,
			// Token: 0x04000131 RID: 305
			IME_CONTROL,
			// Token: 0x04000132 RID: 306
			IME_COMPOSITIONFULL,
			// Token: 0x04000133 RID: 307
			IME_SELECT,
			// Token: 0x04000134 RID: 308
			IME_CHAR,
			// Token: 0x04000135 RID: 309
			IME_REQUEST = 648,
			// Token: 0x04000136 RID: 310
			IME_KEYDOWN = 656,
			// Token: 0x04000137 RID: 311
			IME_KEYUP,
			// Token: 0x04000138 RID: 312
			MOUSEHOVER = 673,
			// Token: 0x04000139 RID: 313
			MOUSELEAVE = 675,
			// Token: 0x0400013A RID: 314
			NCMOUSEHOVER = 672,
			// Token: 0x0400013B RID: 315
			NCMOUSELEAVE = 674,
			// Token: 0x0400013C RID: 316
			WTSSESSION_CHANGE = 689,
			// Token: 0x0400013D RID: 317
			TABLET_FIRST = 704,
			// Token: 0x0400013E RID: 318
			TABLET_LAST = 735,
			// Token: 0x0400013F RID: 319
			CUT = 768,
			// Token: 0x04000140 RID: 320
			COPY,
			// Token: 0x04000141 RID: 321
			PASTE,
			// Token: 0x04000142 RID: 322
			CLEAR,
			// Token: 0x04000143 RID: 323
			UNDO,
			// Token: 0x04000144 RID: 324
			RENDERFORMAT,
			// Token: 0x04000145 RID: 325
			RENDERALLFORMATS,
			// Token: 0x04000146 RID: 326
			DESTROYCLIPBOARD,
			// Token: 0x04000147 RID: 327
			DRAWCLIPBOARD,
			// Token: 0x04000148 RID: 328
			PAINTCLIPBOARD,
			// Token: 0x04000149 RID: 329
			VSCROLLCLIPBOARD,
			// Token: 0x0400014A RID: 330
			SIZECLIPBOARD,
			// Token: 0x0400014B RID: 331
			ASKCBFORMATNAME,
			// Token: 0x0400014C RID: 332
			CHANGECBCHAIN,
			// Token: 0x0400014D RID: 333
			HSCROLLCLIPBOARD,
			// Token: 0x0400014E RID: 334
			QUERYNEWPALETTE,
			// Token: 0x0400014F RID: 335
			PALETTEISCHANGING,
			// Token: 0x04000150 RID: 336
			PALETTECHANGED,
			// Token: 0x04000151 RID: 337
			HOTKEY,
			// Token: 0x04000152 RID: 338
			PRINT = 791,
			// Token: 0x04000153 RID: 339
			PRINTCLIENT,
			// Token: 0x04000154 RID: 340
			APPCOMMAND,
			// Token: 0x04000155 RID: 341
			THEMECHANGED,
			// Token: 0x04000156 RID: 342
			CLIPBOARDUPDATE = 797,
			// Token: 0x04000157 RID: 343
			DWMCOMPOSITIONCHANGED,
			// Token: 0x04000158 RID: 344
			DWMNCRENDERINGCHANGED,
			// Token: 0x04000159 RID: 345
			DWMCOLORIZATIONCOLORCHANGED,
			// Token: 0x0400015A RID: 346
			DWMWINDOWMAXIMIZEDCHANGE,
			// Token: 0x0400015B RID: 347
			GETTITLEBARINFOEX = 831,
			// Token: 0x0400015C RID: 348
			HANDHELDFIRST = 856,
			// Token: 0x0400015D RID: 349
			HANDHELDLAST = 863,
			// Token: 0x0400015E RID: 350
			AFXFIRST,
			// Token: 0x0400015F RID: 351
			AFXLAST = 895,
			// Token: 0x04000160 RID: 352
			PENWINFIRST,
			// Token: 0x04000161 RID: 353
			PENWINLAST = 911,
			// Token: 0x04000162 RID: 354
			APP = 32768,
			// Token: 0x04000163 RID: 355
			USER = 1024,
			// Token: 0x04000164 RID: 356
			CPL_LAUNCH = 5120,
			// Token: 0x04000165 RID: 357
			CPL_LAUNCHED,
			// Token: 0x04000166 RID: 358
			SYSTIMER = 280,
			// Token: 0x04000167 RID: 359
			HSHELL_ACCESSIBILITYSTATE = 11,
			// Token: 0x04000168 RID: 360
			HSHELL_ACTIVATESHELLWINDOW = 3,
			// Token: 0x04000169 RID: 361
			HSHELL_APPCOMMAND = 12,
			// Token: 0x0400016A RID: 362
			HSHELL_GETMINRECT = 5,
			// Token: 0x0400016B RID: 363
			HSHELL_LANGUAGE = 8,
			// Token: 0x0400016C RID: 364
			HSHELL_REDRAW = 6,
			// Token: 0x0400016D RID: 365
			HSHELL_TASKMAN,
			// Token: 0x0400016E RID: 366
			HSHELL_WINDOWCREATED = 1,
			// Token: 0x0400016F RID: 367
			HSHELL_WINDOWDESTROYED,
			// Token: 0x04000170 RID: 368
			HSHELL_WINDOWACTIVATED = 4,
			// Token: 0x04000171 RID: 369
			HSHELL_WINDOWREPLACED = 13
		}
	}
}
