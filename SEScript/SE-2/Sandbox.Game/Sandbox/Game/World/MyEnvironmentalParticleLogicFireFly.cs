using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
	[MyEnvironmentalParticleLogicType(typeof(MyObjectBuilder_EnvironmentalParticleLogicFireFly), true)]
	public class MyEnvironmentalParticleLogicFireFly : MyEnvironmentalParticleLogic
	{
		private struct PathData
		{
			public const int PathPointCount = 16;

			public Vector3D[] PathPoints;
		}

		private int m_particleSpawnInterval = 60;

		private float m_particleSpawnIntervalRandomness = 0.5f;

		private int m_particleSpawnCounter;

		private static int m_updateCounter;

		private const int m_killDeadParticlesInterval = 60;

		private List<HkBodyCollision> m_bodyCollisions = new List<HkBodyCollision>();

		private List<MyEnvironmentItems.ItemInfo> m_tmpItemInfos = new List<MyEnvironmentItems.ItemInfo>();

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!MyFakes.ENABLE_PLANET_FIREFLIES || m_particleSpawnCounter-- > 0)
			{
				return;
			}
			m_particleSpawnCounter = 0;
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity != null)
			{
				MyEntity topMostParent = myEntity.GetTopMostParent();
				if (topMostParent != null)
				{
					try
					{
						m_particleSpawnCounter = (int)Math.Round((float)m_particleSpawnCounter + (float)m_particleSpawnCounter * m_particleSpawnIntervalRandomness * (MyRandom.Instance.NextFloat() * 2f - 1f));
						if (!((MyRandom.Instance.FloatNormal() + 3f) / 6f > m_particleDensity) && !(MyGravityProviderSystem.CalculateNaturalGravityInPoint(MySector.MainCamera.Position).Dot(MySector.DirectionToSunNormalized) <= 0f))
						{
							Vector3 vector = Vector3.Zero;
							if (topMostParent.Physics != null && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator)
							{
								vector = topMostParent.Physics.LinearVelocity;
							}
							float num = vector.Length();
							MyCharacter myCharacter = topMostParent as MyCharacter;
							float num2 = MyGridPhysics.ShipMaxLinearVelocity();
							if (myCharacter != null && myCharacter.Physics != null && myCharacter.Physics.CharacterProxy != null)
							{
								num2 = myCharacter.Physics.CharacterProxy.CharacterFlyingMaxLinearVelocity();
							}
							Vector3 vector2 = Vector3.One * m_particleSpawnDistance;
							if (num / num2 > 1f)
							{
								vector2 += 10f * vector / num2;
							}
							_ = MySector.MainCamera.Position + vector;
							if (MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position) != null)
							{
								_ = MySector.MainCamera.Position;
								Vector3D vector3D = default(Vector3D);
								if (m_tmpItemInfos.Count != 0)
								{
									int index = MyRandom.Instance.Next(0, m_tmpItemInfos.Count - 1);
									vector3D = m_tmpItemInfos[index].Transform.Position;
									MyEnvironmentalParticle myEnvironmentalParticle = Spawn(vector3D);
									if (myEnvironmentalParticle != null)
									{
										InitializePath(myEnvironmentalParticle);
									}
								}
							}
						}
					}
					finally
					{
						m_bodyCollisions.Clear();
						m_tmpItemInfos.Clear();
					}
				}
			}
		}

		public override void Simulate()
		{
			base.Simulate();
			foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
			{
				_ = activeParticle.Position;
				Vector3D interpolatedPosition = GetInterpolatedPosition(activeParticle);
				activeParticle.Position = interpolatedPosition;
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (m_updateCounter++ >= 60)
			{
				foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
				{
					if (IsInGridAABB(activeParticle.Position))
					{
						activeParticle.Deactivate();
					}
				}
				m_updateCounter = 0;
			}
			base.UpdateAfterSimulation();
		}

		public override void Draw()
		{
			base.Draw();
			float thickness = 0.075f / 1.66f;
			float length = 0.075f;
			foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
			{
				if (activeParticle.Active)
				{
					Vector4 color = activeParticle.Color;
					float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - activeParticle.BirthTime) / (float)activeParticle.LifeTime;
					if (num < 0.1f)
					{
						color = activeParticle.Color * num;
					}
					else if (num > 0.9f)
					{
						color = activeParticle.Color * (1f - num);
					}
					Vector3D v = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize(activeParticle.Position - MySector.MainCamera.Position));
					MyTransparentGeometry.AddLineBillboard(activeParticle.Material, color, activeParticle.Position, v, length, thickness);
				}
			}
		}

		private void InitializePath(MyEnvironmentalParticle particle)
		{
			PathData pathData = default(PathData);
			if (pathData.PathPoints == null)
			{
				pathData.PathPoints = new Vector3D[18];
			}
			Vector3D vector3D = Vector3D.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(particle.Position));
			pathData.PathPoints[1] = particle.Position - Vector3D.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(particle.Position)) * MyRandom.Instance.NextFloat() * 2.5;
			for (int i = 2; i < 17; i++)
			{
				float num = 5f;
				Vector3D value = Vector3D.Normalize(new Vector3D(MyRandom.Instance.NextFloat(), MyRandom.Instance.NextFloat(), MyRandom.Instance.NextFloat()) * 2.0 - Vector3D.One);
				pathData.PathPoints[i] = pathData.PathPoints[i - 1] + value * (MyRandom.Instance.NextFloat() + 1f) * num - vector3D / (float)i * num;
			}
			pathData.PathPoints[0] = pathData.PathPoints[1] - vector3D;
			pathData.PathPoints[17] = pathData.PathPoints[16] + Vector3D.Normalize(pathData.PathPoints[16] - pathData.PathPoints[15]);
			particle.UserData = pathData;
		}

		private Vector3D GetInterpolatedPosition(MyEnvironmentalParticle particle)
		{
			Vector3D result = particle.Position;
			if (particle.UserData == null)
			{
				return result;
			}
			double num = MathHelper.Clamp((double)(MySandboxGame.TotalGamePlayTimeInMilliseconds - particle.BirthTime) / (double)particle.LifeTime, 0.0, 1.0);
			int num2 = 14;
			int num3 = 1 + (int)(num * (double)num2);
			float num4 = (float)(num * (double)num2 - Math.Truncate(num * (double)num2));
			PathData value = (particle.UserData as PathData?).Value;
			result = Vector3D.CatmullRom(value.PathPoints[num3 - 1], value.PathPoints[num3], value.PathPoints[num3 + 1], value.PathPoints[num3 + 2], num4);
			if (!result.IsValid())
			{
				result = particle.Position;
			}
			return result;
		}

		private bool IsInGridAABB(Vector3D worldPosition)
		{
			BoundingSphereD boundingSphere = new BoundingSphereD(worldPosition, 0.10000000149011612);
			List<MyEntity> list = null;
			try
			{
				list = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				foreach (MyEntity item in list)
				{
					if (item is MyCubeGrid)
					{
						return true;
					}
				}
			}
			finally
			{
				list?.Clear();
			}
			return false;
		}
	}
}
