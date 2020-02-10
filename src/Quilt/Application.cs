namespace Quilt {
	using System.IO;
	using System.Xml;
	using Quilt.Xml;

	[Element(CoreNamespace.URI)]
	public class Application : QuiltElement {
		protected Application(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {
		}


		public static Application Load(XmlReader reader) {
			var document = new QuiltDocument();

			document.Load(reader);

			return (Application)document.DocumentElement;
		}

		public static Application Load(Stream stream) {
			using var reader = new XmlTextReader(stream);

			return Load(reader);
		}

		public static Application Load(string path) {
			using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

			return Load(stream);
		}
	}
}
