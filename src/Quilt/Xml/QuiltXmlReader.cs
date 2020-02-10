namespace Quilt.Xml {
	using System.Xml;

	internal class QuiltXmlReader : XmlReader {
		private readonly QuiltDocument _document;
		private readonly XmlReader _reader;

		public string ElementNamespaceUri { get; private set; }

		public QuiltXmlReader(QuiltDocument document, XmlReader reader) {
			_document = document;
			_reader = reader;
		}

		public override int AttributeCount => _reader.AttributeCount;

		public override string BaseURI => _reader.BaseURI;

		public override int Depth => _reader.Depth;

		public override bool EOF => _reader.EOF;

		public override bool IsEmptyElement => _reader.IsEmptyElement;

		public override string LocalName => _reader.LocalName;

		public override string NamespaceURI => _reader.NamespaceURI;

		public override XmlNameTable NameTable => _reader.NameTable;

		public override XmlNodeType NodeType => _reader.NodeType;

		public override string Prefix => _reader.Prefix;

		public override ReadState ReadState => _reader.ReadState;

		public override string Value => _reader.Value;

		public override string GetAttribute(int i) => _reader.GetAttribute(i);

		public override string GetAttribute(string name) => _reader.GetAttribute(name);

		public override string GetAttribute(string name, string namespaceURI) => _reader.GetAttribute(name, namespaceURI);

		public override string LookupNamespace(string prefix) => _reader.LookupNamespace(prefix);

		public override bool MoveToAttribute(string name) => _reader.MoveToAttribute(name);

		public override bool MoveToAttribute(string name, string ns) => _reader.MoveToAttribute(name);

		public override bool MoveToElement() => _reader.MoveToElement();

		public override bool MoveToFirstAttribute() => _reader.MoveToFirstAttribute();

		public override bool MoveToNextAttribute() => _reader.MoveToNextAttribute();

		public override bool Read() {
			bool result = _reader.Read();

			switch (_reader.NodeType) {
				case XmlNodeType.Element: {
					ElementNamespaceUri = _reader.NamespaceURI;

					break;
				}

				case XmlNodeType.EndElement: {
					_document._elementStack.Pop();

					break;
				}
			}

			return result;
		}

		public override bool ReadAttributeValue() => _reader.ReadAttributeValue();

		public override void ResolveEntity() => _reader.ResolveEntity();
	}
}
