namespace Quilt.Xml {
	public class ElementAttribute : System.Attribute {
		public string NamespaceURI { get; set; }
		public string LocalName { get; set; }

		public ElementAttribute(string namespaceUri, string localName = null) {
			NamespaceURI = namespaceUri;
			LocalName = localName;
		}
	}
}
