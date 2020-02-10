namespace Quilt.Xml {
	public class AttributeAttribute : System.Attribute {
		public string LocalName { get; set; }

		public AttributeAttribute(string localName = null) {
			LocalName = localName;
		}
	}
}
