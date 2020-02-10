namespace Quilt.Xml {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Xml;

	public class QuiltAttribute : XmlAttribute, INotifyPropertyChanged {
		protected static readonly Type __stringType = typeof(string);
		protected static readonly TypeConverter __stringTypeConverter = TypeDescriptor.GetConverter(__stringType);

		public new QuiltElement OwnerElement => base.OwnerElement as QuiltElement;

		protected QuiltAttribute(string prefix, string localName, string namespaceURI, QuiltDocument doc) : base(prefix, localName, namespaceURI, doc) {

		}

		public event PropertyChangedEventHandler PropertyChanged;

		public override string InnerText {
			set {
				base.InnerText = value;

				OwnerElement.InvokePropertyChanged(LocalName);
			}
		}
	}

	public class Attribute<TElement, TValue> : QuiltAttribute
		where TElement : QuiltElement {

		private static readonly Type __valueType = typeof(TValue);
		private static readonly TypeConverter __valueTypeConverter = TypeDescriptor.GetConverter(__valueType);
		private static readonly EqualityComparer<TValue> __valueComparer = EqualityComparer<TValue>.Default;

		public new TElement OwnerElement => base.OwnerElement as TElement;

		public new TValue Value {
			get {
				string value = base.Value;

				// if the value is null, we return default
				// this will return null for reference types
				// and the default value for value types
				if (value == null) {
					return default;
				}

				// otherwise, if the value is the empty string
				// we create an instance of the value type
				// this will return the default value for value types (the same as above),
				// but will return an empty instance for reference types
				if (string.IsNullOrEmpty(value)) {
					return Activator.CreateInstance<TValue>();
				}

				// try to convert from string to the value type
				if (__stringTypeConverter != null) {
					if (__stringTypeConverter.CanConvertTo(__valueType)) {
						return (TValue)__stringTypeConverter.ConvertTo(value, __valueType);
					}
				}

				// try to convert to the value type from string
				if (__valueTypeConverter != null) {
					if (__valueTypeConverter.CanConvertFrom(__stringType)) {
						return (TValue)__valueTypeConverter.ConvertFrom(value);
					}
				}

				// fall back to attempting a basic ChangeType
				return (TValue)Convert.ChangeType(value, __valueType);
			}
			set {
				// if the incoming value is null for reference types
				// or the default value for value types
				// we store null
				if (__valueComparer.Equals(value, default)) {
					base.Value = null;
				}

				// try to convert from the value type to string
				if (__stringTypeConverter != null) {
					if (__stringTypeConverter.CanConvertFrom(__valueType)) {
						base.Value = (string)__stringTypeConverter.ConvertFrom(value);

						return;
					}
				}

				// try to convert to string from the value type
				if (__valueTypeConverter != null) {
					if (__valueTypeConverter.CanConvertTo(__stringType)) {
						base.Value = (string)__valueTypeConverter.ConvertTo(value, __stringType);

						return;
					}
				}

				// fall back to attempting a basic ChangeType
				base.Value = (string)Convert.ChangeType(value, __stringType);
			}
		}

		protected Attribute(string prefix, string localName, string namespaceURI, QuiltDocument doc) : base(prefix, localName, namespaceURI, doc) {

		}
	}
}
