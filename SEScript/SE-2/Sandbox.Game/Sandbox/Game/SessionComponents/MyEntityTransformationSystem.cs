using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 900)]
	public class MyEntityTransformationSystem : MySessionComponentBase
	{
		public enum CoordinateMode
		{
			LocalCoords,
			WorldCoords
		}

		public enum OperationMode
		{
			Translation,
			Rotation,
			HierarchyAssignment
		}

		private static float PICKING_RAY_LENGTH = 1000f;

		private static float PLANE_THICKNESS = 0.005f;

		private static bool DEBUG = false;

		private MyEntity m_controlledEntity;

		private readonly Dictionary<MyEntity, int> m_cachedBodyCollisionLayers = new Dictionary<MyEntity, int>();

		private MyOrientedBoundingBoxD m_xBB;

		private MyOrientedBoundingBoxD m_yBB;

		private MyOrientedBoundingBoxD m_zBB;

		private MyOrientedBoundingBoxD m_xPlane;

		private MyOrientedBoundingBoxD m_yPlane;

		private MyOrientedBoundingBoxD m_zPlane;

		private int m_selected;

		private MatrixD m_gizmoMatrix;

		private PlaneD m_dragPlane;

		private bool m_dragActive;

		private bool m_dragOverAxis;

		private Vector3D m_dragStartingPoint;

		private Vector3D m_dragAxis;

		private Vector3D m_dragStartingPosition;

		private bool m_rotationActive;

		private PlaneD m_rotationPlane;

		private Vector3D m_rotationAxis;

		private Vector3D m_rotationStartingPoint;

		private MatrixD m_storedOrientation;

		private MatrixD m_storedScale;

		private Vector3D m_storedTranslation;

		private MatrixD m_storedWorldMatrix;

		private LineD m_lastRay;

		private OperationMode m_previousOperation;

		private readonly List<MyLineSegmentOverlapResult<MyEntity>> m_rayCastResultList = new List<MyLineSegmentOverlapResult<MyEntity>>();

		private bool m_active;

		public bool Active
		{
			get
			{
				return m_active;
			}
			set
			{
				if (Session != null && (Session.CreativeMode || MySession.Static.IsUserAdmin(Sync.MyId) || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
				{
					if (!value)
					{
						SetControlledEntity(null);
					}
					m_active = value;
				}
			}
		}

		public bool DisableTransformation
		{
			get;
			set;
		}

		public CoordinateMode Mode
		{
			get;
			set;
		}

		public OperationMode Operation
		{
			get;
			set;
		}

		public MyEntity ControlledEntity => m_controlledEntity;

		public bool DisablePicking
		{
			get;
			set;
		}

		public LineD LastRay => m_lastRay;

		public event Action<MyEntity, MyEntity> ControlledEntityChanged;

		public event Action<LineD> RayCasted;

		public MyEntityTransformationSystem()
		{
			Active = false;
			Mode = CoordinateMode.WorldCoords;
			m_selected = -1;
			MySession.Static.CameraAttachedToChanged += delegate
			{
				Active = false;
			};
		}

		public override void Draw()
		{
			if (!Active)
			{
				return;
			}
			if (DEBUG)
			{
				MyRenderProxy.DebugDrawLine3D(m_lastRay.From, m_lastRay.To, Color.Green, Color.Green, depthRead: true);
			}
			Vector2 vector = new Vector2(Session.Camera.ViewportSize.X * 0.01f, Session.Camera.ViewportSize.Y * 0.05f);
			Vector2 value = new Vector2(Session.Camera.ViewportSize.Y * 0.11f, 0f);
			Vector2 vector2 = new Vector2(0f, Session.Camera.ViewportSize.Y * 0.015f);
			float scale = 0.65f * Math.Min(Session.Camera.ViewportSize.X / 1920f, Session.Camera.ViewportSize.Y / 1200f);
			MyRenderProxy.DebugDrawText2D(vector, "Transform:", Color.Yellow, scale);
			switch (Operation)
			{
			case OperationMode.Translation:
				MyRenderProxy.DebugDrawText2D(vector + value, "Translation", Color.Orange, scale);
				break;
			case OperationMode.Rotation:
				MyRenderProxy.DebugDrawText2D(vector + value, "Rotation", Color.PaleGreen, scale);
				break;
			case OperationMode.HierarchyAssignment:
				MyRenderProxy.DebugDrawText2D(vector + value, "Hierarchy", Color.CornflowerBlue, scale);
				break;
			}
			vector += vector2;
			MyRenderProxy.DebugDrawText2D(vector, "     Coords:", Color.Yellow, scale);
			switch (Mode)
			{
			case CoordinateMode.WorldCoords:
				MyRenderProxy.DebugDrawText2D(vector + value, "World", Color.Orange, scale);
				break;
			case CoordinateMode.LocalCoords:
				MyRenderProxy.DebugDrawText2D(vector + value, "Local", Color.PaleGreen, scale);
				break;
			}
			vector += 1.5f * vector2;
			MyRenderProxy.DebugDrawText2D(vector, "Cam loc:", Color.Yellow, scale);
			new Vector2(Session.Camera.ViewportSize.Y * 0.08f, 0f);
			Vector3D position = MyAPIGateway.Session.Camera.Position;
			MyRenderProxy.DebugDrawText2D(vector + value, position.X.ToString("0.00"), Color.Crimson, scale);
			MyRenderProxy.DebugDrawText2D(vector + value + 1f * vector2, position.Y.ToString("0.00"), Color.PaleGreen, scale);
			MyRenderProxy.DebugDrawText2D(vector + value + 2f * vector2, position.Z.ToString("0.00"), Color.CornflowerBlue, scale);
			vector += 3.5f * vector2;
			bool flag = ControlledEntity != null;
			MyRenderProxy.DebugDrawText2D(vector, flag ? "Selected:" : "No entity selected", Color.Yellow, scale);
			if (flag)
			{
				Vector3D position2 = ControlledEntity.PositionComp.GetPosition();
				MyRenderProxy.DebugDrawText2D(vector + value, position2.X.ToString("0.00"), Color.Crimson, scale);
				MyRenderProxy.DebugDrawText2D(vector + value + 1f * vector2, position2.Y.ToString("0.00"), Color.PaleGreen, scale);
				MyRenderProxy.DebugDrawText2D(vector + value + 2f * vector2, position2.Z.ToString("0.00"), Color.CornflowerBlue, scale);
			}
			if (ControlledEntity == null)
			{
				return;
			}
			if (ControlledEntity.Parent != null)
			{
				for (MyEntity parent = ControlledEntity.Parent; parent != null; parent = parent.Parent)
				{
					MyRenderProxy.DebugDrawLine3D(ControlledEntity.Parent.PositionComp.GetPosition(), ControlledEntity.PositionComp.GetPosition(), Color.Orange, Color.Blue, depthRead: false);
				}
			}
			if (Operation == OperationMode.Translation && !DisableTransformation)
			{
				Vector3D position3 = Session.Camera.Position;
				double scaleFactor = Vector3D.Distance(m_xBB.Center, position3);
				double scaleFactor2 = Session.Camera.ProjectionMatrix.Up.LengthSquared();
				m_xBB.HalfExtent = Vector3D.One * 0.008 * scaleFactor * scaleFactor2;
				m_yBB.HalfExtent = Vector3D.One * 0.008 * scaleFactor * scaleFactor2;
				m_zBB.HalfExtent = Vector3D.One * 0.008 * scaleFactor * scaleFactor2;
				DrawOBB(m_xBB, Color.Red, 0.5f, 0);
				DrawOBB(m_yBB, Color.Green, 0.5f, 1);
				DrawOBB(m_zBB, Color.Blue, 0.5f, 2);
			}
			if (Operation != OperationMode.HierarchyAssignment && !DisableTransformation)
			{
				DrawOBB(m_xPlane, Color.Red, 0.2f, 3);
				DrawOBB(m_yPlane, Color.Green, 0.2f, 4);
				DrawOBB(m_zPlane, Color.Blue, 0.2f, 5);
			}
			else
			{
				Vector3D center = ControlledEntity.PositionComp.WorldVolume.Center;
				double radius = ControlledEntity.PositionComp.WorldVolume.Radius;
				MyRenderProxy.DebugDrawSphere(center, (float)radius, Color.Yellow, 0.2f);
			}
		}

		private void DrawOBB(MyOrientedBoundingBoxD obb, Color color, float alpha, int identificationIndex)
		{
			if (identificationIndex == m_selected)
			{
				MyRenderProxy.DebugDrawOBB(obb, Color.White, 0.2f, depthRead: false, smooth: false);
			}
			else
			{
				MyRenderProxy.DebugDrawOBB(obb, color, alpha, depthRead: false, smooth: false);
			}
		}

		public void ChangeOperationMode(OperationMode mode)
		{
			Operation = mode;
		}

		public void ChangeCoordSystem(bool world)
		{
			if (world)
			{
				Mode = CoordinateMode.WorldCoords;
			}
			else
			{
				Mode = CoordinateMode.LocalCoords;
			}
			if (ControlledEntity != null)
			{
				UpdateGizmoPosition();
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (!Active)
			{
				return;
			}
			if ((m_dragActive || m_rotationActive) && MyInput.Static.IsNewRightMousePressed())
			{
				SetWorldMatrix(ref m_storedWorldMatrix);
				m_dragActive = false;
				m_rotationActive = false;
				m_selected = -1;
			}
			if (!DisableTransformation)
			{
				if (m_dragActive)
				{
					PerformDragg(m_dragOverAxis);
				}
				if (m_rotationActive)
				{
					PerformRotation();
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.R))
			{
				switch (Operation)
				{
				case OperationMode.Translation:
					Operation = OperationMode.Rotation;
					break;
				case OperationMode.Rotation:
					Operation = OperationMode.Translation;
					break;
				case OperationMode.HierarchyAssignment:
					Operation = m_previousOperation;
					break;
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.T))
			{
				switch (Mode)
				{
				case CoordinateMode.LocalCoords:
					Mode = CoordinateMode.WorldCoords;
					break;
				case CoordinateMode.WorldCoords:
					Mode = CoordinateMode.LocalCoords;
					break;
				}
				if (ControlledEntity != null)
				{
					UpdateGizmoPosition();
				}
			}
			if (MyInput.Static.IsNewLeftMousePressed())
			{
				if (DisablePicking)
				{
					return;
				}
				m_lastRay = CreateRayFromCursorPosition();
				if (this.RayCasted != null)
				{
					this.RayCasted(m_lastRay);
				}
				if (ControlledEntity != null && PickControl())
				{
					m_storedWorldMatrix = ControlledEntity.PositionComp.WorldMatrix;
				}
				else
				{
					m_selected = -1;
					MyEntity controlledEntity = PickEntity();
					SetControlledEntity(controlledEntity);
				}
			}
			if (MyInput.Static.IsNewLeftMouseReleased())
			{
				m_dragActive = false;
				m_rotationActive = false;
				m_selected = -1;
			}
			if (ControlledEntity != null && ControlledEntity.Physics != null)
			{
				ControlledEntity.Physics.ClearSpeed();
			}
		}

		private void PerformRotation()
		{
			LineD lineD = CreateRayFromCursorPosition();
			Vector3D vector3D = m_rotationPlane.Intersection(ref lineD.From, ref lineD.Direction);
			if (!(Vector3D.DistanceSquared(m_rotationStartingPoint, vector3D) < 9.88131291682493E-323))
			{
				Vector3D firstVector = m_rotationStartingPoint - m_gizmoMatrix.Translation;
				Vector3D secondVector = vector3D - m_gizmoMatrix.Translation;
				MatrixD matrix = MatrixD.CreateFromQuaternion(QuaternionD.CreateFromTwoVectors(firstVector, secondVector));
				MatrixD newWorldMatrix = m_storedOrientation * matrix * m_storedScale;
				newWorldMatrix.Translation = m_storedTranslation;
				SetWorldMatrix(ref newWorldMatrix);
			}
		}

		private LineD CreateRayFromCursorPosition()
		{
			Vector2 mousePosition = MyInput.Static.GetMousePosition();
			LineD lineD = Session.Camera.WorldLineFromScreen(mousePosition);
			return new LineD(lineD.From, lineD.From + lineD.Direction * PICKING_RAY_LENGTH);
		}

		private void PerformDragg(bool lockToAxis = true)
		{
			LineD lineD = CreateRayFromCursorPosition();
			Vector3D value = m_dragPlane.Intersection(ref lineD.From, ref lineD.Direction) - m_dragStartingPoint;
			if (lockToAxis)
			{
				double num = value.Dot(ref m_dragAxis);
				if (Math.Abs(num) < double.Epsilon)
				{
					return;
				}
				MatrixD newWorldMatrix = ControlledEntity.PositionComp.WorldMatrix;
				newWorldMatrix.Translation = m_dragStartingPosition + m_dragAxis * num;
				SetWorldMatrix(ref newWorldMatrix);
			}
			else
			{
				if (value.LengthSquared() < double.Epsilon)
				{
					return;
				}
				MatrixD newWorldMatrix2 = ControlledEntity.PositionComp.WorldMatrix;
				newWorldMatrix2.Translation = m_dragStartingPosition + value;
				SetWorldMatrix(ref newWorldMatrix2);
			}
			UpdateGizmoPosition();
		}

		private bool PickControl()
		{
			if (m_xBB.Intersects(ref m_lastRay).HasValue)
			{
				m_selected = 0;
				PrepareDrag(null, m_gizmoMatrix.Right);
				m_dragActive = true;
				return true;
			}
			if (m_yBB.Intersects(ref m_lastRay).HasValue)
			{
				m_selected = 1;
				PrepareDrag(null, m_gizmoMatrix.Up);
				m_dragActive = true;
				return true;
			}
			if (m_zBB.Intersects(ref m_lastRay).HasValue)
			{
				m_selected = 2;
				PrepareDrag(null, m_gizmoMatrix.Backward);
				m_dragActive = true;
				return true;
			}
			if (m_xPlane.Intersects(ref m_lastRay).HasValue)
			{
				if (Operation == OperationMode.Rotation)
				{
					PrepareRotation(m_gizmoMatrix.Right);
					m_rotationActive = true;
				}
				else
				{
					PrepareDrag(m_gizmoMatrix.Right, null);
					m_dragActive = true;
				}
				m_selected = 3;
				return true;
			}
			if (m_yPlane.Intersects(ref m_lastRay).HasValue)
			{
				if (Operation == OperationMode.Rotation)
				{
					PrepareRotation(m_gizmoMatrix.Up);
					m_rotationActive = true;
				}
				else
				{
					PrepareDrag(m_gizmoMatrix.Up, null);
					m_dragActive = true;
				}
				m_selected = 4;
				return true;
			}
			if (m_zPlane.Intersects(ref m_lastRay).HasValue)
			{
				if (Operation == OperationMode.Rotation)
				{
					PrepareRotation(m_gizmoMatrix.Backward);
					m_rotationActive = true;
				}
				else
				{
					PrepareDrag(m_gizmoMatrix.Backward, null);
					m_dragActive = true;
				}
				m_selected = 5;
				return true;
			}
			return false;
		}

		private void SetWorldMatrix(ref MatrixD newWorldMatrix)
		{
			MyCubeGrid myCubeGrid = ControlledEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid);
				MatrixD worldMatrixNormalizedInv = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
				myCubeGrid.PositionComp.WorldMatrix = newWorldMatrix;
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node member in group.m_members)
				{
					if (member.NodeData.Parent == null && member.NodeData != myCubeGrid)
					{
						MatrixD worldMatrix = member.NodeData.PositionComp.WorldMatrix * worldMatrixNormalizedInv * newWorldMatrix;
						member.NodeData.PositionComp.WorldMatrix = worldMatrix;
					}
				}
			}
			else if (ControlledEntity.Parent != null)
			{
				ControlledEntity.PositionComp.SetWorldMatrix(newWorldMatrix, ControlledEntity.Parent, forceUpdate: true);
			}
			else
			{
				ControlledEntity.PositionComp.WorldMatrix = newWorldMatrix;
			}
		}

		private void PrepareRotation(Vector3D axis)
		{
			m_rotationAxis = axis;
			m_rotationPlane = new PlaneD(m_gizmoMatrix.Translation, m_rotationAxis);
			m_rotationStartingPoint = m_rotationPlane.Intersection(ref m_lastRay.From, ref m_lastRay.Direction);
			MatrixD worldMatrix = ControlledEntity.PositionComp.WorldMatrix;
			m_storedScale = MatrixD.CreateScale(worldMatrix.Scale);
			m_storedTranslation = worldMatrix.Translation;
			m_storedOrientation = worldMatrix.GetOrientation();
		}

		private void PrepareDrag(Vector3D? planeNormal, Vector3D? axis)
		{
			if (axis.HasValue)
			{
				Vector3D vector = Session.Camera.Position - m_gizmoMatrix.Translation;
				Vector3D vector2 = Vector3D.Cross(axis.Value, vector);
				planeNormal = Vector3D.Cross(axis.Value, vector2);
				m_dragPlane = new PlaneD(m_gizmoMatrix.Translation, planeNormal.Value);
			}
			else if (planeNormal.HasValue)
			{
				m_dragPlane = new PlaneD(m_gizmoMatrix.Translation, planeNormal.Value);
			}
			m_dragStartingPoint = m_dragPlane.Intersection(ref m_lastRay.From, ref m_lastRay.Direction);
			if (axis.HasValue)
			{
				m_dragAxis = axis.Value;
			}
			m_dragOverAxis = axis.HasValue;
			m_dragStartingPosition = ControlledEntity.PositionComp.GetPosition();
		}

		private MyEntity PickEntity()
		{
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(m_lastRay.From, m_lastRay.To);
			m_rayCastResultList.Clear();
			MyGamePruningStructure.GetAllEntitiesInRay(ref m_lastRay, m_rayCastResultList);
			int num = 0;
			for (int i = 0; i < m_rayCastResultList.Count; i++)
			{
				if (!(m_rayCastResultList[i].Element is MyCubeGrid))
				{
					m_rayCastResultList.Swap(num, i);
					num++;
				}
			}
			if (m_rayCastResultList.Count == 0 && !hitInfo.HasValue)
			{
				return null;
			}
			MyEntity myEntity = null;
			double distance = double.MaxValue;
			foreach (MyLineSegmentOverlapResult<MyEntity> rayCastResult in m_rayCastResultList)
			{
				if (rayCastResult.Element.PositionComp.WorldAABB.Intersects(ref m_lastRay, out distance) && (rayCastResult.Element is MyCubeGrid || rayCastResult.Element is MyFloatingObject || rayCastResult.Element.GetType() == typeof(MyEntity)))
				{
					myEntity = rayCastResult.Element;
					break;
				}
			}
			if (hitInfo.HasValue && Vector3D.Distance(hitInfo.Value.Position, m_lastRay.From) < distance)
			{
				IMyEntity hitEntity = hitInfo.Value.HkHitInfo.GetHitEntity();
				if (hitEntity is MyCubeGrid || hitEntity is MyFloatingObject)
				{
					return (MyEntity)hitEntity;
				}
			}
			if (myEntity == null)
			{
				myEntity = ControlledEntity;
			}
			if (myEntity == null)
			{
				return null;
			}
			return myEntity;
		}

		public void SetControlledEntity(MyEntity entity)
		{
			if (ControlledEntity != entity)
			{
				if (ControlledEntity != null)
				{
					ControlledEntity.PositionComp.OnPositionChanged -= ControlledEntityPositionChanged;
					Physics_RestorePreviousCollisionLayerState();
				}
				MyEntity controlledEntity = ControlledEntity;
				m_controlledEntity = entity;
				if (this.ControlledEntityChanged != null)
				{
					this.ControlledEntityChanged(controlledEntity, entity);
				}
				if (entity != null)
				{
					ControlledEntity.PositionComp.OnPositionChanged += ControlledEntityPositionChanged;
					ControlledEntity.OnClosing += delegate(MyEntity myEntity)
					{
						myEntity.PositionComp.OnPositionChanged -= ControlledEntityPositionChanged;
						m_controlledEntity = null;
					};
					Physics_ClearCollisionLayerCache();
					Physics_MoveEntityToNoCollisionLayer(ControlledEntity);
					UpdateGizmoPosition();
				}
			}
		}

		private void Physics_RestorePreviousCollisionLayerState()
		{
			foreach (KeyValuePair<MyEntity, int> cachedBodyCollisionLayer in m_cachedBodyCollisionLayers)
			{
				if (cachedBodyCollisionLayer.Key.Physics != null)
				{
					cachedBodyCollisionLayer.Key.Physics.RigidBody.Layer = cachedBodyCollisionLayer.Value;
				}
			}
		}

		private void Physics_ClearCollisionLayerCache()
		{
			m_cachedBodyCollisionLayers.Clear();
		}

		private void Physics_MoveEntityToNoCollisionLayer(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node member in MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid).m_members)
				{
					if (member.NodeData.Parent == null)
					{
						Physics_MoveEntityToNoCollisionLayerRecursive(member.NodeData);
					}
				}
			}
			else
			{
				Physics_MoveEntityToNoCollisionLayerRecursive(entity);
			}
		}

		private void Physics_MoveEntityToNoCollisionLayerRecursive(MyEntity entity)
		{
			if (entity.Physics != null)
			{
				m_cachedBodyCollisionLayers.Add(entity, entity.Physics.RigidBody.Layer);
				entity.Physics.RigidBody.Layer = 19;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				if (child.Entity.Physics != null && child.Entity.Physics.RigidBody != null)
				{
					Physics_MoveEntityToNoCollisionLayerRecursive((MyEntity)child.Entity);
				}
			}
		}

		private void ControlledEntityPositionChanged(MyPositionComponentBase myPositionComponentBase)
		{
			UpdateGizmoPosition();
		}

		private void UpdateGizmoPosition()
		{
			MatrixD worldMatrix = ControlledEntity.PositionComp.WorldMatrix;
			double num = ControlledEntity.PositionComp.WorldVolume.Radius;
			if (num <= 0.0)
			{
				num += 1.0;
			}
			double num2 = (ControlledEntity.PositionComp.WorldVolume.Center - worldMatrix.Translation).Length();
			num += (double)(float)num2;
			m_gizmoMatrix = MatrixD.Identity;
			if (Mode == CoordinateMode.LocalCoords)
			{
				m_gizmoMatrix = worldMatrix;
				m_gizmoMatrix = MatrixD.Normalize(m_gizmoMatrix);
			}
			else
			{
				m_gizmoMatrix.Translation = worldMatrix.Translation;
			}
			m_xBB.Center = new Vector3D(num, 0.0, 0.0);
			m_yBB.Center = new Vector3D(0.0, num, 0.0);
			m_zBB.Center = new Vector3D(0.0, 0.0, num);
			m_xBB.Orientation = Quaternion.Identity;
			m_yBB.Orientation = Quaternion.Identity;
			m_zBB.Orientation = Quaternion.Identity;
			m_xBB.Transform(m_gizmoMatrix);
			m_yBB.Transform(m_gizmoMatrix);
			m_zBB.Transform(m_gizmoMatrix);
			m_xPlane.Center = new Vector3D((0f - PLANE_THICKNESS) / 2f, num / 2.0, num / 2.0);
			m_yPlane.Center = new Vector3D(num / 2.0, PLANE_THICKNESS / 2f, num / 2.0);
			m_zPlane.Center = new Vector3D(num / 2.0, num / 2.0, PLANE_THICKNESS / 2f);
			m_xPlane.HalfExtent = new Vector3D(PLANE_THICKNESS / 2f, num / 2.0, num / 2.0);
			m_yPlane.HalfExtent = new Vector3D(num / 2.0, PLANE_THICKNESS / 2f, num / 2.0);
			m_zPlane.HalfExtent = new Vector3D(num / 2.0, num / 2.0, PLANE_THICKNESS / 2f);
			m_xPlane.Orientation = Quaternion.Identity;
			m_yPlane.Orientation = Quaternion.Identity;
			m_zPlane.Orientation = Quaternion.Identity;
			m_xPlane.Transform(m_gizmoMatrix);
			m_yPlane.Transform(m_gizmoMatrix);
			m_zPlane.Transform(m_gizmoMatrix);
		}
	}
}
