using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Game;
using VRage.Network;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentCubeGrid : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentCubeGrid_003C_003EActor : IActivator, IActivator<MyRenderComponentCubeGrid>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentCubeGrid();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentCubeGrid CreateInstance()
			{
				return new MyRenderComponentCubeGrid();
			}

			MyRenderComponentCubeGrid IActivator<MyRenderComponentCubeGrid>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyStringId ID_RED_DOT_IGNORE_DEPTH = MyStringId.GetOrCompute("RedDotIgnoreDepth");

		private static readonly MyStringId ID_WEAPON_LASER_IGNORE_DEPTH = MyStringId.GetOrCompute("WeaponLaserIgnoreDepth");

		private static readonly List<MyPhysics.HitInfo> m_tmpHitList = new List<MyPhysics.HitInfo>();

		private MyCubeGrid m_grid;

		private bool m_deferRenderRelease;

		private bool m_shouldReleaseRenderObjects;

		private MyCubeGridRenderData m_renderData;

		private MyParticleEffect m_atmosphericEffect;

		private const float m_atmosphericEffectMinSpeed = 75f;

		private const float m_atmosphericEffectMinFade = 0.85f;

		private const int m_atmosphericEffectVoxelContactDelay = 5000;

		private int m_lastVoxelContactTime;

		private float m_lastWorkingIntersectDistance;

		private static List<Vector3> m_tmpCornerList = new List<Vector3>();

		private List<IMyBlockAdditionalModelGenerator> m_additionalModelGenerators = new List<IMyBlockAdditionalModelGenerator>();

		public MyCubeGrid CubeGrid => m_grid;

		public bool DeferRenderRelease
		{
			get
			{
				return m_deferRenderRelease;
			}
			set
			{
				m_deferRenderRelease = value;
				if (!value && m_shouldReleaseRenderObjects)
				{
					RemoveRenderObjects();
				}
			}
		}

		public MyCubeGridRenderData RenderData => m_renderData;

		public List<IMyBlockAdditionalModelGenerator> AdditionalModelGenerators => m_additionalModelGenerators;

		public MyCubeSize GridSizeEnum => m_grid.GridSizeEnum;

		public float GridSize => m_grid.GridSize;

		public bool IsStatic => m_grid.IsStatic;

		public MyRenderComponentCubeGrid()
		{
			m_renderData = new MyCubeGridRenderData(this);
		}

		public void CreateAdditionalModelGenerators(MyCubeSize gridSizeEnum)
		{
			Assembly[] array = new Assembly[3]
			{
				Assembly.GetExecutingAssembly(),
				MyPlugins.GameAssembly,
				MyPlugins.SandboxAssembly
			};
			if (MyPlugins.UserAssemblies != null)
			{
				array = array.Union(MyPlugins.UserAssemblies).ToArray();
			}
			Assembly[] array2 = array;
			foreach (Assembly assembly in array2)
			{
				if (!(assembly == null))
				{
					Type lookupType = typeof(IMyBlockAdditionalModelGenerator);
					foreach (Type item in from t in assembly.GetTypes()
						where lookupType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract
						select t)
					{
						IMyBlockAdditionalModelGenerator myBlockAdditionalModelGenerator = Activator.CreateInstance(item) as IMyBlockAdditionalModelGenerator;
						if (myBlockAdditionalModelGenerator.Initialize(m_grid, gridSizeEnum))
						{
							AdditionalModelGenerators.Add(myBlockAdditionalModelGenerator);
						}
						else
						{
							myBlockAdditionalModelGenerator.Close();
						}
					}
				}
			}
		}

		public void CloseModelGenerators()
		{
			foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in AdditionalModelGenerators)
			{
				additionalModelGenerator.Close();
			}
			AdditionalModelGenerators.Clear();
		}

		public void RebuildDirtyCells()
		{
			m_renderData.RebuildDirtyCells(GetRenderFlags());
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_grid = (base.Container.Entity as MyCubeGrid);
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (m_atmosphericEffect != null)
			{
				MyParticlesManager.RemoveParticleEffect(m_atmosphericEffect);
				m_atmosphericEffect = null;
			}
		}

		public override void Draw()
		{
			base.Draw();
			foreach (MyCubeBlock item in m_grid.BlocksForDraw)
			{
				if (MyRenderProxy.VisibleObjectsRead.Contains(item.Render.RenderObjectIDs[0]))
				{
					item.Render.Draw();
				}
			}
			if (MyCubeGrid.ShowCenterOfMass && !IsStatic && base.Container.Entity.Physics != null && base.Container.Entity.Physics.HasRigidBody)
			{
				MatrixD worldMatrix = base.Container.Entity.Physics.GetWorldMatrix();
				Vector3D centerOfMassWorld = base.Container.Entity.Physics.CenterOfMassWorld;
				Vector3D position = MySector.MainCamera.Position;
				float num = Vector3.Distance(position, centerOfMassWorld);
				bool flag = false;
				if (num < 30f)
				{
					flag = true;
				}
				else if (num < 200f)
				{
					flag = true;
					MyPhysics.CastRay(position, centerOfMassWorld, m_tmpHitList, 16);
					foreach (MyPhysics.HitInfo tmpHit in m_tmpHitList)
					{
						if (tmpHit.HkHitInfo.GetHitEntity() != this)
						{
							flag = false;
							break;
						}
					}
					m_tmpHitList.Clear();
				}
				if (flag)
				{
					float num2 = MathHelper.Lerp(1f, 9f, num / 200f);
					MyStringId iD_WEAPON_LASER_IGNORE_DEPTH = ID_WEAPON_LASER_IGNORE_DEPTH;
					Vector4 color = Color.Yellow.ToVector4();
					float thickness = 0.02f * num2;
					MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Up * 0.5 * num2, centerOfMassWorld + worldMatrix.Up * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
					MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Forward * 0.5 * num2, centerOfMassWorld + worldMatrix.Forward * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
					MySimpleObjectDraw.DrawLine(centerOfMassWorld - worldMatrix.Right * 0.5 * num2, centerOfMassWorld + worldMatrix.Right * 0.5 * num2, iD_WEAPON_LASER_IGNORE_DEPTH, ref color, thickness, MyBillboard.BlendTypeEnum.AdditiveTop);
					MyTransparentGeometry.AddBillboardOriented(ID_RED_DOT_IGNORE_DEPTH, Color.White.ToVector4(), centerOfMassWorld, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, 0.1f * num2, MyBillboard.BlendTypeEnum.AdditiveTop);
				}
			}
			if (MyCubeGrid.ShowGridPivot)
			{
				MatrixD worldMatrix2 = base.Container.Entity.WorldMatrix;
				Vector3D translation = worldMatrix2.Translation;
				Vector3D position2 = MySector.MainCamera.Position;
				float num3 = Vector3.Distance(position2, translation);
				bool flag2 = false;
				if (num3 < 30f)
				{
					flag2 = true;
				}
				else if (num3 < 200f)
				{
					flag2 = true;
					MyPhysics.CastRay(position2, translation, m_tmpHitList, 16);
					foreach (MyPhysics.HitInfo tmpHit2 in m_tmpHitList)
					{
						if (tmpHit2.HkHitInfo.GetHitEntity() != this)
						{
							flag2 = false;
							break;
						}
					}
					m_tmpHitList.Clear();
				}
				if (flag2)
				{
					float num4 = MathHelper.Lerp(1f, 9f, num3 / 200f);
					MyStringId iD_WEAPON_LASER_IGNORE_DEPTH2 = ID_WEAPON_LASER_IGNORE_DEPTH;
					float thickness2 = 0.02f * num4;
					Vector4 color2 = Color.Green.ToVector4();
					MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix2.Up * 0.5 * num4, iD_WEAPON_LASER_IGNORE_DEPTH2, ref color2, thickness2);
					color2 = Color.Blue.ToVector4();
					MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix2.Forward * 0.5 * num4, iD_WEAPON_LASER_IGNORE_DEPTH2, ref color2, thickness2);
					color2 = Color.Red.ToVector4();
					MySimpleObjectDraw.DrawLine(translation, translation + worldMatrix2.Right * 0.5 * num4, iD_WEAPON_LASER_IGNORE_DEPTH2, ref color2, thickness2);
					MyTransparentGeometry.AddBillboardOriented(ID_RED_DOT_IGNORE_DEPTH, Color.White.ToVector4(), translation, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, 0.1f * num4);
					MyRenderProxy.DebugDrawAxis(worldMatrix2, 0.5f, depthRead: false);
				}
			}
			if (MyFakes.ENABLE_ATMOSPHERIC_ENTRYEFFECT)
			{
				DrawAtmosphericEntryEffect();
			}
			if (m_grid.MarkedAsTrash)
			{
				BoundingBoxD localbox = m_grid.PositionComp.LocalAABB;
				localbox.Max += 0.2f;
				localbox.Min -= 0.20000000298023224;
				MatrixD worldMatrix3 = m_grid.PositionComp.WorldMatrix;
				Color color3 = Color.Red;
				color3.A = (byte)(100.0 * (Math.Sin((float)m_grid.TrashHighlightCounter / 10f) + 1.0) / 2.0 + 100.0);
				color3.R = (byte)(200.0 * (Math.Sin((float)m_grid.TrashHighlightCounter / 10f) + 1.0) / 2.0 + 50.0);
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix3, ref localbox, ref color3, ref color3, MySimpleObjectRasterizer.SolidAndWireframe, 1, 0.008f, null, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
			}
		}

		private void DrawAtmosphericEntryEffect()
		{
			float num = MyGravityProviderSystem.CalculateNaturalGravityInPoint(m_grid.PositionComp.GetPosition()).Length();
			bool num2 = m_grid.Physics == null;
			bool flag = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastVoxelContactTime < 5000;
			bool flag2 = num <= 0f;
			bool flag3 = !num2 && m_grid.Physics.LinearVelocity.Length() < 75f;
			if (num2 || flag || flag2 || flag3)
			{
				if (m_atmosphericEffect != null)
				{
					MyParticlesManager.RemoveParticleEffect(m_atmosphericEffect);
					m_atmosphericEffect = null;
				}
				return;
			}
			Vector3 linearVelocity = m_grid.Physics.LinearVelocity;
			Vector3 vector = Vector3.Normalize(linearVelocity);
			float num3 = linearVelocity.Length();
			BoundingBox boundingBox = (BoundingBox)m_grid.PositionComp.WorldAABB;
			Vector3 center = boundingBox.Center;
			Vector3 position = default(Vector3);
			Vector3[] corners = boundingBox.GetCorners();
			foreach (Vector3 vector2 in corners)
			{
				if ((vector2 - center).Dot(vector) > 0.01f)
				{
					m_tmpCornerList.Add(vector2);
					position += vector2;
					if (m_tmpCornerList.Count == 4)
					{
						break;
					}
				}
			}
			if (m_tmpCornerList.Count > 0)
			{
				position /= (float)m_tmpCornerList.Count;
			}
			Plane plane = new Plane(position, -vector);
			m_tmpCornerList.Clear();
			Vector3D centerOfMassWorld = m_grid.Physics.CenterOfMassWorld;
			m_lastWorkingIntersectDistance = (new Ray(centerOfMassWorld, vector).Intersects(plane) ?? m_lastWorkingIntersectDistance);
			Vector3D v = centerOfMassWorld + 0.875f * vector * m_lastWorkingIntersectDistance;
			Matrix identity = Matrix.Identity;
			identity.Translation = v;
			identity.Forward = vector;
			Vector3 direction = Vector3.Transform(vector, Quaternion.CreateFromAxisAngle(m_grid.PositionComp.WorldMatrix.Left, MathF.PI / 2f));
			identity.Up = Vector3.Normalize(Vector3.Reject(m_grid.PositionComp.WorldMatrix.Left, direction));
			identity.Left = identity.Up.Cross(identity.Forward);
			float num4 = MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(m_grid.PositionComp.GetPosition());
			if (m_atmosphericEffect != null || MyParticlesManager.TryCreateParticleEffect("Dummy", identity, out m_atmosphericEffect))
			{
				m_atmosphericEffect.UserScale = boundingBox.ProjectedArea(vector) / (float)Math.Pow(38.0 * (double)m_grid.GridSize, 2.0);
				m_atmosphericEffect.UserAxisScale = Vector3.Normalize(new Vector3(1f, 1f, 1f + 1.5f * (m_grid.Physics.LinearVelocity.Length() - 75f) / (MyGridPhysics.ShipMaxLinearVelocity() - 75f)));
				m_atmosphericEffect.UserColorMultiplier = new Vector4(MathHelper.Clamp((num3 - 75f) / 37.5f * (float)Math.Pow(num4, 1.5), 0f, 0.85f));
			}
		}

		public void ResetLastVoxelContactTimer()
		{
			m_lastVoxelContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public override void AddRenderObjects()
		{
			MyCubeGrid myCubeGrid = base.Container.Entity as MyCubeGrid;
			if (m_renderObjectIDs[0] == uint.MaxValue && (myCubeGrid.IsDirty() || m_renderData.HasDirtyCells))
			{
				myCubeGrid.UpdateInstanceData();
			}
		}

		public override void RemoveRenderObjects()
		{
			if (m_deferRenderRelease)
			{
				m_shouldReleaseRenderObjects = true;
				return;
			}
			m_shouldReleaseRenderObjects = false;
			m_renderData.OnRemovedFromRender();
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (m_renderObjectIDs[i] != uint.MaxValue)
				{
					ReleaseRenderObjectID(i);
				}
			}
		}

		protected override void UpdateRenderObjectVisibility(bool visible)
		{
			base.UpdateRenderObjectVisibility(visible);
		}

		public void UpdateRenderObjectMatrices(Matrix matrix)
		{
			for (int i = 0; i < m_renderObjectIDs.Length; i++)
			{
				if (m_renderObjectIDs[i] != uint.MaxValue)
				{
					MyRenderProxy.UpdateRenderObject(base.RenderObjectIDs[i], matrix, null, LastMomentUpdateIndex);
				}
			}
		}
	}
}
