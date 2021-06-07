using System;

namespace VRage.Meta
{
	/// <summary>
	/// Attribute indicating a type metadata indexer.
	///
	/// Valid type indexers marked with this attribute are automatically discovered and used.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class MyTypeMetadataIndexerAttribute : Attribute
	{
	}
}
