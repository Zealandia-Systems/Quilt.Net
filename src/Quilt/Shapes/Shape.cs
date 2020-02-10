namespace Quilt.Shapes {
	using System;

	using Quilt.Xml;

	[Element(CoreNamespace.URI)]
	public abstract class Shape : QuiltElement {
		[Attribute]
		public abstract float Left { get; set; }

		[Attribute]
		public float Top {
			get {
				return Convert.ToSingle(GetAttributeNode("Top")?.Value ?? "0");
			}
			set {
				SetAttribute("Top", value.ToString());
			}
		}

		[Attribute]
		public float Width {
			get {
				return Convert.ToSingle(GetAttributeNode("Width")?.Value ?? "0");
			}
			set {
				SetAttribute("Width", value.ToString());
			}
		}

		[Attribute]
		public float Height {
			get {
				return Convert.ToSingle(GetAttributeNode("Height")?.Value ?? "0");
			}
			set {
				SetAttribute("Height", value.ToString());
			}
		}

		protected Shape(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {

		}
	}
}
