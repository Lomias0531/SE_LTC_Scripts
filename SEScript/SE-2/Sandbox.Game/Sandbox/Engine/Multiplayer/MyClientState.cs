using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
	[StaticEventOwner]
	public abstract class MyClientState : MyClientStateBase
	{
		public enum MyContextKind
		{
			None,
			Terminal,
			Inventory,
			Production,
			Building
		}

		protected sealed class AddKnownSector_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long planetId, in long sectorId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddKnownSector(planetId, sectorId);
			}
		}

		protected sealed class RemoveKnownSector_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long planetId, in long sectorId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveKnownSector(planetId, sectorId);
			}
		}

		public readonly Dictionary<long, HashSet<long>> KnownSectors = new Dictionary<long, HashSet<long>>();

		private MyEntity m_positionEntityServer;

		public MyContextKind Context
		{
			get;
			protected set;
		}

		public MyEntity ContextEntity
		{
			get;
			protected set;
		}

		public override Vector3D? Position
		{
			get
			{
				if (m_positionEntityServer == null || m_positionEntityServer.Closed)
				{
					return base.Position;
				}
				return m_positionEntityServer.WorldMatrix.Translation;
			}
			protected set
			{
				base.Position = value;
			}
		}

		public override IMyReplicable ControlledReplicable
		{
			get
			{
				MyPlayer player = this.GetPlayer();
				if (player == null)
				{
					return null;
				}
				MyCharacter character = player.Character;
				if (character == null)
				{
					return null;
				}
				return MyExternalReplicable.FindByObject(character.GetTopMostParent());
			}
		}

		public override IMyReplicable CharacterReplicable
		{
			get
			{
				MyPlayer player = this.GetPlayer();
				if (player == null)
				{
					return null;
				}
				MyCharacter character = player.Character;
				if (character == null)
				{
					return null;
				}
				return MyExternalReplicable.FindByObject(character);
			}
		}

		public override void Serialize(BitStream stream, bool outOfOrder)
		{
			if (stream.Writing)
			{
				Write(stream);
			}
			else
			{
				Read(stream, outOfOrder);
			}
		}

		public override void Update()
		{
			GetControlledEntity(out MyEntity controlledEntity, out bool hasControl);
			if (hasControl)
			{
				controlledEntity?.ApplyLastControls();
			}
			UpdateConrtolledEntityStates(controlledEntity, hasControl);
		}

		private void UpdateConrtolledEntityStates(MyEntity controlledEntity, bool hasControl)
		{
			if (hasControl && controlledEntity != null)
			{
				MyCharacter myCharacter = controlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					base.IsControllingCharacter = !myCharacter.JetpackRunning;
					base.IsControllingJetpack = myCharacter.JetpackRunning;
					base.IsControllingGrid = false;
				}
				else
				{
					base.IsControllingCharacter = false;
					base.IsControllingJetpack = false;
					base.IsControllingGrid = (controlledEntity is MyCubeGrid);
				}
			}
			else
			{
				bool flag2 = base.IsControllingGrid = false;
				bool isControllingCharacter = base.IsControllingJetpack = flag2;
				base.IsControllingCharacter = isControllingCharacter;
			}
		}

		private void GetControlledEntity(out MyEntity controlledEntity, out bool hasControl)
		{
			controlledEntity = null;
			hasControl = false;
			if (!Sync.IsServer && base.EndpointId.Index == 0 && MySession.Static.HasCreativeRights && MySession.Static.CameraController == MySpectatorCameraController.Static && (MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.UserControlled || MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.Orbit))
			{
				MyCharacter myCharacter = MySession.Static.TopMostControlledEntity as MyCharacter;
				if (myCharacter == null || !myCharacter.UpdateRotationsOverride)
				{
					return;
				}
			}
			foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity2 in Sync.Players.ControlledEntities)
			{
				if (controlledEntity2.Value == new MyPlayer.PlayerId(base.EndpointId.Id.Value, base.EndpointId.Index))
				{
					controlledEntity = MyEntities.GetEntityById(controlledEntity2.Key);
					if (controlledEntity != null)
					{
						MyEntity topMostParent = controlledEntity.GetTopMostParent();
						MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(topMostParent);
						if (controllingPlayer != null && controlledEntity2.Value == controllingPlayer.Id)
						{
							controlledEntity = topMostParent;
						}
						break;
					}
				}
			}
			if (controlledEntity != null)
			{
				if (!Sync.IsServer)
				{
					MyPlayer player = this.GetPlayer();
					hasControl = (MySession.Static.LocalHumanPlayer == player);
				}
				else
				{
					hasControl = true;
				}
			}
		}

		private void Write(BitStream stream)
		{
			MyEntity controlledEntity = null;
			bool hasControl = false;
			if (base.PlayerSerialId > 0)
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(base.EndpointId.Id.Value, base.PlayerSerialId));
				if (playerById.Controller.ControlledEntity != null)
				{
					controlledEntity = playerById.Controller.ControlledEntity.Entity.GetTopMostParent();
					hasControl = true;
				}
			}
			else
			{
				GetControlledEntity(out controlledEntity, out hasControl);
			}
			WriteShared(stream, controlledEntity ?? MySession.Static.CameraController.Entity, hasControl);
			int bitPosition = stream.BitPosition;
			stream.WriteInt16(16);
			if (controlledEntity != null)
			{
				WriteInternal(stream, controlledEntity);
				controlledEntity.SerializeControls(stream);
				int bitPosition2 = stream.BitPosition;
				short value = (short)(stream.BitPosition - bitPosition);
				stream.SetBitPositionWrite(bitPosition);
				stream.WriteInt16(value);
				stream.SetBitPositionWrite(bitPosition2);
			}
			stream.WriteInt16(base.Ping);
		}

		private void Read(BitStream stream, bool outOfOrder)
		{
			ReadShared(stream, out MyEntity controlledEntity);
			int bitPosition = stream.BitPosition;
			short num = stream.ReadInt16();
			bool flag = controlledEntity != null;
			if (flag)
			{
				MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(controlledEntity);
				flag &= (controllingPlayer != null && controllingPlayer.Client.SteamUserId == base.EndpointId.Id.Value);
			}
			if (flag)
			{
				ReadInternal(stream, controlledEntity);
				controlledEntity.DeserializeControls(stream, outOfOrder);
			}
			else
			{
				stream.SetBitPositionRead(bitPosition + num);
			}
			base.Ping = stream.ReadInt16();
		}

		protected abstract void WriteInternal(BitStream stream, MyEntity controlledEntity);

		protected abstract void ReadInternal(BitStream stream, MyEntity controlledEntity);

		private void WriteShared(BitStream stream, MyEntity controlledEntity, bool hasControl)
		{
			stream.WriteBool(controlledEntity != null);
			if (controlledEntity == null)
			{
				if (!MySpectatorCameraController.Static.Initialized)
				{
					stream.WriteBool(value: false);
					return;
				}
				stream.WriteBool(value: true);
				Vector3D vec = MySpectatorCameraController.Static.Position;
				stream.Serialize(ref vec);
			}
			else
			{
				stream.WriteInt64(controlledEntity.EntityId);
				stream.WriteBool(hasControl);
			}
		}

		private void ReadShared(BitStream stream, out MyEntity controlledEntity)
		{
			controlledEntity = null;
			bool flag = stream.ReadBool();
			if (!flag)
			{
				if (stream.ReadBool())
				{
					Vector3D vec = Vector3D.Zero;
					stream.Serialize(ref vec);
					m_positionEntityServer = null;
					Position = vec;
				}
			}
			else
			{
				long entityId = stream.ReadInt64();
				bool flag2 = stream.ReadBool();
				if (!MyEntities.TryGetEntityById(entityId, out MyEntity entity, allowClosed: true) || entity.GetTopMostParent().MarkedForClose)
				{
					m_positionEntityServer = null;
					return;
				}
				m_positionEntityServer = entity;
				if (!flag2 || !(entity.SyncObject is MySyncEntity))
				{
					return;
				}
				controlledEntity = entity;
			}
			UpdateConrtolledEntityStates(controlledEntity, flag);
		}

		public override void ResetControlledEntityControls()
		{
			GetControlledEntity(out MyEntity controlledEntity, out bool _);
			controlledEntity?.ResetControls();
		}

		[Event(null, 295)]
		[Reliable]
		[Server]
		public static void AddKnownSector(long planetId, long sectorId)
		{
			MyReplicationServer replicationServer = MyMultiplayer.GetReplicationServer();
			if (replicationServer == null)
			{
				return;
			}
			MyClientState myClientState = (MyClientState)replicationServer.GetClientData(new Endpoint(MyEventContext.Current.Sender, 0));
			if (myClientState != null)
			{
				if (!myClientState.KnownSectors.TryGetValue(planetId, out HashSet<long> value))
				{
					value = new HashSet<long>();
					myClientState.KnownSectors.Add(planetId, value);
				}
				value.Add(sectorId);
			}
		}

		[Event(null, 320)]
		[Reliable]
		[Server]
		public static void RemoveKnownSector(long planetId, long sectorId)
		{
			MyReplicationServer replicationServer = MyMultiplayer.GetReplicationServer();
			if (replicationServer == null)
			{
				return;
			}
			MyClientState myClientState = (MyClientState)replicationServer.GetClientData(new Endpoint(MyEventContext.Current.Sender, 0));
			if (myClientState != null && myClientState.KnownSectors.TryGetValue(planetId, out HashSet<long> value))
			{
				value.Remove(sectorId);
				if (value.Count == 0)
				{
					myClientState.KnownSectors.Remove(planetId);
				}
			}
		}
	}
}
