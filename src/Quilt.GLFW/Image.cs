namespace Quilt.GLFW {
	using System.Runtime.InteropServices;


	[StructLayout(LayoutKind.Sequential)]
	public struct Image {
		public int Width;
		public int Height;

		[MarshalAs(UnmanagedType.LPArray)]
		public byte[] Pixels;
	}
}
