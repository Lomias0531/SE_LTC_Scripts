using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Replication
{
	internal class MyCharacterReplicable : MyEntityReplicableBaseEvent<MyCharacter>
	{
		private MyPropertySyncStateGroup m_propertySync;

		private readonly HashSet<IMyReplicable> m_dependencies = new HashSet<IMyReplicable>();

		private readonly HashSet<VRage.Game.ModAPI.Ingame.IMyEntity> m_dependencyParents = new HashSet<VRage.Game.ModAPI.Ingame.IMyEntity>();

		private long m_ownerId;

		private long m_characterId;

		public HashSetReader<VRage.Game.ModAPI.Ingame.IMyEntity> CachedParentDependencies => new HashSetReader<VRage.Game.ModAPI.Ingame.IMyEntity>(m_dependencyParents);

		public override bool HasToBeChild => base.Instance.Parent != null;

		protected override IMyStateGroup CreatePhysicsGroup()
		{
			return new MyCharacterPhysicsStateGroup(base.Instance, this);
		}

		protected override void OnHook()
		{
			base.OnHook();
			m_propertySync = new MyPropertySyncStateGroup(this, base.Instance.SyncType)
			{
				GlobalValidate = ((MyEventContext context) => HasRights(context.ClientState.EndpointId.Id, ValidationType.Controlled))
			};
			if (base.Instance != null)
			{
				base.Instance.Hierarchy.OnParentChanged += OnParentChanged;
			}
		}

		private void OnParentChanged(MyHierarchyComponentBase oldParent, MyHierarchyComponentBase newParent)
		{
			if (IsReadyForReplication)
			{
				(MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy(this);
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			base.GetStateGroups(resultList);
			if (m_propertySync != null && m_propertySync.PropertyCount > 0)
			{
				resultList.Add(m_propertySync);
			}
		}

		public override IMyReplicable GetParent()
		{
			if (base.Instance == null)
			{
				return null;
			}
			if (base.Instance.Parent != null)
			{
				return MyExternalReplicable.FindByObject(base.Instance.GetTopMostParent());
			}
			return null;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			if (base.Instance == null)
			{
				return false;
			}
			stream.WriteBool(base.Instance.IsUsing is MyShipController);
			if (base.Instance.IsUsing is MyShipController)
			{
				long value = base.Instance.IsUsing.EntityId;
				MySerializer.Write(stream, ref value);
				long value2 = base.Instance.EntityId;
				MySerializer.Write(stream, ref value2);
			}
			else
			{
				byte clientReplicableIslandIndex = MyMultiplayer.GetReplicationServer().GetClientReplicableIslandIndex(this, clientEndpoint);
				stream.WriteByte(clientReplicableIslandIndex);
				MyObjectBuilder_Character value3 = (MyObjectBuilder_Character)base.Instance.GetObjectBuilder();
				MySerializer.Write(stream, ref value3, MyObjectBuilderSerializer.Dynamic);
			}
			return true;
		}

		protected override void OnLoad(BitStream stream, Action<MyCharacter> loadingDoneHandler)
		{
			bool value = true;
			if (stream != null)
			{
				MySerializer.CreateAndRead(stream, out value);
			}
			if (value)
			{
				if (stream != null)
				{
					MySerializer.CreateAndRead(stream, out m_ownerId);
					MySerializer.CreateAndRead(stream, out m_characterId);
				}
				MyEntities.CallAsync(delegate
				{
					LoadAsync(m_ownerId, m_characterId, loadingDoneHandler);
				});
			}
			else
			{
				byte islandIndex = stream.ReadByte();
				MyObjectBuilder_Character myObjectBuilder_Character = (MyObjectBuilder_Character)MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
				TryRemoveExistingEntity(myObjectBuilder_Character.EntityId);
				MyCharacter character = MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_Character) as MyCharacter;
				MyEntities.InitAsync(character, myObjectBuilder_Character, addToScene: true, delegate
				{
					loadingDoneHandler(character);
				}, islandIndex);
			}
		}

		private static void LoadAsync(long ownerId, long characterId, Action<MyCharacter> loadingDoneHandler)
		{
			MyEntities.TryGetEntityById(ownerId, out MyEntity entity);
			MyShipController myShipController = entity as MyShipController;
			if (myShipController != null)
			{
				if (myShipController.Pilot != null)
				{
					loadingDoneHandler(myShipController.Pilot);
					MySession.Static.Players.UpdatePlayerControllers(ownerId);
				}
				else
				{
					MyEntities.TryGetEntityById(characterId, out MyEntity entity2);
					MyCharacter obj = entity2 as MyCharacter;
					loadingDoneHandler(obj);
				}
			}
			else
			{
				loadingDoneHandler(null);
			}
		}

		public override HashSet<IMyReplicable> GetDependencies(bool forPlayer)
		{
			if (!forPlayer)
			{
				return null;
			}
			m_dependencies.Clear();
			m_dependencyParents.Clear();
			if (!Sync.IsServer)
			{
				return m_dependencies;
			}
			foreach (MyDataBroadcaster allRelayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters(base.Instance, base.Instance.GetPlayerIdentityId(), mutual: false))
			{
				if (base.Instance.RadioBroadcaster != allRelayedBroadcaster && !allRelayedBroadcaster.Closed)
				{
					MyFarBroadcasterReplicable myFarBroadcasterReplicable = MyExternalReplicable.FindByObject(allRelayedBroadcaster) as MyFarBroadcasterReplicable;
					if (myFarBroadcasterReplicable != null)
					{
						m_dependencies.Add(myFarBroadcasterReplicable);
						if (myFarBroadcasterReplicable.Instance != null && myFarBroadcasterReplicable.Instance.Entity != null)
						{
							VRage.ModAPI.IMyEntity topMostParent = myFarBroadcasterReplicable.Instance.Entity.GetTopMostParent();
							if (topMostParent != null)
							{
								m_dependencyParents.Add(topMostParent);
							}
						}
					}
				}
			}
			return m_dependencies;
		}

		public override ValidationResult HasRights(EndpointId endpointId, ValidationType validationFlags)
		{
			bool flag = true;
			if (validationFlags.HasFlag(ValidationType.Controlled))
			{
				flag &= (endpointId.Value == base.Instance.GetClientIdentity().SteamId);
			}
			if (!flag)
			{
				return ValidationResult.Kick | ValidationResult.Controlled;
			}
			return ValidationResult.Passed;
		}

		public override bool ShouldReplicate(MyClientInfo client)
		{
			return !base.Instance.IsDead;
		}
	}
}
