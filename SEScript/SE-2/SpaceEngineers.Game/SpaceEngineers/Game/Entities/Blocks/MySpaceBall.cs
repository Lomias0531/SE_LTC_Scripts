using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SpaceBall))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMySpaceBall),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall)
	})]
	public class MySpaceBall : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMySpaceBall, SpaceEngineers.Game.ModAPI.IMyVirtualMass, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyVirtualMass, SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall
	{
		protected class m_friction_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType friction;
				ISyncType result = friction = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MySpaceBall)P_0).m_friction = (Sync<float, SyncDirection.BothWays>)friction;
				return result;
			}
		}

		protected class m_virtualMass_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType virtualMass;
				ISyncType result = virtualMass = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MySpaceBall)P_0).m_virtualMass = (Sync<float, SyncDirection.BothWays>)virtualMass;
				return result;
			}
		}

		protected class m_restitution_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType restitution;
				ISyncType result = restitution = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MySpaceBall)P_0).m_restitution = (Sync<float, SyncDirection.BothWays>)restitution;
				return result;
			}
		}

		protected class m_broadcastSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType broadcastSync;
				ISyncType result = broadcastSync = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MySpaceBall)P_0).m_broadcastSync = (Sync<bool, SyncDirection.BothWays>)broadcastSync;
				return result;
			}
		}

		private readonly Sync<float, SyncDirection.BothWays> m_friction;

		private readonly Sync<float, SyncDirection.BothWays> m_virtualMass;

		private readonly Sync<float, SyncDirection.BothWays> m_restitution;

		private readonly Sync<bool, SyncDirection.BothWays> m_broadcastSync;

		private bool m_savedBroadcast;

		public const float DEFAULT_RESTITUTION = 0.4f;

		public const float DEFAULT_MASS = 100f;

		public const float DEFAULT_FRICTION = 0.5f;

		public const float REAL_MAXIMUM_RESTITUTION = 0.9f;

		public const float REAL_MINIMUM_MASS = 0.01f;

		public float Friction
		{
			get
			{
				return m_friction;
			}
			set
			{
				m_friction.Value = value;
			}
		}

		public float VirtualMass
		{
			get
			{
				return m_virtualMass;
			}
			set
			{
				m_virtualMass.Value = value;
			}
		}

		public float Restitution
		{
			get
			{
				return m_restitution;
			}
			set
			{
				m_restitution.Value = value;
			}
		}

		public bool Broadcast
		{
			get
			{
				if (RadioBroadcaster != null)
				{
					return RadioBroadcaster.Enabled;
				}
				return false;
			}
			set
			{
				m_broadcastSync.Value = value;
			}
		}

		private new MySpaceBallDefinition BlockDefinition => (MySpaceBallDefinition)base.BlockDefinition;

		internal MyRadioBroadcaster RadioBroadcaster
		{
			get
			{
				return (MyRadioBroadcaster)base.Components.Get<MyDataBroadcaster>();
			}
			private set
			{
				base.Components.Add((MyDataBroadcaster)value);
			}
		}

		internal MyRadioReceiver RadioReceiver
		{
			get
			{
				return (MyRadioReceiver)base.Components.Get<MyDataReceiver>();
			}
			set
			{
				base.Components.Add((MyDataReceiver)value);
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.VirtualMass
		{
			get
			{
				return GetMass();
			}
			set
			{
				VirtualMass = MathHelper.Clamp(value, 0.01f, BlockDefinition.MaxVirtualMass);
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.Friction
		{
			get
			{
				return Friction;
			}
			set
			{
				Friction = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.Restitution
		{
			get
			{
				return Restitution;
			}
			set
			{
				Restitution = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.IsBroadcasting => Broadcast;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.Broadcasting
		{
			get
			{
				return Broadcast;
			}
			set
			{
				Broadcast = value;
			}
		}

		public MySpaceBall()
		{
			CreateTerminalControls();
			m_baseIdleSound.Init("BlockArtMass");
			m_virtualMass.ValueChanged += delegate
			{
				RefreshPhysicsBody();
			};
			m_broadcastSync.ValueChanged += delegate
			{
				BroadcastChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySpaceBall>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MySpaceBall> myTerminalControlSlider = new MyTerminalControlSlider<MySpaceBall>("VirtualMass", MySpaceTexts.BlockPropertyDescription_SpaceBallVirtualMass, MySpaceTexts.BlockPropertyDescription_SpaceBallVirtualMass);
				myTerminalControlSlider.Getter = ((MySpaceBall x) => x.VirtualMass);
				myTerminalControlSlider.Setter = delegate(MySpaceBall x, float v)
				{
					x.VirtualMass = v;
				};
				myTerminalControlSlider.DefaultValueGetter = ((MySpaceBall x) => 100f);
				myTerminalControlSlider.SetLimits((MySpaceBall x) => 0f, (MySpaceBall x) => x.BlockDefinition.MaxVirtualMass);
				myTerminalControlSlider.Writer = delegate(MySpaceBall x, StringBuilder result)
				{
					MyValueFormatter.AppendWeightInBestUnit(x.VirtualMass, result);
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				if (MyPerGameSettings.BallFriendlyPhysics)
				{
					MyTerminalControlSlider<MySpaceBall> myTerminalControlSlider2 = new MyTerminalControlSlider<MySpaceBall>("Friction", MySpaceTexts.BlockPropertyDescription_SpaceBallFriction, MySpaceTexts.BlockPropertyDescription_SpaceBallFriction);
					myTerminalControlSlider2.Getter = ((MySpaceBall x) => x.Friction);
					myTerminalControlSlider2.Setter = delegate(MySpaceBall x, float v)
					{
						x.Friction = v;
					};
					myTerminalControlSlider2.DefaultValueGetter = ((MySpaceBall x) => 0.5f);
					myTerminalControlSlider2.SetLimits(0f, 1f);
					myTerminalControlSlider2.Writer = delegate(MySpaceBall x, StringBuilder result)
					{
						result.AppendInt32((int)(x.Friction * 100f)).Append("%");
					};
					myTerminalControlSlider2.EnableActions();
					MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
					MyTerminalControlSlider<MySpaceBall> myTerminalControlSlider3 = new MyTerminalControlSlider<MySpaceBall>("Restitution", MySpaceTexts.BlockPropertyDescription_SpaceBallRestitution, MySpaceTexts.BlockPropertyDescription_SpaceBallRestitution);
					myTerminalControlSlider3.Getter = ((MySpaceBall x) => x.Restitution);
					myTerminalControlSlider3.Setter = delegate(MySpaceBall x, float v)
					{
						x.Restitution = v;
					};
					myTerminalControlSlider3.DefaultValueGetter = ((MySpaceBall x) => 0.4f);
					myTerminalControlSlider3.SetLimits(0f, 1f);
					myTerminalControlSlider3.Writer = delegate(MySpaceBall x, StringBuilder result)
					{
						result.AppendInt32((int)(x.Restitution * 100f)).Append("%");
					};
					myTerminalControlSlider3.EnableActions();
					MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				}
				MyTerminalControlCheckbox<MySpaceBall> obj = new MyTerminalControlCheckbox<MySpaceBall>("EnableBroadCast", MySpaceTexts.Antenna_EnableBroadcast, MySpaceTexts.Antenna_EnableBroadcast)
				{
					Getter = ((MySpaceBall x) => x.Broadcast),
					Setter = delegate(MySpaceBall x, bool v)
					{
						x.Broadcast = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			base.NeedsWorldMatrix = true;
			RadioReceiver = new MyRadioReceiver();
			RadioBroadcaster = new MyRadioBroadcaster(50f);
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_SpaceBall myObjectBuilder_SpaceBall = (MyObjectBuilder_SpaceBall)objectBuilder;
			m_virtualMass.SetLocalValue(MathHelper.Clamp(myObjectBuilder_SpaceBall.VirtualMass, 0f, BlockDefinition.MaxVirtualMass));
			m_restitution.SetLocalValue(MathHelper.Clamp(myObjectBuilder_SpaceBall.Restitution, 0f, 1f));
			m_friction.SetLocalValue(MathHelper.Clamp(myObjectBuilder_SpaceBall.Friction, 0f, 1f));
			base.IsWorkingChanged += MySpaceBall_IsWorkingChanged;
			UpdateIsWorking();
			RefreshPhysicsBody();
			m_savedBroadcast = myObjectBuilder_SpaceBall.EnableBroadcast;
			base.ShowOnHUD = false;
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SpaceBall obj = (MyObjectBuilder_SpaceBall)base.GetObjectBuilderCubeBlock(copy);
			obj.VirtualMass = m_virtualMass;
			obj.Restitution = Restitution;
			obj.Friction = Friction;
			obj.EnableBroadcast = RadioBroadcaster.Enabled;
			return obj;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			UpdatePhysics();
		}

		private void RefreshPhysicsBody()
		{
			if (base.CubeGrid.CreatePhysics)
			{
				if (base.Physics != null)
				{
					base.Physics.Close();
				}
				HkSphereShape shape = new HkSphereShape(base.CubeGrid.GridSize * 0.5f);
				HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(shape.Radius, (VirtualMass != 0f) ? VirtualMass : 0.01f);
				base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_KEYFRAMED_REPORTING);
				base.Physics.IsPhantom = false;
				base.Physics.CreateFromCollisionObject(shape, Vector3.Zero, base.WorldMatrix, value, 25);
				UpdateIsWorking();
				base.Physics.Enabled = (base.IsWorking && base.CubeGrid.Physics != null && base.CubeGrid.Physics.Enabled);
				base.Physics.RigidBody.Activate();
				shape.Base.RemoveReference();
				if (base.CubeGrid != null && base.CubeGrid.Physics != null && !base.CubeGrid.IsStatic)
				{
					base.CubeGrid.Physics.UpdateMass();
				}
			}
		}

		private void UpdatePhysics()
		{
			if (base.Physics != null)
			{
				base.Physics.Enabled = (base.IsWorking && base.CubeGrid.Physics != null && base.CubeGrid.Physics.Enabled);
			}
		}

		public void UpdateRadios(bool isTrue)
		{
			if (RadioBroadcaster != null && RadioReceiver != null)
			{
				RadioBroadcaster.WantsToBeEnabled = isTrue;
				RadioReceiver.Enabled = (isTrue & base.Enabled);
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (base.CubeGrid.Physics != null && !base.CubeGrid.IsStatic)
			{
				base.CubeGrid.Physics.UpdateMass();
			}
			if (base.Physics != null)
			{
				UpdatePhysics();
			}
			UpdateRadios(m_savedBroadcast);
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			RadioReceiver.UpdateBroadcastersInRange();
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			RadioReceiver.UpdateBroadcastersInRange();
		}

		private void MySpaceBall_IsWorkingChanged(MyCubeBlock obj)
		{
			UpdateRadios(base.IsWorking);
		}

		public override void ContactPointCallback(ref MyGridContactInfo value)
		{
			HkContactPointProperties contactProperties = value.Event.ContactProperties;
			value.EnableDeformation = false;
			value.EnableParticles = false;
			value.RubberDeformation = false;
			if (MyPerGameSettings.BallFriendlyPhysics)
			{
				contactProperties.Friction = Friction;
				contactProperties.Restitution = ((Restitution > 0.9f) ? 0.9f : Restitution);
			}
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.MoveBroadcaster();
			}
		}

		public override float GetMass()
		{
			if (!(VirtualMass > 0f))
			{
				return 0.01f;
			}
			return VirtualMass;
		}

		private void BroadcastChanged()
		{
			RadioBroadcaster.Enabled = m_broadcastSync;
			RaisePropertiesChanged();
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			if (base.ShowOnHUD || (base.IsBeingHacked && allowBlink))
			{
				return base.GetHudParams(allowBlink);
			}
			m_hudParams.Clear();
			return m_hudParams;
		}
	}
}
