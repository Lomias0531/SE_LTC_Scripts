using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	public class MyDebugRenderComponentCubeGrid : MyDebugRenderComponent
	{
		private MyCubeGrid m_cubeGrid;

		private Dictionary<Vector3I, MyTimeSpan> m_dirtyBlocks = new Dictionary<Vector3I, MyTimeSpan>();

		private List<Vector3I> m_tmpRemoveList = new List<Vector3I>();

		private List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();

		public MyDebugRenderComponentCubeGrid(MyCubeGrid cubeGrid)
			: base(cubeGrid)
		{
			m_cubeGrid = cubeGrid;
		}

		public override void PrepareForDraw()
		{
			base.PrepareForDraw();
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)
			{
				MyTimeSpan b = MyTimeSpan.FromMilliseconds(1500.0);
				using (m_tmpRemoveList.GetClearToken())
				{
					foreach (KeyValuePair<Vector3I, MyTimeSpan> dirtyBlock in m_dirtyBlocks)
					{
						if (MySandboxGame.Static.TotalTime - dirtyBlock.Value > b)
						{
							m_tmpRemoveList.Add(dirtyBlock.Key);
						}
					}
					foreach (Vector3I tmpRemove in m_tmpRemoveList)
					{
						m_dirtyBlocks.Remove(tmpRemove);
					}
				}
				foreach (Vector3I dirtyBlock2 in m_cubeGrid.DirtyBlocks)
				{
					m_dirtyBlocks[dirtyBlock2] = MySandboxGame.Static.TotalTime;
				}
			}
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_AABB)
			{
				BoundingBox localAABB = m_cubeGrid.PositionComp.LocalAABB;
				MatrixD worldMatrix = m_cubeGrid.PositionComp.WorldMatrix;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(localAABB, worldMatrix);
				Color yellow = Color.Yellow;
				MyRenderProxy.DebugDrawOBB(obb, yellow, 0.2f, depthRead: false, smooth: true);
				MyRenderProxy.DebugDrawAxis(worldMatrix, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_FIXED_BLOCK_QUERIES)
			{
				foreach (MySlimBlock block in m_cubeGrid.GetBlocks())
				{
					BoundingBox geometryLocalBox = block.FatBlock.GetGeometryLocalBox();
					Vector3 halfExtents = geometryLocalBox.Size / 2f;
					block.ComputeScaledCenter(out Vector3D scaledCenter);
					scaledCenter += geometryLocalBox.Center;
					scaledCenter = Vector3D.Transform(scaledCenter, m_cubeGrid.WorldMatrix);
					block.Orientation.GetMatrix(out Matrix result);
					Quaternion rotation = Quaternion.CreateFromRotationMatrix(result * m_cubeGrid.WorldMatrix.GetOrientation());
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref scaledCenter, ref rotation, m_penetrations, 14);
					bool flag = false;
					foreach (HkBodyCollision penetration in m_penetrations)
					{
						IMyEntity collisionEntity = penetration.GetCollisionEntity();
						if (collisionEntity != null && collisionEntity is MyVoxelMap)
						{
							flag = true;
							break;
						}
					}
					m_penetrations.Clear();
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(scaledCenter, halfExtents, rotation), flag ? Color.Green : Color.Red, 0.1f, depthRead: false, smooth: false);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES || MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)
			{
				string text = "";
				Color color = Color.White;
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES)
				{
					text = text + m_cubeGrid.ToString() + " ";
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)
				{
					MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(m_cubeGrid);
					if (controllingPlayer != null)
					{
						text = text + "Controlled by: " + controllingPlayer.DisplayName;
						color = Color.LightGreen;
					}
				}
				MyRenderProxy.DebugDrawText3D(m_cubeGrid.PositionComp.WorldAABB.Center, text, color, 0.7f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
			_ = m_cubeGrid.Render;
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)
			{
				Vector3D translation = m_cubeGrid.PositionComp.WorldMatrix.Translation;
				foreach (MyBlockGroup blockGroup in m_cubeGrid.BlockGroups)
				{
					MyRenderProxy.DebugDrawText3D(translation, blockGroup.Name.ToString(), Color.Red, 1f, depthRead: false);
					translation += m_cubeGrid.PositionComp.WorldMatrix.Right * blockGroup.Name.Length * 0.10000000149011612;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)
			{
				foreach (KeyValuePair<Vector3I, MyTimeSpan> dirtyBlock in m_dirtyBlocks)
				{
					Vector3 v = (m_cubeGrid.GetCubeBlock(dirtyBlock.Key) != null) ? Color.Red.ToVector3() : Color.Yellow.ToVector3();
					MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(m_cubeGrid.GridSize) * Matrix.CreateTranslation(dirtyBlock.Key * m_cubeGrid.GridSize) * m_cubeGrid.WorldMatrix, v, 0.15f, depthRead: false, smooth: true);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES)
			{
				_ = MySector.MainCamera.Position;
				foreach (MySlimBlock cubeBlock in m_cubeGrid.CubeBlocks)
				{
					if (m_cubeGrid.TryGetCube(cubeBlock.Position, out MyCube cube))
					{
						int num = 0;
						MyCubePart[] parts = cube.Parts;
						foreach (MyCubePart myCubePart in parts)
						{
							if (myCubePart.Model.BoneMapping != null && num == MyPetaInputComponent.DEBUG_INDEX)
							{
								for (int j = 0; j < Math.Min(myCubePart.Model.BoneMapping.Length, 9); j++)
								{
									Vector3I a = myCubePart.Model.BoneMapping[j];
									Matrix orientation = myCubePart.InstanceData.LocalMatrix.GetOrientation();
									Vector3 value = a * 1f - Vector3.One;
									Vector3I a2 = Vector3I.Round(Vector3.Transform(value * 1f, orientation));
									Vector3I bonePos = Vector3I.Round(Vector3.Transform(value * 1f, orientation) + Vector3.One);
									Vector3 position = m_cubeGrid.GridSize * (cubeBlock.Position + a2 / 2f);
									Vector3 bone = m_cubeGrid.Skeleton.GetBone(cubeBlock.Position, bonePos);
									Vector3D vector3D = Vector3D.Transform(position, m_cubeGrid.PositionComp.WorldMatrix);
									Vector3D value2 = Vector3D.TransformNormal(bone, m_cubeGrid.PositionComp.WorldMatrix);
									MyRenderProxy.DebugDrawSphere(vector3D, 0.025f, Color.Green, 0.5f, depthRead: false, smooth: true);
									MyRenderProxy.DebugDrawText3D(vector3D, j.ToString(), Color.Green, 0.5f, depthRead: false);
									MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + value2, Color.Red);
								}
							}
							num++;
						}
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CUBES)
			{
				foreach (MySlimBlock cubeBlock2 in m_cubeGrid.CubeBlocks)
				{
					cubeBlock2.GetLocalMatrix(out Matrix localMatrix);
					MyRenderProxy.DebugDrawAxis(localMatrix * m_cubeGrid.WorldMatrix, 1f, depthRead: false);
					cubeBlock2.FatBlock?.DebugDraw();
				}
			}
			m_cubeGrid.GridSystems.DebugDraw();
			_ = MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS;
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_ORIGINS)
			{
				MyRenderProxy.DebugDrawAxis(m_cubeGrid.PositionComp.WorldMatrix, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_ALL)
			{
				foreach (MySlimBlock block2 in m_cubeGrid.GetBlocks())
				{
					if ((m_cubeGrid.GridIntegerToWorld(block2.Position) - MySector.MainCamera.Position).LengthSquared() < 200.0)
					{
						DebugDrawMountPoints(block2);
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_INTEGRITY && MySector.MainCamera != null && (MySector.MainCamera.Position - m_cubeGrid.PositionComp.WorldVolume.Center).Length() < 16.0 + m_cubeGrid.PositionComp.WorldVolume.Radius)
			{
				foreach (MySlimBlock cubeBlock3 in m_cubeGrid.CubeBlocks)
				{
					Vector3D value3 = m_cubeGrid.GridIntegerToWorld(cubeBlock3.Position);
					if (m_cubeGrid.GridSizeEnum == MyCubeSize.Large || (MySector.MainCamera != null && (MySector.MainCamera.Position - value3).LengthSquared() < 9.0))
					{
						float num2 = 0f;
						if (cubeBlock3.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block3 in (cubeBlock3.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								num2 += block3.Integrity * block3.BlockDefinition.MaxIntegrityRatio;
							}
						}
						else
						{
							num2 = cubeBlock3.Integrity * cubeBlock3.BlockDefinition.MaxIntegrityRatio;
						}
						MyRenderProxy.DebugDrawText3D(m_cubeGrid.GridIntegerToWorld(cubeBlock3.Position), ((int)num2).ToString(), Color.White, (m_cubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.65f : 0.5f, depthRead: false);
					}
				}
			}
			base.DebugDraw();
		}

		private void DebugDrawMountPoints(MySlimBlock block)
		{
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					DebugDrawMountPoints(block2);
				}
				return;
			}
			block.GetLocalMatrix(out Matrix localMatrix);
			MatrixD drawMatrix = localMatrix * m_cubeGrid.WorldMatrix;
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(block.BlockDefinition.Id, out MyCubeBlockDefinition blockDefinition);
			if (MyFakes.ENABLE_FRACTURE_COMPONENT && block.FatBlock != null && block.FatBlock.Components.Has<MyFractureComponentBase>())
			{
				MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
				if (fractureComponent != null)
				{
					MyCubeBuilder.DrawMountPoints(m_cubeGrid.GridSize, blockDefinition, drawMatrix, fractureComponent.MountPoints.ToArray());
				}
			}
			else
			{
				MyCubeBuilder.DrawMountPoints(m_cubeGrid.GridSize, blockDefinition, ref drawMatrix);
			}
		}

		public override void DebugDrawInvalidTriangles()
		{
			base.DebugDrawInvalidTriangles();
			foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in m_cubeGrid.Render.RenderData.Cells)
			{
				foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
				{
					MyModel model = cubePart.Key.Model;
					if (model != null)
					{
						int trianglesCount = model.GetTrianglesCount();
						for (int i = 0; i < trianglesCount; i++)
						{
							MyTriangleVertexIndices triangle = model.GetTriangle(i);
							if (MyUtils.IsWrongTriangle(model.GetVertex(triangle.I0), model.GetVertex(triangle.I1), model.GetVertex(triangle.I2)))
							{
								Vector3 vector = Vector3.Transform(model.GetVertex(triangle.I0), (Matrix)m_cubeGrid.PositionComp.WorldMatrix);
								Vector3 vector2 = Vector3.Transform(model.GetVertex(triangle.I1), (Matrix)m_cubeGrid.PositionComp.WorldMatrix);
								Vector3 vector3 = Vector3.Transform(model.GetVertex(triangle.I2), (Matrix)m_cubeGrid.PositionComp.WorldMatrix);
								MyRenderProxy.DebugDrawLine3D(vector, vector2, Color.Purple, Color.Purple, depthRead: false);
								MyRenderProxy.DebugDrawLine3D(vector2, vector3, Color.Purple, Color.Purple, depthRead: false);
								MyRenderProxy.DebugDrawLine3D(vector3, vector, Color.Purple, Color.Purple, depthRead: false);
								Vector3 vector4 = (vector + vector2 + vector3) / 3f;
								MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitX, Color.Yellow, Color.Yellow, depthRead: false);
								MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitY, Color.Yellow, Color.Yellow, depthRead: false);
								MyRenderProxy.DebugDrawLine3D(vector4, vector4 + Vector3.UnitZ, Color.Yellow, Color.Yellow, depthRead: false);
							}
						}
					}
				}
			}
		}
	}
}
