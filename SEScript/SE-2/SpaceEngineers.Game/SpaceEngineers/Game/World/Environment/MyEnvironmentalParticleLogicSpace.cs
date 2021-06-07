using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.World.Environment
{
	[MyEnvironmentalParticleLogicType(typeof(MyObjectBuilder_EnvironmentalParticleLogicSpace), true)]
	internal class MyEnvironmentalParticleLogicSpace : MyEnvironmentalParticleLogic
	{
		private int m_lastParticleSpawn;

		private float m_particlesLeftToSpawn;

		private bool m_isPlanetary;

		public MyEntity ControlledEntity => MySession.Static.ControlledEntity as MyEntity;

		public Vector3 ControlledVelocity
		{
			get
			{
				if (!(ControlledEntity is MyCockpit) && !(ControlledEntity is MyRemoteControl))
				{
					return ControlledEntity.Physics.LinearVelocity;
				}
				return ControlledEntity.GetTopMostParent().Physics.LinearVelocity;
			}
		}

		public bool ShouldDrawParticles
		{
			get
			{
				if (HasControlledNonZeroVelocity())
				{
					return !IsInGridAABB();
				}
				return false;
			}
		}

		public override void Init(MyObjectBuilder_EnvironmentalParticleLogic builder)
		{
			base.Init(builder);
			_ = (builder is MyObjectBuilder_EnvironmentalParticleLogicSpace);
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			try
			{
				if (ShouldDrawParticles && !(ControlledVelocity.Length() < 10f))
				{
					float particleSpawnDistance = base.ParticleSpawnDistance;
					double num = Math.PI / 2.0;
					float num2 = 1f;
					Vector3 value = ControlledVelocity - 8.5f * Vector3.Normalize(ControlledVelocity);
					float num3 = 4f * particleSpawnDistance * particleSpawnDistance * num2;
					m_isPlanetary = IsNearPlanet();
					if (!m_isPlanetary || MyFakes.ENABLE_STARDUST_ON_PLANET)
					{
						m_particlesLeftToSpawn += (0.25f + MyRandom.Instance.NextFloat() * 1.25f) * value.Length() * num3 * base.ParticleDensity * 16f;
						if (!(m_particlesLeftToSpawn < 1f))
						{
							double num4 = num / 2.0;
							double num5 = num4 + num;
							double num6 = num4 + (double)MyRandom.Instance.NextFloat() * (num5 - num4);
							double num7 = num4 + (double)MyRandom.Instance.NextFloat() * (num5 - num4);
							float num8 = 6f;
							while (m_particlesLeftToSpawn-- >= 1f)
							{
								float num9 = MathF.PI / 180f;
								if (Math.Abs(num6 - Math.PI / 2.0) < (double)(num8 * num9) && Math.Abs(num7 - Math.PI / 2.0) < (double)(num8 * num9))
								{
									num6 += (double)((float)Math.Sign(MyRandom.Instance.NextFloat()) * num8 * num9);
									num7 += (double)((float)Math.Sign(MyRandom.Instance.NextFloat()) * num8 * num9);
								}
								float scaleFactor = (float)Math.Sin(num7);
								float scaleFactor2 = (float)Math.Cos(num7);
								float scaleFactor3 = (float)Math.Sin(num6);
								float scaleFactor4 = (float)Math.Cos(num6);
								Vector3 upVector = MySector.MainCamera.UpVector;
								Vector3 vector = Vector3.Normalize(value);
								Vector3 vector2 = Vector3.Cross(vector, -upVector);
								if (Vector3.IsZero(vector2))
								{
									vector2 = Vector3.CalculatePerpendicularVector(vector);
								}
								else
								{
									vector2.Normalize();
								}
								Vector3 value2 = Vector3.Cross(vector, vector2);
								Vector3 position = MySector.MainCamera.Position + particleSpawnDistance * (value2 * scaleFactor2 + vector2 * scaleFactor * scaleFactor4 + vector * scaleFactor * scaleFactor3);
								Spawn(position);
								m_lastParticleSpawn = MySandboxGame.TotalGamePlayTimeInMilliseconds;
							}
						}
					}
				}
			}
			finally
			{
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (!ShouldDrawParticles)
			{
				DeactivateAll();
				m_particlesLeftToSpawn = 0f;
			}
			base.UpdateAfterSimulation();
		}

		public override void Draw()
		{
			base.Draw();
			if (ShouldDrawParticles)
			{
				Vector3 directionNormalized = -Vector3.Normalize(ControlledVelocity);
				float num = 0.025f;
				float num2 = (float)MathHelper.Clamp(ControlledVelocity.Length() / 50f, 0.0, 1.0);
				float num3 = 1f;
				float num4 = 1f;
				if (m_isPlanetary)
				{
					num3 = 1.5f;
					num4 = 3f;
				}
				foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
				{
					if (activeParticle.Active)
					{
						if (m_isPlanetary)
						{
							MyTransparentGeometry.AddLineBillboard(activeParticle.MaterialPlanet, activeParticle.ColorPlanet, activeParticle.Position, directionNormalized, num2 * num4, num * num3, MyBillboard.BlendTypeEnum.LDR);
						}
						else
						{
							MyTransparentGeometry.AddLineBillboard(activeParticle.Material, activeParticle.Color, activeParticle.Position, directionNormalized, num2 * num4, num * num3, MyBillboard.BlendTypeEnum.LDR);
						}
					}
				}
			}
		}

		private bool IsInGridAABB()
		{
			bool result = false;
			BoundingSphereD boundingSphere = new BoundingSphereD(MySector.MainCamera.Position, 0.10000000149011612);
			List<MyEntity> list = null;
			try
			{
				list = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				foreach (MyEntity item in list)
				{
					MyCubeGrid myCubeGrid = item as MyCubeGrid;
					if (myCubeGrid != null && myCubeGrid.GridSizeEnum != MyCubeSize.Small)
					{
						return true;
					}
				}
				return result;
			}
			finally
			{
				list?.Clear();
			}
		}

		private bool HasControlledNonZeroVelocity()
		{
			MyEntity myEntity = ControlledEntity;
			if (myEntity == null || MySession.Static.IsCameraUserControlledSpectator())
			{
				return false;
			}
			MyRemoteControl myRemoteControl = myEntity as MyRemoteControl;
			if (myRemoteControl != null)
			{
				myEntity = myRemoteControl.GetTopMostParent();
			}
			MyCockpit myCockpit = myEntity as MyCockpit;
			if (myCockpit != null)
			{
				myEntity = myCockpit.GetTopMostParent();
			}
			if (myEntity != null && myEntity.Physics != null && myEntity.Physics.LinearVelocity != Vector3.Zero)
			{
				return true;
			}
			return false;
		}

		private bool IsNearPlanet()
		{
			if (ControlledEntity == null)
			{
				return false;
			}
			return !Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(ControlledEntity.PositionComp.GetPosition()));
		}
	}
}
