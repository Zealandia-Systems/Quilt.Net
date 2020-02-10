namespace Quilt {
	using System;

	using Quilt.GLFW;
	using Quilt.Xml;

	[Element(CoreNamespace.URI)]
	public class Window : QuiltElement {
		private Glfw.Window _window;
		//private Context _context;

		protected Window(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {

		}

		private void HandleFramebufferSizeCallback(Glfw.Window window, int width, int height) {
			//_fbWidth = width;
			//_fbHeight = height;
		}

		[Attribute]
		public string Title {
			get {
				return GetAttribute("Title");
			}
			set {
				SetAttribute("Title", value);
			}
		}

		[Attribute]
		public int Left {
			get {
				if (_window == Glfw.Window.None) {
					return Convert.ToInt32(Attributes["Left"]?.Value ?? "0");
				}

				Glfw.GetWindowPos(_window, out var x, out var _);

				return x;
			}
			set {
				if (_window != Glfw.Window.None) {
					Glfw.GetWindowPos(_window, out var _, out var y);
					Glfw.SetWindowPos(_window, value, y);
				}

				SetAttribute("Left", value.ToString());
			}
		}

		[Attribute]
		public int Top {
			get {
				if (_window == Glfw.Window.None) {
					return Convert.ToInt32(Attributes["Top"]?.Value ?? "0");
				}

				Glfw.GetWindowPos(_window, out var _, out var y);

				return y;
			}
			set {
				if (_window != Glfw.Window.None) {
					Glfw.GetWindowPos(_window, out var x, out var _);
					Glfw.SetWindowPos(_window, x, value);
				}

				SetAttribute("Top", value.ToString());
			}
		}

		[Attribute]
		public int Width {
			get {
				if (_window == Glfw.Window.None) {
					return Convert.ToInt32(Attributes["Width"]?.Value ?? "0");
				}

				Glfw.GetWindowSize(_window, out var width, out _);

				return width;
			}
			set {
				if (_window != Glfw.Window.None) {
					Glfw.GetWindowSize(_window, out _, out var height);
					Glfw.SetWindowSize(_window, value, height);
				}

				SetAttribute("Width", value.ToString());
			}
		}

		[Attribute]
		public int Height {
			get {
				if (_window == Glfw.Window.None) {
					return Convert.ToInt32(Attributes["Height"]?.Value ?? "0");
				}

				Glfw.GetWindowSize(_window, out _, out var height);

				return height;
			}
			set {
				if (_window != Glfw.Window.None) {
					Glfw.GetWindowSize(_window, out var width, out _);
					Glfw.SetWindowSize(_window, width, value);
				}

				SetAttribute("Height", value.ToString());
			}
		}

		public void Show() {
			_window = Glfw.CreateWindow(Width, Height, Title);

			if (_window == Glfw.Window.None) {
				throw new Exception();
			}

			Glfw.SetWindowSizeCallback(_window, HandleSizeCallback);
			Glfw.SetWindowPosCallback(_window, HandlePosCallback);
			Glfw.SetWindowRefreshCallback(_window, HandleRefreshCallback);
			Glfw.SetWindowCloseCallback(_window, HandleCloseCallback);
			Glfw.SetWindowFocusCallback(_window, HandleFocusCallback);
			Glfw.SetWindowIconifyCallback(_window, HandleIconifyCallback);
			Glfw.SetFramebufferSizeCallback(_window, HandleFramebufferSizeCallback);

			Glfw.MakeContextCurrent(_window);

			//_context = NVG.CreateGL3Glew(3);

			//_context.CreateFont("sans-bold", "Roboto-Bold.ttf");

			HandleRefreshCallback(_window);
		}

		/*protected virtual void Render(Context context) {
			const int CORNER_RADIUS = 3;

			int x = 100;
			int y = 100;
			int w = 400;
			int h = 400;

			_context.Save();
			_context.BeginPath();
			_context.RoundedRect(x, y, w, h, CORNER_RADIUS);
			_context.FillColor(NVG.RGBA(28, 30, 34, 192));
			_context.Fill();

			var shadowPaint = _context.BoxGradient(x, y + 2, w, h, CORNER_RADIUS * 2, 10, NVG.RGBA(0, 0, 0, 128), NVG.RGBA(0, 0, 0, 0));
			_context.BeginPath();
			_context.Rect(x - 10, y - 10, w + 20, h + 30);
			_context.RoundedRect(x, y, w, h, CORNER_RADIUS);
			_context.PathWinding(Solidity.Hole);
			_context.FillPaint(shadowPaint);
			_context.Fill();

			var headerPaint = _context.LinearGradient(x, y, x, y + 15, NVG.RGBA(255, 255, 255, 8), NVG.RGBA(0, 0, 0, 16));
			_context.BeginPath();
			_context.RoundedRect(x + 1, y + 1, w - 2, 30, CORNER_RADIUS - 1);
			_context.FillPaint(headerPaint);
			_context.Fill();
			_context.MoveTo(x + 0.5f, y + 0.5f + 30);
			_context.LineTo(x + 0.5f + w - 1, y + 0.5f + 30);
			_context.StrokeColor(NVG.RGBA(0, 0, 0, 32));
			_context.Stroke();

			_context.FontSize(18.0f);
			_context.FontFace("sans-bold");
			_context.TextAlign(Align.Center | Align.Middle);
			_context.FontBlur(2);
			_context.FillColor(NVG.RGBA(0, 0, 0, 128));
			_context.Text(x + w / 2, y + 16 + 1, "hello there");

			_context.FontBlur(0);
			_context.FillColor(NVG.RGBA(220, 220, 220, 160));
			_context.Text(x + w / 2, y + 16, "hello there");
			_context.Restore();

			foreach (var child in ChildNodes) {
				switch (child) {
					case Component component: {
						component.Render(context);

						break;
					}

					default: {
						break;
					}
				}
			}
		}*/

		private void HandleIconifyCallback(Glfw.Window window, bool focused) {

		}

		private void HandleFocusCallback(Glfw.Window window, bool focused) {

		}

		private void HandleCloseCallback(Glfw.Window window) {

		}

		private void HandleRefreshCallback(Glfw.Window window) {
			Glfw.MakeContextCurrent(window);

			Glfw.GetFramebufferSize(window, out var fbWidth, out var fbHeight);

			/*Gl.Viewport(0, 0, fbWidth, fbHeight);

			Gl.ClearColor(0.3f, 0.3f, 0.32f, 1.0f);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

			var w = Width;
			var h = Height;

			_context.BeginFrame(w, h, (float)fbWidth / (float)w);

			Render(_context);

			_context.EndFrame();*/

			Glfw.SwapBuffers(window);
		}

		private void HandlePosCallback(Glfw.Window window, int xpos, int ypos) {

		}

		private void HandleSizeCallback(Glfw.Window window, int width, int height) {

		}
	}
}
