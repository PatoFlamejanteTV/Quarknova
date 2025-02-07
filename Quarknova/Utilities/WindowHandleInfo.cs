using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Quarknova.Utilities
{
	// Token: 0x02000004 RID: 4
	public class WindowHandleInfo
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000033E2 File Offset: 0x000015E2
		public WindowHandleInfo(IntPtr handle)
		{
			this.MainHandle = handle;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000033F4 File Offset: 0x000015F4
		private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(lParam);
			if (gchandle.Target == null)
			{
				return false;
			}
			(gchandle.Target as List<IntPtr>).Add(hWnd);
			return true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003428 File Offset: 0x00001628
		public List<IntPtr> GetAllChildHandles()
		{
			List<IntPtr> list = new List<IntPtr>();
			GCHandle value = GCHandle.Alloc(list);
			IntPtr lParam = GCHandle.ToIntPtr(value);
			try
			{
				Imports.EnumWindowProc callback = new Imports.EnumWindowProc(this.EnumWindow);
				Imports.EnumChildWindows(this.MainHandle, callback, lParam);
			}
			finally
			{
				value.Free();
			}
			return list;
		}

		// Token: 0x04000002 RID: 2
		private IntPtr MainHandle;
	}
}
