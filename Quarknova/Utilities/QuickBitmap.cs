using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Quarknova.Utilities
{
	// Token: 0x02000006 RID: 6
	internal class QuickBitmap : IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000034B3 File Offset: 0x000016B3
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000034BB File Offset: 0x000016BB
		public Bitmap Bitmap { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000034C4 File Offset: 0x000016C4
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000034CC File Offset: 0x000016CC
		public int[] Bits { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000034D5 File Offset: 0x000016D5
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000034DD File Offset: 0x000016DD
		public bool Disposed { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000034E6 File Offset: 0x000016E6
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000034EE File Offset: 0x000016EE
		public int Height { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000034F7 File Offset: 0x000016F7
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000034FF File Offset: 0x000016FF
		public int Width { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003508 File Offset: 0x00001708
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003510 File Offset: 0x00001710
		public Size Size { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003519 File Offset: 0x00001719
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003521 File Offset: 0x00001721
		private protected GCHandle BitsHandle { protected get; private set; }

		// Token: 0x0600005C RID: 92 RVA: 0x0000352C File Offset: 0x0000172C
		public QuickBitmap(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.Size = new Size(width, height);
			this.Bits = new int[width * height];
			this.BitsHandle = GCHandle.Alloc(this.Bits, GCHandleType.Pinned);
			this.Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, this.BitsHandle.AddrOfPinnedObject());
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000035A0 File Offset: 0x000017A0
		public QuickBitmap(Bitmap bmp)
		{
			this.Width = bmp.Width;
			this.Height = bmp.Height;
			this.Size = new Size(this.Width, this.Height);
			this.Bits = new int[this.Width * this.Height];
			this.BitsHandle = GCHandle.Alloc(this.Bits, GCHandleType.Pinned);
			this.Bitmap = new Bitmap(bmp);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003618 File Offset: 0x00001818
		public void SetPixel(int x, int y, Color colour)
		{
			int num = x + y * this.Width;
			int num2 = colour.ToArgb();
			this.Bits[num] = num2;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003644 File Offset: 0x00001844
		public Color GetPixel(int x, int y)
		{
			int num = x + y * this.Width;
			return Color.FromArgb(this.Bits[num]);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000366C File Offset: 0x0000186C
		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Disposed = true;
			this.Bitmap.Dispose();
			this.BitsHandle.Free();
		}
	}
}
