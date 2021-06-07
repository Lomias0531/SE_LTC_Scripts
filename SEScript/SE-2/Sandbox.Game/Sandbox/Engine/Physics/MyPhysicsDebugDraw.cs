using Havok;
using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Engine.Physics
{
	internal static class MyPhysicsDebugDraw
	{
		public static bool DebugDrawFlattenHierarchy = false;

		public static bool HkGridShapeCellDebugDraw = false;

		public static HkGeometry DebugGeometry;

		private static Color[] boxColors = MyUtils.GenerateBoxColors();

		private static List<HkShape> m_tmpShapeList = new List<HkShape>();

		private static Dictionary<string, Vector3D> DebugShapesPositions = new Dictionary<string, Vector3D>();

		private static Color GetShapeColor(HkShapeType shapeType, ref int shapeIndex, bool isPhantom)
		{
			if (isPhantom)
			{
				return Color.LightGreen;
			}
			switch (shapeType)
			{
			case HkShapeType.Sphere:
				return Color.White;
			case HkShapeType.Capsule:
				return Color.Yellow;
			case HkShapeType.Cylinder:
				return Color.Orange;
			case HkShapeType.ConvexVertices:
				return Color.Red;
			default:
				return boxColors[++shapeIndex % (boxColors.Length - 1)];
			}
		}

		public static void DrawCollisionShape(HkShape shape, MatrixD worldMatrix, float alpha, ref int shapeIndex, string customText = null, bool isPhantom = false)
		{
			Color shapeColor = GetShapeColor(shape.ShapeType, ref shapeIndex, isPhantom);
			if (isPhantom)
			{
				alpha *= alpha;
			}
			shapeColor.A = (byte)(alpha * 255f);
			bool flag = true;
			float value = 0.02f;
			float num = 1.035f;
			bool flag2 = false;
			switch (shape.ShapeType)
			{
			case HkShapeType.Sphere:
			{
				float radius = ((HkSphereShape)shape).Radius;
				MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, radius, shapeColor, alpha, depthRead: true, flag);
				if (isPhantom)
				{
					MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, radius, shapeColor);
					MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, radius, shapeColor, 1f, depthRead: true, smooth: false, cull: false);
				}
				flag2 = true;
				break;
			}
			case HkShapeType.Capsule:
			{
				HkCapsuleShape hkCapsuleShape = (HkCapsuleShape)shape;
				Vector3D p = Vector3.Transform(hkCapsuleShape.VertexA, worldMatrix);
				Vector3D p2 = Vector3.Transform(hkCapsuleShape.VertexB, worldMatrix);
				MyRenderProxy.DebugDrawCapsule(p, p2, hkCapsuleShape.Radius, shapeColor, depthRead: true, flag);
				flag2 = true;
				break;
			}
			case HkShapeType.Cylinder:
			{
				HkCylinderShape hkCylinderShape = (HkCylinderShape)shape;
				MyRenderProxy.DebugDrawCylinder(worldMatrix, hkCylinderShape.VertexA, hkCylinderShape.VertexB, hkCylinderShape.Radius, shapeColor, alpha, depthRead: true, flag);
				flag2 = true;
				break;
			}
			case HkShapeType.Box:
			{
				HkBoxShape hkBoxShape = (HkBoxShape)shape;
				MyRenderProxy.DebugDrawOBB(MatrixD.CreateScale(hkBoxShape.HalfExtents * 2f + new Vector3(value)) * worldMatrix, shapeColor, alpha, depthRead: true, flag);
				if (isPhantom)
				{
					MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(hkBoxShape.HalfExtents * 2f + new Vector3(value)) * worldMatrix, shapeColor, 1f, depthRead: true, smooth: false);
					MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(hkBoxShape.HalfExtents * 2f + new Vector3(value)) * worldMatrix, shapeColor, 1f, depthRead: true, smooth: false, cull: false);
				}
				flag2 = true;
				break;
			}
			case HkShapeType.ConvexVertices:
			{
				((HkConvexVerticesShape)shape).GetGeometry(DebugGeometry, out Vector3 center);
				Vector3D value4 = Vector3D.Transform(center, worldMatrix.GetOrientation());
				MatrixD matrix = worldMatrix;
				matrix = MatrixD.CreateScale(num) * matrix;
				matrix.Translation -= value4 * (num - 1f);
				DrawGeometry(DebugGeometry, matrix, shapeColor, depthRead: true, shaded: true);
				flag2 = true;
				break;
			}
			case HkShapeType.ConvexTranslate:
			{
				HkConvexTranslateShape hkConvexTranslateShape = (HkConvexTranslateShape)shape;
				DrawCollisionShape(hkConvexTranslateShape.ChildShape, Matrix.CreateTranslation(hkConvexTranslateShape.Translation) * worldMatrix, alpha, ref shapeIndex, customText);
				break;
			}
			case HkShapeType.ConvexTransform:
			{
				HkConvexTransformShape hkConvexTransformShape = (HkConvexTransformShape)shape;
				DrawCollisionShape(hkConvexTransformShape.ChildShape, hkConvexTransformShape.Transform * worldMatrix, alpha, ref shapeIndex, customText);
				break;
			}
			case HkShapeType.Mopp:
				DrawCollisionShape(((HkMoppBvTreeShape)shape).ShapeCollection, worldMatrix, alpha, ref shapeIndex, customText);
				break;
			case HkShapeType.List:
			{
				HkShapeContainerIterator iterator = ((HkListShape)shape).GetIterator();
				while (iterator.IsValid)
				{
					DrawCollisionShape(iterator.CurrentValue, worldMatrix, alpha, ref shapeIndex, customText);
					iterator.Next();
				}
				break;
			}
			case HkShapeType.StaticCompound:
			{
				HkStaticCompoundShape hkStaticCompoundShape = (HkStaticCompoundShape)shape;
				if (DebugDrawFlattenHierarchy)
				{
					HkShapeContainerIterator iterator3 = hkStaticCompoundShape.GetIterator();
					while (iterator3.IsValid)
					{
						if (hkStaticCompoundShape.IsShapeKeyEnabled(iterator3.CurrentShapeKey))
						{
							string customText2 = (customText ?? string.Empty) + "-" + iterator3.CurrentShapeKey + "-";
							DrawCollisionShape(iterator3.CurrentValue, worldMatrix, alpha, ref shapeIndex, customText2);
						}
						iterator3.Next();
					}
					break;
				}
				for (int j = 0; j < hkStaticCompoundShape.InstanceCount; j++)
				{
					bool flag3 = hkStaticCompoundShape.IsInstanceEnabled(j);
					string customText3 = (!flag3) ? ((customText ?? string.Empty) + "(" + j + ")") : ((customText ?? string.Empty) + "<" + j + ">");
					if (flag3)
					{
						DrawCollisionShape(hkStaticCompoundShape.GetInstance(j), hkStaticCompoundShape.GetInstanceTransform(j) * worldMatrix, alpha, ref shapeIndex, customText3);
					}
				}
				break;
			}
			case HkShapeType.Triangle:
			{
				HkTriangleShape hkTriangleShape2 = (HkTriangleShape)shape;
				MyRenderProxy.DebugDrawTriangle(hkTriangleShape2.Pt0, hkTriangleShape2.Pt1, hkTriangleShape2.Pt2, Color.Green, smooth: false, depthRead: false);
				break;
			}
			case HkShapeType.BvTree:
			{
				HkGridShape hkGridShape = (HkGridShape)shape;
				if (HkGridShapeCellDebugDraw && !hkGridShape.Base.IsZero)
				{
					float cellSize = hkGridShape.CellSize;
					int shapeInfoCount = hkGridShape.GetShapeInfoCount();
					for (int i = 0; i < shapeInfoCount; i++)
					{
						try
						{
							hkGridShape.GetShapeInfo(i, out Vector3S min, out Vector3S max, m_tmpShapeList);
							Vector3 value2 = max * cellSize - min * cellSize;
							Vector3 position = (max * cellSize + min * cellSize) / 2f;
							Vector3 value3 = value2 + Vector3.One * cellSize;
							Color color = shapeColor;
							if (min == max)
							{
								color = new Color(1f, 0.2f, 0.1f);
							}
							MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(value3 + new Vector3(value)) * Matrix.CreateTranslation(position) * worldMatrix, color, alpha, depthRead: true, flag);
						}
						finally
						{
							m_tmpShapeList.Clear();
						}
					}
				}
				else
				{
					MyRenderMessageDebugDrawTriangles myRenderMessageDebugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
					try
					{
						using (HkShapeBuffer buffer = new HkShapeBuffer())
						{
							HkShapeContainerIterator iterator2 = ((HkBvTreeShape)shape).GetIterator(buffer);
							while (iterator2.IsValid)
							{
								HkShape currentValue = iterator2.CurrentValue;
								if (currentValue.ShapeType == HkShapeType.Triangle)
								{
									HkTriangleShape hkTriangleShape = (HkTriangleShape)currentValue;
									myRenderMessageDebugDrawTriangles.AddTriangle(hkTriangleShape.Pt0, hkTriangleShape.Pt1, hkTriangleShape.Pt2);
								}
								else
								{
									DrawCollisionShape(currentValue, worldMatrix, alpha, ref shapeIndex);
								}
								iterator2.Next();
							}
						}
					}
					finally
					{
						myRenderMessageDebugDrawTriangles.Color = shapeColor;
						MyRenderProxy.DebugDrawTriangles(myRenderMessageDebugDrawTriangles, worldMatrix, depthRead: false, shaded: false);
					}
				}
				break;
			}
			case HkShapeType.BvCompressedMesh:
				if (MyDebugDrawSettings.DEBUG_DRAW_TRIANGLE_PHYSICS)
				{
					((HkBvCompressedMeshShape)shape).GetGeometry(DebugGeometry);
					DrawGeometry(DebugGeometry, worldMatrix, Color.Green);
					flag2 = true;
				}
				break;
			case HkShapeType.Bv:
			{
				HkBvShape hkBvShape = (HkBvShape)shape;
				DrawCollisionShape(hkBvShape.BoundingVolumeShape, worldMatrix, alpha, ref shapeIndex, null, isPhantom: true);
				DrawCollisionShape(hkBvShape.ChildShape, worldMatrix, alpha, ref shapeIndex);
				break;
			}
			case HkShapeType.PhantomCallback:
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Phantom", Color.Green, 0.75f, depthRead: false);
				break;
			}
			if (flag2 && customText != null)
			{
				shapeColor.A = byte.MaxValue;
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, customText, shapeColor, 0.8f, depthRead: false);
			}
		}

		public static void DrawGeometry(HkGeometry geometry, MatrixD worldMatrix, Color color, bool depthRead = false, bool shaded = false)
		{
			MyRenderMessageDebugDrawTriangles myRenderMessageDebugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
			try
			{
				for (int i = 0; i < geometry.TriangleCount; i++)
				{
					geometry.GetTriangle(i, out int i2, out int i3, out int i4, out int _);
					myRenderMessageDebugDrawTriangles.AddIndex(i2);
					myRenderMessageDebugDrawTriangles.AddIndex(i3);
					myRenderMessageDebugDrawTriangles.AddIndex(i4);
				}
				for (int j = 0; j < geometry.VertexCount; j++)
				{
					myRenderMessageDebugDrawTriangles.AddVertex(geometry.GetVertex(j));
				}
			}
			finally
			{
				myRenderMessageDebugDrawTriangles.Color = color;
				MyRenderProxy.DebugDrawTriangles(myRenderMessageDebugDrawTriangles, worldMatrix, depthRead, shaded);
			}
		}

		public static void DebugDrawBreakable(HkdBreakableBody bb, Vector3 offset)
		{
			DebugShapesPositions.Clear();
			if (bb != null)
			{
				int shapeIndex = 0;
				Matrix rigidBodyMatrix = bb.GetRigidBody().GetRigidBodyMatrix();
				MatrixD worldMatrix = MatrixD.CreateWorld(rigidBodyMatrix.Translation + offset, rigidBodyMatrix.Forward, rigidBodyMatrix.Up);
				DrawBreakableShape(bb.BreakableShape, worldMatrix, 0.3f, ref shapeIndex);
				DrawConnections(bb.BreakableShape, worldMatrix, 0.3f, ref shapeIndex);
			}
		}

		private static void DrawBreakableShape(HkdBreakableShape breakableShape, MatrixD worldMatrix, float alpha, ref int shapeIndex, string customText = null, bool isPhantom = false)
		{
			DrawCollisionShape(breakableShape.GetShape(), worldMatrix, alpha, ref shapeIndex, breakableShape.Name + " Strength: " + breakableShape.GetStrenght() + " Static:" + breakableShape.IsFixed().ToString());
			if (!string.IsNullOrEmpty(breakableShape.Name) && breakableShape.Name != "PineTree175m_v2_001")
			{
				breakableShape.IsFixed();
			}
			DebugShapesPositions[breakableShape.Name] = worldMatrix.Translation;
			List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
			breakableShape.GetChildren(list);
			_ = breakableShape.CoM;
			foreach (HkdShapeInstanceInfo item in list)
			{
				Matrix m = item.GetTransform() * worldMatrix * Matrix.CreateTranslation(Vector3.Right * 2f);
				DrawBreakableShape(item.Shape, m, alpha, ref shapeIndex);
			}
		}

		private static void DrawConnections(HkdBreakableShape breakableShape, MatrixD worldMatrix, float alpha, ref int shapeIndex, string customText = null, bool isPhantom = false)
		{
			List<HkdConnection> list = new List<HkdConnection>();
			breakableShape.GetConnectionList(list);
			List<HkdShapeInstanceInfo> list2 = new List<HkdShapeInstanceInfo>();
			breakableShape.GetChildren(list2);
			foreach (HkdConnection item in list)
			{
				Vector3D pointFrom = DebugShapesPositions[item.ShapeAName];
				Vector3D pointTo = DebugShapesPositions[item.ShapeBName];
				bool flag = false;
				foreach (HkdShapeInstanceInfo item2 in list2)
				{
					if (item2.ShapeName == item.ShapeAName || item2.ShapeName == item.ShapeBName)
					{
						flag = true;
					}
				}
				if (flag)
				{
					MyRenderProxy.DebugDrawLine3D(pointFrom, pointTo, Color.White, Color.White, depthRead: false);
				}
			}
		}

		public static void DebugDrawAddForce(MyPhysicsBody physics, MyPhysicsForceType type, Vector3? force, Vector3D? position, Vector3? torque, bool persistent = false)
		{
			switch (type)
			{
			case MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE:
				if (physics.RigidBody != null)
				{
					Matrix rigidBodyMatrix = physics.RigidBody.GetRigidBodyMatrix();
					Vector3D vector3D3 = physics.CenterOfMassWorld + physics.LinearVelocity * 0.0166666675f;
					if (force.HasValue)
					{
						Vector3 value = Vector3.TransformNormal(force.Value, rigidBodyMatrix) * 0.1f;
						MyRenderProxy.DebugDrawArrow3D(vector3D3, vector3D3 + value, Color.Blue, Color.Red, depthRead: false, 0.1, null, 0.5f, persistent);
					}
					if (torque.HasValue)
					{
						Vector3 value2 = Vector3.TransformNormal(torque.Value, rigidBodyMatrix) * 0.1f;
						MyRenderProxy.DebugDrawArrow3D(vector3D3, vector3D3 + value2, Color.Blue, Color.Purple, depthRead: false, 0.1, null, 0.5f, persistent);
					}
				}
				break;
			case MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE:
			{
				Vector3D vector3D2 = position.Value + physics.LinearVelocity * 0.0166666675f;
				if (force.HasValue)
				{
					MyRenderProxy.DebugDrawArrow3D(vector3D2, vector3D2 + force.Value * 0.1f, Color.Blue, Color.Red, depthRead: false, 0.1, null, 0.5f, persistent);
				}
				if (torque.HasValue)
				{
					MyRenderProxy.DebugDrawArrow3D(vector3D2, vector3D2 + torque.Value * 0.1f, Color.Blue, Color.Purple, depthRead: false, 0.1, null, 0.5f, persistent);
				}
				break;
			}
			case MyPhysicsForceType.APPLY_WORLD_FORCE:
				if (position.HasValue)
				{
					Vector3D vector3D = position.Value + physics.LinearVelocity * 0.0166666675f;
					if (force.HasValue)
					{
						MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + force.Value * 0.0166666675f * 0.1f, Color.Blue, Color.Red, depthRead: false, 0.1, null, 0.5f, persistent);
					}
				}
				break;
			}
		}

		public static void DebugDrawCoordinateSystem(Vector3? position, Vector3? forward, Vector3? side, Vector3? up, float scale = 1f)
		{
			if (position.HasValue)
			{
				Vector3D vector3D = position.Value;
				if (forward.HasValue)
				{
					Vector3 value = forward.Value * scale;
					MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + value, Color.Blue, Color.Red);
				}
				if (side.HasValue)
				{
					Vector3 value2 = side.Value * scale;
					MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + value2, Color.Blue, Color.Green);
				}
				if (up.HasValue)
				{
					Vector3 value3 = up.Value * scale;
					MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + value3, Color.Blue, Color.Blue);
				}
			}
		}

		public static void DebugDrawVector3(Vector3? position, Vector3? vector, Color color, float scale = 0.01f)
		{
			if (position.HasValue)
			{
				Vector3D vector3D = position.Value;
				if (vector.HasValue)
				{
					Vector3 value = vector.Value * scale;
					MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + value, color, color);
				}
			}
		}
	}
}
