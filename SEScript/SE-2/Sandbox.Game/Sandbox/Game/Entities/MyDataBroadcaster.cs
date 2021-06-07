using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MyDataBroadcaster : MyEntityComponentBase, IMyEventProxy, IMyEventOwner
	{
		protected sealed class OnOwnerChanged_003C_003ESystem_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyDataBroadcaster, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyDataBroadcaster @this, in long newOwner, in MyOwnershipShareModeEnum newShare, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnOwnerChanged(newOwner, newShare);
			}
		}

		protected sealed class OnNameChanged_003C_003ESystem_String : ICallSite<MyDataBroadcaster, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyDataBroadcaster @this, in string newName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnNameChanged(newName);
			}
		}

		protected sealed class UpdateRemoteControlState_003C_003ESystem_Boolean_0023System_Nullable_00601_003CSystem_Int64_003E_0023VRage_Game_MyOwnershipShareModeEnum_0023System_Nullable_00601_003CSystem_Int64_003E : ICallSite<MyDataBroadcaster, bool, long?, MyOwnershipShareModeEnum, long?, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyDataBroadcaster @this, in bool hasRemote, in long? owner, in MyOwnershipShareModeEnum sharing, in long? remoteId, in DBNull arg5, in DBNull arg6)
			{
				@this.UpdateRemoteControlState(hasRemote, owner, sharing, remoteId);
			}
		}

		protected sealed class OnUpdateHudParams_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_MyObjectBuilder_HudEntityParams_003E : ICallSite<MyDataBroadcaster, List<MyObjectBuilder_HudEntityParams>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyDataBroadcaster @this, in List<MyObjectBuilder_HudEntityParams> newHudParams, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateHudParams(newHudParams);
			}
		}

		private class Sandbox_Game_Entities_MyDataBroadcaster_003C_003EActor : IActivator, IActivator<MyDataBroadcaster>
		{
			private sealed override object CreateInstance()
			{
				return new MyDataBroadcaster();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDataBroadcaster CreateInstance()
			{
				return new MyDataBroadcaster();
			}

			MyDataBroadcaster IActivator<MyDataBroadcaster>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3D BroadcastPosition => base.Entity.PositionComp.GetPosition();

		public override string ComponentTypeDebugString => "MyDataBroadcaster";

		public MyDataReceiver Receiver
		{
			get
			{
				MyDataReceiver component = null;
				if (base.Container != null)
				{
					base.Container.TryGet(out component);
				}
				return component;
			}
		}

		public bool Closed
		{
			get
			{
				if (base.Entity != null && !base.Entity.MarkedForClose)
				{
					return base.Entity.Closed;
				}
				return true;
			}
		}

		public long Owner => TryGetEntityIdModule()?.Owner ?? 0;

		public virtual bool ShowOnHud => true;

		public bool ShowInTerminal
		{
			get
			{
				if (base.Entity is MyCharacter)
				{
					return false;
				}
				if (base.Entity is MyCubeBlock)
				{
					return true;
				}
				if (base.Entity is MyProxyAntenna)
				{
					return !(base.Entity as MyProxyAntenna).IsCharacter;
				}
				return false;
			}
		}

		public MyAntennaSystem.BroadcasterInfo Info
		{
			get
			{
				MyAntennaSystem.BroadcasterInfo result;
				if (base.Entity is MyCharacter)
				{
					result = default(MyAntennaSystem.BroadcasterInfo);
					result.EntityId = base.Entity.EntityId;
					result.Name = base.Entity.DisplayName;
					return result;
				}
				if (base.Entity is MyCubeBlock)
				{
					MyCubeGrid logicalGroupRepresentative = MyAntennaSystem.Static.GetLogicalGroupRepresentative((base.Entity as MyCubeBlock).CubeGrid);
					result = default(MyAntennaSystem.BroadcasterInfo);
					result.EntityId = logicalGroupRepresentative.EntityId;
					result.Name = logicalGroupRepresentative.DisplayName;
					return result;
				}
				if (base.Entity is MyProxyAntenna)
				{
					return (base.Entity as MyProxyAntenna).Info;
				}
				result = default(MyAntennaSystem.BroadcasterInfo);
				return result;
			}
		}

		public long AntennaEntityId => (base.Entity as MyProxyAntenna)?.AntennaEntityId ?? base.Entity.EntityId;

		public bool HasRemoteControl
		{
			get
			{
				if (base.Entity is MyCharacter)
				{
					return false;
				}
				MyCubeGrid myCubeGrid = TryGetHostingGrid();
				if (myCubeGrid != null)
				{
					return myCubeGrid.GetFatBlockCount<MyRemoteControl>() > 0;
				}
				if (base.Entity is MyProxyAntenna)
				{
					return (base.Entity as MyProxyAntenna).HasRemoteControl;
				}
				return false;
			}
		}

		public long? MainRemoteControlOwner
		{
			get
			{
				if (base.Entity is MyCharacter)
				{
					return null;
				}
				MyCubeGrid myCubeGrid = TryGetHostingGrid();
				if (myCubeGrid != null)
				{
					return GetRemoteConrolForGrid(myCubeGrid)?.OwnerId;
				}
				if (base.Entity is MyProxyAntenna)
				{
					return (base.Entity as MyProxyAntenna).MainRemoteControlOwner;
				}
				return null;
			}
		}

		public long? MainRemoteControlId
		{
			get
			{
				if (base.Entity is MyCharacter)
				{
					return null;
				}
				MyCubeGrid myCubeGrid = TryGetHostingGrid();
				if (myCubeGrid != null)
				{
					return GetRemoteConrolForGrid(myCubeGrid)?.EntityId;
				}
				if (base.Entity is MyProxyAntenna)
				{
					return (base.Entity as MyProxyAntenna).MainRemoteControlId;
				}
				return null;
			}
		}

		public MyOwnershipShareModeEnum MainRemoteControlSharing
		{
			get
			{
				if (base.Entity is MyCharacter)
				{
					return MyOwnershipShareModeEnum.None;
				}
				MyCubeGrid myCubeGrid = TryGetHostingGrid();
				if (myCubeGrid != null)
				{
					return GetRemoteConrolForGrid(myCubeGrid)?.IDModule.ShareMode ?? MyOwnershipShareModeEnum.None;
				}
				if (base.Entity is MyProxyAntenna)
				{
					return (base.Entity as MyProxyAntenna).MainRemoteControlSharing;
				}
				return MyOwnershipShareModeEnum.None;
			}
		}

		public List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			return ((MyEntity)base.Entity).GetHudParams(allowBlink);
		}

		public bool CanBeUsedByPlayer(long playerId)
		{
			return CanBeUsedByPlayer(playerId, base.Entity);
		}

		public virtual void InitProxyObjectBuilder(MyObjectBuilder_ProxyAntenna ob)
		{
			ob.HasReceiver = (Receiver != null);
			ob.IsCharacter = (base.Entity is MyCharacter);
			ob.Position = BroadcastPosition;
			ob.HudParams = new List<MyObjectBuilder_HudEntityParams>();
			foreach (MyHudEntityParams hudParam in GetHudParams(allowBlink: false))
			{
				ob.HudParams.Add(hudParam.GetObjectBuilder());
			}
			ob.InfoEntityId = Info.EntityId;
			ob.InfoName = Info.Name;
			ob.Owner = Owner;
			ob.Share = GetShare();
			ob.AntennaEntityId = base.Entity.EntityId;
			ob.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			ob.HasRemote = HasRemoteControl;
			ob.MainRemoteOwner = MainRemoteControlOwner;
			ob.MainRemoteId = MainRemoteControlId;
			ob.MainRemoteSharing = MainRemoteControlSharing;
		}

		private MyOwnershipShareModeEnum GetShare()
		{
			return TryGetEntityIdModule()?.ShareMode ?? MyOwnershipShareModeEnum.None;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (!Sync.IsServer)
			{
				return;
			}
			MyCubeBlock myCubeBlock = base.Entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				myCubeBlock.CubeGrid.OnNameChanged += RaiseNameChanged;
				MyTerminalBlock myTerminalBlock = myCubeBlock as MyTerminalBlock;
				if (myTerminalBlock != null)
				{
					myTerminalBlock.CustomNameChanged += RaiseAntennaNameChanged;
				}
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (!Sync.IsServer)
			{
				return;
			}
			MyCubeBlock myCubeBlock = base.Entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				myCubeBlock.CubeGrid.OnNameChanged -= RaiseNameChanged;
				MyTerminalBlock myTerminalBlock = myCubeBlock as MyTerminalBlock;
				if (myTerminalBlock != null)
				{
					myTerminalBlock.CustomNameChanged -= RaiseAntennaNameChanged;
				}
			}
		}

		public void RaiseOwnerChanged()
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyDataBroadcaster x) => x.OnOwnerChanged, Owner, GetShare());
				UpdateHudParams(base.Entity as MyEntity);
			}
		}

		public void RaiseNameChanged(MyCubeGrid grid)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyDataBroadcaster x) => x.OnNameChanged, Info.Name);
			}
		}

		public void RaiseAntennaNameChanged(MyTerminalBlock block)
		{
			UpdateHudParams(block);
		}

		public void UpdateRemoteControlInfo()
		{
			if (Sync.IsServer && base.Entity != null && ((MyEntity)base.Entity).IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MyDataBroadcaster x) => x.UpdateRemoteControlState, HasRemoteControl, MainRemoteControlOwner, MainRemoteControlSharing, MainRemoteControlId);
			}
		}

		public void UpdateHudParams(MyEntity entity)
		{
			if (Sync.IsServer)
			{
				List<MyObjectBuilder_HudEntityParams> list = new List<MyObjectBuilder_HudEntityParams>();
				foreach (MyHudEntityParams hudParam in entity.GetHudParams(allowBlink: false))
				{
					list.Add(hudParam.GetObjectBuilder());
				}
				MyMultiplayer.RaiseEvent(this, (MyDataBroadcaster x) => x.OnUpdateHudParams, list);
			}
		}

		[Event(null, 354)]
		[Reliable]
		[Broadcast]
		private void OnOwnerChanged(long newOwner, MyOwnershipShareModeEnum newShare)
		{
			(base.Entity as MyProxyAntenna)?.ChangeOwner(newOwner, newShare);
		}

		[Event(null, 364)]
		[Reliable]
		[Broadcast]
		private void OnNameChanged(string newName)
		{
			MyProxyAntenna myProxyAntenna = base.Entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				myProxyAntenna.Info = new MyAntennaSystem.BroadcasterInfo
				{
					EntityId = myProxyAntenna.Info.EntityId,
					Name = newName
				};
			}
		}

		[Event(null, 374)]
		[Reliable]
		[Broadcast]
		private void UpdateRemoteControlState(bool hasRemote, long? owner, MyOwnershipShareModeEnum sharing, long? remoteId)
		{
			MyProxyAntenna myProxyAntenna = base.Entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				myProxyAntenna.HasRemoteControl = hasRemote;
				myProxyAntenna.MainRemoteControlOwner = owner;
				myProxyAntenna.MainRemoteControlId = remoteId;
				myProxyAntenna.MainRemoteControlSharing = sharing;
			}
		}

		[Event(null, 389)]
		[Reliable]
		[Broadcast]
		private void OnUpdateHudParams(List<MyObjectBuilder_HudEntityParams> newHudParams)
		{
			(base.Entity as MyProxyAntenna)?.ChangeHudParams(newHudParams);
		}

		private MyCubeGrid TryGetHostingGrid()
		{
			return (base.Entity as MyCubeBlock)?.CubeGrid;
		}

		private static MyTerminalBlock GetRemoteConrolForGrid(MyCubeGrid cubeGrid)
		{
			if (cubeGrid.HasMainRemoteControl())
			{
				return cubeGrid.MainRemoteControl;
			}
			MyFatBlockReader<MyRemoteControl> fatBlocks = cubeGrid.GetFatBlocks<MyRemoteControl>();
			if (!fatBlocks.MoveNext())
			{
				return null;
			}
			MyRemoteControl current = fatBlocks.Current;
			if (!fatBlocks.MoveNext())
			{
				return current;
			}
			return null;
		}

		private MyIDModule TryGetEntityIdModule()
		{
			IMyComponentOwner<MyIDModule> myComponentOwner = base.Entity as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule component))
			{
				return component;
			}
			return null;
		}

		public static bool CanBeUsedByPlayer(long playerId, IMyEntity Entity)
		{
			MyTerminalBlock myTerminalBlock = Entity as MyTerminalBlock;
			if (myTerminalBlock != null && myTerminalBlock.HasAdminUseTerminals(playerId))
			{
				return true;
			}
			IMyComponentOwner<MyIDModule> myComponentOwner = Entity as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule component))
			{
				MyRelationsBetweenPlayerAndBlock userRelationToOwner = component.GetUserRelationToOwner(playerId);
				if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.NoOwnership || (uint)(userRelationToOwner - 3) <= 1u)
				{
					return false;
				}
			}
			return true;
		}
	}
}
