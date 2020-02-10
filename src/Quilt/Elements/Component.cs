namespace Quilt.Elements {
	using Quilt.Xml;

	public abstract class Component : QuiltElement {
		[Attribute]
		public abstract float Left { get; set; }

		[Attribute]
		public abstract float Top { get; set; }

		[Attribute]
		public abstract float Width { get; set; }

		[Attribute]
		public abstract float Height { get; set; }
		
		protected Component(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {

		}

		//public abstract void Render(Context context);
	}
}
