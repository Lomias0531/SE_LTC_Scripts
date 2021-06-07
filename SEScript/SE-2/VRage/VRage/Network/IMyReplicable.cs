using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRageMath;

namespace VRage.Network
{
	public interface IMyReplicable : IMyNetObject, IMyEventOwner
	{
		/// <summary>
		/// Child replicables are strongly dependent on parent.
		/// When parent is replicated, children are replicated.
		/// </summary>
		bool HasToBeChild
		{
			get;
		}

		/// <summary>
		/// Determines whether the replicable is present in the world and should be replicated based on its position.
		/// </summary>
		bool IsSpatial
		{
			get;
		}

		/// <summary>
		/// Should this replicable be replicated immediately after it's created (in priority updates)?
		/// </summary>
		bool PriorityUpdate
		{
			get;
		}

		/// <summary>
		/// Whether the replicable should be included in replication islands.
		/// </summary>
		bool IncludeInIslands
		{
			get;
		}

		string InstanceName
		{
			get;
		}

		bool IsReadyForReplication
		{
			get;
		}

		Dictionary<IMyReplicable, Action> ReadyForReplicationAction
		{
			get;
		}

		/// <summary>
		/// Called when root replicable AABB changed
		/// </summary>
		Action<IMyReplicable> OnAABBChanged
		{
			get;
			set;
		}

		/// <summary>
		/// Whether the replicable should be replicated on a specific client. 
		/// Some objects that are created locally don't need to be synchronized until some conditions are met (generated asteroids, trees).
		/// </summary>
		bool ShouldReplicate(MyClientInfo client);

		/// <summary>
		/// Gets parent which must be replicated first.
		/// </summary>
		IMyReplicable GetParent();

		/// <summary>
		/// Serializes object for replication to client.
		/// </summary>
		bool OnSave(BitStream stream, Endpoint clientEndpoint);

		/// <summary>
		/// Client deserializes object and adds it to proper collection (e.g. MyEntities).
		/// Loading done handler can be called synchronously or asynchronously (but always from Update thread).
		/// </summary>
		void OnLoad(BitStream stream, Action<bool> loadingDoneHandler);

		/// <summary>
		/// Load the replicable again if it failed because the parent was still in queue
		/// </summary>
		void Reload(Action<bool> loadingDoneHandler);

		/// <summary>
		/// Called on client when server destroyed this replicable. Should close any objects or entities it created in OnLoad.
		/// </summary>
		void OnDestroyClient();

		/// <summary>
		/// Caled on server when replicable is removed from all clients
		/// </summary>
		void OnRemovedFromReplication();

		/// <summary>
		/// Returns state groups for replicable in a list.
		/// This method can has to return objects in same order every time (e.g. first terminal, second physics etc).
		/// It does not have to return same instances every time.
		/// </summary>
		void GetStateGroups(List<IMyStateGroup> resultList);

		/// <summary>
		/// Root replicables always have spatial representation. Can return invalid if this is not a IsSpatial replicable .
		/// </summary>
		BoundingBoxD GetAABB();

		/// <summary>
		/// Dependend replicables, which might not be in AABB of this replicable. Ie. all relayed antennas are depended 
		/// on mycharacter and need to be synced with him. 
		/// </summary>
		/// <returns></returns>
		HashSet<IMyReplicable> GetDependencies(bool forPlayer);

		/// <summary>
		/// Get the replicable's physical dependencies, i.e. objects close enough for it to physically interact with. 
		/// These will get replicated along with the replicable in one batch (replication island).
		/// </summary>
		HashSet<IMyReplicable> GetPhysicalDependencies(MyTimeSpan timeStamp, MyReplicablesBase replicables);

		/// <summary>
		/// Debug method for testing whether replicables are in concistent state
		/// </summary>
		bool CheckConsistency();

		ValidationResult HasRights(EndpointId client, ValidationType validationFlags);
	}
}
