namespace VRage.Meta
{
	/// <summary>
	/// Base interface for objects that handle metadata indexing in the metadata system.
	/// </summary>
	public interface IMyMetadataIndexer
	{
		/// <summary>
		/// Set the parent indexer, the parent indexer will always be an instance of the same type.
		///
		/// For the majority of indexers the metadata for the parent can be used directly, tohers might need to flaten the whole information hierarchy on a per indexer basis.
		/// </summary>
		/// <param name="indexer">The parent indexer.</param>
		void SetParent(IMyMetadataIndexer indexer);

		/// <summary>
		/// Called when the context that contains this indexer becomes the active one.
		///
		/// Any global references to this indexer need to be update then.
		/// </summary>
		void Activate();

		/// <summary>
		/// Called when the context that contains this indexer is disposed.
		///
		/// All references to this indexer must be invalidated and all held data released.
		/// </summary>
		void Close();

		/// <summary>
		/// Invoked when a batch of assemblies are loaded and indexers are then given a chance to build any additional structures over the data.
		///
		/// This is useful for indexers where there are nontrivial relations between the types that need to be rebuild every time new tipes are added.
		/// </summary>
		void Process();
	}
}
