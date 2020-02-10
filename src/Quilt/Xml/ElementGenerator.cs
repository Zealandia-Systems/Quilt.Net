namespace Quilt.Xml {
	using System;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Xml;

	using Sigil.NonGeneric;

	public class ElementGenerator {
		private readonly Random _random = new Random();

		public Type Generate(Type type) {
			return new Context(this, type).Generate();
		}

		private class Context {
			private static readonly Type __voidType = typeof(void);
			private static readonly Type __boolType = typeof(bool);
			private static readonly Type __stringType = typeof(string);
			private static readonly Type __xmlElementType = typeof(XmlElement);
			private static readonly Type __attributeType = typeof(Attribute<,>);
			private static readonly Type __documentType = typeof(QuiltDocument);
			private static readonly MethodInfo __getAttributeNodeMethodInfo = __xmlElementType.GetMethod(nameof(XmlElement.GetAttributeNode), new[] { __stringType });
			private static readonly Type[] __constructorParameterTypes = new[] { __stringType, __stringType, __stringType, __documentType };
			private static readonly Type[] __emptyTypes = new Type[] { };

			private readonly ElementGenerator _generator;
			private readonly Type _type;

			private AssemblyName _assemblyName;
			private AssemblyBuilder _assemblyBuilder;
			private ModuleBuilder _moduleBuilder;
			private TypeBuilder _typeBuilder;

			public Context(ElementGenerator generator, Type type) {
				_generator = generator;
				_type = type;
			}

			public Type Generate() {
				if (!_type.IsAbstract) {
					throw new InvalidOperationException();
				}

				byte[] buffer = new byte[16];

				_generator._random.NextBytes(buffer);

				_assemblyName = new AssemblyName($"{_type.Assembly.GetName().Name}.x{BitConverter.ToString(buffer).Replace("-", "")}");
				_assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndCollect);
				_moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName.Name);

				_generator._random.NextBytes(buffer);

				_typeBuilder = _moduleBuilder.DefineType($"{_type.Namespace}.x{BitConverter.ToString(buffer).Replace("-", "")}.{_type.Name}", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, _type);

				GenerateConstructor();
				GenerateProperties();

				return _typeBuilder.CreateTypeInfo();
			}

			private void GenerateConstructor() {
				ConstructorInfo constructorInfo = _type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, __constructorParameterTypes, null);

				var emit = Emit.BuildConstructor(__constructorParameterTypes, _typeBuilder, constructorInfo.Attributes);

				emit.LoadArgument(0);
				emit.LoadArgument(1);
				emit.LoadArgument(2);
				emit.LoadArgument(3);
				emit.LoadArgument(4);
				emit.Call(constructorInfo);
				emit.Return();

				emit.CreateConstructor();
			}

			private void GenerateProperties() {
				foreach (var propertyInfo in _type.GetProperties()) {
					if (propertyInfo.GetCustomAttribute<AttributeAttribute>() is AttributeAttribute attribute) {
						if (propertyInfo.GetGetMethod() is MethodInfo getMethodInfo) {
							if (getMethodInfo.IsAbstract) {
								GeneratePropertyGetMethod(propertyInfo, attribute, getMethodInfo);
							}
						}

						if (propertyInfo.GetSetMethod() is MethodInfo setMethodInfo) {
							if (setMethodInfo.IsAbstract) {
								GeneratePropertySetMethod(propertyInfo, attribute, setMethodInfo);
							}
						}
					}
				}
			}

			private void GeneratePropertyGetMethod(PropertyInfo propertyInfo, AttributeAttribute attribute, MethodInfo getMethodInfo) {
				var propertyType = propertyInfo.PropertyType;
				var emit = Emit.BuildInstanceMethod(propertyType, __emptyTypes, _typeBuilder, getMethodInfo.Name, getMethodInfo.Attributes & ~MethodAttributes.Abstract);

				var attributeType = __attributeType.MakeGenericType(_type, propertyInfo.PropertyType);
				var getValueMethodInfo = attributeType.GetProperty("Value", propertyType).GetGetMethod();

				var nodeLocal = emit.DeclareLocal(attributeType);
				var resultLocal = emit.DeclareLocal(propertyInfo.PropertyType);
				var label1 = emit.DefineLabel();
				var label2 = emit.DefineLabel();

				emit.LoadArgument(0);
				emit.LoadConstant(attribute.LocalName ?? propertyInfo.Name);
				emit.CallVirtual(__getAttributeNodeMethodInfo);
				emit.IsInstance(attributeType);
				emit.StoreLocal(nodeLocal);
				emit.LoadLocal(nodeLocal);
				emit.LoadNull();
				emit.BranchIfEqual(label1);
				//emit.UnsignedBranchIfNotEqual(label1);
				emit.LoadLocal(nodeLocal);
				emit.CallVirtual(getValueMethodInfo);
				emit.StoreLocal(resultLocal);
				emit.Branch(label2);
				emit.MarkLabel(label1);
				emit.WriteLine("Get: {0}", nodeLocal);

				if (propertyType.IsValueType) {
					emit.LoadLocalAddress(resultLocal);
					emit.InitializeObject(propertyType);
				} else {
					emit.LoadNull();
					emit.StoreLocal(resultLocal);
				}

				emit.Branch(label2);
				emit.MarkLabel(label2);
				emit.LoadLocal(resultLocal);
				emit.Return();

				_typeBuilder.DefineMethodOverride(emit.CreateMethod(), getMethodInfo);
			}

			private void GeneratePropertySetMethod(PropertyInfo propertyInfo, AttributeAttribute attribute, MethodInfo setMethodInfo) {
				var propertyType = propertyInfo.PropertyType;
				var emit = Emit.BuildInstanceMethod(__voidType, new[] { propertyType }, _typeBuilder, setMethodInfo.Name, setMethodInfo.Attributes & ~MethodAttributes.Abstract);

				var attributeType = __attributeType.MakeGenericType(_type, propertyInfo.PropertyType);
				var setValueMethodInfo = attributeType.GetProperty("Value", propertyType).GetSetMethod();

				var nodeLocal = emit.DeclareLocal(attributeType);
				var label = emit.DefineLabel();

				emit.LoadArgument(0);
				emit.LoadConstant(attribute.LocalName ?? propertyInfo.Name);
				emit.CallVirtual(__getAttributeNodeMethodInfo);
				emit.IsInstance(attributeType);
				emit.StoreLocal(nodeLocal);
				emit.LoadLocal(nodeLocal);
				emit.LoadNull();
				emit.BranchIfEqual(label);
				//emit.UnsignedBranchIfNotEqual(label);
				emit.LoadLocal(nodeLocal);
				emit.LoadArgument(1);
				emit.CallVirtual(setValueMethodInfo);
				emit.MarkLabel(label);
				emit.Return();

				_typeBuilder.DefineMethodOverride(emit.CreateMethod(), setMethodInfo);
			}
		}
	}
}
