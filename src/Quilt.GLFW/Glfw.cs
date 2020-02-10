namespace Quilt.GLFW {
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using System.Security;

	using Quilt.Util;

	public partial class Glfw {
		private const string LIBRARY = "glfw3";

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public delegate void ErrorCallback(ErrorCode error, string description);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MonitorCallback(Monitor monitor, ConnectionEvent @event);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WindowPosCallback(Window window, int x, int y);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WindowSizeCallback(Window window, int width, int height);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WindowCloseCallback(Window window);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WindowRefreshCallback(Window window);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WindowFocusCallback(Window window, [MarshalAs(UnmanagedType.Bool)] bool focused);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void WindowIconifyCallback(Window window, [MarshalAs(UnmanagedType.Bool)] bool focused);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void FramebufferSizeCallback(Window window, int width, int height);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwInit"), SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool Init();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwInit"), SuppressUnmanagedCodeSecurity]
		public static extern void Terminate();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVersion"), SuppressUnmanagedCodeSecurity]
		public static extern void GetVersion(out int major, out int minor, out int revision);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwGetVersionString();

		public static unsafe string GetVersionString() {
			return MarshalExt.FromUTF8(glfwGetVersionString());
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetErrorCallback(IntPtr callback);

		public static void SetErrorCallback(ErrorCallback callback) {
			glfwSetErrorCallback(Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern unsafe IntPtr glfwGetMonitors(int* count);

		public static unsafe Monitor[] GetMonitors() {
			int count;
			var array = glfwGetMonitors(&count);

			if (count == 0) {
				return null;
			}

			var monitors = new Monitor[count];
			int size = Marshal.SizeOf<IntPtr>();

			for (int i = 0; i < count; i++) {
				var handle = Marshal.ReadIntPtr(array, i * size);

				monitors[i] = new Monitor(handle);
			}

			return monitors;
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetPrimaryMonitor"), SuppressUnmanagedCodeSecurity]
		
		public static extern Monitor GetPrimaryMonitor();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorPos"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMonitorPos(Monitor monitor, out int xpos, out int ypos);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorPhysicalSize"), SuppressUnmanagedCodeSecurity]
		public static extern void GetMonitorPhysicalSize(Monitor monitor, out int widthMM, out int heightMM);


		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwGetMonitorName(Monitor monitor);

		public static string GetMonitorName(Monitor monitor) {
			return MarshalExt.FromUTF8(glfwGetMonitorName(monitor));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetMonitorCallback(IntPtr callback);

		public static void SetMonitorCallback(MonitorCallback callback) {
			glfwSetMonitorCallback(Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern unsafe VideoMode* glfwGetVideoModes(Monitor monitor, out int count);

		public static unsafe VideoMode[] GetVideoModes(Monitor monitor) {
			var array = glfwGetVideoModes(monitor, out var count);

			if (count == 0) {
				return null;
			}

			var result = new VideoMode[count];

			for (int i = 0; i < count; i++) {
				result[i] = array[i];
			}

			return result;
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVideoMode"), SuppressUnmanagedCodeSecurity]
		private static extern unsafe IntPtr glfwGetVideoMode(Monitor monitor);

		public static unsafe VideoMode GetVideoMode(Monitor monitor) {
			return Marshal.PtrToStructure<VideoMode>(glfwGetVideoMode(monitor));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwDefaultWindowHints"), SuppressUnmanagedCodeSecurity]
		public static extern void DefaultWindowHints();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowHint"), SuppressUnmanagedCodeSecurity]
		public static extern void WindowHint(Hint target, int hint);

		public static void WindowHint(Hint hint, bool value) {
			WindowHint(hint, value ? 1 : 0);
		}

		public static void WindowHint(Hint hint, Enum value) {
			WindowHint(hint, Convert.ToInt32(value));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		
		private static extern Window glfwCreateWindow(int width, int height, byte[] title, IntPtr monitor, IntPtr share);

		public static Window CreateWindow(int width, int height, string title, Monitor? monitor = null, Window? share = null) {
			return glfwCreateWindow(width, height, MarshalExt.ToUTF8ByteArray(title), monitor.HasValue ? monitor.Value.Handle : IntPtr.Zero, share.HasValue ? share.Value.Handle : IntPtr.Zero);
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwDestroyWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void DestroyWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowShouldClose"), SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool WindowShouldClose(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowShouldClose"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowShouldClose(Window window, [MarshalAs(UnmanagedType.Bool)] bool value);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern void glfwSetWindowTitle(IntPtr window, byte[] title);

		public static void SetWindowTitle(Window window, string title) {
			glfwSetWindowTitle(window.Handle, MarshalExt.ToUTF8ByteArray(title));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern void glfwSetWindowIcon(Window window, int count, IntPtr images);

		public static unsafe void SetWindowIcon(Window window, Image[] images) {
			if (images == null) {
				glfwSetWindowIcon(window, 0, IntPtr.Zero);

				return;
			}

			var handles = new List<GCHandle>();
			var imgs = new InternalImage[images.Length];

			for (int i = 0; i < imgs.Length; i++) {
				var handle = GCHandle.Alloc(images[i].Pixels, GCHandleType.Pinned);

				handles.Add(handle);

				imgs[i] = new InternalImage {
					Width = images[i].Width,
					Height = images[i].Width,
					Pixels = handle.AddrOfPinnedObject()
				};
			}

			fixed (InternalImage* array = imgs) {
				var ptr = new IntPtr((void*)array);

				glfwSetWindowIcon(window, images.Length, ptr);
			}

			foreach (var handle in handles) {
				handle.Free();
			}
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowPos"), SuppressUnmanagedCodeSecurity]
		public static extern unsafe void GetWindowPos(Window window, out int x, out int y);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowPos"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowPos(Window window, int x, int y);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowSize"), SuppressUnmanagedCodeSecurity]
		public static extern unsafe void GetWindowSize(Window window, out int width, out int height);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowSizeLimits"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowSizeLimits(Window window, int minwidth, int minheight, int maxwidth, int maxheight);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowAspectRatio"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowAspectRatio(Window window, int numerator, int denominator);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowSize"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowSize(Window window, int width, int height);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetFramebufferSize"), SuppressUnmanagedCodeSecurity]
		public static extern unsafe void GetFramebufferSize(Window window, out int width, out int height);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowFrameSize"), SuppressUnmanagedCodeSecurity]
		private static extern unsafe void GetWindowFrameSize(IntPtr window, out int left, out int top, out int right, out int bottom);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwIconifyWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void IconifyWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwRestoreWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void RestoreWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwMaximizeWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void MaximizeWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwShowWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void ShowWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwHideWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void HideWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwFocusWindow"), SuppressUnmanagedCodeSecurity]
		public static extern void FocusWindow(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowMonitor"), SuppressUnmanagedCodeSecurity]
		
		public static extern Monitor GetWindowMonitor(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowMonitor"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowMonitor(Window window, Monitor monitor, int xpos, int ypos, int width, int height, int refreshRate);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowAttrib"), SuppressUnmanagedCodeSecurity]
		public static extern int GetWindowAttrib(Window window, [MarshalAs(UnmanagedType.I4)] WindowAttribute attribibute);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowUserPointer"), SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowUserPointer(Window window, IntPtr ptr);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetWindowUserPointer"), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetWindowUserPointer(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetWindowPosCallback(Window window, IntPtr callback);

		public static void SetWindowPosCallback(Window window, WindowPosCallback callback) {
			glfwSetWindowPosCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetWindowSizeCallback(Window window, IntPtr callback);

		public static void SetWindowSizeCallback(Window window, WindowSizeCallback callback) {
			glfwSetWindowSizeCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetWindowCloseCallback(Window window, IntPtr callback);

		public static void SetWindowCloseCallback(Window window, WindowCloseCallback callback) {
			glfwSetWindowCloseCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetWindowRefreshCallback(Window window, IntPtr callback);

		public static void SetWindowRefreshCallback(Window window, WindowRefreshCallback callback) {
			glfwSetWindowRefreshCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetWindowFocusCallback(Window window, IntPtr callback);

		public static void SetWindowFocusCallback(Window window, WindowFocusCallback callback) {
			glfwSetWindowFocusCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetWindowIconifyCallback(Window window, IntPtr callback);

		public static void SetWindowIconifyCallback(Window window, WindowIconifyCallback callback) {
			glfwSetWindowIconifyCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwSetFramebufferSizeCallback(Window window, IntPtr callback);

		public static void SetFramebufferSizeCallback(Window window, FramebufferSizeCallback callback) {
			glfwSetFramebufferSizeCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
		}

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwPollEvents"), SuppressUnmanagedCodeSecurity]
		public static extern void PollEvents();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWaitEvents"), SuppressUnmanagedCodeSecurity]
		public static extern void WaitEvents();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWaitEventsTimeout"), SuppressUnmanagedCodeSecurity]
		public static extern void WaitEventsTimeout(double timeout);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwPostEmptyEvent"), SuppressUnmanagedCodeSecurity]
		public static extern void PostEmptyEvent();


		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwMakeContextCurrent"), SuppressUnmanagedCodeSecurity]
		public static extern void MakeContextCurrent(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetCurrentContext"), SuppressUnmanagedCodeSecurity]
		
		public static extern Window GetCurrentContext();

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSwapBuffers"), SuppressUnmanagedCodeSecurity]
		public static extern void SwapBuffers(Window window);

		[DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSwapInterval"), SuppressUnmanagedCodeSecurity]
		public static extern void SwapInterval(int interval);
	}
}
