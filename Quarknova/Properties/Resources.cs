using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Quarknova.Properties
{
	// Token: 0x02000007 RID: 7
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class Resources
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000036A2 File Offset: 0x000018A2
		internal Resources()
		{
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000036AA File Offset: 0x000018AA
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Quarknova.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000036D6 File Offset: 0x000018D6
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000036DD File Offset: 0x000018DD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000036E5 File Offset: 0x000018E5
		public static byte[] Microsoft_Win32_TaskScheduler
		{
			get
			{
				return (byte[])Resources.ResourceManager.GetObject("Microsoft_Win32_TaskScheduler", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003700 File Offset: 0x00001900
		public static byte[] NAudio
		{
			get
			{
				return (byte[])Resources.ResourceManager.GetObject("NAudio", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000371B File Offset: 0x0000191B
		public static Icon realtek
		{
			get
			{
				return (Icon)Resources.ResourceManager.GetObject("realtek", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003736 File Offset: 0x00001936
		public static byte[] rgyy
		{
			get
			{
				return (byte[])Resources.ResourceManager.GetObject("rgyy", Resources.resourceCulture);
			}
		}

		// Token: 0x04000018 RID: 24
		private static ResourceManager resourceMan;

		// Token: 0x04000019 RID: 25
		private static CultureInfo resourceCulture;
	}
}
