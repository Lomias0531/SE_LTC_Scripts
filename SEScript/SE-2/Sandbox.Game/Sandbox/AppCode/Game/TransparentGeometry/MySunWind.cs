using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.AppCode.Game.TransparentGeometry
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, Priority = 1000)]
	internal class MySunWind : MySessionComponentBase
	{
		private class MySunWindBillboard
		{
			public Vector4 Color;

			public float Radius;

			public float InitialAngle;

			public float RotationSpeed;

			public Vector3 InitialAbsolutePosition;
		}

		private class MySunWindBillboardSmall : MySunWindBillboard
		{
			public float MaxDistance;

			public int TailBillboardsCount;

			public float TailBillboardsDistance;

			public float[] RadiusScales;
		}

		private struct MyEntityRayCastPair
		{
			public MyEntity Entity;

			public LineD _Ray;

			public Vector3D Position;

			public MyParticleEffect Particle;
		}

		public static bool IsActive;

		public static bool IsVisible;

		public static Vector3D Position;

		private static Vector3D m_initialSunWindPosition;

		private static Vector3D m_directionFromSunNormalized;

		private static PlaneD m_planeMiddle;

		private static PlaneD m_planeFront;

		private static PlaneD m_planeBack;

		private static double m_distanceToSunWind;

		private static Vector3D m_positionOnCameraLine;

		private static int m_timeLastUpdate;

		private static float m_speed;

		private static Vector3D m_rightVector;

		private static Vector3D m_downVector;

		private static float m_strength;

		public static Type[] DoNotIgnoreTheseTypes;

		private static MySunWindBillboard[][] m_largeBillboards;

		private static MySunWindBillboardSmall[][] m_smallBillboards;

		private static bool m_smallBillboardsStarted;

		private static List<IMyEntity> m_sunwindEntities;

		private static List<HkBodyCollision> m_intersectionLst;

		private static List<MyEntityRayCastPair> m_rayCastQueue;

		private static int m_computedMaxDistances;

		private static float m_deltaTime;

		private int m_rayCastCounter;

		private List<MyPhysics.HitInfo> m_hitLst = new List<MyPhysics.HitInfo>();

		static MySunWind()
		{
			IsActive = false;
			IsVisible = true;
			DoNotIgnoreTheseTypes = new Type[1]
			{
				typeof(MyVoxelMap)
			};
			m_sunwindEntities = new List<IMyEntity>();
			m_rayCastQueue = new List<MyEntityRayCastPair>();
		}

		public override void LoadData()
		{
			MyLog.Default.WriteLine("MySunWind.LoadData() - START");
			MyLog.Default.IncreaseIndent();
			m_intersectionLst = new List<HkBodyCollision>();
			m_largeBillboards = new MySunWindBillboard[MySunWindConstants.LARGE_BILLBOARDS_SIZE.X][];
			for (int i = 0; i < MySunWindConstants.LARGE_BILLBOARDS_SIZE.X; i++)
			{
				m_largeBillboards[i] = new MySunWindBillboard[MySunWindConstants.LARGE_BILLBOARDS_SIZE.Y];
				for (int j = 0; j < MySunWindConstants.LARGE_BILLBOARDS_SIZE.Y; j++)
				{
					m_largeBillboards[i][j] = new MySunWindBillboard();
					MySunWindBillboard obj = m_largeBillboards[i][j];
					obj.Radius = MyUtils.GetRandomFloat(20000f, 35000f);
					obj.InitialAngle = MyUtils.GetRandomRadian();
					obj.RotationSpeed = MyUtils.GetRandomSign() * MyUtils.GetRandomFloat(0.5f, 1.2f);
					obj.Color.X = MyUtils.GetRandomFloat(0.5f, 3f);
					obj.Color.Y = MyUtils.GetRandomFloat(0.5f, 1f);
					obj.Color.Z = MyUtils.GetRandomFloat(0.5f, 1f);
					obj.Color.W = MyUtils.GetRandomFloat(0.5f, 1f);
				}
			}
			m_smallBillboards = new MySunWindBillboardSmall[MySunWindConstants.SMALL_BILLBOARDS_SIZE.X][];
			for (int k = 0; k < MySunWindConstants.SMALL_BILLBOARDS_SIZE.X; k++)
			{
				m_smallBillboards[k] = new MySunWindBillboardSmall[MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y];
				for (int l = 0; l < MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y; l++)
				{
					m_smallBillboards[k][l] = new MySunWindBillboardSmall();
					MySunWindBillboardSmall mySunWindBillboardSmall = m_smallBillboards[k][l];
					mySunWindBillboardSmall.Radius = MyUtils.GetRandomFloat(250f, 500f);
					mySunWindBillboardSmall.InitialAngle = MyUtils.GetRandomRadian();
					mySunWindBillboardSmall.RotationSpeed = MyUtils.GetRandomSign() * MyUtils.GetRandomFloat(1.4f, 3.5f);
					mySunWindBillboardSmall.Color.X = MyUtils.GetRandomFloat(0.5f, 1f);
					mySunWindBillboardSmall.Color.Y = MyUtils.GetRandomFloat(0.2f, 0.5f);
					mySunWindBillboardSmall.Color.Z = MyUtils.GetRandomFloat(0.2f, 0.5f);
					mySunWindBillboardSmall.Color.W = MyUtils.GetRandomFloat(0.1f, 0.5f);
					mySunWindBillboardSmall.TailBillboardsCount = MyUtils.GetRandomInt(8, 14);
					mySunWindBillboardSmall.TailBillboardsDistance = MyUtils.GetRandomFloat(300f, 450f);
					mySunWindBillboardSmall.RadiusScales = new float[mySunWindBillboardSmall.TailBillboardsCount];
					for (int m = 0; m < mySunWindBillboardSmall.TailBillboardsCount; m++)
					{
						mySunWindBillboardSmall.RadiusScales[m] = MyUtils.GetRandomFloat(0.7f, 1f);
					}
				}
			}
			MyLog.Default.DecreaseIndent();
			MyLog.Default.WriteLine("MySunWind.LoadData() - END");
		}

		protected override void UnloadData()
		{
			MyLog.Default.WriteLine("MySunWind.UnloadData - START");
			MyLog.Default.IncreaseIndent();
			IsActive = false;
			MyLog.Default.DecreaseIndent();
			MyLog.Default.WriteLine("MySunWind.UnloadData - END");
		}

		public static void Start()
		{
			IsActive = true;
			m_smallBillboardsStarted = false;
			m_timeLastUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			Vector3D value = MySector.DirectionToSunNormalized;
			m_initialSunWindPosition = value * 30000.0 / 2.0;
			m_directionFromSunNormalized = -value;
			StopCue();
			m_speed = MyUtils.GetRandomFloat(1300f, 1500f);
			m_strength = MyUtils.GetRandomFloat(0f, 1f);
			m_directionFromSunNormalized.CalculatePerpendicularVector(out m_rightVector);
			m_downVector = MyUtils.Normalize(Vector3D.Cross(m_directionFromSunNormalized, m_rightVector));
			StartBillboards();
			m_computedMaxDistances = 0;
			m_deltaTime = 0f;
			m_sunwindEntities.Clear();
		}

		public override void UpdateBeforeSimulation()
		{
			int num = 0;
			if (m_rayCastQueue.Count > 0 && m_rayCastCounter % 20 == 0)
			{
				while (num < 50 && m_rayCastQueue.Count > 0)
				{
					int randomInt = MyUtils.GetRandomInt(m_rayCastQueue.Count - 1);
					MyEntity entity = m_rayCastQueue[randomInt].Entity;
					_ = m_rayCastQueue[randomInt];
					Vector3D position = m_rayCastQueue[randomInt].Position;
					MyParticleEffect particle = m_rayCastQueue[randomInt].Particle;
					if (entity is MyCubeGrid)
					{
						particle.Stop();
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						MatrixD worldMatrixNormalizedInv = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
						if (myCubeGrid.BlocksDestructionEnabled)
						{
							myCubeGrid.Physics.ApplyDeformation(6f, 3f, 3f, Vector3.Transform(position, worldMatrixNormalizedInv), Vector3.Normalize(Vector3.Transform(m_directionFromSunNormalized, worldMatrixNormalizedInv)), MyDamageType.Environment, 0f, 0f, 0L);
						}
						m_rayCastQueue.RemoveAt(randomInt);
						m_hitLst.Clear();
						break;
					}
				}
			}
			m_rayCastCounter++;
			if (!IsActive)
			{
				return;
			}
			float num2 = ((float)MySandboxGame.TotalGamePlayTimeInMilliseconds - (float)m_timeLastUpdate) / 1000f;
			m_timeLastUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MySandboxGame.IsPaused)
			{
				return;
			}
			m_deltaTime += num2;
			float num3 = m_speed * m_deltaTime;
			if (num3 >= 60000f)
			{
				IsActive = false;
				StopCue();
				return;
			}
			Vector3D point = (MySession.Static.LocalCharacter == null) ? Vector3D.Zero : MySession.Static.LocalCharacter.Entity.WorldMatrix.Translation;
			m_planeMiddle = new PlaneD(m_initialSunWindPosition + m_directionFromSunNormalized * num3, m_directionFromSunNormalized);
			m_distanceToSunWind = m_planeMiddle.DistanceToPoint(ref point);
			m_positionOnCameraLine = -m_directionFromSunNormalized * m_distanceToSunWind;
			Vector3D vector3D = m_positionOnCameraLine + m_directionFromSunNormalized * 2000.0;
			Vector3D position2 = m_positionOnCameraLine + m_directionFromSunNormalized * -2000.0;
			m_planeFront = new PlaneD(vector3D, m_directionFromSunNormalized);
			m_planeBack = new PlaneD(position2, m_directionFromSunNormalized);
			m_planeFront.DistanceToPoint(ref point);
			m_planeBack.DistanceToPoint(ref point);
			int num4 = 0;
			while (num4 < m_sunwindEntities.Count)
			{
				if (m_sunwindEntities[num4].MarkedForClose)
				{
					m_sunwindEntities.RemoveAtFast(num4);
				}
				else
				{
					num4++;
				}
			}
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromDir(m_directionFromSunNormalized, m_downVector));
			Vector3 halfExtents = new Vector3(10000f, 10000f, 2000f);
			MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(vector3D + m_directionFromSunNormalized * 2500.0, halfExtents, rotation), Color.Red.ToVector3(), 1f, depthRead: false, smooth: false);
			if (m_rayCastCounter == 120)
			{
				Vector3D translation = vector3D + m_directionFromSunNormalized * 2500.0;
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_intersectionLst, 15);
				foreach (HkBodyCollision item in m_intersectionLst)
				{
					IMyEntity collisionEntity = item.GetCollisionEntity();
					if (!(collisionEntity is MyVoxelMap) && !m_sunwindEntities.Contains(collisionEntity))
					{
						m_sunwindEntities.Add(collisionEntity);
					}
				}
				m_intersectionLst.Clear();
				int num5;
				for (num5 = 0; num5 < m_sunwindEntities.Count; num5++)
				{
					IMyEntity myEntity = m_sunwindEntities[num5];
					if (myEntity is MyCubeGrid)
					{
						MyCubeGrid myCubeGrid2 = myEntity as MyCubeGrid;
						BoundingBoxD worldAABB = myCubeGrid2.PositionComp.WorldAABB;
						double num6 = (worldAABB.Center - worldAABB.Min).Length();
						double num7 = ((worldAABB.Center - worldAABB.Min) / m_rightVector).AbsMin();
						double num8 = ((worldAABB.Center - worldAABB.Min) / m_downVector).AbsMin();
						Vector3I vector3I = myCubeGrid2.Max - myCubeGrid2.Min;
						Math.Max(vector3I.X, Math.Max(vector3I.Y, vector3I.Z));
						_ = myCubeGrid2.PositionComp.WorldMatrixNormalizedInv;
						Vector3D value = worldAABB.Center - num7 * m_rightVector - num8 * m_downVector;
						for (int i = 0; (double)i < num7 * 2.0; i += ((myCubeGrid2.GridSizeEnum == MyCubeSize.Large) ? 25 : 10))
						{
							for (int j = 0; (double)j < num8 * 2.0; j += ((myCubeGrid2.GridSizeEnum == MyCubeSize.Large) ? 25 : 10))
							{
								Vector3D vector3D2 = value + i * m_rightVector + j * m_downVector;
								vector3D2 += (float)num6 * m_directionFromSunNormalized;
								Vector3 randomVector3CircleNormalized = MyUtils.GetRandomVector3CircleNormalized();
								float randomFloat = MyUtils.GetRandomFloat(0f, (myCubeGrid2.GridSizeEnum == MyCubeSize.Large) ? 10 : 5);
								vector3D2 += m_rightVector * randomVector3CircleNormalized.X * randomFloat + m_downVector * randomVector3CircleNormalized.Z * randomFloat;
								LineD ray = new LineD(vector3D2 - m_directionFromSunNormalized * (float)num6, vector3D2);
								if (myCubeGrid2.RayCastBlocks(ray.From, ray.To).HasValue)
								{
									ray.From = vector3D2 - m_directionFromSunNormalized * 1000.0;
									MyPhysics.CastRay(ray.From, ray.To, m_hitLst);
									m_rayCastCounter++;
									if (m_hitLst.Count == 0 || m_hitLst[0].HkHitInfo.GetHitEntity() != myCubeGrid2.Components)
									{
										m_hitLst.Clear();
										continue;
									}
									MyParticlesManager.TryCreateParticleEffect("Dummy", MatrixD.CreateWorld(m_hitLst[0].Position, Vector3D.Forward, Vector3D.Up), out MyParticleEffect effect);
									m_rayCastQueue.Add(new MyEntityRayCastPair
									{
										Entity = myCubeGrid2,
										_Ray = ray,
										Position = m_hitLst[0].Position,
										Particle = effect
									});
								}
							}
						}
						m_sunwindEntities.Remove(myCubeGrid2);
						num5--;
					}
					else
					{
						m_sunwindEntities.Remove(myEntity);
						num5--;
					}
				}
				m_rayCastCounter = 0;
			}
			if (m_distanceToSunWind <= 10000.0)
			{
				m_smallBillboardsStarted = true;
			}
			ComputeMaxDistances();
			base.UpdateBeforeSimulation();
		}

		public static bool IsActiveForHudWarning()
		{
			return true;
		}

		private static void StopCue()
		{
		}

		public static Vector4 GetSunColor()
		{
			float num = (float)(1.0 - MathHelper.Clamp(Math.Abs(m_distanceToSunWind) / 10000.0, 0.0, 1.0));
			num *= MathHelper.Lerp(3f, 4f, m_strength);
			return new Vector4(MySector.SunProperties.EnvironmentLight.SunColorRaw, 1f) * (1f + num);
		}

		public static float GetParticleDustFieldAlpha()
		{
			return (float)Math.Pow(MathHelper.Clamp(Math.Abs(m_distanceToSunWind) / 27000.0, 0.0, 1.0), 4.0);
		}

		public override void Draw()
		{
			if (IsActive && IsVisible)
			{
				float num = m_speed * m_deltaTime;
				_ = (Vector3)(m_directionFromSunNormalized * num);
				base.Draw();
			}
		}

		private static void StartBillboards()
		{
			for (int i = 0; i < MySunWindConstants.LARGE_BILLBOARDS_SIZE.X; i++)
			{
				for (int j = 0; j < MySunWindConstants.LARGE_BILLBOARDS_SIZE.Y; j++)
				{
					MySunWindBillboard obj = m_largeBillboards[i][j];
					Vector3 vector = new Vector3(MyUtils.GetRandomFloat(-50f, 50f), MyUtils.GetRandomFloat(-50f, 50f), MyUtils.GetRandomFloat(-50f, 50f));
					Vector3 vector2 = new Vector3((float)(i - MySunWindConstants.LARGE_BILLBOARDS_SIZE_HALF.X) * 7500f, (float)(j - MySunWindConstants.LARGE_BILLBOARDS_SIZE_HALF.Y) * 7500f, (float)(i - MySunWindConstants.LARGE_BILLBOARDS_SIZE_HALF.X) * 7500f * 0.2f);
					obj.InitialAbsolutePosition = m_initialSunWindPosition + m_rightVector * (vector.X + vector2.X) + m_downVector * (vector.Y + vector2.Y) + -1.0 * m_directionFromSunNormalized * (vector.Z + vector2.Z);
				}
			}
			Vector3D value = (MySession.Static.LocalCharacter == null) ? Vector3D.Zero : (MySession.Static.LocalCharacter.Entity.WorldMatrix.Translation - m_directionFromSunNormalized * 30000.0 / 3.0);
			for (int k = 0; k < MySunWindConstants.SMALL_BILLBOARDS_SIZE.X; k++)
			{
				for (int l = 0; l < MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y; l++)
				{
					MySunWindBillboardSmall obj2 = m_smallBillboards[k][l];
					Vector2 vector3 = new Vector2(MyUtils.GetRandomFloat(-300f, 300f), MyUtils.GetRandomFloat(-300f, 300f));
					Vector2 vector4 = new Vector2((float)(k - MySunWindConstants.SMALL_BILLBOARDS_SIZE_HALF.X) * 350f, (float)(l - MySunWindConstants.SMALL_BILLBOARDS_SIZE_HALF.Y) * 350f);
					obj2.InitialAbsolutePosition = value + m_rightVector * (vector3.X + vector4.X) + m_downVector * (vector3.Y + vector4.Y);
				}
			}
		}

		private static void ComputeMaxDistances()
		{
			int num = MySunWindConstants.SMALL_BILLBOARDS_SIZE.X * MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y;
			if (m_computedMaxDistances < num)
			{
				int num2 = (int)((float)num / 1f / 0.0166666675f);
				while (m_computedMaxDistances < num && num2 > 0)
				{
					int num3 = m_computedMaxDistances % MySunWindConstants.SMALL_BILLBOARDS_SIZE.Y;
					int num4 = m_computedMaxDistances / MySunWindConstants.SMALL_BILLBOARDS_SIZE.X;
					ComputeMaxDistance(m_smallBillboards[num3][num4]);
					m_computedMaxDistances++;
					num2--;
				}
			}
		}

		private static void ComputeMaxDistance(MySunWindBillboardSmall billboard)
		{
			Vector3 value = m_directionFromSunNormalized * 30000.0;
			Vector3D value2 = -m_directionFromSunNormalized * 30000.0;
			LineD line = new LineD(value + billboard.InitialAbsolutePosition + value2, billboard.InitialAbsolutePosition + m_directionFromSunNormalized * 60000.0);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, null, null);
			if (intersectionWithLine.HasValue)
			{
				billboard.MaxDistance = intersectionWithLine.Value.Triangle.Distance - billboard.Radius;
			}
			else
			{
				billboard.MaxDistance = 60000f;
			}
		}
	}
}
