using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	internal class MyFloatingObjectClipboard
	{
		private List<MyObjectBuilder_FloatingObject> m_copiedFloatingObjects = new List<MyObjectBuilder_FloatingObject>();

		private List<Vector3D> m_copiedFloatingObjectOffsets = new List<Vector3D>();

		private List<MyFloatingObject> m_previewFloatingObjects = new List<MyFloatingObject>();

		private Vector3D m_pastePosition;

		private Vector3D m_pastePositionPrevious;

		private bool m_calculateVelocity = true;

		private Vector3 m_objectVelocity = Vector3.Zero;

		private float m_pasteOrientationAngle;

		private Vector3 m_pasteDirUp = new Vector3(1f, 0f, 0f);

		private Vector3 m_pasteDirForward = new Vector3(0f, 1f, 0f);

		private float m_dragDistance;

		private Vector3D m_dragPointToPositionLocal;

		private bool m_canBePlaced;

		private List<MyPhysics.HitInfo> m_raycastCollisionResults = new List<MyPhysics.HitInfo>();

		private float m_closestHitDistSq = float.MaxValue;

		private Vector3D m_hitPos = new Vector3(0f, 0f, 0f);

		private Vector3 m_hitNormal = new Vector3(1f, 0f, 0f);

		private bool m_visible = true;

		private bool m_enableStationRotation;

		public bool IsActive
		{
			get;
			private set;
		}

		public List<MyFloatingObject> PreviewFloatingObjects => m_previewFloatingObjects;

		public bool EnableStationRotation
		{
			get
			{
				if (m_enableStationRotation)
				{
					return MyFakes.ENABLE_STATION_ROTATION;
				}
				return false;
			}
			set
			{
				m_enableStationRotation = value;
			}
		}

		public string CopiedGridsName
		{
			get
			{
				if (HasCopiedFloatingObjects())
				{
					return m_copiedFloatingObjects[0].Name;
				}
				return null;
			}
		}

		public MyFloatingObjectClipboard(bool calculateVelocity = true)
		{
			m_calculateVelocity = calculateVelocity;
		}

		private void Activate()
		{
			ChangeClipboardPreview(visible: true);
			IsActive = true;
		}

		public void Deactivate()
		{
			ChangeClipboardPreview(visible: false);
			IsActive = false;
		}

		public void Hide()
		{
			ChangeClipboardPreview(visible: false);
		}

		public void Show()
		{
			if (IsActive && m_previewFloatingObjects.Count == 0)
			{
				ChangeClipboardPreview(visible: true);
			}
		}

		public void CutFloatingObject(MyFloatingObject floatingObject)
		{
			if (floatingObject != null)
			{
				CopyfloatingObject(floatingObject);
				DeleteFloatingObject(floatingObject);
			}
		}

		public void DeleteFloatingObject(MyFloatingObject floatingObject)
		{
			if (floatingObject != null)
			{
				MyFloatingObjects.RemoveFloatingObject(floatingObject, sync: true);
				Deactivate();
			}
		}

		public void CopyfloatingObject(MyFloatingObject floatingObject)
		{
			if (floatingObject != null)
			{
				m_copiedFloatingObjects.Clear();
				m_copiedFloatingObjectOffsets.Clear();
				CopyFloatingObjectInternal(floatingObject);
				Activate();
			}
		}

		private void CopyFloatingObjectInternal(MyFloatingObject toCopy)
		{
			m_copiedFloatingObjects.Add((MyObjectBuilder_FloatingObject)toCopy.GetObjectBuilder(copy: true));
			if (m_copiedFloatingObjects.Count == 1)
			{
				MatrixD pasteMatrix = GetPasteMatrix();
				Vector3D translation = toCopy.WorldMatrix.Translation;
				m_dragPointToPositionLocal = Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - translation, toCopy.PositionComp.WorldMatrixNormalizedInv);
				m_dragDistance = (float)(translation - pasteMatrix.Translation).Length();
				m_pasteDirUp = toCopy.WorldMatrix.Up;
				m_pasteDirForward = toCopy.WorldMatrix.Forward;
				m_pasteOrientationAngle = 0f;
			}
			m_copiedFloatingObjectOffsets.Add(toCopy.WorldMatrix.Translation - m_copiedFloatingObjects[0].PositionAndOrientation.Value.Position);
		}

		public bool PasteFloatingObject(MyInventory buildInventory = null)
		{
			if (m_copiedFloatingObjects.Count == 0)
			{
				return false;
			}
			if (m_copiedFloatingObjects.Count > 0 && !IsActive)
			{
				if (CheckPastedFloatingObjects())
				{
					Activate();
				}
				else
				{
					MyHud.Notifications.Add(MyNotificationSingletons.CopyPasteBlockNotAvailable);
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				}
				return true;
			}
			if (!m_canBePlaced)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceItem);
			MyEntities.RemapObjectBuilderCollection(m_copiedFloatingObjects);
			bool result = false;
			int num = 0;
			foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in m_copiedFloatingObjects)
			{
				copiedFloatingObject.PersistentFlags = (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
				copiedFloatingObject.PositionAndOrientation = new MyPositionAndOrientation(m_previewFloatingObjects[num].WorldMatrix);
				num++;
				MyFloatingObjects.RequestSpawnCreative(copiedFloatingObject);
				result = true;
			}
			Deactivate();
			return result;
		}

		private bool CheckPastedFloatingObjects()
		{
			foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in m_copiedFloatingObjects)
			{
				MyDefinitionId id = copiedFloatingObject.Item.PhysicalContent.GetId();
				if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(id, out MyPhysicalItemDefinition _))
				{
					return false;
				}
			}
			return true;
		}

		public void SetFloatingObjectFromBuilder(MyObjectBuilder_FloatingObject floatingObject, Vector3D dragPointDelta, float dragVectorLength)
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedFloatingObjects.Clear();
			m_copiedFloatingObjectOffsets.Clear();
			_ = (Matrix)GetPasteMatrix();
			m_dragPointToPositionLocal = dragPointDelta;
			m_dragDistance = dragVectorLength;
			MyPositionAndOrientation myPositionAndOrientation = floatingObject.PositionAndOrientation ?? MyPositionAndOrientation.Default;
			m_pasteDirUp = myPositionAndOrientation.Up;
			m_pasteDirForward = myPositionAndOrientation.Forward;
			SetFloatingObjectFromBuilderInternal(floatingObject, Vector3D.Zero);
			Activate();
		}

		private void SetFloatingObjectFromBuilderInternal(MyObjectBuilder_FloatingObject floatingObject, Vector3D offset)
		{
			m_copiedFloatingObjects.Add(floatingObject);
			m_copiedFloatingObjectOffsets.Add(offset);
		}

		private void ChangeClipboardPreview(bool visible)
		{
			if (m_copiedFloatingObjects.Count == 0 || !visible)
			{
				foreach (MyFloatingObject previewFloatingObject in m_previewFloatingObjects)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject, enable: false);
					previewFloatingObject.Close();
				}
				m_previewFloatingObjects.Clear();
				m_visible = false;
			}
			else
			{
				MyEntities.RemapObjectBuilderCollection(m_copiedFloatingObjects);
				foreach (MyObjectBuilder_FloatingObject copiedFloatingObject in m_copiedFloatingObjects)
				{
					MyFloatingObject myFloatingObject = MyEntities.CreateFromObjectBuilder(copiedFloatingObject, fadeIn: false) as MyFloatingObject;
					if (myFloatingObject == null)
					{
						ChangeClipboardPreview(visible: false);
						break;
					}
					MakeTransparent(myFloatingObject);
					IsActive = visible;
					m_visible = visible;
					MyEntities.Add(myFloatingObject);
					MyFloatingObjects.UnregisterFloatingObject(myFloatingObject);
					myFloatingObject.Save = false;
					DisablePhysicsRecursively(myFloatingObject);
					m_previewFloatingObjects.Add(myFloatingObject);
				}
			}
		}

		private void MakeTransparent(MyFloatingObject floatingObject)
		{
			floatingObject.Render.Transparency = 0.25f;
		}

		private void DisablePhysicsRecursively(MyEntity entity)
		{
			if (entity.Physics != null && entity.Physics.Enabled)
			{
				entity.Physics.Enabled = false;
			}
			MyFloatingObject myFloatingObject = entity as MyFloatingObject;
			if (myFloatingObject != null)
			{
				myFloatingObject.NeedsUpdate = MyEntityUpdateEnum.NONE;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				DisablePhysicsRecursively(child.Container.Entity as MyEntity);
			}
		}

		public void Update()
		{
			if (IsActive && m_visible)
			{
				UpdateHitEntity();
				UpdatePastePosition();
				UpdateFloatingObjectTransformations();
				if (m_calculateVelocity)
				{
					m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
				}
				m_canBePlaced = TestPlacement();
				UpdatePreviewBBox();
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
				}
			}
		}

		private void UpdateHitEntity()
		{
			MatrixD pasteMatrix = GetPasteMatrix();
			MyPhysics.CastRay(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * m_dragDistance, m_raycastCollisionResults);
			m_closestHitDistSq = float.MaxValue;
			m_hitPos = new Vector3D(0.0, 0.0, 0.0);
			m_hitNormal = new Vector3(1f, 0f, 0f);
			foreach (MyPhysics.HitInfo raycastCollisionResult in m_raycastCollisionResults)
			{
				MyPhysicsBody myPhysicsBody = (MyPhysicsBody)raycastCollisionResult.HkHitInfo.Body.UserObject;
				if (myPhysicsBody != null)
				{
					IMyEntity entity = myPhysicsBody.Entity;
					if (entity is MyVoxelMap || (entity is MyCubeGrid && entity.EntityId != m_previewFloatingObjects[0].EntityId))
					{
						float num = (float)(raycastCollisionResult.Position - pasteMatrix.Translation).LengthSquared();
						if (num < m_closestHitDistSq)
						{
							m_closestHitDistSq = num;
							m_hitPos = raycastCollisionResult.Position;
							m_hitNormal = raycastCollisionResult.HkHitInfo.Normal;
						}
					}
				}
			}
			m_raycastCollisionResults.Clear();
		}

		private bool TestPlacement()
		{
			for (int i = 0; i < m_previewFloatingObjects.Count; i++)
			{
				if (!MyEntities.IsInsideWorld(m_previewFloatingObjects[i].PositionComp.GetPosition()))
				{
					return false;
				}
			}
			return true;
		}

		private void UpdateFloatingObjectTransformations()
		{
			MatrixD matrix = GetFirstGridOrientationMatrix();
			MatrixD matrixD = MatrixD.Invert(m_copiedFloatingObjects[0].PositionAndOrientation.Value.GetMatrix()).GetOrientation() * matrix;
			for (int i = 0; i < m_previewFloatingObjects.Count; i++)
			{
				MatrixD matrix2 = m_copiedFloatingObjects[i].PositionAndOrientation.Value.GetMatrix();
				Vector3D normal = matrix2.Translation - m_copiedFloatingObjects[0].PositionAndOrientation.Value.Position;
				m_copiedFloatingObjectOffsets[i] = Vector3D.TransformNormal(normal, matrixD);
				Vector3D translation = m_pastePosition + m_copiedFloatingObjectOffsets[i];
				matrix2 *= matrixD;
				matrix2.Translation = Vector3D.Zero;
				matrix2 = MatrixD.Orthogonalize(matrix2);
				matrix2.Translation = translation;
				m_previewFloatingObjects[i].PositionComp.SetWorldMatrix(matrix2);
			}
		}

		private void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			MatrixD pasteMatrix = GetPasteMatrix();
			Vector3D value = pasteMatrix.Forward * m_dragDistance;
			m_pastePosition = pasteMatrix.Translation + value;
			MatrixD matrix = GetFirstGridOrientationMatrix();
			m_pastePosition += Vector3D.TransformNormal(m_dragPointToPositionLocal, matrix);
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawSphere(pasteMatrix.Translation + value, 0.15f, Color.Pink, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(m_pastePosition, 0.15f, Color.Pink.ToVector3(), 1f, depthRead: false);
			}
		}

		private static MatrixD GetPasteMatrix()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
			}
			return MySector.MainCamera.WorldMatrix;
		}

		private Matrix GetFirstGridOrientationMatrix()
		{
			return Matrix.CreateWorld(Vector3.Zero, m_pasteDirForward, m_pasteDirUp) * Matrix.CreateFromAxisAngle(m_pasteDirUp, m_pasteOrientationAngle);
		}

		private void UpdatePreviewBBox()
		{
			if (m_previewFloatingObjects == null)
			{
				return;
			}
			if (!m_visible)
			{
				foreach (MyFloatingObject previewFloatingObject in m_previewFloatingObjects)
				{
					MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject, enable: false);
				}
				return;
			}
			Vector4 value = new Vector4(Color.Red.ToVector3() * 0.8f, 1f);
			if (m_canBePlaced)
			{
				value = Color.Gray.ToVector4();
			}
			Vector3 value2 = new Vector3(0.1f);
			foreach (MyFloatingObject previewFloatingObject2 in m_previewFloatingObjects)
			{
				MyEntities.EnableEntityBoundingBoxDraw(previewFloatingObject2, enable: true, value, 0.01f, value2);
			}
		}

		public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
		{
		}

		public bool HasCopiedFloatingObjects()
		{
			return m_copiedFloatingObjects.Count > 0;
		}

		public void HideWhenColliding(List<Vector3D> collisionTestPoints)
		{
			if (m_previewFloatingObjects.Count != 0)
			{
				bool flag = true;
				foreach (Vector3D collisionTestPoint in collisionTestPoints)
				{
					foreach (MyFloatingObject previewFloatingObject in m_previewFloatingObjects)
					{
						Vector3D point = Vector3.Transform(collisionTestPoint, previewFloatingObject.PositionComp.WorldMatrixNormalizedInv);
						if (previewFloatingObject.PositionComp.LocalAABB.Contains(point) == ContainmentType.Contains)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				foreach (MyFloatingObject previewFloatingObject2 in m_previewFloatingObjects)
				{
					previewFloatingObject2.Render.Visible = flag;
				}
			}
		}

		public void ClearClipboard()
		{
			if (IsActive)
			{
				Deactivate();
			}
			m_copiedFloatingObjects.Clear();
			m_copiedFloatingObjectOffsets.Clear();
		}

		public void RotateAroundAxis(int axisIndex, int sign, bool newlyPressed, float angleDelta)
		{
			switch (axisIndex)
			{
			case 0:
				if (sign < 0)
				{
					UpMinus(angleDelta);
				}
				else
				{
					UpPlus(angleDelta);
				}
				break;
			case 1:
				if (sign < 0)
				{
					AngleMinus(angleDelta);
				}
				else
				{
					AnglePlus(angleDelta);
				}
				break;
			case 2:
				if (sign < 0)
				{
					RightPlus(angleDelta);
				}
				else
				{
					RightMinus(angleDelta);
				}
				break;
			}
		}

		private void AnglePlus(float angle)
		{
			m_pasteOrientationAngle += angle;
			if (m_pasteOrientationAngle >= MathF.PI * 2f)
			{
				m_pasteOrientationAngle -= MathF.PI * 2f;
			}
		}

		private void AngleMinus(float angle)
		{
			m_pasteOrientationAngle -= angle;
			if (m_pasteOrientationAngle < 0f)
			{
				m_pasteOrientationAngle += MathF.PI * 2f;
			}
		}

		private void UpPlus(float angle)
		{
			ApplyOrientationAngle();
			Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
			float scaleFactor = (float)Math.Cos(angle);
			float scaleFactor2 = (float)Math.Sin(angle);
			Vector3 pasteDirUp = m_pasteDirUp * scaleFactor - m_pasteDirForward * scaleFactor2;
			m_pasteDirForward = m_pasteDirUp * scaleFactor2 + m_pasteDirForward * scaleFactor;
			m_pasteDirUp = pasteDirUp;
		}

		private void UpMinus(float angle)
		{
			UpPlus(0f - angle);
		}

		private void RightPlus(float angle)
		{
			ApplyOrientationAngle();
			Vector3 value = Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
			float scaleFactor = (float)Math.Cos(angle);
			float scaleFactor2 = (float)Math.Sin(angle);
			m_pasteDirUp = m_pasteDirUp * scaleFactor + value * scaleFactor2;
		}

		private void RightMinus(float angle)
		{
			RightPlus(0f - angle);
		}

		public void MoveEntityFurther()
		{
			m_dragDistance *= 1.1f;
		}

		public void MoveEntityCloser()
		{
			m_dragDistance /= 1.1f;
		}

		private void ApplyOrientationAngle()
		{
			m_pasteDirForward = Vector3.Normalize(m_pasteDirForward);
			m_pasteDirUp = Vector3.Normalize(m_pasteDirUp);
			Vector3 value = Vector3.Cross(m_pasteDirForward, m_pasteDirUp);
			float scaleFactor = (float)Math.Cos(m_pasteOrientationAngle);
			float scaleFactor2 = (float)Math.Sin(m_pasteOrientationAngle);
			m_pasteDirForward = m_pasteDirForward * scaleFactor - value * scaleFactor2;
			m_pasteOrientationAngle = 0f;
		}
	}
}
