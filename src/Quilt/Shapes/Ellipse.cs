namespace Quilt.Shapes {
	using Quilt.Xml;

	[Element(QuiltNamespace.URI)]
	public abstract class Ellipse : Shape {
		protected Ellipse(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {
		}
	}
}
