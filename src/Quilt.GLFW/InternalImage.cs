namespace Quilt.GLFW {
	using System;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	internal struct InternalImage {
		internal int Width;
		internal int Height;
		internal IntPtr Pixels;
	}
}
