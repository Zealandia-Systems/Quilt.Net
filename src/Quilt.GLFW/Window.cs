namespace Quilt.GLFW {
	using System;
	using System.Runtime.InteropServices;

	public partial class Glfw {
		[StructLayout(LayoutKind.Sequential)]
		public struct Window : IEquatable<Window> {
			public static readonly Window None = new Window(IntPtr.Zero);

			public IntPtr Handle;

			internal Window(IntPtr handle) {
				Handle = handle;
			}

			public override bool Equals(object obj) {
				if (obj is Window)
					return Equals((Window)obj);

				return false;
			}

			public bool Equals(Window obj) => Handle == obj.Handle;

			public override string ToString() => Handle.ToString();

			public override int GetHashCode() => Handle.GetHashCode();

			public static bool operator ==(Window a, Window b) => a.Equals(b);

			public static bool operator !=(Window a, Window b) => !a.Equals(b);

			public static implicit operator bool(Window obj) => obj.Handle != IntPtr.Zero;
		}
	}
}
