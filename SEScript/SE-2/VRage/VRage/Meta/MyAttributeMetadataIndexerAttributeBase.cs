using System;

namespace VRage.Meta
{
	/// <summary>
	/// Base class for attributes that provide information about type indexers.
	/// </summary>
	public abstract class MyAttributeMetadataIndexerAttributeBase : Attribute
	{
		/// <summary>
		/// The type of the attribute that is indexed.
		/// </summary>
		public abstract Type AttributeType
		{
			get;
		}

		/// <summary>
		/// The type of the indexer.
		///
		/// When this is null the type is inferred from the annotated type.
		/// </summary>
		public abstract Type TargetType
		{
			get;
		}
	}
}
