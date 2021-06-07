using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Interface providing terrain height in model space.
	/// </summary>
	public interface IMyTerrainHeightProvider
	{
		/// <summary>
		/// Get terrain height in model space.
		/// </summary>
		/// <remarks>The <paramref name="key" /> can be used by the implementation to cache data about this query.
		/// A <paramref name="key" /> with value zero indicates that no caching should be performed.</remarks>
		/// <param name="key">Unique key for this query.</param>
		/// <param name="bonePosition">bone position in model space</param>
		/// <param name="boneRigPosition">bone rig position in model space</param>
		/// <param name="terrainHeight">terrain height in model space</param>
		/// <param name="terrainNormal">terrain normal in (character) model space</param>
		/// <returns>true if the intersection was found</returns>
		bool GetTerrainHeight(int key, Vector3 bonePosition, Vector3 boneRigPosition, out float terrainHeight, out Vector3 terrainNormal);

		/// <summary>
		/// Get reference terrain height - (flat terrain height) in model space.
		/// </summary>
		float GetReferenceTerrainHeight();
	}
}
