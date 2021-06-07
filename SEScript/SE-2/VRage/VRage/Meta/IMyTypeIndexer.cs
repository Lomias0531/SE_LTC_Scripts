using System;

namespace VRage.Meta
{
	/// <summary>
	/// Interface that specifies objects which index types based on non trivial properties they might have.
	///
	/// This may include having a specific parent class or implementing some interface.
	/// These properties are not trivial to compute (computing them for a single indexer
	/// does not help determining if they are meaningfull for another indexer).
	/// Which means they must be computed for all inspected types.
	/// </summary>
	public interface IMyTypeIndexer : IMyMetadataIndexer
	{
		/// <summary>
		/// Index a type.
		/// </summary>
		/// <param name="type"></param>
		void Index(Type type);
	}
}
