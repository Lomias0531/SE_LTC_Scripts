using System;
using VRage.Meta;
using VRage.Utils;

namespace VRage.Factory
{
	/// <summary>
	/// Use this attribute to mark any factories you may use in code. This will automatically register the factory.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public class MyFactorableAttribute : MyAttributeMetadataIndexerAttributeBase
	{
		private readonly Type m_factoryType;

		private readonly Type m_attributeType;

		public override Type AttributeType => m_attributeType;

		public override Type TargetType => m_factoryType;

		public MyFactorableAttribute(Type factoryType)
		{
			m_factoryType = factoryType;
			Type typeFromHandle = typeof(MyObjectFactory<, >);
			if (!m_factoryType.IsInstanceOfGenericType(typeFromHandle))
			{
				MyLog.Default.Error("Type {0} is not an object factory");
				return;
			}
			while (!factoryType.IsGenericType || factoryType.GetGenericTypeDefinition() != typeFromHandle)
			{
				factoryType = factoryType.BaseType;
			}
			m_attributeType = factoryType.GenericTypeArguments[0];
		}
	}
}
