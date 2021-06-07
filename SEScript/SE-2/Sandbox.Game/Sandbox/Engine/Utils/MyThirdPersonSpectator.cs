using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyThirdPersonSpectator : MySessionComponentBase
	{
		private enum MyCameraRaycastResult
		{
			Ok,
			FoundOccluder,
			FoundOccluderNoSpace
		}

		public class SpringInfo
		{
			public float Stiffness;

			public float Dampening;

			public float Mass;

			public float RotationGain;

			public SpringInfo(float stiffness, float dampening, float mass, float rotationGain)
			{
				Stiffness = stiffness;
				Dampening = dampening;
				Mass = mass;
				RotationGain = rotationGain;
			}

			public SpringInfo(SpringInfo spring)
			{
				Setup(spring);
			}

			public void Setup(SpringInfo spring)
			{
				Stiffness = spring.Stiffness;
				Dampening = spring.Dampening;
				Mass = spring.Mass;
			}
		}

		public static MyThirdPersonSpectator Static;

		public const float MIN_VIEWER_DISTANCE = 2.5f;

		public const float MAX_VIEWER_DISTANCE = 200f;

		public const float DEFAULT_CAMERA_RADIUS_CUBEGRID_MUL = 1.5f;

		public const float DEFAULT_CAMERA_RADIUS_CHARACTER = 0.125f;

		public const float CAMERA_MAX_RAYCAST_DISTANCE = 500f;

		private readonly Vector3D m_initialLookAtDirection = Vector3D.Normalize(new Vector3D(0.0, 5.0, 12.0));

		private readonly Vector3D m_initialLookAtDirectionCharacter = Vector3D.Normalize(new Vector3D(0.0, 0.0, 12.0));

		private const float m_lookAtOffsetY = 0f;

		private const double m_lookAtDefaultLength = 2.5999999046325684;

		private int m_positionSafeZoomingOutDefaultTimeoutMs = 700;

		private bool m_disableSpringThisFrame;

		private float m_currentCameraRadius = 0.125f;

		private bool m_zoomingOutSmoothly;

		private const int SAFE_START_FILTER_FRAME_COUNT = 20;

		private readonly MyAverageFiltering m_safeStartSmoothing = new MyAverageFiltering(20);

		private float m_safeStartSmoothingFiltering = 1f;

		private Vector3D m_lookAt;

		private Vector3D m_clampedlookAt;

		private Vector3D m_transformedLookAt;

		private Vector3D m_target;

		private Vector3D m_lastTarget;

		private MatrixD m_targetOrientation = MatrixD.Identity;

		private Vector3D m_position;

		private Vector3D m_desiredPosition;

		private Vector3D m_positionSafe;

		private float m_positionSafeZoomingOutParam;

		private int m_positionSafeZoomingOutTimeout;

		private float m_lastRaycastDist = float.PositiveInfinity;

		private TimeSpan m_positionCurrentIsSafeSinceTime = TimeSpan.Zero;

		private const int POSITION_IS_SAFE_TIMEOUT_MS = 1000;

		public SpringInfo NormalSpring;

		public SpringInfo NormalSpringCharacter;

		private float m_springChangeTime;

		private SpringInfo m_currentSpring;

		private Vector3 m_velocity;

		private float m_angleVelocity;

		private Quaternion m_orientation;

		private Matrix m_orientationMatrix;

		private readonly List<MyPhysics.HitInfo> m_raycastList = new List<MyPhysics.HitInfo>(64);

		private readonly List<HkBodyCollision> m_collisionList = new List<HkBodyCollision>(64);

		private readonly List<MyEntity> m_entityList = new List<MyEntity>();

		private bool m_saveSettings;

		private bool m_debugDraw;

		private bool m_enableDebugDrawTrail;

		private double m_safeMinimumDistance = 2.5;

		private double m_safeMaximumDistance = 200.0;

		private float m_safeMaximumDistanceTimeout;

		private Sandbox.Game.Entities.IMyControllableEntity m_lastControllerEntity;

		private List<Vector3D> m_debugLastSpectatorPositions;

		private List<Vector3D> m_debugLastSpectatorDesiredPositions;

		private float m_lastZoomingOutSpeed;

		private BoundingBoxD m_safeAABB;

		public bool EnableDebugDraw
		{
			get
			{
				return m_debugDraw;
			}
			set
			{
				m_debugDraw = value;
			}
		}

		public MyThirdPersonSpectator()
		{
			NormalSpring = new SpringInfo(14000f, 2000f, 94f, 0.05f);
			NormalSpringCharacter = new SpringInfo(30000f, 2500f, 40f, 0.2f);
			m_currentSpring = NormalSpring;
			m_lookAt = m_initialLookAtDirectionCharacter * 2.5999999046325684;
			m_clampedlookAt = m_lookAt;
			m_saveSettings = false;
			ResetViewerDistance();
		}

		public override void LoadData()
		{
			Static = this;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public void ResetSpring()
		{
			m_position = m_desiredPosition;
			m_disableSpringThisFrame = true;
		}

		public void Update()
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = MySession.Static.ControlledEntity;
			if (myControllableEntity == null || myControllableEntity.Entity == null)
			{
				myControllableEntity = (MySession.Static.CameraController as Sandbox.Game.Entities.IMyControllableEntity);
				if (myControllableEntity == null || myControllableEntity.Entity == null)
				{
					return;
				}
			}
			MyEntity controlledEntity = GetControlledEntity(myControllableEntity);
			m_lastTarget = m_target;
			if (controlledEntity != null && controlledEntity.PositionComp != null)
			{
				MyPositionComponentBase positionComp = controlledEntity.PositionComp;
				_ = positionComp.LocalAABB;
				_ = positionComp.LocalAABB;
				MatrixD headMatrix = myControllableEntity.GetHeadMatrix(includeY: true);
				MyCharacter myCharacter = controlledEntity as MyCharacter;
				m_target = headMatrix.Translation;
				if (myCharacter != null)
				{
					m_currentCameraRadius = 0.125f;
					m_currentSpring = NormalSpringCharacter;
				}
				else
				{
					m_currentSpring = NormalSpring;
					if (controlledEntity is MyTerminalBlock)
					{
						m_currentCameraRadius = 0.5f;
					}
					else
					{
						MyCubeGrid myCubeGrid = controlledEntity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							m_currentCameraRadius = myCubeGrid.GridSize;
						}
					}
				}
				if (myCharacter == null || !myCharacter.IsDead)
				{
					bool flag = !m_disableSpringThisFrame;
					flag &= !MyDX9Gui.LookaroundEnabled;
					m_targetOrientation = MatrixD.Lerp(m_targetOrientation, headMatrix.GetOrientation(), flag ? m_currentSpring.RotationGain : 1f);
				}
				m_transformedLookAt = Vector3D.Transform(m_clampedlookAt, m_targetOrientation);
				m_desiredPosition = m_target + m_transformedLookAt;
				m_position += m_target - m_lastTarget;
				m_positionSafe += m_target - m_lastTarget;
			}
			else
			{
				MatrixD headMatrix2 = myControllableEntity.GetHeadMatrix(includeY: true);
				m_target = headMatrix2.Translation;
				m_targetOrientation = headMatrix2.GetOrientation();
				m_transformedLookAt = Vector3D.Transform(m_clampedlookAt, m_targetOrientation);
				m_position = m_desiredPosition;
			}
			if (myControllableEntity != m_lastControllerEntity)
			{
				m_disableSpringThisFrame = true;
				m_lastTarget = m_target;
				m_lastControllerEntity = myControllableEntity;
			}
			Vector3D position = m_position;
			if (m_disableSpringThisFrame)
			{
				m_position = m_desiredPosition;
				m_velocity = Vector3.Zero;
			}
			else
			{
				m_position = m_desiredPosition;
				m_velocity = Vector3.Zero;
			}
			if (controlledEntity != null)
			{
				if (!controlledEntity.Closed)
				{
					HandleIntersection(controlledEntity, ref m_target);
				}
				else
				{
					m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
				}
			}
			if (m_saveSettings)
			{
				MySession.Static.SaveControlledEntityCameraSettings(isFirstPerson: false);
				m_saveSettings = false;
			}
			if (m_disableSpringThisFrame)
			{
				double amount = 0.8;
				m_position = Vector3D.Lerp(position, m_desiredPosition, amount);
				m_velocity = Vector3.Zero;
				m_disableSpringThisFrame = (Vector3D.DistanceSquared(m_position, m_desiredPosition) > (double)(m_currentCameraRadius * m_currentCameraRadius));
			}
			DebugDrawTrail();
		}

		private static MyEntity GetControlledEntity(Sandbox.Game.Entities.IMyControllableEntity genericControlledEntity)
		{
			if (genericControlledEntity == null)
			{
				return null;
			}
			MyRemoteControl myRemoteControl = genericControlledEntity as MyRemoteControl;
			MyEntity myEntity = genericControlledEntity.Entity;
			if (myRemoteControl != null)
			{
				MyEntity myEntity2 = myRemoteControl.PreviousControlledEntity as MyEntity;
				if (myEntity2 != null)
				{
					myEntity = myEntity2;
				}
				else if (myRemoteControl.Pilot != null)
				{
					myEntity = myRemoteControl.Pilot;
				}
			}
			while (myEntity != null && myEntity.Parent is MyCockpit)
			{
				myEntity = myEntity.Parent;
			}
			return myEntity;
		}

		private void ProcessSpringCalculation()
		{
			Vector3D value = m_position - m_desiredPosition;
			Vector3 value2 = (Vector3)((0f - m_currentSpring.Stiffness) * value - m_currentSpring.Dampening * m_velocity) / m_currentSpring.Mass;
			m_velocity += value2 * 0.0166666675f;
			m_position += m_velocity * 0.0166666675f;
		}

		public void Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(Vector3.Zero, rotationIndicator, rollIndicator);
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			UpdateZoom();
		}

		private void SetPositionAndLookAt(Vector3D lookAt)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = MySession.Static.ControlledEntity;
			if (myControllableEntity == null)
			{
				myControllableEntity = (MySession.Static.CameraController as Sandbox.Game.Entities.IMyControllableEntity);
				if (myControllableEntity == null)
				{
					return;
				}
			}
			MyEntity controlledEntity = GetControlledEntity(myControllableEntity);
			MyPositionComponentBase positionComp = controlledEntity.PositionComp;
			_ = positionComp.LocalAABB;
			_ = positionComp.LocalAABB;
			m_target = myControllableEntity.GetHeadMatrix(includeY: true).Translation;
			double scaleFactor = lookAt.Length();
			Vector3D value = (MySession.Static == null || (MySession.Static.ControlledEntity != null && !(MySession.Static.CameraController is MyCharacter))) ? m_initialLookAtDirection : m_initialLookAtDirectionCharacter;
			m_lookAt = value * scaleFactor;
			m_lastTarget = m_target;
			if (controlledEntity.Physics != null)
			{
				m_target += controlledEntity.Physics.LinearVelocity * 60f;
			}
			m_transformedLookAt = Vector3D.Transform(lookAt, m_targetOrientation);
			m_positionSafe = m_target + m_transformedLookAt;
			if (controlledEntity.Physics != null)
			{
				m_positionSafe -= controlledEntity.Physics.LinearVelocity * 60f;
			}
			m_desiredPosition = m_positionSafe;
			m_position = m_positionSafe;
			m_velocity = Vector3.Zero;
			m_disableSpringThisFrame = true;
			m_safeStartSmoothing.Clear();
			m_positionSafeZoomingOutParam = 0f;
			m_positionSafeZoomingOutTimeout = 0;
		}

		public MatrixD GetViewMatrix()
		{
			if (MySession.Static.CameraController == null)
			{
				return MatrixD.Identity;
			}
			return MatrixD.CreateLookAt(m_positionSafe, m_target, m_targetOrientation.Up);
		}

		public bool IsCameraForced()
		{
			return m_positionCurrentIsSafeSinceTime == TimeSpan.MaxValue;
		}

		public bool IsCameraForcedWithDelay()
		{
			if (!IsCameraForced())
			{
				return (MySession.Static.ElapsedGameTime - m_positionCurrentIsSafeSinceTime).TotalMilliseconds < 1000.0;
			}
			return true;
		}

		private MyCameraRaycastResult RaycastOccludingObjects(MyEntity controlledEntity, ref Vector3D raycastOrigin, ref Vector3D raycastEnd, ref Vector3D raycastSafeCameraStart, ref Vector3D outSafePosition)
		{
			Vector3D vector3D = raycastEnd - raycastOrigin;
			vector3D.Normalize();
			double num = double.PositiveInfinity;
			bool flag = false;
			Vector3D? vector3D2 = null;
			if (controlledEntity is MyCharacter)
			{
				MatrixD worldMatrix = controlledEntity.PositionComp.WorldMatrix;
				Vector3D translation = worldMatrix.Translation;
				Vector3D vector3D3 = translation + worldMatrix.Up * controlledEntity.PositionComp.LocalAABB.Max.Y * 1.1499999761581421;
				translation += worldMatrix.Up * controlledEntity.PositionComp.LocalAABB.Max.Y * 0.85000002384185791;
				m_raycastList.Clear();
				MyPhysics.CastRay(translation, vector3D3, m_raycastList);
				if (m_debugDraw && MySession.Static.CameraController != controlledEntity)
				{
					MyRenderProxy.DebugDrawLine3D(translation, vector3D3, Color.Red, Color.Red, depthRead: false);
				}
				foreach (MyPhysics.HitInfo raycast in m_raycastList)
				{
					MyEntity hitEntity = (MyEntity)raycast.HkHitInfo.GetHitEntity();
					HkRigidBody body = raycast.HkHitInfo.Body;
					HkHitInfo hkHitInfo = raycast.HkHitInfo;
					if (!IsEntityFiltered(hitEntity, controlledEntity, body, hkHitInfo.GetShapeKey(0)))
					{
						vector3D2 = raycast.Position;
						num = 0.0;
						double scaleFactor = 0f - m_currentCameraRadius;
						outSafePosition = raycastOrigin + vector3D * scaleFactor;
						flag = true;
					}
				}
			}
			if (m_debugDraw && vector3D2.HasValue)
			{
				MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
				MyDebugDrawHelper.DrawNamedPoint(vector3D2.Value, "OCCLUDER", Color.Red, viewMatrix);
				vector3D2 = null;
			}
			bool flag2 = false;
			float num2 = 1f;
			m_collisionList.Clear();
			Vector3 halfExtents = new Vector3(m_currentCameraRadius, m_currentCameraRadius, m_currentCameraRadius) * 0.02f;
			Vector3D translation2 = raycastOrigin + m_currentCameraRadius * vector3D;
			MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation2, ref Quaternion.Identity, m_collisionList, 15);
			if (EnableDebugDraw)
			{
				MyRenderProxy.DebugDrawAABB(new BoundingBoxD(translation2 - halfExtents, translation2 + halfExtents), Color.Red);
			}
			foreach (HkBodyCollision collision in m_collisionList)
			{
				MyEntity hitEntity2 = (MyEntity)collision.GetCollisionEntity();
				if (!IsEntityFiltered(hitEntity2, controlledEntity, collision.Body, collision.ShapeKey))
				{
					flag2 = true;
				}
			}
			BoundingBoxD boundingBox = new BoundingBoxD(translation2 - 9.9999997473787516E-06, translation2 + 1E-05f);
			m_entityList.Clear();
			MyEntities.GetElementsInBox(ref boundingBox, m_entityList);
			foreach (MyEntity entity in m_entityList)
			{
				MyVoxelBase myVoxelBase = entity as MyVoxelBase;
				if (myVoxelBase != null && myVoxelBase.IsAnyAabbCornerInside(ref MatrixD.Identity, boundingBox))
				{
					flag2 = true;
					break;
				}
			}
			if ((raycastEnd - raycastOrigin).LengthSquared() > (double)(m_currentCameraRadius * m_currentCameraRadius))
			{
				HkShape shape = new HkSphereShape(m_currentCameraRadius);
				MatrixD transform = MatrixD.Identity;
				transform.Translation = ((controlledEntity is MyCharacter) ? raycastOrigin : (raycastOrigin + m_currentCameraRadius * vector3D));
				double num3 = (transform.Translation - m_target).LengthSquared();
				m_raycastList.Clear();
				MyPhysics.CastShapeReturnContactBodyDatas(raycastEnd, shape, ref transform, 0u, 0f, m_raycastList);
				if (EnableDebugDraw)
				{
					MyRenderProxy.DebugDrawLine3D(transform.Translation, raycastEnd, Color.Red, Color.Red, depthRead: true);
					MyDebugDrawHelper.DrawNamedPoint(transform.Translation, "RAY_START");
					MyDebugDrawHelper.DrawNamedPoint(raycastEnd, "RAY_END");
				}
				foreach (MyPhysics.HitInfo raycast2 in m_raycastList)
				{
					if (!(Vector3.Dot(raycast2.HkHitInfo.Normal, vector3D) > 0f))
					{
						MyEntity hitEntity3 = (MyEntity)raycast2.HkHitInfo.GetHitEntity();
						HkRigidBody body2 = raycast2.HkHitInfo.Body;
						HkHitInfo hkHitInfo = raycast2.HkHitInfo;
						if (!IsEntityFiltered(hitEntity3, controlledEntity, body2, hkHitInfo.GetShapeKey(0)))
						{
							double num4 = (raycast2.Position - m_target).LengthSquared();
							if (!(num3 > num4))
							{
								float hitFraction = raycast2.HkHitInfo.HitFraction;
								Vector3D vector3D4 = Vector3D.Lerp(transform.Translation, raycastEnd, Math.Max(hitFraction, 0.0001));
								double num5 = Vector3D.DistanceSquared(transform.Translation, vector3D4);
								if (hitFraction < num2 && num5 < num)
								{
									vector3D2 = raycast2.Position;
									outSafePosition = vector3D4;
									num = num5;
									flag = true;
									num2 = hitFraction;
								}
							}
						}
					}
				}
				shape.RemoveReference();
			}
			if (flag2 && !flag)
			{
				vector3D2 = raycastOrigin;
				outSafePosition = raycastOrigin;
				num = 0.0;
				flag = true;
				num2 = 0f;
			}
			if (m_debugDraw && vector3D2.HasValue)
			{
				MatrixD viewMatrix2 = MySector.MainCamera.ViewMatrix;
				MyDebugDrawHelper.DrawNamedPoint(vector3D2.Value, "OCCLUDER", Color.Red, viewMatrix2);
				vector3D2 = null;
			}
			if (controlledEntity is MyCharacter)
			{
				float currentCameraRadius = m_currentCameraRadius;
				if (num < (double)(currentCameraRadius * currentCameraRadius))
				{
					m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
					return MyCameraRaycastResult.FoundOccluderNoSpace;
				}
			}
			else
			{
				float num6 = m_currentCameraRadius + (IsCameraForced() ? 2.5f : 0.0249999985f);
				if (num < (double)(num6 * num6))
				{
					m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
					return MyCameraRaycastResult.FoundOccluderNoSpace;
				}
			}
			if (!flag)
			{
				MyPhysics.CastRay(raycastOrigin, raycastSafeCameraStart, m_raycastList);
				foreach (MyPhysics.HitInfo raycast3 in m_raycastList)
				{
					if (raycast3.HkHitInfo.GetHitEntity() is MyVoxelBase)
					{
						m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
						return MyCameraRaycastResult.FoundOccluderNoSpace;
					}
				}
			}
			if (!flag)
			{
				return MyCameraRaycastResult.Ok;
			}
			return MyCameraRaycastResult.FoundOccluder;
		}

		private bool IsEntityFiltered(MyEntity hitEntity, MyEntity controlledEntity, HkRigidBody hitRigidBody, uint shapeKey)
		{
			if (hitRigidBody == null || hitRigidBody.UserObject == null || hitEntity == controlledEntity || !(hitRigidBody.UserObject is MyPhysicsBody) || (hitRigidBody.UserObject as MyPhysicsBody).IsPhantom)
			{
				return true;
			}
			if (shapeKey != uint.MaxValue && hitEntity is MyCubeGrid)
			{
				MySlimBlock blockFromShapeKey = ((MyCubeGrid)hitEntity).Physics.Shape.GetBlockFromShapeKey(shapeKey);
				if (blockFromShapeKey != null)
				{
					MyLadder myLadder = blockFromShapeKey.FatBlock as MyLadder;
					if (myLadder != null)
					{
						hitEntity = myLadder;
					}
				}
			}
			if (hitEntity is IMyHandheldGunObject<MyDeviceBase>)
			{
				return true;
			}
			if (hitEntity is MyFloatingObject || hitEntity is MyDebrisBase)
			{
				return true;
			}
			if (hitEntity is MyCharacter)
			{
				return true;
			}
			MyEntity myEntity = hitEntity.GetTopMostParent() ?? hitEntity;
			MyEntity myEntity2 = controlledEntity.GetTopMostParent() ?? controlledEntity;
			if (myEntity == myEntity2)
			{
				return true;
			}
			MyCubeGrid myCubeGrid = myEntity2 as MyCubeGrid;
			MyCubeGrid myCubeGrid2 = myEntity as MyCubeGrid;
			if (myCubeGrid != null && myCubeGrid2 != null && MyGridPhysicalHierarchy.Static.InSameHierarchy(myCubeGrid, myCubeGrid2))
			{
				return true;
			}
			MyCharacter myCharacter = controlledEntity as MyCharacter;
			if (myCharacter != null && myCharacter.Ladder != null && hitEntity is MyLadder && myCharacter.Ladder.GetTopMostParent() == hitEntity.GetTopMostParent())
			{
				return true;
			}
			return false;
		}

		public void ResetInternalTimers()
		{
			m_positionCurrentIsSafeSinceTime = TimeSpan.Zero;
		}

		private void HandleIntersection(MyEntity controlledEntity, ref Vector3D lastTargetPos)
		{
			MyEntity myEntity = controlledEntity.GetTopMostParent() ?? controlledEntity;
			MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
			if (myCubeGrid != null && myCubeGrid.IsStatic)
			{
				myEntity = controlledEntity;
			}
			Vector3D target = m_target;
			Vector3D raycastEnd = m_position;
			double num = (raycastEnd - m_target).LengthSquared();
			if (num > 0.0)
			{
				double scaleFactor = m_lookAt.Length() / Math.Sqrt(num);
				raycastEnd = m_target + (raycastEnd - m_target) * scaleFactor;
			}
			LineD line = new LineD(target, raycastEnd);
			if (line.Length > 500.0)
			{
				return;
			}
			MyOrientedBoundingBoxD safeObb = ComputeCompleteSafeOBB(myEntity);
			MyOrientedBoundingBoxD safeObbWithCollisionExtents = new MyOrientedBoundingBoxD(safeObb.Center, safeObb.HalfExtent + 2f * m_currentCameraRadius, safeObb.Orientation);
			Vector3D castStartSafe;
			LineD safeOBBLine;
			MyCameraRaycastResult myCameraRaycastResult = FindSafeStart(controlledEntity, line, ref safeObb, ref safeObbWithCollisionExtents, out castStartSafe, out safeOBBLine);
			if (controlledEntity is MyCharacter)
			{
				m_safeMinimumDistance = m_currentCameraRadius;
			}
			else
			{
				m_safeMinimumDistance = (castStartSafe - target).Length();
				m_safeMinimumDistance = Math.Max(m_safeMinimumDistance, 2.5);
			}
			Vector3D raycastOrigin = (controlledEntity is MyCharacter) ? target : castStartSafe;
			Vector3D outSafePosition = raycastEnd;
			if (myCameraRaycastResult == MyCameraRaycastResult.Ok)
			{
				if ((raycastEnd - raycastOrigin).LengthSquared() < m_safeMinimumDistance * m_safeMinimumDistance)
				{
					raycastEnd = raycastOrigin + Vector3D.Normalize(raycastEnd - raycastOrigin) * m_safeMinimumDistance;
				}
				myCameraRaycastResult = RaycastOccludingObjects(controlledEntity, ref raycastOrigin, ref raycastEnd, ref castStartSafe, ref outSafePosition);
				if (m_safeMaximumDistanceTimeout >= 0f)
				{
					m_safeMaximumDistanceTimeout -= 16.6666679f;
				}
				switch (myCameraRaycastResult)
				{
				case MyCameraRaycastResult.Ok:
					if (m_safeMaximumDistanceTimeout <= 0f)
					{
						m_safeMaximumDistance = 200.0;
					}
					break;
				case MyCameraRaycastResult.FoundOccluder:
				{
					double num2 = (outSafePosition - target).Length();
					double num3 = num2 + (double)m_currentCameraRadius;
					if (m_safeMaximumDistanceTimeout <= 0f || num3 < m_safeMaximumDistance)
					{
						m_safeMaximumDistance = num3;
					}
					if (num2 < m_safeMaximumDistance + (double)m_currentCameraRadius && !IsCameraForcedWithDelay())
					{
						m_safeMaximumDistanceTimeout = m_positionSafeZoomingOutDefaultTimeoutMs;
					}
					m_safeMinimumDistance = Math.Min(m_safeMinimumDistance, num2);
					if (controlledEntity is MyCharacter && safeObbWithCollisionExtents.Contains(ref outSafePosition))
					{
						myCameraRaycastResult = MyCameraRaycastResult.FoundOccluderNoSpace;
					}
					break;
				}
				}
			}
			if (IsCameraForced())
			{
				m_positionSafe = m_target;
			}
			if ((uint)myCameraRaycastResult <= 1u)
			{
				bool flag = false;
				if (m_positionCurrentIsSafeSinceTime == TimeSpan.MaxValue)
				{
					ResetInternalTimers();
					ResetSpring();
					flag = true;
					m_safeMaximumDistanceTimeout = 0f;
				}
				PerformZoomInOut(castStartSafe, outSafePosition);
				if (flag)
				{
					m_positionCurrentIsSafeSinceTime = MySession.Static.ElapsedGameTime;
				}
				PerformZoomInOut(castStartSafe, outSafePosition);
				if (m_positionCurrentIsSafeSinceTime == TimeSpan.MaxValue)
				{
					m_positionCurrentIsSafeSinceTime = MySession.Static.ElapsedGameTime;
				}
			}
			else
			{
				m_positionSafeZoomingOutParam = 1f;
				m_positionCurrentIsSafeSinceTime = TimeSpan.MaxValue;
				m_lastRaycastDist = 0f;
				m_zoomingOutSmoothly = true;
				m_positionSafeZoomingOutTimeout = 0;
			}
			if (IsCameraForced())
			{
				m_positionSafe = m_target;
			}
			if (m_debugDraw)
			{
				MyRenderProxy.DebugDrawArrow3D(safeOBBLine.From, safeOBBLine.To, Color.White, Color.Purple, depthRead: false, 0.02);
				MyRenderProxy.DebugDrawArrow3D(safeOBBLine.From, castStartSafe, Color.White, Color.Red, depthRead: false, 0.02);
				MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
				MyDebugDrawHelper.DrawNamedPoint(m_position, "mpos", Color.Gray, viewMatrix);
				MyDebugDrawHelper.DrawNamedPoint(m_target, "target", Color.Purple, viewMatrix);
				MyDebugDrawHelper.DrawNamedPoint(castStartSafe, "safeStart", Color.Lime, viewMatrix);
				MyDebugDrawHelper.DrawNamedPoint(outSafePosition, "safePosCand", Color.Pink, viewMatrix);
				MyDebugDrawHelper.DrawNamedPoint(m_positionSafe, "posSafe", Color.White, viewMatrix);
				MyRenderProxy.DebugDrawOBB(safeObbWithCollisionExtents, Color.Olive, 0f, depthRead: false, smooth: true);
				MyRenderProxy.DebugDrawOBB(safeObb, Color.OliveDrab, 0f, depthRead: false, smooth: true);
				MyRenderProxy.DebugDrawText3D(safeObbWithCollisionExtents.Center - Vector3D.Transform(safeObbWithCollisionExtents.HalfExtent, safeObbWithCollisionExtents.Orientation), "safeObb", Color.Olive, 0.5f, depthRead: false);
				MyRenderProxy.DebugDrawText2D(new Vector2(30f, 30f), myCameraRaycastResult.ToString(), Color.Azure, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(30f, 50f), (!IsCameraForcedWithDelay()) ? "Unforced" : (IsCameraForced() ? "Forced" : "ForcedDelay"), Color.Azure, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(30f, 70f), m_zoomingOutSmoothly ? "zooming out" : "ready", Color.Azure, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(30f, 90f), "v=" + m_velocity.Length().ToString("0.00"), Color.Azure, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(30f, 110f), "maxsafedist=" + m_safeMaximumDistance, Color.Azure, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(30f, 130f), "maxsafedisttimeout=" + (0.001f * m_safeMaximumDistanceTimeout).ToString("0.0"), (m_safeMaximumDistanceTimeout <= 0f) ? Color.Red : Color.Azure, 1f);
			}
		}

		private MyCameraRaycastResult FindSafeStart(MyEntity controlledEntity, LineD line, ref MyOrientedBoundingBoxD safeObb, ref MyOrientedBoundingBoxD safeObbWithCollisionExtents, out Vector3D castStartSafe, out LineD safeOBBLine)
		{
			safeOBBLine = new LineD(safeObbWithCollisionExtents.Center, line.From + line.Direction * 2.0 * safeObbWithCollisionExtents.HalfExtent.Length());
			double? num = safeObbWithCollisionExtents.Intersects(ref safeOBBLine);
			castStartSafe = (num.HasValue ? (safeOBBLine.From + safeOBBLine.Direction * num.Value) : line.From);
			MyCameraRaycastResult result = MyCameraRaycastResult.Ok;
			if (num.HasValue)
			{
				m_raycastList.Clear();
				MatrixD transform = MatrixD.CreateTranslation(castStartSafe);
				HkShape shape = new HkSphereShape(m_currentCameraRadius);
				MyPhysics.CastShapeReturnContactBodyDatas(line.From, shape, ref transform, 0u, 0f, m_raycastList);
				if (EnableDebugDraw)
				{
					MyDebugDrawHelper.DrawDashedLine(castStartSafe + 0.1f * Vector3.Up, line.From + 0.1f * Vector3.Up, Color.Red);
					MyRenderProxy.DebugDrawSphere(castStartSafe, m_currentCameraRadius, Color.Red);
				}
				MyPhysics.HitInfo? hitInfo = null;
				foreach (MyPhysics.HitInfo raycast in m_raycastList)
				{
					MyEntity hitEntity = raycast.HkHitInfo.GetHitEntity() as MyEntity;
					HkRigidBody body = raycast.HkHitInfo.Body;
					HkHitInfo hkHitInfo = raycast.HkHitInfo;
					if (!IsEntityFiltered(hitEntity, controlledEntity, body, hkHitInfo.GetShapeKey(0)))
					{
						hitInfo = raycast;
						break;
					}
				}
				if (!hitInfo.HasValue)
				{
					m_collisionList.Clear();
					MyPhysics.GetPenetrationsShape(shape, ref castStartSafe, ref Quaternion.Identity, m_collisionList, 15);
					foreach (HkBodyCollision collision in m_collisionList)
					{
						MyEntity hitEntity2 = collision.GetCollisionEntity() as MyEntity;
						if (!IsEntityFiltered(hitEntity2, controlledEntity, collision.Body, collision.ShapeKey))
						{
							result = MyCameraRaycastResult.FoundOccluderNoSpace;
							break;
						}
					}
				}
				shape.RemoveReference();
				if (hitInfo.HasValue)
				{
					castStartSafe += ((double)hitInfo.Value.HkHitInfo.HitFraction - (double)m_currentCameraRadius / safeOBBLine.Length) * (line.From - castStartSafe);
					result = MyCameraRaycastResult.FoundOccluderNoSpace;
				}
				double num2 = (line.To - line.From).LengthSquared();
				double num3 = (castStartSafe - line.From).LengthSquared();
				double num4 = Math.Sqrt(num3) - (double)(m_currentCameraRadius * 0.01f);
				double num5 = m_safeStartSmoothing.Get();
				if (num5 > num3)
				{
					m_safeStartSmoothingFiltering = Math.Max(m_safeStartSmoothingFiltering - 0.025f, 0f);
					num5 = MathHelper.Lerp(num3, num5, m_safeStartSmoothingFiltering);
					double scaleFactor = Math.Sqrt(num5 / num3);
					castStartSafe = line.From + (castStartSafe - line.From) * scaleFactor;
					num3 = num5;
					num4 = Math.Sqrt(num3);
				}
				else
				{
					m_safeStartSmoothingFiltering = Math.Min(m_safeStartSmoothingFiltering + 0.05f, 1f);
				}
				m_safeStartSmoothing.Add(num3);
				if (num2 < num4 * num4)
				{
					m_position = castStartSafe;
					m_positionSafe = castStartSafe;
					m_positionSafeZoomingOutTimeout = 0;
				}
				if (num2 * 2.0 < num3)
				{
					m_disableSpringThisFrame = true;
				}
			}
			return result;
		}

		private void PerformZoomInOut(Vector3D safeCastStart, Vector3D safePositionCandidate)
		{
			double val = (safePositionCandidate - m_target).Length();
			val = Math.Min(val, m_safeMaximumDistance);
			double num = (m_positionSafe - m_target).Length();
			if (m_disableSpringThisFrame)
			{
				m_lastRaycastDist = (float)val;
				m_zoomingOutSmoothly = false;
			}
			if (IsCameraForced())
			{
				m_positionSafeZoomingOutTimeout = 0;
				m_positionSafeZoomingOutParam = 0f;
				num = 0.0;
				m_zoomingOutSmoothly = true;
				m_desiredPosition = safeCastStart;
				m_position = safeCastStart;
			}
			if (!m_disableSpringThisFrame && val > num + (double)m_currentCameraRadius)
			{
				m_zoomingOutSmoothly = true;
				if (m_lastZoomingOutSpeed <= 1E-05f && m_positionSafeZoomingOutTimeout <= -m_positionSafeZoomingOutDefaultTimeoutMs)
				{
					m_positionSafeZoomingOutTimeout = m_positionSafeZoomingOutDefaultTimeoutMs;
				}
			}
			else if (val < num + (double)m_currentCameraRadius && Math.Abs(m_lookAt.LengthSquared() - val * val) <= 9.9999997473787516E-06)
			{
				m_zoomingOutSmoothly = false;
			}
			if (m_zoomingOutSmoothly)
			{
				m_positionSafeZoomingOutTimeout -= 16;
				if (m_positionSafeZoomingOutTimeout <= 0 && !IsCameraForced())
				{
					double val2 = (safePositionCandidate - m_target).Length();
					val2 = Math.Min(val2, m_safeMaximumDistance);
					float num2 = 1f - MathHelper.Clamp((float)((double)m_lastRaycastDist - val2), 0.95f, 1f);
					m_lastZoomingOutSpeed = Math.Abs(num2);
					m_positionSafeZoomingOutParam += num2;
					m_positionSafeZoomingOutParam = MathHelper.Clamp(m_positionSafeZoomingOutParam, 0f, 1f);
					double scaleFactor = Math.Min((m_positionSafe - m_target).Length(), m_safeMaximumDistance);
					Vector3D value = m_target + Vector3D.Normalize(safePositionCandidate - m_target) * scaleFactor;
					m_positionSafe = Vector3D.Lerp(value, safePositionCandidate, m_positionSafeZoomingOutParam);
					m_lastRaycastDist = (float)val;
				}
				else
				{
					m_lastZoomingOutSpeed = 0f;
					double scaleFactor2 = (m_positionSafe - m_target).Length();
					if (!IsCameraForced())
					{
						m_positionSafe = m_target + Vector3D.Normalize(safePositionCandidate - m_target) * scaleFactor2;
					}
				}
			}
			else
			{
				if (!m_disableSpringThisFrame)
				{
					m_positionSafeZoomingOutParam = 0f;
					m_lastZoomingOutSpeed = 0f;
					m_positionSafeZoomingOutTimeout = m_positionSafeZoomingOutDefaultTimeoutMs;
				}
				if (!IsCameraForced())
				{
					double val3 = Math.Min((safePositionCandidate - m_target).Length(), m_safeMaximumDistance);
					val3 = Math.Max(val3, m_safeMinimumDistance);
					m_positionSafe = m_target + Vector3D.Normalize(safePositionCandidate - m_target) * val3;
				}
				m_lastRaycastDist = (float)val;
			}
		}

		private void MergeAABB(MyCubeGrid grid)
		{
			m_safeAABB.Include(grid.PositionComp.WorldAABB);
		}

		private MyOrientedBoundingBoxD ComputeEntitySafeOBB(MyEntity controlledEntity)
		{
			MatrixD worldMatrix = controlledEntity.WorldMatrix;
			BoundingBox localAABB = controlledEntity.PositionComp.LocalAABB;
			Vector3D center = Vector3D.Transform((Vector3D)localAABB.Center, worldMatrix);
			MyOrientedBoundingBoxD result = new MyOrientedBoundingBoxD(center, localAABB.HalfExtents, Quaternion.CreateFromRotationMatrix(worldMatrix));
			MyCubeGrid myCubeGrid = controlledEntity.GetTopMostParent() as MyCubeGrid;
			if (myCubeGrid != null)
			{
				m_safeAABB = myCubeGrid.PositionComp.WorldAABB;
				MyGridPhysicalHierarchy.Static.ApplyOnChildren(myCubeGrid, MergeAABB);
				return MyOrientedBoundingBoxD.CreateFromBoundingBox(m_safeAABB);
			}
			return result;
		}

		private MyOrientedBoundingBoxD ComputeCompleteSafeOBB(MyEntity controlledEntity)
		{
			MyCubeGrid myCubeGrid = controlledEntity.GetTopMostParent() as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return ComputeEntitySafeOBB(controlledEntity);
			}
			MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(myCubeGrid);
			return ComputeEntitySafeOBB(root);
		}

		public void RecalibrateCameraPosition(bool isCharacter = false)
		{
			IMyCameraController cameraController = MySession.Static.CameraController;
			if (!(cameraController is MyEntity))
			{
				return;
			}
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = GetControlledEntity(MySession.Static.ControlledEntity) as Sandbox.Game.Entities.IMyControllableEntity;
			if (myControllableEntity == null)
			{
				return;
			}
			if (!isCharacter)
			{
				MatrixD headMatrix = myControllableEntity.GetHeadMatrix(includeY: true);
				m_targetOrientation = headMatrix.GetOrientation();
				m_target = headMatrix.Translation;
			}
			MyEntity topMostParent = ((MyEntity)cameraController).GetTopMostParent();
			if (!topMostParent.Closed)
			{
				MatrixD worldMatrixNormalizedInv = topMostParent.PositionComp.WorldMatrixNormalizedInv;
				Vector3D value = Vector3D.Transform(m_target, worldMatrixNormalizedInv);
				MatrixD matrixD = m_targetOrientation * worldMatrixNormalizedInv;
				BoundingBox localAABB = topMostParent.PositionComp.LocalAABB;
				localAABB.Inflate(1.2f);
				Vector3D vector = value - localAABB.Center;
				Vector3D vector2 = Vector3D.Normalize(matrixD.Backward);
				double num = Vector3D.Dot(vector, vector2);
				double num2 = Math.Max(Math.Abs(Vector3D.Dot(localAABB.HalfExtents, vector2)) - num, 2.5999999046325684);
				double num3 = 2.5999999046325684;
				if (Math.Abs(vector2.Z) > 0.0001)
				{
					num3 = localAABB.HalfExtents.X * 1.5f;
				}
				else if (Math.Abs(vector2.X) > 0.0001)
				{
					num3 = localAABB.HalfExtents.Z * 1.5f;
				}
				double num4 = Math.Tan((double)MySector.MainCamera.FieldOfView * 0.5);
				double value2 = num3 / (2.0 * num4) + num2;
				m_safeMaximumDistance = 200.0;
				m_safeMinimumDistance = 2.5;
				double scaleFactor = MathHelper.Clamp(value2, 2.5, 200.0);
				Vector3D positionAndLookAt = m_initialLookAtDirectionCharacter * scaleFactor;
				SetPositionAndLookAt(positionAndLookAt);
			}
		}

		private static bool HasSamePhysicalGroup(IMyEntity entityA, IMyEntity entityB)
		{
			if (entityA == entityB)
			{
				return true;
			}
			MyCubeGrid myCubeGrid = entityA as MyCubeGrid;
			MyCubeGrid myCubeGrid2 = entityB as MyCubeGrid;
			if (myCubeGrid == null || myCubeGrid2 == null)
			{
				return false;
			}
			return MyCubeGridGroups.Static.Physical.HasSameGroup(myCubeGrid, myCubeGrid2);
		}

		public void UpdateZoom()
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			bool num = !MyPerGameSettings.ZoomRequiresLookAroundPressed || MyControllerHelper.IsControl(context, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED);
			double num2 = 0.0;
			if (num)
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					num2 = m_lookAt.Length() / 1.2000000476837158;
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					num2 = m_lookAt.Length() * 1.2000000476837158;
					m_positionSafeZoomingOutTimeout = -m_positionSafeZoomingOutDefaultTimeoutMs;
				}
				float num3 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.CAMERA_ZOOM_IN);
				float num4 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.CAMERA_ZOOM_OUT);
				if (num3 > 0f)
				{
					num2 = m_lookAt.Length() / (double)(1f + 0.06f * num3);
				}
				else if (num4 > 0f)
				{
					num2 = m_lookAt.Length() * (double)(1f + 0.06f * num4);
					m_positionSafeZoomingOutTimeout = -m_positionSafeZoomingOutDefaultTimeoutMs;
				}
			}
			bool flag = false;
			if (num2 > 0.0)
			{
				double num5 = m_lookAt.Length();
				num2 = MathHelper.Clamp(num2, Math.Max(m_safeMinimumDistance, 2.5), m_safeMaximumDistance);
				m_lookAt *= num2 / num5;
				flag = (num2 > num5);
				if (flag)
				{
					m_positionSafeZoomingOutTimeout = 0;
					m_safeMaximumDistanceTimeout = 0f;
				}
				SaveSettings();
			}
			else
			{
				double num6 = m_lookAt.Length();
				double num7 = MathHelper.Clamp(num6, 2.5, 200.0);
				m_lookAt *= num7 / num6;
				SaveSettings();
			}
			m_clampedlookAt = m_lookAt;
			double num8 = m_clampedlookAt.Length();
			m_clampedlookAt = m_clampedlookAt * MathHelper.Clamp(num8, m_safeMinimumDistance, m_safeMaximumDistance) / num8;
			if (flag && m_lookAt.LengthSquared() < m_safeMinimumDistance * m_safeMinimumDistance)
			{
				m_lookAt = m_clampedlookAt;
			}
		}

		public bool ResetViewerDistance(double? newDistance = null)
		{
			if (!newDistance.HasValue)
			{
				return false;
			}
			newDistance = MathHelper.Clamp(newDistance.Value, 2.5, 200.0);
			Vector3D positionAndLookAt = ((MySession.Static != null && MySession.Static.ControlledEntity is MyCharacter) ? m_initialLookAtDirectionCharacter : m_initialLookAtDirection) * newDistance.Value;
			SetPositionAndLookAt(positionAndLookAt);
			m_disableSpringThisFrame = true;
			m_lastRaycastDist = (float)newDistance.Value;
			m_safeMaximumDistanceTimeout = 0f;
			m_zoomingOutSmoothly = false;
			m_positionSafeZoomingOutTimeout = -m_positionSafeZoomingOutDefaultTimeoutMs;
			m_saveSettings = false;
			Update();
			UpdateZoom();
			SaveSettings();
			return true;
		}

		public bool ResetViewerAngle(Vector2? headAngle)
		{
			if (!headAngle.HasValue)
			{
				return false;
			}
			Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity == null)
			{
				return false;
			}
			controlledEntity.HeadLocalXAngle = headAngle.Value.X;
			controlledEntity.HeadLocalYAngle = headAngle.Value.Y;
			return true;
		}

		public double GetViewerDistance()
		{
			return m_clampedlookAt.Length();
		}

		public void SaveSettings()
		{
			m_saveSettings = true;
		}

		public Vector3D GetCrosshair()
		{
			return m_target + m_targetOrientation.Forward * 25000.0;
		}

		public void CompensateQuickTransformChange(ref MatrixD transformDelta)
		{
			m_position = Vector3D.Transform(m_position, ref transformDelta);
			m_positionSafe = Vector3D.Transform(m_positionSafe, ref transformDelta);
			m_lastTarget = Vector3D.Transform(m_lastTarget, ref transformDelta);
			m_target = Vector3D.Transform(m_target, ref transformDelta);
			m_desiredPosition = Vector3D.Transform(m_desiredPosition, ref transformDelta);
		}

		private void DebugDrawTrail()
		{
			if (m_debugDraw && m_enableDebugDrawTrail)
			{
				if (m_debugLastSpectatorPositions == null)
				{
					m_debugLastSpectatorPositions = new List<Vector3D>(1024);
					m_debugLastSpectatorDesiredPositions = new List<Vector3D>(1024);
				}
				m_debugLastSpectatorPositions.Add(m_position);
				m_debugLastSpectatorDesiredPositions.Add(m_desiredPosition);
				if (m_debugLastSpectatorDesiredPositions.Count > 60)
				{
					m_debugLastSpectatorPositions.RemoveRange(0, 1);
					m_debugLastSpectatorDesiredPositions.RemoveRange(0, 1);
				}
				for (int i = 1; i < m_debugLastSpectatorPositions.Count; i++)
				{
					float num = (float)i / (float)m_debugLastSpectatorPositions.Count;
					Color color = new Color(num * num, 0f, 0f);
					MyRenderProxy.DebugDrawLine3D(m_debugLastSpectatorPositions[i - 1], m_debugLastSpectatorPositions[i], color, color, depthRead: true);
					color = new Color(num * num, num * num, num * num);
					MyRenderProxy.DebugDrawLine3D(m_debugLastSpectatorDesiredPositions[i - 1], m_debugLastSpectatorDesiredPositions[i], color, color, depthRead: true);
				}
			}
			else
			{
				m_debugLastSpectatorPositions = null;
				m_debugLastSpectatorDesiredPositions = null;
			}
		}
	}
}
