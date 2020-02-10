namespace Quilt.Xml {
	using System.Collections.Generic;
	using System.Xml;

	public class QuiltDocument : XmlDocument {
		private readonly Dictionary<string, Namespace> _namespaces;
		private QuiltXmlReader _reader;

		internal Stack<QuiltElement> _elementStack = new Stack<QuiltElement>();

		public QuiltDocument() {
			_namespaces = Namespace.GetNamespaces();
		}

		public override void Load(XmlReader reader) {
			_elementStack.Clear();

			_reader = new QuiltXmlReader(this, reader);

			base.Load(_reader);
		}

		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI) {
			if (!_namespaces.TryGetValue(namespaceURI, out var @namespace)) {
				throw new System.Exception();
			}

			if (!@namespace.TryCreateElement(prefix, localName, namespaceURI, this, out var element)) {
				throw new System.Exception();
			}

			_elementStack.Push(element);

			return element;
		}

		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI) {
			if (string.IsNullOrEmpty(prefix) && localName == "xmlns") {
				return base.CreateAttribute(prefix, localName, namespaceURI);
			}

			if (string.IsNullOrEmpty(namespaceURI)) {
				namespaceURI = _reader.ElementNamespaceUri;
			}

			if (!_namespaces.TryGetValue(namespaceURI, out var @namespace)) {
				throw new System.Exception();
			}

			if (!@namespace.TryCreateAttribute(_elementStack.Peek(), prefix, localName, namespaceURI, this, out var attribute)) {
				throw new System.Exception();
			}

			return attribute;
		}
	}
}
