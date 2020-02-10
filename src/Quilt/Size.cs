namespace Quilt {
	public struct Size {
		public int Width { get; set; }
		public int Height { get; set; }

		public Size(int width, int height) {
			Width = width;
			Height = height;
		}

		public void Deconstruct(out int width, out int height) {
			width = Width;
			height = Height;
		}
	}
}
