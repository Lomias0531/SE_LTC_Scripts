using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentBuilder(typeof(MyObjectBuilder_FractureComponentCubeBlock), true)]
	public class MyFractureComponentCubeBlock : MyFractureComponentBase
	{
		private class MyFractureComponentBlockDebugRender : MyDebugRenderComponentBase
		{
			private MyCubeBlock m_block;

			public MyFractureComponentBlockDebugRender(MyCubeBlock b)
			{
				m_block = b;
			}

			public override void DebugDraw()
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS && m_block.Components.Has<MyFractureComponentBase>())
				{
					MyFractureComponentCubeBlock fractureComponent = m_block.GetFractureComponent();
					if (fractureComponent != null)
					{
						MatrixD worldMatrix = m_block.CubeGrid.PositionComp.WorldMatrix;
						worldMatrix.Translation = m_block.CubeGrid.GridIntegerToWorld(m_block.Position);
						MyCubeBuilder.DrawMountPoints(m_block.CubeGrid.GridSize, m_block.BlockDefinition, worldMatrix, fractureComponent.MountPoints.ToArray());
					}
				}
			}

			public override void DebugDrawInvalidTriangles()
			{
			}
		}

		private class Sandbox_Game_EntityComponents_MyFractureComponentCubeBlock_003C_003EActor : IActivator, IActivator<MyFractureComponentCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyFractureComponentCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFractureComponentCubeBlock CreateInstance()
			{
				return new MyFractureComponentCubeBlock();
			}

			MyFractureComponentCubeBlock IActivator<MyFractureComponentCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly List<MyObjectBuilder_FractureComponentBase.FracturedShape> m_tmpShapeListInit = new List<MyObjectBuilder_FractureComponentBase.FracturedShape>();

		private MyObjectBuilder_ComponentBase m_obFracture;

		public MySlimBlock Block
		{
			get;
			private set;
		}

		public MyCubeBlockDefinition.MountPoint[] MountPoints
		{
			get;
			private set;
		}

		public override MyPhysicalModelDefinition PhysicalModelDefinition => Block.BlockDefinition;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			Block = (base.Entity as MyCubeBlock).SlimBlock;
			Block.FatBlock.CheckConnectionAllowed = true;
			MySlimBlock cubeBlock = Block.CubeGrid.GetCubeBlock(Block.Position);
			if (cubeBlock != null)
			{
				cubeBlock.FatBlock.CheckConnectionAllowed = true;
			}
			if (m_obFracture != null)
			{
				Init(m_obFracture);
				m_obFracture = null;
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			Block.FatBlock.CheckConnectionAllowed = false;
			MySlimBlock cubeBlock = Block.CubeGrid.GetCubeBlock(Block.Position);
			if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
			{
				bool flag = false;
				foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					flag |= block.FatBlock.CheckConnectionAllowed;
				}
				if (!flag)
				{
					cubeBlock.FatBlock.CheckConnectionAllowed = false;
				}
			}
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_FractureComponentCubeBlock myObjectBuilder_FractureComponentCubeBlock = base.Serialize() as MyObjectBuilder_FractureComponentCubeBlock;
			SerializeInternal(myObjectBuilder_FractureComponentCubeBlock);
			return myObjectBuilder_FractureComponentCubeBlock;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			if (Block != null)
			{
				Init(builder);
			}
			else
			{
				m_obFracture = builder;
			}
		}

		public override void SetShape(HkdBreakableShape shape, bool compound)
		{
			base.SetShape(shape, compound);
			CreateMountPoints();
			MySlimBlock cubeBlock = Block.CubeGrid.GetCubeBlock(Block.Position);
			cubeBlock?.CubeGrid.UpdateBlockNeighbours(cubeBlock);
			if (Block.CubeGrid.Physics != null)
			{
				Block.CubeGrid.Physics.AddDirtyBlock(Block);
			}
		}

		public override bool RemoveChildShapes(IEnumerable<string> shapeNames)
		{
			base.RemoveChildShapes(shapeNames);
			if (!Shape.IsValid() || Shape.GetChildrenCount() == 0)
			{
				MountPoints = Array.Empty<MyCubeBlockDefinition.MountPoint>();
				if (Sync.IsServer)
				{
					return true;
				}
				Block.FatBlock.Components.Remove<MyFractureComponentBase>();
			}
			return false;
		}

		private void Init(MyObjectBuilder_ComponentBase builder)
		{
			MyObjectBuilder_FractureComponentCubeBlock myObjectBuilder_FractureComponentCubeBlock = builder as MyObjectBuilder_FractureComponentCubeBlock;
			if (myObjectBuilder_FractureComponentCubeBlock.Shapes.Count == 0)
			{
				throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed. Block definition: " + Block.BlockDefinition.Id.ToString());
			}
			RecreateShape(myObjectBuilder_FractureComponentCubeBlock.Shapes);
		}

		public void OnCubeGridChanged()
		{
			m_tmpShapeList.Clear();
			GetCurrentFracturedShapeList(m_tmpShapeList);
			RecreateShape(m_tmpShapeList);
			m_tmpShapeList.Clear();
		}

		protected override void RecreateShape(List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList)
		{
			if (Shape.IsValid())
			{
				Shape.RemoveReference();
				Shape = new HkdBreakableShape();
			}
			MyRenderComponentFracturedPiece myRenderComponentFracturedPiece = Block.FatBlock.Render as MyRenderComponentFracturedPiece;
			if (myRenderComponentFracturedPiece != null)
			{
				myRenderComponentFracturedPiece.ClearModels();
				myRenderComponentFracturedPiece.UpdateRenderObject(visible: false);
			}
			if (shapeList.Count == 0)
			{
				return;
			}
			List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
			MyCubeBlockDefinition blockDefinition = Block.BlockDefinition;
			string model = blockDefinition.Model;
			if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
			{
				MyDestructionData.Static.LoadModelDestruction(model, blockDefinition, Vector3.One);
			}
			HkdBreakableShape hkdBreakableShape = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
			HkdShapeInstanceInfo item = new HkdShapeInstanceInfo(hkdBreakableShape, null, null);
			list.Add(item);
			m_tmpChildren.Add(item);
			hkdBreakableShape.GetChildren(m_tmpChildren);
			if (blockDefinition.BuildProgressModels != null)
			{
				MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = blockDefinition.BuildProgressModels;
				for (int i = 0; i < buildProgressModels.Length; i++)
				{
					model = buildProgressModels[i].File;
					if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
					{
						MyDestructionData.Static.LoadModelDestruction(model, blockDefinition, Vector3.One);
					}
					hkdBreakableShape = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
					item = new HkdShapeInstanceInfo(hkdBreakableShape, null, null);
					list.Add(item);
					m_tmpChildren.Add(item);
					hkdBreakableShape.GetChildren(m_tmpChildren);
				}
			}
			m_tmpShapeListInit.Clear();
			m_tmpShapeListInit.AddRange(shapeList);
			for (int j = 0; j < m_tmpChildren.Count; j++)
			{
				HkdShapeInstanceInfo child = m_tmpChildren[j];
				IEnumerable<MyObjectBuilder_FractureComponentBase.FracturedShape> source = m_tmpShapeListInit.Where((MyObjectBuilder_FractureComponentBase.FracturedShape s) => s.Name == child.ShapeName);
				if (source.Count() > 0)
				{
					MyObjectBuilder_FractureComponentBase.FracturedShape item2 = source.First();
					HkdShapeInstanceInfo item3 = new HkdShapeInstanceInfo(child.Shape.Clone(), Matrix.Identity);
					if (item2.Fixed)
					{
						item3.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
					}
					list.Add(item3);
					m_tmpShapeInfos.Add(item3);
					m_tmpShapeListInit.Remove(item2);
				}
				else
				{
					child.GetChildren(m_tmpChildren);
				}
			}
			m_tmpShapeListInit.Clear();
			if (shapeList.Count > 0 && m_tmpShapeInfos.Count == 0)
			{
				m_tmpChildren.Clear();
				throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed. Block definition: " + Block.BlockDefinition.Id.ToString());
			}
			if (myRenderComponentFracturedPiece != null)
			{
				foreach (HkdShapeInstanceInfo tmpShapeInfo in m_tmpShapeInfos)
				{
					if (!string.IsNullOrEmpty(tmpShapeInfo.Shape.Name))
					{
						myRenderComponentFracturedPiece.AddPiece(tmpShapeInfo.Shape.Name, Matrix.Identity);
					}
				}
				myRenderComponentFracturedPiece.UpdateRenderObject(visible: true);
			}
			m_tmpChildren.Clear();
			if (Block.CubeGrid.CreatePhysics)
			{
				HkdBreakableShape hkdBreakableShape2 = new HkdCompoundBreakableShape(null, m_tmpShapeInfos);
				((HkdCompoundBreakableShape)hkdBreakableShape2).RecalcMassPropsFromChildren();
				HkMassProperties massProperties = default(HkMassProperties);
				hkdBreakableShape2.BuildMassProperties(ref massProperties);
				Shape = new HkdBreakableShape(hkdBreakableShape2.GetShape(), ref massProperties);
				hkdBreakableShape2.RemoveReference();
				foreach (HkdShapeInstanceInfo tmpShapeInfo2 in m_tmpShapeInfos)
				{
					HkdShapeInstanceInfo shapeInfo = tmpShapeInfo2;
					Shape.AddShape(ref shapeInfo);
				}
				Shape.SetStrenght(MyDestructionConstants.STRENGTH);
				CreateMountPoints();
				MySlimBlock cubeBlock = Block.CubeGrid.GetCubeBlock(Block.Position);
				cubeBlock?.CubeGrid.UpdateBlockNeighbours(cubeBlock);
				if (Block.CubeGrid.Physics != null)
				{
					Block.CubeGrid.Physics.AddDirtyBlock(Block);
				}
			}
			foreach (HkdShapeInstanceInfo tmpShapeInfo3 in m_tmpShapeInfos)
			{
				tmpShapeInfo3.Shape.RemoveReference();
			}
			m_tmpShapeInfos.Clear();
			foreach (HkdShapeInstanceInfo item4 in list)
			{
				item4.RemoveReference();
			}
		}

		private void CreateMountPoints()
		{
			if (MyFakes.FRACTURED_BLOCK_AABB_MOUNT_POINTS)
			{
				List<MyCubeBlockDefinition.MountPoint> list = new List<MyCubeBlockDefinition.MountPoint>();
				MyCubeBlockDefinition blockDefinition = Block.BlockDefinition;
				Vector3 value = new Vector3(blockDefinition.Size);
				BoundingBox blockBB = new BoundingBox(-value / 2f, value / 2f);
				Vector3 halfExtents = blockBB.HalfExtents;
				blockBB.Min += halfExtents;
				blockBB.Max += halfExtents;
				Shape.GetChildren(m_tmpChildren);
				if (m_tmpChildren.Count > 0)
				{
					foreach (HkdShapeInstanceInfo tmpChild in m_tmpChildren)
					{
						AddMountForShape(tmpChild.Shape, Matrix.Identity, ref blockBB, Block.CubeGrid.GridSize, list);
					}
				}
				else
				{
					AddMountForShape(Shape, Matrix.Identity, ref blockBB, Block.CubeGrid.GridSize, list);
				}
				MountPoints = list.ToArray();
				m_tmpChildren.Clear();
			}
			else
			{
				MountPoints = MyCubeBuilder.AutogenerateMountpoints(new HkShape[1]
				{
					Shape.GetShape()
				}, Block.CubeGrid.GridSize);
			}
		}

		public static HkdBreakableShape AddMountForShape(HkdBreakableShape shape, Matrix transform, ref BoundingBox blockBB, float gridSize, List<MyCubeBlockDefinition.MountPoint> outMountPoints)
		{
			shape.GetShape().GetLocalAABB(0.01f, out Vector4 min, out Vector4 max);
			BoundingBox box = new BoundingBox(new Vector3(min), new Vector3(max));
			box = box.Transform(transform);
			box.Min /= gridSize;
			box.Max /= gridSize;
			box.Inflate(0.04f);
			box.Min += blockBB.HalfExtents;
			box.Max += blockBB.HalfExtents;
			if (blockBB.Contains(box) == ContainmentType.Intersects)
			{
				box.Inflate(-0.04f);
				Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
				for (int i = 0; i < enumDirections.Length; i++)
				{
					int num = (int)enumDirections[i];
					Vector3 vector = Base6Directions.Directions[num];
					Vector3 vector2 = Vector3.Abs(vector);
					MyCubeBlockDefinition.MountPoint item = default(MyCubeBlockDefinition.MountPoint);
					item.Start = box.Min;
					item.End = box.Max;
					item.Enabled = true;
					Vector3 vector3 = item.Start * vector2 / (blockBB.HalfExtents * 2f) - vector2 * 0.04f;
					Vector3 vector4 = item.End * vector2 / (blockBB.HalfExtents * 2f) + vector2 * 0.04f;
					bool flag = false;
					bool flag2 = false;
					if (vector3.Max() < 1f && vector4.Max() > 1f && vector.Max() > 0f)
					{
						flag = true;
						flag2 = true;
					}
					else if (vector3.Min() < 0f && vector4.Max() > 0f && vector.Min() < 0f)
					{
						flag = true;
					}
					if (flag)
					{
						item.Start -= item.Start * vector2 - vector2 * 0.04f;
						item.End -= item.End * vector2 + vector2 * 0.04f;
						if (flag2)
						{
							item.Start += vector2 * blockBB.HalfExtents * 2f;
							item.End += vector2 * blockBB.HalfExtents * 2f;
						}
						item.Start -= blockBB.HalfExtents - Vector3.One / 2f;
						item.End -= blockBB.HalfExtents - Vector3.One / 2f;
						item.Normal = new Vector3I(vector);
						outMountPoints.Add(item);
					}
				}
			}
			return shape;
		}

		public float GetIntegrityRatioFromFracturedPieceCounts()
		{
			if (Shape.IsValid() && Block != null)
			{
				int totalBreakableShapeChildrenCount = Block.GetTotalBreakableShapeChildrenCount();
				if (totalBreakableShapeChildrenCount > 0)
				{
					int totalChildrenCount = Shape.GetTotalChildrenCount();
					if (totalChildrenCount <= totalBreakableShapeChildrenCount)
					{
						return (float)totalChildrenCount / (float)totalBreakableShapeChildrenCount;
					}
				}
			}
			return 0f;
		}
	}
}
