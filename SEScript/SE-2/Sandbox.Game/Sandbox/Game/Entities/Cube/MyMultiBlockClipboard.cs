using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyMultiBlockClipboard : MyGridClipboardAdvanced
	{
		private static List<Vector3D> m_tmpCollisionPoints = new List<Vector3D>();

		private static List<MyEntity> m_tmpNearEntities = new List<MyEntity>();

		private MyMultiBlockDefinition m_multiBlockDefinition;

		public MySlimBlock RemoveBlock;

		public ushort? BlockIdInCompound;

		private Vector3I m_addPos;

		public HashSet<Tuple<MySlimBlock, ushort?>> RemoveBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();

		private HashSet<Vector3I> m_tmpBlockPositionsSet = new HashSet<Vector3I>();

		private bool m_lastVoxelState;

		protected override bool AnyCopiedGridIsStatic => false;

		public MyMultiBlockClipboard(MyPlacementSettings settings, bool calculateVelocity = true)
			: base(settings, calculateVelocity)
		{
			m_useDynamicPreviews = false;
		}

		public override void Deactivate(bool afterPaste = false)
		{
			m_multiBlockDefinition = null;
			base.Deactivate(afterPaste);
		}

		public override void Update()
		{
			if (!base.IsActive)
			{
				return;
			}
			UpdateHitEntity();
			if (!m_visible)
			{
				ShowPreview(show: false);
			}
			else
			{
				if (base.PreviewGrids.Count == 0)
				{
					return;
				}
				if (m_dragDistance == 0f)
				{
					SetupDragDistance();
				}
				if (m_dragDistance > MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
				{
					m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
				}
				UpdatePastePosition();
				UpdateGridTransformations();
				FixSnapTransformationBase6();
				if (m_calculateVelocity)
				{
					m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
				}
				m_canBePlaced = TestPlacement();
				if (!m_visible)
				{
					ShowPreview(show: false);
					return;
				}
				ShowPreview(show: true);
				TestBuildingMaterials();
				m_canBePlaced &= base.CharacterHasEnoughMaterials;
				UpdatePreview();
				if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
				}
			}
		}

		public override bool PasteGrid(bool deactivate = true, bool showWarning = true)
		{
			if (base.CopiedGrids.Count > 0 && !base.IsActive)
			{
				Activate();
				return true;
			}
			if (!m_canBePlaced)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			if (base.PreviewGrids.Count == 0)
			{
				return false;
			}
			bool flag = RemoveBlock != null && !RemoveBlock.CubeGrid.IsStatic;
			if (MyCubeBuilder.Static.DynamicMode || flag)
			{
				return PasteGridsInDynamicMode(deactivate);
			}
			return PasteGridsInStaticMode(deactivate);
		}

		public override bool EntityCanPaste(MyEntity pastingEntity)
		{
			if (base.CopiedGrids.Count < 1)
			{
				return false;
			}
			if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				return true;
			}
			MyCubeBuilder.BuildComponent.GetMultiBlockPlacementMaterials(m_multiBlockDefinition);
			return MyCubeBuilder.BuildComponent.HasBuildingMaterials(pastingEntity);
		}

		private bool PasteGridsInDynamicMode(bool deactivate)
		{
			List<bool> list = new List<bool>();
			foreach (MyObjectBuilder_CubeGrid copiedGrid in base.CopiedGrids)
			{
				list.Add(copiedGrid.IsStatic);
				copiedGrid.IsStatic = false;
				BeforeCreateGrid(copiedGrid);
			}
			bool result = PasteGridInternal(deactivate, null, null, null, multiBlock: true);
			for (int i = 0; i < base.CopiedGrids.Count; i++)
			{
				base.CopiedGrids[i].IsStatic = list[i];
			}
			return result;
		}

		private bool PasteGridsInStaticMode(bool deactivate)
		{
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			List<MatrixD> list2 = new List<MatrixD>();
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = base.CopiedGrids[0];
			BeforeCreateGrid(myObjectBuilder_CubeGrid);
			list.Add(myObjectBuilder_CubeGrid);
			MatrixD worldMatrix = base.PreviewGrids[0].WorldMatrix;
			MyObjectBuilder_CubeGrid value = MyCubeBuilder.ConvertGridBuilderToStatic(myObjectBuilder_CubeGrid, worldMatrix);
			base.CopiedGrids[0] = value;
			list2.Add(worldMatrix);
			for (int i = 1; i < base.CopiedGrids.Count; i++)
			{
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid2 = base.CopiedGrids[i];
				BeforeCreateGrid(myObjectBuilder_CubeGrid2);
				list.Add(myObjectBuilder_CubeGrid2);
				MatrixD worldMatrix2 = base.PreviewGrids[i].WorldMatrix;
				list2.Add(worldMatrix2);
				if (base.CopiedGrids[i].IsStatic)
				{
					MyObjectBuilder_CubeGrid value2 = MyCubeBuilder.ConvertGridBuilderToStatic(myObjectBuilder_CubeGrid2, worldMatrix2);
					base.CopiedGrids[i] = value2;
				}
			}
			bool result = PasteGridInternal(deactivate, null, m_touchingGrids, null, multiBlock: true);
			base.CopiedGrids.Clear();
			base.CopiedGrids.AddRange(list);
			for (int j = 0; j < base.PreviewGrids.Count; j++)
			{
				base.PreviewGrids[j].WorldMatrix = list2[j];
			}
			return result;
		}

		protected override void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			if (MyCubeBuilder.Static.HitInfo.HasValue)
			{
				m_pastePosition = MyCubeBuilder.Static.HitInfo.Value.Position;
			}
			else
			{
				m_pastePosition = MyCubeBuilder.Static.FreePlacementTarget;
			}
			double gridSize = MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
			MyCoordinateSystem.CoordSystemData coordSystemData = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref m_pastePosition, gridSize, m_settings.StaticGridAlignToCenter);
			base.EnableStationRotation = MyCubeBuilder.Static.DynamicMode;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				AlignClipboardToGravity();
				m_visible = true;
				base.IsSnapped = false;
				m_lastVoxelState = false;
			}
			else if (RemoveBlock != null)
			{
				m_pastePosition = Vector3D.Transform(m_addPos * RemoveBlock.CubeGrid.GridSize, RemoveBlock.CubeGrid.WorldMatrix);
				if (!base.IsSnapped && RemoveBlock.CubeGrid.IsStatic)
				{
					m_pasteOrientationAngle = 0f;
					m_pasteDirForward = RemoveBlock.CubeGrid.WorldMatrix.Forward;
					m_pasteDirUp = RemoveBlock.CubeGrid.WorldMatrix.Up;
				}
				base.IsSnapped = true;
				m_lastVoxelState = false;
			}
			else
			{
				if (!MyFakes.ENABLE_BLOCK_PLACEMENT_ON_VOXEL || !(m_hitEntity is MyVoxelBase))
				{
					return;
				}
				if (MyCoordinateSystem.Static.LocalCoordExist)
				{
					m_pastePosition = coordSystemData.SnappedTransform.Position;
					if (!m_lastVoxelState)
					{
						AlignRotationToCoordSys();
					}
				}
				base.IsSnapped = true;
				m_lastVoxelState = true;
			}
		}

		public override void MoveEntityFurther()
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				base.MoveEntityFurther();
				if (m_dragDistance > MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
				{
					m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
				}
			}
		}

		public override void MoveEntityCloser()
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				base.MoveEntityCloser();
				if (m_dragDistance < MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
				{
					m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
				}
			}
		}

		protected override void ChangeClipboardPreview(bool visible, List<MyCubeGrid> previewGrids, List<MyObjectBuilder_CubeGrid> copiedGrids)
		{
			base.ChangeClipboardPreview(visible, previewGrids, copiedGrids);
			if (visible && MySession.Static.SurvivalMode)
			{
				foreach (MyCubeGrid previewGrid in base.PreviewGrids)
				{
					foreach (MySlimBlock block in previewGrid.GetBlocks())
					{
						MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
						if (myCompoundCubeBlock != null)
						{
							foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
							{
								SetBlockToFullIntegrity(block2);
							}
						}
						else
						{
							SetBlockToFullIntegrity(block);
						}
					}
				}
			}
		}

		private static void SetBlockToFullIntegrity(MySlimBlock block)
		{
			float buildRatio = block.ComponentStack.BuildRatio;
			block.ComponentStack.SetIntegrity(block.ComponentStack.MaxIntegrity, block.ComponentStack.MaxIntegrity);
			if (block.BlockDefinition.ModelChangeIsNeeded(buildRatio, block.ComponentStack.BuildRatio))
			{
				block.UpdateVisual();
			}
		}

		private void UpdateHitEntity()
		{
			m_closestHitDistSq = float.MaxValue;
			m_hitPos = new Vector3(0f, 0f, 0f);
			m_hitNormal = new Vector3(1f, 0f, 0f);
			m_hitEntity = null;
			m_addPos = Vector3I.Zero;
			RemoveBlock = null;
			BlockIdInCompound = null;
			RemoveBlocksInMultiBlock.Clear();
			m_dynamicBuildAllowed = false;
			m_visible = false;
			m_canBePlaced = false;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				m_visible = true;
				return;
			}
			MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
			if (MyCubeBuilder.Static.CurrentGrid == null && MyCubeBuilder.Static.CurrentVoxelBase == null)
			{
				MyCubeBuilder.Static.ChooseHitObject();
			}
			if (MyCubeBuilder.Static.HitInfo.HasValue)
			{
				float cubeSize = MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
				MyCubeGrid myCubeGrid = MyCubeBuilder.Static.HitInfo.Value.HkHitInfo.GetHitEntity() as MyCubeGrid;
				bool placingSmallGridOnLargeStatic = myCubeGrid != null && myCubeGrid.IsStatic && myCubeGrid.GridSizeEnum == MyCubeSize.Large && base.CopiedGrids[0].GridSizeEnum == MyCubeSize.Small && MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE;
				if (MyCubeBuilder.Static.GetAddAndRemovePositions(cubeSize, placingSmallGridOnLargeStatic, out m_addPos, out Vector3? _, out Vector3I addDir, out Vector3I _, out RemoveBlock, out BlockIdInCompound, RemoveBlocksInMultiBlock))
				{
					if (RemoveBlock != null)
					{
						m_hitPos = MyCubeBuilder.Static.HitInfo.Value.Position;
						m_closestHitDistSq = (float)(m_hitPos - pasteMatrix.Translation).LengthSquared();
						m_hitNormal = addDir;
						m_hitEntity = RemoveBlock.CubeGrid;
						double num = MyDefinitionManager.Static.GetCubeSize(RemoveBlock.CubeGrid.GridSizeEnum);
						double num2 = MyDefinitionManager.Static.GetCubeSize(base.CopiedGrids[0].GridSizeEnum);
						if (num / num2 < 1.0)
						{
							RemoveBlock = null;
						}
						m_visible = (RemoveBlock != null);
					}
					else if (MyFakes.ENABLE_BLOCK_PLACEMENT_ON_VOXEL && MyCubeBuilder.Static.HitInfo.Value.HkHitInfo.GetHitEntity() is MyVoxelBase)
					{
						m_hitPos = MyCubeBuilder.Static.HitInfo.Value.Position;
						m_closestHitDistSq = (float)(m_hitPos - pasteMatrix.Translation).LengthSquared();
						m_hitNormal = addDir;
						m_hitEntity = (MyCubeBuilder.Static.HitInfo.Value.HkHitInfo.GetHitEntity() as MyVoxelBase);
						m_visible = true;
					}
					else
					{
						m_visible = false;
					}
				}
				else
				{
					m_visible = false;
				}
			}
			else
			{
				m_visible = false;
			}
		}

		private new void FixSnapTransformationBase6()
		{
			if (base.CopiedGrids.Count == 0)
			{
				return;
			}
			MyCubeGrid myCubeGrid = m_hitEntity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			Matrix rotationDeltaMatrixToHitGrid = GetRotationDeltaMatrixToHitGrid(myCubeGrid);
			foreach (MyCubeGrid previewGrid in base.PreviewGrids)
			{
				Matrix matrix = previewGrid.WorldMatrix.GetOrientation();
				matrix *= rotationDeltaMatrixToHitGrid;
				MatrixD worldMatrix = MatrixD.CreateWorld(m_pastePosition, matrix.Forward, matrix.Up);
				previewGrid.PositionComp.SetWorldMatrix(worldMatrix);
			}
			if (myCubeGrid.GridSizeEnum == MyCubeSize.Large && base.PreviewGrids[0].GridSizeEnum == MyCubeSize.Small)
			{
				Vector3 v = MyCubeBuilder.TransformLargeGridHitCoordToSmallGrid(m_hitPos, myCubeGrid.PositionComp.WorldMatrixNormalizedInv, myCubeGrid.GridSize);
				m_pastePosition = myCubeGrid.GridIntegerToWorld(v);
			}
			else
			{
				Vector3I vector = Vector3I.Round(m_hitNormal);
				Vector3I vector3I = myCubeGrid.WorldToGridInteger(m_pastePosition);
				Vector3I min = base.PreviewGrids[0].Min;
				Vector3I vector2 = Vector3I.Abs(Vector3I.Round(Vector3D.TransformNormal(Vector3D.TransformNormal((Vector3D)(base.PreviewGrids[0].Max - min + Vector3I.One), base.PreviewGrids[0].WorldMatrix), myCubeGrid.PositionComp.WorldMatrixNormalizedInv)));
				int num = Math.Abs(Vector3I.Dot(ref vector, ref vector2));
				int i;
				for (i = 0; i < num; i++)
				{
					if (myCubeGrid.CanMergeCubes(base.PreviewGrids[0], vector3I))
					{
						break;
					}
					vector3I += vector;
				}
				if (i == num)
				{
					vector3I = myCubeGrid.WorldToGridInteger(m_pastePosition);
				}
				m_pastePosition = myCubeGrid.GridIntegerToWorld(vector3I);
			}
			for (int j = 0; j < base.PreviewGrids.Count; j++)
			{
				MyCubeGrid myCubeGrid2 = base.PreviewGrids[j];
				MatrixD worldMatrix2 = myCubeGrid2.WorldMatrix;
				worldMatrix2.Translation = m_pastePosition + Vector3.Transform(m_copiedGridOffsets[j], rotationDeltaMatrixToHitGrid);
				myCubeGrid2.PositionComp.SetWorldMatrix(worldMatrix2);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawLine3D(m_hitPos, m_hitPos + m_hitNormal, Color.Red, Color.Green, depthRead: false);
			}
		}

		public Matrix GetRotationDeltaMatrixToHitGrid(MyCubeGrid hitGrid)
		{
			Matrix axisDefinitionMatrix = hitGrid.WorldMatrix.GetOrientation();
			Matrix toAlign = base.PreviewGrids[0].WorldMatrix.GetOrientation();
			Matrix matrix = Matrix.AlignRotationToAxes(ref toAlign, ref axisDefinitionMatrix);
			return Matrix.Invert(toAlign) * matrix;
		}

		private new bool TestPlacement()
		{
			bool flag = true;
			m_touchingGrids.Clear();
			for (int i = 0; i < base.PreviewGrids.Count; i++)
			{
				MyCubeGrid myCubeGrid = base.PreviewGrids[i];
				if (!MyEntities.IsInsideWorld(myCubeGrid.PositionComp.GetPosition()))
				{
					return false;
				}
				MyGridPlacementSettings settings = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum);
				m_touchingGrids.Add(null);
				if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					if (i == 0 && MyBlockBuilderBase.CameraControllerSpectator)
					{
						m_visible = false;
						return false;
					}
					if (i == 0 && !MyCubeBuilder.Static.DynamicMode)
					{
						MatrixD invGridWorldMatrix = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
						if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, myCubeGrid.PositionComp.LocalAABB, myCubeGrid.GridSize, MyBlockBuilderBase.IntersectionDistance))
						{
							m_visible = false;
							return false;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (MyCubeBuilder.Static.DynamicMode)
				{
					MyGridPlacementSettings settings2 = (myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? m_settings.LargeGrid : m_settings.SmallGrid;
					bool flag2 = false;
					foreach (MySlimBlock block in myCubeGrid.GetBlocks())
					{
						Vector3 v = block.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
						Vector3 v2 = block.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
						BoundingBoxD localAabb = new BoundingBoxD(v, v2);
						if (!flag2)
						{
							flag2 = MyGridClipboardAdvanced.TestVoxelPlacement(block, ref settings, dynamicMode: true);
						}
						flag = (flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings2, localAabb, dynamicBuildMode: true, null, testVoxel: false));
						if (!flag)
						{
							break;
						}
					}
					flag = (flag && flag2);
				}
				else if (i == 0 && m_hitEntity is MyCubeGrid && base.IsSnapped)
				{
					MyCubeGrid myCubeGrid2 = m_hitEntity as MyCubeGrid;
					MyGridPlacementSettings settings3 = m_settings.GetGridPlacementSettings(myCubeGrid2.GridSizeEnum, myCubeGrid2.IsStatic);
					flag = ((myCubeGrid2.GridSizeEnum != 0 || myCubeGrid.GridSizeEnum != MyCubeSize.Small) ? (flag && TestGridPlacementOnGrid(myCubeGrid, ref settings3, myCubeGrid2)) : (flag && MyCubeGrid.TestPlacementArea(myCubeGrid, ref settings, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false)));
					m_touchingGrids.Clear();
					m_touchingGrids.Add(myCubeGrid2);
				}
				else if (i == 0 && m_hitEntity is MyVoxelMap)
				{
					bool flag3 = false;
					foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
					{
						if (cubeBlock.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block2 in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								if (!flag3)
								{
									flag3 = MyGridClipboardAdvanced.TestVoxelPlacement(block2, ref settings, dynamicMode: false);
								}
								flag = (flag && MyGridClipboardAdvanced.TestBlockPlacementArea(block2, ref settings, dynamicMode: false, testVoxel: false));
								if (!flag)
								{
									break;
								}
							}
						}
						else
						{
							if (!flag3)
							{
								flag3 = MyGridClipboardAdvanced.TestVoxelPlacement(cubeBlock, ref settings, dynamicMode: false);
							}
							flag = (flag && MyGridClipboardAdvanced.TestBlockPlacementArea(cubeBlock, ref settings, dynamicMode: false, testVoxel: false));
						}
						if (!flag)
						{
							break;
						}
					}
					flag = (flag && flag3);
					m_touchingGrids[i] = DetectTouchingGrid();
				}
				else
				{
					MyGridPlacementSettings settings4 = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic && !MyCubeBuilder.Static.DynamicMode);
					flag = (flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings4, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false));
				}
				BoundingBoxD boundingBoxD = myCubeGrid.PositionComp.LocalAABB;
				MatrixD worldMatrixNormalizedInv = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
				if (MySector.MainCamera != null)
				{
					Vector3D point = Vector3D.Transform(MySector.MainCamera.Position, worldMatrixNormalizedInv);
					flag = (flag && boundingBoxD.Contains(point) != ContainmentType.Contains);
				}
				if (flag)
				{
					m_tmpCollisionPoints.Clear();
					MyCubeBuilder.PrepareCharacterCollisionPoints(m_tmpCollisionPoints);
					foreach (Vector3D tmpCollisionPoint in m_tmpCollisionPoints)
					{
						Vector3D point2 = Vector3D.Transform(tmpCollisionPoint, worldMatrixNormalizedInv);
						flag = (flag && boundingBoxD.Contains(point2) != ContainmentType.Contains);
						if (!flag)
						{
							break;
						}
					}
				}
			}
			return flag;
		}

		private MyCubeGrid DetectTouchingGrid()
		{
			if (base.PreviewGrids == null || base.PreviewGrids.Count == 0)
			{
				return null;
			}
			foreach (MySlimBlock cubeBlock in base.PreviewGrids[0].CubeBlocks)
			{
				MyCubeGrid myCubeGrid = DetectTouchingGrid(cubeBlock);
				if (myCubeGrid != null)
				{
					return myCubeGrid;
				}
			}
			return null;
		}

		private MyCubeGrid DetectTouchingGrid(MySlimBlock block)
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				return null;
			}
			if (block == null)
			{
				return null;
			}
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					MyCubeGrid myCubeGrid = DetectTouchingGrid(block2);
					if (myCubeGrid != null)
					{
						return myCubeGrid;
					}
				}
				return null;
			}
			float gridSize = block.CubeGrid.GridSize;
			block.GetWorldBoundingBox(out BoundingBoxD aabb);
			aabb.Inflate(gridSize / 2f);
			m_tmpNearEntities.Clear();
			MyEntities.GetElementsInBox(ref aabb, m_tmpNearEntities);
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
			try
			{
				for (int i = 0; i < m_tmpNearEntities.Count; i++)
				{
					MyCubeGrid myCubeGrid2 = m_tmpNearEntities[i] as MyCubeGrid;
					if (myCubeGrid2 != null && myCubeGrid2 != block.CubeGrid && myCubeGrid2.Physics != null && myCubeGrid2.Physics.Enabled && myCubeGrid2.IsStatic && myCubeGrid2.GridSizeEnum == block.CubeGrid.GridSizeEnum)
					{
						Vector3I gridOffset = myCubeGrid2.WorldToGridInteger(m_pastePosition);
						if (myCubeGrid2.CanMergeCubes(block.CubeGrid, gridOffset))
						{
							MatrixI transformation = myCubeGrid2.CalculateMergeTransform(block.CubeGrid, gridOffset);
							Base6Directions.Direction direction = transformation.GetDirection(block.Orientation.Forward);
							Base6Directions.Direction direction2 = transformation.GetDirection(block.Orientation.Up);
							new MyBlockOrientation(direction, direction2).GetQuaternion(out Quaternion result);
							Vector3I position = Vector3I.Transform(block.Position, transformation);
							if (MyCubeGrid.CheckConnectivity(myCubeGrid2, block.BlockDefinition, buildProgressModelMountPoints, ref result, ref position))
							{
								return myCubeGrid2;
							}
						}
					}
				}
			}
			finally
			{
				m_tmpNearEntities.Clear();
			}
			return null;
		}

		private void UpdatePreview()
		{
			if (base.PreviewGrids != null && m_visible && HasPreviewBBox)
			{
				MyStringId value = m_canBePlaced ? MyGridClipboard.ID_GIZMO_DRAW_LINE : MyGridClipboard.ID_GIZMO_DRAW_LINE_RED;
				Color color = Color.White;
				foreach (MyCubeGrid previewGrid in base.PreviewGrids)
				{
					BoundingBoxD localbox = previewGrid.PositionComp.LocalAABB;
					MatrixD worldMatrix = previewGrid.PositionComp.WorldMatrix;
					MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value);
				}
				Vector4 v = new Vector4(Color.Red.ToVector3() * 0.8f, 1f);
				if (RemoveBlocksInMultiBlock.Count > 0)
				{
					m_tmpBlockPositionsSet.Clear();
					MyCubeBuilder.GetAllBlocksPositions(RemoveBlocksInMultiBlock, m_tmpBlockPositionsSet);
					foreach (Vector3I item in m_tmpBlockPositionsSet)
					{
						MyCubeBuilder.DrawSemiTransparentBox(item, item, RemoveBlock.CubeGrid, v, onlyWireframe: false, MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
					}
					m_tmpBlockPositionsSet.Clear();
				}
				else if (RemoveBlock != null)
				{
					MyCubeBuilder.DrawSemiTransparentBox(RemoveBlock.CubeGrid, RemoveBlock, v, onlyWireframe: false, MyGridClipboard.ID_GIZMO_DRAW_LINE_RED);
				}
			}
		}

		protected override void SetupDragDistance()
		{
			if (base.IsActive)
			{
				base.SetupDragDistance();
				if (MySession.Static.SurvivalMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					m_dragDistance = MyBlockBuilderBase.IntersectionDistance;
				}
			}
		}

		public void SetGridFromBuilder(MyMultiBlockDefinition multiBlockDefinition, MyObjectBuilder_CubeGrid grid, Vector3 dragPointDelta, float dragVectorLength)
		{
			ChangeClipboardPreview(visible: false, m_previewGrids, m_copiedGrids);
			m_multiBlockDefinition = multiBlockDefinition;
			SetGridFromBuilder(grid, dragPointDelta, dragVectorLength);
			ChangeClipboardPreview(visible: true, m_previewGrids, m_copiedGrids);
		}

		public static void TakeMaterialsFromBuilder(List<MyObjectBuilder_CubeGrid> blocksToBuild, MyEntity builder)
		{
			if (blocksToBuild.Count == 0)
			{
				return;
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = blocksToBuild[0].CubeBlocks.FirstOrDefault();
			if (myObjectBuilder_CubeBlock == null)
			{
				return;
			}
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = myObjectBuilder_CubeBlock as MyObjectBuilder_CompoundCubeBlock;
			MyDefinitionId id;
			if (myObjectBuilder_CompoundCubeBlock != null)
			{
				if (myObjectBuilder_CompoundCubeBlock.Blocks == null || myObjectBuilder_CompoundCubeBlock.Blocks.Length == 0 || !myObjectBuilder_CompoundCubeBlock.Blocks[0].MultiBlockDefinition.HasValue)
				{
					return;
				}
				id = myObjectBuilder_CompoundCubeBlock.Blocks[0].MultiBlockDefinition.Value;
			}
			else
			{
				if (!myObjectBuilder_CubeBlock.MultiBlockDefinition.HasValue)
				{
					return;
				}
				id = myObjectBuilder_CubeBlock.MultiBlockDefinition.Value;
			}
			MyMultiBlockDefinition myMultiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(id);
			if (myMultiBlockDefinition != null)
			{
				MyCubeBuilder.BuildComponent.GetMultiBlockPlacementMaterials(myMultiBlockDefinition);
				MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builder, instantBuild: false);
			}
		}
	}
}
