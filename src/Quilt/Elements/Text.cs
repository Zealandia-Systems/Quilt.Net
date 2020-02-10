namespace Quilt.Elements {
	using Quilt.Xml;

	[Element(QuiltNamespace.URI)]
	public abstract class Text : Component {
		protected Text(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {

		}

		/*public override void Render(Context context) {
			context.Save();

			context.FontSize(18.0f);
			context.FontFace("sans-bold");
			context.TextAlign(Align.Center | Align.Middle);
			context.FontBlur(2);
			context.FillColor(NVG.RGBA(0, 0, 0, 128));
			context.Text(Left + Width / 2, Top + 16 + 1, InnerText);

			context.FontBlur(0);
			context.FillColor(NVG.RGBA(255, 255, 255, 255));
			context.Text(Left + Width / 2, Top + 16, InnerText);

			context.Restore();
		}*/
	}
}
