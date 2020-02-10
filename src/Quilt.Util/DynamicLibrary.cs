namespace Quilt.Util {
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;

	public class DynamicLibrary {
		private delegate IntPtr LoadLibrary(string name);
		private delegate IntPtr GetProcAddress(IntPtr library, string name);

		private static readonly LoadLibrary __loadLibrary;
		private static readonly GetProcAddress __getProcAddress;

		private readonly IntPtr _library;

		static DynamicLibrary() {
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				__loadLibrary = Kernel32.LoadLibrary;
				__getProcAddress = Kernel32.GetProcAddress;
			}
		}

		private DynamicLibrary(IntPtr library) {
			_library = library;
		}

		public TFunc GetFunction<TFunc>(string name) where TFunc : Delegate {
			return (TFunc)GetFunction(typeof(TFunc), name);
		}

		public Delegate GetFunction(Type delegateType, string name) {
			return Marshal.GetDelegateForFunctionPointer(__getProcAddress(_library, name), delegateType);
		}


		public static DynamicLibrary Load(string name) {
			IntPtr library = __loadLibrary(name);

			if (library == IntPtr.Zero) {
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			return new DynamicLibrary(library);
		}

		private static class Kernel32 {
			[DllImport("Kernel32.dll", SetLastError = true)]
			public static extern IntPtr LoadLibrary(string name);

			[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
			public static extern IntPtr GetProcAddress(IntPtr library, string name);
		}
	}
}
