using Sandbox.Engine.Platform;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyFunctionalBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyFunctionalBlock)
	})]
	public class MyFunctionalBlock : MyTerminalBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		protected class m_enabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType enabled;
				ISyncType result = enabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyFunctionalBlock)P_0).m_enabled = (Sync<bool, SyncDirection.BothWays>)enabled;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyFunctionalBlock_003C_003EActor : IActivator, IActivator<MyFunctionalBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyFunctionalBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFunctionalBlock CreateInstance()
			{
				return new MyFunctionalBlock();
			}

			MyFunctionalBlock IActivator<MyFunctionalBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected MySoundPair m_baseIdleSound = new MySoundPair();

		protected MySoundPair m_actionSound = new MySoundPair();

		public MyEntity3DSoundEmitter m_soundEmitter;

		private readonly Sync<bool, SyncDirection.BothWays> m_enabled;

		internal MyEntity3DSoundEmitter SoundEmitter => m_soundEmitter;

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled.Value = value;
			}
		}

		public event Action<MyTerminalBlock> EnabledChanged;

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyFunctionalBlock.EnabledChanged
		{
			add
			{
				EnabledChanged += GetDelegate(value);
			}
			remove
			{
				EnabledChanged -= GetDelegate(value);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.OnRemovedFromScene(source);
		}

		private void EnabledSyncChanged()
		{
			UpdateIsWorking();
			OnEnabledChanged();
		}

		public MyFunctionalBlock()
		{
			CreateTerminalControls();
			m_enabled.ValueChanged += delegate
			{
				EnabledSyncChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyFunctionalBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyFunctionalBlock> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyFunctionalBlock>("OnOff", MySpaceTexts.BlockAction_Toggle);
				myTerminalControlOnOffSwitch.Getter = ((MyFunctionalBlock x) => x.Enabled);
				myTerminalControlOnOffSwitch.Setter = delegate(MyFunctionalBlock x, bool v)
				{
					x.Enabled = v;
				};
				myTerminalControlOnOffSwitch.EnableToggleAction();
				myTerminalControlOnOffSwitch.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(0, myTerminalControlOnOffSwitch);
				MyTerminalControlFactory.AddControl(1, new MyTerminalControlSeparator<MyFunctionalBlock>());
			}
		}

		protected override bool CheckIsWorking()
		{
			if (Enabled)
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_FunctionalBlock myObjectBuilder_FunctionalBlock = (MyObjectBuilder_FunctionalBlock)objectBuilder;
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_enabled.SetLocalValue(myObjectBuilder_FunctionalBlock.Enabled);
			base.IsWorkingChanged += CubeBlock_IsWorkingChanged;
			m_baseIdleSound = base.BlockDefinition.PrimarySound;
			m_actionSound = base.BlockDefinition.ActionSound;
		}

		private void CubeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			else
			{
				OnStopWorking();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_FunctionalBlock obj = (MyObjectBuilder_FunctionalBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.Enabled = Enabled;
			return obj;
		}

		protected virtual void OnEnabledChanged()
		{
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			else
			{
				OnStopWorking();
			}
			this.EnabledChanged?.Invoke(this);
			RaisePropertiesChanged();
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_soundEmitter != null && SilenceInChange)
			{
				SilenceInChange = m_soundEmitter.FastUpdate(IsSilenced);
				if (!SilenceInChange && !UsedUpdateEveryFrame && !base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.UpdateSoundOcclusion();
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (m_soundEmitter != null && MySector.MainCamera != null)
			{
				UpdateSoundEmitters();
			}
		}

		public virtual void UpdateSoundEmitters()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		protected virtual void OnStartWorking()
		{
			if (base.InScene && base.CubeGrid.Physics != null && m_soundEmitter != null && m_baseIdleSound != null && m_baseIdleSound != MySoundPair.Empty)
			{
				m_soundEmitter.PlaySound(m_baseIdleSound, stopPrevious: true);
			}
		}

		protected virtual void OnStopWorking()
		{
			if (m_soundEmitter != null && (base.BlockDefinition.DamagedSound == null || m_soundEmitter.SoundId != base.BlockDefinition.DamagedSound.SoundId))
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		public override void SetDamageEffect(bool show)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			base.SetDamageEffect(show);
			if (m_soundEmitter != null && base.BlockDefinition.DamagedSound != null)
			{
				if (show)
				{
					m_soundEmitter.PlaySound(base.BlockDefinition.DamagedSound, stopPrevious: true);
				}
				else if (m_soundEmitter.SoundId == base.BlockDefinition.DamagedSound.SoundId)
				{
					m_soundEmitter.StopSound(forced: false);
				}
			}
		}

		public override void StopDamageEffect(bool stopSound = true)
		{
			base.StopDamageEffect(stopSound);
			if (stopSound && m_soundEmitter != null && base.BlockDefinition.DamagedSound != null && (m_soundEmitter.SoundId == base.BlockDefinition.DamagedSound.Arcade || m_soundEmitter.SoundId != base.BlockDefinition.DamagedSound.Realistic))
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		public virtual int GetBlockSpecificState()
		{
			return -1;
		}

		private Action<MyTerminalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock> value)
		{
			return (Action<MyTerminalBlock>)Delegate.CreateDelegate(typeof(Action<MyTerminalBlock>), value.Target, value.Method);
		}

		void Sandbox.ModAPI.Ingame.IMyFunctionalBlock.RequestEnable(bool enable)
		{
			Enabled = enable;
		}
	}
}
