using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_FracturedBlock))]
	public class MyFracturedBlock : MyCubeBlock
	{
		public class MultiBlockPartInfo
		{
			public MyDefinitionId MultiBlockDefinition;

			public int MultiBlockId;
		}

		public struct Info
		{
			public HkdBreakableShape Shape;

			public Vector3I Position;

			public bool Compound;

			public List<MyDefinitionId> OriginalBlocks;

			public List<MyBlockOrientation> Orientations;

			public List<MultiBlockPartInfo> MultiBlocks;
		}

		private class MyFBDebugRender : MyDebugRenderComponentBase
		{
			private MyFracturedBlock m_block;

			public MyFBDebugRender(MyFracturedBlock b)
			{
				m_block = b;
			}

			public override void DebugDraw()
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS && m_block.MountPoints != null)
				{
					MatrixD worldMatrix = m_block.CubeGrid.PositionComp.WorldMatrix;
					worldMatrix.Translation = m_block.CubeGrid.GridIntegerToWorld(m_block.Position);
					MyCubeBuilder.DrawMountPoints(m_block.CubeGrid.GridSize, m_block.BlockDefinition, worldMatrix, m_block.MountPoints.ToArray());
				}
			}

			public override void DebugDrawInvalidTriangles()
			{
			}
		}

		private class Sandbox_Game_Entities_Cube_MyFracturedBlock_003C_003EActor : IActivator, IActivator<MyFracturedBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyFracturedBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFracturedBlock CreateInstance()
			{
				return new MyFracturedBlock();
			}

			MyFracturedBlock IActivator<MyFracturedBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static List<HkdShapeInstanceInfo> m_children = new List<HkdShapeInstanceInfo>();

		private static List<HkdShapeInstanceInfo> m_shapeInfos = new List<HkdShapeInstanceInfo>();

		private static HashSet<Tuple<string, float>> m_tmpNamesAndBuildProgress = new HashSet<Tuple<string, float>>();

		public HkdBreakableShape Shape;

		public List<MyDefinitionId> OriginalBlocks;

		public List<MyBlockOrientation> Orientations;

		public List<MultiBlockPartInfo> MultiBlocks;

		private List<MyObjectBuilder_FracturedBlock.ShapeB> m_shapes = new List<MyObjectBuilder_FracturedBlock.ShapeB>();

		private List<MyCubeBlockDefinition.MountPoint> m_mpCache = new List<MyCubeBlockDefinition.MountPoint>();

		private new MyRenderComponentFracturedPiece Render
		{
			get
			{
				return (MyRenderComponentFracturedPiece)base.Render;
			}
			set
			{
				base.Render = value;
			}
		}

		public List<MyCubeBlockDefinition.MountPoint> MountPoints
		{
			get;
			private set;
		}

		public MyFracturedBlock()
		{
			base.EntityId = MyEntityIdentifier.AllocateId();
			Render = new MyRenderComponentFracturedPiece();
			Render.NeedsDraw = true;
			Render.PersistentFlags = MyPersistentEntityFlags2.Enabled;
			base.CheckConnectionAllowed = true;
			AddDebugRenderComponent(new MyFBDebugRender(this));
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_FracturedBlock myObjectBuilder_FracturedBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_FracturedBlock;
			MyObjectBuilder_FracturedBlock.ShapeB shapeB;
			if (string.IsNullOrEmpty(Shape.Name) || Shape.IsCompound())
			{
				Shape.GetChildren(m_children);
				foreach (HkdShapeInstanceInfo child in m_children)
				{
					shapeB = default(MyObjectBuilder_FracturedBlock.ShapeB);
					shapeB.Name = child.ShapeName;
					shapeB.Orientation = Quaternion.CreateFromRotationMatrix(child.GetTransform().GetOrientation());
					shapeB.Fixed = MyDestructionHelper.IsFixed(child.Shape);
					MyObjectBuilder_FracturedBlock.ShapeB item = shapeB;
					myObjectBuilder_FracturedBlock.Shapes.Add(item);
				}
				m_children.Clear();
			}
			else
			{
				List<MyObjectBuilder_FracturedBlock.ShapeB> shapes = myObjectBuilder_FracturedBlock.Shapes;
				shapeB = new MyObjectBuilder_FracturedBlock.ShapeB
				{
					Name = Shape.Name
				};
				shapes.Add(shapeB);
			}
			foreach (MyDefinitionId originalBlock in OriginalBlocks)
			{
				myObjectBuilder_FracturedBlock.BlockDefinitions.Add(originalBlock);
			}
			foreach (MyBlockOrientation orientation in Orientations)
			{
				myObjectBuilder_FracturedBlock.BlockOrientations.Add(orientation);
			}
			if (MultiBlocks != null)
			{
				foreach (MultiBlockPartInfo multiBlock in MultiBlocks)
				{
					if (multiBlock != null)
					{
						myObjectBuilder_FracturedBlock.MultiBlocks.Add(new MyObjectBuilder_FracturedBlock.MyMultiBlockPart
						{
							MultiBlockDefinition = multiBlock.MultiBlockDefinition,
							MultiBlockId = multiBlock.MultiBlockId
						});
					}
					else
					{
						myObjectBuilder_FracturedBlock.MultiBlocks.Add(null);
					}
				}
				return myObjectBuilder_FracturedBlock;
			}
			return myObjectBuilder_FracturedBlock;
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			base.CheckConnectionAllowed = true;
			MyObjectBuilder_FracturedBlock myObjectBuilder_FracturedBlock = builder as MyObjectBuilder_FracturedBlock;
			if (myObjectBuilder_FracturedBlock.Shapes.Count == 0)
			{
				if (!myObjectBuilder_FracturedBlock.CreatingFracturedBlock)
				{
					throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed.");
				}
				return;
			}
			OriginalBlocks = new List<MyDefinitionId>();
			Orientations = new List<MyBlockOrientation>();
			List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
			foreach (SerializableDefinitionId blockDefinition in myObjectBuilder_FracturedBlock.BlockDefinitions)
			{
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockDefinition);
				string model = cubeBlockDefinition.Model;
				if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
				{
					MyDestructionData.Static.LoadModelDestruction(model, cubeBlockDefinition, Vector3.One);
				}
				HkdBreakableShape hkdBreakableShape = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
				HkdShapeInstanceInfo item = new HkdShapeInstanceInfo(hkdBreakableShape, null, null);
				list.Add(item);
				m_children.Add(item);
				hkdBreakableShape.GetChildren(m_children);
				if (cubeBlockDefinition.BuildProgressModels != null)
				{
					MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = cubeBlockDefinition.BuildProgressModels;
					for (int i = 0; i < buildProgressModels.Length; i++)
					{
						model = buildProgressModels[i].File;
						if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
						{
							MyDestructionData.Static.LoadModelDestruction(model, cubeBlockDefinition, Vector3.One);
						}
						hkdBreakableShape = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
						item = new HkdShapeInstanceInfo(hkdBreakableShape, null, null);
						list.Add(item);
						m_children.Add(item);
						hkdBreakableShape.GetChildren(m_children);
					}
				}
				OriginalBlocks.Add(blockDefinition);
			}
			foreach (SerializableBlockOrientation blockOrientation in myObjectBuilder_FracturedBlock.BlockOrientations)
			{
				Orientations.Add(blockOrientation);
			}
			if (myObjectBuilder_FracturedBlock.MultiBlocks.Count > 0)
			{
				MultiBlocks = new List<MultiBlockPartInfo>();
				foreach (MyObjectBuilder_FracturedBlock.MyMultiBlockPart multiBlock in myObjectBuilder_FracturedBlock.MultiBlocks)
				{
					if (multiBlock != null)
					{
						MultiBlocks.Add(new MultiBlockPartInfo
						{
							MultiBlockDefinition = multiBlock.MultiBlockDefinition,
							MultiBlockId = multiBlock.MultiBlockId
						});
					}
					else
					{
						MultiBlocks.Add(null);
					}
				}
			}
			m_shapes.AddRange(myObjectBuilder_FracturedBlock.Shapes);
			for (int j = 0; j < m_children.Count; j++)
			{
				HkdShapeInstanceInfo child = m_children[j];
				Func<MyObjectBuilder_FracturedBlock.ShapeB, bool> predicate = (MyObjectBuilder_FracturedBlock.ShapeB s) => s.Name == child.ShapeName;
				IEnumerable<MyObjectBuilder_FracturedBlock.ShapeB> source = m_shapes.Where(predicate);
				if (source.Count() > 0)
				{
					MyObjectBuilder_FracturedBlock.ShapeB item2 = source.First();
					Matrix transform = Matrix.CreateFromQuaternion(item2.Orientation);
					transform.Translation = child.GetTransform().Translation;
					HkdShapeInstanceInfo item3 = new HkdShapeInstanceInfo(child.Shape.Clone(), transform);
					if (item2.Fixed)
					{
						item3.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
					}
					list.Add(item3);
					m_shapeInfos.Add(item3);
					m_shapes.Remove(item2);
				}
				else
				{
					child.GetChildren(m_children);
				}
			}
			if (m_shapeInfos.Count == 0)
			{
				m_children.Clear();
				throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed.");
			}
			foreach (HkdShapeInstanceInfo shapeInfo2 in m_shapeInfos)
			{
				if (!string.IsNullOrEmpty(shapeInfo2.Shape.Name))
				{
					Render.AddPiece(shapeInfo2.Shape.Name, Matrix.CreateFromQuaternion(Quaternion.CreateFromRotationMatrix(shapeInfo2.GetTransform().GetOrientation())));
				}
			}
			if (base.CubeGrid.CreatePhysics)
			{
				HkdBreakableShape hkdBreakableShape2 = new HkdCompoundBreakableShape(null, m_shapeInfos);
				((HkdCompoundBreakableShape)hkdBreakableShape2).RecalcMassPropsFromChildren();
				Shape = hkdBreakableShape2;
				HkMassProperties massProperties = default(HkMassProperties);
				hkdBreakableShape2.BuildMassProperties(ref massProperties);
				Shape = new HkdBreakableShape(hkdBreakableShape2.GetShape(), ref massProperties);
				hkdBreakableShape2.RemoveReference();
				foreach (HkdShapeInstanceInfo shapeInfo3 in m_shapeInfos)
				{
					HkdShapeInstanceInfo shapeInfo = shapeInfo3;
					Shape.AddShape(ref shapeInfo);
				}
				Shape.SetStrenght(MyDestructionConstants.STRENGTH);
				CreateMountPoints();
			}
			m_children.Clear();
			foreach (HkdShapeInstanceInfo shapeInfo4 in m_shapeInfos)
			{
				shapeInfo4.Shape.RemoveReference();
			}
			foreach (HkdShapeInstanceInfo item4 in list)
			{
				item4.RemoveReference();
			}
			m_shapeInfos.Clear();
		}

		public void SetDataFromCompound(HkdBreakableShape compound)
		{
			MyRenderComponentFracturedPiece render = Render;
			if (render != null)
			{
				compound.GetChildren(m_shapeInfos);
				foreach (HkdShapeInstanceInfo shapeInfo in m_shapeInfos)
				{
					if (shapeInfo.IsValid())
					{
						render.AddPiece(shapeInfo.ShapeName, shapeInfo.GetTransform());
					}
				}
				m_shapeInfos.Clear();
			}
		}

		private void AddMeshBuilderRecursively(List<HkdShapeInstanceInfo> children)
		{
			MyRenderComponentFracturedPiece render = Render;
			foreach (HkdShapeInstanceInfo child in children)
			{
				render.AddPiece(child.ShapeName, Matrix.Identity);
			}
		}

		internal void SetDataFromHavok(HkdBreakableShape shape, bool compound)
		{
			Shape = shape;
			if (compound)
			{
				SetDataFromCompound(shape);
			}
			else
			{
				Render.AddPiece(shape.Name, Matrix.Identity);
			}
			CreateMountPoints();
		}

		private void CreateMountPoints()
		{
			if (MyFakes.FRACTURED_BLOCK_AABB_MOUNT_POINTS)
			{
				MountPoints = new List<MyCubeBlockDefinition.MountPoint>();
				BoundingBox blockBB = BoundingBox.CreateInvalid();
				for (int i = 0; i < OriginalBlocks.Count; i++)
				{
					MyDefinitionId id = OriginalBlocks[i];
					Orientations[i].GetMatrix(out Matrix result);
					Vector3 value = new Vector3(MyDefinitionManager.Static.GetCubeBlockDefinition(id).Size);
					blockBB = blockBB.Include(new BoundingBox(-value / 2f, value / 2f).Transform(result));
				}
				Vector3 halfExtents = blockBB.HalfExtents;
				blockBB.Min += halfExtents;
				blockBB.Max += halfExtents;
				Shape.GetChildren(m_children);
				foreach (HkdShapeInstanceInfo child in m_children)
				{
					MyFractureComponentCubeBlock.AddMountForShape(child.Shape, child.GetTransform(), ref blockBB, base.CubeGrid.GridSize, MountPoints);
				}
				if (m_children.Count == 0)
				{
					MyFractureComponentCubeBlock.AddMountForShape(Shape, Matrix.Identity, ref blockBB, base.CubeGrid.GridSize, MountPoints);
				}
				m_children.Clear();
			}
			else
			{
				MountPoints = new List<MyCubeBlockDefinition.MountPoint>(MyCubeBuilder.AutogenerateMountpoints(new HkShape[1]
				{
					Shape.GetShape()
				}, base.CubeGrid.GridSize));
			}
		}

		protected override void Closing()
		{
			if (Shape.IsValid())
			{
				Shape.RemoveReference();
			}
			base.Closing();
		}

		public override bool ConnectionAllowed(ref Vector3I otherBlockPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			if (MountPoints == null)
			{
				return true;
			}
			Vector3I positionB = base.Position + faceNormal;
			MySlimBlock cubeBlock = base.CubeGrid.GetCubeBlock(positionB);
			MyBlockOrientation orientation = cubeBlock?.Orientation ?? MyBlockOrientation.Identity;
			Vector3I positionA = base.Position;
			m_mpCache.Clear();
			if (cubeBlock != null && cubeBlock.FatBlock is MyFracturedBlock)
			{
				m_mpCache.AddRange((cubeBlock.FatBlock as MyFracturedBlock).MountPoints);
			}
			else if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
			{
				List<MyCubeBlockDefinition.MountPoint> list = new List<MyCubeBlockDefinition.MountPoint>();
				foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
					MyCubeGrid.TransformMountPoints(list, block.BlockDefinition, buildProgressModelMountPoints, ref block.Orientation);
					m_mpCache.AddRange(list);
				}
			}
			else if (cubeBlock != null)
			{
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints2 = def.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
				MyCubeGrid.TransformMountPoints(m_mpCache, def, buildProgressModelMountPoints2, ref orientation);
			}
			return MyCubeGrid.CheckMountPointsForSide(MountPoints, ref SlimBlock.Orientation, ref positionA, base.BlockDefinition.Id, ref faceNormal, m_mpCache, ref orientation, ref positionB, def.Id);
		}

		public bool IsMultiBlockPart(MyDefinitionId multiBlockDefinition, int multiblockId)
		{
			if (MultiBlocks == null)
			{
				return false;
			}
			foreach (MultiBlockPartInfo multiBlock in MultiBlocks)
			{
				if (multiBlock != null && multiBlock.MultiBlockDefinition == multiBlockDefinition && multiBlock.MultiBlockId == multiblockId)
				{
					return true;
				}
			}
			return false;
		}

		public MyObjectBuilder_CubeBlock ConvertToOriginalBlocksWithFractureComponent()
		{
			List<MyObjectBuilder_CubeBlock> list = new List<MyObjectBuilder_CubeBlock>();
			for (int i = 0; i < OriginalBlocks.Count; i++)
			{
				MyDefinitionId myDefinitionId = OriginalBlocks[i];
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(myDefinitionId, out MyCubeBlockDefinition blockDefinition);
				if (blockDefinition == null)
				{
					continue;
				}
				MyBlockOrientation blockOrientation = Orientations[i];
				MultiBlockPartInfo multiBlockPartInfo = (MultiBlocks != null && MultiBlocks.Count > i) ? MultiBlocks[i] : null;
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(myDefinitionId) as MyObjectBuilder_CubeBlock;
				blockOrientation.GetQuaternion(out Quaternion result);
				myObjectBuilder_CubeBlock.Orientation = result;
				myObjectBuilder_CubeBlock.Min = base.Position;
				myObjectBuilder_CubeBlock.MultiBlockId = (multiBlockPartInfo?.MultiBlockId ?? 0);
				myObjectBuilder_CubeBlock.MultiBlockDefinition = null;
				if (multiBlockPartInfo != null)
				{
					myObjectBuilder_CubeBlock.MultiBlockDefinition = multiBlockPartInfo.MultiBlockDefinition;
				}
				myObjectBuilder_CubeBlock.ComponentContainer = new MyObjectBuilder_ComponentContainer();
				MyObjectBuilder_FractureComponentCubeBlock myObjectBuilder_FractureComponentCubeBlock = new MyObjectBuilder_FractureComponentCubeBlock();
				m_tmpNamesAndBuildProgress.Clear();
				GetAllBlockBreakableShapeNames(blockDefinition, m_tmpNamesAndBuildProgress);
				ConvertAllShapesToFractureComponentShapeBuilder(Shape, ref Matrix.Identity, blockOrientation, m_tmpNamesAndBuildProgress, myObjectBuilder_FractureComponentCubeBlock, out float buildProgress);
				m_tmpNamesAndBuildProgress.Clear();
				if (myObjectBuilder_FractureComponentCubeBlock.Shapes.Count == 0)
				{
					continue;
				}
				if (blockDefinition.BuildProgressModels != null)
				{
					MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = blockDefinition.BuildProgressModels;
					foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in buildProgressModels)
					{
						if (buildProgressModel.BuildRatioUpperBound >= buildProgress)
						{
							break;
						}
						_ = buildProgressModel.BuildRatioUpperBound;
					}
				}
				MyObjectBuilder_ComponentContainer.ComponentData componentData = new MyObjectBuilder_ComponentContainer.ComponentData();
				componentData.TypeId = typeof(MyFractureComponentBase).Name;
				componentData.Component = myObjectBuilder_FractureComponentCubeBlock;
				myObjectBuilder_CubeBlock.ComponentContainer.Components.Add(componentData);
				myObjectBuilder_CubeBlock.BuildPercent = buildProgress;
				myObjectBuilder_CubeBlock.IntegrityPercent = MyDefinitionManager.Static.DestructionDefinition.ConvertedFractureIntegrityRatio * buildProgress;
				if (i == 0 && base.CubeGrid.GridSizeEnum == MyCubeSize.Small)
				{
					return myObjectBuilder_CubeBlock;
				}
				list.Add(myObjectBuilder_CubeBlock);
			}
			if (list.Count > 0)
			{
				return MyCompoundCubeBlock.CreateBuilder(list);
			}
			return null;
		}

		public static MyObjectBuilder_CubeGrid ConvertFracturedBlocksToComponents(MyObjectBuilder_CubeGrid gridBuilder)
		{
			bool flag = false;
			foreach (MyObjectBuilder_CubeBlock cubeBlock in gridBuilder.CubeBlocks)
			{
				if (cubeBlock is MyObjectBuilder_FracturedBlock)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return gridBuilder;
			}
			bool enableSmallToLargeConnections = gridBuilder.EnableSmallToLargeConnections;
			gridBuilder.EnableSmallToLargeConnections = false;
			bool createPhysics = gridBuilder.CreatePhysics;
			gridBuilder.CreatePhysics = true;
			MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(gridBuilder, fadeIn: false) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return gridBuilder;
			}
			myCubeGrid.ConvertFracturedBlocksToComponents();
			MyObjectBuilder_CubeGrid obj = (MyObjectBuilder_CubeGrid)myCubeGrid.GetObjectBuilder();
			gridBuilder.EnableSmallToLargeConnections = enableSmallToLargeConnections;
			obj.EnableSmallToLargeConnections = enableSmallToLargeConnections;
			gridBuilder.CreatePhysics = createPhysics;
			obj.CreatePhysics = createPhysics;
			myCubeGrid.Close();
			MyEntities.RemapObjectBuilder(obj);
			return obj;
		}

		public static void GetAllBlockBreakableShapeNames(MyCubeBlockDefinition blockDef, HashSet<Tuple<string, float>> outNamesAndBuildProgress)
		{
			string model = blockDef.Model;
			if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
			{
				MyDestructionData.Static.LoadModelDestruction(model, blockDef, Vector3.One);
			}
			GetAllBlockBreakableShapeNames(MyModels.GetModelOnlyData(model).HavokBreakableShapes[0], outNamesAndBuildProgress, 1f);
			if (blockDef.BuildProgressModels == null)
			{
				return;
			}
			float num = 0f;
			MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = blockDef.BuildProgressModels;
			foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in buildProgressModels)
			{
				model = buildProgressModel.File;
				if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
				{
					MyDestructionData.Static.LoadModelDestruction(model, blockDef, Vector3.One);
				}
				HkdBreakableShape shape = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
				float buildProgress = 0.5f * (buildProgressModel.BuildRatioUpperBound + num);
				GetAllBlockBreakableShapeNames(shape, outNamesAndBuildProgress, buildProgress);
				num = buildProgressModel.BuildRatioUpperBound;
			}
		}

		public static void GetAllBlockBreakableShapeNames(HkdBreakableShape shape, HashSet<Tuple<string, float>> outNamesAndBuildProgress, float buildProgress)
		{
			string name = shape.Name;
			if (!string.IsNullOrEmpty(name))
			{
				outNamesAndBuildProgress.Add(new Tuple<string, float>(name, buildProgress));
			}
			if (shape.GetChildrenCount() > 0)
			{
				List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
				shape.GetChildren(list);
				foreach (HkdShapeInstanceInfo item in list)
				{
					GetAllBlockBreakableShapeNames(item.Shape, outNamesAndBuildProgress, buildProgress);
				}
			}
		}

		private static void ConvertAllShapesToFractureComponentShapeBuilder(HkdBreakableShape shape, ref Matrix shapeRotation, MyBlockOrientation blockOrientation, HashSet<Tuple<string, float>> namesAndBuildProgress, MyObjectBuilder_FractureComponentCubeBlock fractureComponentBuilder, out float buildProgress)
		{
			buildProgress = 1f;
			string name = shape.Name;
			Tuple<string, float> tuple = null;
			foreach (Tuple<string, float> item2 in namesAndBuildProgress)
			{
				if (item2.Item1 == name)
				{
					tuple = item2;
					break;
				}
			}
			if (tuple != null && new MyBlockOrientation(ref shapeRotation) == blockOrientation)
			{
				MyObjectBuilder_FractureComponentBase.FracturedShape item = default(MyObjectBuilder_FractureComponentBase.FracturedShape);
				item.Name = name;
				item.Fixed = MyDestructionHelper.IsFixed(shape);
				fractureComponentBuilder.Shapes.Add(item);
				buildProgress = tuple.Item2;
			}
			if (shape.GetChildrenCount() > 0)
			{
				List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
				shape.GetChildren(list);
				foreach (HkdShapeInstanceInfo item3 in list)
				{
					Matrix shapeRotation2 = item3.GetTransform();
					ConvertAllShapesToFractureComponentShapeBuilder(item3.Shape, ref shapeRotation2, blockOrientation, namesAndBuildProgress, fractureComponentBuilder, out float buildProgress2);
					if (tuple == null)
					{
						buildProgress = buildProgress2;
					}
				}
			}
		}
	}
}
