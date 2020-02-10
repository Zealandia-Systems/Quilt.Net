namespace Quilt.GLFW {
	using System;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Monitor : IEquatable<Monitor> {
		public static readonly Monitor None = new Monitor(IntPtr.Zero);

		public IntPtr Handle;

		internal Monitor(IntPtr handle) {
			Handle = handle;
		}

		public override bool Equals(object obj) {
			if (obj is Monitor)
				return Equals((Monitor)obj);

			return false;
		}

		public bool Equals(Monitor obj) => Handle == obj.Handle;

		public override string ToString() => Handle.ToString();

		public override int GetHashCode() => Handle.GetHashCode();

		public static bool operator ==(Monitor a, Monitor b) => a.Equals(b);

		public static bool operator !=(Monitor a, Monitor b) => !a.Equals(b);

		public static implicit operator bool(Monitor obj) => obj.Handle != IntPtr.Zero;
	}
}
