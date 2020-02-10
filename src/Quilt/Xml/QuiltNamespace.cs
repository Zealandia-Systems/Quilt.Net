namespace Quilt.Xml {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	public abstract class Namespace {
		private const int ELEMENT_LENGTH = 7;

		private static readonly Type __stringType = typeof(string);
		private static readonly Type __quiltDocumentType = typeof(QuiltDocument);
		private static readonly Type __quiltAttributeType = typeof(Attribute<,>);
		private static readonly Type __namespaceType = typeof(Namespace);
		private static readonly Type __elementAttributeType = typeof(ElementAttribute);
		private static readonly Type __attributeAttributeType = typeof(AttributeAttribute);
		private static readonly ElementGenerator __generator = new ElementGenerator();

		private readonly Dictionary<string, ElementInfo> _elementTypes = new Dictionary<string, ElementInfo>();

		public string Uri { get; }

		protected Namespace(string uri) {
			Uri = uri;
		}

		public bool TryCreateElement(string prefix, string localName, string namespaceURI, QuiltDocument document, out QuiltElement element) {
			if (!_elementTypes.TryGetValue(localName, out var elementInfo)) {
				element = null;

				return false;
			}

			element = elementInfo.Create(prefix, localName, namespaceURI, document);

			return true;
		}

		public bool TryCreateAttribute(QuiltElement element, string prefix, string localName, string namespaceURI, QuiltDocument document, out QuiltAttribute attribute) {
			if (!_elementTypes.TryGetValue(element.LocalName, out var elementInfo)) {
				attribute = null;

				return false;
			}

			if (!elementInfo.AttributeInfos.TryGetValue(localName, out var attributeInfo)) {
				attribute = null;

				return false;
			}

			attribute = attributeInfo.Create(prefix, localName, namespaceURI, document);

			return true;
		}

		private static Func<string, string, string, QuiltDocument, QuiltElement> CreateElementLambda(Type type) {
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { __stringType, __stringType, __stringType, __quiltDocumentType }, null);

			var prefix = Expression.Parameter(__stringType, "prefix");
			var localName = Expression.Parameter(__stringType, "localName");
			var namespaceURI = Expression.Parameter(__stringType, "namespaceURI");
			var document = Expression.Parameter(__quiltDocumentType, "document");

			var parameters = new ParameterExpression[] {
				prefix,
				localName,
				namespaceURI,
				document
			};

			return Expression.Lambda<Func<string, string, string, QuiltDocument, QuiltElement>>(Expression.New(constructor, parameters), parameters).Compile();
		}

		private static Func<string, string, string, QuiltDocument, QuiltAttribute> CreateAttributeLambda(Type elementType, PropertyInfo property) {
			ConstructorInfo constructor = __quiltAttributeType.MakeGenericType(elementType, property.PropertyType).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { __stringType, __stringType, __stringType, __quiltDocumentType }, null);

			var prefix = Expression.Parameter(__stringType, "prefix");
			var localName = Expression.Parameter(__stringType, "localName");
			var namespaceURI = Expression.Parameter(__stringType, "namespaceURI");
			var document = Expression.Parameter(__quiltDocumentType, "document");

			var parameters = new ParameterExpression[] {
				prefix,
				localName,
				namespaceURI,
				document
			};

			return Expression.Lambda<Func<string, string, string, QuiltDocument, QuiltAttribute>>(Expression.New(constructor, parameters), parameters).Compile();
		}

		public static Dictionary<string, Namespace> GetNamespaces() {
			var types = __namespaceType.Assembly.GetTypes();
			var namespaces = new Dictionary<string, Namespace>();

			// scan over the types once to find all Namespace types
			foreach (var type in types) {
				if (__namespaceType.IsAssignableFrom(type) && __namespaceType != type) {
					var @namespace = (Namespace)Activator.CreateInstance(type, true);

					namespaces.Add(@namespace.Uri, @namespace);
				}
			}

			// scan over the types again to distribute Element types to Namespace instances
			foreach (var type in types) {
				if (type.GetCustomAttributes(__elementAttributeType, false).FirstOrDefault() is ElementAttribute elementAttribute) {
					if (!namespaces.TryGetValue(elementAttribute.NamespaceURI, out var @namespace)) {
						// TODO: log error

						continue;
					}

					var concreteType = type;

					if (type.IsAbstract) {
						concreteType = GenerateConcreteType(type);
					}

					var elementInfo = new ElementInfo(type, CreateElementLambda(concreteType));

					@namespace._elementTypes.Add(elementAttribute.LocalName ?? type.Name, elementInfo);

					foreach (PropertyInfo property in type.GetProperties()) {
						if (property.GetCustomAttributes(__attributeAttributeType, false).FirstOrDefault() is AttributeAttribute attributeAttribute) {
							elementInfo.AttributeInfos.Add(attributeAttribute.LocalName ?? property.Name, new AttributeInfo(CreateAttributeLambda(type, property)));
						}
					}
				}
			}

			return namespaces;
		}

		private static Type GenerateConcreteType(Type type) {
			return __generator.Generate(type);
		}

		private class ElementInfo {
			public Type Type { get; }
			public Func<string, string, string, QuiltDocument, QuiltElement> Create { get; }

			public Dictionary<string, AttributeInfo> AttributeInfos { get; } = new Dictionary<string, AttributeInfo>();

			public ElementInfo(Type type, Func<string, string, string, QuiltDocument, QuiltElement> create) {
				Type = type;
				Create = create;
			}
		}

		private class AttributeInfo {
			public Func<string, string, string, QuiltDocument, QuiltAttribute> Create { get; }

			public AttributeInfo(Func<string, string, string, QuiltDocument, QuiltAttribute> create) {
				Create = create;
			}
		}
	}
}
