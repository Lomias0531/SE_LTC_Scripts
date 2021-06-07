using VRage.Game.Components;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Ingame (Programmable Block) interface for all entities.
	/// </summary>
	public interface IMyEntity
	{
		MyEntityComponentContainer Components
		{
			get;
		}

		long EntityId
		{
			get;
		}

		string Name
		{
			get;
		}

		string DisplayName
		{
			get;
		}

		/// <summary>
		/// Returns true if this entity has got at least one inventory. 
		/// Note that one aggregate inventory can contain zero simple inventories =&gt; zero will be returned even if GetInventory() != null.
		/// </summary>
		bool HasInventory
		{
			get;
		}

		/// <summary>
		/// Returns the count of the number of inventories this entity has.
		/// </summary>
		int InventoryCount
		{
			get;
		}

		BoundingBoxD WorldAABB
		{
			get;
		}

		BoundingBoxD WorldAABBHr
		{
			get;
		}

		MatrixD WorldMatrix
		{
			get;
		}

		BoundingSphereD WorldVolume
		{
			get;
		}

		BoundingSphereD WorldVolumeHr
		{
			get;
		}

		/// <summary>
		/// Simply get the MyInventoryBase component stored in this entity.
		/// </summary>
		/// <returns></returns>
		IMyInventory GetInventory();

		/// <summary>
		/// Search for inventory component with maching index.
		/// </summary>
		IMyInventory GetInventory(int index);

		Vector3D GetPosition();
	}
}
