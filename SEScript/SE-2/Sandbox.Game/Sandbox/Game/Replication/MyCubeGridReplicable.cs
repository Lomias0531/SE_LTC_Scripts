using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRage;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyCubeGridReplicable : MyEntityReplicableBaseEvent<MyCubeGrid>, IMyStreamableReplicable
	{
		private Action<MyCubeGrid> m_loadingDoneHandler;

		private MyStreamingEntityStateGroup<MyCubeGridReplicable> m_streamingGroup;

		private readonly HashSet<IMyReplicable> m_dependencies = new HashSet<IMyReplicable>();

		private readonly List<MyCubeGrid> m_tmpCubeGrids = new List<MyCubeGrid>();

		private MyPropertySyncStateGroup m_propertySync;

		private MyCubeGrid Grid => base.Instance;

		public bool NeedsToBeStreamed
		{
			get
			{
				if (Sync.IsServer)
				{
					return !Grid.IsSplit;
				}
				return m_streamingGroup != null;
			}
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			if (Grid.IsSplit)
			{
				stream.WriteBool(value: true);
				stream.WriteInt64(Grid.EntityId);
				return true;
			}
			return false;
		}

		protected override void OnLoad(BitStream stream, Action<MyCubeGrid> loadingDoneHandler)
		{
			if (stream.ReadBool())
			{
				long gridId = stream.ReadInt64();
				Action<MyCubeGrid> findGrid = null;
				findGrid = delegate(MyCubeGrid grid)
				{
					if (grid.EntityId == gridId)
					{
						loadingDoneHandler(grid);
						MyCubeGrid.OnSplitGridCreated -= findGrid;
					}
				};
				MyCubeGrid.OnSplitGridCreated += findGrid;
				return;
			}
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
			TryRemoveExistingEntity(myObjectBuilder_EntityBase.EntityId);
			MyCubeGrid grid2 = MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_EntityBase) as MyCubeGrid;
			bool fadeIn = false;
			if (myObjectBuilder_EntityBase.PositionAndOrientation.HasValue && (myObjectBuilder_EntityBase.PositionAndOrientation.Value.Position - MySector.MainCamera.Position).LengthSquared() > 1000000.0)
			{
				fadeIn = true;
			}
			byte islandIndex = stream.ReadByte();
			double serializationTimestamp = stream.ReadDouble();
			MyEntities.InitAsync(grid2, myObjectBuilder_EntityBase, addToScene: true, delegate
			{
				loadingDoneHandler(grid2);
			}, islandIndex, serializationTimestamp, fadeIn);
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			if (m_streamingGroup != null)
			{
				resultList.Add(m_streamingGroup);
			}
			base.GetStateGroups(resultList);
			resultList.Add(m_propertySync);
		}

		protected override void OnHook()
		{
			base.OnHook();
			m_propertySync = new MyPropertySyncStateGroup(this, Grid.SyncType)
			{
				GlobalValidate = ((MyEventContext context) => HasRights(context.ClientState.EndpointId.Id, ValidationType.Access | ValidationType.Controlled))
			};
		}

		public void OnLoadBegin(Action<bool> loadingDoneHandler)
		{
			m_loadingDoneHandler = delegate(MyCubeGrid instance)
			{
				OnLoadDone(instance, loadingDoneHandler);
			};
		}

		public void CreateStreamingStateGroup()
		{
			m_streamingGroup = new MyStreamingEntityStateGroup<MyCubeGridReplicable>(this, this);
		}

		public IMyStateGroup GetStreamingStateGroup()
		{
			return m_streamingGroup;
		}

		public void Serialize(BitStream stream, HashSet<string> cachedData, Endpoint forClient, Action writeData)
		{
			if (!Grid.Closed)
			{
				stream.WriteBool(value: false);
				MyObjectBuilder_EntityBase builder = Grid.GetObjectBuilder();
				byte replicableIsland = MyMultiplayer.GetReplicationServer().GetClientReplicableIslandIndex(this, forClient);
				double time = MyMultiplayer.GetReplicationServer().GetClientRelevantServerTimestamp(forClient).Milliseconds;
				Parallel.Start(delegate
				{
					try
					{
						MySerializer.Write(stream, ref builder, MyObjectBuilderSerializer.Dynamic);
					}
					catch (Exception)
					{
						XmlSerializer serializer = MyXmlSerializerManager.GetSerializer(builder.GetType());
						MyLog.Default.WriteLine("Grid data - START");
						try
						{
							serializer.Serialize(MyLog.Default.GetTextWriter(), builder);
						}
						catch
						{
							MyLog.Default.WriteLine("Failed");
						}
						MyLog.Default.WriteLine("Grid data - END");
						throw;
					}
					stream.WriteByte(replicableIsland);
					stream.WriteDouble(time);
					writeData();
				});
			}
		}

		public void LoadDone(BitStream stream)
		{
			OnLoad(stream, m_loadingDoneHandler);
		}

		public void LoadCancel()
		{
			m_loadingDoneHandler(null);
		}

		public override HashSet<IMyReplicable> GetDependencies(bool forPlayer)
		{
			m_dependencies.Clear();
			if (!Sync.IsServer)
			{
				return m_dependencies;
			}
			if (base.Instance == null)
			{
				return m_dependencies;
			}
			foreach (MyLaserReceiver laserReceiver in base.Instance.GridSystems.RadioSystem.LaserReceivers)
			{
				foreach (MyDataBroadcaster item in laserReceiver.BroadcastersInRange)
				{
					if (!item.Closed)
					{
						MyExternalReplicable myExternalReplicable = MyExternalReplicable.FindByObject(item);
						if (myExternalReplicable != null)
						{
							m_dependencies.Add(myExternalReplicable);
						}
					}
				}
			}
			return m_dependencies;
		}

		public override HashSet<IMyReplicable> GetPhysicalDependencies(MyTimeSpan timeStamp, MyReplicablesBase replicables)
		{
			HashSet<IMyReplicable> physicalDependencies = base.GetPhysicalDependencies(timeStamp, replicables);
			if (base.Instance == null)
			{
				return physicalDependencies;
			}
			MyGridPhysicalHierarchy.Static.GetGroupNodes(base.Instance, m_tmpCubeGrids);
			foreach (MyCubeGrid tmpCubeGrid in m_tmpCubeGrids)
			{
				MyExternalReplicable myExternalReplicable = MyExternalReplicable.FindByObject(tmpCubeGrid);
				if (myExternalReplicable != null)
				{
					physicalDependencies.Add(myExternalReplicable);
				}
			}
			m_tmpCubeGrids.Clear();
			return physicalDependencies;
		}

		public override ValidationResult HasRights(EndpointId endpointId, ValidationType validationFlags)
		{
			ValidationResult validationResult = ValidationResult.Passed;
			long identityId = MySession.Static.Players.TryGetIdentityId(endpointId.Value);
			if (validationFlags.HasFlag(ValidationType.Controlled))
			{
				validationResult |= MyReplicableRightsValidator.GetControlled(base.Instance, endpointId);
				if (validationResult.HasFlag(ValidationResult.Kick))
				{
					return validationResult;
				}
			}
			if ((validationFlags.HasFlag(ValidationType.Ownership) || validationFlags.HasFlag(ValidationType.BigOwner)) && !MyReplicableRightsValidator.GetBigOwner(base.Instance, endpointId, identityId, spaceMaster: false))
			{
				return ValidationResult.Kick | ValidationResult.Ownership | ValidationResult.BigOwner;
			}
			if (validationFlags.HasFlag(ValidationType.BigOwnerSpaceMaster) && !MyReplicableRightsValidator.GetBigOwner(base.Instance, endpointId, identityId, spaceMaster: true))
			{
				return ValidationResult.Kick | ValidationResult.BigOwnerSpaceMaster;
			}
			if (validationFlags.HasFlag(ValidationType.Access))
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (myIdentity == null || myIdentity.Character == null)
				{
					return ValidationResult.Kick | ValidationResult.Access;
				}
				if (Grid == null)
				{
					return ValidationResult.Kick | ValidationResult.Access;
				}
				MyCharacterReplicable myCharacterReplicable = MyExternalReplicable.FindByObject(myIdentity.Character) as MyCharacterReplicable;
				if (myCharacterReplicable == null)
				{
					return ValidationResult.Kick | ValidationResult.Access;
				}
				Vector3D position = myIdentity.Character.PositionComp.GetPosition();
				MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(Grid);
				bool flag = MyReplicableRightsValidator.GetAccess(myCharacterReplicable, position, Grid, group, physical: true);
				if (!flag)
				{
					myCharacterReplicable.GetDependencies(forPlayer: true);
					flag |= MyReplicableRightsValidator.GetAccess(myCharacterReplicable, position, Grid, group, physical: false);
				}
				if (!flag)
				{
					return ValidationResult.Access;
				}
			}
			return validationResult;
		}
	}
}
