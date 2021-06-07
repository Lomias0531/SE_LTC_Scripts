using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Warhead))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyWarhead),
		typeof(Sandbox.ModAPI.Ingame.IMyWarhead)
	})]
	public class MyWarhead : MyTerminalBlock, IMyDestroyableObject, Sandbox.ModAPI.IMyWarhead, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyWarhead
	{
		protected sealed class DetonateRequest_003C_003E : ICallSite<MyWarhead, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyWarhead @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DetonateRequest();
			}
		}

		protected sealed class SetCountdown_003C_003ESystem_Boolean : ICallSite<MyWarhead, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyWarhead @this, in bool countdownState, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetCountdown(countdownState);
			}
		}

		protected sealed class SetCountdownClient_003C_003ESystem_Boolean : ICallSite<MyWarhead, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyWarhead @this, in bool countdownState, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetCountdownClient(countdownState);
			}
		}

		protected class m_countdownMs_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType countdownMs;
				ISyncType result = countdownMs = new Sync<int, SyncDirection.BothWays>(P_1, P_2);
				((MyWarhead)P_0).m_countdownMs = (Sync<int, SyncDirection.BothWays>)countdownMs;
				return result;
			}
		}

		protected class m_isArmed_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isArmed;
				ISyncType result = isArmed = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyWarhead)P_0).m_isArmed = (Sync<bool, SyncDirection.BothWays>)isArmed;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyWarhead_003C_003EActor : IActivator, IActivator<MyWarhead>
		{
			private sealed override object CreateInstance()
			{
				return new MyWarhead();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWarhead CreateInstance()
			{
				return new MyWarhead();
			}

			MyWarhead IActivator<MyWarhead>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const int MAX_COUNTDOWN = 3600;

		private const int MIN_COUNTDOWN = 1;

		private const float m_maxExplosionRadius = 30f;

		public static float ExplosionImpulse = 30000f;

		private bool m_isExploded;

		private MyStringHash m_damageType = MyDamageType.Deformation;

		public int RemainingMS;

		private BoundingSphereD m_explosionShrinkenSphere;

		private BoundingSphereD m_explosionFullSphere;

		private BoundingSphereD m_explosionParticleSphere;

		private bool m_marked;

		private int m_warheadsInsideCount;

		private readonly List<MyEntity> m_entitiesInShrinkenSphere = new List<MyEntity>();

		private bool m_countdownEmissivityColor;

		private readonly Sync<int, SyncDirection.BothWays> m_countdownMs;

		public static Action<MyWarhead> OnCreated;

		public static Action<MyWarhead> OnDeleted;

		private readonly Sync<bool, SyncDirection.BothWays> m_isArmed;

		private MyWarheadDefinition m_warheadDefinition;

		public bool IsCountingDown
		{
			get;
			private set;
		}

		private int BlinkDelay
		{
			get
			{
				if ((int)m_countdownMs < 10000)
				{
					return 100;
				}
				if ((int)m_countdownMs < 30000)
				{
					return 250;
				}
				if ((int)m_countdownMs < 60000)
				{
					return 500;
				}
				return 1000;
			}
		}

		public bool IsArmed
		{
			get
			{
				return m_isArmed;
			}
			set
			{
				m_isArmed.Value = value;
			}
		}

		public bool UseDamageSystem
		{
			get;
			private set;
		}

		float IMyDestroyableObject.Integrity => 1f;

		bool IMyDestroyableObject.UseDamageSystem => UseDamageSystem;

		public float DetonationTime
		{
			get
			{
				return (int)m_countdownMs / 1000;
			}
			set
			{
				m_countdownMs.Value = (int)(value * 1000f);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyWarhead.IsCountingDown => IsCountingDown;

		float Sandbox.ModAPI.Ingame.IMyWarhead.DetonationTime
		{
			get
			{
				return DetonationTime;
			}
			set
			{
				DetonationTime = value;
			}
		}

		public MyWarhead()
		{
			CreateTerminalControls();
			m_isArmed.ValueChanged += delegate
			{
				SetEmissiveStateWorking();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyWarhead>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MyWarhead> myTerminalControlSlider = new MyTerminalControlSlider<MyWarhead>("DetonationTime", MySpaceTexts.TerminalControlPanel_Warhead_DetonationTime, MySpaceTexts.TerminalControlPanel_Warhead_DetonationTime);
				myTerminalControlSlider.SetLogLimits(1f, 3600f);
				myTerminalControlSlider.DefaultValue = 10f;
				myTerminalControlSlider.Enabled = ((MyWarhead x) => !x.IsCountingDown);
				myTerminalControlSlider.Getter = ((MyWarhead x) => x.DetonationTime);
				myTerminalControlSlider.Setter = delegate(MyWarhead x, float v)
				{
					x.m_countdownMs.Value = (int)(v * 1000f);
				};
				myTerminalControlSlider.Writer = delegate(MyWarhead x, StringBuilder sb)
				{
					MyValueFormatter.AppendTimeExact(Math.Max(x.m_countdownMs, 1000) / 1000, sb);
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlButton<MyWarhead> myTerminalControlButton = new MyTerminalControlButton<MyWarhead>("StartCountdown", MySpaceTexts.TerminalControlPanel_Warhead_StartCountdown, MySpaceTexts.TerminalControlPanel_Warhead_StartCountdown, delegate(MyWarhead b)
				{
					MyMultiplayer.RaiseEvent(b, (MyWarhead x) => x.SetCountdown, arg2: true);
				});
				myTerminalControlButton.EnableAction();
				MyTerminalControlFactory.AddControl(myTerminalControlButton);
				MyTerminalControlButton<MyWarhead> myTerminalControlButton2 = new MyTerminalControlButton<MyWarhead>("StopCountdown", MySpaceTexts.TerminalControlPanel_Warhead_StopCountdown, MySpaceTexts.TerminalControlPanel_Warhead_StopCountdown, delegate(MyWarhead b)
				{
					MyMultiplayer.RaiseEvent(b, (MyWarhead x) => x.SetCountdown, arg2: false);
				});
				myTerminalControlButton2.EnableAction();
				MyTerminalControlFactory.AddControl(myTerminalControlButton2);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyWarhead>());
				MyTerminalControlCheckbox<MyWarhead> obj = new MyTerminalControlCheckbox<MyWarhead>("Safety", MySpaceTexts.TerminalControlPanel_Warhead_Safety, MySpaceTexts.TerminalControlPanel_Warhead_SafetyTooltip, MySpaceTexts.TerminalControlPanel_Warhead_SwitchTextArmed, MySpaceTexts.TerminalControlPanel_Warhead_SwitchTextDisarmed)
				{
					Getter = ((MyWarhead x) => x.IsArmed),
					Setter = delegate(MyWarhead x, bool v)
					{
						x.IsArmed = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyWarhead> obj2 = new MyTerminalControlButton<MyWarhead>("Detonate", MySpaceTexts.TerminalControlPanel_Warhead_Detonate, MySpaceTexts.TerminalControlPanel_Warhead_Detonate, delegate(MyWarhead b)
				{
					if (b.IsArmed)
					{
						MyMultiplayer.RaiseEvent(b, (MyWarhead x) => x.DetonateRequest);
					}
				})
				{
					Enabled = ((MyWarhead x) => x.IsArmed)
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			m_warheadDefinition = (MyWarheadDefinition)base.BlockDefinition;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_Warhead myObjectBuilder_Warhead = (MyObjectBuilder_Warhead)objectBuilder;
			m_countdownMs.SetLocalValue(MathHelper.Clamp(myObjectBuilder_Warhead.CountdownMs, 1000, 3600000));
			m_isArmed.SetLocalValue(myObjectBuilder_Warhead.IsArmed);
			IsCountingDown = myObjectBuilder_Warhead.IsCountingDown;
			base.IsWorkingChanged += MyWarhead_IsWorkingChanged;
			UseDamageSystem = true;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Warhead obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Warhead;
			obj.CountdownMs = m_countdownMs;
			obj.IsCountingDown = IsCountingDown;
			obj.IsArmed = IsArmed;
			return obj;
		}

		private void MyWarhead_IsWorkingChanged(MyCubeBlock obj)
		{
			if (IsCountingDown && !base.IsWorking)
			{
				StopCountdown();
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			if (base.IsWorking)
			{
				if (IsCountingDown)
				{
					if (m_countdownEmissivityColor)
					{
						return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
					}
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
				}
				if (IsArmed)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		public override void ContactPointCallback(ref MyGridContactInfo value)
		{
			base.ContactPointCallback(ref value);
			if (!(value.CollidingEntity is MyDebrisBase) && Math.Abs(value.Event.SeparatingVelocity) > 5f && base.IsFunctional && IsArmed && base.CubeGrid.BlocksDestructionEnabled)
			{
				Explode();
			}
		}

		public bool StartCountdown()
		{
			if (!base.IsFunctional || IsCountingDown)
			{
				return false;
			}
			IsCountingDown = true;
			MyWarheads.AddWarhead(this);
			RaisePropertiesChanged();
			SetEmissiveStateWorking();
			return true;
		}

		public bool StopCountdown()
		{
			if (!base.IsFunctional || !IsCountingDown)
			{
				return false;
			}
			IsCountingDown = false;
			MyWarheads.RemoveWarhead(this);
			RaisePropertiesChanged();
			SetEmissiveStateWorking();
			return true;
		}

		public bool Countdown(int frameMs)
		{
			if (!base.IsFunctional)
			{
				return false;
			}
			if (Sync.IsServer)
			{
				m_countdownMs.Value -= frameMs;
			}
			if ((int)m_countdownMs % BlinkDelay < frameMs)
			{
				m_countdownEmissivityColor = !m_countdownEmissivityColor;
				SetEmissiveStateWorking();
			}
			RaisePropertiesChanged();
			return (int)m_countdownMs <= 0;
		}

		public void Detonate()
		{
			if (base.IsFunctional)
			{
				Explode();
			}
		}

		public void Explode()
		{
			if (!m_isExploded && MySession.Static.WeaponsEnabled && base.CubeGrid.Physics != null)
			{
				m_isExploded = true;
				if (!m_marked)
				{
					MarkForExplosion();
				}
				MyExplosionTypeEnum myExplosionTypeEnum = MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;
				myExplosionTypeEnum = ((m_explosionFullSphere.Radius <= 6.0) ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_02 : ((m_explosionFullSphere.Radius <= 20.0) ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_15 : ((!(m_explosionFullSphere.Radius <= 40.0)) ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_50 : MyExplosionTypeEnum.WARHEAD_EXPLOSION_30)));
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = m_warheadDefinition.WarheadExplosionDamage;
				myExplosionInfo.ExplosionType = myExplosionTypeEnum;
				myExplosionInfo.ExplosionSphere = m_explosionFullSphere;
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = this;
				myExplosionInfo.ParticleScale = 1f;
				myExplosionInfo.OwnerEntity = base.CubeGrid;
				myExplosionInfo.Direction = base.WorldMatrix.Forward;
				myExplosionInfo.VoxelExplosionCenter = m_explosionFullSphere.Center;
				myExplosionInfo.ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION);
				myExplosionInfo.VoxelCutoutScale = 1f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
				MyExplosionInfo explosionInfo = myExplosionInfo;
				if (base.CubeGrid.Physics != null)
				{
					explosionInfo.Velocity = base.CubeGrid.Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo);
			}
		}

		private void MarkForExplosion()
		{
			m_marked = true;
			float num = 4f;
			float num2 = base.CubeGrid.GridSize * num;
			float num3 = 0.85f;
			m_explosionShrinkenSphere = new BoundingSphereD(base.PositionComp.GetPosition(), (double)num2 * (double)num3);
			m_explosionParticleSphere = BoundingSphereD.CreateInvalid();
			MyGamePruningStructure.GetAllEntitiesInSphere(ref m_explosionShrinkenSphere, m_entitiesInShrinkenSphere);
			m_warheadsInsideCount = 0;
			foreach (MyEntity item in m_entitiesInShrinkenSphere)
			{
				if (!(item is MyDebrisBase) && (!(item is MyCubeBlock) || (item as MyCubeBlock).CubeGrid.Projector == null) && Vector3D.DistanceSquared(base.PositionComp.GetPosition(), item.PositionComp.GetPosition()) < (double)(num2 * num3 * num2 * num3))
				{
					MyWarhead myWarhead = item as MyWarhead;
					if (myWarhead != null)
					{
						m_warheadsInsideCount++;
						m_explosionParticleSphere = m_explosionParticleSphere.Include(new BoundingSphereD(myWarhead.PositionComp.GetPosition(), base.CubeGrid.GridSize * num + myWarhead.CubeGrid.GridSize));
					}
				}
			}
			m_entitiesInShrinkenSphere.Clear();
			float num4 = Math.Min(30f, (1f + 0.024f * (float)m_warheadsInsideCount) * m_warheadDefinition.ExplosionRadius);
			m_explosionFullSphere = new BoundingSphereD(m_explosionParticleSphere.Center, (float)Math.Max(num4, m_explosionParticleSphere.Radius));
			if (MyExplosion.DEBUG_EXPLOSIONS)
			{
				MyWarheads.DebugWarheadShrinks.Add(m_explosionShrinkenSphere);
				MyWarheads.DebugWarheadGroupSpheres.Add(m_explosionFullSphere);
				_ = m_explosionParticleSphere;
			}
		}

		public override void OnDestroy()
		{
			if (base.IsFunctional && IsArmed)
			{
				if (m_damageType == MyDamageType.Bullet)
				{
					Explode();
					return;
				}
				MarkForExplosion();
				ExplodeDelayed(500);
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (IsCountingDown)
			{
				IsCountingDown = false;
				StartCountdown();
			}
			else
			{
				SetEmissiveStateWorking();
			}
			if (OnCreated != null)
			{
				OnCreated(this);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (IsCountingDown)
			{
				StopCountdown();
				IsCountingDown = true;
			}
			if (OnDeleted != null)
			{
				OnDeleted(this);
			}
		}

		private void ExplodeDelayed(int maxMiliseconds)
		{
			RemainingMS = MyUtils.GetRandomInt(maxMiliseconds);
			m_countdownMs.Value = 0;
			StartCountdown();
		}

		[Event(null, 462)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DetonateRequest()
		{
			Detonate();
		}

		[Event(null, 468)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void SetCountdown(bool countdownState)
		{
			bool flag = false;
			if ((!countdownState) ? StopCountdown() : StartCountdown())
			{
				MyMultiplayer.RaiseEvent(this, (MyWarhead x) => x.SetCountdownClient, countdownState);
			}
		}

		[Event(null, 483)]
		[Reliable]
		[Broadcast]
		private void SetCountdownClient(bool countdownState)
		{
			if (countdownState)
			{
				StartCountdown();
			}
			else
			{
				StopCountdown();
			}
		}

		void IMyDestroyableObject.OnDestroy()
		{
			OnDestroy();
		}

		bool IMyDestroyableObject.DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
		{
			if (!MySession.Static.DestructibleBlocks)
			{
				return false;
			}
			if (!IsArmed)
			{
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Damage, 0L, 0uL))
			{
				return false;
			}
			if (sync)
			{
				if (Sync.IsServer)
				{
					MySyncDamage.DoDamageSynced(this, damage, damageType, attackerId);
				}
			}
			else
			{
				MyDamageInformation info = new MyDamageInformation(isDeformation: false, damage, damageType, attackerId);
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseBeforeDamageApplied(this, ref info);
				}
				m_damageType = damageType;
				if (info.Amount > 0f)
				{
					if (UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseAfterDamageApplied(this, info);
					}
					if ((bool)m_isArmed)
					{
						OnDestroy();
					}
					if (UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseDestroyed(this, info);
					}
				}
			}
			return true;
		}

		void Sandbox.ModAPI.Ingame.IMyWarhead.Detonate()
		{
			Detonate();
		}
	}
}
