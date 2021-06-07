using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	public abstract class MyBlockBuilderBase : MySessionComponentBase
	{
		protected static readonly MyStringId[] m_rotationControls;

		protected static MyCubeBuilderDefinition m_cubeBuilderDefinition;

		private static float m_intersectionDistance;

		protected static readonly int[] m_rotationDirections;

		protected MyCubeGrid m_currentGrid;

		protected MatrixD m_invGridWorldMatrix = MatrixD.Identity;

		protected MyVoxelBase m_currentVoxelBase;

		protected MyPhysics.HitInfo? m_hitInfo;

		private static IMyPlacementProvider m_placementProvider;

		public static float IntersectionDistance
		{
			get
			{
				return m_intersectionDistance;
			}
			set
			{
				m_intersectionDistance = value;
				if (PlacementProvider != null)
				{
					PlacementProvider.IntersectionDistance = value;
				}
			}
		}

		protected internal abstract MyCubeGrid CurrentGrid
		{
			get;
			protected set;
		}

		protected internal abstract MyVoxelBase CurrentVoxelBase
		{
			get;
			protected set;
		}

		public abstract MyCubeBlockDefinition CurrentBlockDefinition
		{
			get;
			protected set;
		}

		public MyPhysics.HitInfo? HitInfo => m_hitInfo;

		private static bool AdminSpectatorIsBuilding
		{
			get
			{
				if (MyFakes.ENABLE_ADMIN_SPECTATOR_BUILDING && MySession.Static != null && MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator && MyMultiplayer.Static != null)
				{
					return MySession.Static.IsUserAdmin(Sync.MyId);
				}
				return false;
			}
		}

		private static bool DeveloperSpectatorIsBuilding
		{
			get
			{
				if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator)
				{
					if (MySession.Static.SurvivalMode)
					{
						return MyInput.Static.ENABLE_DEVELOPER_KEYS;
					}
					return true;
				}
				return false;
			}
		}

		public static bool SpectatorIsBuilding
		{
			get
			{
				if (!DeveloperSpectatorIsBuilding)
				{
					return AdminSpectatorIsBuilding;
				}
				return true;
			}
		}

		public static bool CameraControllerSpectator
		{
			get
			{
				MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
				if (cameraControllerEnum != 0 && cameraControllerEnum != MyCameraControllerEnum.SpectatorDelta)
				{
					return cameraControllerEnum == MyCameraControllerEnum.SpectatorOrbit;
				}
				return true;
			}
		}

		public static Vector3D IntersectionStart => PlacementProvider.RayStart;

		public static Vector3D IntersectionDirection => PlacementProvider.RayDirection;

		public Vector3D FreePlacementTarget => IntersectionStart + IntersectionDirection * IntersectionDistance;

		public static IMyPlacementProvider PlacementProvider
		{
			get
			{
				return m_placementProvider;
			}
			set
			{
				m_placementProvider = (value ?? new MyDefaultPlacementProvider(IntersectionDistance));
			}
		}

		public static MyCubeBuilderDefinition CubeBuilderDefinition => m_cubeBuilderDefinition;

		public abstract bool IsActivated
		{
			get;
		}

		static MyBlockBuilderBase()
		{
			m_rotationControls = new MyStringId[6]
			{
				MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE,
				MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE,
				MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE,
				MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE,
				MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE,
				MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE
			};
			m_rotationDirections = new int[6]
			{
				-1,
				1,
				1,
				-1,
				1,
				-1
			};
			PlacementProvider = new MyDefaultPlacementProvider(IntersectionDistance);
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
			m_cubeBuilderDefinition = (definition as MyCubeBuilderDefinition);
			_ = m_cubeBuilderDefinition;
			IntersectionDistance = m_cubeBuilderDefinition.DefaultBlockBuildingDistance;
		}

		public abstract void Activate(MyDefinitionId? blockDefinitionId = null);

		public abstract void Deactivate();

		protected internal virtual void ChooseHitObject()
		{
			FindClosestPlacementObject(out MyCubeGrid closestGrid, out MyVoxelBase closestVoxelMap);
			CurrentGrid = closestGrid;
			CurrentVoxelBase = closestVoxelMap;
			m_invGridWorldMatrix = ((CurrentGrid != null) ? CurrentGrid.PositionComp.WorldMatrixInvScaled : MatrixD.Identity);
		}

		protected static void AddFastBuildModelWithSubparts(ref MatrixD matrix, List<MatrixD> matrices, List<string> models, MyCubeBlockDefinition blockDefinition, float gridScale)
		{
			if (string.IsNullOrEmpty(blockDefinition.Model))
			{
				return;
			}
			matrices.Add(matrix);
			models.Add(blockDefinition.Model);
			MyEntitySubpart.Data outData = default(MyEntitySubpart.Data);
			MyModel modelOnlyData = MyModels.GetModelOnlyData(blockDefinition.Model);
			modelOnlyData.Rescale(gridScale);
			foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyData.Dummies)
			{
				MyCubeBlockDefinition subBlockDefinition;
				MatrixD subBlockMatrix;
				Vector3 dummyPosition;
				if (MyEntitySubpart.GetSubpartFromDummy(blockDefinition.Model, dummy.Key, dummy.Value, ref outData))
				{
					MyModels.GetModelOnlyData(outData.File)?.Rescale(gridScale);
					MatrixD item = MatrixD.Multiply(outData.InitialTransform, matrix);
					matrices.Add(item);
					models.Add(outData.File);
				}
				else if (MyFakes.ENABLE_SUBBLOCKS && MyCubeBlock.GetSubBlockDataFromDummy(blockDefinition, dummy.Key, dummy.Value, useOffset: false, out subBlockDefinition, out subBlockMatrix, out dummyPosition) && !string.IsNullOrEmpty(subBlockDefinition.Model))
				{
					MyModels.GetModelOnlyData(subBlockDefinition.Model)?.Rescale(gridScale);
					Vector3I vector = Vector3I.Round(Vector3.DominantAxisProjection(subBlockMatrix.Forward));
					Vector3I b = Vector3I.One - Vector3I.Abs(vector);
					Vector3I vector2 = Vector3I.Round(Vector3.DominantAxisProjection((Vector3)subBlockMatrix.Right * b));
					Vector3I.Cross(ref vector2, ref vector, out Vector3I result);
					subBlockMatrix.Forward = vector;
					subBlockMatrix.Right = vector2;
					subBlockMatrix.Up = result;
					MatrixD item2 = MatrixD.Multiply(subBlockMatrix, matrix);
					matrices.Add(item2);
					models.Add(subBlockDefinition.Model);
				}
			}
			if (!MyFakes.ENABLE_GENERATED_BLOCKS || blockDefinition.IsGeneratedBlock || blockDefinition.GeneratedBlockDefinitions == null)
			{
				return;
			}
			MyDefinitionId[] generatedBlockDefinitions = blockDefinition.GeneratedBlockDefinitions;
			foreach (MyDefinitionId defId in generatedBlockDefinitions)
			{
				if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out MyCubeBlockDefinition blockDefinition2))
				{
					MyModels.GetModelOnlyData(blockDefinition2.Model)?.Rescale(gridScale);
				}
			}
		}

		public MyCubeGrid FindClosestGrid()
		{
			return PlacementProvider.ClosestGrid;
		}

		public bool FindClosestPlacementObject(out MyCubeGrid closestGrid, out MyVoxelBase closestVoxelMap)
		{
			closestGrid = null;
			closestVoxelMap = null;
			m_hitInfo = null;
			if (MySession.Static.ControlledEntity == null)
			{
				return false;
			}
			closestGrid = PlacementProvider.ClosestGrid;
			closestVoxelMap = PlacementProvider.ClosestVoxelMap;
			m_hitInfo = PlacementProvider.HitInfo;
			if (closestGrid == null)
			{
				return closestVoxelMap != null;
			}
			return true;
		}

		protected Vector3I? IntersectCubes(MyCubeGrid grid, out double distance)
		{
			distance = 3.4028234663852886E+38;
			LineD line = new LineD(IntersectionStart, IntersectionStart + IntersectionDirection * IntersectionDistance);
			Vector3I position = Vector3I.Zero;
			double distanceSquared = double.MaxValue;
			if (grid.GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared))
			{
				distance = Math.Sqrt(distanceSquared);
				return position;
			}
			return null;
		}

		protected Vector3D? IntersectExact(MyCubeGrid grid, ref MatrixD inverseGridWorldMatrix, out Vector3D intersection, out MySlimBlock intersectedBlock)
		{
			intersection = Vector3D.Zero;
			LineD line = new LineD(IntersectionStart, IntersectionStart + IntersectionDirection * IntersectionDistance);
			double distance;
			Vector3D? lineIntersectionExactAll = grid.GetLineIntersectionExactAll(ref line, out distance, out intersectedBlock);
			if (lineIntersectionExactAll.HasValue)
			{
				Vector3D value = Vector3D.Transform(IntersectionStart, inverseGridWorldMatrix);
				Vector3D value2 = Vector3D.Normalize(Vector3D.TransformNormal(IntersectionDirection, inverseGridWorldMatrix));
				intersection = value + distance * value2;
				intersection *= (double)(1f / grid.GridSize);
			}
			return lineIntersectionExactAll;
		}

		protected Vector3D? GetIntersectedBlockData(ref MatrixD inverseGridWorldMatrix, out Vector3D intersection, out MySlimBlock intersectedBlock, out ushort? compoundBlockId)
		{
			intersection = Vector3D.Zero;
			intersectedBlock = null;
			compoundBlockId = null;
			if (CurrentGrid == null)
			{
				return null;
			}
			double distanceSquared = double.MaxValue;
			Vector3D? vector3D = null;
			LineD line = new LineD(IntersectionStart, IntersectionStart + IntersectionDirection * IntersectionDistance);
			Vector3I position = Vector3I.Zero;
			if (!CurrentGrid.GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared, m_hitInfo.Value))
			{
				return null;
			}
			distanceSquared = Math.Sqrt(distanceSquared);
			vector3D = position;
			intersectedBlock = CurrentGrid.GetCubeBlock(position);
			if (intersectedBlock == null)
			{
				return null;
			}
			if (intersectedBlock.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = intersectedBlock.FatBlock as MyCompoundCubeBlock;
				ushort? num = null;
				if (myCompoundCubeBlock.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? _, out ushort blockId))
				{
					num = blockId;
				}
				else if (myCompoundCubeBlock.GetBlocksCount() == 1)
				{
					num = myCompoundCubeBlock.GetBlockId(myCompoundCubeBlock.GetBlocks()[0]);
				}
				compoundBlockId = num;
			}
			Vector3D value = Vector3D.Transform(IntersectionStart, inverseGridWorldMatrix);
			Vector3D value2 = Vector3D.Normalize(Vector3D.TransformNormal(IntersectionDirection, inverseGridWorldMatrix));
			intersection = value + distanceSquared * value2;
			intersection *= (double)(1f / CurrentGrid.GridSize);
			return vector3D;
		}

		protected void IntersectInflated(List<Vector3I> outHitPositions, MyCubeGrid grid)
		{
			float num = 2000f;
			Vector3I vector3I = new Vector3I(100, 100, 100);
			if (grid != null)
			{
				PlacementProvider.RayCastGridCells(grid, outHitPositions, vector3I, num);
				return;
			}
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
			MyCubeGrid.RayCastStaticCells(IntersectionStart, IntersectionStart + IntersectionDirection * num, outHitPositions, cubeSize, vector3I);
		}

		protected BoundingBoxD GetCubeBoundingBox(Vector3I cubePos)
		{
			Vector3D value = cubePos * CurrentGrid.GridSize;
			Vector3 value2 = new Vector3(0.06f, 0.06f, 0.06f);
			return new BoundingBoxD(value - new Vector3D(CurrentGrid.GridSize / 2f) - value2, value + new Vector3D(CurrentGrid.GridSize / 2f) + value2);
		}

		protected bool GetCubeAddAndRemovePositions(Vector3I intersectedCube, bool placingSmallGridOnLargeStatic, out Vector3I addPos, out Vector3I addDir, out Vector3I removePos)
		{
			bool result = false;
			addPos = default(Vector3I);
			addDir = default(Vector3I);
			removePos = default(Vector3I);
			MatrixD worldMatrixInvScaled = CurrentGrid.PositionComp.WorldMatrixInvScaled;
			addPos = intersectedCube;
			addDir = Vector3I.Forward;
			Vector3D vector3D = Vector3D.Transform(IntersectionStart, worldMatrixInvScaled);
			Vector3D vector3D2 = Vector3D.Normalize(Vector3D.TransformNormal(IntersectionDirection, worldMatrixInvScaled));
			RayD ray = new RayD(vector3D, vector3D2);
			for (int i = 0; i < 100; i++)
			{
				BoundingBoxD cubeBoundingBox = GetCubeBoundingBox(addPos);
				if (!placingSmallGridOnLargeStatic && cubeBoundingBox.Contains(vector3D) == ContainmentType.Contains)
				{
					break;
				}
				double? num = cubeBoundingBox.Intersects(ray);
				if (!num.HasValue)
				{
					break;
				}
				removePos = addPos;
				Vector3D value = vector3D + vector3D2 * num.Value;
				Vector3 value2 = removePos * CurrentGrid.GridSize;
				Vector3I vector3I = Vector3I.Sign(Vector3.DominantAxisProjection(value - value2));
				addPos = removePos + vector3I;
				addDir = vector3I;
				if (!CurrentGrid.CubeExists(addPos))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		protected bool GetBlockAddPosition(float gridSize, bool placingSmallGridOnLargeStatic, out MySlimBlock intersectedBlock, out Vector3D intersectedBlockPos, out Vector3D intersectExactPos, out Vector3I addPositionBlock, out Vector3I addDirectionBlock, out ushort? compoundBlockId)
		{
			intersectedBlock = null;
			intersectedBlockPos = default(Vector3D);
			intersectExactPos = default(Vector3);
			addDirectionBlock = default(Vector3I);
			addPositionBlock = default(Vector3I);
			compoundBlockId = null;
			if (CurrentVoxelBase != null)
			{
				Vector3I intVector = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(m_hitInfo.Value.HkHitInfo.Normal));
				double scaleFactor = IntersectionDistance * m_hitInfo.Value.HkHitInfo.HitFraction;
				Vector3D intersectionStart = IntersectionStart;
				Vector3D value = Vector3D.Normalize(IntersectionDirection);
				Vector3D vector3D = intersectionStart + scaleFactor * value;
				addPositionBlock = MyCubeGrid.StaticGlobalGrid_WorldToUGInt(vector3D + 0.1f * Vector3.Half * intVector * gridSize, gridSize, CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter);
				addDirectionBlock = intVector;
				intersectedBlockPos = addPositionBlock - intVector;
				intersectExactPos = MyCubeGrid.StaticGlobalGrid_WorldToUG(vector3D, gridSize, CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter);
				intersectExactPos = (Vector3D.One - Vector3.Abs(intVector)) * intersectExactPos + (intersectedBlockPos + 0.5f * intVector) * Vector3.Abs(intVector);
				return true;
			}
			Vector3D? intersectedBlockData = GetIntersectedBlockData(ref m_invGridWorldMatrix, out intersectExactPos, out intersectedBlock, out compoundBlockId);
			if (!intersectedBlockData.HasValue)
			{
				return false;
			}
			intersectedBlockPos = intersectedBlockData.Value;
			if (!GetCubeAddAndRemovePositions(Vector3I.Round(intersectedBlockPos), placingSmallGridOnLargeStatic, out addPositionBlock, out addDirectionBlock, out Vector3I removePos))
			{
				return false;
			}
			if (!placingSmallGridOnLargeStatic)
			{
				if (MyFakes.ENABLE_BLOCK_PLACING_ON_INTERSECTED_POSITION)
				{
					Vector3I vector3I = Vector3I.Round(intersectedBlockPos);
					if (vector3I != removePos)
					{
						if (m_hitInfo.HasValue)
						{
							Vector3I vector3I2 = addDirectionBlock = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(m_hitInfo.Value.HkHitInfo.Normal));
						}
						removePos = vector3I;
						addPositionBlock = removePos + addDirectionBlock;
					}
				}
				else if (CurrentGrid.CubeExists(addPositionBlock))
				{
					return false;
				}
			}
			if (placingSmallGridOnLargeStatic)
			{
				removePos = Vector3I.Round(intersectedBlockPos);
			}
			intersectedBlockPos = removePos;
			intersectedBlock = CurrentGrid.GetCubeBlock(removePos);
			if (intersectedBlock == null)
			{
				return false;
			}
			return true;
		}

		public static void ComputeSteps(Vector3I start, Vector3I end, Vector3I rotatedSize, out Vector3I stepDelta, out Vector3I counter, out int stepCount)
		{
			Vector3I value = end - start;
			stepDelta = Vector3I.Sign(value) * rotatedSize;
			counter = Vector3I.Abs(end - start) / rotatedSize + Vector3I.One;
			stepCount = counter.Size;
		}
	}
}
