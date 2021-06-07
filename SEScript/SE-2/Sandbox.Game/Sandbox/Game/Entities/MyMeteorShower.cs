using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	[StaticEventOwner]
	internal class MyMeteorShower : MySessionComponentBase
	{
		protected sealed class UpdateShowerTarget_003C_003ESystem_Nullable_00601_003CVRageMath_BoundingSphereD_003E : ICallSite<IMyEventOwner, BoundingSphereD?, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in BoundingSphereD? target, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateShowerTarget(target);
			}
		}

		private static readonly int WAVES_IN_SHOWER = 1;

		private static readonly double HORIZON_ANGLE_FROM_ZENITH_RATIO = Math.Sin(0.35);

		private static readonly double METEOR_BLUR_KOEF = 2.5;

		private static Vector3D m_tgtPos;

		private static Vector3D m_normalSun;

		private static Vector3D m_pltTgtDir;

		private static Vector3D m_mirrorDir;

		private static int m_waveCounter;

		private static List<MyEntity> m_meteorList = new List<MyEntity>();

		private static List<MyEntity> m_tmpEntityList = new List<MyEntity>();

		private static BoundingSphereD? m_currentTarget;

		private static List<BoundingSphereD> m_targetList = new List<BoundingSphereD>();

		private static int m_lastTargetCount;

		private static Vector3 m_downVector;

		private static Vector3 m_rightVector = Vector3.Zero;

		private static int m_meteorcount;

		private static List<MyCubeGrid> m_tmpHitGroup = new List<MyCubeGrid>();

		private static string[] m_enviromentHostilityName = new string[4]
		{
			"Safe",
			"MeteorWave",
			"MeteorWaveCataclysm",
			"MeteorWaveCataclysmUnreal"
		};

		private static Vector3D m_meteorHitPos;

		public static BoundingSphereD? CurrentTarget
		{
			get
			{
				return m_currentTarget;
			}
			set
			{
				m_currentTarget = value;
			}
		}

		public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME;

		public override void LoadData()
		{
			m_waveCounter = -1;
			m_lastTargetCount = 0;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			foreach (MyEntity meteor in m_meteorList)
			{
				if (!meteor.MarkedForClose)
				{
					meteor.Close();
				}
			}
			m_meteorList.Clear();
			m_currentTarget = null;
			m_targetList.Clear();
			base.UnloadData();
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!Sync.IsServer)
			{
				return;
			}
			MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWave"));
			if (eventById == null)
			{
				eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWaveCataclysm"));
			}
			if (eventById == null)
			{
				eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWaveCataclysmUnreal"));
			}
			if (eventById == null && MySession.Static.EnvironmentHostility != 0 && MyFakes.ENABLE_METEOR_SHOWERS)
			{
				MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWave"));
				myGlobalEventBase.SetActivationTime(CalculateShowerTime(MySession.Static.EnvironmentHostility));
				MyGlobalEvents.AddGlobalEvent(myGlobalEventBase);
			}
			else
			{
				if (eventById == null)
				{
					return;
				}
				if (MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.SAFE || !MyFakes.ENABLE_METEOR_SHOWERS)
				{
					eventById.Enabled = false;
					return;
				}
				eventById.Enabled = true;
				if (MySession.Static.PreviousEnvironmentHostility.HasValue && MySession.Static.EnvironmentHostility != MySession.Static.PreviousEnvironmentHostility.Value)
				{
					eventById.SetActivationTime(CalculateShowerTime(MySession.Static.EnvironmentHostility, MySession.Static.PreviousEnvironmentHostility.Value, eventById.ActivationTime));
					MySession.Static.PreviousEnvironmentHostility = null;
				}
			}
		}

		private static void MeteorWaveInternal(object senderEvent)
		{
			if (MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.SAFE)
			{
				((MyGlobalEventBase)senderEvent).Enabled = false;
			}
			else
			{
				if (!Sync.IsServer)
				{
					return;
				}
				m_waveCounter++;
				if (m_waveCounter == 0)
				{
					ClearMeteorList();
					if (m_targetList.Count == 0)
					{
						GetTargets();
						if (m_targetList.Count == 0)
						{
							m_waveCounter = WAVES_IN_SHOWER + 1;
							RescheduleEvent(senderEvent);
							return;
						}
					}
					m_currentTarget = m_targetList.ElementAt(MyUtils.GetRandomInt(m_targetList.Count - 1));
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateShowerTarget, m_currentTarget);
					m_targetList.Remove(m_currentTarget.Value);
					m_meteorcount = (int)(Math.Pow(m_currentTarget.Value.Radius, 2.0) * Math.PI / 6000.0);
					m_meteorcount /= ((MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM || MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL) ? 1 : 8);
					m_meteorcount = MathHelper.Clamp(m_meteorcount, 1, 30);
				}
				RescheduleEvent(senderEvent);
				CheckTargetValid();
				if (m_waveCounter >= 0)
				{
					StartWave();
				}
			}
		}

		private static void StartWave()
		{
			if (!m_currentTarget.HasValue)
			{
				return;
			}
			Vector3 correctedDirection = GetCorrectedDirection(MySector.DirectionToSunNormalized);
			SetupDirVectors(correctedDirection);
			float randomFloat = MyUtils.GetRandomFloat(Math.Min(2, m_meteorcount - 3), m_meteorcount + 3);
			Vector3 randomVector3CircleNormalized = MyUtils.GetRandomVector3CircleNormalized();
			float randomFloat2 = MyUtils.GetRandomFloat(0f, 1f);
			Vector3D value = randomVector3CircleNormalized.X * m_rightVector + randomVector3CircleNormalized.Z * m_downVector;
			Vector3D vector3D = m_currentTarget.Value.Center + Math.Pow(randomFloat2, 0.699999988079071) * m_currentTarget.Value.Radius * value * METEOR_BLUR_KOEF;
			Vector3D vector3D2 = -Vector3D.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D));
			if (vector3D2 != Vector3D.Zero)
			{
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(vector3D + vector3D2 * 3000.0, vector3D, 15);
				if (hitInfo.HasValue)
				{
					vector3D = hitInfo.Value.Position;
				}
			}
			m_meteorHitPos = vector3D;
			for (int i = 0; (float)i < randomFloat; i++)
			{
				randomVector3CircleNormalized = MyUtils.GetRandomVector3CircleNormalized();
				randomFloat2 = MyUtils.GetRandomFloat(0f, 1f);
				value = randomVector3CircleNormalized.X * m_rightVector + randomVector3CircleNormalized.Z * m_downVector;
				vector3D += Math.Pow(randomFloat2, 0.699999988079071) * m_currentTarget.Value.Radius * value;
				Vector3 value2 = correctedDirection * (2000 + 100 * i);
				randomVector3CircleNormalized = MyUtils.GetRandomVector3CircleNormalized();
				value = randomVector3CircleNormalized.X * m_rightVector + randomVector3CircleNormalized.Z * m_downVector;
				Vector3D vector3D3 = vector3D + value2 + (float)Math.Tan(MyUtils.GetRandomFloat(0f, MathF.PI / 18f)) * value;
				m_meteorList.Add(MyMeteor.SpawnRandom(vector3D3, Vector3.Normalize(vector3D - vector3D3)));
			}
			m_rightVector = Vector3.Zero;
		}

		private static Vector3 GetCorrectedDirection(Vector3 direction)
		{
			if (!m_currentTarget.HasValue)
			{
				return direction;
			}
			Vector3D vector3D = m_tgtPos = m_currentTarget.Value.Center;
			if (!MyGravityProviderSystem.IsPositionInNaturalGravity(vector3D))
			{
				return direction;
			}
			Vector3D vector3D2 = -Vector3D.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D));
			Vector3D vector3D3 = Vector3D.Normalize(Vector3D.Cross(vector3D2, direction));
			Vector3D vector3D4 = m_mirrorDir = Vector3D.Normalize(Vector3D.Cross(vector3D3, vector3D2));
			m_pltTgtDir = vector3D2;
			m_normalSun = vector3D3;
			double num = vector3D2.Dot(direction);
			if (num < 0.0 - HORIZON_ANGLE_FROM_ZENITH_RATIO)
			{
				return Vector3D.Reflect(-direction, vector3D4);
			}
			if (!(num < HORIZON_ANGLE_FROM_ZENITH_RATIO))
			{
				return direction;
			}
			MatrixD matrix = MatrixD.CreateFromAxisAngle(vector3D3, 0.0 - Math.Asin(HORIZON_ANGLE_FROM_ZENITH_RATIO));
			return Vector3D.Transform(vector3D4, matrix);
		}

		public static void StartDebugWave(Vector3 pos)
		{
			m_currentTarget = new BoundingSphereD(pos, 100.0);
			m_meteorcount = (int)(Math.Pow(m_currentTarget.Value.Radius, 2.0) * Math.PI / 3000.0);
			m_meteorcount /= ((MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM || MySession.Static.EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL) ? 1 : 8);
			m_meteorcount = MathHelper.Clamp(m_meteorcount, 1, 40);
			StartWave();
		}

		public override void Draw()
		{
			base.Draw();
			if (MyDebugDrawSettings.DEBUG_DRAW_METEORITS_DIRECTIONS)
			{
				Vector3D value = GetCorrectedDirection(MySector.DirectionToSunNormalized);
				MyRenderProxy.DebugDrawPoint(m_meteorHitPos, Color.White, depthRead: false);
				MyRenderProxy.DebugDrawText3D(m_meteorHitPos, "Hit position", Color.White, 0.5f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(m_tgtPos, m_tgtPos + 10f * MySector.DirectionToSunNormalized, Color.Yellow, Color.Yellow, depthRead: false);
				MyRenderProxy.DebugDrawText3D(m_tgtPos + 10f * MySector.DirectionToSunNormalized, "Sun direction (sd)", Color.Yellow, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
				MyRenderProxy.DebugDrawLine3D(m_tgtPos, m_tgtPos + 10.0 * value, Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawText3D(m_tgtPos + 10.0 * value, "Current meteorits direction (cd)", Color.Red, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
				if (MyGravityProviderSystem.IsPositionInNaturalGravity(m_tgtPos))
				{
					MyRenderProxy.DebugDrawLine3D(m_tgtPos, m_tgtPos + 10.0 * m_normalSun, Color.Blue, Color.Blue, depthRead: false);
					MyRenderProxy.DebugDrawText3D(m_tgtPos + 10.0 * m_normalSun, "Perpendicular to sd and n0 ", Color.Blue, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawLine3D(m_tgtPos, m_tgtPos + 10.0 * m_pltTgtDir, Color.Green, Color.Green, depthRead: false);
					MyRenderProxy.DebugDrawText3D(m_tgtPos + 10.0 * m_pltTgtDir, "Dir from center of planet to target (n0)", Color.Green, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawLine3D(m_tgtPos, m_tgtPos + 10.0 * m_mirrorDir, Color.Purple, Color.Purple, depthRead: false);
					MyRenderProxy.DebugDrawText3D(m_tgtPos + 10.0 * m_mirrorDir, "Horizon in plane n0 and sd (ho)", Color.Purple, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				}
			}
		}

		private static void CheckTargetValid()
		{
			if (m_currentTarget.HasValue)
			{
				m_tmpEntityList.Clear();
				BoundingSphereD boundingSphere = m_currentTarget.Value;
				m_tmpEntityList = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				if (m_tmpEntityList.OfType<MyCubeGrid>().ToList().Count == 0)
				{
					m_waveCounter = -1;
				}
				if (m_waveCounter >= 0 && MyMusicController.Static != null)
				{
					foreach (MyEntity tmpEntity in m_tmpEntityList)
					{
						if (tmpEntity is MyCharacter && MySession.Static != null && tmpEntity as MyCharacter == MySession.Static.LocalCharacter)
						{
							MyMusicController.Static.MeteorShowerIncoming();
							break;
						}
					}
				}
				m_tmpEntityList.Clear();
			}
		}

		private static void RescheduleEvent(object senderEvent)
		{
			if (m_waveCounter > WAVES_IN_SHOWER)
			{
				TimeSpan time = CalculateShowerTime(MySession.Static.EnvironmentHostility);
				MyGlobalEvents.RescheduleEvent((MyGlobalEventBase)senderEvent, time);
				m_waveCounter = -1;
				m_currentTarget = null;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateShowerTarget, m_currentTarget);
			}
			else
			{
				TimeSpan time2 = TimeSpan.FromSeconds((float)m_meteorcount / 5f + MyUtils.GetRandomFloat(2f, 5f));
				MyGlobalEvents.RescheduleEvent((MyGlobalEventBase)senderEvent, time2);
			}
		}

		public static double GetActivationTime(MyEnvironmentHostilityEnum hostility, double defaultMinMinutes, double defaultMaxMinutes)
		{
			MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), m_enviromentHostilityName[(int)hostility]));
			if (eventDefinition != null)
			{
				if (eventDefinition.MinActivationTime.HasValue)
				{
					defaultMinMinutes = eventDefinition.MinActivationTime.Value.TotalMinutes;
				}
				if (eventDefinition.MaxActivationTime.HasValue)
				{
					defaultMaxMinutes = eventDefinition.MaxActivationTime.Value.TotalMinutes;
				}
			}
			return MyUtils.GetRandomDouble(defaultMinMinutes, defaultMaxMinutes);
		}

		public static TimeSpan CalculateShowerTime(MyEnvironmentHostilityEnum hostility)
		{
			double value = 5.0;
			switch (hostility)
			{
			case MyEnvironmentHostilityEnum.NORMAL:
				value = GetActivationTime(hostility, 16.0, 24.0) / (double)MathHelper.Max(1f, m_lastTargetCount);
				value = MathHelper.Max(0.4, value);
				break;
			case MyEnvironmentHostilityEnum.CATACLYSM:
				value = GetActivationTime(hostility, 1.0, 1.5) / (double)MathHelper.Max(1f, m_lastTargetCount);
				value = MathHelper.Max(0.4, value);
				break;
			case MyEnvironmentHostilityEnum.CATACLYSM_UNREAL:
				value = GetActivationTime(hostility, 0.10000000149011612, 0.30000001192092896);
				break;
			}
			return TimeSpan.FromMinutes(value);
		}

		private static double GetMaxActivationTime(MyEnvironmentHostilityEnum environment)
		{
			double result = 0.0;
			switch (environment)
			{
			case MyEnvironmentHostilityEnum.NORMAL:
				result = 24.0;
				break;
			case MyEnvironmentHostilityEnum.CATACLYSM:
				result = 1.5;
				break;
			case MyEnvironmentHostilityEnum.CATACLYSM_UNREAL:
				result = 0.30000001192092896;
				break;
			}
			MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), m_enviromentHostilityName[(int)environment]));
			if (eventDefinition != null && eventDefinition.MaxActivationTime.HasValue)
			{
				result = eventDefinition.MaxActivationTime.Value.TotalMinutes;
			}
			return result;
		}

		public static TimeSpan CalculateShowerTime(MyEnvironmentHostilityEnum newHostility, MyEnvironmentHostilityEnum oldHostility, TimeSpan oldTime)
		{
			double totalMinutes = oldTime.TotalMinutes;
			double num = 1.0;
			if (oldHostility != 0)
			{
				num = totalMinutes / GetMaxActivationTime(oldHostility);
			}
			totalMinutes = num * GetMaxActivationTime(newHostility);
			return TimeSpan.FromMinutes(totalMinutes);
		}

		private static void GetTargets()
		{
			List<MyCubeGrid> list = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList();
			for (int i = 0; i < list.Count; i++)
			{
				if ((list[i].Max - list[i].Min + Vector3I.One).Size < 16 || !MySessionComponentTriggerSystem.Static.IsAnyTriggerActive(list[i]))
				{
					list.RemoveAt(i);
					i--;
				}
			}
			while (list.Count > 0)
			{
				MyCubeGrid myCubeGrid = list[MyUtils.GetRandomInt(list.Count - 1)];
				m_tmpHitGroup.Add(myCubeGrid);
				list.Remove(myCubeGrid);
				BoundingSphereD worldVolume = myCubeGrid.PositionComp.WorldVolume;
				bool flag = true;
				while (flag)
				{
					flag = false;
					foreach (MyCubeGrid item in m_tmpHitGroup)
					{
						worldVolume.Include(item.PositionComp.WorldVolume);
					}
					m_tmpHitGroup.Clear();
					worldVolume.Radius += 10.0;
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].PositionComp.WorldVolume.Intersects(worldVolume))
						{
							flag = true;
							m_tmpHitGroup.Add(list[j]);
							list.RemoveAt(j);
							j--;
						}
					}
				}
				worldVolume.Radius += 150.0;
				m_targetList.Add(worldVolume);
			}
			m_lastTargetCount = m_targetList.Count;
		}

		private static void ClearMeteorList()
		{
			m_meteorList.Clear();
		}

		private static void SetupDirVectors(Vector3 direction)
		{
			if (m_rightVector == Vector3.Zero)
			{
				direction.CalculatePerpendicularVector(out m_rightVector);
				m_downVector = MyUtils.Normalize(Vector3.Cross(direction, m_rightVector));
			}
		}

		[MyGlobalEventHandler(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWave")]
		public static void MeteorWave(object senderEvent)
		{
			MeteorWaveInternal(senderEvent);
		}

		[Event(null, 506)]
		[Reliable]
		[Broadcast]
		private static void UpdateShowerTarget([Serialize(MyObjectFlags.DefaultZero)] BoundingSphereD? target)
		{
			if (target.HasValue)
			{
				CurrentTarget = new BoundingSphereD(target.Value.Center, target.Value.Radius);
			}
			else
			{
				CurrentTarget = null;
			}
		}
	}
}
