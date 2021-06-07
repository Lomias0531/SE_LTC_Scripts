using System;

namespace VRage.Meta
{
	/// <summary>
	/// Indexer for metadata related to types that are annotated with a specific attribute.
	/// </summary>
	public interface IMyAttributeIndexer : IMyMetadataIndexer
	{
		/// <summary>
		/// Observe the given type which is annotated with a relevant attribute.
		/// </summary>
		/// <param name="attribute">Instance of the attribute that maps to this indexer.</param>
		/// <param name="type">Annotated type.</param>
		void Observe(Attribute attribute, Type type);
	}
}
