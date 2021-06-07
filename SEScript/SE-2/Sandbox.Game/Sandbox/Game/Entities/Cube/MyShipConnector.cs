using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ShipConnector))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyShipConnector),
		typeof(Sandbox.ModAPI.Ingame.IMyShipConnector)
	})]
	public class MyShipConnector : MyFunctionalBlock, IMyInventoryOwner, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyShipConnector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipConnector
	{
		[Serializable]
		private struct State
		{
			protected class Sandbox_Game_Entities_Cube_MyShipConnector_003C_003EState_003C_003EIsMaster_003C_003EAccessor : IMemberAccessor<State, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in bool value)
				{
					owner.IsMaster = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out bool value)
				{
					value = owner.IsMaster;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MyShipConnector_003C_003EState_003C_003EOtherEntityId_003C_003EAccessor : IMemberAccessor<State, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in long value)
				{
					owner.OtherEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out long value)
				{
					value = owner.OtherEntityId;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MyShipConnector_003C_003EState_003C_003EMasterToSlave_003C_003EAccessor : IMemberAccessor<State, MyDeltaTransform?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in MyDeltaTransform? value)
				{
					owner.MasterToSlave = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out MyDeltaTransform? value)
				{
					value = owner.MasterToSlave;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MyShipConnector_003C_003EState_003C_003EMasterToSlaveGrid_003C_003EAccessor : IMemberAccessor<State, MyDeltaTransform?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref State owner, in MyDeltaTransform? value)
				{
					owner.MasterToSlaveGrid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref State owner, out MyDeltaTransform? value)
				{
					value = owner.MasterToSlaveGrid;
				}
			}

			public static readonly State Detached = default(State);

			public static readonly State DetachedMaster = new State
			{
				IsMaster = true
			};

			public bool IsMaster;

			public long OtherEntityId;

			public MyDeltaTransform? MasterToSlave;

			public MyDeltaTransform? MasterToSlaveGrid;
		}

		private enum Mode
		{
			Ejector,
			Connector
		}

		protected sealed class TradingEnabled_RequestChange_003C_003ESystem_Boolean : ICallSite<MyShipConnector, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipConnector @this, in bool value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TradingEnabled_RequestChange(value);
			}
		}

		protected sealed class TryConnect_003C_003E : ICallSite<MyShipConnector, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipConnector @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TryConnect();
			}
		}

		protected sealed class TryDisconnect_003C_003E : ICallSite<MyShipConnector, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipConnector @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TryDisconnect();
			}
		}

		protected sealed class NotifyDisconnectTime_003C_003ESystem_Boolean : ICallSite<MyShipConnector, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipConnector @this, in bool setTradingProtection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.NotifyDisconnectTime(setTradingProtection);
			}
		}

		protected sealed class PlayActionSound_003C_003E : ICallSite<MyShipConnector, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipConnector @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PlayActionSound();
			}
		}

		protected class TradingEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType tradingEnabled;
				ISyncType result = tradingEnabled = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyShipConnector)P_0).TradingEnabled = (Sync<bool, SyncDirection.FromServer>)tradingEnabled;
				return result;
			}
		}

		protected class TimeOfConnection_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType timeOfConnection;
				ISyncType result = timeOfConnection = new Sync<int, SyncDirection.FromServer>(P_1, P_2);
				((MyShipConnector)P_0).TimeOfConnection = (Sync<int, SyncDirection.FromServer>)timeOfConnection;
				return result;
			}
		}

		protected class AutoUnlockTime_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoUnlockTime;
				ISyncType result = autoUnlockTime = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyShipConnector)P_0).AutoUnlockTime = (Sync<float, SyncDirection.BothWays>)autoUnlockTime;
				return result;
			}
		}

		protected class ThrowOut_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType throwOut;
				ISyncType result = throwOut = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyShipConnector)P_0).ThrowOut = (Sync<bool, SyncDirection.BothWays>)throwOut;
				return result;
			}
		}

		protected class CollectAll_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType collectAll;
				ISyncType result = collectAll = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyShipConnector)P_0).CollectAll = (Sync<bool, SyncDirection.BothWays>)collectAll;
				return result;
			}
		}

		protected class Strength_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType strength;
				ISyncType result = strength = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyShipConnector)P_0).Strength = (Sync<float, SyncDirection.BothWays>)strength;
				return result;
			}
		}

		protected class m_connectionState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType connectionState;
				ISyncType result = connectionState = new Sync<State, SyncDirection.FromServer>(P_1, P_2);
				((MyShipConnector)P_0).m_connectionState = (Sync<State, SyncDirection.FromServer>)connectionState;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyShipConnector_003C_003EActor : IActivator, IActivator<MyShipConnector>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipConnector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipConnector CreateInstance()
			{
				return new MyShipConnector();
			}

			MyShipConnector IActivator<MyShipConnector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyTimeSpan DisconnectSleepTime = MyTimeSpan.FromSeconds(4.0);

		private const float MinStrength = 1E-06f;

		public readonly Sync<bool, SyncDirection.FromServer> TradingEnabled;

		public readonly Sync<int, SyncDirection.FromServer> TimeOfConnection;

		public readonly Sync<float, SyncDirection.BothWays> AutoUnlockTime;

		public readonly Sync<bool, SyncDirection.BothWays> ThrowOut;

		public readonly Sync<bool, SyncDirection.BothWays> CollectAll;

		public readonly Sync<float, SyncDirection.BothWays> Strength;

		private readonly Sync<State, SyncDirection.FromServer> m_connectionState;

		private MyAttachableConveyorEndpoint m_attachableConveyorEndpoint;

		private int m_update10Counter;

		private bool m_canReloadDummies = true;

		private Vector3 m_connectionPosition;

		private float m_detectorRadius;

		private static readonly int TRADING_FRAMES_TO_WAIT_AFTER_DISCONNECT = 9;

		private int m_tradingBlockTimer;

		private HkConstraint m_constraint;

		private MyShipConnector m_other;

		private bool m_defferedDisconnect;

		private static HashSet<MySlimBlock> m_tmpBlockSet = new HashSet<MySlimBlock>();

		private int m_manualDisconnectTime;

		private MyPhysicsBody m_connectorDummy;

		private Mode m_connectorMode;

		private bool m_hasConstraint;

		private MyConcurrentHashSet<MyEntity> m_detectedFloaters = new MyConcurrentHashSet<MyEntity>();

		private MyConcurrentHashSet<MyEntity> m_detectedGrids = new MyConcurrentHashSet<MyEntity>();

		protected HkConstraint m_connectorConstraint;

		protected HkFixedConstraintData m_connectorConstraintsData;

		protected HkConstraint m_ejectorConstraint;

		protected HkFixedConstraintData m_ejectorConstraintsData;

		private Matrix m_connectorDummyLocal;

		private Vector3 m_connectorCenter;

		private Vector3 m_connectorHalfExtents;

		private bool m_isMaster;

		private bool m_welded;

		private bool m_welding;

		private bool m_isInitOnceBeforeFrameUpdate;

		private long? m_lastAttachedOther;

		private long? m_lastWeldedOther;

		protected static MyTerminalControlButton<MyShipConnector> LockButton;

		protected static MyTerminalControlButton<MyShipConnector> UnlockButton;

		public MyShipConnector Other => m_other;

		private MyShipConnectorDefinition ConnectorDefinition => base.BlockDefinition as MyShipConnectorDefinition;

		private bool IsMaster
		{
			get
			{
				if (!Sync.IsServer)
				{
					return m_connectionState.Value.IsMaster;
				}
				return m_isMaster;
			}
			set
			{
				m_isMaster = value;
			}
		}

		public bool IsReleasing => (double)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_manualDisconnectTime) < DisconnectSleepTime.Milliseconds;

		public bool InConstraint => m_constraint != null;

		public bool Connected
		{
			get;
			set;
		}

		private Vector3 ConnectionPosition => Vector3.Transform(m_connectionPosition, base.PositionComp.LocalMatrix);

		public int DetectedGridCount => m_detectedGrids.Count;

		IMyConveyorEndpoint IMyConveyorEndpointBlock.ConveyorEndpoint => m_attachableConveyorEndpoint;

		bool Sandbox.ModAPI.Ingame.IMyShipConnector.ThrowOut
		{
			get
			{
				return ThrowOut;
			}
			set
			{
				ThrowOut.Value = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipConnector.CollectAll
		{
			get
			{
				return CollectAll;
			}
			set
			{
				CollectAll.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyShipConnector.PullStrength
		{
			get
			{
				return Strength;
			}
			set
			{
				if (m_connectorMode == Mode.Connector)
				{
					value = MathHelper.Clamp(value, 1E-06f, 1f);
					Strength.Value = value;
				}
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipConnector.IsLocked
		{
			get
			{
				if (base.IsWorking)
				{
					return InConstraint;
				}
				return false;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipConnector.IsConnected => Connected;

		MyShipConnectorStatus Sandbox.ModAPI.Ingame.IMyShipConnector.Status
		{
			get
			{
				if (Connected)
				{
					return MyShipConnectorStatus.Connected;
				}
				if (base.IsWorking && InConstraint)
				{
					return MyShipConnectorStatus.Connectable;
				}
				return MyShipConnectorStatus.Unconnected;
			}
		}

		Sandbox.ModAPI.Ingame.IMyShipConnector Sandbox.ModAPI.Ingame.IMyShipConnector.OtherConnector => m_other;

		Sandbox.ModAPI.IMyShipConnector Sandbox.ModAPI.IMyShipConnector.OtherConnector => m_other;

		public bool UseConveyorSystem
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public MyShipConnector()
		{
			CreateTerminalControls();
			m_connectionState.ValueChanged += delegate
			{
				OnConnectionStateChanged();
			};
			m_connectionState.AlwaysReject();
			m_manualDisconnectTime = -(int)DisconnectSleepTime.Milliseconds;
			Strength.Validate = ((float o) => (float)Strength >= 0f && (float)Strength <= 1f);
			if (!Sync.IsServer)
			{
				base.NeedsWorldMatrix = true;
			}
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyShipConnector>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyShipConnector> obj = new MyTerminalControlOnOffSwitch<MyShipConnector>("ThrowOut", MySpaceTexts.Terminal_ThrowOut)
				{
					Getter = ((MyShipConnector block) => block.ThrowOut),
					Setter = delegate(MyShipConnector block, bool value)
					{
						block.ThrowOut.Value = value;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlOnOffSwitch<MyShipConnector> obj2 = new MyTerminalControlOnOffSwitch<MyShipConnector>("CollectAll", MySpaceTexts.Terminal_CollectAll)
				{
					Getter = ((MyShipConnector block) => block.CollectAll),
					Setter = delegate(MyShipConnector block, bool value)
					{
						block.CollectAll.Value = value;
					}
				};
				obj2.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlOnOffSwitch<MyShipConnector> obj3 = new MyTerminalControlOnOffSwitch<MyShipConnector>("Trading", MySpaceTexts.Terminal_Trading, MySpaceTexts.Terminal_Trading_Tooltip)
				{
					Getter = ((MyShipConnector block) => block.TradingEnabled),
					Setter = TradingEnabled_UIChanged,
					Enabled = ((MyShipConnector block) => !block.Connected && block.m_connectorMode == Mode.Connector),
					Visible = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector)
				};
				obj3.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj3);
				MyTerminalControlSlider<MyShipConnector> obj4 = new MyTerminalControlSlider<MyShipConnector>("AutoUnlockTime", MySpaceTexts.BlockPropertyTitle_Connector_AutoUnlockTime, MySpaceTexts.BlockPropertyDescription_Connector_AutoUnlockTime)
				{
					Getter = ((MyShipConnector x) => x.AutoUnlockTime),
					Setter = delegate(MyShipConnector x, float v)
					{
						x.AutoUnlockTime.Value = v;
					},
					DefaultValue = 0f
				};
				obj4.EnableActions(0.05f, (MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				obj4.Enabled = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				obj4.Visible = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				obj4.SetLimits((MyShipConnector x) => (x.ConnectorDefinition == null) ? 0f : x.ConnectorDefinition.AutoUnlockTime_Min, (MyShipConnector x) => (x.ConnectorDefinition == null) ? 3600f : x.ConnectorDefinition.AutoUnlockTime_Max);
				obj4.Writer = delegate(MyShipConnector x, StringBuilder result)
				{
					int num = (int)(float)x.AutoUnlockTime;
					if (num == 0)
					{
						result.Append("Never");
					}
					else
					{
						MyValueFormatter.AppendTimeExact(num, result);
					}
				};
				MyTerminalControlFactory.AddControl(obj4);
				LockButton = new MyTerminalControlButton<MyShipConnector>("Lock", MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.Blank, delegate(MyShipConnector b)
				{
					b.TryConnect();
				});
				LockButton.Enabled = ((MyShipConnector b) => b.IsWorking && b.InConstraint);
				LockButton.Visible = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				LockButton.EnableAction().Enabled = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				MyTerminalControlFactory.AddControl(LockButton);
				UnlockButton = new MyTerminalControlButton<MyShipConnector>("Unlock", MySpaceTexts.BlockActionTitle_Unlock, MySpaceTexts.Blank, delegate(MyShipConnector b)
				{
					b.TryDisconnect();
				});
				UnlockButton.Enabled = ((MyShipConnector b) => b.IsWorking && b.InConstraint);
				UnlockButton.Visible = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				UnlockButton.EnableAction().Enabled = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				MyTerminalControlFactory.AddControl(UnlockButton);
				StringBuilder name = MyTexts.Get(MySpaceTexts.BlockActionTitle_SwitchLock);
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyShipConnector>("SwitchLock", name, MyTerminalActionIcons.TOGGLE)
				{
					Action = delegate(MyShipConnector b)
					{
						b.TrySwitch();
					},
					Writer = delegate(MyShipConnector b, StringBuilder sb)
					{
						b.WriteLockStateValue(sb);
					},
					Enabled = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector)
				});
				MyTerminalControlSlider<MyShipConnector> myTerminalControlSlider = new MyTerminalControlSlider<MyShipConnector>("Strength", MySpaceTexts.BlockPropertyTitle_Connector_Strength, MySpaceTexts.BlockPropertyDescription_Connector_Strength);
				myTerminalControlSlider.Getter = ((MyShipConnector x) => (float)x.Strength * 100f);
				myTerminalControlSlider.Setter = delegate(MyShipConnector x, float v)
				{
					x.Strength.Value = v * 0.01f;
				};
				myTerminalControlSlider.DefaultValue = 0.00015f;
				myTerminalControlSlider.SetLogLimits(1E-06f, 1f);
				myTerminalControlSlider.EnableActions(0.05f, (MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				myTerminalControlSlider.Enabled = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				myTerminalControlSlider.Visible = ((MyShipConnector b) => b.m_connectorMode == Mode.Connector);
				myTerminalControlSlider.SetLimits((MyShipConnector x) => 0f, (MyShipConnector x) => 100f);
				myTerminalControlSlider.Writer = delegate(MyShipConnector x, StringBuilder result)
				{
					if ((float)x.Strength <= 1E-06f)
					{
						result.Append((object)MyTexts.Get(MyCommonTexts.Disabled));
					}
					else
					{
						result.AppendFormatedDecimal("", (float)x.Strength * 100f, 4, " %");
					}
				};
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

		private void TradingEnabled_UIChanged(MyShipConnector block, bool value)
		{
			MyMultiplayer.RaiseEvent(block, (MyShipConnector x) => x.TradingEnabled_RequestChange, value);
		}

		[Event(null, 261)]
		[Reliable]
		[Server]
		public void TradingEnabled_RequestChange(bool value)
		{
			if (!Connected)
			{
				TradingEnabled.ValidateAndSet(value);
			}
		}

		private void UpdateTerminalVisuals()
		{
			LockButton.UpdateVisual();
			UnlockButton.UpdateVisual();
		}

		private void OnConnectionStateChanged()
		{
			UpdateTerminalVisuals();
			if (!Sync.IsServer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				if ((Connected || InConstraint) && m_connectionState.Value.MasterToSlave.HasValue)
				{
					Detach(synchronize: false);
				}
			}
		}

		public void WriteLockStateValue(StringBuilder sb)
		{
			if (InConstraint && Connected)
			{
				sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Locked));
			}
			else if (InConstraint)
			{
				sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_ReadyToLock));
			}
			else
			{
				sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Unlocked));
			}
		}

		public void TrySwitch()
		{
			if (InConstraint)
			{
				if (Connected)
				{
					TryDisconnect();
				}
				else
				{
					TryConnect();
				}
			}
		}

		[Event(null, 312)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public void TryConnect()
		{
			if (!InConstraint || Connected)
			{
				return;
			}
			if (Sync.IsServer)
			{
				if (m_tradingBlockTimer <= 0)
				{
					Weld();
				}
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyShipConnector x) => x.TryConnect);
			}
		}

		public bool IsProtectedFromLockingByTrading()
		{
			return m_tradingBlockTimer > 0;
		}

		public int GetProtectionFromLockingTime()
		{
			return (int)(100f * (float)m_tradingBlockTimer / 60f);
		}

		[Event(null, 337)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public void TryDisconnect()
		{
			if (!InConstraint || !Connected)
			{
				return;
			}
			m_manualDisconnectTime = (m_other.m_manualDisconnectTime = MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (Sync.IsServer)
			{
				bool arg = false;
				if (TradingEnabled.Value || Other.TradingEnabled.Value)
				{
					SetTradingProtection();
					Other.SetTradingProtection();
					arg = true;
				}
				Detach();
				MyMultiplayer.RaiseEvent(this, (MyShipConnector x) => x.NotifyDisconnectTime, arg);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyShipConnector x) => x.TryDisconnect);
			}
		}

		private void SetTradingProtection()
		{
			m_tradingBlockTimer = TRADING_FRAMES_TO_WAIT_AFTER_DISCONNECT;
		}

		[Event(null, 367)]
		[Reliable]
		[Broadcast]
		public void NotifyDisconnectTime(bool setTradingProtection = false)
		{
			if (setTradingProtection)
			{
				SetTradingProtection();
			}
			if (m_other != null)
			{
				if (setTradingProtection)
				{
					Other.SetTradingProtection();
				}
				m_manualDisconnectTime = (m_other.m_manualDisconnectTime = MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected float GetEffectiveStrength(MyShipConnector otherConnector)
		{
			float num = 0f;
			if (!IsReleasing && m_tradingBlockTimer <= 0)
			{
				num = Math.Min(Strength, otherConnector.Strength);
				if (num < 1E-06f)
				{
					num = 1E-06f;
				}
			}
			return num;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			float num = 0.001f;
			if (cubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				num *= 0.01f;
			}
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute("Conveyors"), num, () => (!base.CheckIsWorking()) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_ShipConnector myObjectBuilder_ShipConnector = objectBuilder as MyObjectBuilder_ShipConnector;
			Vector3 size = base.BlockDefinition.Size * base.CubeGrid.GridSize * 0.8f;
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = new MyInventory(size.Volume, size, MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_ShipConnector.Inventory);
			}
			ThrowOut.SetLocalValue(myObjectBuilder_ShipConnector.ThrowOut);
			CollectAll.SetLocalValue(myObjectBuilder_ShipConnector.CollectAll);
			TradingEnabled.SetLocalValue(myObjectBuilder_ShipConnector.TradingEnabled);
			AutoUnlockTime.SetLocalValue(myObjectBuilder_ShipConnector.AutoUnlockTime);
			TimeOfConnection.SetLocalValue(myObjectBuilder_ShipConnector.TimeOfConnection);
			SlimBlock.DeformationRatio = myObjectBuilder_ShipConnector.DeformationRatio;
			SlimBlock.ComponentStack.IsFunctionalChanged += UpdateReceiver;
			base.EnabledChanged += UpdateReceiver;
			base.ResourceSink.Update();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			if (base.CubeGrid.CreatePhysics)
			{
				LoadDummies();
			}
			Strength.SetLocalValue(MathHelper.Clamp(myObjectBuilder_ShipConnector.Strength, 0f, 1f));
			if (myObjectBuilder_ShipConnector.ConnectedEntityId != 0L)
			{
				MyDeltaTransform? masterToSlave = myObjectBuilder_ShipConnector.MasterToSlaveTransform.HasValue ? new MyDeltaTransform?(myObjectBuilder_ShipConnector.MasterToSlaveTransform.Value) : null;
				if (myObjectBuilder_ShipConnector.Connected)
				{
					masterToSlave = default(MyDeltaTransform);
				}
				if (!myObjectBuilder_ShipConnector.IsMaster.HasValue)
				{
					myObjectBuilder_ShipConnector.IsMaster = (myObjectBuilder_ShipConnector.ConnectedEntityId < base.EntityId);
				}
				IsMaster = myObjectBuilder_ShipConnector.IsMaster.Value;
				m_connectionState.SetLocalValue(new State
				{
					IsMaster = myObjectBuilder_ShipConnector.IsMaster.Value,
					OtherEntityId = myObjectBuilder_ShipConnector.ConnectedEntityId,
					MasterToSlave = masterToSlave,
					MasterToSlaveGrid = myObjectBuilder_ShipConnector.MasterToSlaveGrid
				});
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				m_isInitOnceBeforeFrameUpdate = true;
			}
			if (base.BlockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
			{
				base.BlockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("ConnectBlock");
			}
			base.IsWorkingChanged += MyShipConnector_IsWorkingChanged;
			base.OnPhysicsChanged += MyShipConnector_OnPhysicsChanged;
			AddDebugRenderComponent(new MyDebugRenderCompoonentShipConnector(this));
		}

		private void MyShipConnector_OnPhysicsChanged(MyEntity obj)
		{
			if (!base.MarkedForClose && base.CubeGrid.CreatePhysics && m_connectorMode == Mode.Connector && m_canReloadDummies)
			{
				LoadDummies(recreateOnlyConnector: true);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			State value = m_connectionState.Value;
			MyObjectBuilder_ShipConnector obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ShipConnector;
			obj.ThrowOut = ThrowOut;
			obj.TradingEnabled = TradingEnabled;
			obj.AutoUnlockTime = AutoUnlockTime;
			obj.TimeOfConnection = TimeOfConnection;
			obj.CollectAll = CollectAll;
			obj.Strength = Strength;
			obj.ConnectedEntityId = value.OtherEntityId;
			obj.IsMaster = value.IsMaster;
			obj.MasterToSlaveTransform = (value.MasterToSlave.HasValue ? new MyPositionAndOrientation?(value.MasterToSlave.Value) : null);
			obj.MasterToSlaveGrid = value.MasterToSlaveGrid;
			return obj;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			if (Connected)
			{
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_ConnectorDetail_Part1));
				MyValueFormatter.AppendTimeExact((MySession.Static.GameplayFrameCounter - (int)TimeOfConnection) / 60, detailedInfo);
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_ConnectorDetail_Part2));
				float val = ((float)AutoUnlockTime == 0f) ? float.MaxValue : ((float)AutoUnlockTime);
				float val2 = ((float)m_other.AutoUnlockTime == 0f) ? float.MaxValue : ((float)m_other.AutoUnlockTime);
				float num = Math.Min(val, val2);
				if (num == float.MaxValue)
				{
					detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_ConnectorDetail_Part3));
				}
				else
				{
					MyValueFormatter.AppendTimeExact((int)num, detailedInfo);
				}
			}
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (Sync.IsServer && Connected && (!base.IsFunctional || !base.Enabled))
			{
				if (IsMaster)
				{
					m_connectionState.Value = State.DetachedMaster;
				}
				else
				{
					m_connectionState.Value = State.Detached;
				}
			}
			DisposeBodyConstraint(ref m_connectorConstraint, ref m_connectorConstraintsData);
			DisposeBodyConstraint(ref m_ejectorConstraint, ref m_ejectorConstraintsData);
			if (base.Physics != null)
			{
				base.Physics.Enabled = true;
			}
			if (m_connectorDummy != null)
			{
				m_connectorDummy.Close();
				m_connectorDummy = CreatePhysicsBody(Mode.Connector, ref m_connectorDummyLocal, ref m_connectorCenter, ref m_connectorHalfExtents);
			}
			CreateBodyConstraint();
			UpdateConnectionState();
			TryReattachAfterMerge();
			UpdateTerminalVisuals();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Sync.IsServer && Connected && (float)AutoUnlockTime > 0f && (float)(int)TimeOfConnection + 60f * (float)AutoUnlockTime <= (float)MySession.Static.GameplayFrameCounter)
			{
				TryDisconnect();
			}
			m_tradingBlockTimer--;
			if (m_tradingBlockTimer == 0)
			{
				SetEmissiveStateWorking();
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void TryReattachAfterMerge()
		{
			if (base.Enabled && !InConstraint && !m_connectionState.Value.MasterToSlave.HasValue && m_lastAttachedOther.HasValue)
			{
				TryAttach(m_lastAttachedOther);
				if (m_lastWeldedOther.HasValue)
				{
					TryConnect();
				}
			}
			m_lastAttachedOther = null;
			m_lastWeldedOther = null;
		}

		private void CreateBodyConstraint()
		{
			if (m_connectorDummy != null)
			{
				m_canReloadDummies = false;
				m_connectorDummy.Enabled = true;
				m_canReloadDummies = true;
				CreateBodyConstraint(m_connectorDummy, out m_connectorConstraintsData, out m_connectorConstraint);
				base.CubeGrid.Physics.AddConstraint(m_connectorConstraint);
			}
			if (base.Physics != null)
			{
				CreateBodyConstraint(base.Physics, out m_ejectorConstraintsData, out m_ejectorConstraint);
				base.CubeGrid.Physics.AddConstraint(m_ejectorConstraint);
			}
			base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnBodyPhysicsChanged;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnBodyPhysicsChanged;
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
			}
		}

		internal void UpdateHavokCollisionSystemID(int HavokCollisionSystemID)
		{
			if (m_connectorDummy != null)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(24, HavokCollisionSystemID, 1, 1);
				m_connectorDummy.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				if (m_connectorDummy.HavokWorld != null)
				{
					m_connectorDummy.HavokWorld.RefreshCollisionFilterOnEntity(m_connectorDummy.RigidBody);
				}
			}
			if (base.Physics != null)
			{
				uint collisionFilterInfo2 = HkGroupFilter.CalcFilterInfo(26, HavokCollisionSystemID, 1, 1);
				base.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo2);
				if (base.Physics.HavokWorld != null)
				{
					base.Physics.HavokWorld.RefreshCollisionFilterOnEntity(base.Physics.RigidBody);
				}
			}
		}

		private void MyShipConnector_IsWorkingChanged(MyCubeBlock obj)
		{
			if (Sync.IsServer && Connected && (!base.IsFunctional || !base.IsWorking))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		private void LoadDummies(bool recreateOnlyConnector = false)
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(base.BlockDefinition.Model).Dummies)
			{
				bool flag = dummy.Key.ToLower().Contains("connector");
				bool flag2 = flag || dummy.Key.ToLower().Contains("ejector");
				if (flag || flag2)
				{
					Matrix dummyLocal = Matrix.Normalize(dummy.Value.Matrix);
					m_connectionPosition = dummyLocal.Translation;
					dummyLocal *= base.PositionComp.LocalMatrix;
					Vector3 halfExtents = dummy.Value.Matrix.Scale / 2f;
					halfExtents = new Vector3(halfExtents.X, halfExtents.Y, halfExtents.Z);
					m_detectorRadius = halfExtents.AbsMax();
					Vector3 center = dummy.Value.Matrix.Translation;
					if (flag)
					{
						MySandboxGame.Static.Invoke(delegate
						{
							RecreateConnectorDummy(ref dummyLocal, ref center, ref halfExtents);
						}, "MyShipConnector::RecreateConnectorDummy");
					}
					if (flag2 && !recreateOnlyConnector)
					{
						DisposePhysicsBody(base.Physics);
						base.Physics = CreatePhysicsBody(Mode.Ejector, ref dummyLocal, ref center, ref halfExtents);
					}
					if (flag)
					{
						m_connectorMode = Mode.Connector;
					}
					else
					{
						m_connectorMode = Mode.Ejector;
					}
					break;
				}
			}
		}

		private void RecreateConnectorDummy(ref Matrix dummyLocal, ref Vector3 center, ref Vector3 halfExtents)
		{
			DisposeBodyConstraint(ref m_connectorConstraint, ref m_connectorConstraintsData);
			if (m_connectorDummy != null)
			{
				m_connectorDummy.Close();
			}
			m_connectorDummyLocal = dummyLocal;
			m_connectorCenter = center;
			m_connectorHalfExtents = halfExtents;
			m_connectorDummy = CreatePhysicsBody(Mode.Connector, ref dummyLocal, ref center, ref halfExtents);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private MyPhysicsBody CreatePhysicsBody(Mode mode, ref Matrix dummyLocal, ref Vector3 center, ref Vector3 halfExtents)
		{
			MyPhysicsBody myPhysicsBody = null;
			if (mode == Mode.Ejector || Sync.IsServer)
			{
				HkBvShape shape = CreateDetectorShape(halfExtents, mode);
				int collisionFilter = (mode != Mode.Connector) ? 26 : 24;
				myPhysicsBody = new MyPhysicsBody(this, RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
				myPhysicsBody.IsPhantom = true;
				myPhysicsBody.CreateFromCollisionObject(shape, center, dummyLocal, null, collisionFilter);
				myPhysicsBody.RigidBody.ContactPointCallbackEnabled = true;
				shape.Base.RemoveReference();
			}
			return myPhysicsBody;
		}

		private void DisposePhysicsBody(MyPhysicsBody body)
		{
			DisposePhysicsBody(ref body);
		}

		private void DisposePhysicsBody(ref MyPhysicsBody body)
		{
			if (body != null)
			{
				body.Close();
				body = null;
			}
		}

		private HkBvShape CreateDetectorShape(Vector3 extents, Mode mode)
		{
			if (mode != 0)
			{
				return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_EnterConnector, phantom_LeaveConnector), boundingVolumeShape: new HkSphereShape(extents.AbsMax()), policy: HkReferencePolicy.TakeOwnership);
			}
			return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_EnterEjector, phantom_LeaveEjector), boundingVolumeShape: new HkBoxShape(extents), policy: HkReferencePolicy.TakeOwnership);
		}

		private void phantom_LeaveEjector(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			bool flag = m_detectedFloaters.Count == 2;
			List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
			foreach (VRage.ModAPI.IMyEntity item in allEntities)
			{
				m_detectedFloaters.Remove((MyEntity)item);
			}
			allEntities.Clear();
			if (flag)
			{
				SetEmissiveStateWorking();
			}
		}

		private void phantom_LeaveConnector(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
			foreach (VRage.ModAPI.IMyEntity item in allEntities)
			{
				m_detectedGrids.Remove(item as MyCubeGrid);
			}
			allEntities.Clear();
		}

		private void phantom_EnterEjector(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			bool flag = false;
			List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
			foreach (VRage.ModAPI.IMyEntity item in allEntities)
			{
				if (item is MyFloatingObject)
				{
					flag |= (m_detectedFloaters.Count == 1);
					m_detectedFloaters.Add((MyFloatingObject)item);
				}
			}
			allEntities.Clear();
			if (flag)
			{
				SetEmissiveStateWorking();
			}
		}

		private void phantom_EnterConnector(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
			using (allEntities.GetClearToken())
			{
				foreach (VRage.ModAPI.IMyEntity item in allEntities)
				{
					MyCubeGrid myCubeGrid = item.GetTopMostParent() as MyCubeGrid;
					if (myCubeGrid != null && myCubeGrid != base.CubeGrid)
					{
						m_detectedGrids.Add(myCubeGrid);
					}
				}
			}
		}

		private void GetBoxFromMatrix(Matrix m, out Vector3 halfExtents, out Vector3 position, out Quaternion orientation)
		{
			halfExtents = Vector3.Zero;
			position = Vector3.Zero;
			orientation = Quaternion.Identity;
		}

		private void UpdateReceiver(MyTerminalBlock block)
		{
			base.ResourceSink.Update();
		}

		private void UpdateReceiver()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory());
			base.OnDestroy();
		}

		public override bool SetEmissiveStateWorking()
		{
			if (InConstraint)
			{
				MyShipConnector myShipConnector = this;
				if (m_other != null && m_other.IsMaster)
				{
					myShipConnector = m_other;
				}
				if (myShipConnector.Connected)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, base.Render.RenderObjectIDs[0], "Emissive");
				}
				if (m_tradingBlockTimer > 0)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0], "Emissive");
				}
				if (myShipConnector.IsReleasing)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Autolock, base.Render.RenderObjectIDs[0], "Emissive");
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Constraint, base.Render.RenderObjectIDs[0], "Emissive");
			}
			if (m_tradingBlockTimer > 0)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0], "Emissive");
			}
			if (base.IsWorking)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0], "Emissive");
			}
			if (base.IsFunctional)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0], "Emissive");
			}
			SetEmissiveStateDamaged();
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Damaged, base.Render.RenderObjectIDs[0], "Emissive");
		}

		public override bool SetEmissiveStateDisabled()
		{
			return SetEmissiveStateWorking();
		}

		private void TryAttach(long? otherConnectorId = null)
		{
			MyShipConnector myShipConnector = FindOtherConnector(otherConnectorId);
			if (myShipConnector != null && myShipConnector.FriendlyWithBlock(this) && base.CubeGrid.Physics != null && myShipConnector.CubeGrid.Physics != null)
			{
				Vector3D value = ConstraintPositionWorld();
				Vector3D value2 = myShipConnector.ConstraintPositionWorld();
				(value2 - value).LengthSquared();
				if (myShipConnector.m_connectorMode == Mode.Connector && myShipConnector.IsFunctional && (value2 - value).LengthSquared() < 0.34999999403953552)
				{
					MyShipConnector master = GetMaster(this, myShipConnector);
					master.IsMaster = true;
					if (master == this)
					{
						CreateConstraint(myShipConnector);
						myShipConnector.IsMaster = false;
					}
					else
					{
						myShipConnector.CreateConstraint(this);
						IsMaster = false;
					}
				}
			}
			else
			{
				m_connectionState.Value = State.DetachedMaster;
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (Sync.IsServer && base.IsWorking)
			{
				m_update10Counter++;
				if (m_update10Counter % 8 == 0 && base.Enabled)
				{
					if ((bool)CollectAll)
					{
						MyGridConveyorSystem.PullAllRequest(this, this.GetInventory(), base.OwnerId, all: true);
					}
					if (!InConstraint && (bool)ThrowOut && m_detectedFloaters.Count < 2)
					{
						TryThrowOutItem();
					}
				}
				if (m_detectedFloaters.Count == 0 && m_connectorMode == Mode.Connector && m_update10Counter % 4 == 0 && base.Enabled && !InConstraint && !m_connectionState.Value.MasterToSlave.HasValue)
				{
					TryAttach();
				}
			}
			else if (Sync.IsServer && !base.IsWorking)
			{
				if (InConstraint && !Connected)
				{
					Detach();
				}
				else if (InConstraint && Connected && (!base.IsFunctional || !base.Enabled))
				{
					Detach();
				}
			}
			if (base.IsWorking && InConstraint && !Connected)
			{
				float effectiveStrength = GetEffectiveStrength(m_other);
				HkMalleableConstraintData hkMalleableConstraintData = m_constraint.ConstraintData as HkMalleableConstraintData;
				if (hkMalleableConstraintData != null && hkMalleableConstraintData.Strength != effectiveStrength && IsMaster)
				{
					hkMalleableConstraintData.Strength = effectiveStrength;
					base.CubeGrid.Physics.RigidBody.Activate();
					SetEmissiveStateWorking();
					m_other.SetEmissiveStateWorking();
				}
			}
			if (Sync.IsServer && InConstraint && !Connected && m_connectorMode == Mode.Connector)
			{
				Vector3D value = ConstraintPositionWorld();
				if ((m_other.ConstraintPositionWorld() - value).LengthSquared() > 0.5)
				{
					Detach();
				}
			}
			UpdateConnectionState();
		}

		private void UpdateConnectionState()
		{
			if (m_isInitOnceBeforeFrameUpdate)
			{
				m_isInitOnceBeforeFrameUpdate = false;
			}
			else if (m_other == null && m_connectionState.Value.OtherEntityId != 0L && Sync.IsServer)
			{
				m_connectionState.Value = State.Detached;
			}
			if (!IsMaster || base.CubeGrid.Physics == null)
			{
				return;
			}
			State value = m_connectionState.Value;
			if (value.OtherEntityId == 0L)
			{
				if (InConstraint)
				{
					Detach(synchronize: false);
					SetEmissiveStateWorking();
					if (m_other != null)
					{
						m_other.SetEmissiveStateWorking();
					}
				}
				return;
			}
			if (!value.MasterToSlave.HasValue)
			{
				if (Connected || (InConstraint && m_other.EntityId != value.OtherEntityId))
				{
					Detach(synchronize: false);
					SetEmissiveStateWorking();
					if (m_other != null)
					{
						m_other.SetEmissiveStateWorking();
					}
				}
				if (InConstraint || !MyEntities.TryGetEntityById(value.OtherEntityId, out MyShipConnector entity) || !entity.FriendlyWithBlock(this) || entity.Closed || entity.MarkedForClose || base.Physics == null || entity.Physics == null)
				{
					return;
				}
				if (!Sync.IsServer && value.MasterToSlaveGrid.HasValue && base.CubeGrid != entity.CubeGrid)
				{
					if (base.CubeGrid.IsStatic)
					{
						entity.WorldMatrix = MatrixD.Multiply(MatrixD.Invert(value.MasterToSlaveGrid.Value), base.CubeGrid.WorldMatrix);
					}
					else
					{
						base.CubeGrid.WorldMatrix = MatrixD.Multiply(value.MasterToSlaveGrid.Value, entity.WorldMatrix);
					}
				}
				CreateConstraintNosync(entity);
				SetEmissiveStateWorking();
				if (m_other != null)
				{
					m_other.SetEmissiveStateWorking();
				}
				return;
			}
			if (Connected && m_other.EntityId != value.OtherEntityId)
			{
				Detach(synchronize: false);
				SetEmissiveStateWorking();
				if (m_other != null)
				{
					m_other.SetEmissiveStateWorking();
				}
			}
			MyEntities.TryGetEntityById(value.OtherEntityId, out MyShipConnector entity2);
			if (Connected || entity2 == null || !entity2.FriendlyWithBlock(this))
			{
				return;
			}
			m_other = entity2;
			MyDeltaTransform? transform = value.MasterToSlave;
			if (transform.HasValue && transform.Value.IsZero)
			{
				transform = null;
			}
			if (!Sync.IsServer && value.MasterToSlaveGrid.HasValue && base.CubeGrid != entity2.CubeGrid)
			{
				if (base.CubeGrid.IsStatic)
				{
					entity2.WorldMatrix = MatrixD.Multiply(MatrixD.Invert(value.MasterToSlaveGrid.Value), base.CubeGrid.WorldMatrix);
				}
				else
				{
					base.CubeGrid.WorldMatrix = MatrixD.Multiply(value.MasterToSlaveGrid.Value, entity2.WorldMatrix);
				}
			}
			Weld(transform);
			SetEmissiveStateWorking();
			if (m_other != null)
			{
				m_other.SetEmissiveStateWorking();
			}
		}

		private void TryThrowOutItem()
		{
			float num = (base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 2.5f : 0.5f;
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			int num2 = 0;
			MyPhysicalInventoryItem myPhysicalInventoryItem;
			MyPhysicalItemDefinition definition;
			do
			{
				if (num2 < this.GetInventory().GetItems().Count)
				{
					myPhysicalInventoryItem = items[num2];
					continue;
				}
				return;
			}
			while (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(myPhysicalInventoryItem.Content.GetId(), out definition));
			float randomFloat = MyUtils.GetRandomFloat(0f, (base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.5f : 0.07f);
			Vector3 randomVector3CircleNormalized = MyUtils.GetRandomVector3CircleNormalized();
			Vector3D position = Vector3D.Transform(ConnectionPosition, base.CubeGrid.PositionComp.WorldMatrix) + base.PositionComp.WorldMatrix.Right * randomVector3CircleNormalized.X * randomFloat + base.PositionComp.WorldMatrix.Up * randomVector3CircleNormalized.Z * randomFloat;
			MyFixedPoint myFixedPoint = (MyFixedPoint)(num / definition.Volume);
			if (myPhysicalInventoryItem.Content.TypeId != typeof(MyObjectBuilder_Ore) && myPhysicalInventoryItem.Content.TypeId != typeof(MyObjectBuilder_Ingot))
			{
				myFixedPoint = MyFixedPoint.Ceiling(myFixedPoint);
			}
			MyFixedPoint myFixedPoint2 = 0;
			MyPhysicalInventoryItem item;
			if (myPhysicalInventoryItem.Amount < myFixedPoint)
			{
				num -= (float)myPhysicalInventoryItem.Amount * definition.Volume;
				myFixedPoint2 = myPhysicalInventoryItem.Amount;
				item = myPhysicalInventoryItem;
				this.GetInventory().RemoveItems(myPhysicalInventoryItem.ItemId);
				num2++;
			}
			else
			{
				num = 0f;
				MyPhysicalInventoryItem myPhysicalInventoryItem2 = new MyPhysicalInventoryItem(myPhysicalInventoryItem.GetObjectBuilder());
				myPhysicalInventoryItem2.Amount = myFixedPoint;
				item = myPhysicalInventoryItem2;
				myFixedPoint2 = myFixedPoint;
				this.GetInventory().RemoveItems(myPhysicalInventoryItem.ItemId, myFixedPoint);
			}
			if (!(myFixedPoint2 > 0))
			{
				return;
			}
			float num3 = definition.Size.Max() * myPhysicalInventoryItem.Scale * 0.5f;
			if (myPhysicalInventoryItem.Content.TypeId == typeof(MyObjectBuilder_Ore))
			{
				string amountBasedDebrisVoxel = MyDebris.GetAmountBasedDebrisVoxel(Math.Max((float)myFixedPoint2, 50f));
				if (amountBasedDebrisVoxel != null)
				{
					MyModel modelOnlyData = MyModels.GetModelOnlyData(amountBasedDebrisVoxel);
					if (modelOnlyData != null)
					{
						num3 = modelOnlyData.BoundingBoxSizeHalf.Max();
					}
				}
			}
			position += base.PositionComp.WorldMatrix.Forward * num3;
			MyFloatingObjects.Spawn(item, position, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up, base.CubeGrid.Physics, delegate(MyEntity entity)
			{
				MyPhysicsComponentBase physics = entity.Physics;
				physics.LinearVelocity += base.PositionComp.WorldMatrix.Forward;
				if (m_soundEmitter != null)
				{
					m_soundEmitter.PlaySound(m_actionSound);
					MyMultiplayer.RaiseEvent(this, (MyShipConnector x) => x.PlayActionSound);
				}
				if (MyParticlesManager.TryCreateParticleEffect("Smoke_Collector", entity.WorldMatrix, out MyParticleEffect effect))
				{
					effect.Velocity = base.CubeGrid.Physics.LinearVelocity;
				}
			});
		}

		[Event(null, 1243)]
		[Reliable]
		[Broadcast]
		private void PlayActionSound()
		{
			m_soundEmitter.PlaySound(m_actionSound);
		}

		private MyShipConnector FindOtherConnector(long? otherConnectorId = null)
		{
			MyShipConnector entity = null;
			BoundingSphereD sphere = new BoundingSphereD(ConnectionPosition, m_detectorRadius);
			if (otherConnectorId.HasValue)
			{
				MyEntities.TryGetEntityById(otherConnectorId.Value, out entity);
			}
			else
			{
				sphere = sphere.Transform(base.CubeGrid.PositionComp.WorldMatrix);
				entity = TryFindConnectorInGrid(ref sphere, base.CubeGrid, this);
			}
			if (entity != null)
			{
				return entity;
			}
			foreach (MyEntity detectedGrid in m_detectedGrids)
			{
				if (!detectedGrid.MarkedForClose && detectedGrid is MyCubeGrid)
				{
					MyCubeGrid myCubeGrid = detectedGrid as MyCubeGrid;
					if (myCubeGrid != base.CubeGrid)
					{
						entity = TryFindConnectorInGrid(ref sphere, myCubeGrid, this);
						if (entity != null)
						{
							return entity;
						}
					}
				}
			}
			return null;
		}

		private static MyShipConnector TryFindConnectorInGrid(ref BoundingSphereD sphere, MyCubeGrid grid, MyShipConnector thisConnector = null)
		{
			m_tmpBlockSet.Clear();
			grid.GetBlocksInsideSphere(ref sphere, m_tmpBlockSet);
			foreach (MySlimBlock item in m_tmpBlockSet)
			{
				if (item.FatBlock != null && item.FatBlock is MyShipConnector)
				{
					MyShipConnector myShipConnector = item.FatBlock as MyShipConnector;
					if (!myShipConnector.InConstraint && myShipConnector != thisConnector && myShipConnector.IsWorking && myShipConnector.FriendlyWithBlock(thisConnector))
					{
						m_tmpBlockSet.Clear();
						return myShipConnector;
					}
				}
			}
			m_tmpBlockSet.Clear();
			return null;
		}

		private void CreateConstraint(MyShipConnector otherConnector)
		{
			CreateConstraintNosync(otherConnector);
			if (Sync.IsServer)
			{
				MatrixD matrix = base.CubeGrid.WorldMatrix * MatrixD.Invert(m_other.WorldMatrix);
				Sync<State, SyncDirection.FromServer> connectionState = m_connectionState;
				State value = new State
				{
					IsMaster = true,
					OtherEntityId = otherConnector.EntityId,
					MasterToSlave = null,
					MasterToSlaveGrid = matrix
				};
				connectionState.Value = value;
				Sync<State, SyncDirection.FromServer> connectionState2 = otherConnector.m_connectionState;
				value = new State
				{
					IsMaster = false,
					OtherEntityId = base.EntityId,
					MasterToSlave = null
				};
				connectionState2.Value = value;
			}
		}

		private void CreateConstraintNosync(MyShipConnector otherConnector)
		{
			Vector3 posA = ConstraintPositionInGridSpace();
			Vector3 posB = otherConnector.ConstraintPositionInGridSpace();
			Vector3 axisA = ConstraintAxisGridSpace();
			Vector3 axisB = -otherConnector.ConstraintAxisGridSpace();
			HkHingeConstraintData hkHingeConstraintData = new HkHingeConstraintData();
			hkHingeConstraintData.SetInBodySpace(posA, posB, axisA, axisB, base.CubeGrid.Physics, otherConnector.CubeGrid.Physics);
			HkMalleableConstraintData hkMalleableConstraintData = new HkMalleableConstraintData();
			hkMalleableConstraintData.SetData(hkHingeConstraintData);
			hkHingeConstraintData.ClearHandle();
			hkHingeConstraintData = null;
			hkMalleableConstraintData.Strength = GetEffectiveStrength(otherConnector);
			HkConstraint newConstraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, otherConnector.CubeGrid.Physics.RigidBody, hkMalleableConstraintData);
			SetConstraint(otherConnector, newConstraint);
			otherConnector.SetConstraint(this, newConstraint);
			AddConstraint(newConstraint);
		}

		private void SetConstraint(MyShipConnector other, HkConstraint newConstraint)
		{
			m_other = other;
			m_constraint = newConstraint;
			SetEmissiveStateWorking();
		}

		private void UnsetConstraint()
		{
			m_other = null;
			m_constraint = null;
			SetEmissiveStateWorking();
		}

		public Vector3D ConstraintPositionWorld()
		{
			return Vector3D.Transform(ConstraintPositionInGridSpace(), base.CubeGrid.PositionComp.WorldMatrix);
		}

		private Vector3 ConstraintPositionInGridSpace()
		{
			Vector3 vector = (base.Max + base.Min) * base.CubeGrid.GridSize * 0.5f;
			Vector3 value = ConnectionPosition - vector;
			value = Vector3.DominantAxisProjection(value);
			MatrixI matrix = new MatrixI(Vector3I.Zero, base.Orientation.Forward, base.Orientation.Up);
			Vector3.Transform(ref value, ref matrix, out Vector3 _);
			return vector + value;
		}

		private Vector3 ConstraintAxisGridSpace()
		{
			Vector3 value = (base.Max + base.Min) * base.CubeGrid.GridSize * 0.5f;
			return Vector3.Normalize(Vector3.DominantAxisProjection(ConnectionPosition - value));
		}

		private Vector3 ProjectPerpendicularFromWorld(Vector3 worldPerpAxis)
		{
			Vector3 vector = ConstraintAxisGridSpace();
			Vector3 vector2 = Vector3.TransformNormal(worldPerpAxis, base.CubeGrid.PositionComp.WorldMatrixNormalizedInv);
			float scaleFactor = Vector3.Dot(vector2, vector);
			Vector3.Normalize(vector2 - scaleFactor * vector);
			return Vector3.Normalize(vector2 - scaleFactor * vector);
		}

		private void Weld()
		{
			(IsMaster ? this : m_other).Weld(null);
			if (MyVisualScriptLogicProvider.ConnectorStateChanged != null && m_other != null)
			{
				MyVisualScriptLogicProvider.ConnectorStateChanged(base.EntityId, base.CubeGrid.EntityId, Name, base.CubeGrid.Name, m_other.EntityId, m_other.CubeGrid.EntityId, m_other.Name, m_other.CubeGrid.Name, isConnected: true);
			}
		}

		private void Weld(MatrixD? masterToSlave)
		{
			m_welding = true;
			m_welded = true;
			m_other.m_welded = true;
			if (!masterToSlave.HasValue)
			{
				masterToSlave = base.WorldMatrix * MatrixD.Invert(m_other.WorldMatrix);
			}
			if (m_constraint != null)
			{
				RemoveConstraint(m_other, m_constraint);
				m_constraint = null;
				m_other.m_constraint = null;
			}
			WeldInternal();
			if (Sync.IsServer)
			{
				MatrixD matrix = base.CubeGrid.WorldMatrix * MatrixD.Invert(m_other.WorldMatrix);
				Sync<State, SyncDirection.FromServer> connectionState = m_connectionState;
				State value = new State
				{
					IsMaster = true,
					OtherEntityId = m_other.EntityId,
					MasterToSlave = masterToSlave.Value,
					MasterToSlaveGrid = matrix
				};
				connectionState.Value = value;
				Sync<State, SyncDirection.FromServer> connectionState2 = m_other.m_connectionState;
				value = new State
				{
					IsMaster = false,
					OtherEntityId = base.EntityId,
					MasterToSlave = masterToSlave.Value
				};
				connectionState2.Value = value;
			}
			m_other.m_other = this;
			m_welding = false;
		}

		private void RecreateConstraintInternal()
		{
			if (m_constraint != null)
			{
				RemoveConstraint(m_other, m_constraint);
				m_constraint = null;
				m_other.m_constraint = null;
			}
			HkFixedConstraintData hkFixedConstraintData = new HkFixedConstraintData();
			MatrixD matrix = MatrixD.CreateWorld(Vector3D.Transform(ConnectionPosition, base.CubeGrid.WorldMatrix));
			Matrix pivotA = matrix * base.CubeGrid.PositionComp.WorldMatrixNormalizedInv;
			Matrix pivotB = matrix * m_other.CubeGrid.PositionComp.WorldMatrixNormalizedInv;
			hkFixedConstraintData.SetInBodySpaceInternal(ref pivotA, ref pivotB);
			hkFixedConstraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
			HkConstraint newConstraint = new HkConstraint(base.CubeGrid.Physics.RigidBody, m_other.CubeGrid.Physics.RigidBody, hkFixedConstraintData);
			SetConstraint(m_other, newConstraint);
			m_other.SetConstraint(this, newConstraint);
			AddConstraint(newConstraint);
		}

		private void WeldInternal()
		{
			if (m_attachableConveyorEndpoint.AlreadyAttached())
			{
				m_attachableConveyorEndpoint.DetachAll();
			}
			m_attachableConveyorEndpoint.Attach(m_other.m_attachableConveyorEndpoint);
			Connected = true;
			m_other.Connected = true;
			RecreateConstraintInternal();
			SetEmissiveStateWorking();
			m_other.SetEmissiveStateWorking();
			if (base.CubeGrid != m_other.CubeGrid)
			{
				if (!TradingEnabled && !m_other.TradingEnabled)
				{
					OnConstraintAdded(GridLinkTypeEnum.Logical, m_other.CubeGrid);
				}
				OnConstraintAdded(GridLinkTypeEnum.Physical, m_other.CubeGrid);
				MyFixedGrids.Link(base.CubeGrid, m_other.CubeGrid, this);
				MyGridPhysicalHierarchy.Static.CreateLink(base.EntityId, base.CubeGrid, m_other.CubeGrid);
			}
			base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			m_other.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			if (Sync.IsServer)
			{
				TimeOfConnection.ValidateAndSet(MySession.Static.GameplayFrameCounter);
				m_other.TimeOfConnection.ValidateAndSet(MySession.Static.GameplayFrameCounter);
			}
		}

		private void CubeGrid_OnBodyPhysicsChanged(MyEntity obj)
		{
			DisposeBodyConstraint(ref m_connectorConstraint, ref m_connectorConstraintsData);
			DisposeBodyConstraint(ref m_ejectorConstraint, ref m_ejectorConstraintsData);
			if (Sync.IsServer && !m_welding && InConstraint && m_hasConstraint)
			{
				RemoveConstraint(m_other, m_constraint);
				m_constraint = null;
				m_other.m_constraint = null;
				m_hasConstraint = false;
				m_other.m_hasConstraint = false;
				if (m_welded)
				{
					RecreateConstraintInternal();
				}
				else if (!m_connectionState.Value.MasterToSlave.HasValue && base.CubeGrid.Physics != null && m_other.CubeGrid.Physics != null)
				{
					CreateConstraintNosync(m_other);
				}
			}
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics = base.CubeGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Remove(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
				MyGridPhysics physics2 = base.CubeGrid.Physics;
				physics2.EnabledChanged = (Action)Delegate.Combine(physics2.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			oldGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
			oldGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_OnHavokSystemIDChanged;
			if (oldGrid.Physics != null)
			{
				MyGridPhysics physics = oldGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Remove(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics2 = base.CubeGrid.Physics;
				physics2.EnabledChanged = (Action)Delegate.Combine(physics2.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
			base.OnCubeGridChanged(oldGrid);
		}

		private void CubeGrid_OnHavokSystemIDChanged(int id)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (base.CubeGrid.Physics != null)
				{
					UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
				}
			}, "MyShipConnector::CubeGrid_OnHavokSystemIDChanged");
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			CubeGrid_OnBodyPhysicsChanged(obj);
		}

		private void AddConstraint(HkConstraint newConstraint)
		{
			m_hasConstraint = true;
			if (newConstraint.RigidBodyA != newConstraint.RigidBodyB)
			{
				base.CubeGrid.Physics.AddConstraint(newConstraint);
			}
		}

		public void Detach(bool synchronize = true)
		{
			if (!IsMaster)
			{
				if (m_other != null && m_other.IsMaster)
				{
					if (base.IsWorking && !Connected)
					{
						base.CubeGrid.Physics.RigidBody.Activate();
					}
					m_other.Detach(synchronize);
				}
			}
			else
			{
				if (!InConstraint || m_other == null)
				{
					return;
				}
				if (synchronize && Sync.IsServer)
				{
					m_connectionState.Value = State.DetachedMaster;
					m_other.m_connectionState.Value = State.Detached;
				}
				if (base.IsWorking && !Connected)
				{
					base.CubeGrid.Physics.RigidBody.Activate();
				}
				MyShipConnector other = m_other;
				DetachInternal();
				if (MyVisualScriptLogicProvider.ConnectorStateChanged != null && m_other != null)
				{
					MyVisualScriptLogicProvider.ConnectorStateChanged(base.EntityId, base.CubeGrid.EntityId, Name, base.CubeGrid.Name, m_other.EntityId, m_other.CubeGrid.EntityId, m_other.Name, m_other.CubeGrid.Name, isConnected: false);
				}
				if (m_welded)
				{
					m_welding = true;
					m_welded = false;
					other.m_welded = false;
					SetEmissiveStateWorking();
					other.SetEmissiveStateWorking();
					m_welding = false;
					if (Sync.IsServer && !other.Closed && !other.MarkedForClose && synchronize)
					{
						TryAttach(other.EntityId);
					}
				}
			}
		}

		private void DetachInternal()
		{
			if (!IsMaster)
			{
				m_other.DetachInternal();
			}
			else
			{
				if (!InConstraint || m_other == null || !m_other.InConstraint || m_other.m_other == null)
				{
					return;
				}
				MyShipConnector other = m_other;
				HkConstraint constraint = m_constraint;
				bool connected = Connected;
				Connected = false;
				UnsetConstraint();
				other.Connected = false;
				other.UnsetConstraint();
				bool flag = true;
				if (constraint != null)
				{
					flag = RemoveConstraint(other, constraint);
				}
				if (connected)
				{
					if (flag)
					{
						RemoveLinks(other);
					}
					else
					{
						other.RemoveLinks(this);
					}
				}
			}
		}

		private void RemoveLinks(MyShipConnector otherConnector)
		{
			m_attachableConveyorEndpoint.Detach(otherConnector.m_attachableConveyorEndpoint);
			if (base.CubeGrid != otherConnector.CubeGrid)
			{
				if (!TradingEnabled && !otherConnector.TradingEnabled)
				{
					OnConstraintRemoved(GridLinkTypeEnum.Logical, otherConnector.CubeGrid);
				}
				OnConstraintRemoved(GridLinkTypeEnum.Physical, otherConnector.CubeGrid);
				MyFixedGrids.BreakLink(base.CubeGrid, otherConnector.CubeGrid, this);
				MyGridPhysicalHierarchy.Static.BreakLink(base.EntityId, base.CubeGrid, otherConnector.CubeGrid);
			}
			base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			otherConnector.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			if (Sync.IsServer)
			{
				TimeOfConnection.ValidateAndSet(0);
				otherConnector.TimeOfConnection.ValidateAndSet(0);
			}
		}

		private bool RemoveConstraint(MyShipConnector otherConnector, HkConstraint constraint)
		{
			if (m_hasConstraint)
			{
				if (base.CubeGrid.Physics != null)
				{
					base.CubeGrid.Physics.RemoveConstraint(constraint);
				}
				m_hasConstraint = false;
				constraint.Dispose();
				return true;
			}
			if (otherConnector.m_hasConstraint)
			{
				otherConnector.RemoveConstraint(this, constraint);
			}
			return false;
		}

		public override void OnAddedToScene(object source)
		{
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (m_connectorDummy != null)
			{
				m_connectorDummy.Activate();
			}
			SetEmissiveStateWorking();
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics = base.CubeGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Combine(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
		}

		private void OnPhysicsEnabledChanged()
		{
			if (m_connectorDummy != null)
			{
				m_connectorDummy.Enabled = base.CubeGrid.Physics.Enabled;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			DisposeBodyConstraint(ref m_connectorConstraint, ref m_connectorConstraintsData);
			DisposeBodyConstraint(ref m_ejectorConstraint, ref m_ejectorConstraintsData);
			base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			if (base.CubeGrid.Physics != null)
			{
				MyGridPhysics physics = base.CubeGrid.Physics;
				physics.EnabledChanged = (Action)Delegate.Remove(physics.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
			base.OnRemovedFromScene(source);
			if (base.Physics != null)
			{
				MyPhysicsBody physics2 = base.Physics;
				physics2.EnabledChanged = (Action)Delegate.Remove(physics2.EnabledChanged, new Action(OnPhysicsEnabledChanged));
			}
			if (m_connectorDummy != null)
			{
				m_connectorDummy.Deactivate();
			}
			if (InConstraint)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				m_lastAttachedOther = ((m_other != null) ? new long?(m_other.EntityId) : null);
				m_lastWeldedOther = (m_welded ? m_lastAttachedOther : null);
				Detach(synchronize: false);
			}
			base.ResourceSink.IsPoweredChanged -= Receiver_IsPoweredChanged;
		}

		protected void CreateBodyConstraint(MyPhysicsBody body, out HkFixedConstraintData constraintData, out HkConstraint constraint)
		{
			constraintData = new HkFixedConstraintData();
			constraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
			constraintData.SetInertiaStabilizationFactor(1f);
			constraintData.SetInBodySpace(base.PositionComp.LocalMatrix, Matrix.CreateTranslation(-m_connectionPosition), base.CubeGrid.Physics, body);
			constraint = new HkConstraint((base.CubeGrid.Physics.RigidBody2 != null && base.CubeGrid.Physics.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC)) ? base.CubeGrid.Physics.RigidBody2 : base.CubeGrid.Physics.RigidBody, body.RigidBody, constraintData);
			uint collisionFilterInfo = base.CubeGrid.Physics.RigidBody.GetCollisionFilterInfo();
			collisionFilterInfo = HkGroupFilter.CalcFilterInfo(base.CubeGrid.Physics.RigidBody.Layer, HkGroupFilter.GetSystemGroupFromFilterInfo(collisionFilterInfo), 1, 1);
			constraint.WantRuntime = true;
			m_canReloadDummies = false;
			body.Enabled = true;
			m_canReloadDummies = true;
			body.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
			base.CubeGrid.Physics.HavokWorld.RefreshCollisionFilterOnEntity(body.RigidBody);
		}

		protected void DisposeBodyConstraint(ref HkConstraint constraint, ref HkFixedConstraintData constraintData)
		{
			if (!(constraint == null))
			{
				base.CubeGrid.Physics.RemoveConstraint(constraint);
				constraint.Dispose();
				constraint = null;
				constraintData = null;
			}
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			if (InConstraint && !m_other.FriendlyWithBlock(this))
			{
				Detach();
			}
		}

		protected override void Closing()
		{
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			if (Connected)
			{
				Detach();
			}
			m_lastAttachedOther = null;
			m_lastWeldedOther = null;
			base.Closing();
		}

		protected override void BeforeDelete()
		{
			DisposeBodyConstraint(ref m_connectorConstraint, ref m_connectorConstraintsData);
			DisposeBodyConstraint(ref m_ejectorConstraint, ref m_ejectorConstraintsData);
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_OnHavokSystemIDChanged;
			base.BeforeDelete();
			DisposePhysicsBody(ref m_connectorDummy);
		}

		public override void DebugDrawPhysics()
		{
			base.DebugDrawPhysics();
			if (m_connectorDummy != null)
			{
				m_connectorDummy.DebugDraw();
			}
		}

		private MyShipConnector GetMaster(MyShipConnector first, MyShipConnector second)
		{
			MyCubeGrid cubeGrid = first.CubeGrid;
			MyCubeGrid cubeGrid2 = second.CubeGrid;
			if (cubeGrid.IsStatic != cubeGrid2.IsStatic)
			{
				if (cubeGrid.IsStatic)
				{
					return second;
				}
				if (cubeGrid2.IsStatic)
				{
					return first;
				}
			}
			else if (cubeGrid.GridSize != cubeGrid2.GridSize)
			{
				if (cubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					return second;
				}
				if (cubeGrid2.GridSizeEnum == MyCubeSize.Large)
				{
					return first;
				}
			}
			if (first.EntityId >= second.EntityId)
			{
				return first;
			}
			return second;
		}

		internal List<long> GetInventoryEntities(long identityId)
		{
			List<long> list = new List<long>();
			if (m_other == null)
			{
				return list;
			}
			List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Mechanical).GetGroupNodes(m_other.CubeGrid);
			if (groupNodes == null || groupNodes.Count == 0)
			{
				return list;
			}
			if (m_other == null)
			{
				return list;
			}
			foreach (MyCubeGrid item in groupNodes)
			{
				if (item.BigOwners.Contains(identityId))
				{
					foreach (MySlimBlock cubeBlock in item.CubeBlocks)
					{
						if (cubeBlock.FatBlock != null)
						{
							MyCargoContainer myCargoContainer;
							if ((myCargoContainer = (cubeBlock.FatBlock as MyCargoContainer)) != null)
							{
								if (!myCargoContainer.HasPlayerAccess(identityId))
								{
									continue;
								}
								if (myCargoContainer.GetInventoryCount() != 0)
								{
									list.Add(myCargoContainer.EntityId);
								}
							}
							MyGasTank myGasTank;
							if ((myGasTank = (cubeBlock.FatBlock as MyGasTank)) != null && myGasTank.HasPlayerAccess(identityId))
							{
								list.Add(myGasTank.EntityId);
							}
						}
					}
				}
			}
			return list;
		}

		public override void OnTeleport()
		{
			if (base.IsWorking && InConstraint && !Connected)
			{
				Detach(synchronize: false);
			}
		}

		void IMyConveyorEndpointBlock.InitializeConveyorEndpoint()
		{
			m_attachableConveyorEndpoint = new MyAttachableConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_attachableConveyorEndpoint));
		}

		void Sandbox.ModAPI.Ingame.IMyShipConnector.Connect()
		{
			if (m_connectorMode == Mode.Connector)
			{
				TryConnect();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyShipConnector.Disconnect()
		{
			if (m_connectorMode == Mode.Connector)
			{
				TryDisconnect();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyShipConnector.ToggleConnect()
		{
			if (m_connectorMode == Mode.Connector)
			{
				TrySwitch();
			}
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty Constraint")
			};
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
