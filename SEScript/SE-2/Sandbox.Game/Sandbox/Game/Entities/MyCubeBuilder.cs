using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Cube.CubeBuilder;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Models;

namespace Sandbox.Game.Entities
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
	[StaticEventOwner]
	public class MyCubeBuilder : MyBlockBuilderBase, IMyFocusHolder, IMyCubeBuilder
	{
		[Serializable]
		private struct BuildData
		{
			protected class Sandbox_Game_Entities_MyCubeBuilder_003C_003EBuildData_003C_003EPosition_003C_003EAccessor : IMemberAccessor<BuildData, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildData owner, in Vector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildData owner, out Vector3D value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeBuilder_003C_003EBuildData_003C_003EForward_003C_003EAccessor : IMemberAccessor<BuildData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildData owner, in Vector3 value)
				{
					owner.Forward = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildData owner, out Vector3 value)
				{
					value = owner.Forward;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeBuilder_003C_003EBuildData_003C_003EUp_003C_003EAccessor : IMemberAccessor<BuildData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildData owner, in Vector3 value)
				{
					owner.Up = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildData owner, out Vector3 value)
				{
					value = owner.Up;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeBuilder_003C_003EBuildData_003C_003EAbsolutePosition_003C_003EAccessor : IMemberAccessor<BuildData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildData owner, in bool value)
				{
					owner.AbsolutePosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildData owner, out bool value)
				{
					value = owner.AbsolutePosition;
				}
			}

			public Vector3D Position;

			public Vector3 Forward;

			public Vector3 Up;

			public bool AbsolutePosition;
		}

		[Flags]
		public enum SpawnFlags : ushort
		{
			None = 0x0,
			AddToScene = 0x1,
			CreatePhysics = 0x2,
			EnableSmallTolargeConnections = 0x4,
			SpawnAsMaster = 0x8,
			Default = 0x7
		}

		[Serializable]
		private struct Author
		{
			protected class Sandbox_Game_Entities_MyCubeBuilder_003C_003EAuthor_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<Author, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Author owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Author owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeBuilder_003C_003EAuthor_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<Author, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Author owner, in long value)
				{
					owner.IdentityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Author owner, out long value)
				{
					value = owner.IdentityId;
				}
			}

			public long EntityId;

			public long IdentityId;

			public Author(long entityId, long identityId)
			{
				EntityId = entityId;
				IdentityId = identityId;
			}
		}

		public enum BuildingModeEnum
		{
			SingleBlock,
			Line,
			Plane
		}

		public enum CubePlacementModeEnum
		{
			LocalCoordinateSystem,
			FreePlacement,
			GravityAligned
		}

		private struct MyColoringArea
		{
			public Vector3I Start;

			public Vector3I End;
		}

		protected sealed class RequestGridSpawn_003C_003ESandbox_Game_Entities_MyCubeBuilder_003C_003EAuthor_0023VRage_Game_DefinitionIdBlit_0023Sandbox_Game_Entities_MyCubeBuilder_003C_003EBuildData_0023System_Boolean_0023System_Boolean_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals : ICallSite<IMyEventOwner, Author, DefinitionIdBlit, BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Author author, in DefinitionIdBlit definition, in BuildData position, in bool instantBuild, in bool forceStatic, in MyCubeGrid.MyBlockVisuals visuals)
			{
				RequestGridSpawn(author, definition, position, instantBuild, forceStatic, visuals);
			}
		}

		protected sealed class SpawnGridReply_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool success, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SpawnGridReply(success);
			}
		}

		private static float SEMI_TRANSPARENT_BOX_MODIFIER;

		private static readonly MyStringId ID_SQUARE;

		private static readonly MyStringId ID_GIZMO_DRAW_LINE_RED;

		private static readonly MyStringId ID_GIZMO_DRAW_LINE;

		private static readonly MyStringId ID_GIZMO_DRAW_LINE_WHITE;

		private const float DEBUG_SCALE = 0.5f;

		private static string[] m_mountPointSideNames;

		private MyBlockRemovalData m_removalTemporalData;

		public static MyCubeBuilder Static;

		protected static double BLOCK_ROTATION_SPEED;

		private static readonly MyDefinitionId DEFAULT_BLOCK;

		public MyCubeBuilderToolType ToolType;

		private static MyColoringArea[] m_currColoringArea;

		private static List<Vector3I> m_cacheGridIntersections;

		private static int m_cycle;

		public static Dictionary<MyPlayer.PlayerId, List<Vector3>> AllPlayersColors;

		protected bool canBuild = true;

		private List<Vector3D> m_collisionTestPoints = new List<Vector3D>(12);

		private int m_lastInputHandleTime;

		private bool m_customRotation;

		private float m_animationSpeed = 0.1f;

		private bool m_animationLock;

		private bool m_stationPlacement;

		protected MyBlockBuilderRotationHints m_rotationHints = new MyBlockBuilderRotationHints();

		protected MyBlockBuilderRenderData m_renderData = new MyBlockBuilderRenderData();

		private int m_selectedAxis;

		private bool m_showAxis;

		private bool m_blockCreationActivated;

		private bool m_useSymmetry;

		private bool m_useTransparency = true;

		private bool m_alignToDefault = true;

		public Vector3D? MaxGridDistanceFrom;

		private bool AllowFreeSpacePlacement = true;

		private float FreeSpacePlacementDistance = 20f;

		private StringBuilder m_cubeCountStringBuilder = new StringBuilder(10);

		private const int MAX_CUBES_BUILT_AT_ONCE = 2048;

		private const int MAX_CUBES_BUILT_IN_ONE_AXIS = 255;

		private const float CONTINUE_BUILDING_VIEW_ANGLE_CHANGE_THRESHOLD = 0.998f;

		private const float CONTINUE_BUILDING_VIEW_POINT_CHANGE_THRESHOLD = 0.25f;

		protected MyCubeBuilderGizmo m_gizmo;

		private MySymmetrySettingModeEnum m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;

		private Vector3D m_initialIntersectionStart;

		private Vector3D m_initialIntersectionDirection;

		protected MyCubeBuilderState m_cubeBuilderState;

		protected MyCoordinateSystem.CoordSystemData m_lastLocalCoordSysData;

		private MyCubeBlockDefinition m_lastBlockDefinition;

		private MyHudNotification m_blockNotAvailableNotification;

		private MyHudNotification m_symmetryNotification;

		private CubePlacementModeEnum m_cubePlacementMode;

		private bool m_isBuildMode;

		private MyHudNotification m_buildModeHint;

		private MyHudNotification m_cubePlacementModeNotification;

		private MyHudNotification m_cubePlacementModeHint;

		private MyHudNotification m_cubePlacementUnable;

		private MyHudNotification m_coloringToolHints;

		public Action OnToolTypeChanged;

		protected HashSet<MyCubeGrid.MyBlockLocation> m_blocksBuildQueue = new HashSet<MyCubeGrid.MyBlockLocation>();

		protected List<Vector3I> m_tmpBlockPositionList = new List<Vector3I>();

		protected List<Tuple<Vector3I, ushort>> m_tmpCompoundBlockPositionIdList = new List<Tuple<Vector3I, ushort>>();

		protected HashSet<Vector3I> m_tmpBlockPositionsSet = new HashSet<Vector3I>();

		protected MySessionComponentGameInventory m_gameInventory;

		public override Type[] Dependencies
		{
			get
			{
				Type[] array = new Type[base.Dependencies.Length + 1];
				for (int i = 0; i < base.Dependencies.Length; i++)
				{
					array[i] = base.Dependencies[i];
				}
				array[array.Length - 1] = typeof(MyToolbarComponent);
				return array;
			}
		}

		public static MyBuildComponentBase BuildComponent
		{
			get;
			set;
		}

		public bool CompoundEnabled
		{
			get;
			protected set;
		}

		public int RotationAxis => m_selectedAxis;

		public bool BlockCreationIsActivated
		{
			get
			{
				return m_blockCreationActivated;
			}
			private set
			{
				m_blockCreationActivated = value;
			}
		}

		public override bool IsActivated => BlockCreationIsActivated;

		public bool UseSymmetry
		{
			get
			{
				if (m_useSymmetry && MySession.Static != null && (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
				{
					return !(MySession.Static.ControlledEntity is MyShipController);
				}
				return false;
			}
			set
			{
				if (m_useSymmetry != value)
				{
					m_useSymmetry = value;
					MySandboxGame.Config.CubeBuilderUseSymmetry = value;
					MySandboxGame.Config.Save();
				}
			}
		}

		public bool UseTransparency
		{
			get
			{
				return m_useTransparency;
			}
			set
			{
				if (m_useTransparency != value)
				{
					m_useTransparency = value;
					m_renderData.BeginCollectingInstanceData();
					m_rotationHints.Clear();
					MatrixD gridWorldMatrix = (CurrentGrid != null) ? CurrentGrid.WorldMatrix : MatrixD.Identity;
					m_renderData.EndCollectingInstanceData(gridWorldMatrix, UseTransparency);
				}
			}
		}

		public bool AlignToDefault
		{
			get
			{
				return m_alignToDefault;
			}
			set
			{
				if (m_alignToDefault != value)
				{
					m_alignToDefault = value;
					MySandboxGame.Config.CubeBuilderAlignToDefault = value;
					MySandboxGame.Config.Save();
				}
			}
		}

		public bool FreezeGizmo
		{
			get;
			set;
		}

		public bool ShowRemoveGizmo
		{
			get;
			set;
		}

		public MyCubeBuilderState CubeBuilderState => m_cubeBuilderState;

		protected internal override MyCubeGrid CurrentGrid
		{
			get
			{
				return m_currentGrid;
			}
			protected set
			{
				if (FreezeGizmo || m_currentGrid == value)
				{
					return;
				}
				BeforeCurrentGridChange(value);
				m_currentGrid = value;
				m_customRotation = false;
				if (IsCubeSizeModesAvailable && CurrentBlockDefinition != null && m_currentGrid != null)
				{
					MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName);
					int num = m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(CurrentBlockDefinition);
					MyCubeSize gridSizeEnum = m_currentGrid.GridSizeEnum;
					if (gridSizeEnum != CurrentBlockDefinition.CubeSize && ((gridSizeEnum == MyCubeSize.Small && definitionGroup.Small != null) || (gridSizeEnum == MyCubeSize.Large && definitionGroup.Large != null)))
					{
						m_cubeBuilderState.SetCubeSize(gridSizeEnum);
						SetSurvivalIntersectionDist();
						if (gridSizeEnum == MyCubeSize.Small && CubePlacementMode == CubePlacementModeEnum.LocalCoordinateSystem)
						{
							CycleCubePlacementMode();
						}
						if (num != -1 && num < m_cubeBuilderState.CurrentBlockDefinitionStages.Count)
						{
							UpdateCubeBlockStageDefinition(m_cubeBuilderState.CurrentBlockDefinitionStages[num]);
						}
					}
				}
				if (m_currentGrid == null)
				{
					RemoveSymmetryNotification();
					m_gizmo.Clear();
				}
			}
		}

		protected internal override MyVoxelBase CurrentVoxelBase
		{
			get
			{
				return m_currentVoxelBase;
			}
			protected set
			{
				if (!FreezeGizmo && m_currentVoxelBase != value)
				{
					m_currentVoxelBase = value;
					if (m_currentVoxelBase == null)
					{
						RemoveSymmetryNotification();
						m_gizmo.Clear();
					}
				}
			}
		}

		public override MyCubeBlockDefinition CurrentBlockDefinition
		{
			get
			{
				if (m_cubeBuilderState == null)
				{
					return null;
				}
				return m_cubeBuilderState.CurrentBlockDefinition;
			}
			protected set
			{
				if (m_cubeBuilderState != null)
				{
					if (m_cubeBuilderState.CurrentBlockDefinition != null)
					{
						m_lastBlockDefinition = m_cubeBuilderState.CurrentBlockDefinition;
					}
					m_cubeBuilderState.CurrentBlockDefinition = value;
				}
			}
		}

		public MyCubeBlockDefinition ToolbarBlockDefinition
		{
			get
			{
				if (m_cubeBuilderState == null)
				{
					return null;
				}
				if (MyFakes.ENABLE_BLOCK_STAGES && m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0)
				{
					return m_cubeBuilderState.CurrentBlockDefinitionStages[0];
				}
				return CurrentBlockDefinition;
			}
		}

		public static BuildingModeEnum BuildingMode
		{
			get
			{
				int num = MySandboxGame.Config.CubeBuilderBuildingMode;
				if (!Enum.IsDefined(typeof(BuildingModeEnum), num))
				{
					num = 0;
				}
				return (BuildingModeEnum)num;
			}
			set
			{
				MySandboxGame.Config.CubeBuilderBuildingMode = (int)value;
			}
		}

		public virtual bool IsCubeSizeModesAvailable => true;

		public bool IsBuildMode
		{
			get
			{
				return m_isBuildMode;
			}
			set
			{
				m_isBuildMode = value;
				MyHud.IsBuildMode = value;
				if (value)
				{
					ActivateBuildModeNotifications(MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed && MyFakes.ENABLE_CONTROLLER_HINTS);
				}
				else
				{
					DeactivateBuildModeNotifications();
				}
			}
		}

		public CubePlacementModeEnum CubePlacementMode
		{
			get
			{
				return m_cubePlacementMode;
			}
			set
			{
				if (m_cubePlacementMode != value)
				{
					m_cubePlacementMode = value;
					if (IsBuildToolActive())
					{
						ShowCubePlacementNotification();
					}
					else if (IsOnlyColorToolActive())
					{
						ShowColorToolNotifications();
					}
				}
			}
		}

		public bool DynamicMode
		{
			get;
			protected set;
		}

		protected bool GridValid
		{
			get
			{
				if (BlockCreationIsActivated)
				{
					return CurrentGrid != null;
				}
				return false;
			}
		}

		protected bool GridAndBlockValid
		{
			get
			{
				if (GridValid && CurrentBlockDefinition != null)
				{
					if (CurrentBlockDefinition.CubeSize != CurrentGrid.GridSizeEnum)
					{
						return PlacingSmallGridOnLargeStatic;
					}
					return true;
				}
				return false;
			}
		}

		protected bool VoxelMapAndBlockValid
		{
			get
			{
				if (BlockCreationIsActivated && CurrentVoxelBase != null)
				{
					return CurrentBlockDefinition != null;
				}
				return false;
			}
		}

		public bool PlacingSmallGridOnLargeStatic
		{
			get
			{
				if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && GridValid && CurrentBlockDefinition != null && CurrentBlockDefinition.CubeSize == MyCubeSize.Small && CurrentGrid.GridSizeEnum == MyCubeSize.Large)
				{
					return CurrentGrid.IsStatic;
				}
				return false;
			}
		}

		protected bool BuildInputValid
		{
			get
			{
				if (!GridAndBlockValid && !VoxelMapAndBlockValid && !DynamicMode)
				{
					return CurrentBlockDefinition != null;
				}
				return true;
			}
		}

		private float CurrentBlockScale
		{
			get
			{
				if (CurrentBlockDefinition == null)
				{
					return 1f;
				}
				return MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize) / MyDefinitionManager.Static.GetCubeSizeOriginal(CurrentBlockDefinition.CubeSize);
			}
		}

		private bool IsInSymmetrySettingMode => m_symmetrySettingMode != MySymmetrySettingModeEnum.NoPlane;

		bool IMyCubeBuilder.BlockCreationIsActivated => BlockCreationIsActivated;

		bool IMyCubeBuilder.FreezeGizmo
		{
			get
			{
				return FreezeGizmo;
			}
			set
			{
				FreezeGizmo = value;
			}
		}

		bool IMyCubeBuilder.ShowRemoveGizmo
		{
			get
			{
				return ShowRemoveGizmo;
			}
			set
			{
				ShowRemoveGizmo = value;
			}
		}

		bool IMyCubeBuilder.UseSymmetry
		{
			get
			{
				return UseSymmetry;
			}
			set
			{
				UseSymmetry = value;
			}
		}

		bool IMyCubeBuilder.UseTransparency
		{
			get
			{
				return UseTransparency;
			}
			set
			{
				UseTransparency = value;
			}
		}

		bool IMyCubeBuilder.IsActivated => IsActivated;

		public event Action OnBlockSizeChanged;

		public event Action<MyCubeBlockDefinition> OnBlockAdded;

		public event Action OnActivated;

		public event Action OnDeactivated;

		public event Action OnBlockVariantChanged;

		public event Action OnSymmetrySetupModeChanged;

		public static void DrawSemiTransparentBox(MyCubeGrid grid, MySlimBlock block, Color color, bool onlyWireframe = false, MyStringId? lineMaterial = null, Vector4? lineColor = null)
		{
			DrawSemiTransparentBox(block.Min, block.Max, grid, color, onlyWireframe, lineMaterial, lineColor);
		}

		public static void DrawSemiTransparentBox(Vector3I minPosition, Vector3I maxPosition, MyCubeGrid grid, Color color, bool onlyWireframe = false, MyStringId? lineMaterial = null, Vector4? lineColor = null)
		{
			float gridSize = grid.GridSize;
			Vector3 v = minPosition * gridSize - new Vector3(gridSize / 2f * SEMI_TRANSPARENT_BOX_MODIFIER);
			Vector3 v2 = maxPosition * gridSize + new Vector3(gridSize / 2f * SEMI_TRANSPARENT_BOX_MODIFIER);
			BoundingBoxD localbox = new BoundingBoxD(v, v2);
			MatrixD worldMatrix = grid.WorldMatrix;
			Color color2 = Color.White;
			if (lineColor.HasValue)
			{
				color2 = lineColor.Value;
			}
			MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color2, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, lineMaterial, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
			if (!onlyWireframe)
			{
				Color color3 = new Color(color * 0.2f, 0.3f);
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color3, MySimpleObjectRasterizer.Solid, 0, 0.04f, ID_SQUARE, null, onlyFrontFaces: true, -1, MyBillboard.BlendTypeEnum.LDR);
			}
		}

		protected void ClearRenderData()
		{
			m_renderData.BeginCollectingInstanceData();
			m_renderData.EndCollectingInstanceData((CurrentGrid != null) ? CurrentGrid.WorldMatrix : MatrixD.Identity, UseTransparency);
		}

		public override void Draw()
		{
			base.Draw();
			DebugDraw();
			if (BlockCreationIsActivated)
			{
				MyHud.Crosshair.Recenter();
			}
			if (!IsActivated || CurrentBlockDefinition == null)
			{
				ClearRenderData();
				return;
			}
			if (!BuildInputValid)
			{
				ClearRenderData();
				return;
			}
			DrawBuildingStepsCount(m_gizmo.SpaceDefault.m_startBuild, m_gizmo.SpaceDefault.m_startRemove, m_gizmo.SpaceDefault.m_continueBuild, ref m_gizmo.SpaceDefault.m_localMatrixAdd);
			bool flag = m_gizmo.SpaceDefault.m_startBuild.HasValue;
			bool removePos = false;
			float num = 0f;
			if (CurrentBlockDefinition != null)
			{
				num = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
			}
			if (DynamicMode)
			{
				PlaneD planeD = new PlaneD(MySector.MainCamera.Position, MySector.MainCamera.UpVector);
				Vector3D point = MyBlockBuilderBase.IntersectionStart;
				point = planeD.ProjectPoint(ref point);
				Vector3D defaultPos = point + MyBlockBuilderBase.IntersectionDistance * MyBlockBuilderBase.IntersectionDirection;
				if (m_hitInfo.HasValue)
				{
					defaultPos = m_hitInfo.Value.Position;
				}
				flag = CaluclateDynamicModePos(defaultPos, IsDynamicOverride());
				MyCoordinateSystem.Static.Visible = false;
			}
			else if (!m_gizmo.SpaceDefault.m_startBuild.HasValue && !m_gizmo.SpaceDefault.m_startRemove.HasValue)
			{
				if (!FreezeGizmo)
				{
					if (CurrentGrid != null)
					{
						MyCoordinateSystem.Static.Visible = false;
						flag = GetAddAndRemovePositions(num, PlacingSmallGridOnLargeStatic, out m_gizmo.SpaceDefault.m_addPos, out m_gizmo.SpaceDefault.m_addPosSmallOnLarge, out m_gizmo.SpaceDefault.m_addDir, out m_gizmo.SpaceDefault.m_removePos, out m_gizmo.SpaceDefault.m_removeBlock, out m_gizmo.SpaceDefault.m_blockIdInCompound, m_gizmo.SpaceDefault.m_removeBlocksInMultiBlock);
						if (flag || m_gizmo.SpaceDefault.m_removeBlock != null)
						{
							if (PlacingSmallGridOnLargeStatic)
							{
								m_gizmo.SpaceDefault.m_localMatrixAdd.Translation = m_gizmo.SpaceDefault.m_addPosSmallOnLarge.Value;
							}
							else
							{
								m_gizmo.SpaceDefault.m_localMatrixAdd.Translation = m_gizmo.SpaceDefault.m_addPos;
							}
							Vector3D translation = m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation;
							m_gizmo.SpaceDefault.m_worldMatrixAdd = m_gizmo.SpaceDefault.m_localMatrixAdd * CurrentGrid.WorldMatrix;
							m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation = translation;
							Vector3I? singleMountPointNormal = GetSingleMountPointNormal();
							if (singleMountPointNormal.HasValue && GridAndBlockValid && m_gizmo.SpaceDefault.m_addDir != Vector3I.Zero)
							{
								m_gizmo.SetupLocalAddMatrix(m_gizmo.SpaceDefault, singleMountPointNormal.Value);
							}
						}
					}
					else
					{
						MyCoordinateSystem.Static.Visible = true;
						Vector3D localSnappedPos = m_lastLocalCoordSysData.LocalSnappedPos;
						if (!MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter)
						{
							localSnappedPos -= new Vector3D(0.5 * (double)num, 0.5 * (double)num, -0.5 * (double)num);
						}
						Vector3I addPos = Vector3I.Round(localSnappedPos / num);
						m_gizmo.SpaceDefault.m_addPos = addPos;
						m_gizmo.SpaceDefault.m_localMatrixAdd.Translation = m_lastLocalCoordSysData.LocalSnappedPos;
						m_gizmo.SpaceDefault.m_worldMatrixAdd = m_lastLocalCoordSysData.Origin.TransformMatrix;
						flag = true;
					}
				}
				if (m_gizmo.SpaceDefault.m_removeBlock != null)
				{
					removePos = true;
				}
			}
			if (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyCockpit) || MyBlockBuilderBase.SpectatorIsBuilding)
			{
				if (IsInSymmetrySettingMode)
				{
					m_gizmo.SpaceDefault.m_continueBuild = null;
					flag = false;
					removePos = false;
					if (m_gizmo.SpaceDefault.m_removeBlock != null)
					{
						Vector3 value = m_gizmo.SpaceDefault.m_removeBlock.Min * CurrentGrid.GridSize;
						Vector3 value2 = m_gizmo.SpaceDefault.m_removeBlock.Max * CurrentGrid.GridSize;
						Vector3 center = (value + value2) * 0.5f;
						DrawSemiTransparentBox(color: DrawSymmetryPlane(m_symmetrySettingMode, CurrentGrid, center).ToVector4(), grid: CurrentGrid, block: m_gizmo.SpaceDefault.m_removeBlock, onlyWireframe: false, lineMaterial: ID_GIZMO_DRAW_LINE_RED);
					}
				}
				if (CurrentGrid != null && (UseSymmetry || IsInSymmetrySettingMode))
				{
					if (CurrentGrid.XSymmetryPlane.HasValue)
					{
						Vector3 center2 = CurrentGrid.XSymmetryPlane.Value * CurrentGrid.GridSize;
						DrawSymmetryPlane(CurrentGrid.XSymmetryOdd ? MySymmetrySettingModeEnum.XPlaneOdd : MySymmetrySettingModeEnum.XPlane, CurrentGrid, center2);
					}
					if (CurrentGrid.YSymmetryPlane.HasValue)
					{
						Vector3 center3 = CurrentGrid.YSymmetryPlane.Value * CurrentGrid.GridSize;
						DrawSymmetryPlane(CurrentGrid.YSymmetryOdd ? MySymmetrySettingModeEnum.YPlaneOdd : MySymmetrySettingModeEnum.YPlane, CurrentGrid, center3);
					}
					if (CurrentGrid.ZSymmetryPlane.HasValue)
					{
						Vector3 center4 = CurrentGrid.ZSymmetryPlane.Value * CurrentGrid.GridSize;
						DrawSymmetryPlane(CurrentGrid.ZSymmetryOdd ? MySymmetrySettingModeEnum.ZPlaneOdd : MySymmetrySettingModeEnum.ZPlane, CurrentGrid, center4);
					}
				}
				if (m_showAxis)
				{
					DrawRotationAxis(m_selectedAxis);
				}
			}
			UpdateGizmos(flag, removePos, draw: true);
			if (CurrentGrid == null || (DynamicMode && CurrentGrid != null))
			{
				MatrixD matrix = m_gizmo.SpaceDefault.m_worldMatrixAdd;
				Vector3D.TransformNormal(ref CurrentBlockDefinition.ModelOffset, ref matrix, out Vector3D result);
				matrix.Translation += result;
				m_renderData.EndCollectingInstanceData(matrix, UseTransparency);
			}
			else
			{
				m_renderData.EndCollectingInstanceData(CurrentGrid.WorldMatrix, UseTransparency);
			}
			UpdateBlockInfoHud();
		}

		protected virtual bool CaluclateDynamicModePos(Vector3D defaultPos, bool isDynamicOverride = false)
		{
			bool valid = true;
			if (!FreezeGizmo)
			{
				m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation = defaultPos;
				if (isDynamicOverride)
				{
					defaultPos = GetFreeSpacePlacementPosition(out valid);
					m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation = defaultPos;
				}
			}
			return valid;
		}

		protected void DrawBuildingStepsCount(Vector3I? startBuild, Vector3I? startRemove, Vector3I? continueBuild, ref Matrix localMatrixAdd)
		{
			Vector3I? vector3I = startBuild ?? startRemove;
			if (vector3I.HasValue && continueBuild.HasValue)
			{
				Vector3I.TransformNormal(ref CurrentBlockDefinition.Size, ref localMatrixAdd, out Vector3I result);
				result = Vector3I.Abs(result);
				MyBlockBuilderBase.ComputeSteps(vector3I.Value, continueBuild.Value, startBuild.HasValue ? result : Vector3I.One, out Vector3I _, out Vector3I _, out int stepCount);
				m_cubeCountStringBuilder.Clear();
				StringBuilder value = MyTexts.Get(MyCommonTexts.Clipboard_TotalBlocks);
				m_cubeCountStringBuilder.Append((object)value);
				m_cubeCountStringBuilder.AppendInt32(stepCount);
				MyGuiManager.DrawString("White", m_cubeCountStringBuilder, new Vector2(0.51f, 0.51f), 0.7f);
			}
		}

		private void DebugDraw()
		{
			if (MyPerGameSettings.EnableAi && MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != 0)
			{
				MyCubeBlockDefinition currentBlockDefinition = CurrentBlockDefinition;
				if (currentBlockDefinition != null && CurrentGrid != null)
				{
					Vector3 translation = Vector3.Transform(m_gizmo.SpaceDefault.m_addPos * 2.5f, CurrentGrid.PositionComp.WorldMatrix);
					Matrix matrix = m_gizmo.SpaceDefault.m_worldMatrixAdd;
					matrix.Translation = translation;
					matrix = Matrix.Rescale(matrix, CurrentGrid.GridSize);
					if (currentBlockDefinition.NavigationDefinition != null)
					{
						_ = currentBlockDefinition.NavigationDefinition.Mesh;
					}
				}
			}
			if (MyFakes.ENABLE_DEBUG_DRAW_TEXTURE_NAMES)
			{
				DebugDrawModelTextures();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_MODEL_INFO)
			{
				DebugDrawModelInfo();
			}
			if (MyFakes.ENABLE_DEBUG_DRAW_GENERATING_BLOCK)
			{
				DebugDrawGeneratingBlock();
			}
		}

		private void DebugDrawGeneratingBlock()
		{
			LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 200.0);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, MySession.Static.LocalCharacter, null);
			if (intersectionWithLine.HasValue && intersectionWithLine.Value.Entity is MyCubeGrid)
			{
				MyCubeGrid obj = intersectionWithLine.Value.Entity as MyCubeGrid;
				MyIntersectionResultLineTriangleEx? t = null;
				MySlimBlock slimBlock = null;
				if (obj.GetIntersectionWithLine(ref line, out t, out slimBlock) && t.HasValue && slimBlock != null && slimBlock.BlockDefinition.IsGeneratedBlock)
				{
					DebugDrawGeneratingBlock(slimBlock);
				}
			}
		}

		private void DebugDrawGeneratingBlock(MySlimBlock generatedBlock)
		{
			MySlimBlock generatingBlock = generatedBlock.CubeGrid.GetGeneratingBlock(generatedBlock);
			if (generatingBlock != null)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "Generated SubTypeId: " + generatedBlock.BlockDefinition.Id.SubtypeName + " " + generatedBlock.Min.ToString() + " " + generatedBlock.Orientation.ToString(), Color.Yellow, 0.5f);
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 14f), "Generating SubTypeId: " + generatingBlock.BlockDefinition.Id.SubtypeName + " " + generatingBlock.Min.ToString() + " " + generatingBlock.Orientation.ToString(), Color.Yellow, 0.5f);
				Vector4 value = new Vector4(Color.Blue.ToVector3() * 0.8f, 1f);
				DrawSemiTransparentBox(generatingBlock.CubeGrid, generatingBlock, Color.Blue, onlyWireframe: false, null, value);
			}
		}

		private void DebugDrawModelTextures()
		{
			LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 200.0);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, MySession.Static.LocalCharacter, null);
			if (!intersectionWithLine.HasValue)
			{
				return;
			}
			float yPos = 0f;
			if (intersectionWithLine.Value.Entity is MyCubeGrid)
			{
				MyCubeGrid obj = intersectionWithLine.Value.Entity as MyCubeGrid;
				MyIntersectionResultLineTriangleEx? t = null;
				MySlimBlock slimBlock = null;
				if (obj.GetIntersectionWithLine(ref line, out t, out slimBlock) && t.HasValue && slimBlock != null)
				{
					DebugDrawModelTextures(slimBlock.FatBlock, ref yPos);
				}
			}
		}

		private void DebugDrawModelInfo()
		{
			MyGuiScreenDebugRenderDebug.ClipboardText.Clear();
			LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 1000.0);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, MySession.Static.LocalCharacter, null, ignoreChildren: false, ignoreFloatingObjects: false, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: false);
			IMyEntity myEntity = null;
			Vector3D vector3D = Vector3D.Zero;
			if (intersectionWithLine.HasValue)
			{
				myEntity = intersectionWithLine.Value.Entity;
				vector3D = intersectionWithLine.Value.IntersectionPointInWorldSpace;
			}
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(line.From, line.To, 30);
			if (hitInfo.HasValue && (!intersectionWithLine.HasValue || (hitInfo.Value.Position - line.From).Length() < (vector3D - line.From).Length()))
			{
				myEntity = hitInfo.Value.HkHitInfo.GetHitEntity();
				vector3D = hitInfo.Value.Position;
			}
			float num = 20f;
			if (myEntity != null)
			{
				double num2 = (vector3D - line.From).Length();
				if (myEntity is MyEnvironmentSector)
				{
					MyEnvironmentSector myEnvironmentSector = myEntity as MyEnvironmentSector;
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Type: EnvironmentSector " + myEnvironmentSector.SectorId, Color.Yellow, 0.5f);
					num += 10f;
					int itemFromShapeKey = myEnvironmentSector.GetItemFromShapeKey(hitInfo.Value.HkHitInfo.GetShapeKey(0));
					short modelIndex = myEnvironmentSector.GetModelIndex(itemFromShapeKey);
					DebugDrawModelInfo(MyModels.GetModelOnlyData(myEnvironmentSector.Owner.GetModelForId(modelIndex).Model), ref num);
				}
				else if (myEntity is MyVoxelBase)
				{
					MyVoxelBase myVoxelBase = (MyVoxelBase)myEntity;
					if (myVoxelBase.RootVoxel != null)
					{
						myVoxelBase = myVoxelBase.RootVoxel;
					}
					Vector3D worldPosition = vector3D;
					MyVoxelMaterialDefinition materialAt = myVoxelBase.GetMaterialAt(ref worldPosition);
					if (myVoxelBase.RootVoxel is MyPlanet)
					{
						MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Type: planet/moon", Color.Yellow, 0.5f);
						num += 10f;
						MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Terrain: " + materialAt, Color.Yellow, 0.5f);
						num += 10f;
					}
					else
					{
						MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Type: asteroid", Color.Yellow, 0.5f);
						num += 10f;
						MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Terrain: " + materialAt, Color.Yellow, 0.5f);
						num += 10f;
					}
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Object size: " + myVoxelBase.SizeInMetres, Color.Yellow, 0.5f);
					num += 10f;
				}
				else if (myEntity is MyCubeGrid)
				{
					MyCubeGrid myCubeGrid = (MyCubeGrid)myEntity;
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Detected grid object", Color.Yellow, 0.5f);
					num += 10f;
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"Grid name: {myCubeGrid.DisplayName}", Color.Yellow, 0.5f);
					num += 10f;
					if (myCubeGrid.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? t, out MySlimBlock slimBlock) && t.HasValue && slimBlock != null)
					{
						if (slimBlock.FatBlock != null)
						{
							DebugDrawModelTextures(slimBlock.FatBlock, ref num);
						}
						else
						{
							DebugDrawBareBlockInfo(slimBlock, ref num);
						}
					}
				}
				else if (intersectionWithLine.HasValue && intersectionWithLine.Value.Entity is MyCubeBlock)
				{
					MyCubeBlock myCubeBlock = (MyCubeBlock)intersectionWithLine.Value.Entity;
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Detected block", Color.Yellow, 0.5f);
					num += 10f;
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), $"Block name: {myCubeBlock.DisplayName}", Color.Yellow, 0.5f);
					num += 10f;
					DebugDrawModelTextures(myCubeBlock, ref num);
				}
				else
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Unknown object detected", Color.Yellow, 0.5f);
					num += 10f;
					MyModel model;
					if ((model = (myEntity.Model as MyModel)) != null)
					{
						DebugDrawModelInfo(model, ref num);
					}
				}
				MyRenderProxy.DebugDrawText2D(new Vector2(20f, num), "Distance " + num2 + "m", Color.Yellow, 0.5f);
			}
			else
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(20f, 20f), "Nothing detected nearby", Color.Yellow, 0.5f);
			}
		}

		private static void DebugDrawTexturesInfo(MyModel model, ref float yPos)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (MyMesh mesh in model.GetMeshList())
			{
				if (mesh.Material.Textures == null)
				{
					hashSet.Add("<null material>");
				}
				else
				{
					hashSet.Add("Material: " + mesh.Material.Name);
					foreach (string value in mesh.Material.Textures.Values)
					{
						if (!string.IsNullOrWhiteSpace(value))
						{
							hashSet.Add(value);
						}
					}
				}
			}
			foreach (string item in hashSet.OrderBy((string s) => s, StringComparer.InvariantCultureIgnoreCase))
			{
				MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine(item);
				MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), item, Color.White, 0.5f);
				yPos += 10f;
			}
		}

		private static void DebugDrawBareBlockInfo(MySlimBlock block, ref float yPos)
		{
			yPos += 20f;
			MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine($"Display Name: {block.BlockDefinition.DisplayNameText}");
			MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine($"Cube type: {block.BlockDefinition.CubeDefinition.CubeTopology}");
			MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine($"Skin: {block.SkinSubtypeId}");
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), $"Display Name: {block.BlockDefinition.DisplayNameText}", Color.Yellow, 0.5f);
			yPos += 10f;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), $"Cube type: {block.BlockDefinition.CubeDefinition.CubeTopology}", Color.Yellow, 0.5f);
			yPos += 10f;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), $"Skin: {block.SkinSubtypeId}", Color.Yellow, 0.5f);
			yPos += 10f;
			foreach (string item in block.BlockDefinition.CubeDefinition.Model.Distinct().OrderBy((string s) => s, StringComparer.InvariantCultureIgnoreCase))
			{
				DebugDrawModelInfo(MyModels.GetModel(item), ref yPos);
			}
		}

		private static void DebugDrawModelTextures(MyCubeBlock block, ref float yPos)
		{
			MyModel myModel = null;
			if (block != null)
			{
				myModel = block.Model;
			}
			if (myModel == null)
			{
				return;
			}
			yPos += 20f;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "SubTypeId: " + block.BlockDefinition.Id.SubtypeName, Color.Yellow, 0.5f);
			yPos += 10f;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Display name: " + block.BlockDefinition.DisplayNameText, Color.Yellow, 0.5f);
			yPos += 10f;
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Skin: " + block.SlimBlock.SkinSubtypeId, Color.Yellow, 0.5f);
			yPos += 10f;
			if (block.SlimBlock.IsMultiBlockPart)
			{
				MyCubeGridMultiBlockInfo multiBlockInfo = block.CubeGrid.GetMultiBlockInfo(block.SlimBlock.MultiBlockId);
				if (multiBlockInfo != null)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Multiblock: " + multiBlockInfo.MultiBlockDefinition.Id.SubtypeName + " (Id:" + block.SlimBlock.MultiBlockId + ")", Color.Yellow, 0.5f);
					yPos += 10f;
				}
			}
			if (block.BlockDefinition.IsGeneratedBlock)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Generated block: " + block.BlockDefinition.GeneratedBlockType, Color.Yellow, 0.5f);
				yPos += 10f;
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "BlockID: " + block.EntityId, Color.Yellow, 0.5f);
			yPos += 10f;
			if (block.ModelCollision != null)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Collision: " + block.ModelCollision.AssetName, Color.Yellow, 0.5f);
			}
			yPos += 10f;
			DebugDrawModelInfo(myModel, ref yPos);
		}

		private static void DebugDrawModelInfo(MyModel model, ref float yPos)
		{
			MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine("Asset: " + model.AssetName);
			MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Asset: " + model.AssetName, Color.Yellow, 0.5f);
			yPos += 10f;
			int num = model.AssetName.LastIndexOf("\\") + 1;
			if (num != -1 && num < model.AssetName.Length)
			{
				MyTomasInputComponent.ClipboardText = model.AssetName.Substring(num);
			}
			else
			{
				MyTomasInputComponent.ClipboardText = model.AssetName;
			}
			DebugDrawTexturesInfo(model, ref yPos);
		}

		private Color DrawSymmetryPlane(MySymmetrySettingModeEnum plane, MyCubeGrid localGrid, Vector3 center)
		{
			BoundingBox localAABB = localGrid.PositionComp.LocalAABB;
			float num = 1f;
			float gridSize = localGrid.GridSize;
			Vector3 position = Vector3.Zero;
			Vector3 position2 = Vector3.Zero;
			Vector3 position3 = Vector3.Zero;
			Vector3 position4 = Vector3.Zero;
			float a = 0.1f;
			Color color = Color.Gray;
			switch (plane)
			{
			case MySymmetrySettingModeEnum.XPlane:
			case MySymmetrySettingModeEnum.XPlaneOdd:
				color = new Color(1f, 0f, 0f, a);
				center.X -= localAABB.Center.X + ((plane == MySymmetrySettingModeEnum.XPlaneOdd) ? (localGrid.GridSize * 0.50025f) : 0f);
				center.Y = 0f;
				center.Z = 0f;
				position = new Vector3(0f, localAABB.HalfExtents.Y * num + gridSize, localAABB.HalfExtents.Z * num + gridSize) + localAABB.Center + center;
				position2 = new Vector3(0f, (0f - localAABB.HalfExtents.Y) * num - gridSize, localAABB.HalfExtents.Z * num + gridSize) + localAABB.Center + center;
				position3 = new Vector3(0f, localAABB.HalfExtents.Y * num + gridSize, (0f - localAABB.HalfExtents.Z) * num - gridSize) + localAABB.Center + center;
				position4 = new Vector3(0f, (0f - localAABB.HalfExtents.Y) * num - gridSize, (0f - localAABB.HalfExtents.Z) * num - gridSize) + localAABB.Center + center;
				break;
			case MySymmetrySettingModeEnum.YPlane:
			case MySymmetrySettingModeEnum.YPlaneOdd:
				color = new Color(0f, 1f, 0f, a);
				center.X = 0f;
				center.Y -= localAABB.Center.Y + ((plane == MySymmetrySettingModeEnum.YPlaneOdd) ? (localGrid.GridSize * 0.50025f) : 0f);
				center.Z = 0f;
				position = new Vector3(localAABB.HalfExtents.X * num + gridSize, 0f, localAABB.HalfExtents.Z * num + gridSize) + localAABB.Center + center;
				position2 = new Vector3((0f - localAABB.HalfExtents.X) * num - gridSize, 0f, localAABB.HalfExtents.Z * num + gridSize) + localAABB.Center + center;
				position3 = new Vector3(localAABB.HalfExtents.X * num + gridSize, 0f, (0f - localAABB.HalfExtents.Z) * num - gridSize) + localAABB.Center + center;
				position4 = new Vector3((0f - localAABB.HalfExtents.X) * num - gridSize, 0f, (0f - localAABB.HalfExtents.Z) * num - gridSize) + localAABB.Center + center;
				break;
			case MySymmetrySettingModeEnum.ZPlane:
			case MySymmetrySettingModeEnum.ZPlaneOdd:
				color = new Color(0f, 0f, 1f, a);
				center.X = 0f;
				center.Y = 0f;
				center.Z -= localAABB.Center.Z - ((plane == MySymmetrySettingModeEnum.ZPlaneOdd) ? (localGrid.GridSize * 0.50025f) : 0f);
				position = new Vector3(localAABB.HalfExtents.X * num + gridSize, localAABB.HalfExtents.Y * num + gridSize, 0f) + localAABB.Center + center;
				position2 = new Vector3((0f - localAABB.HalfExtents.X) * num - gridSize, localAABB.HalfExtents.Y * num + gridSize, 0f) + localAABB.Center + center;
				position3 = new Vector3(localAABB.HalfExtents.X * num + gridSize, (0f - localAABB.HalfExtents.Y) * num - gridSize, 0f) + localAABB.Center + center;
				position4 = new Vector3((0f - localAABB.HalfExtents.X) * num - gridSize, (0f - localAABB.HalfExtents.Y) * num - gridSize, 0f) + localAABB.Center + center;
				break;
			}
			MatrixD matrix = CurrentGrid.WorldMatrix;
			Vector3D.Transform(ref position, ref matrix, out Vector3D result);
			Vector3D.Transform(ref position2, ref matrix, out Vector3D result2);
			Vector3D.Transform(ref position3, ref matrix, out Vector3D result3);
			Vector3D.Transform(ref position4, ref matrix, out Vector3D result4);
			MyRenderProxy.DebugDrawTriangle(result, result2, result3, color, smooth: true, depthRead: true);
			MyRenderProxy.DebugDrawTriangle(result3, result2, result4, color, smooth: true, depthRead: true);
			return color;
		}

		private void DrawRotationAxis(int axis)
		{
			MatrixD worldMatrixAdd = m_gizmo.SpaceDefault.m_worldMatrixAdd;
			Vector3D value = Vector3D.Zero;
			Color color = Color.White;
			switch (axis)
			{
			case 0:
				value = worldMatrixAdd.Left;
				color = Color.Red;
				break;
			case 1:
				value = worldMatrixAdd.Up;
				color = Color.Green;
				break;
			case 2:
				value = worldMatrixAdd.Forward;
				color = Color.Blue;
				break;
			}
			value *= (double)((CurrentBlockDefinition.CubeSize == MyCubeSize.Small) ? 1.5f : 3f);
			Vector4 color2 = color.ToVector4();
			MySimpleObjectDraw.DrawLine(worldMatrixAdd.Translation + value, worldMatrixAdd.Translation - value, ID_GIZMO_DRAW_LINE_WHITE, ref color2, 0.15f, MyBillboard.BlendTypeEnum.LDR);
		}

		public static void DrawMountPoints(float cubeSize, MyCubeBlockDefinition def, ref MatrixD drawMatrix)
		{
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = def.GetBuildProgressModelMountPoints(1f);
			if (buildProgressModelMountPoints == null)
			{
				return;
			}
			if (!MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AUTOGENERATE)
			{
				DrawMountPoints(cubeSize, def, drawMatrix, buildProgressModelMountPoints);
			}
			else if (def.Model != null)
			{
				int shapeIndex = 0;
				MyModel model = MyModels.GetModel(def.Model);
				HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
				for (int i = 0; i < havokCollisionShapes.Length; i++)
				{
					MyPhysicsDebugDraw.DrawCollisionShape(havokCollisionShapes[i], drawMatrix, 0.2f, ref shapeIndex);
				}
				MyCubeBlockDefinition.MountPoint[] mountPoints = AutogenerateMountpoints(model, cubeSize);
				DrawMountPoints(cubeSize, def, drawMatrix, mountPoints);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS_HELPERS)
			{
				DrawMountPointsAxisHelpers(def, ref drawMatrix, cubeSize);
			}
		}

		public static MyCubeBlockDefinition.MountPoint[] AutogenerateMountpoints(MyModel model, float gridSize)
		{
			HkShape[] array = model.HavokCollisionShapes;
			if (array == null)
			{
				if (model.HavokBreakableShapes == null)
				{
					return Array.Empty<MyCubeBlockDefinition.MountPoint>();
				}
				array = new HkShape[1]
				{
					model.HavokBreakableShapes[0].GetShape()
				};
			}
			return AutogenerateMountpoints(array, gridSize);
		}

		public static MyCubeBlockDefinition.MountPoint[] AutogenerateMountpoints(HkShape[] shapes, float gridSize)
		{
			_ = new List<BoundingBox>[Base6Directions.EnumDirections.Length];
			List<MyCubeBlockDefinition.MountPoint> list = new List<MyCubeBlockDefinition.MountPoint>();
			Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
			for (int i = 0; i < enumDirections.Length; i++)
			{
				int num = (int)enumDirections[i];
				Vector3 direction = Base6Directions.Directions[num];
				for (int j = 0; j < shapes.Length; j++)
				{
					HkShape shape = shapes[j];
					if (shape.ShapeType == HkShapeType.List)
					{
						HkShapeContainerIterator iterator = ((HkListShape)shape).GetIterator();
						while (iterator.IsValid)
						{
							HkShape currentValue = iterator.CurrentValue;
							if (currentValue.ShapeType == HkShapeType.ConvexTransform)
							{
								FindMountPoint(((HkConvexTransformShape)currentValue).Base, direction, gridSize, list);
							}
							else if (currentValue.ShapeType == HkShapeType.ConvexTranslate)
							{
								FindMountPoint(((HkConvexTranslateShape)currentValue).Base, direction, gridSize, list);
							}
							else
							{
								FindMountPoint(currentValue, direction, gridSize, list);
							}
							iterator.Next();
						}
						break;
					}
					if (shape.ShapeType == HkShapeType.Mopp)
					{
						HkMoppBvTreeShape hkMoppBvTreeShape = (HkMoppBvTreeShape)shape;
						for (int k = 0; k < hkMoppBvTreeShape.ShapeCollection.ShapeCount; k++)
						{
							HkShape shape2 = hkMoppBvTreeShape.ShapeCollection.GetShape((uint)k, null);
							if (shape2.ShapeType == HkShapeType.ConvexTranslate)
							{
								FindMountPoint(((HkConvexTranslateShape)shape2).Base, direction, gridSize, list);
							}
						}
						break;
					}
					FindMountPoint(shape, direction, gridSize, list);
				}
			}
			return list.ToArray();
		}

		private static bool FindMountPoint(HkShape shape, Vector3 direction, float gridSize, List<MyCubeBlockDefinition.MountPoint> mountPoints)
		{
			float d = gridSize * 0.75f / 2f;
			Plane plane = new Plane(-direction, d);
			float value = 0.2f;
			if (HkShapeCutterUtil.Cut(shape, new Vector4(plane.Normal.X, plane.Normal.Y, plane.Normal.Z, plane.D), out Vector3 aabbMin, out Vector3 aabbMax))
			{
				BoundingBox boundingBox = new BoundingBox(aabbMin, aabbMax);
				boundingBox.InflateToMinimum(new Vector3(value));
				float value2 = gridSize * 0.5f;
				MyCubeBlockDefinition.MountPoint item = default(MyCubeBlockDefinition.MountPoint);
				item.Normal = new Vector3I(direction);
				item.Start = (boundingBox.Min + new Vector3(value2)) / gridSize;
				item.End = (boundingBox.Max + new Vector3(value2)) / gridSize;
				item.Enabled = true;
				Vector3 vector = Vector3.Abs(direction) * item.Start;
				bool num = vector.AbsMax() > 0.5f;
				item.Start -= vector;
				item.Start -= direction * 0.04f;
				item.End -= Vector3.Abs(direction) * item.End;
				item.End += direction * 0.04f;
				if (num)
				{
					item.Start += Vector3.Abs(direction);
					item.End += Vector3.Abs(direction);
				}
				mountPoints.Add(item);
				return true;
			}
			return false;
		}

		public static void DrawMountPoints(float cubeSize, MyCubeBlockDefinition def, MatrixD drawMatrix, MyCubeBlockDefinition.MountPoint[] mountPoints)
		{
			Color yellow = Color.Yellow;
			Color blue = Color.Blue;
			Vector3I center = def.Center;
			Vector3 value = def.Size * 0.5f;
			MatrixD transform = MatrixD.CreateTranslation((center - value) * cubeSize) * drawMatrix;
			for (int i = 0; i < mountPoints.Length; i++)
			{
				if ((!(mountPoints[i].Normal == Base6Directions.IntDirections[0]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS0) && (!(mountPoints[i].Normal == Base6Directions.IntDirections[1]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS1) && (!(mountPoints[i].Normal == Base6Directions.IntDirections[2]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS2) && (!(mountPoints[i].Normal == Base6Directions.IntDirections[3]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS3) && (!(mountPoints[i].Normal == Base6Directions.IntDirections[4]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS4) && (!(mountPoints[i].Normal == Base6Directions.IntDirections[5]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS5))
				{
					Vector3 value2 = mountPoints[i].Start - center;
					Vector3 value3 = mountPoints[i].End - center;
					BoundingBoxD box = new BoundingBoxD(Vector3.Min(value2, value3) * cubeSize, Vector3.Max(value2, value3) * cubeSize);
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box, transform), mountPoints[i].Default ? blue : yellow, 0.2f, depthRead: true, smooth: false);
				}
			}
		}

		private static void DrawMountPointsAxisHelpers(MyCubeBlockDefinition def, ref MatrixD drawMatrix, float cubeSize)
		{
			Vector3I center = def.Center;
			Vector3 value = def.Size * 0.5f;
			MatrixD matrix = MatrixD.CreateTranslation(center - value) * MatrixD.CreateScale(cubeSize) * drawMatrix;
			for (int i = 0; i < 6; i++)
			{
				Base6Directions.Direction mountPointDirection = (Base6Directions.Direction)i;
				Vector3D vector3D = Vector3D.Zero;
				vector3D.Z = -0.20000000298023224;
				Vector3D v = Vector3.Forward;
				Vector3D v2 = Vector3.Right;
				Vector3D v3 = Vector3.Up;
				vector3D = def.MountPointLocalToBlockLocal(vector3D, mountPointDirection);
				vector3D = Vector3D.Transform(vector3D, matrix);
				v = def.MountPointLocalNormalToBlockLocal(v, mountPointDirection);
				v = Vector3D.TransformNormal(v, matrix);
				v3 = def.MountPointLocalNormalToBlockLocal(v3, mountPointDirection);
				v3 = Vector3D.TransformNormal(v3, matrix);
				v2 = def.MountPointLocalNormalToBlockLocal(v2, mountPointDirection);
				v2 = Vector3D.TransformNormal(v2, matrix);
				MatrixD worldMatrix = MatrixD.CreateWorld(vector3D + v2 * 0.25, v, v2);
				MatrixD worldMatrix2 = MatrixD.CreateWorld(vector3D + v3 * 0.25, v, v3);
				Vector4 vctColor = Color.Red.ToVector4();
				Vector4 vctColor2 = Color.Green.ToVector4();
				MyRenderProxy.DebugDrawSphere(vector3D, 0.03f * cubeSize, Color.Red.ToVector3());
				MySimpleObjectDraw.DrawTransparentCylinder(ref worldMatrix, 0f, 0.03f * cubeSize, 0.5f * cubeSize, ref vctColor, bWireFramed: false, 16, 0.01f * cubeSize);
				MySimpleObjectDraw.DrawTransparentCylinder(ref worldMatrix2, 0f, 0.03f * cubeSize, 0.5f * cubeSize, ref vctColor2, bWireFramed: false, 16, 0.01f * cubeSize);
				MyRenderProxy.DebugDrawLine3D(vector3D, vector3D - v * 0.20000000298023224, Color.Red, Color.Red, depthRead: true);
				float num = 0.5f * cubeSize;
				float num2 = 0.5f * cubeSize;
				float num3 = 0.5f * cubeSize;
				if (MySector.MainCamera != null)
				{
					float num4 = (float)(vector3D + v2 * 0.550000011920929 - MySector.MainCamera.Position).Length();
					float num5 = (float)(vector3D + v3 * 0.550000011920929 - MySector.MainCamera.Position).Length();
					float num6 = (float)(vector3D + v * 0.10000000149011612 - MySector.MainCamera.Position).Length();
					num = num * 6f / num4;
					num2 = num2 * 6f / num5;
					num3 = num3 * 6f / num6;
				}
				MyRenderProxy.DebugDrawText3D(vector3D + v2 * 0.550000011920929, "X", Color.Red, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawText3D(vector3D + v3 * 0.550000011920929, "Y", Color.Green, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawText3D(vector3D + v * 0.10000000149011612, m_mountPointSideNames[i], Color.White, num3, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
			float num7 = (float)(matrix.Translation - MySector.MainCamera.Position).Length();
			BoundingBoxD localbox = new BoundingBoxD(-def.Size * cubeSize * 0.5f, def.Size * cubeSize * 0.5f);
			float num8 = num7 - (float)localbox.Size.Max() * 0.866f;
			Color color = Color.Black;
			if (num8 < cubeSize * 3f)
			{
				MySimpleObjectDraw.DrawTransparentBox(ref drawMatrix, ref localbox, ref color, ref color, MySimpleObjectRasterizer.Wireframe, def.Size * 10, 0.005f / (float)localbox.Size.Max() * cubeSize, null, null, onlyFrontFaces: true);
			}
		}

		protected static void DrawRemovingCubes(Vector3I? startRemove, Vector3I? continueBuild, MySlimBlock removeBlock)
		{
			if (startRemove.HasValue && continueBuild.HasValue && removeBlock != null)
			{
				Color color = Color.White;
				MyBlockBuilderBase.ComputeSteps(startRemove.Value, continueBuild.Value, Vector3I.One, out Vector3I _, out Vector3I counter, out int _);
				MatrixD worldMatrix = removeBlock.CubeGrid.WorldMatrix;
				BoundingBoxD localbox = BoundingBoxD.CreateInvalid();
				localbox.Include(startRemove.Value * removeBlock.CubeGrid.GridSize);
				localbox.Include(continueBuild.Value * removeBlock.CubeGrid.GridSize);
				localbox.Min -= new Vector3(removeBlock.CubeGrid.GridSize / 2f + 0.02f);
				localbox.Max += new Vector3(removeBlock.CubeGrid.GridSize / 2f + 0.02f);
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, ref color, MySimpleObjectRasterizer.Wireframe, counter, 0.04f, null, ID_GIZMO_DRAW_LINE_RED, onlyFrontFaces: true);
				Color color2 = new Color(Color.Red * 0.2f, 0.3f);
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color2, MySimpleObjectRasterizer.Solid, 0, 0.04f, ID_SQUARE, null, onlyFrontFaces: true);
			}
		}

		static MyCubeBuilder()
		{
			SEMI_TRANSPARENT_BOX_MODIFIER = 1.1f;
			ID_SQUARE = MyStringId.GetOrCompute("Square");
			ID_GIZMO_DRAW_LINE_RED = MyStringId.GetOrCompute("GizmoDrawLineRed");
			ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");
			ID_GIZMO_DRAW_LINE_WHITE = MyStringId.GetOrCompute("GizmoDrawLineWhite");
			m_mountPointSideNames = new string[6]
			{
				"Front",
				"Back",
				"Left",
				"Right",
				"Top",
				"Bottom"
			};
			BLOCK_ROTATION_SPEED = 0.002;
			DEFAULT_BLOCK = MyDefinitionId.Parse("MyObjectBuilder_CubeBlock/LargeBlockArmorBlock");
			m_currColoringArea = new MyColoringArea[8];
			m_cacheGridIntersections = new List<Vector3I>();
			m_cycle = 0;
			AllPlayersColors = null;
			if (Sync.IsServer)
			{
				AllPlayersColors = new Dictionary<MyPlayer.PlayerId, List<Vector3>>();
			}
		}

		public MyCubeBuilder()
		{
			m_gizmo = new MyCubeBuilderGizmo();
			InitializeNotifications();
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
		}

		public override void LoadData()
		{
			base.LoadData();
			m_cubeBuilderState = new MyCubeBuilderState();
			Static = this;
			m_gameInventory = MySession.Static.GetComponent<MySessionComponentGameInventory>();
			m_useSymmetry = MySandboxGame.Config.CubeBuilderUseSymmetry;
			m_alignToDefault = MySandboxGame.Config.CubeBuilderAlignToDefault;
		}

		protected virtual void RotateAxis(int index, int sign, double angleDelta, bool newlyPressed)
		{
			if (DynamicMode)
			{
				MatrixD currentMatrix = m_gizmo.SpaceDefault.m_worldMatrixAdd;
				if (CalculateBlockRotation(index, sign, ref currentMatrix, out MatrixD rotatedMatrix, angleDelta))
				{
					m_gizmo.SpaceDefault.m_worldMatrixAdd = rotatedMatrix;
				}
			}
			else if (newlyPressed)
			{
				angleDelta = Math.PI / 2.0;
				MatrixD currentMatrix2 = m_gizmo.SpaceDefault.m_localMatrixAdd;
				if (CalculateBlockRotation(index, sign, ref currentMatrix2, out MatrixD rotatedMatrix2, angleDelta, (CurrentBlockDefinition != null) ? CurrentBlockDefinition.Direction : MyBlockDirection.Both, (CurrentBlockDefinition != null) ? CurrentBlockDefinition.Rotation : MyBlockRotation.Both))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudRotateBlock);
					m_gizmo.RotateAxis(ref rotatedMatrix2);
				}
			}
		}

		public static bool CalculateBlockRotation(int index, int sign, ref MatrixD currentMatrix, out MatrixD rotatedMatrix, double angle, MyBlockDirection blockDirection = MyBlockDirection.Both, MyBlockRotation blockRotation = MyBlockRotation.Both)
		{
			MatrixD matrixD = MatrixD.Identity;
			if (index == 2)
			{
				sign *= -1;
			}
			Vector3D zero = Vector3D.Zero;
			switch (index)
			{
			case 0:
				zero.X += (double)sign * angle;
				matrixD = MatrixD.CreateFromAxisAngle(currentMatrix.Right, (double)sign * angle);
				break;
			case 1:
				zero.Y += (double)sign * angle;
				matrixD = MatrixD.CreateFromAxisAngle(currentMatrix.Up, (double)sign * angle);
				break;
			case 2:
				zero.Z += (double)sign * angle;
				matrixD = MatrixD.CreateFromAxisAngle(currentMatrix.Forward, (double)sign * angle);
				break;
			}
			rotatedMatrix = currentMatrix;
			rotatedMatrix *= matrixD;
			rotatedMatrix = MatrixD.Orthogonalize(rotatedMatrix);
			bool flag = CheckValidBlockRotation(rotatedMatrix, blockDirection, blockRotation);
			if (flag && !Static.DynamicMode)
			{
				if (!Static.m_animationLock)
				{
					Static.m_animationLock = true;
				}
				else
				{
					flag = !flag;
				}
			}
			return flag;
		}

		private void ActivateBlockCreation(MyDefinitionId? blockDefinitionId = null)
		{
			if (MySession.Static.CameraController != null)
			{
				_ = MySession.Static.CameraController.AllowCubeBuilding;
			}
			if ((MySession.Static.ControlledEntity is MyShipController && !(MySession.Static.ControlledEntity as MyShipController).BuildingMode) || (MySession.Static.ControlledEntity is MyCharacter && (MySession.Static.ControlledEntity as MyCharacter).IsDead))
			{
				return;
			}
			if (!blockDefinitionId.HasValue)
			{
				if (m_lastBlockDefinition != null)
				{
					blockDefinitionId = m_lastBlockDefinition.Id;
				}
				else
				{
					blockDefinitionId = DEFAULT_BLOCK;
				}
			}
			bool updateNotAvailableNotification = false;
			if (IsCubeSizeModesAvailable && blockDefinitionId.HasValue && CurrentBlockDefinition != null)
			{
				bool flag = true;
				if (CheckBlock(CurrentBlockDefinition))
				{
					flag = false;
				}
				else
				{
					foreach (MyCubeBlockDefinition currentBlockDefinitionStage in m_cubeBuilderState.CurrentBlockDefinitionStages)
					{
						if (CheckBlock(currentBlockDefinitionStage))
						{
							flag = false;
							break;
						}
					}
				}
				MyCubeBlockDefinition myCubeBlockDefinition = CurrentBlockDefinition;
				if (flag)
				{
					myCubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockDefinitionId.Value);
				}
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition.BlockPairName);
				this.OnBlockSizeChanged.InvokeIfNotNull();
				MyCubeSize myCubeSize = (m_cubeBuilderState.CubeSizeMode == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large;
				if (flag || definitionGroup[myCubeSize] != null)
				{
					if (flag)
					{
						m_cubeBuilderState.CurrentBlockDefinition = myCubeBlockDefinition;
						m_cubeBuilderState.SetCubeSize(myCubeBlockDefinition.CubeSize);
					}
					else
					{
						m_cubeBuilderState.SetCubeSize(myCubeSize);
					}
					SetSurvivalIntersectionDist();
					if (myCubeSize == MyCubeSize.Small && CubePlacementMode == CubePlacementModeEnum.LocalCoordinateSystem)
					{
						CycleCubePlacementMode();
					}
					int num = m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(CurrentBlockDefinition);
					if (num != -1 && m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0)
					{
						UpdateCubeBlockStageDefinition(m_cubeBuilderState.CurrentBlockDefinitionStages[num]);
					}
				}
				else
				{
					updateNotAvailableNotification = true;
				}
			}
			else if (CurrentBlockDefinition == null && blockDefinitionId.HasValue)
			{
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockDefinitionId.Value);
				MyCubeSize myCubeSize2 = m_cubeBuilderState.CubeSizeMode;
				if (cubeBlockDefinition.CubeSize != myCubeSize2 && ((cubeBlockDefinition.CubeSize == MyCubeSize.Large) ? MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Small : MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Large) == null)
				{
					myCubeSize2 = cubeBlockDefinition.CubeSize;
				}
				m_cubeBuilderState.SetCubeSize(myCubeSize2);
				CubePlacementMode = CubePlacementModeEnum.FreePlacement;
				if (IsBuildToolActive())
				{
					ShowCubePlacementNotification();
				}
				else if (IsOnlyColorToolActive())
				{
					ShowColorToolNotifications();
				}
			}
			UpdateNotificationBlockNotAvailable(updateNotAvailableNotification);
			UpdateCubeBlockDefinition(blockDefinitionId);
			SetSurvivalIntersectionDist();
			if (MySession.Static.CreativeMode)
			{
				AllowFreeSpacePlacement = false;
				MaxGridDistanceFrom = null;
				ShowRemoveGizmo = MyFakes.SHOW_REMOVE_GIZMO;
			}
			else
			{
				AllowFreeSpacePlacement = false;
				ShowRemoveGizmo = true;
			}
			ActivateNotifications();
			if (!(MySession.Static.ControlledEntity is MyShipController) || !(MySession.Static.ControlledEntity as MyShipController).BuildingMode)
			{
				MyHud.Crosshair.ResetToDefault();
			}
			BlockCreationIsActivated = true;
			AlignToGravity();
			bool CheckBlock(MyCubeBlockDefinition b)
			{
				MyCubeBlockDefinitionGroup definitionGroup2 = MyDefinitionManager.Static.GetDefinitionGroup(b.BlockPairName);
				MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
				foreach (MyCubeSize size in values)
				{
					if (definitionGroup2[size]?.Id == blockDefinitionId.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void DeactivateBlockCreation()
		{
			if (m_cubeBuilderState != null && m_cubeBuilderState.CurrentBlockDefinition != null)
			{
				m_cubeBuilderState.UpdateCubeBlockDefinition(m_cubeBuilderState.CurrentBlockDefinition.Id, m_gizmo.SpaceDefault.m_localMatrixAdd);
			}
			BlockCreationIsActivated = false;
			DeactivateNotifications();
		}

		private void ActivateNotifications()
		{
			if (m_cubePlacementModeHint != null)
			{
				m_cubePlacementModeHint.Level = MyNotificationLevel.Control;
				MyHud.Notifications.Add(m_cubePlacementModeHint);
			}
		}

		private void DeactivateNotifications()
		{
		}

		protected virtual bool IsDynamicOverride()
		{
			if (m_cubeBuilderState == null || m_cubeBuilderState.CurrentBlockDefinition == null || CurrentGrid == null)
			{
				return false;
			}
			return m_cubeBuilderState.CurrentBlockDefinition.CubeSize != CurrentGrid.GridSizeEnum;
		}

		private void ActivateBuildModeNotifications(bool joystick)
		{
		}

		private void DeactivateBuildModeNotifications()
		{
		}

		private void InitializeNotifications()
		{
			m_cubePlacementModeNotification = new MyHudNotification(MyCommonTexts.NotificationCubePlacementModeChanged);
			UpdatePlacementNotificationState();
			m_cubePlacementModeHint = new MyHudNotification(MyCommonTexts.ControlHintCubePlacementMode, MyHudNotificationBase.INFINITE);
			m_cubePlacementModeHint.Level = MyNotificationLevel.Control;
			m_cubePlacementUnable = new MyHudNotification(MyCommonTexts.NotificationCubePlacementUnable, 2500, "Red");
			m_coloringToolHints = new MyHudNotification(MyCommonTexts.ControlHintColoringTool, MyHudNotificationBase.INFINITE);
			m_coloringToolHints.Level = MyNotificationLevel.Control;
		}

		private void UpdatePlacementNotificationState()
		{
			m_cubePlacementModeNotification.m_lifespanMs = ((!MySandboxGame.Config.ControlsHints) ? 2500 : 0);
		}

		public override void Deactivate()
		{
			if (base.Loaded)
			{
				if (BlockCreationIsActivated)
				{
					DeactivateBlockCreation();
				}
				if (m_cubeBuilderState != null)
				{
					CurrentBlockDefinition = null;
				}
				m_stationPlacement = false;
				CurrentGrid = null;
				CurrentVoxelBase = null;
				IsBuildMode = false;
				MyBlockBuilderBase.PlacementProvider = null;
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_rotationHints.ReleaseRenderData();
				}
				MyCoordinateSystem.Static.Visible = false;
				MyHud.Notifications.Remove(m_cubePlacementModeNotification);
				MyHud.Notifications.Remove(m_cubePlacementModeHint);
				MyHud.Notifications.Remove(m_coloringToolHints);
				this.OnDeactivated?.Invoke();
			}
		}

		public void OnLostFocus()
		{
			Deactivate();
		}

		public override void Activate(MyDefinitionId? blockDefinitionId = null)
		{
			ToolType = MyCubeBuilderToolType.Combined;
			if (MySession.Static.CameraController != null)
			{
				MySession.Static.GameFocusManager.Register(this);
			}
			ActivateBlockCreation(blockDefinitionId);
			this.OnActivated?.Invoke();
		}

		public void SetToolType(MyCubeBuilderToolType type)
		{
			ToolType = type;
			if (IsBuildToolActive())
			{
				ShowCubePlacementNotification();
			}
			else if (IsOnlyColorToolActive())
			{
				ShowColorToolNotifications();
			}
			OnToolTypeChanged.InvokeIfNotNull();
		}

		protected virtual void UpdateCubeBlockStageDefinition(MyCubeBlockDefinition stageCubeBlockDefinition)
		{
			if (CurrentBlockDefinition != null && stageCubeBlockDefinition != null)
			{
				Quaternion value = Quaternion.CreateFromRotationMatrix(m_gizmo.SpaceDefault.m_localMatrixAdd);
				m_cubeBuilderState.RotationsByDefinitionHash[CurrentBlockDefinition.Id] = value;
			}
			CurrentBlockDefinition = stageCubeBlockDefinition;
			m_gizmo.RotationOptions = MyCubeGridDefinitions.GetCubeRotationOptions(CurrentBlockDefinition);
			if (m_cubeBuilderState.RotationsByDefinitionHash.TryGetValue(stageCubeBlockDefinition.Id, out Quaternion value2))
			{
				m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.CreateFromQuaternion(value2);
			}
			else
			{
				m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.Identity;
			}
		}

		protected virtual void UpdateCubeBlockDefinition(MyDefinitionId? id)
		{
			m_cubeBuilderState.UpdateCubeBlockDefinition(id, m_gizmo.SpaceDefault.m_localMatrixAdd);
			if (CurrentBlockDefinition != null && IsCubeSizeModesAvailable)
			{
				m_cubeBuilderState.UpdateComplementBlock();
			}
			m_cubeBuilderState.UpdateCurrentBlockToLastSelectedVariant();
			if (m_cubeBuilderState.CurrentBlockDefinition != null)
			{
				m_gizmo.RotationOptions = MyCubeGridDefinitions.GetCubeRotationOptions(CurrentBlockDefinition);
				MyDefinitionId key = id.HasValue ? id.Value : default(MyDefinitionId);
				if (m_cubeBuilderState.RotationsByDefinitionHash.TryGetValue(key, out Quaternion value))
				{
					m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.CreateFromQuaternion(value);
				}
				else
				{
					m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.Identity;
				}
			}
		}

		public void AddFastBuildModels(MatrixD baseMatrix, ref Matrix localMatrixAdd, List<MatrixD> matrices, List<string> models, MyCubeBlockDefinition definition, Vector3I? startBuild, Vector3I? continueBuild)
		{
			MyBlockBuilderBase.AddFastBuildModelWithSubparts(ref baseMatrix, matrices, models, definition, CurrentBlockScale);
			if (CurrentBlockDefinition == null || !startBuild.HasValue || !continueBuild.HasValue)
			{
				return;
			}
			Vector3I.TransformNormal(ref CurrentBlockDefinition.Size, ref localMatrixAdd, out Vector3I result);
			result = Vector3I.Abs(result);
			MyBlockBuilderBase.ComputeSteps(startBuild.Value, continueBuild.Value, result, out Vector3I stepDelta, out Vector3I counter, out int _);
			Vector3I zero = Vector3I.Zero;
			int num = 0;
			while (num < counter.X)
			{
				zero.Y = 0;
				int num2 = 0;
				while (num2 < counter.Y)
				{
					zero.Z = 0;
					int num3 = 0;
					while (num3 < counter.Z)
					{
						Vector3I a = zero;
						Vector3 vector;
						if (CurrentGrid != null)
						{
							vector = Vector3.Transform(a * CurrentGrid.GridSize, CurrentGrid.WorldMatrix.GetOrientation());
						}
						else
						{
							float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
							vector = a * cubeSize;
						}
						MatrixD matrix = baseMatrix;
						matrix.Translation += vector;
						MyBlockBuilderBase.AddFastBuildModelWithSubparts(ref matrix, matrices, models, definition, CurrentBlockScale);
						num3++;
						zero.Z += stepDelta.Z;
					}
					num2++;
					zero.Y += stepDelta.Y;
				}
				num++;
				zero.X += stepDelta.X;
			}
		}

		private void AddFastBuildModels(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace, MatrixD baseMatrix, List<MatrixD> matrices, List<string> models, MyCubeBlockDefinition definition)
		{
			AddFastBuildModels(baseMatrix, ref gizmoSpace.m_localMatrixAdd, matrices, models, definition, gizmoSpace.m_startBuild, gizmoSpace.m_continueBuild);
		}

		public void AlignToGravity(bool alignToCamera = true)
		{
			Vector3 vector = MyGravityProviderSystem.CalculateTotalGravityInPoint(MyBlockBuilderBase.IntersectionStart);
			if (vector.LengthSquared() > 0f)
			{
				Matrix matrix = m_gizmo.SpaceDefault.m_worldMatrixAdd;
				vector.Normalize();
				Vector3D v = (!(MySector.MainCamera != null && alignToCamera)) ? Vector3D.Reject(m_gizmo.SpaceDefault.m_worldMatrixAdd.Forward, vector) : Vector3D.Reject(MySector.MainCamera.ForwardVector, vector);
				if (!v.IsValid() || v.LengthSquared() <= double.Epsilon)
				{
					v = Vector3D.CalculatePerpendicularVector(vector);
				}
				v.Normalize();
				m_gizmo.SpaceDefault.m_worldMatrixAdd = Matrix.CreateWorld(matrix.Translation, v, -vector);
			}
		}

		public virtual bool HandleGameInput()
		{
			if (HandleExportInput())
			{
				return true;
			}
			if (MyGuiScreenGamePlay.DisableInput)
			{
				return false;
			}
			if (MyControllerHelper.IsControl(MySession.Static.ControlledEntity?.ControlContext ?? MyStringId.NullOrEmpty, MyControlsSpace.LANDING_GEAR) && MySession.Static.ControlledEntity == MySession.Static.LocalCharacter && MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Identity.Character == MySession.Static.ControlledEntity && !MyInput.Static.IsAnyShiftKeyPressed() && MyGuiScreenGamePlay.ActiveGameplayScreen == null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenColorPicker());
			}
			MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.SLOT0))
			{
				Deactivate();
				return true;
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.COLOR_PICKER))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenColorPicker());
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT))
			{
				AlignToDefault = !AlignToDefault;
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
			}
			if (!IsActivated)
			{
				return false;
			}
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastInputHandleTime;
			m_lastInputHandleTime += num;
			bool flag = MySession.Static.ControlledEntity is MyCockpit && !MyBlockBuilderBase.SpectatorIsBuilding;
			if (flag && MySession.Static.ControlledEntity is MyCockpit && (MySession.Static.ControlledEntity as MyCockpit).BuildingMode)
			{
				flag = false;
			}
			if (MySandboxGame.IsPaused || flag)
			{
				return false;
			}
			if (MySession.Static.LocalCharacter != null && MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION) && MySession.Static.ControlledEntity is MyCockpit && (MySession.Static.ControlledEntity as MyCockpit).BuildingMode && MySession.Static.SurvivalMode)
			{
				MySession.Static.LocalCharacter.BeginShoot(MyShootActionEnum.PrimaryAction);
			}
			if (IsActivated && MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_MODE))
			{
				IsBuildMode = !IsBuildMode;
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.FREE_ROTATION))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				CycleCubePlacementMode();
			}
			GamepadControls(context);
			if (HandleAdminAndCreativeInput(context))
			{
				return true;
			}
			HandleSurvivalInput(context);
			if (CurrentGrid != null && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.LANDING_GEAR) && MyInput.Static.IsAnyShiftKeyPressed())
			{
				PickColor();
			}
			bool flag2 = false;
			if (MySession.Static.LocalCharacter != null)
			{
				MyCharacterDetectorComponent myCharacterDetectorComponent = MySession.Static.LocalCharacter.Components.Get<MyCharacterDetectorComponent>();
				if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.SupportedActions.HasFlag(UseActionEnum.BuildPlanner))
				{
					flag2 = true;
				}
				if (!MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER, MyControlStateType.PRESSED))
				{
					flag2 = false;
				}
			}
			if (CurrentGrid != null && MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_COLOR_CHANGE, MyControlStateType.PRESSED) && !flag2)
			{
				RecolorControlsKeyboard();
			}
			if (CurrentGrid != null && IsOnlyColorToolActive())
			{
				RecolorControlsGamepad();
			}
			if (HandleRotationInput(context, num))
			{
				return true;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_LEFT))
			{
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					SwitchToPreviousSkin();
				}
				else
				{
					SwitchToPreviousColor();
				}
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_RIGHT))
			{
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					SwitchToNextSkin();
				}
				else
				{
					SwitchToNextColor();
				}
			}
			if (HandleBlockVariantsInput(context))
			{
				return true;
			}
			return false;
		}

		private void PickColor()
		{
			MyCubeBuilderGizmo.MyGizmoSpaceProperties[] spaces = m_gizmo.Spaces;
			foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties in spaces)
			{
				if (myGizmoSpaceProperties.m_removeBlock != null && MySession.Static.LocalHumanPlayer != null)
				{
					MySession.Static.LocalHumanPlayer.ChangeOrSwitchToColor(myGizmoSpaceProperties.m_removeBlock.ColorMaskHSV);
					MyStringHash skinSubtypeId = myGizmoSpaceProperties.m_removeBlock.SkinSubtypeId;
					MySession.Static.LocalHumanPlayer.BuildArmorSkin = ((skinSubtypeId != MyStringHash.NullOrEmpty) ? skinSubtypeId.ToString() : string.Empty);
				}
			}
		}

		private void RecolorControlsKeyboard()
		{
			int expand = 0;
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsAnyShiftKeyPressed())
			{
				expand = -1;
			}
			else if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				expand = 1;
			}
			else if (MyInput.Static.IsAnyShiftKeyPressed())
			{
				expand = 3;
			}
			Change(expand);
		}

		private void RecolorControlsGamepad()
		{
			int expand = 0;
			bool flag = false;
			MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.RECOLOR, MyControlStateType.PRESSED))
			{
				expand = 0;
				flag = true;
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.MEDIUM_COLOR_BRUSH, MyControlStateType.PRESSED))
			{
				expand = 1;
				flag = true;
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.LARGE_COLOR_BRUSH, MyControlStateType.PRESSED))
			{
				expand = 3;
				flag = true;
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.RECOLOR_WHOLE_GRID, MyControlStateType.PRESSED))
			{
				expand = -1;
				flag = true;
			}
			if (flag)
			{
				Change(expand);
			}
		}

		private bool HandleBlockVariantsInput(MyStringId context)
		{
			if (MyFakes.ENABLE_BLOCK_STAGES && CurrentBlockDefinition != null && m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0 && !FreezeGizmo)
			{
				bool? flag = null;
				int num = MyInput.Static.MouseScrollWheelValue();
				if (!MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
				{
					if ((num != 0 && MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue()) || MyControllerHelper.IsControl(context, MyControlsSpace.NEXT_BLOCK_STAGE))
					{
						flag = true;
					}
					else if ((num != 0 && MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue()) || MyControllerHelper.IsControl(context, MyControlsSpace.PREV_BLOCK_STAGE))
					{
						flag = false;
					}
				}
				if (flag.HasValue)
				{
					int num2 = m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(CurrentBlockDefinition);
					int num3 = flag.Value ? 1 : (-1);
					int num4 = num2;
					int num5 = (m_cubeBuilderState.CurrentBlockDefinitionStages.Count + 1) * 2;
					while ((num4 += num3) != num2 && num5-- != 0)
					{
						if (num4 >= m_cubeBuilderState.CurrentBlockDefinitionStages.Count)
						{
							num4 = 0;
						}
						else if (num4 < 0)
						{
							num4 = m_cubeBuilderState.CurrentBlockDefinitionStages.Count - 1;
						}
						MyCubeBlockDefinition myCubeBlockDefinition = m_cubeBuilderState.CurrentBlockDefinitionStages[num4];
						if ((!MySession.Static.SurvivalMode || (myCubeBlockDefinition.AvailableInSurvival && (MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION || myCubeBlockDefinition.MultiBlock == null))) && MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(myCubeBlockDefinition, Sync.MyId) && (MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, myCubeBlockDefinition.Id) || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
						{
							UpdateCubeBlockStageDefinition(m_cubeBuilderState.CurrentBlockDefinitionStages[num4]);
							CubeBuilderState.SetCurrentBlockForBlockVariantGroup(MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName));
							this.OnBlockVariantChanged?.Invoke();
							break;
						}
					}
				}
			}
			if (IsCubeSizeModesAvailable && CurrentBlockDefinition != null && MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE))
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(CurrentBlockDefinition.BlockPairName);
				this.OnBlockSizeChanged?.Invoke();
				if ((CurrentBlockDefinition.CubeSize == MyCubeSize.Large && definitionGroup.Small != null) || (CurrentBlockDefinition.CubeSize == MyCubeSize.Small && definitionGroup.Large != null))
				{
					MyCubeSize myCubeSize = (m_cubeBuilderState.CubeSizeMode == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large;
					m_cubeBuilderState.SetCubeSize(myCubeSize);
					SetSurvivalIntersectionDist();
					if (myCubeSize == MyCubeSize.Small && CubePlacementMode == CubePlacementModeEnum.LocalCoordinateSystem)
					{
						CycleCubePlacementMode();
					}
					int num6 = m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(CurrentBlockDefinition);
					if (num6 != -1 && m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0)
					{
						UpdateCubeBlockStageDefinition(m_cubeBuilderState.CurrentBlockDefinitionStages[num6]);
					}
					UpdateNotificationBlockNotAvailable(updateNotAvailableNotification: false);
				}
				else
				{
					UpdateNotificationBlockNotAvailable(updateNotAvailableNotification: true);
				}
				return true;
			}
			return false;
		}

		public MyCubeBlockDefinition GetNextBlockVariantDefinition()
		{
			if (m_cubeBuilderState.CurrentBlockDefinitionStages.Count <= 0)
			{
				return null;
			}
			int num = m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(CurrentBlockDefinition);
			int num2 = 0;
			MyCubeBlockDefinition myCubeBlockDefinition;
			do
			{
				int index = ++num % m_cubeBuilderState.CurrentBlockDefinitionStages.Count;
				myCubeBlockDefinition = m_cubeBuilderState.CurrentBlockDefinitionStages[index];
				num2++;
				if (num2 > m_cubeBuilderState.CurrentBlockDefinitionStages.Count)
				{
					return null;
				}
			}
			while (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(myCubeBlockDefinition, Sync.MyId) || (!MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, myCubeBlockDefinition.Id) && !MySession.Static.CreativeToolsEnabled(Sync.MyId)));
			return myCubeBlockDefinition;
		}

		private void SetSurvivalIntersectionDist()
		{
			if (CurrentBlockDefinition != null && MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				if (CurrentBlockDefinition.CubeSize == MyCubeSize.Large)
				{
					MyBlockBuilderBase.IntersectionDistance = (float)MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistLargeSurvivalCharacter;
				}
				else
				{
					MyBlockBuilderBase.IntersectionDistance = (float)MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistSmallSurvivalCharacter;
				}
			}
		}

		private bool HandleRotationInput(MyStringId context, int frameDt)
		{
			if (IsActivated)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!MyControllerHelper.IsControl(context, MyBlockBuilderBase.m_rotationControls[i], MyControlStateType.PRESSED))
					{
						continue;
					}
					if (AlignToDefault)
					{
						m_customRotation = true;
					}
					bool flag = MyControllerHelper.IsControl(context, MyBlockBuilderBase.m_rotationControls[i]);
					int num = -1;
					int direction = MyBlockBuilderBase.m_rotationDirections[i];
					if (MyFakes.ENABLE_STANDARD_AXES_ROTATION)
					{
						num = GetStandardRotationAxisAndDirection(i, ref direction);
					}
					else
					{
						if (i < 2)
						{
							num = m_rotationHints.RotationUpAxis;
							direction *= m_rotationHints.RotationUpDirection;
						}
						if (i >= 2 && i < 4)
						{
							num = m_rotationHints.RotationRightAxis;
							direction *= m_rotationHints.RotationRightDirection;
						}
						if (i >= 4)
						{
							num = m_rotationHints.RotationForwardAxis;
							direction *= m_rotationHints.RotationForwardDirection;
						}
					}
					if (num == -1)
					{
						continue;
					}
					if (CurrentBlockDefinition != null && CurrentBlockDefinition.Rotation == MyBlockRotation.None)
					{
						return false;
					}
					double angleDelta = (double)frameDt * BLOCK_ROTATION_SPEED;
					if (MyInput.Static.IsAnyCtrlKeyPressed() || m_cubePlacementMode == CubePlacementModeEnum.GravityAligned)
					{
						if (!flag)
						{
							return false;
						}
						angleDelta = Math.PI / 2.0;
					}
					if (MyInput.Static.IsAnyAltKeyPressed())
					{
						if (!flag)
						{
							return false;
						}
						angleDelta = MathHelperD.ToRadians(1.0);
					}
					RotateAxis(num, direction, angleDelta, flag);
					m_showAxis = false;
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CHANGE_ROTATION_AXIS))
				{
					m_selectedAxis = (m_selectedAxis + 1) % 3;
					m_showAxis = true;
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED))
				{
					if (AlignToDefault)
					{
						m_customRotation = true;
					}
					bool newlyPressed = MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT);
					double angleDelta2 = (double)frameDt * BLOCK_ROTATION_SPEED;
					RotateAxis(m_selectedAxis, 1, angleDelta2, newlyPressed);
					m_showAxis = true;
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED))
				{
					if (AlignToDefault)
					{
						m_customRotation = true;
					}
					bool newlyPressed2 = MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT);
					double angleDelta3 = (double)frameDt * BLOCK_ROTATION_SPEED;
					RotateAxis(m_selectedAxis, -1, angleDelta3, newlyPressed2);
					m_showAxis = true;
				}
			}
			return false;
		}

		private bool HandleAdminAndCreativeInput(MyStringId context)
		{
			bool flag = (MySession.Static.CreativeToolsEnabled(Sync.MyId) && MySession.Static.HasCreativeRights) || MySession.Static.CreativeMode;
			if (!flag)
			{
				if (!MyBlockBuilderBase.SpectatorIsBuilding)
				{
				}
			}
			else
			{
				if (!(MySession.Static.ControlledEntity is MyShipController) && HandleBlockCreationMovement(context))
				{
					return true;
				}
				if (DynamicMode)
				{
					if (IsBuildToolActive() && flag && MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
					{
						Add();
					}
				}
				else if (CurrentGrid != null)
				{
					HandleCurrentGridInput(context);
				}
				else if (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
				{
					Add();
				}
			}
			return false;
		}

		private void HandleSurvivalInput(MyStringId context)
		{
			if ((!MySession.Static.CreativeToolsEnabled(Sync.MyId) || !MySession.Static.HasCreativeRights) && !MySession.Static.CreativeMode && IsBuildToolActive() && MyInput.Static.IsJoystickLastUsed && MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
			{
				MySession.Static.GetComponent<MyToolSwitcher>().SwitchToGrinder();
			}
		}

		public void ActivateColorTool()
		{
			MyDefinitionId weaponDefinition = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
			MySession.Static.LocalCharacter?.SwitchToWeapon(weaponDefinition);
			Static.CubeBuilderState.SetCubeSize(MyCubeSize.Large);
			MyDefinitionId myDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(myDefinitionId, out MyCubeBlockDefinition blockDefinition);
			if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(blockDefinition, Sync.MyId) && (MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, myDefinitionId) || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
			{
				Static.Activate(myDefinitionId);
				Static.SetToolType(MyCubeBuilderToolType.ColorTool);
			}
		}

		public void ColorPickerOk()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				ActivateColorTool();
			}
		}

		public void ColorPickerCancel()
		{
			if (MyInput.Static.IsJoystickLastUsed && IsOnlyColorToolActive())
			{
				MySession.Static.LocalCharacter?.UnequipWeapon();
			}
		}

		public bool IsSymmetrySetupMode()
		{
			if (m_symmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
			{
				return m_symmetrySettingMode != MySymmetrySettingModeEnum.Disabled;
			}
			return false;
		}

		public void ToggleSymmetrySetup()
		{
			MySymmetrySettingModeEnum symmetrySettingMode = m_symmetrySettingMode;
			if (symmetrySettingMode == MySymmetrySettingModeEnum.NoPlane)
			{
				m_symmetrySettingMode = MySymmetrySettingModeEnum.XPlane;
			}
			else
			{
				m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
			}
			UseSymmetry = true;
			UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryX, MyCommonTexts.SettingSymmetryXGamepad);
			this.OnSymmetrySetupModeChanged?.Invoke();
		}

		private void GamepadControls(MyStringId context)
		{
			if ((MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SWITCH) || (MyControllerHelper.IsControl(context, MyControlsSpace.CHANGE_ROTATION_AXIS) && IsSymmetrySetupMode())) && !(MySession.Static.ControlledEntity is MyShipController))
			{
				if (BlockCreationIsActivated)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				}
				switch (m_symmetrySettingMode)
				{
				case MySymmetrySettingModeEnum.NoPlane:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.XPlane;
					UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryX, MyCommonTexts.SettingSymmetryXGamepad);
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				case MySymmetrySettingModeEnum.XPlane:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.XPlaneOdd;
					UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryXOffset, MyCommonTexts.SettingSymmetryXOffsetGamepad);
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				case MySymmetrySettingModeEnum.XPlaneOdd:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.YPlane;
					UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryY, MyCommonTexts.SettingSymmetryYGamepad);
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				case MySymmetrySettingModeEnum.YPlane:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.YPlaneOdd;
					UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryYOffset, MyCommonTexts.SettingSymmetryYOffsetGamepad);
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				case MySymmetrySettingModeEnum.YPlaneOdd:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.ZPlane;
					UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryZ, MyCommonTexts.SettingSymmetryZGamepad);
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				case MySymmetrySettingModeEnum.ZPlane:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.ZPlaneOdd;
					UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryZOffset, MyCommonTexts.SettingSymmetryZOffsetGamepad);
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				case MySymmetrySettingModeEnum.ZPlaneOdd:
					m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
					RemoveSymmetryNotification();
					this.OnSymmetrySetupModeChanged?.Invoke();
					break;
				}
				UseSymmetry = true;
			}
			if (IsOnlyColorToolActive())
			{
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_SKIN_RIGHT))
				{
					SwitchToNextSkin();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_SKIN_LEFT))
				{
					SwitchToPreviousSkin();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_COLOR_RIGHT))
				{
					SwitchToNextColor();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_COLOR_LEFT))
				{
					SwitchToPreviousColor();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.SATURATION_INCREASE))
				{
					IncreaseSaturation();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.SATURATION_DECREASE))
				{
					DecreaseSaturation();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.VALUE_INCREASE))
				{
					IncreaseValue();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.VALUE_DECREASE))
				{
					DecreaseValue();
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.COPY_COLOR))
				{
					PickColor();
				}
			}
		}

		private bool HandleCurrentGridInput(MyStringId context)
		{
			if (MyControllerHelper.IsControl(context, MyControlsSpace.USE_SYMMETRY) && !MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SWITCH_MODE, MyControlStateType.PRESSED) && !(MySession.Static.ControlledEntity is MyShipController))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				if (ToggleSymmetry())
				{
					return true;
				}
			}
			if (CurrentBlockDefinition == null || !BlockCreationIsActivated)
			{
				m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
				RemoveSymmetryNotification();
			}
			if (IsInSymmetrySettingMode && !(MySession.Static.ControlledEntity is MyShipController))
			{
				if (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION) || (MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SETUP_ADD) && IsSymmetrySetupMode()))
				{
					if (m_gizmo.SpaceDefault.m_removeBlock != null)
					{
						Vector3I value = (m_gizmo.SpaceDefault.m_removeBlock.Min + m_gizmo.SpaceDefault.m_removeBlock.Max) / 2;
						MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
						switch (m_symmetrySettingMode)
						{
						case MySymmetrySettingModeEnum.XPlane:
							CurrentGrid.XSymmetryPlane = value;
							CurrentGrid.XSymmetryOdd = false;
							break;
						case MySymmetrySettingModeEnum.XPlaneOdd:
							CurrentGrid.XSymmetryPlane = value;
							CurrentGrid.XSymmetryOdd = true;
							break;
						case MySymmetrySettingModeEnum.YPlane:
							CurrentGrid.YSymmetryPlane = value;
							CurrentGrid.YSymmetryOdd = false;
							break;
						case MySymmetrySettingModeEnum.YPlaneOdd:
							CurrentGrid.YSymmetryPlane = value;
							CurrentGrid.YSymmetryOdd = true;
							break;
						case MySymmetrySettingModeEnum.ZPlane:
							CurrentGrid.ZSymmetryPlane = value;
							CurrentGrid.ZSymmetryOdd = false;
							break;
						case MySymmetrySettingModeEnum.ZPlaneOdd:
							CurrentGrid.ZSymmetryPlane = value;
							CurrentGrid.ZSymmetryOdd = true;
							break;
						}
					}
					return true;
				}
				if (MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION) || (MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SETUP_REMOVE) && IsSymmetrySetupMode()))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudDeleteBlock);
					switch (m_symmetrySettingMode)
					{
					case MySymmetrySettingModeEnum.XPlane:
					case MySymmetrySettingModeEnum.XPlaneOdd:
						CurrentGrid.XSymmetryPlane = null;
						CurrentGrid.XSymmetryOdd = false;
						break;
					case MySymmetrySettingModeEnum.YPlane:
					case MySymmetrySettingModeEnum.YPlaneOdd:
						CurrentGrid.YSymmetryPlane = null;
						CurrentGrid.YSymmetryOdd = false;
						break;
					case MySymmetrySettingModeEnum.ZPlane:
					case MySymmetrySettingModeEnum.ZPlaneOdd:
						CurrentGrid.ZSymmetryPlane = null;
						CurrentGrid.ZSymmetryOdd = false;
						break;
					}
					MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION);
					return false;
				}
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_CANCEL) && m_symmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
			{
				m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
				RemoveSymmetryNotification();
				return true;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
			{
				if (m_symmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
				{
					m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
					RemoveSymmetryNotification();
					return true;
				}
				if (CancelBuilding())
				{
					return true;
				}
			}
			if (IsBuildToolActive())
			{
				if (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
				{
					if (!PlacingSmallGridOnLargeStatic && (MyInput.Static.IsAnyCtrlKeyPressed() || BuildingMode != 0))
					{
						StartBuilding();
					}
					else
					{
						Add();
					}
				}
			}
			else if (IsOnlyColorToolActive() && MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED))
			{
				RecolorControlsKeyboard();
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION))
			{
				if (IsBuildToolActive())
				{
					if (MyInput.Static.IsAnyCtrlKeyPressed() || BuildingMode != 0)
					{
						StartRemoving();
					}
					else
					{
						if (MyFakes.ENABLE_COMPOUND_BLOCKS && !CompoundEnabled)
						{
							MyCubeBuilderGizmo.MyGizmoSpaceProperties[] spaces = m_gizmo.Spaces;
							foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties in spaces)
							{
								if (myGizmoSpaceProperties.Enabled)
								{
									myGizmoSpaceProperties.m_blockIdInCompound = null;
								}
							}
						}
						PrepareBlocksToRemove();
						Remove();
					}
				}
				else
				{
					PickColor();
				}
			}
			if (IsBuildToolActive())
			{
				if (MyInput.Static.IsLeftMousePressed() || MyInput.Static.IsRightMousePressed() || MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.PRESSED))
				{
					ContinueBuilding(MyInput.Static.IsAnyShiftKeyPressed() || BuildingMode == BuildingModeEnum.Plane);
				}
				if (MyInput.Static.IsNewLeftMouseReleased() || MyInput.Static.IsNewRightMouseReleased() || MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED) || MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
				{
					StopBuilding();
				}
			}
			return false;
		}

		private void DecreaseValue()
		{
		}

		private void IncreaseValue()
		{
		}

		private void DecreaseSaturation()
		{
		}

		private void IncreaseSaturation()
		{
		}

		private void SwitchToPreviousColor()
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (IsActivated && (CurrentBlockDefinition == null || MyFakes.ENABLE_BLOCK_COLORING) && localHumanPlayer != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				localHumanPlayer.SelectedBuildColorSlot = (localHumanPlayer.SelectedBuildColorSlot + localHumanPlayer.BuildColorSlots.Count - 1) % localHumanPlayer.BuildColorSlots.Count;
			}
		}

		private void SwitchToNextColor()
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (IsActivated && (CurrentBlockDefinition == null || MyFakes.ENABLE_BLOCK_COLORING) && localHumanPlayer != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				localHumanPlayer.SelectedBuildColorSlot = (localHumanPlayer.SelectedBuildColorSlot + 1) % localHumanPlayer.BuildColorSlots.Count;
			}
		}

		private void SwitchToPreviousSkin()
		{
			SwitchToSkin(next: false);
		}

		private void SwitchToNextSkin()
		{
			SwitchToSkin();
		}

		private void SwitchToSkin(bool next = true)
		{
			List<string> availableSkins = GetAvailableSkins();
			string buildArmorSkin = MySession.Static.LocalHumanPlayer.BuildArmorSkin;
			_ = string.Empty;
			int num = -1;
			for (int i = 0; i < availableSkins.Count; i++)
			{
				if (availableSkins[i] == buildArmorSkin)
				{
					num = i;
					break;
				}
			}
			int num2 = availableSkins.Count + 1;
			int num3 = next ? 1 : (num2 - 1);
			int num4 = (num + 1 + num3) % num2 - 1;
			if (num4 == -1)
			{
				MySession.Static.LocalHumanPlayer.BuildArmorSkin = null;
			}
			else
			{
				MySession.Static.LocalHumanPlayer.BuildArmorSkin = availableSkins[num4];
			}
		}

		private List<string> GetAvailableSkins()
		{
			List<string> list = new List<string>();
			List<MyGameInventoryItemDefinition> list2 = (from e in MyGameService.GetDefinitionsForSlot(MyGameInventoryItemSlot.Armor)
				orderby e.Name
				select e).ToList();
			for (int i = 0; i < list2.Count; i++)
			{
				MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), list2[i].AssetModifierId));
				if (assetModifierDefinition != null && CheckArmorSkin(list2[i], assetModifierDefinition))
				{
					list.Add(list2[i].AssetModifierId);
				}
			}
			return list;
		}

		private bool CheckArmorSkin(MyGameInventoryItemDefinition item, MyAssetModifierDefinition definition)
		{
			return true;
		}

		public bool ToggleSymmetry()
		{
			if (m_symmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
			{
				UseSymmetry = false;
				m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
				RemoveSymmetryNotification();
				return true;
			}
			UseSymmetry = !UseSymmetry;
			return false;
		}

		private bool HandleExportInput()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.E) && MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyMousePressed() && MyPerGameSettings.EnableObjectExport)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				MyCubeGrid targetGrid = MyCubeGrid.GetTargetGrid();
				if (targetGrid != null)
				{
					MyCubeGrid.ExportObject(targetGrid, convertModelsFromSBC: false, exportObjAndSBC: true);
				}
				return true;
			}
			return false;
		}

		private bool HandleBlockCreationMovement(MyStringId context)
		{
			bool flag = MyInput.Static.IsAnyCtrlKeyPressed();
			if ((flag && MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue()) || MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_FURTHER, MyControlStateType.PRESSED))
			{
				float intersectionDistance = MyBlockBuilderBase.IntersectionDistance;
				MyBlockBuilderBase.IntersectionDistance *= 1.1f;
				if (MyBlockBuilderBase.IntersectionDistance > MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
				{
					MyBlockBuilderBase.IntersectionDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
				}
				if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && CurrentBlockDefinition != null)
				{
					float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
					BoundingBoxD gizmoBox = new BoundingBoxD(-CurrentBlockDefinition.Size * cubeSize * 0.5f, CurrentBlockDefinition.Size * cubeSize * 0.5f);
					MatrixD worldMatrixAdd = m_gizmo.SpaceDefault.m_worldMatrixAdd;
					worldMatrixAdd.Translation = base.FreePlacementTarget;
					MatrixD invGridWorldMatrix = MatrixD.Invert(worldMatrixAdd);
					if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, gizmoBox, cubeSize, MyBlockBuilderBase.IntersectionDistance))
					{
						MyBlockBuilderBase.IntersectionDistance = intersectionDistance;
					}
				}
				return true;
			}
			if ((flag && MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue()) || MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_CLOSER, MyControlStateType.PRESSED))
			{
				MyBlockBuilderBase.IntersectionDistance /= 1.1f;
				if (MyBlockBuilderBase.IntersectionDistance < MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
				{
					MyBlockBuilderBase.IntersectionDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
				}
				return true;
			}
			return false;
		}

		private int GetStandardRotationAxisAndDirection(int index, ref int direction)
		{
			int result = -1;
			MatrixD matrix = MatrixD.Transpose(m_gizmo.SpaceDefault.m_localMatrixAdd);
			Vector3I vector = Vector3I.Round(Vector3D.TransformNormal(Vector3D.Up, matrix));
			if (MyInput.Static.IsAnyShiftKeyPressed())
			{
				direction *= -1;
			}
			if (CubePlacementMode == CubePlacementModeEnum.FreePlacement)
			{
				return (new int[6]
				{
					1,
					1,
					0,
					0,
					2,
					2
				})[index];
			}
			Vector3I? singleMountPointNormal = GetSingleMountPointNormal();
			if (singleMountPointNormal.HasValue)
			{
				Vector3I vector2 = singleMountPointNormal.Value;
				int num = Vector3I.Dot(ref vector2, ref Vector3I.Up);
				int num2 = Vector3I.Dot(ref vector2, ref Vector3I.Right);
				int num3 = Vector3I.Dot(ref vector2, ref Vector3I.Forward);
				if (num == 1 || num == -1)
				{
					result = 1;
					direction *= num;
				}
				else if (num2 == 1 || num2 == -1)
				{
					result = 0;
					direction *= num2;
				}
				else if (num3 == 1 || num3 == -1)
				{
					result = 2;
					direction *= num3;
				}
			}
			else if (index < 2)
			{
				int num4 = Vector3I.Dot(ref vector, ref Vector3I.Up);
				int num5 = Vector3I.Dot(ref vector, ref Vector3I.Right);
				int num6 = Vector3I.Dot(ref vector, ref Vector3I.Forward);
				if (num4 == 1 || num4 == -1)
				{
					result = 1;
					direction *= num4;
				}
				else if (num5 == 1 || num5 == -1)
				{
					result = 0;
					direction *= num5;
				}
				else if (num6 == 1 || num6 == -1)
				{
					result = 2;
					direction *= num6;
				}
			}
			else if (index >= 2 && index < 4)
			{
				Vector3I vector3 = Vector3I.Round(m_gizmo.SpaceDefault.m_localMatrixAdd.Forward);
				if (Vector3I.Dot(ref vector3, ref Vector3I.Up) == 0)
				{
					Vector3I.Cross(ref vector3, ref Vector3I.Up, out Vector3I result2);
					result2 = Vector3I.Round(Vector3D.TransformNormal((Vector3)result2, matrix));
					int num7 = Vector3I.Dot(ref result2, ref Vector3I.Up);
					int num8 = Vector3I.Dot(ref result2, ref Vector3I.Right);
					int num9 = Vector3I.Dot(ref result2, ref Vector3I.Forward);
					if (num7 == 1 || num7 == -1)
					{
						result = 1;
						direction *= num7;
					}
					else if (num8 == 1 || num8 == -1)
					{
						result = 0;
					}
					else if (num9 == 1 || num9 == -1)
					{
						result = 2;
						direction *= num9;
					}
				}
				else
				{
					result = 0;
				}
			}
			else if (index >= 4)
			{
				result = 2;
			}
			return result;
		}

		public void InputLost()
		{
			m_gizmo.Clear();
		}

		private void UpdateSymmetryNotification(MyStringId myTextsWrapperEnum, MyStringId myTextsWrapperEnumGamepad)
		{
			RemoveSymmetryNotification();
			if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
			{
				m_symmetryNotification = new MyHudNotification(myTextsWrapperEnum, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
				m_symmetryNotification.SetTextFormatArguments(MyInput.Static.GetGameControl(MyControlsSpace.PRIMARY_TOOL_ACTION), MyInput.Static.GetGameControl(MyControlsSpace.SECONDARY_TOOL_ACTION));
			}
			else
			{
				m_symmetryNotification = new MyHudNotification(myTextsWrapperEnumGamepad, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
				m_symmetryNotification.SetTextFormatArguments(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION), MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH_ALTERNATIVE));
			}
			MyHud.Notifications.Add(m_symmetryNotification);
		}

		private void RemoveSymmetryNotification()
		{
			if (m_symmetryNotification != null)
			{
				MyHud.Notifications.Remove(m_symmetryNotification);
				m_symmetryNotification = null;
			}
		}

		public static void PrepareCharacterCollisionPoints(List<Vector3D> outList)
		{
			MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
			if (myCharacter == null)
			{
				return;
			}
			float scaleFactor = myCharacter.Definition.CharacterCollisionHeight * 0.7f;
			float scaleFactor2 = myCharacter.Definition.CharacterCollisionWidth * 0.2f;
			if (myCharacter != null)
			{
				if (myCharacter.IsCrouching)
				{
					scaleFactor = myCharacter.Definition.CharacterCollisionCrouchHeight;
				}
				Vector3 value = myCharacter.PositionComp.LocalMatrix.Up * scaleFactor;
				Vector3 value2 = myCharacter.PositionComp.LocalMatrix.Forward * scaleFactor2;
				Vector3 value3 = myCharacter.PositionComp.LocalMatrix.Right * scaleFactor2;
				Vector3D value4 = myCharacter.Entity.PositionComp.GetPosition() + myCharacter.PositionComp.LocalMatrix.Up * 0.2f;
				float num = 0f;
				for (int i = 0; i < 6; i++)
				{
					float scaleFactor3 = (float)Math.Sin(num);
					float scaleFactor4 = (float)Math.Cos(num);
					Vector3D vector3D = value4 + scaleFactor3 * value3 + scaleFactor4 * value2;
					outList.Add(vector3D);
					outList.Add(vector3D + value);
					num += MathF.PI / 3f;
				}
			}
		}

		protected virtual void UpdateGizmo(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace, bool add, bool remove, bool draw)
		{
			if (gizmoSpace.Enabled)
			{
				if (!Static.canBuild)
				{
					gizmoSpace.m_showGizmoCube = false;
					gizmoSpace.m_buildAllowed = false;
				}
				if (DynamicMode)
				{
					UpdateGizmo_DynamicMode(gizmoSpace);
				}
				else if (CurrentGrid != null)
				{
					UpdateGizmo_Grid(gizmoSpace, add, remove, draw);
				}
				else
				{
					UpdateGizmo_VoxelMap(gizmoSpace, add, remove, draw);
				}
			}
		}

		private void UpdateGizmo_DynamicMode(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace)
		{
			gizmoSpace.m_animationProgress = 1f;
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
			BoundingBoxD localbox = new BoundingBoxD(-CurrentBlockDefinition.Size * cubeSize * 0.5f, CurrentBlockDefinition.Size * cubeSize * 0.5f);
			MyGridPlacementSettings settings = (CurrentBlockDefinition.CubeSize == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallGrid;
			MatrixD m = gizmoSpace.m_worldMatrixAdd;
			MyCubeGrid.GetCubeParts(CurrentBlockDefinition, Vector3I.Zero, Matrix.Identity, cubeSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, topologyCheck: true);
			if (gizmoSpace.m_showGizmoCube)
			{
				m_gizmo.AddFastBuildParts(gizmoSpace, CurrentBlockDefinition, null);
				m_gizmo.UpdateGizmoCubeParts(gizmoSpace, m_renderData, ref MatrixD.Identity, CurrentBlockDefinition);
			}
			BuildComponent.GetGridSpawnMaterials(CurrentBlockDefinition, m, isStatic: false);
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				gizmoSpace.m_buildAllowed &= BuildComponent.HasBuildingMaterials(MySession.Static.LocalCharacter);
			}
			MatrixD inverseBlockInGridWorldMatrix = MatrixD.Invert(m);
			if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref inverseBlockInGridWorldMatrix, localbox, cubeSize, MyBlockBuilderBase.IntersectionDistance) || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator)
				{
					gizmoSpace.m_buildAllowed = false;
					gizmoSpace.m_removeBlock = null;
				}
				if (MyBlockBuilderBase.CameraControllerSpectator)
				{
					gizmoSpace.m_showGizmoCube = false;
					gizmoSpace.m_buildAllowed = false;
					return;
				}
			}
			if (!gizmoSpace.m_dynamicBuildAllowed)
			{
				bool flag = MyCubeGrid.TestBlockPlacementArea(CurrentBlockDefinition, null, m, ref settings, localbox, DynamicMode);
				gizmoSpace.m_buildAllowed &= flag;
			}
			gizmoSpace.m_showGizmoCube = IsBuildToolActive();
			gizmoSpace.m_cubeMatricesTemp.Clear();
			gizmoSpace.m_cubeModelsTemp.Clear();
			bool draw = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode")).CurrentValue == 1f && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && MyFakes.ENABLE_ROTATION_HINTS && !MyInput.Static.IsJoystickLastUsed;
			m_rotationHints.CalculateRotationHints(m, draw);
			if (!CurrentBlockDefinition.IsStandAlone)
			{
				gizmoSpace.m_buildAllowed = false;
			}
			gizmoSpace.m_buildAllowed &= !IntersectsCharacterOrCamera(gizmoSpace, cubeSize, ref inverseBlockInGridWorldMatrix);
			if (!MySessionComponentSafeZones.IsActionAllowed(localbox.TransformFast(ref m), MySafeZoneAction.Building, 0L, Sync.MyId))
			{
				gizmoSpace.m_buildAllowed = false;
				gizmoSpace.m_removeBlock = null;
			}
			if (!gizmoSpace.m_showGizmoCube)
			{
				return;
			}
			Color color = Color.White;
			MyStringId value = gizmoSpace.m_buildAllowed ? ID_GIZMO_DRAW_LINE : ID_GIZMO_DRAW_LINE_RED;
			if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
			{
				MySimpleObjectDraw.DrawTransparentBox(ref m, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
			}
			AddFastBuildModels(gizmoSpace, MatrixD.Identity, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_blockDefinition);
			for (int i = 0; i < gizmoSpace.m_cubeMatricesTemp.Count; i++)
			{
				string text = gizmoSpace.m_cubeModelsTemp[i];
				if (!string.IsNullOrEmpty(text))
				{
					int id = MyModel.GetId(text);
					m_renderData.AddInstance(id, gizmoSpace.m_cubeMatricesTemp[i], ref MatrixD.Identity, MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin));
				}
			}
		}

		public bool IsBuildToolActive()
		{
			if (ToolType != MyCubeBuilderToolType.BuildTool)
			{
				return ToolType == MyCubeBuilderToolType.Combined;
			}
			return true;
		}

		public bool IsOnlyColorToolActive()
		{
			return ToolType == MyCubeBuilderToolType.ColorTool;
		}

		private void UpdateGizmo_VoxelMap(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace, bool add, bool remove, bool draw)
		{
			if (!m_animationLock)
			{
				gizmoSpace.m_animationLastMatrix = gizmoSpace.m_localMatrixAdd;
			}
			MatrixD matrixD = gizmoSpace.m_localMatrixAdd;
			if (gizmoSpace.m_animationProgress < 1f)
			{
				matrixD = MatrixD.Slerp(gizmoSpace.m_animationLastMatrix, gizmoSpace.m_localMatrixAdd, gizmoSpace.m_animationProgress);
			}
			else if (gizmoSpace.m_animationProgress >= 1f)
			{
				m_animationLock = false;
				gizmoSpace.m_animationLastMatrix = gizmoSpace.m_localMatrixAdd;
			}
			Color color = new Color(Color.Green * 0.6f, 1f);
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
			Vector3D vector3D = Vector3D.Zero;
			MatrixD worldMatrixAdd = gizmoSpace.m_worldMatrixAdd;
			MatrixD orientation = matrixD.GetOrientation();
			Color color2 = color;
			gizmoSpace.m_showGizmoCube = !IntersectsCharacterOrCamera(gizmoSpace, cubeSize, ref MatrixD.Identity);
			int num = 0;
			Vector3 vector = default(Vector3);
			vector.X = 0f;
			while (vector.X < (float)CurrentBlockDefinition.Size.X)
			{
				vector.Y = 0f;
				while (vector.Y < (float)CurrentBlockDefinition.Size.Y)
				{
					vector.Z = 0f;
					while (vector.Z < (float)CurrentBlockDefinition.Size.Z)
					{
						Vector3I vector3I = gizmoSpace.m_positions[num++];
						Vector3D vector3D2 = vector3I * cubeSize;
						if (!MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter)
						{
							vector3D2 += new Vector3D(0.5 * (double)cubeSize, 0.5 * (double)cubeSize, -0.5 * (double)cubeSize);
						}
						Vector3D translation = Vector3D.Transform(vector3D2, gizmoSpace.m_worldMatrixAdd);
						vector3D += vector3D2;
						MyCubeGrid.GetCubeParts(CurrentBlockDefinition, vector3I, orientation, cubeSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, topologyCheck: false);
						if (gizmoSpace.m_showGizmoCube)
						{
							for (int i = 0; i < gizmoSpace.m_cubeMatricesTemp.Count; i++)
							{
								MatrixD value = gizmoSpace.m_cubeMatricesTemp[i] * gizmoSpace.m_worldMatrixAdd;
								value.Translation = translation;
								gizmoSpace.m_cubeMatricesTemp[i] = value;
							}
							worldMatrixAdd.Translation = translation;
							MatrixD invGridWorldMatrix = MatrixD.Invert(orientation * worldMatrixAdd);
							m_gizmo.AddFastBuildParts(gizmoSpace, CurrentBlockDefinition, null);
							m_gizmo.UpdateGizmoCubeParts(gizmoSpace, m_renderData, ref invGridWorldMatrix, CurrentBlockDefinition);
						}
						vector.Z += 1f;
					}
					vector.Y += 1f;
				}
				vector.X += 1f;
			}
			vector3D /= (double)CurrentBlockDefinition.Size.Size;
			if (!m_animationLock)
			{
				gizmoSpace.m_animationProgress = 0f;
				gizmoSpace.m_animationLastPosition = vector3D;
			}
			else if (gizmoSpace.m_animationProgress < 1f)
			{
				vector3D = Vector3D.Lerp(gizmoSpace.m_animationLastPosition, vector3D, gizmoSpace.m_animationProgress);
			}
			vector3D = (worldMatrixAdd.Translation = Vector3D.Transform(vector3D, gizmoSpace.m_worldMatrixAdd));
			worldMatrixAdd = orientation * worldMatrixAdd;
			BoundingBoxD localbox = new BoundingBoxD(-CurrentBlockDefinition.Size * cubeSize * 0.5f, CurrentBlockDefinition.Size * cubeSize * 0.5f);
			MyGridPlacementSettings settings = (CurrentBlockDefinition.CubeSize == MyCubeSize.Large) ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
			MyBlockOrientation value2 = new MyBlockOrientation(ref Quaternion.Identity);
			bool flag = CheckValidBlockRotation(gizmoSpace.m_localMatrixAdd, CurrentBlockDefinition.Direction, CurrentBlockDefinition.Rotation) && MyCubeGrid.TestBlockPlacementArea(CurrentBlockDefinition, value2, worldMatrixAdd, ref settings, localbox, dynamicBuildMode: false);
			gizmoSpace.m_buildAllowed &= flag;
			gizmoSpace.m_buildAllowed &= gizmoSpace.m_showGizmoCube;
			gizmoSpace.m_worldMatrixAdd = worldMatrixAdd;
			BuildComponent.GetGridSpawnMaterials(CurrentBlockDefinition, worldMatrixAdd, isStatic: true);
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				gizmoSpace.m_buildAllowed &= BuildComponent.HasBuildingMaterials(MySession.Static.LocalCharacter);
			}
			if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				BoundingBoxD gizmoBox = localbox.TransformFast(ref worldMatrixAdd);
				if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref MatrixD.Identity, gizmoBox, cubeSize, MyBlockBuilderBase.IntersectionDistance) || MyBlockBuilderBase.CameraControllerSpectator)
				{
					gizmoSpace.m_buildAllowed = false;
					gizmoSpace.m_showGizmoCube = false;
					gizmoSpace.m_removeBlock = null;
					return;
				}
			}
			color2 = Color.White;
			MyStringId value3 = gizmoSpace.m_buildAllowed ? ID_GIZMO_DRAW_LINE : ID_GIZMO_DRAW_LINE_RED;
			if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
			{
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrixAdd, ref localbox, ref color2, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value3, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
				bool draw2 = !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && draw && MyFakes.ENABLE_ROTATION_HINTS && !MyInput.Static.IsJoystickLastUsed;
				m_rotationHints.CalculateRotationHints(worldMatrixAdd, draw2);
			}
			gizmoSpace.m_cubeMatricesTemp.Clear();
			gizmoSpace.m_cubeModelsTemp.Clear();
			if (gizmoSpace.m_showGizmoCube)
			{
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS)
				{
					DrawMountPoints(cubeSize, CurrentBlockDefinition, ref worldMatrixAdd);
				}
				AddFastBuildModels(gizmoSpace, MatrixD.Identity, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_blockDefinition);
				for (int j = 0; j < gizmoSpace.m_cubeMatricesTemp.Count; j++)
				{
					string text = gizmoSpace.m_cubeModelsTemp[j];
					if (!string.IsNullOrEmpty(text))
					{
						m_renderData.AddInstance(MyModel.GetId(text), gizmoSpace.m_cubeMatricesTemp[j], ref MatrixD.Identity, MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin));
					}
				}
			}
			gizmoSpace.m_animationProgress += m_animationSpeed;
		}

		private void UpdateGizmo_Grid(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace, bool add, bool remove, bool draw)
		{
			Color color = new Color(Color.Green * 0.6f, 1f);
			Color color2 = new Color(Color.Red * 0.8f, 1f);
			_ = Color.Yellow;
			_ = Color.Black;
			_ = Color.Gray;
			_ = Color.White;
			_ = Color.CornflowerBlue;
			if (add)
			{
				if (!m_animationLock)
				{
					gizmoSpace.m_animationLastMatrix = gizmoSpace.m_localMatrixAdd;
				}
				MatrixD matrix = gizmoSpace.m_localMatrixAdd;
				if (gizmoSpace.m_animationProgress < 1f)
				{
					matrix = MatrixD.Slerp(gizmoSpace.m_animationLastMatrix, gizmoSpace.m_localMatrixAdd, gizmoSpace.m_animationProgress);
				}
				else if (gizmoSpace.m_animationProgress >= 1f)
				{
					m_animationLock = false;
					gizmoSpace.m_animationLastMatrix = gizmoSpace.m_localMatrixAdd;
				}
				MatrixD worldMatrix = matrix * CurrentGrid.WorldMatrix;
				if (gizmoSpace.m_startBuild.HasValue && gizmoSpace.m_continueBuild.HasValue)
				{
					gizmoSpace.m_buildAllowed = true;
				}
				if (PlacingSmallGridOnLargeStatic && gizmoSpace.m_positionsSmallOnLarge.Count == 0)
				{
					return;
				}
				if (CurrentBlockDefinition != null)
				{
					Matrix m = gizmoSpace.m_localMatrixAdd.GetOrientation();
					MyBlockOrientation orientation = new MyBlockOrientation(ref m);
					if (!PlacingSmallGridOnLargeStatic)
					{
						bool flag = CheckValidBlockRotation(gizmoSpace.m_localMatrixAdd, CurrentBlockDefinition.Direction, CurrentBlockDefinition.Rotation);
						bool flag2 = CurrentGrid.CanPlaceBlock(gizmoSpace.m_min, gizmoSpace.m_max, orientation, gizmoSpace.m_blockDefinition, Sync.MyId);
						gizmoSpace.m_buildAllowed &= (flag && flag2);
					}
					BuildComponent.GetBlockPlacementMaterials(gizmoSpace.m_blockDefinition, gizmoSpace.m_addPos, orientation, CurrentGrid);
					if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
					{
						gizmoSpace.m_buildAllowed &= BuildComponent.HasBuildingMaterials(MySession.Static.LocalCharacter);
					}
					if (!PlacingSmallGridOnLargeStatic && MySession.Static.SurvivalMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MyBlockBuilderBase.SpectatorIsBuilding)
					{
						Vector3 v = (m_gizmo.SpaceDefault.m_min - new Vector3(0.5f)) * CurrentGrid.GridSize;
						Vector3 v2 = (m_gizmo.SpaceDefault.m_max + new Vector3(0.5f)) * CurrentGrid.GridSize;
						if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(gizmoBox: new BoundingBoxD(v, v2), invGridWorldMatrix: ref m_invGridWorldMatrix, gridSize: CurrentGrid.GridSize, intersectionDistance: MyBlockBuilderBase.IntersectionDistance) || MyBlockBuilderBase.CameraControllerSpectator)
						{
							gizmoSpace.m_buildAllowed = false;
							gizmoSpace.m_removeBlock = null;
							return;
						}
					}
					if (gizmoSpace.m_buildAllowed)
					{
						Quaternion.CreateFromRotationMatrix(ref gizmoSpace.m_localMatrixAdd, out gizmoSpace.m_rotation);
						if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled && !PlacingSmallGridOnLargeStatic)
						{
							MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = CurrentBlockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
							gizmoSpace.m_buildAllowed = MyCubeGrid.CheckConnectivity(CurrentGrid, CurrentBlockDefinition, buildProgressModelMountPoints, ref gizmoSpace.m_rotation, ref gizmoSpace.m_centerPos);
						}
					}
					Color color3 = color;
					if (PlacingSmallGridOnLargeStatic)
					{
						MatrixD inverseBlockInGridWorldMatrix = MatrixD.Invert(gizmoSpace.m_worldMatrixAdd);
						gizmoSpace.m_showGizmoCube = (IsBuildToolActive() && !IntersectsCharacterOrCamera(gizmoSpace, CurrentGrid.GridSize, ref inverseBlockInGridWorldMatrix));
					}
					else
					{
						gizmoSpace.m_showGizmoCube = (IsBuildToolActive() && !IntersectsCharacterOrCamera(gizmoSpace, CurrentGrid.GridSize, ref m_invGridWorldMatrix));
					}
					gizmoSpace.m_buildAllowed &= gizmoSpace.m_showGizmoCube;
					Vector3D vector3D = Vector3D.Zero;
					_ = gizmoSpace.m_worldMatrixAdd.Translation;
					MatrixD drawMatrix = gizmoSpace.m_worldMatrixAdd;
					int num = 0;
					Vector3 vector = default(Vector3);
					vector.X = 0f;
					while (vector.X < (float)CurrentBlockDefinition.Size.X)
					{
						vector.Y = 0f;
						while (vector.Y < (float)CurrentBlockDefinition.Size.Y)
						{
							vector.Z = 0f;
							while (vector.Z < (float)CurrentBlockDefinition.Size.Z)
							{
								if (PlacingSmallGridOnLargeStatic)
								{
									float num2 = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize) / CurrentGrid.GridSize;
									Vector3D value = gizmoSpace.m_positionsSmallOnLarge[num++];
									Vector3I inputPosition = Vector3I.Round(value / num2);
									Vector3D vector3D2 = Vector3D.Transform(value * CurrentGrid.GridSize, CurrentGrid.WorldMatrix);
									vector3D += vector3D2;
									drawMatrix.Translation = vector3D2;
									MyCubeGrid.GetCubeParts(CurrentBlockDefinition, inputPosition, gizmoSpace.m_localMatrixAdd.GetOrientation(), CurrentGrid.GridSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, topologyCheck: true);
									if (gizmoSpace.m_showGizmoCube)
									{
										for (int i = 0; i < gizmoSpace.m_cubeMatricesTemp.Count; i++)
										{
											MatrixD value2 = gizmoSpace.m_cubeMatricesTemp[i];
											value2.Translation *= (double)num2;
											value2 *= CurrentGrid.WorldMatrix;
											value2.Translation = vector3D2;
											gizmoSpace.m_cubeMatricesTemp[i] = value2;
										}
										m_gizmo.AddFastBuildParts(gizmoSpace, CurrentBlockDefinition, CurrentGrid);
										m_gizmo.UpdateGizmoCubeParts(gizmoSpace, m_renderData, ref m_invGridWorldMatrix);
									}
								}
								else
								{
									Vector3I vector3I = gizmoSpace.m_positions[num++];
									Vector3D translation = Vector3D.Transform(vector3I * CurrentGrid.GridSize, CurrentGrid.WorldMatrix);
									vector3D += vector3I * CurrentGrid.GridSize;
									MyCubeGrid.GetCubeParts(CurrentBlockDefinition, vector3I, matrix.GetOrientation(), CurrentGrid.GridSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, topologyCheck: false);
									if (gizmoSpace.m_showGizmoCube)
									{
										for (int j = 0; j < gizmoSpace.m_cubeMatricesTemp.Count; j++)
										{
											MatrixD value3 = gizmoSpace.m_cubeMatricesTemp[j] * CurrentGrid.WorldMatrix;
											value3.Translation = translation;
											gizmoSpace.m_cubeMatricesTemp[j] = value3;
										}
										m_gizmo.AddFastBuildParts(gizmoSpace, CurrentBlockDefinition, CurrentGrid);
										m_gizmo.UpdateGizmoCubeParts(gizmoSpace, m_renderData, ref m_invGridWorldMatrix, CurrentBlockDefinition);
									}
								}
								vector.Z += 1f;
							}
							vector.Y += 1f;
						}
						vector.X += 1f;
					}
					vector3D /= (double)CurrentBlockDefinition.Size.Size;
					if (!m_animationLock)
					{
						gizmoSpace.m_animationProgress = 0f;
						gizmoSpace.m_animationLastPosition = vector3D;
					}
					else if (gizmoSpace.m_animationProgress < 1f)
					{
						vector3D = Vector3D.Lerp(gizmoSpace.m_animationLastPosition, vector3D, gizmoSpace.m_animationProgress);
					}
					vector3D = (drawMatrix.Translation = Vector3D.Transform(vector3D, CurrentGrid.WorldMatrix));
					gizmoSpace.m_worldMatrixAdd = drawMatrix;
					float num3 = PlacingSmallGridOnLargeStatic ? MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize) : CurrentGrid.GridSize;
					BoundingBoxD localbox = new BoundingBoxD(-CurrentBlockDefinition.Size * num3 * 0.5f, CurrentBlockDefinition.Size * num3 * 0.5f);
					MyGridPlacementSettings gridPlacementSettings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(CurrentBlockDefinition.CubeSize, CurrentGrid.IsStatic);
					MyBlockOrientation value4 = new MyBlockOrientation(ref Quaternion.Identity);
					bool flag3 = MyCubeGrid.TestVoxelPlacement(CurrentBlockDefinition, gridPlacementSettings, dynamicBuildMode: false, drawMatrix, localbox);
					gizmoSpace.m_buildAllowed &= flag3;
					if (PlacingSmallGridOnLargeStatic)
					{
						if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
						{
							MatrixD invGridWorldMatrix = Matrix.Invert(drawMatrix);
							BuildComponent.GetBlockPlacementMaterials(CurrentBlockDefinition, gizmoSpace.m_addPos, orientation, CurrentGrid);
							gizmoSpace.m_buildAllowed &= BuildComponent.HasBuildingMaterials(MySession.Static.LocalCharacter);
							if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, localbox, num3, MyBlockBuilderBase.IntersectionDistance) || MyBlockBuilderBase.CameraControllerSpectator)
							{
								gizmoSpace.m_buildAllowed = false;
								gizmoSpace.m_removeBlock = null;
								return;
							}
						}
						MyGridPlacementSettings settings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(CurrentGrid.GridSizeEnum, CurrentGrid.IsStatic);
						bool flag4 = CheckValidBlockRotation(gizmoSpace.m_localMatrixAdd, CurrentBlockDefinition.Direction, CurrentBlockDefinition.Rotation) && MyCubeGrid.TestBlockPlacementArea(CurrentBlockDefinition, value4, drawMatrix, ref settings, localbox, !CurrentGrid.IsStatic, null, testVoxel: false);
						gizmoSpace.m_buildAllowed &= flag4;
						if (gizmoSpace.m_buildAllowed && gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
						{
							gizmoSpace.m_buildAllowed &= MyCubeGrid.CheckConnectivitySmallBlockToLargeGrid(CurrentGrid, CurrentBlockDefinition, ref gizmoSpace.m_rotation, ref gizmoSpace.m_addDir);
						}
						gizmoSpace.m_worldMatrixAdd = drawMatrix;
					}
					if (!MySessionComponentSafeZones.IsActionAllowed(localbox.TransformFast(ref drawMatrix), MySafeZoneAction.Building, 0L, Sync.MyId))
					{
						gizmoSpace.m_buildAllowed = false;
						gizmoSpace.m_removeBlock = null;
					}
					color3 = Color.White;
					MyStringId value5 = gizmoSpace.m_buildAllowed ? ID_GIZMO_DRAW_LINE : ID_GIZMO_DRAW_LINE_RED;
					if (IsBuildToolActive() && gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
					{
						if (MyFakes.ENABLE_VR_BUILDING)
						{
							Vector3 value6 = -0.5f * gizmoSpace.m_addDir;
							if (gizmoSpace.m_addPosSmallOnLarge.HasValue)
							{
								float num4 = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize) / CurrentGrid.GridSize;
								value6 = -0.5f * num4 * gizmoSpace.m_addDir;
							}
							value6 *= CurrentGrid.GridSize;
							Vector3I a = Vector3I.Round(Vector3.Abs(Vector3.TransformNormal((Vector3)CurrentBlockDefinition.Size, gizmoSpace.m_localMatrixAdd)));
							Vector3I b = Vector3I.One - Vector3I.Abs(gizmoSpace.m_addDir);
							Vector3 vector2 = num3 * 0.5f * (a * b) + 0.02f * Vector3I.Abs(gizmoSpace.m_addDir);
							BoundingBoxD localbox2 = new BoundingBoxD(-vector2 + value6, vector2 + value6);
							MySimpleObjectDraw.DrawTransparentBox(ref drawMatrix, ref localbox2, ref color3, MySimpleObjectRasterizer.Wireframe, 1, gizmoSpace.m_addPosSmallOnLarge.HasValue ? 0.04f : 0.06f, null, value5, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
						}
						else
						{
							worldMatrix.Translation = drawMatrix.Translation;
							MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color3, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, null, value5, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
						}
					}
					gizmoSpace.m_cubeMatricesTemp.Clear();
					gizmoSpace.m_cubeModelsTemp.Clear();
					if (gizmoSpace.m_showGizmoCube)
					{
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS)
						{
							float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
							if (!PlacingSmallGridOnLargeStatic)
							{
								cubeSize = CurrentGrid.GridSize;
							}
							DrawMountPoints(cubeSize, CurrentBlockDefinition, ref drawMatrix);
						}
						Vector3D.TransformNormal(ref CurrentBlockDefinition.ModelOffset, ref gizmoSpace.m_worldMatrixAdd, out Vector3D result);
						worldMatrix.Translation = vector3D + CurrentGrid.GridScale * result;
						AddFastBuildModels(gizmoSpace, worldMatrix, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_blockDefinition);
						for (int k = 0; k < gizmoSpace.m_cubeMatricesTemp.Count; k++)
						{
							string text = gizmoSpace.m_cubeModelsTemp[k];
							if (!string.IsNullOrEmpty(text))
							{
								m_renderData.AddInstance(MyModel.GetId(text), gizmoSpace.m_cubeMatricesTemp[k], ref m_invGridWorldMatrix, MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin));
							}
						}
					}
					if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
					{
						IMyHudStat stat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode"));
						bool draw2 = !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && draw && MyFakes.ENABLE_ROTATION_HINTS && stat.CurrentValue == 1f && !MyInput.Static.IsJoystickLastUsed;
						m_rotationHints.CalculateRotationHints(worldMatrix, draw2);
					}
				}
			}
			if (gizmoSpace.m_startRemove.HasValue && gizmoSpace.m_continueBuild.HasValue)
			{
				gizmoSpace.m_buildAllowed = IsBuildToolActive();
				DrawRemovingCubes(gizmoSpace.m_startRemove, gizmoSpace.m_continueBuild, gizmoSpace.m_removeBlock);
			}
			else if (remove && ShowRemoveGizmo)
			{
				if (gizmoSpace.m_removeBlocksInMultiBlock.Count > 0)
				{
					m_tmpBlockPositionsSet.Clear();
					GetAllBlocksPositions(gizmoSpace.m_removeBlocksInMultiBlock, m_tmpBlockPositionsSet);
					foreach (Vector3I item in m_tmpBlockPositionsSet)
					{
						DrawSemiTransparentBox(item, item, CurrentGrid, color2, onlyWireframe: false, ID_GIZMO_DRAW_LINE_RED);
					}
					m_tmpBlockPositionsSet.Clear();
				}
				else if (gizmoSpace.m_removeBlock != null && !MyFakes.ENABLE_VR_BUILDING)
				{
					Color white = Color.White;
					Color white2 = Color.White;
					MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
					if (IsBuildToolActive())
					{
						white = color2;
						nullOrEmpty = ID_GIZMO_DRAW_LINE_WHITE;
						white2 = color2;
					}
					else if (CurrentGrid.ColorGridOrBlockRequestValidation(MySession.Static.LocalPlayerId))
					{
						white = Color.Lime;
						nullOrEmpty = ID_GIZMO_DRAW_LINE_WHITE;
						white2 = Color.Lime;
					}
					else
					{
						white = color2;
						nullOrEmpty = ID_GIZMO_DRAW_LINE_WHITE;
						white2 = color2;
					}
					DrawSemiTransparentBox(CurrentGrid, gizmoSpace.m_removeBlock, white, onlyWireframe: true, nullOrEmpty, white2);
				}
				if (gizmoSpace.m_removeBlock != null && MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_REMOVE_CUBE_COORDS)
				{
					MySlimBlock removeBlock = gizmoSpace.m_removeBlock;
					MyCubeGrid cubeGrid = removeBlock.CubeGrid;
					Matrix matrix2 = cubeGrid.WorldMatrix;
					MyRenderProxy.DebugDrawText3D(Vector3.Transform(removeBlock.Position * cubeGrid.GridSize, matrix2), removeBlock.Position.ToString(), Color.White, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				}
			}
			else if (MySession.Static.SurvivalMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				_ = MyBlockBuilderBase.CameraControllerSpectator;
				Vector3 v3 = (m_gizmo.SpaceDefault.m_min - new Vector3(0.5f)) * CurrentGrid.GridSize;
				Vector3 v4 = (m_gizmo.SpaceDefault.m_max + new Vector3(0.5f)) * CurrentGrid.GridSize;
				if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(gizmoBox: new BoundingBoxD(v3, v4), invGridWorldMatrix: ref m_invGridWorldMatrix, gridSize: CurrentGrid.GridSize, intersectionDistance: MyBlockBuilderBase.IntersectionDistance))
				{
					gizmoSpace.m_removeBlock = null;
				}
			}
			gizmoSpace.m_animationProgress += m_animationSpeed;
		}

		private bool IntersectsCharacterOrCamera(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace, float gridSize, ref MatrixD inverseBlockInGridWorldMatrix)
		{
			if (CurrentBlockDefinition == null)
			{
				return false;
			}
			bool flag = false;
			if (MySector.MainCamera != null)
			{
				flag = m_gizmo.PointInsideGizmo(MySector.MainCamera.Position, gizmoSpace.SourceSpace, ref inverseBlockInGridWorldMatrix, gridSize, 0.05f, CurrentVoxelBase != null, DynamicMode);
			}
			if (flag)
			{
				return true;
			}
			if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity is MyCharacter)
			{
				m_collisionTestPoints.Clear();
				PrepareCharacterCollisionPoints(m_collisionTestPoints);
				flag = m_gizmo.PointsAABBIntersectsGizmo(m_collisionTestPoints, gizmoSpace.SourceSpace, ref inverseBlockInGridWorldMatrix, gridSize, 0.05f, CurrentVoxelBase != null, DynamicMode);
			}
			return flag;
		}

		public static bool CheckValidBlockRotation(Matrix localMatrix, MyBlockDirection blockDirection, MyBlockRotation blockRotation)
		{
			Vector3I vector = Vector3I.Round(localMatrix.Forward);
			Vector3I vector2 = Vector3I.Round(localMatrix.Up);
			int num = Vector3I.Dot(ref vector, ref vector);
			int num2 = Vector3I.Dot(ref vector2, ref vector2);
			if (num > 1 || num2 > 1)
			{
				if (blockDirection == MyBlockDirection.Both)
				{
					return true;
				}
				return false;
			}
			if (blockDirection == MyBlockDirection.Horizontal)
			{
				if (vector == Vector3I.Up || vector == -Vector3I.Up)
				{
					return false;
				}
				if (blockRotation == MyBlockRotation.Vertical && vector2 != Vector3I.Up)
				{
					return false;
				}
			}
			return true;
		}

		public static bool CheckValidBlocksRotation(Matrix gridLocalMatrix, MyCubeGrid grid)
		{
			bool flag = true;
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
				Matrix result;
				if (myCompoundCubeBlock != null)
				{
					foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
					{
						block2.Orientation.GetMatrix(out result);
						result *= gridLocalMatrix;
						flag = (flag && CheckValidBlockRotation(result, block2.BlockDefinition.Direction, block2.BlockDefinition.Rotation));
						if (!flag)
						{
							break;
						}
					}
				}
				else
				{
					block.Orientation.GetMatrix(out result);
					result *= gridLocalMatrix;
					flag = (flag && CheckValidBlockRotation(result, block.BlockDefinition.Direction, block.BlockDefinition.Rotation));
				}
				if (!flag)
				{
					return flag;
				}
			}
			return flag;
		}

		public virtual void Add()
		{
			if (CurrentBlockDefinition == null)
			{
				return;
			}
			m_blocksBuildQueue.Clear();
			bool flag = true;
			MyCubeBuilderGizmo.MyGizmoSpaceProperties[] spaces = m_gizmo.Spaces;
			foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties in spaces)
			{
				if (BuildInputValid && myGizmoSpaceProperties.Enabled && myGizmoSpaceProperties.m_buildAllowed && Static.canBuild)
				{
					flag = false;
					AddBlocksToBuildQueueOrSpawn(myGizmoSpaceProperties);
				}
			}
			if (flag)
			{
				NotifyPlacementUnable();
			}
			if (m_blocksBuildQueue.Count > 0)
			{
				if (MyMusicController.Static != null)
				{
					MyMusicController.Static.Building(2000);
				}
				CurrentGrid.BuildBlocks(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), m_blocksBuildQueue, MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId);
			}
		}

		public void NotifyPlacementUnable()
		{
			if (CurrentBlockDefinition != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				m_cubePlacementUnable.SetTextFormatArguments(CurrentBlockDefinition.DisplayNameText);
				MyHud.Notifications.Add(m_cubePlacementUnable);
			}
		}

		public bool AddBlocksToBuildQueueOrSpawn(MyCubeBlockDefinition blockDefinition, ref MatrixD worldMatrixAdd, Vector3I min, Vector3I max, Vector3I center, Quaternion localOrientation)
		{
			return AddBlocksToBuildQueueOrSpawn(blockDefinition, ref worldMatrixAdd, min, max, center, localOrientation, new MyCubeGrid.MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)));
		}

		private bool AddBlocksToBuildQueueOrSpawn(MyCubeBlockDefinition blockDefinition, ref MatrixD worldMatrixAdd, Vector3I min, Vector3I max, Vector3I center, Quaternion localOrientation, MyCubeGrid.MyBlockVisuals visuals)
		{
			bool flag = false;
			if (!MySession.Static.Players.TryGetPlayerId(MySession.Static.LocalPlayerId, out MyPlayer.PlayerId result))
			{
				return false;
			}
			if (!MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
			{
				return false;
			}
			bool flag2 = MySession.Static.CreativeToolsEnabled(result.SteamId) || MySession.Static.CreativeMode;
			if (!MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, blockDefinition.BlockPairName, flag2 ? blockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 1))
			{
				return false;
			}
			BuildData arg = default(BuildData);
			if (GridAndBlockValid)
			{
				if (PlacingSmallGridOnLargeStatic)
				{
					MatrixD matrixD = worldMatrixAdd;
					arg.Position = matrixD.Translation;
					if (MySession.Static.ControlledEntity != null)
					{
						arg.Position -= MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
					}
					else
					{
						arg.AbsolutePosition = true;
					}
					arg.Forward = matrixD.Forward;
					arg.Up = matrixD.Up;
					MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<Author, DefinitionIdBlit, BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>>)((IMyEventOwner s) => RequestGridSpawn), new Author(MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId), (DefinitionIdBlit)blockDefinition.Id, arg, MySession.Static.CreativeToolsEnabled(Sync.MyId), arg6: true, visuals, default(EndpointId), (Vector3D?)null);
				}
				else
				{
					m_blocksBuildQueue.Add(new MyCubeGrid.MyBlockLocation(blockDefinition.Id, min, max, center, localOrientation, MyEntityIdentifier.AllocateId(), MySession.Static.LocalPlayerId));
				}
				flag = true;
			}
			else
			{
				arg.Position = worldMatrixAdd.Translation;
				if (MySession.Static.ControlledEntity != null)
				{
					arg.Position -= MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
				}
				else
				{
					arg.AbsolutePosition = true;
				}
				arg.Forward = worldMatrixAdd.Forward;
				arg.Up = worldMatrixAdd.Up;
				MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<Author, DefinitionIdBlit, BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>>)((IMyEventOwner s) => RequestGridSpawn), new Author(MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId), (DefinitionIdBlit)blockDefinition.Id, arg, MySession.Static.CreativeToolsEnabled(Sync.MyId), arg6: false, visuals, default(EndpointId), (Vector3D?)null);
				flag = true;
				MySession.Static.TotalBlocksCreated++;
				if (MySession.Static.ControlledEntity is MyCockpit)
				{
					MySession.Static.TotalBlocksCreatedFromShips++;
				}
			}
			if (this.OnBlockAdded != null)
			{
				this.OnBlockAdded(blockDefinition);
			}
			return flag;
		}

		private bool AddBlocksToBuildQueueOrSpawn(MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace)
		{
			return AddBlocksToBuildQueueOrSpawn(gizmoSpace.m_blockDefinition, ref gizmoSpace.m_worldMatrixAdd, gizmoSpace.m_min, gizmoSpace.m_max, gizmoSpace.m_centerPos, gizmoSpace.LocalOrientation);
		}

		private void UpdateGizmos(bool addPos, bool removePos, bool draw)
		{
			if (CurrentBlockDefinition == null || (CurrentGrid != null && CurrentGrid.Physics != null && CurrentGrid.Physics.RigidBody.HasProperty(254)))
			{
				return;
			}
			m_gizmo.SpaceDefault.m_blockDefinition = CurrentBlockDefinition;
			m_gizmo.EnableGizmoSpaces(CurrentBlockDefinition, CurrentGrid, UseSymmetry);
			m_renderData.BeginCollectingInstanceData();
			m_rotationHints.Clear();
			int num = m_gizmo.Spaces.Length;
			if (CurrentGrid != null)
			{
				m_invGridWorldMatrix = CurrentGrid.PositionComp.WorldMatrixInvScaled;
			}
			for (int i = 0; i < num; i++)
			{
				MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties = m_gizmo.Spaces[i];
				bool flag = addPos && BuildInputValid;
				if (myGizmoSpaceProperties.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
				{
					Quaternion q = myGizmoSpaceProperties.LocalOrientation;
					if (!PlacingSmallGridOnLargeStatic && CurrentGrid != null && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
					{
						flag &= CurrentGrid.CanAddCube(myGizmoSpaceProperties.m_addPos, new MyBlockOrientation(ref q), CurrentBlockDefinition);
					}
				}
				else
				{
					flag &= UseSymmetry;
					removePos &= UseSymmetry;
				}
				UpdateGizmo(myGizmoSpaceProperties, flag || FreezeGizmo, removePos || FreezeGizmo, draw);
			}
		}

		public MyOrientedBoundingBoxD GetBuildBoundingBox(float inflate = 0f)
		{
			if (m_gizmo.SpaceDefault.m_blockDefinition == null)
			{
				return default(MyOrientedBoundingBoxD);
			}
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(m_gizmo.SpaceDefault.m_blockDefinition.CubeSize);
			Vector3 value = m_gizmo.SpaceDefault.m_blockDefinition.Size * cubeSize * 0.5f + inflate;
			MatrixD worldMatrixAdd = m_gizmo.SpaceDefault.m_worldMatrixAdd;
			if (m_gizmo.SpaceDefault.m_removeBlock != null && !m_gizmo.SpaceDefault.m_addPosSmallOnLarge.HasValue)
			{
				MySlimBlock removeBlock = m_gizmo.SpaceDefault.m_removeBlock;
				Vector3D vector3D2 = worldMatrixAdd.Translation = Vector3D.Transform(m_gizmo.SpaceDefault.m_addPos * cubeSize, removeBlock.CubeGrid.PositionComp.WorldMatrix);
			}
			Vector3D min = Vector3D.Zero - value;
			Vector3D max = Vector3D.Zero + value;
			return new MyOrientedBoundingBoxD(new BoundingBoxD(min, max), worldMatrixAdd);
		}

		public virtual bool CanStartConstruction(MyEntity buildingEntity)
		{
			MatrixD worldMatrixAdd = m_gizmo.SpaceDefault.m_worldMatrixAdd;
			BuildComponent.GetGridSpawnMaterials(CurrentBlockDefinition, worldMatrixAdd, isStatic: false);
			return BuildComponent.HasBuildingMaterials(buildingEntity);
		}

		public virtual bool AddConstruction(MyEntity builder)
		{
			MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(builder);
			if (!canBuild || (controllingPlayer != null && !controllingPlayer.IsLocalPlayer))
			{
				return false;
			}
			if (controllingPlayer == null || controllingPlayer.IsRemotePlayer)
			{
				MyEntity isUsing = (builder as MyCharacter).IsUsing;
				if (isUsing == null)
				{
					return false;
				}
				controllingPlayer = Sync.Players.GetControllingPlayer(isUsing);
				if (controllingPlayer == null || controllingPlayer.IsRemotePlayer)
				{
					return false;
				}
			}
			MyCubeBuilderGizmo.MyGizmoSpaceProperties spaceDefault = m_gizmo.SpaceDefault;
			if (spaceDefault.Enabled && BuildInputValid && spaceDefault.m_buildAllowed && canBuild)
			{
				m_blocksBuildQueue.Clear();
				bool num = AddBlocksToBuildQueueOrSpawn(spaceDefault);
				if (num && CurrentGrid != null && m_blocksBuildQueue.Count > 0)
				{
					if (MySession.Static != null && builder == MySession.Static.LocalCharacter && MyMusicController.Static != null)
					{
						MyMusicController.Static.Building(2000);
					}
					if (Sync.IsServer)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
					}
					if (builder == MySession.Static.LocalCharacter)
					{
						MySession.Static.TotalBlocksCreated++;
						if (MySession.Static.ControlledEntity is MyCockpit)
						{
							MySession.Static.TotalBlocksCreatedFromShips++;
						}
					}
					CurrentGrid.BuildBlocks(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), m_blocksBuildQueue, builder.EntityId, controllingPlayer.Identity.IdentityId);
				}
				return num;
			}
			NotifyPlacementUnable();
			return false;
		}

		private Vector3I MakeCubePosition(Vector3D position)
		{
			position -= CurrentGrid.WorldMatrix.Translation;
			Vector3D value = new Vector3D(CurrentGrid.GridSize);
			Vector3D vector3D = position / value;
			Vector3I result = default(Vector3I);
			result.X = (int)Math.Round(vector3D.X);
			result.Y = (int)Math.Round(vector3D.Y);
			result.Z = (int)Math.Round(vector3D.Z);
			return result;
		}

		public void GetAddPosition(out Vector3D position)
		{
			position = m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation;
		}

		public virtual bool GetAddAndRemovePositions(float gridSize, bool placingSmallGridOnLargeStatic, out Vector3I addPos, out Vector3? addPosSmallOnLarge, out Vector3I addDir, out Vector3I removePos, out MySlimBlock removeBlock, out ushort? compoundBlockId, HashSet<Tuple<MySlimBlock, ushort?>> removeBlocksInMultiBlock)
		{
			bool flag = false;
			addPosSmallOnLarge = null;
			removePos = default(Vector3I);
			removeBlock = null;
			flag = GetBlockAddPosition(gridSize, placingSmallGridOnLargeStatic, out MySlimBlock intersectedBlock, out Vector3D intersectedBlockPos, out Vector3D intersectExactPos, out addPos, out addDir, out compoundBlockId);
			float num = placingSmallGridOnLargeStatic ? CurrentGrid.GridSize : gridSize;
			if (!MaxGridDistanceFrom.HasValue || Vector3D.DistanceSquared(intersectExactPos * num, Vector3D.Transform(MaxGridDistanceFrom.Value, m_invGridWorldMatrix)) < (double)(MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance * MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance))
			{
				removePos = Vector3I.Round(intersectedBlockPos);
				removeBlock = intersectedBlock;
				if (removeBlock != null && removeBlock.FatBlock != null && MySession.Static.ControlledEntity as MyShipController == removeBlock.FatBlock)
				{
					removeBlock = null;
				}
			}
			else if (AllowFreeSpacePlacement && CurrentGrid != null)
			{
				Vector3D position = MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * Math.Min(FreeSpacePlacementDistance, MyBlockBuilderBase.IntersectionDistance);
				addPos = MakeCubePosition(position);
				addDir = new Vector3I(0, 0, 1);
				removePos = addPos - addDir;
				removeBlock = CurrentGrid.GetCubeBlock(removePos);
				if (removeBlock != null && removeBlock.FatBlock != null && MySession.Static.ControlledEntity as MyShipController == removeBlock.FatBlock)
				{
					removeBlock = null;
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (!Static.canBuild)
			{
				return false;
			}
			if (flag && placingSmallGridOnLargeStatic)
			{
				MatrixD matrix = Matrix.Identity;
				if (intersectedBlock != null)
				{
					matrix = intersectedBlock.CubeGrid.WorldMatrix.GetOrientation();
					if (intersectedBlock.FatBlock != null)
					{
						if (compoundBlockId.HasValue)
						{
							MyCompoundCubeBlock myCompoundCubeBlock = intersectedBlock.FatBlock as MyCompoundCubeBlock;
							if (myCompoundCubeBlock != null)
							{
								MySlimBlock block = myCompoundCubeBlock.GetBlock(compoundBlockId.Value);
								if (block != null && block.FatBlock.Components.Has<MyFractureComponentBase>())
								{
									return false;
								}
							}
						}
						else if (intersectedBlock.FatBlock.Components.Has<MyFractureComponentBase>())
						{
							return false;
						}
					}
				}
				MatrixD.Invert(matrix);
				if (m_hitInfo.HasValue)
				{
					Vector3 value = Vector3.TransformNormal(m_hitInfo.Value.HkHitInfo.Normal, m_invGridWorldMatrix);
					addDir = Vector3I.Sign(Vector3.DominantAxisProjection(value));
				}
				Vector3 vector = removePos + 0.5f * addDir;
				Vector3D vector3D = intersectExactPos - vector;
				Vector3I vector3I = Vector3I.Abs(addDir);
				vector3D = (Vector3I.One - vector3I) * Vector3.Clamp(vector3D, new Vector3(-0.495f), new Vector3(0.495f)) + vector3I * vector3D;
				Vector3D value2 = vector + vector3D;
				float num2 = gridSize / CurrentGrid.GridSize;
				float num3 = MyFakes.ENABLE_VR_BUILDING ? 0.25f : 0.1f;
				Vector3I b = Vector3I.Round((value2 + num3 * num2 * addDir - num2 * Vector3.Half) / num2);
				addPosSmallOnLarge = num2 * b + num2 * Vector3.Half;
			}
			return flag;
		}

		protected virtual void PrepareBlocksToRemove()
		{
			m_tmpBlockPositionList.Clear();
			m_tmpCompoundBlockPositionIdList.Clear();
			MyCubeBuilderGizmo.MyGizmoSpaceProperties[] spaces = m_gizmo.Spaces;
			foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties in spaces)
			{
				if (myGizmoSpaceProperties.Enabled && GridAndBlockValid && myGizmoSpaceProperties.m_removeBlock != null && (myGizmoSpaceProperties.m_removeBlock.FatBlock == null || !myGizmoSpaceProperties.m_removeBlock.FatBlock.IsSubBlock) && CurrentGrid == myGizmoSpaceProperties.m_removeBlock.CubeGrid)
				{
					if (myGizmoSpaceProperties.m_removeBlocksInMultiBlock.Count > 0)
					{
						foreach (Tuple<MySlimBlock, ushort?> item in myGizmoSpaceProperties.m_removeBlocksInMultiBlock)
						{
							RemoveBlock(item.Item1, item.Item2);
						}
					}
					else
					{
						RemoveBlock(myGizmoSpaceProperties.m_removeBlock, myGizmoSpaceProperties.m_blockIdInCompound, checkExisting: true);
					}
					myGizmoSpaceProperties.m_removeBlock = null;
					myGizmoSpaceProperties.m_removeBlocksInMultiBlock.Clear();
				}
			}
		}

		protected void Remove()
		{
			if (m_tmpBlockPositionList.Count > 0 || m_tmpCompoundBlockPositionIdList.Count > 0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudDeleteBlock);
				if (m_tmpBlockPositionList.Count > 0)
				{
					CurrentGrid.RazeBlocks(m_tmpBlockPositionList, 0L, Sync.MyId);
					m_tmpBlockPositionList.Clear();
				}
				if (m_tmpCompoundBlockPositionIdList.Count > 0)
				{
					CurrentGrid.RazeBlockInCompoundBlock(m_tmpCompoundBlockPositionIdList);
				}
			}
		}

		protected void RemoveBlock(MySlimBlock block, ushort? blockIdInCompound, bool checkExisting = false)
		{
			if (block == null || (block.FatBlock != null && block.FatBlock.IsSubBlock))
			{
				return;
			}
			MyCockpit myCockpit = block.FatBlock as MyCockpit;
			if (myCockpit != null && myCockpit.Pilot != null)
			{
				m_removalTemporalData = new MyBlockRemovalData(block, blockIdInCompound, checkExisting);
				if (!MySession.Static.CreativeMode && MySession.Static.IsUserAdmin(Sync.MyId))
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedMessageBox, messageText: MyTexts.Get(MyCommonTexts.RemovePilotToo)));
				}
				else
				{
					OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum.NO);
				}
			}
			else
			{
				RemoveBlockInternal(block, blockIdInCompound, checkExisting);
			}
		}

		protected void RemoveBlockInternal(MySlimBlock block, ushort? blockIdInCompound, bool checkExisting = false)
		{
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				if (blockIdInCompound.HasValue)
				{
					if (!checkExisting || !m_tmpCompoundBlockPositionIdList.Exists((Tuple<Vector3I, ushort> t) => t.Item1 == block.Min && t.Item2 == blockIdInCompound.Value))
					{
						m_tmpCompoundBlockPositionIdList.Add(new Tuple<Vector3I, ushort>(block.Min, blockIdInCompound.Value));
					}
				}
				else if (!checkExisting || !m_tmpBlockPositionList.Contains(block.Min))
				{
					m_tmpBlockPositionList.Add(block.Min);
				}
			}
			else if (!checkExisting || !m_tmpBlockPositionList.Contains(block.Min))
			{
				m_tmpBlockPositionList.Add(block.Min);
			}
		}

		public void OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (m_removalTemporalData != null && m_removalTemporalData.Block != null && CurrentGrid != null && (m_removalTemporalData.Block.FatBlock == null || !m_removalTemporalData.Block.FatBlock.IsSubBlock) && m_removalTemporalData.Block.CubeGrid != null && !m_removalTemporalData.Block.CubeGrid.Closed)
			{
				MyCockpit myCockpit = m_removalTemporalData.Block.FatBlock as MyCockpit;
				if (result == MyGuiScreenMessageBox.ResultEnum.NO && myCockpit != null && !myCockpit.Closed && myCockpit.Pilot != null)
				{
					myCockpit.RequestRemovePilot();
				}
				RemoveBlockInternal(m_removalTemporalData.Block, m_removalTemporalData.BlockIdInCompound, m_removalTemporalData.CheckExisting);
				Remove();
			}
			m_removalTemporalData = null;
		}

		private void Change(int expand = 0)
		{
			m_tmpBlockPositionList.Clear();
			if (expand == -1)
			{
				CurrentGrid.SkinGrid(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), playSound: true, validateOwnership: true, MyGuiScreenColorPicker.ApplyColor, MyGuiScreenColorPicker.ApplySkin);
				return;
			}
			int num = -1;
			bool flag = false;
			MyCubeBuilderGizmo.MyGizmoSpaceProperties[] spaces = m_gizmo.Spaces;
			foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties in spaces)
			{
				num++;
				if (myGizmoSpaceProperties.Enabled && myGizmoSpaceProperties.m_removeBlock != null)
				{
					flag = false;
					Vector3I vector3I = myGizmoSpaceProperties.m_removeBlock.Position - Vector3I.One * expand;
					Vector3I vector3I2 = myGizmoSpaceProperties.m_removeBlock.Position + Vector3I.One * expand;
					if (m_currColoringArea[num].Start != vector3I || m_currColoringArea[num].End != vector3I2)
					{
						m_currColoringArea[num].Start = vector3I;
						m_currColoringArea[num].End = vector3I2;
						flag = true;
					}
					CurrentGrid.SkinBlocks(vector3I, vector3I2, MyGuiScreenColorPicker.ApplyColor ? new Vector3?(MyPlayer.SelectedColor) : null, MyGuiScreenColorPicker.ApplySkin ? new MyStringHash?(MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)) : null, flag, validateOwnership: true);
				}
			}
		}

		private Vector3I? GetSingleMountPointNormal()
		{
			if (CurrentBlockDefinition == null)
			{
				return null;
			}
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = CurrentBlockDefinition.GetBuildProgressModelMountPoints(1f);
			if (buildProgressModelMountPoints == null || buildProgressModelMountPoints.Length == 0)
			{
				return null;
			}
			Vector3I normal = buildProgressModelMountPoints[0].Normal;
			if (AlignToDefault && !m_customRotation)
			{
				for (int i = 0; i < buildProgressModelMountPoints.Length; i++)
				{
					if (buildProgressModelMountPoints[i].Default)
					{
						return buildProgressModelMountPoints[i].Normal;
					}
				}
				for (int j = 0; j < buildProgressModelMountPoints.Length; j++)
				{
					if (MyCubeBlockDefinition.NormalToBlockSide(buildProgressModelMountPoints[j].Normal) == BlockSideEnum.Bottom)
					{
						return buildProgressModelMountPoints[j].Normal;
					}
				}
			}
			Vector3I b = -normal;
			switch (CurrentBlockDefinition.AutorotateMode)
			{
			case MyAutorotateMode.OneDirection:
			{
				for (int l = 1; l < buildProgressModelMountPoints.Length; l++)
				{
					if (buildProgressModelMountPoints[l].Normal != normal)
					{
						return null;
					}
				}
				break;
			}
			case MyAutorotateMode.OppositeDirections:
			{
				for (int k = 1; k < buildProgressModelMountPoints.Length; k++)
				{
					Vector3I normal2 = buildProgressModelMountPoints[k].Normal;
					if (normal2 != normal && normal2 != b)
					{
						return null;
					}
				}
				break;
			}
			default:
				return null;
			case MyAutorotateMode.FirstDirection:
				break;
			}
			return normal;
		}

		public void CycleCubePlacementMode()
		{
			switch (CubePlacementMode)
			{
			case CubePlacementModeEnum.LocalCoordinateSystem:
				CubePlacementMode = CubePlacementModeEnum.FreePlacement;
				break;
			case CubePlacementModeEnum.FreePlacement:
				CubePlacementMode = CubePlacementModeEnum.GravityAligned;
				break;
			case CubePlacementModeEnum.GravityAligned:
				CubePlacementMode = ((CurrentBlockDefinition == null || CurrentBlockDefinition.CubeSize != 0) ? CubePlacementModeEnum.FreePlacement : CubePlacementModeEnum.LocalCoordinateSystem);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void ShowCubePlacementNotification()
		{
			MyHud.Notifications.Remove(m_coloringToolHints);
			string text = string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.FREE_ROTATION), "]");
			switch (CubePlacementMode)
			{
			case CubePlacementModeEnum.LocalCoordinateSystem:
				m_cubePlacementModeNotification.SetTextFormatArguments(MyTexts.GetString(MyCommonTexts.NotificationCubePlacementMode_LocalCoordSystem));
				m_cubePlacementModeHint.SetTextFormatArguments(text, MyTexts.GetString(MyCommonTexts.ControlHintCubePlacementMode_LocalCoordSystem));
				break;
			case CubePlacementModeEnum.FreePlacement:
				m_cubePlacementModeNotification.SetTextFormatArguments(MyTexts.GetString(MyCommonTexts.NotificationCubePlacementMode_FreePlacement));
				m_cubePlacementModeHint.SetTextFormatArguments(text, MyTexts.GetString(MyCommonTexts.ControlHintCubePlacementMode_FreePlacement));
				break;
			case CubePlacementModeEnum.GravityAligned:
				m_cubePlacementModeNotification.SetTextFormatArguments(MyTexts.GetString(MyCommonTexts.NotificationCubePlacementMode_GravityAligned));
				m_cubePlacementModeHint.SetTextFormatArguments(text, MyTexts.GetString(MyCommonTexts.ControlHintCubePlacementMode_GravityAligned));
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			UpdatePlacementNotificationState();
			MyHud.Notifications.Add(m_cubePlacementModeNotification);
			MyHud.Notifications.Remove(m_cubePlacementModeHint);
			MyHud.Notifications.Add(m_cubePlacementModeHint);
		}

		private void ShowColorToolNotifications()
		{
			MyHud.Notifications.Remove(m_cubePlacementModeNotification);
			MyHud.Notifications.Remove(m_cubePlacementModeHint);
			string text;
			string text2;
			if (MyInput.Static.IsJoystickLastUsed)
			{
				IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
				if (controlledEntity == null)
				{
					_ = MyStringId.NullOrEmpty;
				}
				else
				{
					_ = controlledEntity.AuxiliaryContext;
				}
				text = '\ue006'.ToString();
				text2 = '\ue005'.ToString();
			}
			else
			{
				text = "[Ctrl]";
				text2 = "[Shift]";
			}
			m_coloringToolHints.SetTextFormatArguments(text, text2);
			MyHud.Notifications.Remove(m_coloringToolHints);
			MyHud.Notifications.Add(m_coloringToolHints);
		}

		private void CalculateCubePlacement()
		{
			if (!IsActivated || CurrentBlockDefinition == null)
			{
				return;
			}
			ChooseHitObject();
			Vector3D worldPos = m_hitInfo.HasValue ? m_hitInfo.Value.Position : (MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDistance * MyBlockBuilderBase.IntersectionDirection);
			if (CurrentBlockDefinition != null)
			{
				float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
				if (MyCoordinateSystem.Static != null)
				{
					double num = (worldPos - m_lastLocalCoordSysData.Origin.Position).LengthSquared();
					long value = (m_currentGrid != null && m_currentGrid.LocalCoordSystem != m_lastLocalCoordSysData.Id) ? m_currentGrid.LocalCoordSystem : ((num > (double)MyCoordinateSystem.Static.CoordSystemSizeSquared) ? 0 : m_lastLocalCoordSysData.Id);
					m_lastLocalCoordSysData = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref worldPos, cubeSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, value);
				}
				switch (CubePlacementMode)
				{
				case CubePlacementModeEnum.LocalCoordinateSystem:
					CalculateLocalCoordinateSystemMode(worldPos);
					break;
				case CubePlacementModeEnum.FreePlacement:
					CalculateFreePlacementMode(worldPos);
					break;
				case CubePlacementModeEnum.GravityAligned:
					CalculateGravityAlignedMode(worldPos);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		protected void CalculateLocalCoordinateSystemMode(Vector3D position)
		{
			DynamicMode = (m_currentGrid == null && m_currentVoxelBase == null);
		}

		protected void CalculateFreePlacementMode(Vector3D position)
		{
			DynamicMode = (m_currentGrid == null || IsDynamicOverride());
		}

		protected void CalculateGravityAlignedMode(Vector3D position)
		{
			DynamicMode = (m_currentGrid == null || IsDynamicOverride());
			if (!m_animationLock)
			{
				AlignToGravity(alignToCamera: false);
			}
		}

		public override void UpdateBeforeSimulation()
		{
			UpdateNotificationBlockLimit();
			MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
			if (Static.IsActivated && myShipController != null)
			{
				if (myShipController.hasPower && myShipController.BuildingMode && MyEntities.IsInsideWorld(myShipController.PositionComp.GetPosition()))
				{
					Static.canBuild = true;
				}
				else
				{
					Static.canBuild = false;
				}
			}
			else
			{
				Static.canBuild = true;
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			CalculateCubePlacement();
		}

		protected override void UnloadData()
		{
			Deactivate();
			base.UnloadData();
			RemoveSymmetryNotification();
			m_gizmo.Clear();
			CurrentGrid = null;
			UnloadRenderObjects();
			m_cubeBuilderState = null;
		}

		private void UnloadRenderObjects()
		{
			m_gizmo.RemoveGizmoCubeParts();
			m_renderData.UnloadRenderObjects();
		}

		private void UpdateNotificationBlockLimit()
		{
		}

		public void UpdateNotificationBlockNotAvailable(bool updateNotAvailableNotification)
		{
			if (!MyFakes.ENABLE_NOTIFICATION_BLOCK_NOT_AVAILABLE)
			{
				return;
			}
			if (!updateNotAvailableNotification)
			{
				HideNotificationBlockNotAvailable();
				return;
			}
			bool flag = MySession.Static.GetCameraControllerEnum() != 0 && false;
			bool flag2 = MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity is MyCockpit && !flag;
			if (BlockCreationIsActivated && CurrentBlockDefinition != null)
			{
				if (CurrentGrid != null && CurrentBlockDefinition.CubeSize != CurrentGrid.GridSizeEnum && !flag2 && !PlacingSmallGridOnLargeStatic)
				{
					MyStringId grid1Text = (CurrentGrid.GridSizeEnum == MyCubeSize.Small) ? MySpaceTexts.NotificationArgLargeShip : MySpaceTexts.NotificationArgSmallShip;
					MyStringId grid2Text = (CurrentGrid.GridSizeEnum == MyCubeSize.Small) ? MySpaceTexts.NotificationArgSmallShip : (CurrentGrid.IsStatic ? MySpaceTexts.NotificationArgStation : MySpaceTexts.NotificationArgLargeShip);
					ShowNotificationBlockNotAvailable(grid1Text, CurrentBlockDefinition.DisplayNameText, grid2Text);
				}
				else if (BlockCreationIsActivated && CurrentBlockDefinition != null && CurrentGrid == null)
				{
					MyStringId grid1Text2 = (CurrentBlockDefinition.CubeSize == MyCubeSize.Small) ? MySpaceTexts.NotificationArgSmallShip : MySpaceTexts.NotificationArgLargeShip;
					MyStringId grid2Text2 = (CurrentBlockDefinition.CubeSize == MyCubeSize.Small) ? MySpaceTexts.NotificationArgLargeShip : MySpaceTexts.NotificationArgSmallShip;
					ShowNotificationBlockNotAvailable(grid1Text2, CurrentBlockDefinition.DisplayNameText, grid2Text2);
				}
			}
		}

		private void ShowNotificationBlockNotAvailable(MyStringId grid1Text, string blockDisplayName, MyStringId grid2Text)
		{
			if (MyFakes.ENABLE_NOTIFICATION_BLOCK_NOT_AVAILABLE)
			{
				if (m_blockNotAvailableNotification == null)
				{
					m_blockNotAvailableNotification = new MyHudNotification(MySpaceTexts.NotificationBlockNotAvailableFor, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 1);
				}
				m_blockNotAvailableNotification.SetTextFormatArguments(MyTexts.Get(grid1Text).ToLower().FirstLetterUpperCase(), blockDisplayName.ToLower(), MyTexts.Get(grid2Text).ToLower());
				MyHud.Notifications.Add(m_blockNotAvailableNotification);
			}
		}

		private void HideNotificationBlockNotAvailable()
		{
			if (m_blockNotAvailableNotification != null && m_blockNotAvailableNotification.Alive)
			{
				MyHud.Notifications.Remove(m_blockNotAvailableNotification);
			}
		}

		public virtual void StartBuilding()
		{
			StartBuilding(ref m_gizmo.SpaceDefault.m_startBuild, m_gizmo.SpaceDefault.m_startRemove);
		}

		protected void StartBuilding(ref Vector3I? startBuild, Vector3I? startRemove)
		{
			if ((GridAndBlockValid || VoxelMapAndBlockValid) && !PlacingSmallGridOnLargeStatic)
			{
				m_initialIntersectionStart = MyBlockBuilderBase.IntersectionStart;
				m_initialIntersectionDirection = MyBlockBuilderBase.IntersectionDirection;
				float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
				if (!startRemove.HasValue && GetAddAndRemovePositions(cubeSize, PlacingSmallGridOnLargeStatic, out Vector3I addPos, out Vector3? _, out Vector3I _, out Vector3I _, out MySlimBlock _, out ushort? _, null))
				{
					startBuild = addPos;
				}
				else
				{
					startBuild = null;
				}
			}
		}

		protected virtual void StartRemoving()
		{
			StartRemoving(m_gizmo.SpaceDefault.m_startBuild, ref m_gizmo.SpaceDefault.m_startRemove);
		}

		protected void StartRemoving(Vector3I? startBuild, ref Vector3I? startRemove)
		{
			if (!PlacingSmallGridOnLargeStatic)
			{
				m_initialIntersectionStart = MyBlockBuilderBase.IntersectionStart;
				m_initialIntersectionDirection = MyBlockBuilderBase.IntersectionDirection;
				if (CurrentGrid != null && !startBuild.HasValue)
				{
					startRemove = IntersectCubes(CurrentGrid, out double _);
				}
			}
		}

		public virtual void ContinueBuilding(bool planeBuild)
		{
			MyCubeBuilderGizmo.MyGizmoSpaceProperties spaceDefault = m_gizmo.SpaceDefault;
			ContinueBuilding(planeBuild, spaceDefault.m_startBuild, spaceDefault.m_startRemove, ref spaceDefault.m_continueBuild, spaceDefault.m_min, spaceDefault.m_max);
		}

		protected void ContinueBuilding(bool planeBuild, Vector3I? startBuild, Vector3I? startRemove, ref Vector3I? continueBuild, Vector3I blockMinPosision, Vector3I blockMaxPosition)
		{
			if ((!startBuild.HasValue && !startRemove.HasValue) || (!GridAndBlockValid && !VoxelMapAndBlockValid))
			{
				return;
			}
			continueBuild = null;
			if (CheckSmallViewChange())
			{
				return;
			}
			IntersectInflated(m_cacheGridIntersections, CurrentGrid);
			Vector3I vector3I = startBuild.HasValue ? blockMinPosision : startRemove.Value;
			Vector3I vector3I2 = startBuild.HasValue ? blockMaxPosition : startRemove.Value;
			Vector3I b = default(Vector3I);
			b.X = vector3I.X;
			while (b.X <= vector3I2.X)
			{
				b.Y = vector3I.Y;
				while (b.Y <= vector3I2.Y)
				{
					b.Z = vector3I.Z;
					while (b.Z <= vector3I2.Z)
					{
						if (planeBuild)
						{
							foreach (Vector3I cacheGridIntersection in m_cacheGridIntersections)
							{
								if (cacheGridIntersection.X == b.X || cacheGridIntersection.Y == b.Y || cacheGridIntersection.Z == b.Z)
								{
									_ = Vector3.Zero;
									_ = Vector3.Zero;
									if (cacheGridIntersection.X == b.X)
									{
										if (CurrentGrid != null)
										{
											_ = (Vector3)CurrentGrid.WorldMatrix.Up;
											_ = (Vector3)CurrentGrid.WorldMatrix.Forward;
										}
										else
										{
											_ = Vector3.Up;
											_ = Vector3.Forward;
										}
									}
									else if (cacheGridIntersection.Y == b.Y)
									{
										if (CurrentGrid != null)
										{
											_ = (Vector3)CurrentGrid.WorldMatrix.Right;
											_ = (Vector3)CurrentGrid.WorldMatrix.Forward;
										}
										else
										{
											_ = Vector3.Right;
											_ = Vector3.Forward;
										}
									}
									else if (cacheGridIntersection.Z == b.Z)
									{
										if (CurrentGrid != null)
										{
											_ = (Vector3)CurrentGrid.WorldMatrix.Up;
											_ = (Vector3)CurrentGrid.WorldMatrix.Right;
										}
										else
										{
											_ = Vector3.Up;
											_ = Vector3.Right;
										}
									}
									Vector3I vector3I3 = Vector3I.Abs(cacheGridIntersection - b) + Vector3I.One;
									if (vector3I3.Size < 2048 && vector3I3.AbsMax() <= 255)
									{
										continueBuild = cacheGridIntersection;
										break;
									}
								}
							}
						}
						else
						{
							foreach (Vector3I cacheGridIntersection2 in m_cacheGridIntersections)
							{
								if (((cacheGridIntersection2.X == b.X && cacheGridIntersection2.Y == b.Y) || (cacheGridIntersection2.Y == b.Y && cacheGridIntersection2.Z == b.Z) || (cacheGridIntersection2.X == b.X && cacheGridIntersection2.Z == b.Z)) && (cacheGridIntersection2 - b + Vector3I.One).AbsMax() <= 255)
								{
									continueBuild = cacheGridIntersection2;
									break;
								}
							}
						}
						b.Z++;
					}
					b.Y++;
				}
				b.X++;
			}
		}

		public virtual void StopBuilding()
		{
			MyCubeBuilderGizmo.MyGizmoSpaceProperties[] spaces;
			if (!GridAndBlockValid && !VoxelMapAndBlockValid)
			{
				spaces = m_gizmo.Spaces;
				foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties obj in spaces)
				{
					obj.m_startBuild = null;
					obj.m_continueBuild = null;
					obj.m_startRemove = null;
				}
				return;
			}
			bool smallViewChange = CheckSmallViewChange();
			m_blocksBuildQueue.Clear();
			m_tmpBlockPositionList.Clear();
			UpdateGizmos(addPos: true, removePos: true, draw: false);
			int num = 0;
			spaces = m_gizmo.Spaces;
			for (int i = 0; i < spaces.Length; i++)
			{
				if (spaces[i].Enabled)
				{
					num++;
				}
			}
			spaces = m_gizmo.Spaces;
			foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties myGizmoSpaceProperties in spaces)
			{
				if (myGizmoSpaceProperties.Enabled)
				{
					StopBuilding(smallViewChange, ref myGizmoSpaceProperties.m_startBuild, ref myGizmoSpaceProperties.m_startRemove, ref myGizmoSpaceProperties.m_continueBuild, myGizmoSpaceProperties.m_min, myGizmoSpaceProperties.m_max, myGizmoSpaceProperties.m_centerPos, ref myGizmoSpaceProperties.m_localMatrixAdd, myGizmoSpaceProperties.m_blockDefinition);
				}
			}
			if (m_blocksBuildQueue.Count > 0)
			{
				CurrentGrid.BuildBlocks(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), m_blocksBuildQueue, MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId);
				m_blocksBuildQueue.Clear();
			}
			if (m_tmpBlockPositionList.Count > 0)
			{
				CurrentGrid.RazeBlocks(m_tmpBlockPositionList, MySession.Static.LocalCharacterEntityId, 0uL);
				m_tmpBlockPositionList.Clear();
			}
		}

		protected void StopBuilding(bool smallViewChange, ref Vector3I? startBuild, ref Vector3I? startRemove, ref Vector3I? continueBuild, Vector3I blockMinPosition, Vector3I blockMaxPosition, Vector3I blockCenterPosition, ref Matrix localMatrixAdd, MyCubeBlockDefinition blockDefinition)
		{
			if (startBuild.HasValue && (continueBuild.HasValue || smallViewChange))
			{
				Vector3I vec = blockMinPosition - blockCenterPosition;
				Vector3I vec2 = blockMaxPosition - blockCenterPosition;
				Vector3I.TransformNormal(ref CurrentBlockDefinition.Size, ref localMatrixAdd, out Vector3I result);
				result = Vector3I.Abs(result);
				if (smallViewChange)
				{
					continueBuild = startBuild;
				}
				MyBlockBuilderBase.ComputeSteps(startBuild.Value, continueBuild.Value, result, out Vector3I stepDelta, out Vector3I counter, out int _);
				Quaternion rot = Quaternion.CreateFromRotationMatrix(localMatrixAdd);
				MyDefinitionId id = blockDefinition.Id;
				if (blockDefinition.RandomRotation && blockDefinition.Size.X == blockDefinition.Size.Y && blockDefinition.Size.X == blockDefinition.Size.Z && (blockDefinition.Rotation == MyBlockRotation.Both || blockDefinition.Rotation == MyBlockRotation.Vertical))
				{
					m_blocksBuildQueue.Clear();
					Vector3I a = default(Vector3I);
					a.X = 0;
					while (a.X < counter.X)
					{
						a.Y = 0;
						while (a.Y < counter.Y)
						{
							a.Z = 0;
							while (a.Z < counter.Z)
							{
								Vector3I center = blockCenterPosition + a * stepDelta;
								Vector3I min = blockMinPosition + a * stepDelta;
								Vector3I max = blockMaxPosition + a * stepDelta;
								Quaternion orientation;
								if (blockDefinition.Rotation == MyBlockRotation.Both)
								{
									Base6Directions.Direction direction = (Base6Directions.Direction)(Math.Abs(MyRandom.Instance.Next()) % 6);
									Base6Directions.Direction dir = direction;
									while (Vector3I.Dot(Base6Directions.GetIntVector(direction), Base6Directions.GetIntVector(dir)) != 0)
									{
										dir = (Base6Directions.Direction)(Math.Abs(MyRandom.Instance.Next()) % 6);
									}
									orientation = Quaternion.CreateFromForwardUp(Base6Directions.GetIntVector(direction), Base6Directions.GetIntVector(dir));
								}
								else
								{
									Base6Directions.Direction direction2 = Base6Directions.Direction.Up;
									Base6Directions.Direction dir2 = direction2;
									while (Vector3I.Dot(Base6Directions.GetIntVector(dir2), Base6Directions.GetIntVector(direction2)) != 0)
									{
										dir2 = (Base6Directions.Direction)(Math.Abs(MyRandom.Instance.Next()) % 6);
									}
									orientation = Quaternion.CreateFromForwardUp(Base6Directions.GetIntVector(dir2), Base6Directions.GetIntVector(direction2));
								}
								m_blocksBuildQueue.Add(new MyCubeGrid.MyBlockLocation(blockDefinition.Id, min, max, center, orientation, MyEntityIdentifier.AllocateId(), MySession.Static.LocalPlayerId));
								a.Z++;
							}
							a.Y++;
						}
						a.X++;
					}
					if (m_blocksBuildQueue.Count > 0)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
					}
				}
				else
				{
					MyCubeGrid.MyBlockBuildArea area = default(MyCubeGrid.MyBlockBuildArea);
					area.PosInGrid = blockCenterPosition;
					area.BlockMin = new Vector3B(vec);
					area.BlockMax = new Vector3B(vec2);
					area.BuildAreaSize = new Vector3UByte(counter);
					area.StepDelta = new Vector3B(stepDelta);
					area.OrientationForward = Base6Directions.GetForward(ref rot);
					area.OrientationUp = Base6Directions.GetUp(ref rot);
					area.DefinitionId = id;
					area.ColorMaskHSV = MyPlayer.SelectedColor.PackHSVToUint();
					area.SkinId = MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin);
					CurrentGrid.BuildBlocks(ref area, MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId);
					if (this.OnBlockAdded != null)
					{
						this.OnBlockAdded(blockDefinition);
					}
				}
			}
			else if (startRemove.HasValue && (continueBuild.HasValue || smallViewChange))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudDeleteBlock);
				Vector3I value = startRemove.Value;
				Vector3I value2 = startRemove.Value;
				if (smallViewChange)
				{
					continueBuild = startRemove;
				}
				MyBlockBuilderBase.ComputeSteps(startRemove.Value, continueBuild.Value, Vector3I.One, out Vector3I _, out Vector3I _, out int _);
				value = Vector3I.Min(startRemove.Value, continueBuild.Value);
				value2 = Vector3I.Max(startRemove.Value, continueBuild.Value);
				Vector3UByte size = new Vector3UByte(value2 - value);
				MyStringId context = (IsActivated && MySession.Static.ControlledEntity is MyCharacter) ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
				MyCharacterDetectorComponent myCharacterDetectorComponent = MySession.Static.LocalCharacter.Components.Get<MyCharacterDetectorComponent>();
				if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null)
				{
					myCharacterDetectorComponent.UseObject.SupportedActions.HasFlag(UseActionEnum.BuildPlanner);
				}
				MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER, MyControlStateType.NEW_RELEASED);
				if (0 == 0)
				{
					CurrentGrid.RazeBlocksDelayed(ref value, ref size, MySession.Static.LocalCharacterEntityId);
				}
			}
			startBuild = null;
			continueBuild = null;
			startRemove = null;
		}

		protected virtual bool CancelBuilding()
		{
			if (m_gizmo.SpaceDefault.m_continueBuild.HasValue)
			{
				m_gizmo.SpaceDefault.m_startBuild = null;
				m_gizmo.SpaceDefault.m_startRemove = null;
				m_gizmo.SpaceDefault.m_continueBuild = null;
				return true;
			}
			return false;
		}

		protected virtual bool IsBuilding()
		{
			if (!m_gizmo.SpaceDefault.m_startBuild.HasValue)
			{
				return m_gizmo.SpaceDefault.m_startRemove.HasValue;
			}
			return true;
		}

		protected bool CheckSmallViewChange()
		{
			double num = Vector3D.Dot(m_initialIntersectionDirection, MyBlockBuilderBase.IntersectionDirection);
			double num2 = (m_initialIntersectionStart - MyBlockBuilderBase.IntersectionStart).Length();
			if (num > 0.99800002574920654)
			{
				return num2 < 0.25;
			}
			return false;
		}

		protected internal override void ChooseHitObject()
		{
			if (!IsBuilding())
			{
				base.ChooseHitObject();
				m_gizmo.Clear();
			}
		}

		private Vector3D GetFreeSpacePlacementPosition(out bool valid)
		{
			valid = false;
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize);
			Vector3 halfExtents = CurrentBlockDefinition.Size * cubeSize * 0.5f;
			MatrixD transform = m_gizmo.SpaceDefault.m_worldMatrixAdd;
			Vector3D intersectionStart = MyBlockBuilderBase.IntersectionStart;
			Vector3D freePlacementTarget = base.FreePlacementTarget;
			transform.Translation = intersectionStart;
			HkShape shape = new HkBoxShape(halfExtents);
			double num = double.MaxValue;
			try
			{
				float? num2 = MyPhysics.CastShape(freePlacementTarget, shape, ref transform, 30);
				if (num2.HasValue && num2.Value != 0f)
				{
					num = (intersectionStart + num2.Value * (freePlacementTarget - intersectionStart) - MyBlockBuilderBase.IntersectionStart).Length() * 0.98;
					valid = true;
				}
			}
			finally
			{
				shape.RemoveReference();
			}
			float num3 = LowLimitDistanceForDynamicMode();
			if (num < (double)num3)
			{
				num = MyBlockBuilderBase.IntersectionDistance;
				valid = false;
			}
			if (num > (double)MyBlockBuilderBase.IntersectionDistance)
			{
				num = MyBlockBuilderBase.IntersectionDistance;
				valid = false;
			}
			if (!MyEntities.IsInsideWorld(MyBlockBuilderBase.IntersectionStart + num * MyBlockBuilderBase.IntersectionDirection))
			{
				valid = false;
			}
			return MyBlockBuilderBase.IntersectionStart + num * MyBlockBuilderBase.IntersectionDirection;
		}

		private float LowLimitDistanceForDynamicMode()
		{
			if (CurrentBlockDefinition != null)
			{
				return MyDefinitionManager.Static.GetCubeSize(CurrentBlockDefinition.CubeSize) + 0.1f;
			}
			return 2.6f;
		}

		protected static void UpdateBlockInfoHud()
		{
			MyHud.BlockInfo.Visible = false;
			MyCubeBlockDefinition currentBlockDefinition = Static.CurrentBlockDefinition;
			if (currentBlockDefinition != null && Static.IsActivated && (MyFakes.ENABLE_SMALL_GRID_BLOCK_INFO || currentBlockDefinition == null || currentBlockDefinition.CubeSize != MyCubeSize.Small))
			{
				MySlimBlock.SetBlockComponents(MyHud.BlockInfo, currentBlockDefinition, BuildComponent.GetBuilderInventory(MySession.Static.LocalCharacter));
				MyHud.BlockInfo.Visible = Static.IsBuildToolActive();
			}
		}

		public void StartStaticGridPlacement(MyCubeSize cubeSize, bool isStatic)
		{
			MySession.Static.LocalCharacter?.SwitchToWeapon((MyToolbarItemWeapon)null);
			MyDefinitionId defId = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
			if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out MyCubeBlockDefinition blockDefinition))
			{
				Activate(blockDefinition.Id);
				m_stationPlacement = true;
			}
		}

		protected static MyObjectBuilder_CubeGrid CreateMultiBlockGridBuilder(MyMultiBlockDefinition multiCubeBlockDefinition, Matrix rotationMatrix, Vector3D position = default(Vector3D))
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(position, rotationMatrix.Forward, rotationMatrix.Up);
			myObjectBuilder_CubeGrid.IsStatic = false;
			myObjectBuilder_CubeGrid.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			if (multiCubeBlockDefinition.BlockDefinitions == null)
			{
				return null;
			}
			MyCubeSize? myCubeSize = null;
			Vector3I value = Vector3I.MaxValue;
			Vector3I value2 = Vector3I.MinValue;
			int num;
			for (num = MyRandom.Instance.Next(); num == 0; num = MyRandom.Instance.Next())
			{
			}
			for (int i = 0; i < multiCubeBlockDefinition.BlockDefinitions.Length; i++)
			{
				MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition = multiCubeBlockDefinition.BlockDefinitions[i];
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out MyCubeBlockDefinition blockDefinition);
				if (blockDefinition == null)
				{
					continue;
				}
				if (!myCubeSize.HasValue)
				{
					myCubeSize = blockDefinition.CubeSize;
				}
				else if (myCubeSize.Value != blockDefinition.CubeSize)
				{
					continue;
				}
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(blockDefinition.Id) as MyObjectBuilder_CubeBlock;
				myObjectBuilder_CubeBlock.Orientation = Base6Directions.GetOrientation(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up);
				myObjectBuilder_CubeBlock.Min = myMultiBlockPartDefinition.Min;
				myObjectBuilder_CubeBlock.ColorMaskHSV = MyPlayer.SelectedColor;
				myObjectBuilder_CubeBlock.SkinSubtypeId = MyPlayer.SelectedArmorSkin;
				myObjectBuilder_CubeBlock.MultiBlockId = num;
				myObjectBuilder_CubeBlock.MultiBlockIndex = i;
				myObjectBuilder_CubeBlock.MultiBlockDefinition = multiCubeBlockDefinition.Id;
				myObjectBuilder_CubeBlock.EntityId = MyEntityIdentifier.AllocateId();
				bool flag = false;
				bool flag2 = true;
				bool flag3 = MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition);
				foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
				{
					if (cubeBlock.Min == myObjectBuilder_CubeBlock.Min)
					{
						if (MyFakes.ENABLE_COMPOUND_BLOCKS && cubeBlock is MyObjectBuilder_CompoundCubeBlock)
						{
							if (flag3)
							{
								MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
								MyObjectBuilder_CubeBlock[] array = new MyObjectBuilder_CubeBlock[myObjectBuilder_CompoundCubeBlock.Blocks.Length + 1];
								Array.Copy(myObjectBuilder_CompoundCubeBlock.Blocks, array, myObjectBuilder_CompoundCubeBlock.Blocks.Length);
								array[array.Length - 1] = myObjectBuilder_CubeBlock;
								myObjectBuilder_CompoundCubeBlock.Blocks = array;
								flag = true;
							}
							else
							{
								flag2 = false;
							}
						}
						else
						{
							flag2 = false;
						}
						break;
					}
				}
				if (!flag2)
				{
					continue;
				}
				if (!flag)
				{
					if (MyFakes.ENABLE_COMPOUND_BLOCKS && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
					{
						MyObjectBuilder_CompoundCubeBlock item = MyCompoundCubeBlock.CreateBuilder(myObjectBuilder_CubeBlock);
						myObjectBuilder_CubeGrid.CubeBlocks.Add(item);
					}
					else
					{
						myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CubeBlock);
					}
				}
				value = Vector3I.Min(value, myMultiBlockPartDefinition.Min);
				value2 = Vector3I.Max(value2, myMultiBlockPartDefinition.Min);
			}
			if (myObjectBuilder_CubeGrid.CubeBlocks.Count == 0)
			{
				return null;
			}
			myObjectBuilder_CubeGrid.GridSizeEnum = myCubeSize.Value;
			return myObjectBuilder_CubeGrid;
		}

		protected static void AfterGridBuild(MyEntity builder, MyCubeGrid grid, bool instantBuild, ulong senderId)
		{
			if (grid == null || grid.Closed)
			{
				SpawnGridReply(canSpawn: false, senderId);
				return;
			}
			MySlimBlock cubeBlock = grid.GetCubeBlock(Vector3I.Zero);
			if (cubeBlock == null)
			{
				return;
			}
			if (grid.IsStatic)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				MySlimBlock mySlimBlock = (myCompoundCubeBlock != null && myCompoundCubeBlock.GetBlocksCount() > 0) ? myCompoundCubeBlock.GetBlocks()[0] : null;
				MyCubeGrid myCubeGrid = grid.DetectMerge(cubeBlock, null, null, newGrid: true);
				if (myCubeGrid == null)
				{
					myCubeGrid = grid;
				}
				MySlimBlock mainBlock = cubeBlock;
				if (mySlimBlock != null)
				{
					mainBlock = myCubeGrid.GetCubeBlock(mySlimBlock.Position);
				}
				myCubeGrid.AdditionalModelGenerators.ForEach(delegate(IMyBlockAdditionalModelGenerator g)
				{
					g.UpdateAfterGridSpawn(mainBlock);
				});
				if (MyCubeGridSmallToLargeConnection.Static != null && Sync.IsServer && !MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(cubeBlock) && grid.GridSizeEnum == MyCubeSize.Small)
				{
					cubeBlock.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridCopied;
				}
			}
			if (Sync.IsServer)
			{
				BuildComponent.AfterSuccessfulBuild(builder, instantBuild);
			}
			if (cubeBlock.FatBlock != null)
			{
				cubeBlock.FatBlock.OnBuildSuccess(cubeBlock.BuiltBy, instantBuild);
			}
			if (grid.IsStatic && grid.GridSizeEnum != MyCubeSize.Small)
			{
				MatrixD tranform = grid.WorldMatrix;
				if (MyCoordinateSystem.Static.IsLocalCoordSysExist(ref tranform, grid.GridSize))
				{
					MyCoordinateSystem.Static.RegisterCubeGrid(grid);
				}
				else
				{
					MyCoordinateSystem.Static.CreateCoordSys(grid, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, sync: true);
				}
			}
			MyCubeGrids.NotifyBlockBuilt(grid, cubeBlock);
			SpawnGridReply(canSpawn: true, senderId);
		}

		public static MyCubeGrid SpawnStaticGrid(MyCubeBlockDefinition blockDefinition, MyEntity builder, MatrixD worldMatrix, Vector3 color, MyStringHash skinId, SpawnFlags spawnFlags = SpawnFlags.Default, long builtBy = 0L, Action<MyEntity> completionCallback = null)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
			Vector3 value = Vector3.TransformNormal(MyCubeBlock.GetBlockGridOffset(blockDefinition), worldMatrix);
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix.Translation - value, worldMatrix.Forward, worldMatrix.Up);
			myObjectBuilder_CubeGrid.GridSizeEnum = blockDefinition.CubeSize;
			myObjectBuilder_CubeGrid.IsStatic = true;
			myObjectBuilder_CubeGrid.CreatePhysics = ((spawnFlags & SpawnFlags.CreatePhysics) != 0);
			myObjectBuilder_CubeGrid.EnableSmallToLargeConnections = ((spawnFlags & SpawnFlags.EnableSmallTolargeConnections) != 0);
			myObjectBuilder_CubeGrid.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			if ((spawnFlags & SpawnFlags.AddToScene) != 0)
			{
				myObjectBuilder_CubeGrid.EntityId = MyEntityIdentifier.AllocateId();
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(blockDefinition.Id) as MyObjectBuilder_CubeBlock;
			myObjectBuilder_CubeBlock.Orientation = Quaternion.CreateFromForwardUp(Vector3I.Forward, Vector3I.Up);
			myObjectBuilder_CubeBlock.Min = blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One;
			if ((spawnFlags & SpawnFlags.AddToScene) != 0)
			{
				myObjectBuilder_CubeBlock.EntityId = MyEntityIdentifier.AllocateId();
			}
			myObjectBuilder_CubeBlock.ColorMaskHSV = color;
			myObjectBuilder_CubeBlock.SkinSubtypeId = skinId.String;
			myObjectBuilder_CubeBlock.BuiltBy = builtBy;
			myObjectBuilder_CubeBlock.Owner = builtBy;
			BuildComponent.BeforeCreateBlock(blockDefinition, builder, myObjectBuilder_CubeBlock, (spawnFlags & SpawnFlags.SpawnAsMaster) != 0);
			myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CubeBlock);
			if ((spawnFlags & SpawnFlags.AddToScene) != 0)
			{
				return MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_CubeGrid, addToScene: true, completionCallback, null, null, null, checkPosition: true) as MyCubeGrid;
			}
			return MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_CubeGrid, addToScene: false, completionCallback, null, null, null, checkPosition: true) as MyCubeGrid;
		}

		public static MyCubeGrid SpawnDynamicGrid(MyCubeBlockDefinition blockDefinition, MyEntity builder, MatrixD worldMatrix, Vector3 color, MyStringHash skinId, long entityId = 0L, SpawnFlags spawnFlags = SpawnFlags.Default, long builtBy = 0L, Action<MyEntity> completionCallback = null)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
			Vector3 value = Vector3.TransformNormal(MyCubeBlock.GetBlockGridOffset(blockDefinition), worldMatrix);
			Vector3D? relativeOffset = worldMatrix.Translation - value - builder.WorldMatrix.Translation;
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix.Translation - value, worldMatrix.Forward, worldMatrix.Up);
			myObjectBuilder_CubeGrid.GridSizeEnum = blockDefinition.CubeSize;
			myObjectBuilder_CubeGrid.IsStatic = false;
			myObjectBuilder_CubeGrid.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(blockDefinition.Id) as MyObjectBuilder_CubeBlock;
			myObjectBuilder_CubeBlock.Orientation = Quaternion.CreateFromForwardUp(Vector3I.Forward, Vector3I.Up);
			myObjectBuilder_CubeBlock.Min = blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One;
			myObjectBuilder_CubeBlock.ColorMaskHSV = color;
			myObjectBuilder_CubeBlock.SkinSubtypeId = skinId.String;
			myObjectBuilder_CubeBlock.BuiltBy = builtBy;
			myObjectBuilder_CubeBlock.Owner = builtBy;
			BuildComponent.BeforeCreateBlock(blockDefinition, builder, myObjectBuilder_CubeBlock, (spawnFlags & SpawnFlags.SpawnAsMaster) != 0);
			myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CubeBlock);
			MyCubeGrid result = null;
			if (builder != null)
			{
				MyEntity myEntity = (builder.Parent == null) ? builder : ((builder.Parent is MyCubeBlock) ? ((MyCubeBlock)builder.Parent).CubeGrid : builder.Parent);
				if (myEntity.Physics != null && myEntity.Physics.LinearVelocity.LengthSquared() >= 225f)
				{
					myObjectBuilder_CubeGrid.LinearVelocity = myEntity.Physics.LinearVelocity;
				}
			}
			if (entityId != 0L)
			{
				myObjectBuilder_CubeGrid.EntityId = entityId;
				myObjectBuilder_CubeBlock.EntityId = entityId + 1;
				result = (MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_CubeGrid, addToScene: true, completionCallback, null, null, null, checkPosition: true) as MyCubeGrid);
			}
			else if (Sync.IsServer)
			{
				myObjectBuilder_CubeGrid.EntityId = MyEntityIdentifier.AllocateId();
				myObjectBuilder_CubeBlock.EntityId = myObjectBuilder_CubeGrid.EntityId + 1;
				result = (MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_CubeGrid, addToScene: true, completionCallback, null, builder, relativeOffset, checkPosition: true) as MyCubeGrid);
			}
			return result;
		}

		public static void SelectBlockToToolbar(MySlimBlock block, bool selectToNextSlot = true)
		{
			MyDefinitionId v = block.BlockDefinition.Id;
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
				m_cycle %= myCompoundCubeBlock.GetBlocksCount();
				v = myCompoundCubeBlock.GetBlocks()[m_cycle].BlockDefinition.Id;
				m_cycle++;
			}
			if (block.FatBlock is MyFracturedBlock)
			{
				MyFracturedBlock myFracturedBlock = block.FatBlock as MyFracturedBlock;
				m_cycle %= myFracturedBlock.OriginalBlocks.Count;
				v = myFracturedBlock.OriginalBlocks[m_cycle];
				m_cycle++;
			}
			if (MyToolbarComponent.CurrentToolbar.SelectedSlot.HasValue)
			{
				int num = MyToolbarComponent.CurrentToolbar.SelectedSlot.Value;
				if (selectToNextSlot)
				{
					num++;
				}
				if (!MyToolbarComponent.CurrentToolbar.IsValidSlot(num))
				{
					num = 0;
				}
				MyToolbarItem item = MyToolbarItemFactory.CreateToolbarItem(new MyObjectBuilder_ToolbarItemCubeBlock
				{
					DefinitionId = v
				});
				MyToolbarComponent.CurrentToolbar.SetItemAtSlot(num, item);
			}
			else
			{
				int i;
				for (i = 0; MyToolbarComponent.CurrentToolbar.GetSlotItem(i) != null; i++)
				{
				}
				if (!MyToolbarComponent.CurrentToolbar.IsValidSlot(i))
				{
					i = 0;
				}
				MyToolbarItem item2 = MyToolbarItemFactory.CreateToolbarItem(new MyObjectBuilder_ToolbarItemCubeBlock
				{
					DefinitionId = v
				});
				MyToolbarComponent.CurrentToolbar.SetItemAtSlot(i, item2);
			}
		}

		private void BeforeCurrentGridChange(MyCubeGrid newCurrentGrid)
		{
			TriggerRespawnShipNotification(newCurrentGrid);
		}

		private void TriggerRespawnShipNotification(MyCubeGrid newCurrentGrid)
		{
			MyNotificationSingletons singleNotification = MySession.Static.Settings.RespawnShipDelete ? MyNotificationSingletons.RespawnShipWarning : MyNotificationSingletons.BuildingOnRespawnShipWarning;
			if (newCurrentGrid != null && newCurrentGrid.IsRespawnGrid)
			{
				MyHud.Notifications.Add(singleNotification);
			}
			else
			{
				MyHud.Notifications.Remove(singleNotification);
			}
		}

		public static double? GetCurrentRayIntersection()
		{
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + 2000.0 * MyBlockBuilderBase.IntersectionDirection, 30);
			if (hitInfo.HasValue)
			{
				return (hitInfo.Value.Position - MyBlockBuilderBase.IntersectionStart).Length();
			}
			return null;
		}

		public static Vector3 TransformLargeGridHitCoordToSmallGrid(Vector3D coords, MatrixD worldMatrixNormalizedInv, float gridSize)
		{
			Vector3D vector3D = Vector3D.Transform(coords, worldMatrixNormalizedInv);
			vector3D /= (double)gridSize;
			vector3D *= 10.0;
			Vector3I vector3I = Vector3I.Sign(vector3D);
			vector3D -= 0.5 * vector3I;
			vector3D = vector3I * Vector3I.Round(Vector3D.Abs(vector3D));
			vector3D += 0.5 * vector3I;
			vector3D /= 10.0;
			return vector3D;
		}

		public static MyObjectBuilder_CubeGrid ConvertGridBuilderToStatic(MyObjectBuilder_CubeGrid originalGrid, MatrixD worldMatrix)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
			myObjectBuilder_CubeGrid.EntityId = originalGrid.EntityId;
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix.Translation, worldMatrix.Forward, worldMatrix.Up);
			myObjectBuilder_CubeGrid.GridSizeEnum = originalGrid.GridSizeEnum;
			myObjectBuilder_CubeGrid.IsStatic = true;
			myObjectBuilder_CubeGrid.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			foreach (MyObjectBuilder_CubeBlock cubeBlock in originalGrid.CubeBlocks)
			{
				if (cubeBlock is MyObjectBuilder_CompoundCubeBlock)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock2 = ConvertDynamicGridBlockToStatic(ref worldMatrix, cubeBlock) as MyObjectBuilder_CompoundCubeBlock;
					if (myObjectBuilder_CompoundCubeBlock2 != null)
					{
						myObjectBuilder_CompoundCubeBlock2.Blocks = new MyObjectBuilder_CubeBlock[myObjectBuilder_CompoundCubeBlock.Blocks.Length];
						for (int i = 0; i < myObjectBuilder_CompoundCubeBlock.Blocks.Length; i++)
						{
							MyObjectBuilder_CubeBlock origBlock = myObjectBuilder_CompoundCubeBlock.Blocks[i];
							MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = ConvertDynamicGridBlockToStatic(ref worldMatrix, origBlock);
							if (myObjectBuilder_CubeBlock != null)
							{
								myObjectBuilder_CompoundCubeBlock2.Blocks[i] = myObjectBuilder_CubeBlock;
							}
						}
						myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CompoundCubeBlock2);
					}
				}
				else
				{
					MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock2 = ConvertDynamicGridBlockToStatic(ref worldMatrix, cubeBlock);
					if (myObjectBuilder_CubeBlock2 != null)
					{
						myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CubeBlock2);
					}
				}
			}
			return myObjectBuilder_CubeGrid;
		}

		public static MyObjectBuilder_CubeBlock ConvertDynamicGridBlockToStatic(ref MatrixD worldMatrix, MyObjectBuilder_CubeBlock origBlock)
		{
			MyDefinitionId myDefinitionId = new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(myDefinitionId, out MyCubeBlockDefinition blockDefinition);
			if (blockDefinition == null)
			{
				return null;
			}
			MyObjectBuilder_CubeBlock obj = MyObjectBuilderSerializer.CreateNewObject(myDefinitionId) as MyObjectBuilder_CubeBlock;
			obj.EntityId = origBlock.EntityId;
			((MyBlockOrientation)origBlock.BlockOrientation).GetQuaternion(out Quaternion result);
			Matrix matrix = Matrix.CreateFromQuaternion(result);
			_ = (Matrix)(matrix * worldMatrix);
			obj.Orientation = Quaternion.CreateFromRotationMatrix(matrix);
			Vector3I b = Vector3I.Abs(Vector3I.Round(Vector3.TransformNormal((Vector3)blockDefinition.Size, matrix)));
			Vector3I vector3I = origBlock.Min;
			Vector3I vector3I2 = origBlock.Min + b - Vector3I.One;
			Vector3I.Round(Vector3.TransformNormal(vector3I, worldMatrix));
			Vector3I.Round(Vector3.TransformNormal(vector3I2, worldMatrix));
			obj.Min = Vector3I.Min(vector3I, vector3I2);
			obj.MultiBlockId = origBlock.MultiBlockId;
			obj.MultiBlockDefinition = origBlock.MultiBlockDefinition;
			obj.MultiBlockIndex = origBlock.MultiBlockIndex;
			obj.BuildPercent = origBlock.BuildPercent;
			obj.IntegrityPercent = origBlock.BuildPercent;
			return obj;
		}

		public static void GetAllBlocksPositions(HashSet<Tuple<MySlimBlock, ushort?>> blockInCompoundIDs, HashSet<Vector3I> outPositions)
		{
			foreach (Tuple<MySlimBlock, ushort?> blockInCompoundID in blockInCompoundIDs)
			{
				Vector3I next = blockInCompoundID.Item1.Min;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref blockInCompoundID.Item1.Min, ref blockInCompoundID.Item1.Max);
				while (vector3I_RangeIterator.IsValid())
				{
					outPositions.Add(next);
					vector3I_RangeIterator.GetNext(out next);
				}
			}
		}

		[Event(null, 5142)]
		[Reliable]
		[Server]
		private static void RequestGridSpawn(Author author, DefinitionIdBlit definition, BuildData position, bool instantBuild, bool forceStatic, MyCubeGrid.MyBlockVisuals visuals)
		{
			MyEntity builder = null;
			bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeToolsEnabled(Sync.MyId);
			if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(definition, MyEventContext.Current.Sender.Value) || (MySession.Static.ResearchEnabled && !flag && !MySessionComponentResearch.Static.CanUse(author.IdentityId, definition)) || (instantBuild && !flag))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase)?.ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyEntities.TryGetEntityById(author.EntityId, out builder);
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(definition);
			Vector3D position2 = position.Position;
			if (!position.AbsolutePosition)
			{
				position2 += builder.PositionComp.GetPosition();
			}
			MatrixD m = MatrixD.CreateWorld(position2, position.Forward, position.Up);
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return;
			}
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize);
			BoundingBoxD localAabb = new BoundingBoxD(-cubeBlockDefinition.Size * cubeSize * 0.5f, cubeBlockDefinition.Size * cubeSize * 0.5f);
			if (!MySessionComponentSafeZones.IsActionAllowed(localAabb.TransformFast(ref m), MySafeZoneAction.Building, builder.EntityId, MyEventContext.Current.Sender.Value))
			{
				return;
			}
			MyGridPlacementSettings gridPlacementSettings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(cubeBlockDefinition.CubeSize);
			VoxelPlacementSettings voxelPlacementSettings = default(VoxelPlacementSettings);
			voxelPlacementSettings.PlacementMode = VoxelPlacementMode.OutsideVoxel;
			VoxelPlacementSettings value = voxelPlacementSettings;
			gridPlacementSettings.VoxelPlacement = value;
			bool flag2 = forceStatic || MyCubeGrid.IsAabbInsideVoxel(m, localAabb, gridPlacementSettings) || Static.m_stationPlacement;
			BuildComponent.GetGridSpawnMaterials(cubeBlockDefinition, m, flag2);
			bool flag3 = (flag && instantBuild) || BuildComponent.HasBuildingMaterials(builder);
			ulong senderId = MyEventContext.Current.Sender.Value;
			if (!flag3)
			{
				SpawnGridReply(flag3, senderId);
				return;
			}
			SpawnFlags spawnFlags = SpawnFlags.Default;
			if (flag && instantBuild)
			{
				spawnFlags |= SpawnFlags.SpawnAsMaster;
			}
			Vector3 color = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
			MyStringHash skinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, senderId) ?? MyStringHash.NullOrEmpty;
			if (flag2)
			{
				SpawnStaticGrid(cubeBlockDefinition, builder, m, color, skinId, spawnFlags, author.IdentityId, delegate(MyEntity grid)
				{
					AfterGridBuild(builder, grid as MyCubeGrid, instantBuild, senderId);
				});
			}
			else
			{
				SpawnDynamicGrid(cubeBlockDefinition, builder, m, color, skinId, 0L, spawnFlags, author.IdentityId, delegate(MyEntity grid)
				{
					AfterGridBuild(builder, grid as MyCubeGrid, instantBuild, senderId);
				});
			}
		}

		private static void SpawnGridReply(bool canSpawn, ulong senderId)
		{
			if (senderId == 0L)
			{
				SpawnGridReply(canSpawn);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SpawnGridReply, canSpawn, new EndpointId(senderId));
			}
		}

		[Event(null, 5232)]
		[Reliable]
		[Client]
		private static void SpawnGridReply(bool success)
		{
			if (success)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
			}
			else
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		bool IMyCubeBuilder.AddConstruction(IMyEntity buildingEntity)
		{
			return false;
		}

		IMyCubeGrid IMyCubeBuilder.FindClosestGrid()
		{
			return FindClosestGrid();
		}

		void IMyCubeBuilder.Activate(MyDefinitionId? blockDefinitionId = null)
		{
			Activate(blockDefinitionId);
		}

		void IMyCubeBuilder.Deactivate()
		{
			Deactivate();
		}

		void IMyCubeBuilder.DeactivateBlockCreation()
		{
			DeactivateBlockCreation();
		}

		void IMyCubeBuilder.StartNewGridPlacement(MyCubeSize cubeSize, bool isStatic)
		{
			StartStaticGridPlacement(cubeSize, isStatic);
		}
	}
}
