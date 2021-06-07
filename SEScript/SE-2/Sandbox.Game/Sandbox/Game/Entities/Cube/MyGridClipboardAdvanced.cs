using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridClipboardAdvanced : MyGridClipboard
	{
		private static List<Vector3D> m_tmpCollisionPoints = new List<Vector3D>();

		protected bool m_dynamicBuildAllowed;

		protected override bool AnyCopiedGridIsStatic => false;

		public MyGridClipboardAdvanced(MyPlacementSettings settings, bool calculateVelocity = true)
			: base(settings, calculateVelocity)
		{
			m_useDynamicPreviews = false;
			m_dragDistance = 0f;
		}

		public override void Update()
		{
			if (!base.IsActive || !m_visible)
			{
				return;
			}
			bool flag = UpdateHitEntity(canPasteLargeOnSmall: false);
			if (MyFakes.ENABLE_VR_BUILDING && !flag)
			{
				Hide();
				return;
			}
			if (!m_visible)
			{
				Hide();
				return;
			}
			Show();
			if (m_dragDistance == 0f)
			{
				SetupDragDistance();
			}
			UpdatePastePosition();
			UpdateGridTransformations();
			if (MyCubeBuilder.Static.CubePlacementMode != MyCubeBuilder.CubePlacementModeEnum.FreePlacement)
			{
				FixSnapTransformationBase6();
			}
			if (m_calculateVelocity)
			{
				m_objectVelocity = (m_pastePosition - m_pastePositionPrevious) / 0.01666666753590107;
			}
			m_canBePlaced = TestPlacement();
			TestBuildingMaterials();
			UpdatePreview();
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "FW: " + m_pasteDirForward.ToString(), Color.Red, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 20f), "UP: " + m_pasteDirUp.ToString(), Color.Red, 1f);
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 40f), "AN: " + m_pasteOrientationAngle, Color.Red, 1f);
			}
		}

		public override void Activate(Action callback = null)
		{
			base.Activate(callback);
			SetupDragDistance();
		}

		private static void ConvertGridBuilderToStatic(MyObjectBuilder_CubeGrid originalGrid, MatrixD worldMatrix)
		{
			originalGrid.IsStatic = true;
			originalGrid.PositionAndOrientation = new MyPositionAndOrientation(originalGrid.PositionAndOrientation.Value.Position, Vector3.Forward, Vector3.Up);
			Vector3 vec = worldMatrix.Forward;
			Vector3 vec2 = worldMatrix.Up;
			Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(vec);
			Base6Directions.Direction direction = Base6Directions.GetClosestDirection(vec2);
			if (direction == closestDirection)
			{
				direction = Base6Directions.GetPerpendicular(closestDirection);
			}
			MatrixI transform = new MatrixI(Vector3I.Zero, closestDirection, direction);
			foreach (MyObjectBuilder_CubeBlock cubeBlock in originalGrid.CubeBlocks)
			{
				if (cubeBlock is MyObjectBuilder_CompoundCubeBlock)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
					ConvertRotatedGridCompoundBlockToStatic(ref transform, myObjectBuilder_CompoundCubeBlock);
					for (int i = 0; i < myObjectBuilder_CompoundCubeBlock.Blocks.Length; i++)
					{
						MyObjectBuilder_CubeBlock origBlock = myObjectBuilder_CompoundCubeBlock.Blocks[i];
						ConvertRotatedGridBlockToStatic(ref transform, origBlock);
					}
				}
				else
				{
					ConvertRotatedGridBlockToStatic(ref transform, cubeBlock);
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
			bool flag = m_hitEntity is MyCubeGrid && !((MyCubeGrid)m_hitEntity).IsStatic && !MyCubeBuilder.Static.DynamicMode;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				return PasteGridsInDynamicMode(deactivate);
			}
			if (flag)
			{
				return PasteGridInternal(deactivate);
			}
			if (MyFakes.ENABLE_VR_BUILDING)
			{
				return PasteGridInternal(deactivate);
			}
			return PasteGridsInStaticMode(deactivate);
		}

		private bool PasteGridsInDynamicMode(bool deactivate)
		{
			List<bool> list = new List<bool>();
			foreach (MyObjectBuilder_CubeGrid copiedGrid in base.CopiedGrids)
			{
				list.Add(copiedGrid.IsStatic);
				copiedGrid.IsStatic = false;
			}
			bool result = PasteGridInternal(deactivate);
			for (int i = 0; i < base.CopiedGrids.Count; i++)
			{
				base.CopiedGrids[i].IsStatic = list[i];
			}
			return result;
		}

		private bool PasteGridsInStaticMode(bool deactivate)
		{
			MyObjectBuilder_CubeGrid originalGrid = base.CopiedGrids[0];
			MatrixD worldMatrix = base.PreviewGrids[0].WorldMatrix;
			ConvertGridBuilderToStatic(originalGrid, worldMatrix);
			base.PreviewGrids[0].WorldMatrix = MatrixD.CreateTranslation(worldMatrix.Translation);
			for (int i = 1; i < base.CopiedGrids.Count; i++)
			{
				if (base.CopiedGrids[i].IsStatic)
				{
					MyObjectBuilder_CubeGrid originalGrid2 = base.CopiedGrids[i];
					MatrixD worldMatrix2 = base.PreviewGrids[i].WorldMatrix;
					ConvertGridBuilderToStatic(originalGrid2, worldMatrix2);
					base.PreviewGrids[i].WorldMatrix = MatrixD.CreateTranslation(worldMatrix2.Translation);
				}
			}
			List<MyObjectBuilder_CubeGrid> pastedBuilders = new List<MyObjectBuilder_CubeGrid>();
			bool num = PasteGridInternal(deactivate: true, pastedBuilders, m_touchingGrids, delegate(List<MyObjectBuilder_CubeGrid> pastedBuildersInCallback)
			{
				UpdateAfterPaste(deactivate, pastedBuildersInCallback);
			});
			if (num)
			{
				UpdateAfterPaste(deactivate, pastedBuilders);
			}
			return num;
		}

		private void UpdateAfterPaste(bool deactivate, List<MyObjectBuilder_CubeGrid> pastedBuilders)
		{
			if (base.CopiedGrids.Count == pastedBuilders.Count)
			{
				m_copiedGridOffsets.Clear();
				for (int i = 0; i < base.CopiedGrids.Count; i++)
				{
					base.CopiedGrids[i].PositionAndOrientation = pastedBuilders[i].PositionAndOrientation;
					m_copiedGridOffsets.Add((Vector3D)base.CopiedGrids[i].PositionAndOrientation.Value.Position - (Vector3D)base.CopiedGrids[0].PositionAndOrientation.Value.Position);
				}
				m_pasteOrientationAngle = 0f;
				m_pasteDirForward = Vector3I.Forward;
				m_pasteDirUp = Vector3I.Up;
				if (!deactivate)
				{
					Activate();
				}
			}
		}

		private static void ConvertRotatedGridBlockToStatic(ref MatrixI transform, MyObjectBuilder_CubeBlock origBlock)
		{
			MyDefinitionId defId = new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out MyCubeBlockDefinition blockDefinition);
			if (blockDefinition != null)
			{
				MyBlockOrientation orientation = origBlock.BlockOrientation;
				Vector3I min = origBlock.Min;
				MySlimBlock.ComputeMax(blockDefinition, orientation, ref min, out Vector3I max);
				Vector3I.Transform(ref min, ref transform, out Vector3I result);
				Vector3I.Transform(ref max, ref transform, out Vector3I result2);
				Base6Directions.Direction direction = transform.GetDirection(orientation.Forward);
				Base6Directions.Direction direction2 = transform.GetDirection(orientation.Up);
				new MyBlockOrientation(direction, direction2).GetQuaternion(out Quaternion result3);
				origBlock.Orientation = result3;
				origBlock.Min = Vector3I.Min(result, result2);
			}
		}

		private static void ConvertRotatedGridCompoundBlockToStatic(ref MatrixI transform, MyObjectBuilder_CompoundCubeBlock origBlock)
		{
			MyDefinitionId defId = new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out MyCubeBlockDefinition blockDefinition);
			if (blockDefinition != null)
			{
				MyBlockOrientation orientation = origBlock.BlockOrientation;
				Vector3I min = origBlock.Min;
				MySlimBlock.ComputeMax(blockDefinition, orientation, ref min, out Vector3I max);
				Vector3I.Transform(ref min, ref transform, out Vector3I result);
				Vector3I.Transform(ref max, ref transform, out Vector3I result2);
				origBlock.Min = Vector3I.Min(result, result2);
			}
		}

		protected override void UpdatePastePosition()
		{
			m_pastePositionPrevious = m_pastePosition;
			if (MyCubeBuilder.Static.DynamicMode)
			{
				m_visible = true;
				base.IsSnapped = false;
				m_pastePosition = MyBlockBuilderBase.IntersectionStart + m_dragDistance * MyBlockBuilderBase.IntersectionDirection;
				Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
				Vector3D vector3D = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix);
				m_pastePosition += vector3D;
				return;
			}
			m_visible = true;
			if (!base.IsSnapped)
			{
				m_pasteOrientationAngle = 0f;
				m_pasteDirForward = Vector3I.Forward;
				m_pasteDirUp = Vector3I.Up;
			}
			base.IsSnapped = true;
			MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
			Vector3 value = pasteMatrix.Forward * m_dragDistance;
			MyGridPlacementSettings gridPlacementSettings = m_settings.GetGridPlacementSettings(base.PreviewGrids[0].GridSizeEnum);
			if (!TrySnapToSurface(gridPlacementSettings.SnapMode))
			{
				m_pastePosition = pasteMatrix.Translation + value;
				Matrix firstGridOrientationMatrix2 = GetFirstGridOrientationMatrix();
				Vector3D vector3D2 = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix2);
				m_pastePosition += vector3D2;
				base.IsSnapped = true;
			}
			if (!MyFakes.ENABLE_VR_BUILDING)
			{
				double num = base.PreviewGrids[0].GridSize;
				if (m_settings.StaticGridAlignToCenter)
				{
					m_pastePosition = Vector3I.Round(m_pastePosition / num) * num;
				}
				else
				{
					m_pastePosition = Vector3I.Round(m_pastePosition / num + 0.5) * num - 0.5 * num;
				}
			}
		}

		public void SetDragDistance(float dragDistance)
		{
			m_dragDistance = dragDistance;
		}

		private static double DistanceFromCharacterPlane(ref Vector3D point)
		{
			return Vector3D.Dot(point - MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionDirection);
		}

		protected Vector3D? GetFreeSpacePlacementPosition(bool copyPaste, out bool buildAllowed)
		{
			Vector3D? result = null;
			buildAllowed = false;
			double num = double.MaxValue;
			double? currentRayIntersection = MyCubeBuilder.GetCurrentRayIntersection();
			if (currentRayIntersection.HasValue)
			{
				num = currentRayIntersection.Value;
			}
			Vector3D value = Vector3D.Zero;
			if (copyPaste)
			{
				Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
				value = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix);
			}
			Vector3D value2 = base.PreviewGrids[0].GridIntegerToWorld(Vector3I.Zero);
			foreach (MySlimBlock block in base.PreviewGrids[0].GetBlocks())
			{
				Vector3 halfExtents = block.BlockDefinition.Size * base.PreviewGrids[0].GridSize * 0.5f;
				Vector3 value3 = block.Min * base.PreviewGrids[0].GridSize - Vector3.Half * base.PreviewGrids[0].GridSize;
				Vector3 value4 = block.Max * base.PreviewGrids[0].GridSize + Vector3.Half * base.PreviewGrids[0].GridSize;
				block.Orientation.GetMatrix(out Matrix result2);
				result2.Translation = 0.5f * (value3 + value4);
				MatrixD matrixD = result2 * base.PreviewGrids[0].WorldMatrix;
				Vector3D value5 = matrixD.Translation + value - value2;
				HkShape shape = new HkBoxShape(halfExtents);
				Vector3D point = MyBlockBuilderBase.IntersectionStart + value5;
				double num2 = DistanceFromCharacterPlane(ref point);
				point -= num2 * MyBlockBuilderBase.IntersectionDirection;
				Vector3D vector3D = MyBlockBuilderBase.IntersectionStart + ((double)m_dragDistance - num2) * MyBlockBuilderBase.IntersectionDirection + value5;
				MatrixD transform = matrixD;
				transform.Translation = point;
				try
				{
					float? num3 = MyPhysics.CastShape(vector3D, shape, ref transform, 30);
					if (num3.HasValue && num3.Value != 0f)
					{
						Vector3D point2 = point + num3.Value * (vector3D - point);
						double num4 = DistanceFromCharacterPlane(ref point2) - num2;
						if (num4 <= 0.0)
						{
							num4 = 0.0;
							num = 0.0;
							goto IL_02a4;
						}
						if (num4 < num)
						{
							num = num4;
						}
						buildAllowed = true;
					}
				}
				finally
				{
					shape.RemoveReference();
				}
			}
			goto IL_02a4;
			IL_02a4:
			float num5 = (float)base.PreviewGrids[0].PositionComp.WorldAABB.HalfExtents.Length();
			float num6 = 1.5f * num5;
			if (num < (double)num6)
			{
				num = num6;
				buildAllowed = false;
			}
			if (num < (double)m_dragDistance)
			{
				result = MyBlockBuilderBase.IntersectionStart + num * MyBlockBuilderBase.IntersectionDirection;
			}
			return result;
		}

		protected Vector3D? GetFreeSpacePlacementPositionGridAabbs(bool copyPaste, out bool buildAllowed)
		{
			Vector3D? result = null;
			buildAllowed = true;
			_ = base.PreviewGrids[0].GridSize;
			double num = double.MaxValue;
			double? currentRayIntersection = MyCubeBuilder.GetCurrentRayIntersection();
			if (currentRayIntersection.HasValue)
			{
				num = currentRayIntersection.Value;
			}
			Vector3D value = Vector3D.Zero;
			if (copyPaste)
			{
				Matrix firstGridOrientationMatrix = GetFirstGridOrientationMatrix();
				value = Vector3.TransformNormal(m_dragPointToPositionLocal, firstGridOrientationMatrix);
			}
			Vector3D value2 = base.PreviewGrids[0].GridIntegerToWorld(Vector3I.Zero);
			foreach (MyCubeGrid previewGrid in base.PreviewGrids)
			{
				Vector3 halfExtents = previewGrid.PositionComp.LocalAABB.HalfExtents;
				Vector3 value3 = previewGrid.Min * previewGrid.GridSize - Vector3.Half * previewGrid.GridSize;
				Vector3 value4 = previewGrid.Max * previewGrid.GridSize + Vector3.Half * previewGrid.GridSize;
				MatrixD identity = MatrixD.Identity;
				identity.Translation = 0.5f * (value3 + value4);
				identity *= previewGrid.WorldMatrix;
				Vector3.TransformNormal((Vector3)(Vector3I.Abs((previewGrid.Max - previewGrid.Min + Vector3I.One) % 2 - Vector3I.One) * 0.5 * previewGrid.GridSize), previewGrid.WorldMatrix);
				Vector3D value5 = identity.Translation + value - value2;
				HkShape shape = new HkBoxShape(halfExtents);
				Vector3D point = MyBlockBuilderBase.IntersectionStart + value5;
				double num2 = DistanceFromCharacterPlane(ref point);
				point -= num2 * MyBlockBuilderBase.IntersectionDirection;
				Vector3D vector3D = MyBlockBuilderBase.IntersectionStart + ((double)m_dragDistance - num2) * MyBlockBuilderBase.IntersectionDirection + value5;
				MatrixD transform = identity;
				transform.Translation = point;
				try
				{
					float? num3 = MyPhysics.CastShape(vector3D, shape, ref transform, 30);
					if (!num3.HasValue || num3.Value == 0f)
					{
						buildAllowed = false;
						continue;
					}
					Vector3D point2 = point + num3.Value * (vector3D - point);
					Color color = Color.Green;
					BoundingBoxD localbox = new BoundingBoxD(-halfExtents, halfExtents);
					localbox.Inflate(0.029999999329447746);
					MatrixD worldMatrix = transform;
					worldMatrix.Translation = point2;
					MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f);
					double num4 = DistanceFromCharacterPlane(ref point2) - num2;
					if (!(num4 <= 0.0))
					{
						if (num4 < num)
						{
							num = num4;
						}
						continue;
					}
					num4 = 0.0;
					num = 0.0;
					buildAllowed = false;
				}
				finally
				{
					shape.RemoveReference();
				}
				break;
			}
			if (num != 0.0 && num < (double)m_dragDistance)
			{
				result = MyBlockBuilderBase.IntersectionStart + num * MyBlockBuilderBase.IntersectionDirection;
			}
			else
			{
				buildAllowed = false;
			}
			return result;
		}

		private new bool TestPlacement()
		{
			bool flag = true;
			m_touchingGrids.Clear();
			for (int i = 0; i < base.PreviewGrids.Count; i++)
			{
				MyCubeGrid myCubeGrid = base.PreviewGrids[i];
				m_touchingGrids.Add(null);
				if (MyCubeBuilder.Static.DynamicMode)
				{
					if (!m_dynamicBuildAllowed)
					{
						MyGridPlacementSettings settings = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, isStatic: false);
						BoundingBoxD localAabb = myCubeGrid.PositionComp.LocalAABB;
						MatrixD worldMatrix = myCubeGrid.WorldMatrix;
						if (MyFakes.ENABLE_VOXEL_MAP_AABB_CORNER_TEST)
						{
							flag = (flag && MyCubeGrid.TestPlacementVoxelMapOverlap(null, ref settings, ref localAabb, ref worldMatrix));
						}
						flag = (flag && MyCubeGrid.TestPlacementArea(myCubeGrid, targetGridIsStatic: false, ref settings, localAabb, dynamicBuildMode: true));
						if (!flag)
						{
							break;
						}
					}
				}
				else if (i == 0 && m_hitEntity is MyCubeGrid && base.IsSnapped && base.SnapMode == SnapMode.Base6Directions)
				{
					MyGridPlacementSettings settings2 = (myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
					MyCubeGrid myCubeGrid2 = m_hitEntity as MyCubeGrid;
					if (myCubeGrid2.GridSizeEnum == MyCubeSize.Small && myCubeGrid.GridSizeEnum == MyCubeSize.Large)
					{
						flag = false;
						break;
					}
					bool flag2 = myCubeGrid2.GridSizeEnum == MyCubeSize.Large && myCubeGrid.GridSizeEnum == MyCubeSize.Small;
					if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && flag2)
					{
						if (!myCubeGrid2.IsStatic)
						{
							flag = false;
							break;
						}
						foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
						{
							if (cubeBlock.FatBlock is MyCompoundCubeBlock)
							{
								foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
								{
									flag = (flag && TestBlockPlacement(block, ref settings2));
									if (!flag)
									{
										break;
									}
								}
							}
							else
							{
								flag = (flag && TestBlockPlacement(cubeBlock, ref settings2));
							}
							if (!flag)
							{
								break;
							}
						}
					}
					else
					{
						flag = (flag && TestGridPlacementOnGrid(myCubeGrid, ref settings2, myCubeGrid2));
					}
				}
				else
				{
					MyCubeGrid myCubeGrid3 = null;
					MyGridPlacementSettings settings3 = (i != 0) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum) : ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid);
					if (myCubeGrid.IsStatic)
					{
						if (i == 0)
						{
							Matrix gridLocalMatrix = myCubeGrid.WorldMatrix.GetOrientation();
							flag = (flag && MyCubeBuilder.CheckValidBlocksRotation(gridLocalMatrix, myCubeGrid));
						}
						foreach (MySlimBlock cubeBlock2 in myCubeGrid.CubeBlocks)
						{
							if (cubeBlock2.FatBlock is MyCompoundCubeBlock)
							{
								foreach (MySlimBlock block2 in (cubeBlock2.FatBlock as MyCompoundCubeBlock).GetBlocks())
								{
									MyCubeGrid touchingGrid = null;
									flag = (flag && TestBlockPlacementNoAABBInflate(block2, ref settings3, out touchingGrid));
									if (flag && touchingGrid != null && myCubeGrid3 == null)
									{
										myCubeGrid3 = touchingGrid;
									}
									if (!flag)
									{
										break;
									}
								}
							}
							else
							{
								MyCubeGrid touchingGrid2 = null;
								flag = (flag && TestBlockPlacementNoAABBInflate(cubeBlock2, ref settings3, out touchingGrid2));
								if (flag && touchingGrid2 != null && myCubeGrid3 == null)
								{
									myCubeGrid3 = touchingGrid2;
								}
							}
							if (!flag)
							{
								break;
							}
						}
						if (flag && myCubeGrid3 != null)
						{
							m_touchingGrids[i] = myCubeGrid3;
						}
					}
					else
					{
						foreach (MySlimBlock cubeBlock3 in myCubeGrid.CubeBlocks)
						{
							Vector3 v = cubeBlock3.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
							Vector3 v2 = cubeBlock3.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
							BoundingBoxD localAabb2 = new BoundingBoxD(v, v2);
							flag = (flag && MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings3, localAabb2, dynamicBuildMode: false));
							if (!flag)
							{
								break;
							}
						}
						m_touchingGrids[i] = null;
					}
					if (flag && m_touchingGrids[i] != null)
					{
						MyGridPlacementSettings settings4 = (myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
						flag = (flag && TestGridPlacementOnGrid(myCubeGrid, ref settings4, m_touchingGrids[i]));
					}
					if (flag && i == 0)
					{
						if ((myCubeGrid.GridSizeEnum == MyCubeSize.Small && myCubeGrid.IsStatic) || !myCubeGrid.IsStatic)
						{
							MyGridPlacementSettings settings5 = (i == 0) ? m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, isStatic: false) : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
							bool flag3 = true;
							foreach (MySlimBlock cubeBlock4 in myCubeGrid.CubeBlocks)
							{
								Vector3 v3 = cubeBlock4.Min * base.PreviewGrids[i].GridSize - Vector3.Half * base.PreviewGrids[i].GridSize;
								Vector3 v4 = cubeBlock4.Max * base.PreviewGrids[i].GridSize + Vector3.Half * base.PreviewGrids[i].GridSize;
								BoundingBoxD localAabb3 = new BoundingBoxD(v3, v4);
								flag3 = (flag3 && MyCubeGrid.TestPlacementArea(myCubeGrid, targetGridIsStatic: false, ref settings5, localAabb3, dynamicBuildMode: false));
								if (!flag3)
								{
									break;
								}
							}
							flag = (flag && !flag3);
						}
						else if (m_touchingGrids[i] == null)
						{
							MyGridPlacementSettings settings6 = m_settings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, i == 0 || myCubeGrid.IsStatic);
							MyCubeGrid touchingGrid3 = null;
							bool flag4 = false;
							foreach (MySlimBlock cubeBlock5 in myCubeGrid.CubeBlocks)
							{
								if (cubeBlock5.FatBlock is MyCompoundCubeBlock)
								{
									foreach (MySlimBlock block3 in (cubeBlock5.FatBlock as MyCompoundCubeBlock).GetBlocks())
									{
										flag4 |= TestBlockPlacementNoAABBInflate(block3, ref settings6, out touchingGrid3);
										if (flag4)
										{
											break;
										}
									}
								}
								else
								{
									flag4 |= TestBlockPlacementNoAABBInflate(cubeBlock5, ref settings6, out touchingGrid3);
								}
								if (flag4)
								{
									break;
								}
							}
							flag = (flag && flag4);
						}
					}
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
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		protected bool TestGridPlacementOnGrid(MyCubeGrid previewGrid, ref MyGridPlacementSettings settings, MyCubeGrid hitGrid)
		{
			bool flag = true;
			Vector3I gridOffset = hitGrid.WorldToGridInteger(previewGrid.PositionComp.WorldMatrix.Translation);
			MatrixI transform = hitGrid.CalculateMergeTransform(previewGrid, gridOffset);
			Matrix floatMatrix = transform.GetFloatMatrix();
			floatMatrix.Translation *= previewGrid.GridSize;
			if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 60f), "First grid offset: " + gridOffset.ToString(), Color.Red, 1f);
			}
			flag = (flag && MyCubeBuilder.CheckValidBlocksRotation(floatMatrix, previewGrid) && hitGrid.GridSizeEnum == previewGrid.GridSizeEnum && hitGrid.CanMergeCubes(previewGrid, gridOffset) && MyCubeGrid.CheckMergeConnectivity(hitGrid, previewGrid, gridOffset));
			if (flag)
			{
				bool flag2 = false;
				foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
				{
					if (cubeBlock.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							flag2 |= CheckConnectivityOnGrid(block, ref transform, ref settings, hitGrid);
							if (flag2)
							{
								break;
							}
						}
					}
					else
					{
						flag2 |= CheckConnectivityOnGrid(cubeBlock, ref transform, ref settings, hitGrid);
					}
					if (flag2)
					{
						break;
					}
				}
				flag = (flag && flag2);
			}
			if (flag)
			{
				foreach (MySlimBlock cubeBlock2 in previewGrid.CubeBlocks)
				{
					if (cubeBlock2.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block2 in (cubeBlock2.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							flag = (flag && TestBlockPlacementOnGrid(block2, ref transform, ref settings, hitGrid));
							if (!flag)
							{
								break;
							}
						}
					}
					else
					{
						flag = (flag && TestBlockPlacementOnGrid(cubeBlock2, ref transform, ref settings, hitGrid));
					}
					if (!flag)
					{
						return flag;
					}
				}
				return flag;
			}
			return flag;
		}

		protected static bool CheckConnectivityOnGrid(MySlimBlock block, ref MatrixI transform, ref MyGridPlacementSettings settings, MyCubeGrid hitGrid)
		{
			Vector3I.Transform(ref block.Position, ref transform, out Vector3I result);
			Base6Directions.Direction direction = transform.GetDirection(block.Orientation.Forward);
			Base6Directions.Direction direction2 = transform.GetDirection(block.Orientation.Up);
			new MyBlockOrientation(direction, direction2).GetQuaternion(out Quaternion result2);
			MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
			return MyCubeGrid.CheckConnectivity(hitGrid, blockDefinition, blockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio), ref result2, ref result);
		}

		protected static bool TestBlockPlacementOnGrid(MySlimBlock block, ref MatrixI transform, ref MyGridPlacementSettings settings, MyCubeGrid hitGrid)
		{
			Vector3I.Transform(ref block.Min, ref transform, out Vector3I result);
			Vector3I.Transform(ref block.Max, ref transform, out Vector3I result2);
			Vector3I min = Vector3I.Min(result, result2);
			Vector3I max = Vector3I.Max(result, result2);
			Base6Directions.Direction direction = transform.GetDirection(block.Orientation.Forward);
			Base6Directions.Direction direction2 = transform.GetDirection(block.Orientation.Up);
			MyBlockOrientation orientation = new MyBlockOrientation(direction, direction2);
			return hitGrid.CanPlaceBlock(min, max, orientation, block.BlockDefinition, ref settings, 0uL);
		}

		protected static bool TestBlockPlacement(MySlimBlock block, ref MyGridPlacementSettings settings)
		{
			return MyCubeGrid.TestPlacementAreaCube(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, 0uL, block.CubeGrid);
		}

		protected static bool TestBlockPlacement(MySlimBlock block, ref MyGridPlacementSettings settings, out MyCubeGrid touchingGrid)
		{
			return MyCubeGrid.TestPlacementAreaCube(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, 0uL, block.CubeGrid);
		}

		protected static bool TestBlockPlacementNoAABBInflate(MySlimBlock block, ref MyGridPlacementSettings settings, out MyCubeGrid touchingGrid)
		{
			return MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, 0uL, block.CubeGrid);
		}

		protected static bool TestVoxelPlacement(MySlimBlock block, ref MyGridPlacementSettings settings, bool dynamicMode)
		{
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f);
			localAabb.Include(block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			return MyCubeGrid.TestVoxelPlacement(block.BlockDefinition, settings, dynamicMode, block.CubeGrid.WorldMatrix, localAabb);
		}

		protected static bool TestBlockPlacementArea(MySlimBlock block, ref MyGridPlacementSettings settings, bool dynamicMode, bool testVoxel = true)
		{
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f);
			localAabb.Include(block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			return MyCubeGrid.TestBlockPlacementArea(block.BlockDefinition, block.Orientation, block.CubeGrid.WorldMatrix, ref settings, localAabb, dynamicMode, block.CubeGrid, testVoxel);
		}

		private void UpdatePreview()
		{
			if (base.PreviewGrids != null && m_visible && HasPreviewBBox)
			{
				MyStringId value = m_canBePlaced ? MyGridClipboard.ID_GIZMO_DRAW_LINE : MyGridClipboard.ID_GIZMO_DRAW_LINE_RED;
				if (!MyFakes.ENABLE_VR_BUILDING || !m_canBePlaced)
				{
					Color color = Color.White;
					foreach (MyCubeGrid previewGrid in base.PreviewGrids)
					{
						BoundingBoxD localbox = previewGrid.PositionComp.LocalAABB;
						MatrixD worldMatrix = previewGrid.PositionComp.WorldMatrix;
						MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value);
					}
				}
			}
		}

		internal void DynamicModeChanged()
		{
			if (MyCubeBuilder.Static.DynamicMode)
			{
				SetupDragDistance();
			}
		}

		protected virtual void SetupDragDistance()
		{
			if (!base.IsActive)
			{
				return;
			}
			if (base.PreviewGrids.Count > 0)
			{
				double? currentRayIntersection = MyCubeBuilder.GetCurrentRayIntersection();
				if (currentRayIntersection.HasValue && (double)m_dragDistance > currentRayIntersection.Value)
				{
					m_dragDistance = (float)currentRayIntersection.Value;
				}
				float num = (float)base.PreviewGrids[0].PositionComp.WorldAABB.HalfExtents.Length();
				float num2 = 2.5f * num;
				if (m_dragDistance < num2)
				{
					m_dragDistance = num2;
				}
			}
			else
			{
				m_dragDistance = 0f;
			}
		}

		public override void MoveEntityCloser()
		{
			base.MoveEntityCloser();
			if (m_dragDistance < MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
			{
				m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
			}
		}
	}
}
