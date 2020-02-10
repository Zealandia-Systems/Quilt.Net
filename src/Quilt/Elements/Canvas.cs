namespace Quilt.Elements {
	using Quilt.Xml;

	[Element(CoreNamespace.URI)]
	public abstract class Canvas : Component {
		protected Canvas(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {

		}

		/*public override void Render(Context context) {
			context.Save();

			foreach (var child in ChildNodes.Cast<XmlNode>()) {
				switch (child) {
					case Rectangle rect: {
						context.BeginPath();
						context.Rect(rect.Left, rect.Top, rect.Width, rect.Height);
						context.FillColor(NVG.RGBA(255, 255, 255, 255));
						context.Fill();


						break;
					}

					case Ellipse ellipse: {
						context.BeginPath();
						context.Ellipse(ellipse.Left, ellipse.Top, ellipse.Width, ellipse.Height);
						context.FillColor(NVG.RGBA(255, 255, 255, 255));
						context.Fill();

						break;
					}

					default: {

						break;
					}
				}
			}

			context.Restore();
		}*/
	}
}
