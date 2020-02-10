namespace Quilt.GLFW {
	/// <seealso cref="Hint.ClientApi"/>
	public enum ClientApi {
		None = 0,
		OpenGL = 0x00030001,
		OpenGLES = 0x00030002
	}

	/// <seealso cref="Hint.ContextCreationApi"/>
	public enum ContextApi {
		Native = 0x00036001,
		EGL = 0x00036002
	}

	/// <seealso cref="Hint.ContextReleaseBehavior"/>
	public enum ContextReleaseBehavior {
		Any = 0,
		Flush = 0x00035001,
		None = 0x00035002
	}

	/// <seealso cref="Hint.ContextRobustness"/>
	public enum ContextRobustness {
		None = 0,
		NoResetNotification = 0x00031001,
		LoseContextOnReset = 0x00031002
	}

	/// <seealso cref="Hint.OpenglProfile"/>
	public enum OpenGLProfile {
		Any = 0,
		Core = 0x00032001,
		Compat = 0x00032002
	}

	public enum Hint {
		Focused = 0x00020001,
		Resizable = 0x00020003,
		Visible = 0x00020004,
		Decorated = 0x00020005,
		AutoIconify = 0x00020006,
		Floating = 0x00020007,
		Maximized = 0x00020008,
		RedBits = 0x00021001,
		GreenBits = 0x00021002,
		BlueBits = 0x00021003,
		AlphaBits = 0x00021004,
		DepthBits = 0x00021005,
		StencilBits = 0x00021006,
		AccumRedBits = 0x00021007,
		AccumGreenBits = 0x00021008,
		AccumBlueBits = 0x00021009,
		AccumAlphaBits = 0x0002100a,
		AuxBuffers = 0x0002100b,
		Stereo = 0x0002100c,
		Samples = 0x0002100d,
		sRGBCapable = 0x0002100e,
		Doublebuffer = 0x00021010,
		RefreshRate = 0x0002100f,
		ClientApi = 0x00022001,
		ContextVersionMajor = 0x00022002,
		ContextVersionMinor = 0x00022003,
		ContextRevision = 0x00022004,
		ContextRobustness = 0x00022005,
		OpenglForwardCompat = 0x00022006,
		OpenglDebugContext = 0x00022007,
		OpenglProfile = 0x00022008,
		ContextReleaseBehavior = 0x00022009,
		ContextNoError = 0x0002200a,
		ContextCreationApi = 0x0002200b
	}
}
