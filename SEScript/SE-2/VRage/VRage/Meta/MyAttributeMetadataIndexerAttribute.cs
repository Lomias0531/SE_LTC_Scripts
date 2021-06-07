using System;

namespace VRage.Meta
{
	/// <summary>
	/// Simple identification of a metadata indexer class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
	public class MyAttributeMetadataIndexerAttribute : MyAttributeMetadataIndexerAttributeBase
	{
		private readonly Type m_attrType;

		private readonly Type m_target;

		public override Type AttributeType => m_attrType;

		public override Type TargetType => m_target;

		public MyAttributeMetadataIndexerAttribute(Type attrType)
		{
			m_attrType = attrType;
			m_target = null;
		}

		public MyAttributeMetadataIndexerAttribute(Type attrType, Type indexerType)
		{
			m_attrType = attrType;
			m_target = indexerType;
		}
	}
}
