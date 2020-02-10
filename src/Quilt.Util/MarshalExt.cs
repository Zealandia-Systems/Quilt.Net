namespace Quilt.Util {
	using System;
	using System.Diagnostics;
	using System.Runtime.InteropServices;
	using System.Text;

	public static class MarshalExt {
		public unsafe static IntPtr ToUTF8(string input) {
			fixed (char* pInput = input) {
				var length = Encoding.UTF8.GetByteCount(pInput, input.Length);
				var pResult = (byte*)Marshal.AllocHGlobal(length + 1).ToPointer();
				var bytesWritten = Encoding.UTF8.GetBytes(pInput, input.Length, pResult, length);

				Trace.Assert(bytesWritten == length);

				pResult[length] = 0;

				return (IntPtr)pResult;
			}
		}

		public static byte[] ToUTF8ByteArray(string input) {
			return input != null ? Encoding.UTF8.GetBytes(input) : null;
		}

		public unsafe static string FromUTF8(IntPtr ptr) => FromUTF8((byte*)ptr);
		public unsafe static string FromUTF8(byte* ptr) {
			var length = 0;

			while (ptr[length] != 0) {
				length++;
			}

			return Encoding.UTF8.GetString(ptr, length);
		}
	}
}
