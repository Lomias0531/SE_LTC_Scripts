using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyTerminalReplicable : MyExternalReplicableEvent<MySyncedBlock>
	{
		private MyPropertySyncStateGroup m_propertySync;

		private long m_blockEntityId;

		private MySyncedBlock Block => base.Instance;

		public override bool IsValid
		{
			get
			{
				if (Block != null)
				{
					return !Block.MarkedForClose;
				}
				return false;
			}
		}

		public override bool HasToBeChild => true;

		protected override void OnHook()
		{
			base.OnHook();
			m_propertySync = new MyPropertySyncStateGroup(this, Block.SyncType)
			{
				GlobalValidate = ((MyEventContext context) => HasRights(context.ClientState.EndpointId.Id, ValidationType.Access | ValidationType.Ownership))
			};
			Block.OnClose += delegate
			{
				RaiseDestroyed();
			};
			Block.SlimBlock.CubeGridChanged += OnBlockCubeGridChanged;
			if (Sync.IsServer)
			{
				Block.AddedToScene += MarkDirty;
			}
			m_parent = MyExternalReplicable.FindByObject(Block.CubeGrid);
		}

		private void MarkDirty(MyEntity entity)
		{
			m_propertySync.MarkDirty();
		}

		private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
		{
			m_parent = MyExternalReplicable.FindByObject(Block.CubeGrid);
			(MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy(this);
		}

		public override ValidationResult HasRights(EndpointId endpointId, ValidationType validationFlags)
		{
			if (Block == null)
			{
				return ValidationResult.Kick;
			}
			ValidationResult validationResult = ValidationResult.Passed;
			long identityId = MySession.Static.Players.TryGetIdentityId(endpointId.Value);
			if (validationFlags.HasFlag(ValidationType.Ownership) && (!MySession.Static.RemoteAdminSettings.TryGetValue(endpointId.Value, out AdminSettingsEnum value) || !value.HasFlag(AdminSettingsEnum.UseTerminals)))
			{
				MyRelationsBetweenPlayerAndBlock userRelationToOwner = Block.GetUserRelationToOwner(identityId);
				if (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.FactionShare && userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Owner && userRelationToOwner != 0)
				{
					return ValidationResult.Kick | ValidationResult.Ownership;
				}
			}
			if (validationFlags.HasFlag(ValidationType.BigOwner) && !MyReplicableRightsValidator.GetBigOwner(Block.CubeGrid, endpointId, identityId, spaceMaster: false))
			{
				return ValidationResult.Kick | ValidationResult.BigOwner;
			}
			if (validationFlags.HasFlag(ValidationType.BigOwnerSpaceMaster) && !MyReplicableRightsValidator.GetBigOwner(Block.CubeGrid, endpointId, identityId, spaceMaster: true))
			{
				return ValidationResult.Kick | ValidationResult.BigOwnerSpaceMaster;
			}
			if (validationFlags.HasFlag(ValidationType.Controlled))
			{
				validationResult = MyReplicableRightsValidator.GetControlled(Block.CubeGrid, endpointId);
				if (validationResult == ValidationResult.Kick)
				{
					return validationResult | ValidationResult.Controlled;
				}
			}
			if (validationFlags.HasFlag(ValidationType.Access))
			{
				if (Block.CubeGrid == null)
				{
					return ValidationResult.Kick | ValidationResult.Access;
				}
				MyCubeGrid cubeGrid = Block.CubeGrid;
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (myIdentity == null || myIdentity.Character == null)
				{
					return ValidationResult.Kick | ValidationResult.Access;
				}
				MyCharacterReplicable myCharacterReplicable = MyExternalReplicable.FindByObject(myIdentity.Character) as MyCharacterReplicable;
				if (myCharacterReplicable == null)
				{
					return ValidationResult.Kick | ValidationResult.Access;
				}
				Vector3D position = myIdentity.Character.PositionComp.GetPosition();
				MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(cubeGrid);
				bool flag = MyReplicableRightsValidator.GetAccess(myCharacterReplicable, position, cubeGrid, group, physical: true);
				if (!flag)
				{
					myCharacterReplicable.GetDependencies(forPlayer: true);
					flag |= MyReplicableRightsValidator.GetAccess(myCharacterReplicable, position, cubeGrid, group, physical: false);
				}
				if (!flag)
				{
					return ValidationResult.Access;
				}
			}
			return validationResult;
		}

		public override IMyReplicable GetParent()
		{
			return m_parent;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			stream.WriteInt64(Block.EntityId);
			return true;
		}

		private MySyncedBlock FindBlock()
		{
			MyEntities.TryGetEntityById(m_blockEntityId, out MySyncedBlock entity);
			if (entity != null && entity.GetTopMostParent().MarkedForClose)
			{
				return null;
			}
			return entity;
		}

		protected override void OnLoad(BitStream stream, Action<MySyncedBlock> loadingDoneHandler)
		{
			if (stream != null)
			{
				m_blockEntityId = stream.ReadInt64();
			}
			MyEntities.CallAsync(delegate
			{
				loadingDoneHandler(FindBlock());
			});
		}

		public override void OnDestroyClient()
		{
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			resultList.Add(m_propertySync);
		}

		public override BoundingBoxD GetAABB()
		{
			return BoundingBoxD.CreateInvalid();
		}
	}
}
