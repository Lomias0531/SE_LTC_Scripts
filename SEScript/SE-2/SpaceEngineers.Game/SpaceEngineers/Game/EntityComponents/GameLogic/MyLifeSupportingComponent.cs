using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.World;
using System;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace SpaceEngineers.Game.EntityComponents.GameLogic
{
	public class MyLifeSupportingComponent : MyEntityComponentBase
	{
		private int m_lastTimeUsed;

		private readonly MySoundPair m_progressSound;

		private readonly MyEntity3DSoundEmitter m_progressSoundEmitter;

		private string m_actionName;

		private float m_rechargeMultiplier = 1f;

		public new IMyLifeSupportingBlock Entity => (IMyLifeSupportingBlock)base.Entity;

		public MyCharacter User
		{
			get;
			private set;
		}

		public MyRechargeSocket RechargeSocket
		{
			get;
			private set;
		}

		public override string ComponentTypeDebugString => GetType().Name;

		public MyLifeSupportingComponent(MyEntity owner, MySoundPair progressSound, string actionName = "GenericHeal", float rechargeMultiplier = 1f)
		{
			RechargeSocket = new MyRechargeSocket();
			m_actionName = actionName;
			m_rechargeMultiplier = rechargeMultiplier;
			m_progressSound = progressSound;
			m_progressSoundEmitter = new MyEntity3DSoundEmitter(owner, useStaticList: true);
			m_progressSoundEmitter.EmitterMethods[1].Add((Func<bool>)(() => MySession.Static.ControlledEntity != null && User == MySession.Static.ControlledEntity.Entity));
			if (MySession.Static != null && MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound)
			{
				m_progressSoundEmitter.EmitterMethods[0].Add((Func<bool>)(() => MySession.Static.ControlledEntity != null && User == MySession.Static.ControlledEntity.Entity));
			}
		}

		public void OnSupportRequested(MyCharacter user)
		{
			if (User == null || User == user)
			{
				Entity.BroadcastSupportRequest(user);
			}
		}

		public void ProvideSupport(MyCharacter user)
		{
			if (!Entity.IsWorking)
			{
				return;
			}
			bool flag = false;
			if (User == null)
			{
				User = user;
				if (Entity.RefuelAllowed)
				{
					user.SuitBattery.ResourceSink.TemporaryConnectedEntity = Entity;
					user.SuitBattery.RechargeMultiplier = m_rechargeMultiplier;
					RechargeSocket.PlugIn(user.SuitBattery.ResourceSink);
					flag = true;
					MyVisualScriptLogicProvider.PlayerSuitRecharging?.Invoke(User.GetPlayerIdentityId(), Entity.BlockType);
				}
			}
			m_lastTimeUsed = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (User.StatComp != null && Entity.HealingAllowed)
			{
				User.StatComp.DoAction(m_actionName);
				flag = true;
				PlayerHealthRechargeEvent playerHealthRecharging = MyVisualScriptLogicProvider.PlayerHealthRecharging;
				if (playerHealthRecharging != null)
				{
					float value = (User.StatComp.Health != null) ? User.StatComp.Health.Value : 0f;
					playerHealthRecharging(User.GetPlayerIdentityId(), Entity.BlockType, value);
				}
			}
			if (flag)
			{
				PlayProgressLoopSound();
			}
		}

		private void PlayProgressLoopSound()
		{
			if (!m_progressSoundEmitter.IsPlaying)
			{
				m_progressSoundEmitter.PlaySound(m_progressSound, stopPrevious: true);
			}
		}

		private void StopProgressLoopSound()
		{
			m_progressSoundEmitter.StopSound(forced: false);
		}

		public void UpdateSoundEmitters()
		{
			m_progressSoundEmitter.Update();
		}

		public void Update10()
		{
			if (User != null && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeUsed >= 100)
			{
				Unplug();
			}
		}

		public override void OnRemovedFromScene()
		{
			Unplug();
			base.OnRemovedFromScene();
		}

		private void Unplug()
		{
			if (User != null)
			{
				RechargeSocket.Unplug();
				User.SuitBattery.ResourceSink.TemporaryConnectedEntity = null;
				User.SuitBattery.RechargeMultiplier = 1f;
				User = null;
				StopProgressLoopSound();
			}
		}
	}
}
