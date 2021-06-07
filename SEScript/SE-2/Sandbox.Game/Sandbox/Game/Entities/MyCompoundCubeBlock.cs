using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_CompoundCubeBlock))]
	public class MyCompoundCubeBlock : MyCubeBlock, IMyDecalProxy
	{
		private class MyCompoundBlockPosComponent : MyBlockPosComponent
		{
			private class Sandbox_Game_Entities_MyCompoundCubeBlock_003C_003EMyCompoundBlockPosComponent_003C_003EActor : IActivator, IActivator<MyCompoundBlockPosComponent>
			{
				private sealed override object CreateInstance()
				{
					return new MyCompoundBlockPosComponent();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyCompoundBlockPosComponent CreateInstance()
				{
					return new MyCompoundBlockPosComponent();
				}

				MyCompoundBlockPosComponent IActivator<MyCompoundBlockPosComponent>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private MyCompoundCubeBlock m_block;

			public override void OnAddedToContainer()
			{
				base.OnAddedToContainer();
				m_block = (base.Container.Entity as MyCompoundCubeBlock);
			}

			public override void UpdateWorldMatrix(ref MatrixD parentWorldMatrix, object source = null, bool updateChildren = true, bool forceUpdateAllChildren = false)
			{
				m_block.UpdateBlocksWorldMatrix(ref parentWorldMatrix, source);
				base.UpdateWorldMatrix(ref parentWorldMatrix, source, updateChildren, forceUpdateAllChildren);
			}
		}

		private class MyDebugRenderComponentCompoundBlock : MyDebugRenderComponent
		{
			private readonly MyCompoundCubeBlock m_compoundBlock;

			public MyDebugRenderComponentCompoundBlock(MyCompoundCubeBlock compoundBlock)
				: base(compoundBlock)
			{
				m_compoundBlock = compoundBlock;
			}

			public override void DebugDraw()
			{
				foreach (MySlimBlock block in m_compoundBlock.GetBlocks())
				{
					if (block.FatBlock != null)
					{
						block.FatBlock.DebugDraw();
					}
				}
			}
		}

		private class Sandbox_Game_Entities_MyCompoundCubeBlock_003C_003EActor : IActivator, IActivator<MyCompoundCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyCompoundCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCompoundCubeBlock CreateInstance()
			{
				return new MyCompoundCubeBlock();
			}

			MyCompoundCubeBlock IActivator<MyCompoundCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static List<VertexArealBoneIndexWeight> m_boneIndexWeightTmp;

		private static readonly string COMPOUND_DUMMY = "compound_";

		private static readonly ushort BLOCK_IN_COMPOUND_LOCAL_ID = 32768;

		private static readonly ushort BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE = 32767;

		private static readonly MyStringId BUILD_TYPE_ANY = MyStringId.GetOrCompute("any");

		private static readonly string COMPOUND_BLOCK_SUBTYPE_NAME = "CompoundBlock";

		private static readonly HashSet<string> m_tmpTemplates = new HashSet<string>();

		private static readonly List<MyModelDummy> m_tmpDummies = new List<MyModelDummy>();

		private static readonly List<MyModelDummy> m_tmpOtherDummies = new List<MyModelDummy>();

		private readonly Dictionary<ushort, MySlimBlock> m_mapIdToBlock = new Dictionary<ushort, MySlimBlock>();

		private readonly List<MySlimBlock> m_blocks = new List<MySlimBlock>();

		private ushort m_nextId;

		private ushort m_localNextId;

		private readonly HashSet<string> m_templates = new HashSet<string>();

		private static readonly List<uint> m_tmpIds = new List<uint>();

		public MyCompoundCubeBlock()
		{
			base.PositionComp = new MyCompoundBlockPosComponent();
			base.Render = new MyRenderComponentCompoundCubeBlock();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = objectBuilder as MyObjectBuilder_CompoundCubeBlock;
			if (myObjectBuilder_CompoundCubeBlock.Blocks != null)
			{
				if (myObjectBuilder_CompoundCubeBlock.BlockIds != null)
				{
					for (int i = 0; i < myObjectBuilder_CompoundCubeBlock.Blocks.Length; i++)
					{
						ushort key = myObjectBuilder_CompoundCubeBlock.BlockIds[i];
						if (!m_mapIdToBlock.ContainsKey(key))
						{
							MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = myObjectBuilder_CompoundCubeBlock.Blocks[i];
							object obj = MyCubeBlockFactory.CreateCubeBlock(myObjectBuilder_CubeBlock);
							MySlimBlock mySlimBlock = obj as MySlimBlock;
							if (mySlimBlock == null)
							{
								mySlimBlock = new MySlimBlock();
							}
							mySlimBlock.Init(myObjectBuilder_CubeBlock, cubeGrid, obj as MyCubeBlock);
							if (mySlimBlock.FatBlock != null)
							{
								mySlimBlock.FatBlock.HookMultiplayer();
								mySlimBlock.FatBlock.Hierarchy.Parent = base.Hierarchy;
								m_mapIdToBlock.Add(key, mySlimBlock);
								m_blocks.Add(mySlimBlock);
							}
						}
					}
					RefreshNextId();
				}
				else
				{
					for (int j = 0; j < myObjectBuilder_CompoundCubeBlock.Blocks.Length; j++)
					{
						MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock2 = myObjectBuilder_CompoundCubeBlock.Blocks[j];
						object obj2 = MyCubeBlockFactory.CreateCubeBlock(myObjectBuilder_CubeBlock2);
						MySlimBlock mySlimBlock2 = obj2 as MySlimBlock;
						if (mySlimBlock2 == null)
						{
							mySlimBlock2 = new MySlimBlock();
						}
						mySlimBlock2.Init(myObjectBuilder_CubeBlock2, cubeGrid, obj2 as MyCubeBlock);
						mySlimBlock2.FatBlock.HookMultiplayer();
						mySlimBlock2.FatBlock.Hierarchy.Parent = base.Hierarchy;
						ushort key2 = CreateId(mySlimBlock2);
						m_mapIdToBlock.Add(key2, mySlimBlock2);
						m_blocks.Add(mySlimBlock2);
					}
				}
			}
			RefreshTemplates();
			AddDebugRenderComponent(new MyDebugRenderComponentCompoundBlock(this));
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = (MyObjectBuilder_CompoundCubeBlock)base.GetObjectBuilderCubeBlock(copy);
			if (m_mapIdToBlock.Count > 0)
			{
				myObjectBuilder_CompoundCubeBlock.Blocks = new MyObjectBuilder_CubeBlock[m_mapIdToBlock.Count];
				myObjectBuilder_CompoundCubeBlock.BlockIds = new ushort[m_mapIdToBlock.Count];
				int num = 0;
				{
					foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
					{
						myObjectBuilder_CompoundCubeBlock.BlockIds[num] = item.Key;
						if (!copy)
						{
							myObjectBuilder_CompoundCubeBlock.Blocks[num] = item.Value.GetObjectBuilder();
						}
						else
						{
							myObjectBuilder_CompoundCubeBlock.Blocks[num] = item.Value.GetCopyObjectBuilder();
						}
						num++;
					}
					return myObjectBuilder_CompoundCubeBlock;
				}
			}
			return myObjectBuilder_CompoundCubeBlock;
		}

		public override void OnAddedToScene(object source)
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null)
				{
					item.Value.FatBlock.OnAddedToScene(source);
				}
			}
			base.OnAddedToScene(source);
		}

		public override void OnRemovedFromScene(object source)
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null)
				{
					item.Value.FatBlock.OnRemovedFromScene(source);
				}
			}
			base.OnRemovedFromScene(source);
		}

		public override void UpdateVisual()
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null)
				{
					item.Value.FatBlock.UpdateVisual();
				}
			}
			base.UpdateVisual();
		}

		public override float GetMass()
		{
			float num = 0f;
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				num += item.Value.GetMass();
			}
			return num;
		}

		private void UpdateBlocksWorldMatrix(ref MatrixD parentWorldMatrix, object source = null)
		{
			MatrixD localMatrix = MatrixD.Identity;
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null)
				{
					GetBlockLocalMatrixFromGridPositionAndOrientation(item.Value, ref localMatrix);
					MatrixD worldMatrix = localMatrix * parentWorldMatrix;
					item.Value.FatBlock.PositionComp.SetWorldMatrix(worldMatrix, this, forceUpdate: true);
				}
			}
		}

		protected override void Closing()
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null)
				{
					item.Value.FatBlock.Close();
				}
			}
			base.Closing();
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				item.Value.CubeGrid = base.CubeGrid;
			}
		}

		internal override void OnTransformed(ref MatrixI transform)
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				item.Value.Transform(ref transform);
			}
		}

		internal override void UpdateWorldMatrix()
		{
			base.UpdateWorldMatrix();
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null)
				{
					item.Value.FatBlock.UpdateWorldMatrix();
				}
			}
		}

		public override bool ConnectionAllowed(ref Vector3I otherBlockPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null && item.Value.FatBlock.ConnectionAllowed(ref otherBlockPos, ref faceNormal, def))
				{
					return true;
				}
			}
			return false;
		}

		public override bool ConnectionAllowed(ref Vector3I otherBlockMinPos, ref Vector3I otherBlockMaxPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if (item.Value.FatBlock != null && item.Value.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def))
				{
					return true;
				}
			}
			return false;
		}

		public bool Add(MySlimBlock block, out ushort id)
		{
			id = CreateId(block);
			return Add(id, block);
		}

		public bool Add(ushort id, MySlimBlock block)
		{
			if (!CanAddBlock(block))
			{
				return false;
			}
			if (m_mapIdToBlock.ContainsKey(id))
			{
				return false;
			}
			m_mapIdToBlock.Add(id, block);
			m_blocks.Add(block);
			MatrixD worldMatrix = base.Parent.WorldMatrix;
			MatrixD localMatrix = MatrixD.Identity;
			GetBlockLocalMatrixFromGridPositionAndOrientation(block, ref localMatrix);
			MatrixD worldMatrix2 = localMatrix * worldMatrix;
			block.FatBlock.PositionComp.SetWorldMatrix(worldMatrix2, this, forceUpdate: true);
			block.FatBlock.Hierarchy.Parent = base.Hierarchy;
			block.FatBlock.OnAddedToScene(this);
			base.CubeGrid.UpdateBlockNeighbours(SlimBlock);
			RefreshTemplates();
			if (block.IsMultiBlockPart)
			{
				base.CubeGrid.AddMultiBlockInfo(block);
			}
			return true;
		}

		public bool Remove(MySlimBlock block, bool merged = false)
		{
			KeyValuePair<ushort, MySlimBlock> keyValuePair = m_mapIdToBlock.FirstOrDefault((KeyValuePair<ushort, MySlimBlock> p) => p.Value == block);
			if (keyValuePair.Value == block)
			{
				return Remove(keyValuePair.Key, merged);
			}
			return false;
		}

		public bool Remove(ushort blockId, bool merged = false)
		{
			if (m_mapIdToBlock.TryGetValue(blockId, out MySlimBlock value))
			{
				m_mapIdToBlock.Remove(blockId);
				m_blocks.Remove(value);
				if (!merged)
				{
					if (value.IsMultiBlockPart)
					{
						base.CubeGrid.RemoveMultiBlockInfo(value);
					}
					value.FatBlock.OnRemovedFromScene(this);
					value.FatBlock.Close();
				}
				if (value.FatBlock.Hierarchy.Parent == base.Hierarchy)
				{
					value.FatBlock.Hierarchy.Parent = null;
				}
				if (!merged)
				{
					base.CubeGrid.UpdateBlockNeighbours(SlimBlock);
				}
				RefreshTemplates();
				return true;
			}
			return false;
		}

		public bool CanAddBlock(MySlimBlock block)
		{
			if (block == null || block.FatBlock == null)
			{
				return false;
			}
			if (m_mapIdToBlock.ContainsValue(block))
			{
				return false;
			}
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					if (!CanAddBlock(block2.BlockDefinition, block2.Orientation, block2.MultiBlockId))
					{
						return false;
					}
				}
				return true;
			}
			return CanAddBlock(block.BlockDefinition, block.Orientation, block.MultiBlockId);
		}

		public bool CanAddBlock(MyCubeBlockDefinition definition, MyBlockOrientation? orientation, int multiBlockId = 0, bool ignoreSame = false)
		{
			if (!IsCompoundEnabled(definition))
			{
				return false;
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
			{
				if (!orientation.HasValue)
				{
					return false;
				}
				if (m_blocks.Count == 0)
				{
					return true;
				}
				orientation.Value.GetMatrix(out Matrix result);
				m_tmpOtherDummies.Clear();
				GetCompoundCollisionDummies(definition, m_tmpOtherDummies);
				foreach (MySlimBlock block in m_blocks)
				{
					if (block.BlockDefinition.Id.SubtypeId == definition.Id.SubtypeId && block.Orientation == orientation.Value)
					{
						if (!ignoreSame)
						{
							return false;
						}
					}
					else if ((multiBlockId == 0 || block.MultiBlockId != multiBlockId) && !block.BlockDefinition.IsGeneratedBlock)
					{
						m_tmpDummies.Clear();
						GetCompoundCollisionDummies(block.BlockDefinition, m_tmpDummies);
						block.Orientation.GetMatrix(out Matrix result2);
						if (CompoundDummiesIntersect(ref result2, ref result, m_tmpDummies, m_tmpOtherDummies))
						{
							m_tmpDummies.Clear();
							m_tmpOtherDummies.Clear();
							return false;
						}
					}
				}
				m_tmpDummies.Clear();
				m_tmpOtherDummies.Clear();
				return true;
			}
			if (orientation.HasValue)
			{
				foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
				{
					if (item.Value.BlockDefinition.Id.SubtypeId == definition.Id.SubtypeId)
					{
						MyBlockOrientation orientation2 = item.Value.Orientation;
						MyBlockOrientation? orientation3 = orientation;
						if (orientation2 == orientation3)
						{
							return false;
						}
					}
					_ = definition.BuildType;
					if (item.Value.BlockDefinition.BuildType == definition.BuildType)
					{
						MyBlockOrientation orientation2 = item.Value.Orientation;
						MyBlockOrientation? orientation3 = orientation;
						if (orientation2 == orientation3)
						{
							return false;
						}
					}
				}
			}
			string[] compoundTemplates = definition.CompoundTemplates;
			foreach (string text in compoundTemplates)
			{
				if (!m_templates.Contains(text))
				{
					continue;
				}
				MyCompoundBlockTemplateDefinition templateDefinition = GetTemplateDefinition(text);
				if (templateDefinition == null || templateDefinition.Bindings == null)
				{
					continue;
				}
				MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding templateDefinitionBinding = GetTemplateDefinitionBinding(templateDefinition, definition);
				if (templateDefinitionBinding == null)
				{
					continue;
				}
				if (templateDefinitionBinding.BuildType == BUILD_TYPE_ANY)
				{
					return true;
				}
				if (!templateDefinitionBinding.Multiple)
				{
					bool flag = false;
					foreach (KeyValuePair<ushort, MySlimBlock> item2 in m_mapIdToBlock)
					{
						if (item2.Value.BlockDefinition.BuildType == definition.BuildType)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
				}
				if (orientation.HasValue)
				{
					bool flag2 = false;
					foreach (KeyValuePair<ushort, MySlimBlock> item3 in m_mapIdToBlock)
					{
						MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding templateDefinitionBinding2 = GetTemplateDefinitionBinding(templateDefinition, item3.Value.BlockDefinition);
						if (templateDefinitionBinding2 != null && !(templateDefinitionBinding2.BuildType == BUILD_TYPE_ANY))
						{
							MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding rotationBinding = GetRotationBinding(templateDefinition, definition, item3.Value.BlockDefinition);
							if (rotationBinding != null)
							{
								if (rotationBinding.BuildTypeReference == definition.BuildType)
								{
									if (IsRotationValid(orientation.Value, item3.Value.Orientation, rotationBinding.Rotations) || (rotationBinding.BuildTypeReference == item3.Value.BlockDefinition.BuildType && IsRotationValid(item3.Value.Orientation, orientation.Value, rotationBinding.Rotations)))
									{
										continue;
									}
								}
								else if (IsRotationValid(item3.Value.Orientation, orientation.Value, rotationBinding.Rotations))
								{
									continue;
								}
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						continue;
					}
				}
				return true;
			}
			return false;
		}

		public static bool CanAddBlocks(MyCubeBlockDefinition definition, MyBlockOrientation orientation, MyCubeBlockDefinition otherDefinition, MyBlockOrientation otherOrientation)
		{
			if (!IsCompoundEnabled(definition) || !IsCompoundEnabled(otherDefinition))
			{
				return false;
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
			{
				orientation.GetMatrix(out Matrix result);
				m_tmpDummies.Clear();
				GetCompoundCollisionDummies(definition, m_tmpDummies);
				otherOrientation.GetMatrix(out Matrix result2);
				m_tmpOtherDummies.Clear();
				GetCompoundCollisionDummies(otherDefinition, m_tmpOtherDummies);
				bool num = CompoundDummiesIntersect(ref result, ref result2, m_tmpDummies, m_tmpOtherDummies);
				m_tmpDummies.Clear();
				m_tmpOtherDummies.Clear();
				return !num;
			}
			return true;
		}

		private static bool CompoundDummiesIntersect(ref Matrix thisRotation, ref Matrix otherRotation, List<MyModelDummy> thisDummies, List<MyModelDummy> otherDummies)
		{
			foreach (MyModelDummy thisDummy in thisDummies)
			{
				Vector3 vector = new Vector3(thisDummy.Matrix.Right.Length(), thisDummy.Matrix.Up.Length(), thisDummy.Matrix.Forward.Length()) * 0.5f;
				BoundingBox box = new BoundingBox(-vector, vector);
				Matrix matrix = Matrix.Normalize(thisDummy.Matrix);
				Matrix.Multiply(ref matrix, ref thisRotation, out Matrix result);
				Matrix.Invert(ref result, out matrix);
				foreach (MyModelDummy otherDummy in otherDummies)
				{
					Vector3 vector2 = new Vector3(otherDummy.Matrix.Right.Length(), otherDummy.Matrix.Up.Length(), otherDummy.Matrix.Forward.Length()) * 0.5f;
					BoundingBox boundingBox = new BoundingBox(-vector2, vector2);
					Matrix matrix2 = Matrix.Normalize(otherDummy.Matrix);
					Matrix.Multiply(ref matrix2, ref otherRotation, out Matrix result2);
					Matrix.Multiply(ref result2, ref matrix, out matrix2);
					if (MyOrientedBoundingBox.Create(boundingBox, matrix2).Intersects(ref box))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void DebugDrawAABB(BoundingBox aabb, Matrix localMatrix)
		{
			MatrixD matrix = Matrix.CreateScale(2f * aabb.HalfExtents) * (MatrixD)localMatrix * base.PositionComp.WorldMatrix;
			MyRenderProxy.DebugDrawAxis(MatrixD.Normalize(matrix), 0.1f, depthRead: false);
			MyRenderProxy.DebugDrawOBB(matrix, Color.Green, 0.1f, depthRead: false, smooth: false);
		}

		private void DebugDrawOBB(MyOrientedBoundingBox obb, Matrix localMatrix)
		{
			MatrixD matrix = Matrix.CreateFromTransformScale(obb.Orientation, obb.Center, 2f * obb.HalfExtent) * (MatrixD)localMatrix * base.PositionComp.WorldMatrix;
			MyRenderProxy.DebugDrawAxis(MatrixD.Normalize(matrix), 0.1f, depthRead: false);
			MyRenderProxy.DebugDrawOBB(matrix, Vector3.One, 0.1f, depthRead: false, smooth: false);
		}

		private bool IsRotationValid(MyBlockOrientation refOrientation, MyBlockOrientation orientation, MyBlockOrientation[] validRotations)
		{
			MatrixI matrix = new MatrixI(Vector3I.Zero, refOrientation.Forward, refOrientation.Up);
			MatrixI.Invert(ref matrix, out MatrixI result);
			Matrix floatMatrix = result.GetFloatMatrix();
			Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(Vector3.TransformNormal((Vector3)Base6Directions.GetIntVector(orientation.Forward), floatMatrix));
			Base6Directions.Direction closestDirection2 = Base6Directions.GetClosestDirection(Vector3.TransformNormal((Vector3)Base6Directions.GetIntVector(orientation.Up), floatMatrix));
			for (int i = 0; i < validRotations.Length; i++)
			{
				MyBlockOrientation myBlockOrientation = validRotations[i];
				if (myBlockOrientation.Forward == closestDirection && myBlockOrientation.Up == closestDirection2)
				{
					return true;
				}
			}
			return false;
		}

		public MySlimBlock GetBlock(ushort id)
		{
			if (m_mapIdToBlock.TryGetValue(id, out MySlimBlock value))
			{
				return value;
			}
			return null;
		}

		public ushort? GetBlockId(MySlimBlock block)
		{
			KeyValuePair<ushort, MySlimBlock> keyValuePair = m_mapIdToBlock.FirstOrDefault((KeyValuePair<ushort, MySlimBlock> p) => p.Value == block);
			if (keyValuePair.Value == block)
			{
				return keyValuePair.Key;
			}
			return null;
		}

		public ListReader<MySlimBlock> GetBlocks()
		{
			return m_blocks;
		}

		public int GetBlocksCount()
		{
			return m_blocks.Count;
		}

		public static MyObjectBuilder_CompoundCubeBlock CreateBuilder(MyObjectBuilder_CubeBlock cubeBlockBuilder)
		{
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompoundCubeBlock>(COMPOUND_BLOCK_SUBTYPE_NAME);
			myObjectBuilder_CompoundCubeBlock.EntityId = MyEntityIdentifier.AllocateId();
			myObjectBuilder_CompoundCubeBlock.Min = cubeBlockBuilder.Min;
			myObjectBuilder_CompoundCubeBlock.BlockOrientation = new MyBlockOrientation(ref Quaternion.Identity);
			myObjectBuilder_CompoundCubeBlock.ColorMaskHSV = cubeBlockBuilder.ColorMaskHSV;
			myObjectBuilder_CompoundCubeBlock.Blocks = new MyObjectBuilder_CubeBlock[1];
			myObjectBuilder_CompoundCubeBlock.Blocks[0] = cubeBlockBuilder;
			return myObjectBuilder_CompoundCubeBlock;
		}

		public static MyObjectBuilder_CompoundCubeBlock CreateBuilder(List<MyObjectBuilder_CubeBlock> cubeBlockBuilders)
		{
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompoundCubeBlock>(COMPOUND_BLOCK_SUBTYPE_NAME);
			myObjectBuilder_CompoundCubeBlock.EntityId = MyEntityIdentifier.AllocateId();
			myObjectBuilder_CompoundCubeBlock.Min = cubeBlockBuilders[0].Min;
			myObjectBuilder_CompoundCubeBlock.BlockOrientation = new MyBlockOrientation(ref Quaternion.Identity);
			myObjectBuilder_CompoundCubeBlock.ColorMaskHSV = cubeBlockBuilders[0].ColorMaskHSV;
			myObjectBuilder_CompoundCubeBlock.Blocks = cubeBlockBuilders.ToArray();
			return myObjectBuilder_CompoundCubeBlock;
		}

		public static MyCubeBlockDefinition GetCompoundCubeBlockDefinition()
		{
			return MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId(typeof(MyObjectBuilder_CompoundCubeBlock), COMPOUND_BLOCK_SUBTYPE_NAME));
		}

		private static MyCompoundBlockTemplateDefinition GetTemplateDefinition(string template)
		{
			return MyDefinitionManager.Static.GetCompoundBlockTemplateDefinition(new MyDefinitionId(typeof(MyObjectBuilder_CompoundBlockTemplateDefinition), template));
		}

		private static MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding GetTemplateDefinitionBinding(MyCompoundBlockTemplateDefinition templateDefinition, MyCubeBlockDefinition blockDefinition)
		{
			MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding[] bindings = templateDefinition.Bindings;
			foreach (MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding myCompoundBlockBinding in bindings)
			{
				if (myCompoundBlockBinding.BuildType == BUILD_TYPE_ANY)
				{
					return myCompoundBlockBinding;
				}
			}
			bindings = templateDefinition.Bindings;
			foreach (MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding myCompoundBlockBinding2 in bindings)
			{
				if (myCompoundBlockBinding2.BuildType == blockDefinition.BuildType && blockDefinition.BuildType != MyStringId.NullOrEmpty)
				{
					return myCompoundBlockBinding2;
				}
			}
			return null;
		}

		private static MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding GetRotationBinding(MyCompoundBlockTemplateDefinition templateDefinition, MyCubeBlockDefinition blockDefinition1, MyCubeBlockDefinition blockDefinition2)
		{
			MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding templateDefinitionBinding = GetTemplateDefinitionBinding(templateDefinition, blockDefinition1);
			if (templateDefinitionBinding == null)
			{
				return null;
			}
			MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding rotationBinding = GetRotationBinding(templateDefinitionBinding, blockDefinition2);
			if (rotationBinding != null)
			{
				return rotationBinding;
			}
			templateDefinitionBinding = GetTemplateDefinitionBinding(templateDefinition, blockDefinition2);
			if (templateDefinitionBinding == null)
			{
				return null;
			}
			return GetRotationBinding(templateDefinitionBinding, blockDefinition1);
		}

		private static MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding GetRotationBinding(MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding binding, MyCubeBlockDefinition blockDefinition)
		{
			if (binding.RotationBinds != null)
			{
				MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding[] rotationBinds = binding.RotationBinds;
				foreach (MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding myCompoundBlockRotationBinding in rotationBinds)
				{
					if (myCompoundBlockRotationBinding.BuildTypeReference == blockDefinition.BuildType)
					{
						return myCompoundBlockRotationBinding;
					}
				}
			}
			return null;
		}

		private void RefreshTemplates()
		{
			m_templates.Clear();
			if (!MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
			{
				foreach (KeyValuePair<ushort, MySlimBlock> item3 in m_mapIdToBlock)
				{
					if (item3.Value.BlockDefinition.CompoundTemplates != null)
					{
						if (m_templates.Count == 0)
						{
							string[] compoundTemplates = item3.Value.BlockDefinition.CompoundTemplates;
							foreach (string item in compoundTemplates)
							{
								m_templates.Add(item);
							}
						}
						else
						{
							m_tmpTemplates.Clear();
							string[] compoundTemplates = item3.Value.BlockDefinition.CompoundTemplates;
							foreach (string item2 in compoundTemplates)
							{
								m_tmpTemplates.Add(item2);
							}
							m_templates.IntersectWith(m_tmpTemplates);
						}
					}
				}
			}
		}

		public override BoundingBox GetGeometryLocalBox()
		{
			BoundingBox result = BoundingBox.CreateInvalid();
			foreach (MySlimBlock block in GetBlocks())
			{
				if (block.FatBlock != null)
				{
					block.Orientation.GetMatrix(out Matrix result2);
					result.Include(block.FatBlock.Model.BoundingBox.Transform(result2));
				}
			}
			return result;
		}

		private void RefreshNextId()
		{
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				if ((item.Key & BLOCK_IN_COMPOUND_LOCAL_ID) == BLOCK_IN_COMPOUND_LOCAL_ID)
				{
					ushort val = (ushort)(item.Key & ~BLOCK_IN_COMPOUND_LOCAL_ID);
					m_localNextId = Math.Max(m_localNextId, val);
				}
				else
				{
					ushort key = item.Key;
					m_nextId = Math.Max(m_nextId, key);
				}
			}
			if (m_nextId == BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
			{
				m_nextId = 0;
			}
			else
			{
				m_nextId++;
			}
			if (m_localNextId == BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
			{
				m_localNextId = 0;
			}
			else
			{
				m_localNextId++;
			}
		}

		private ushort CreateId(MySlimBlock block)
		{
			bool isGeneratedBlock = block.BlockDefinition.IsGeneratedBlock;
			ushort num = 0;
			if (isGeneratedBlock)
			{
				num = (ushort)(m_localNextId | BLOCK_IN_COMPOUND_LOCAL_ID);
				while (m_mapIdToBlock.ContainsKey(num))
				{
					if (m_localNextId == BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
					{
						m_localNextId = 0;
					}
					else
					{
						m_localNextId++;
					}
					num = (ushort)(m_localNextId | BLOCK_IN_COMPOUND_LOCAL_ID);
				}
				m_localNextId++;
			}
			else
			{
				num = m_nextId;
				while (m_mapIdToBlock.ContainsKey(num))
				{
					if (m_nextId == BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
					{
						m_nextId = 0;
					}
					else
					{
						m_nextId++;
					}
					num = m_nextId;
				}
				m_nextId++;
			}
			return num;
		}

		internal void DoDamage(float damage, MyStringHash damageType, MyHitInfo? hitInfo, long attackerId)
		{
			float num = 0f;
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				num += item.Value.MaxIntegrity;
			}
			for (int num2 = m_blocks.Count - 1; num2 >= 0; num2--)
			{
				MySlimBlock mySlimBlock = m_blocks[num2];
				mySlimBlock.DoDamage(damage * (mySlimBlock.MaxIntegrity / num), damageType, hitInfo, addDirtyParts: true, attackerId);
			}
		}

		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, object customdata, IMyDecalHandler decalHandler, MyStringHash material)
		{
			MyCubeGrid.MyCubeGridHitInfo myCubeGridHitInfo = customdata as MyCubeGrid.MyCubeGridHitInfo;
			if (myCubeGridHitInfo != null)
			{
				MyPhysicalMaterialDefinition physicalMaterial = m_mapIdToBlock.First().Value.BlockDefinition.PhysicalMaterial;
				MyDecalRenderInfo renderInfo = default(MyDecalRenderInfo);
				renderInfo.Position = Vector3D.Transform(hitInfo.Position, base.CubeGrid.PositionComp.WorldMatrixInvScaled);
				renderInfo.Normal = Vector3D.TransformNormal(hitInfo.Normal, base.CubeGrid.PositionComp.WorldMatrixInvScaled);
				renderInfo.RenderObjectIds = base.CubeGrid.Render.RenderObjectIDs;
				renderInfo.Source = source;
				VertexBoneIndicesWeights? affectingBoneIndicesWeights = myCubeGridHitInfo.Triangle.GetAffectingBoneIndicesWeights(ref m_boneIndexWeightTmp);
				if (affectingBoneIndicesWeights.HasValue)
				{
					renderInfo.BoneIndices = affectingBoneIndicesWeights.Value.Indices;
					renderInfo.BoneWeights = affectingBoneIndicesWeights.Value.Weights;
				}
				if (material.GetHashCode() == 0)
				{
					renderInfo.Material = MyStringHash.GetOrCompute(physicalMaterial.Id.SubtypeName);
				}
				else
				{
					renderInfo.Material = material;
				}
				m_tmpIds.Clear();
				decalHandler.AddDecal(ref renderInfo, m_tmpIds);
				foreach (uint tmpId in m_tmpIds)
				{
					base.CubeGrid.RenderData.AddDecal(base.Position, myCubeGridHitInfo, tmpId);
				}
			}
		}

		public bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, out ushort blockId, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES, bool checkZFight = false, bool ignoreGenerated = false)
		{
			t = null;
			blockId = 0;
			double num = double.MaxValue;
			bool result = false;
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				MySlimBlock value = item.Value;
				if ((!ignoreGenerated || !value.BlockDefinition.IsGeneratedBlock) && value.FatBlock.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? t2) && t2.HasValue)
				{
					double num2 = (t2.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
					if (num2 < num && (!checkZFight || !(num < num2 + 0.0010000000474974513)))
					{
						num = num2;
						t = t2;
						blockId = item.Key;
						result = true;
					}
				}
			}
			return result;
		}

		public bool GetIntersectionWithLine_FullyBuiltProgressModels(ref LineD line, out MyIntersectionResultLineTriangleEx? t, out ushort blockId, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES, bool checkZFight = false, bool ignoreGenerated = false)
		{
			t = null;
			blockId = 0;
			double num = double.MaxValue;
			bool result = false;
			foreach (KeyValuePair<ushort, MySlimBlock> item in m_mapIdToBlock)
			{
				MySlimBlock value = item.Value;
				if (!ignoreGenerated || !value.BlockDefinition.IsGeneratedBlock)
				{
					MyModel modelOnlyData = MyModels.GetModelOnlyData(value.BlockDefinition.Model);
					if (modelOnlyData != null)
					{
						MyIntersectionResultLineTriangleEx? intersectionWithLine = modelOnlyData.GetTrianglePruningStructure().GetIntersectionWithLine(value.FatBlock, ref line, flags);
						if (intersectionWithLine.HasValue)
						{
							double num2 = (intersectionWithLine.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
							if (num2 < num && (!checkZFight || !(num < num2 + 0.0010000000474974513)))
							{
								num = num2;
								t = intersectionWithLine;
								blockId = item.Key;
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		private static void GetBlockLocalMatrixFromGridPositionAndOrientation(MySlimBlock block, ref MatrixD localMatrix)
		{
			block.Orientation.GetMatrix(out Matrix result);
			localMatrix = result;
			localMatrix.Translation = block.CubeGrid.GridSize * block.Position;
		}

		private static void GetCompoundCollisionDummies(MyCubeBlockDefinition definition, List<MyModelDummy> outDummies)
		{
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(definition.Model);
			if (modelOnlyDummies != null)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
				{
					if (dummy.Key.ToLower().StartsWith(COMPOUND_DUMMY))
					{
						outDummies.Add(dummy.Value);
					}
				}
			}
		}

		public static bool IsCompoundEnabled(MyCubeBlockDefinition blockDefinition)
		{
			if (!MyFakes.ENABLE_COMPOUND_BLOCKS)
			{
				return false;
			}
			if (blockDefinition == null)
			{
				return false;
			}
			if (blockDefinition.CubeSize != 0)
			{
				return false;
			}
			if (blockDefinition.Size != Vector3I.One)
			{
				return false;
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
			{
				return blockDefinition.CompoundEnabled;
			}
			if (blockDefinition.CompoundTemplates != null)
			{
				return blockDefinition.CompoundTemplates.Length != 0;
			}
			return false;
		}
	}
}
