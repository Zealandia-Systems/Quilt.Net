namespace Quilt.GLFW {
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct VideoMode {
		public int Width;

		public int Height;

		public int RedBits;

		public int GreenBits;

		public int BlueBits;

		public int RefreshRate;

		public override string ToString() {
			return $"VideoMode(Width={Width}, Height={Height}, RedBits={RedBits}, GreenBits={GreenBits}, BlueBits={BlueBits}, RefreshRate={RefreshRate})";
		}
	}
}
