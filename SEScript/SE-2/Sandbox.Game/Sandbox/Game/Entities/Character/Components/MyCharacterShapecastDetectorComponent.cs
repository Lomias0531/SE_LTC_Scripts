using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	public class MyCharacterShapecastDetectorComponent : MyCharacterDetectorComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyCharacterShapecastDetectorComponent_003C_003EActor : IActivator, IActivator<MyCharacterShapecastDetectorComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterShapecastDetectorComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterShapecastDetectorComponent CreateInstance()
			{
				return new MyCharacterShapecastDetectorComponent();
			}

			MyCharacterShapecastDetectorComponent IActivator<MyCharacterShapecastDetectorComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const float DEFAULT_SHAPE_RADIUS = 0.1f;

		private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();

		private Vector3D m_rayOrigin = Vector3D.Zero;

		private Vector3D m_rayDirection = Vector3D.Zero;

		public float ShapeRadius
		{
			get;
			set;
		}

		public MyCharacterShapecastDetectorComponent()
		{
			ShapeRadius = 0.1f;
		}

		protected override void DoDetection(bool useHead)
		{
			DoDetection(useHead, doModelIntersection: false);
		}

		public void DoDetectionModel()
		{
			DoDetection(!base.Character.TargetFromCamera, doModelIntersection: true);
		}

		private int CompareHits(MyPhysics.HitInfo info1, MyPhysics.HitInfo info2)
		{
			IMyEntity hitEntity = info1.HkHitInfo.GetHitEntity();
			IMyEntity hitEntity2 = info2.HkHitInfo.GetHitEntity();
			Type type = hitEntity.GetType();
			Type type2 = hitEntity2.GetType();
			if (type != type2)
			{
				Type typeFromHandle = typeof(MyVoxelMap);
				if (type == typeFromHandle)
				{
					return 1;
				}
				if (type2 == typeFromHandle)
				{
					return -1;
				}
				Type typeFromHandle2 = typeof(MyVoxelPhysics);
				if (type == typeFromHandle2)
				{
					return 1;
				}
				if (type2 == typeFromHandle2)
				{
					return -1;
				}
				Type typeFromHandle3 = typeof(MyCubeGrid);
				if (type == typeFromHandle3)
				{
					return 1;
				}
				if (type2 == typeFromHandle3)
				{
					return -1;
				}
			}
			Vector3D value = info1.Position - m_rayOrigin;
			Vector3D value2 = info2.Position - m_rayOrigin;
			float value3 = Vector3.Dot(m_rayDirection, Vector3.Normalize(value));
			int num = Vector3.Dot(m_rayDirection, Vector3.Normalize(value2)).CompareTo(value3);
			if (num != 0)
			{
				return num;
			}
			int num2 = value2.LengthSquared().CompareTo(value.LengthSquared());
			if (num2 != 0)
			{
				return num2;
			}
			return 0;
		}

		private void DoDetection(bool useHead, bool doModelIntersection)
		{
			if (base.Character == MySession.Static.ControlledEntity)
			{
				MyHud.SelectedObjectHighlight.RemoveHighlight();
			}
			MatrixD headMatrix = base.Character.GetHeadMatrix(includeY: false);
			Vector3D vector3D = headMatrix.Translation;
			Vector3D forward = headMatrix.Forward;
			if (!useHead)
			{
				Vector3D vector3D2 = headMatrix.Translation - headMatrix.Forward * 0.3;
				if (base.Character == MySession.Static.LocalCharacter)
				{
					vector3D = MySector.MainCamera.WorldMatrix.Translation;
					forward = MySector.MainCamera.WorldMatrix.Forward;
					vector3D = MyUtils.LinePlaneIntersection(vector3D2, forward, vector3D, forward);
				}
				else
				{
					vector3D = vector3D2;
					forward = headMatrix.Forward;
				}
			}
			Vector3D vector3D3 = vector3D + forward * 2.5;
			base.StartPosition = vector3D;
			MatrixD transform = MatrixD.CreateTranslation(vector3D);
			HkShape shape = new HkSphereShape(ShapeRadius);
			IMyEntity myEntity = null;
			base.ShapeKey = uint.MaxValue;
			base.HitPosition = Vector3D.Zero;
			base.HitNormal = Vector3.Zero;
			base.HitMaterial = MyStringHash.NullOrEmpty;
			base.HitTag = null;
			m_hits.Clear();
			Vector3 value = Vector3D.Zero;
			try
			{
				EnableDetectorsInArea(vector3D);
				MyPhysics.CastShapeReturnContactBodyDatas(vector3D3, shape, ref transform, 0u, 0f, m_hits);
				m_rayOrigin = vector3D;
				m_rayDirection = forward;
				m_hits.Sort(CompareHits);
				if (m_hits.Count > 0)
				{
					bool flag = false;
					bool flag2 = false;
					for (int i = 0; i < m_hits.Count; i++)
					{
						HkRigidBody body = m_hits[i].HkHitInfo.Body;
						IMyEntity myEntity2 = m_hits[i].HkHitInfo.GetHitEntity();
						if (myEntity2 != base.Character)
						{
							if (myEntity2 is MyEntitySubpart)
							{
								myEntity2 = myEntity2.Parent;
							}
							flag = (body != null && myEntity2 != null && myEntity2 != base.Character && !body.HasProperty(254));
							flag2 = (myEntity2 != null && myEntity2.Physics != null);
							if (myEntity == null && flag)
							{
								myEntity = myEntity2;
								base.ShapeKey = m_hits[i].HkHitInfo.GetShapeKey(0);
							}
							if (myEntity2 is MyCubeGrid)
							{
								List<MyCube> list = (myEntity2 as MyCubeGrid).RayCastBlocksAllOrdered(vector3D, vector3D3);
								if (list != null && list.Count > 0)
								{
									MySlimBlock cubeBlock = list[0].CubeBlock;
									if (cubeBlock.FatBlock != null)
									{
										myEntity2 = cubeBlock.FatBlock;
										flag2 = true;
										myEntity = myEntity2;
										base.ShapeKey = 0u;
									}
								}
							}
							if (base.HitMaterial.Equals(MyStringHash.NullOrEmpty) && flag && flag2)
							{
								base.HitBody = body;
								base.HitNormal = m_hits[i].HkHitInfo.Normal;
								base.HitPosition = m_hits[i].GetFixedPosition();
								base.HitMaterial = body.GetBody().GetMaterialAt(base.HitPosition);
								value = base.HitPosition;
								break;
							}
							if (body != null)
							{
								value = m_hits[i].GetFixedPosition();
								break;
							}
							i++;
						}
					}
				}
			}
			finally
			{
				shape.RemoveReference();
			}
			bool flag3 = false;
			IMyUseObject myUseObject = myEntity as IMyUseObject;
			base.DetectedEntity = myEntity;
			if (myEntity != null)
			{
				MyUseObjectsComponentBase component = null;
				myEntity.Components.TryGet(out component);
				if (component != null)
				{
					myUseObject = component.GetInteractiveObject(base.ShapeKey);
				}
				if (doModelIntersection)
				{
					LineD line = new LineD(vector3D, vector3D3);
					MyCharacter myCharacter = myEntity as MyCharacter;
					if (myCharacter == null)
					{
						if (myEntity.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? tri, IntersectionFlags.ALL_TRIANGLES))
						{
							base.HitPosition = tri.Value.IntersectionPointInWorldSpace;
							base.HitNormal = tri.Value.NormalInWorldSpace;
						}
					}
					else if (myCharacter.GetIntersectionWithLine(ref line, ref CharHitInfo))
					{
						base.HitPosition = CharHitInfo.Triangle.IntersectionPointInWorldSpace;
						base.HitNormal = CharHitInfo.Triangle.NormalInWorldSpace;
						base.HitTag = CharHitInfo;
					}
				}
			}
			if (myUseObject != null && myUseObject.SupportedActions != 0 && Vector3D.Distance(vector3D, value) < (double)myUseObject.InteractiveDistance && base.Character == MySession.Static.ControlledEntity)
			{
				MyCharacterDetectorComponent.HandleInteractiveObject(myUseObject);
				base.UseObject = myUseObject;
				flag3 = true;
			}
			if (!flag3)
			{
				base.UseObject = null;
			}
			DisableDetectors();
		}
	}
}
