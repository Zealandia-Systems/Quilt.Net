namespace Quilt.Xml {
	using System.ComponentModel;
	using System.Xml;

	public abstract class QuiltElement : XmlElement, INotifyPropertyChanged {
		public new QuiltDocument OwnerDocument { get; }

		protected QuiltElement(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {
			OwnerDocument = document;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		internal void InvokePropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
