using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Game.Utils;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	public abstract class MyProjectorBase : MyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.IMyProjector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProjector, Sandbox.ModAPI.IMyTextSurfaceProvider
	{
		private class MyProjectorUpdateWork : IWork
		{
			private static readonly MyDynamicObjectPool<MyProjectorUpdateWork> InstancePool = new MyDynamicObjectPool<MyProjectorUpdateWork>(8);

			private MyProjectorBase m_projector;

			private MyCubeGrid m_grid;

			private HashSet<MySlimBlock> m_visibleBlocks = new HashSet<MySlimBlock>();

			private HashSet<MySlimBlock> m_buildableBlocks = new HashSet<MySlimBlock>();

			private HashSet<MySlimBlock> m_hiddenBlocks = new HashSet<MySlimBlock>();

			private int m_remainingBlocks;

			private int m_buildableBlocksCount;

			public WorkOptions Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Block, "Projector");

			public static Task Start(MyProjectorBase projector)
			{
				MyProjectorUpdateWork myProjectorUpdateWork = InstancePool.Allocate();
				myProjectorUpdateWork.m_projector = projector;
				myProjectorUpdateWork.m_grid = projector.ProjectedGrid;
				return Parallel.Start(myProjectorUpdateWork, myProjectorUpdateWork.OnComplete);
			}

			private void OnComplete()
			{
				if (!m_projector.Closed && !m_projector.CubeGrid.Closed && m_projector.ProjectedGrid != null)
				{
					if (!m_projector.AllowWelding)
					{
						foreach (MyCubeGrid previewGrid in m_projector.m_clipboard.PreviewGrids)
						{
							foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
							{
								if (m_projector.Enabled)
								{
									m_projector.ShowCube(cubeBlock, canBuild: false);
								}
								else
								{
									m_projector.HideCube(cubeBlock);
								}
							}
						}
					}
					else
					{
						foreach (MySlimBlock visibleBlock in m_visibleBlocks)
						{
							if (!m_projector.m_visibleBlocks.Contains(visibleBlock))
							{
								if (m_projector.Enabled)
								{
									m_projector.ShowCube(visibleBlock, canBuild: false);
								}
								else
								{
									m_projector.HideCube(visibleBlock);
								}
							}
						}
					}
					MyUtils.Swap(ref m_visibleBlocks, ref m_projector.m_visibleBlocks);
					if (m_projector.BlockDefinition.AllowWelding)
					{
						foreach (MySlimBlock buildableBlock in m_buildableBlocks)
						{
							if (!m_projector.m_buildableBlocks.Contains(buildableBlock))
							{
								if (m_projector.Enabled)
								{
									m_projector.ShowCube(buildableBlock, canBuild: true);
								}
								else
								{
									m_projector.HideCube(buildableBlock);
								}
							}
						}
					}
					MyUtils.Swap(ref m_buildableBlocks, ref m_projector.m_buildableBlocks);
					foreach (MySlimBlock hiddenBlock in m_hiddenBlocks)
					{
						if (!m_projector.m_hiddenBlocks.Contains(hiddenBlock))
						{
							m_projector.HideCube(hiddenBlock);
						}
					}
					MyUtils.Swap(ref m_hiddenBlocks, ref m_projector.m_hiddenBlocks);
					m_projector.m_remainingBlocks = m_remainingBlocks;
					m_projector.m_buildableBlocksCount = m_buildableBlocksCount;
					if (m_projector.m_remainingBlocks == 0 && !m_projector.m_keepProjection)
					{
						m_projector.RemoveProjection(m_projector.m_keepProjection);
					}
					else
					{
						m_projector.UpdateSounds();
						m_projector.SetEmissiveStateWorking();
					}
					m_projector.m_statsDirty = true;
					if (m_projector.m_shouldUpdateTexts)
					{
						m_projector.UpdateText();
						m_projector.m_shouldUpdateTexts = false;
					}
					m_projector.m_clipboard.HasPreviewBBox = false;
					m_projector = null;
					m_visibleBlocks.Clear();
					m_buildableBlocks.Clear();
					m_hiddenBlocks.Clear();
					InstancePool.Deallocate(this);
				}
			}

			public void DoWork(WorkData workData = null)
			{
				m_remainingBlocks = m_grid.BlocksCount;
				m_buildableBlocksCount = 0;
				foreach (MySlimBlock cubeBlock2 in m_grid.CubeBlocks)
				{
					Vector3D coords = m_grid.GridIntegerToWorld(cubeBlock2.Position);
					Vector3I pos = m_projector.CubeGrid.WorldToGridInteger(coords);
					MySlimBlock cubeBlock = m_projector.CubeGrid.GetCubeBlock(pos);
					if (cubeBlock != null && cubeBlock2.BlockDefinition.Id == cubeBlock.BlockDefinition.Id)
					{
						m_hiddenBlocks.Add(cubeBlock2);
						m_remainingBlocks--;
					}
					else if (m_projector.CanBuild(cubeBlock2))
					{
						m_buildableBlocks.Add(cubeBlock2);
						m_buildableBlocksCount++;
					}
					else if (m_projector.AllowWelding && m_projector.m_showOnlyBuildable)
					{
						m_hiddenBlocks.Add(cubeBlock2);
					}
					else
					{
						m_visibleBlocks.Add(cubeBlock2);
					}
				}
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyProjectorBase, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyProjectorBase, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyProjectorBase, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class OnSpawnProjection_003C_003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSpawnProjection();
			}
		}

		protected sealed class OnConfirmSpawnProjection_003C_003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConfirmSpawnProjection();
			}
		}

		protected sealed class BuildInternal_003C_003EVRageMath_Vector3I_0023System_Int64_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyProjectorBase, Vector3I, long, long, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in Vector3I cubeBlockPosition, in long owner, in long builder, in bool requestInstant, in long builtBy, in DBNull arg6)
			{
				@this.BuildInternal(cubeBlockPosition, owner, builder, requestInstant, builtBy);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyProjectorBase, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyProjectorBase, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyProjectorBase, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyProjectorBase, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnNewBlueprintSuccess_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_MyObjectBuilder_CubeGrid_003E : ICallSite<MyProjectorBase, List<MyObjectBuilder_CubeGrid>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in List<MyObjectBuilder_CubeGrid> projectedGrids, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnNewBlueprintSuccess(projectedGrids);
			}
		}

		protected sealed class ShowScriptRemoveMessage_003C_003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShowScriptRemoveMessage();
			}
		}

		protected sealed class OnOffsetChangedSuccess_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023System_Single_0023System_Boolean : ICallSite<MyProjectorBase, Vector3I, Vector3I, float, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in Vector3I positionOffset, in Vector3I rotationOffset, in float scale, in bool showOnlyBuildable, in DBNull arg5, in DBNull arg6)
			{
				@this.OnOffsetChangedSuccess(positionOffset, rotationOffset, scale, showOnlyBuildable);
			}
		}

		protected sealed class OnRemoveProjectionRequest_003C_003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProjectorBase @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveProjectionRequest();
			}
		}

		protected class m_keepProjection_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType keepProjection;
				ISyncType result = keepProjection = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyProjectorBase)P_0).m_keepProjection = (Sync<bool, SyncDirection.BothWays>)keepProjection;
				return result;
			}
		}

		protected class m_instantBuildingEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType instantBuildingEnabled;
				ISyncType result = instantBuildingEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyProjectorBase)P_0).m_instantBuildingEnabled = (Sync<bool, SyncDirection.BothWays>)instantBuildingEnabled;
				return result;
			}
		}

		protected class m_maxNumberOfProjections_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxNumberOfProjections;
				ISyncType result = maxNumberOfProjections = new Sync<int, SyncDirection.BothWays>(P_1, P_2);
				((MyProjectorBase)P_0).m_maxNumberOfProjections = (Sync<int, SyncDirection.BothWays>)maxNumberOfProjections;
				return result;
			}
		}

		protected class m_maxNumberOfBlocksPerProjection_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType maxNumberOfBlocksPerProjection;
				ISyncType result = maxNumberOfBlocksPerProjection = new Sync<int, SyncDirection.BothWays>(P_1, P_2);
				((MyProjectorBase)P_0).m_maxNumberOfBlocksPerProjection = (Sync<int, SyncDirection.BothWays>)maxNumberOfBlocksPerProjection;
				return result;
			}
		}

		protected class m_getOwnershipFromProjector_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType getOwnershipFromProjector;
				ISyncType result = getOwnershipFromProjector = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyProjectorBase)P_0).m_getOwnershipFromProjector = (Sync<bool, SyncDirection.BothWays>)getOwnershipFromProjector;
				return result;
			}
		}

		private const int PROJECTION_UPDATE_TIME = 2000;

		protected const int OFFSET_LIMIT = 50;

		protected const int ROTATION_LIMIT = 2;

		protected const float SCALE_LIMIT = 0.02f;

		protected const int MAX_SCALED_DRAW_DISTANCE = 50;

		protected const int MAX_SCALED_DRAW_DISTANCE_SQUARED = 2500;

		private int m_lastUpdate;

		private readonly MyProjectorClipboard m_clipboard;

		private readonly MyProjectorClipboard m_spawnClipboard;

		protected Vector3I m_projectionOffset;

		protected Vector3I m_projectionRotation;

		protected float m_projectionScale = 1f;

		private MySlimBlock m_hiddenBlock;

		private bool m_shouldUpdateProjection;

		private bool m_forceUpdateProjection;

		private bool m_shouldUpdateTexts;

		private bool m_shouldResetBuildable;

		private List<MyObjectBuilder_CubeGrid> m_savedProjections;

		protected bool m_showOnlyBuildable;

		private int m_frameCount;

		private bool m_removeRequested;

		private Task m_updateTask;

		private List<MyObjectBuilder_CubeGrid> m_originalGridBuilders;

		protected const int MAX_NUMBER_OF_PROJECTIONS = 1000;

		protected const int MAX_NUMBER_OF_BLOCKS = 10000;

		private int m_projectionsRemaining;

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBox;

		private readonly Sync<bool, SyncDirection.BothWays> m_keepProjection;

		private readonly Sync<bool, SyncDirection.BothWays> m_instantBuildingEnabled;

		private readonly Sync<int, SyncDirection.BothWays> m_maxNumberOfProjections;

		private readonly Sync<int, SyncDirection.BothWays> m_maxNumberOfBlocksPerProjection;

		private readonly Sync<bool, SyncDirection.BothWays> m_getOwnershipFromProjector;

		private bool m_isTextPanelOpen;

		private HashSet<MySlimBlock> m_visibleBlocks = new HashSet<MySlimBlock>();

		private HashSet<MySlimBlock> m_buildableBlocks = new HashSet<MySlimBlock>();

		private HashSet<MySlimBlock> m_hiddenBlocks = new HashSet<MySlimBlock>();

		private int m_remainingBlocks;

		private int m_totalBlocks;

		private readonly Dictionary<MyCubeBlockDefinition, int> m_remainingBlocksPerType = new Dictionary<MyCubeBlockDefinition, int>();

		private int m_remainingArmorBlocks;

		private int m_buildableBlocksCount;

		private bool m_statsDirty;

		public new MyProjectorDefinition BlockDefinition => (MyProjectorDefinition)base.BlockDefinition;

		public MyProjectorClipboard Clipboard => m_clipboard;

		internal new MyRenderComponentScreenAreas Render
		{
			get
			{
				return base.Render as MyRenderComponentScreenAreas;
			}
			set
			{
				base.Render = value;
			}
		}

		public Vector3I ProjectionOffset => m_projectionOffset;

		public Vector3I ProjectionRotation => m_projectionRotation;

		public Quaternion ProjectionRotationQuaternion
		{
			get
			{
				Vector3 vector = ProjectionRotation * MathHelper.ToRadians(BlockDefinition.RotationAngleStepDeg);
				return Quaternion.CreateFromYawPitchRoll(vector.X, vector.Y, vector.Z);
			}
		}

		public MyCubeGrid ProjectedGrid
		{
			get
			{
				if (m_clipboard.PreviewGrids.Count != 0)
				{
					return m_clipboard.PreviewGrids[0];
				}
				return null;
			}
		}

		protected bool InstantBuildingEnabled
		{
			get
			{
				return m_instantBuildingEnabled;
			}
			set
			{
				m_instantBuildingEnabled.Value = value;
			}
		}

		protected int MaxNumberOfProjections
		{
			get
			{
				return m_maxNumberOfProjections;
			}
			set
			{
				m_maxNumberOfProjections.Value = value;
			}
		}

		protected int MaxNumberOfBlocksPerProjection
		{
			get
			{
				return m_maxNumberOfBlocksPerProjection;
			}
			set
			{
				m_maxNumberOfBlocksPerProjection.Value = value;
			}
		}

		protected bool GetOwnershipFromProjector
		{
			get
			{
				return m_getOwnershipFromProjector;
			}
			set
			{
				m_getOwnershipFromProjector.Value = value;
			}
		}

		protected bool KeepProjection
		{
			get
			{
				return m_keepProjection;
			}
			set
			{
				m_keepProjection.Value = value;
			}
		}

		public bool IsActivating
		{
			get;
			private set;
		}

		public float Scale
		{
			get
			{
				if (!BlockDefinition.AllowScaling)
				{
					return 1f;
				}
				return m_projectionScale;
			}
		}

		public bool AllowScaling => BlockDefinition.AllowScaling;

		public bool AllowWelding
		{
			get
			{
				if (BlockDefinition.AllowWelding && !BlockDefinition.AllowScaling)
				{
					return !BlockDefinition.IgnoreSize;
				}
				return false;
			}
		}

		MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => m_multiPanel;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_multiPanel == null)
				{
					return null;
				}
				return m_multiPanel.PanelComponent;
			}
		}

		public bool IsTextPanelOpen
		{
			get
			{
				return m_isTextPanelOpen;
			}
			set
			{
				if (m_isTextPanelOpen != value)
				{
					m_isTextPanelOpen = value;
					RaisePropertiesChanged();
				}
			}
		}

		VRage.Game.ModAPI.IMyCubeGrid Sandbox.ModAPI.IMyProjector.ProjectedGrid => ProjectedGrid;

		Vector3I Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffset
		{
			get
			{
				return m_projectionOffset;
			}
			set
			{
				m_projectionOffset = value;
			}
		}

		Vector3I Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotation
		{
			get
			{
				return m_projectionRotation;
			}
			set
			{
				m_projectionRotation = value;
			}
		}

		[Obsolete("Use ProjectionOffset vector instead.")]
		int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffsetX => m_projectionOffset.X;

		[Obsolete("Use ProjectionOffset vector instead.")]
		int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffsetY => m_projectionOffset.Y;

		[Obsolete("Use ProjectionOffset vector instead.")]
		int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffsetZ => m_projectionOffset.Z;

		[Obsolete("Use ProjectionRotation vector instead.")]
		int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotX => m_projectionRotation.X * 90;

		[Obsolete("Use ProjectionRotation vector instead.")]
		int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotY => m_projectionRotation.Y * 90;

		[Obsolete("Use ProjectionRotation vector instead.")]
		int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotZ => m_projectionRotation.Z * 90;

		bool Sandbox.ModAPI.Ingame.IMyProjector.IsProjecting => IsProjecting();

		int Sandbox.ModAPI.Ingame.IMyProjector.RemainingBlocks => m_remainingBlocks;

		int Sandbox.ModAPI.Ingame.IMyProjector.TotalBlocks => m_totalBlocks;

		int Sandbox.ModAPI.Ingame.IMyProjector.RemainingArmorBlocks => m_remainingArmorBlocks;

		int Sandbox.ModAPI.Ingame.IMyProjector.BuildableBlocksCount => m_buildableBlocksCount;

		Dictionary<MyDefinitionBase, int> Sandbox.ModAPI.Ingame.IMyProjector.RemainingBlocksPerType
		{
			get
			{
				Dictionary<MyDefinitionBase, int> dictionary = new Dictionary<MyDefinitionBase, int>();
				foreach (KeyValuePair<MyCubeBlockDefinition, int> item in m_remainingBlocksPerType)
				{
					dictionary.Add(item.Key, item.Value);
				}
				return dictionary;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyProjector.ShowOnlyBuildable
		{
			get
			{
				return m_showOnlyBuildable;
			}
			set
			{
				m_showOnlyBuildable = value;
			}
		}

		int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount
		{
			get
			{
				if (m_multiPanel == null)
				{
					return 0;
				}
				return m_multiPanel.SurfaceCount;
			}
		}

		public MyProjectorBase()
		{
			m_clipboard = new MyProjectorClipboard(this, MyClipboardComponent.ClipboardDefinition.PastingSettings);
			m_spawnClipboard = new MyProjectorClipboard(this, MyClipboardComponent.ClipboardDefinition.PastingSettings);
			m_instantBuildingEnabled.ValueChanged += m_instantBuildingEnabled_ValueChanged;
			m_maxNumberOfProjections.ValueChanged += m_maxNumberOfProjections_ValueChanged;
			m_maxNumberOfBlocksPerProjection.ValueChanged += m_maxNumberOfBlocksPerProjection_ValueChanged;
			m_getOwnershipFromProjector.ValueChanged += m_getOwnershipFromProjector_ValueChanged;
			Render = new MyRenderComponentScreenAreas(this);
		}

		protected bool IsProjecting()
		{
			return m_clipboard.IsActive;
		}

		protected bool CanProject()
		{
			UpdateIsWorking();
			UpdateText();
			return base.IsWorking;
		}

		protected void OnOffsetsChanged()
		{
			m_shouldUpdateProjection = true;
			m_shouldUpdateTexts = true;
			SendNewOffset(m_projectionOffset, m_projectionRotation, m_projectionScale, m_showOnlyBuildable);
			if (AllowWelding)
			{
				Remap();
			}
		}

		public void SelectBlueprint()
		{
			MyEntity interactedEntity = null;
			if (MyGuiScreenTerminal.IsOpen)
			{
				interactedEntity = MyGuiScreenTerminal.InteractedEntity;
				MyGuiScreenTerminal.Hide();
			}
			SendRemoveProjection();
			if (MyFakes.I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN)
			{
				MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(m_clipboard, allowCopyToClipboard: true, MyBlueprintAccessType.PROJECTOR);
				myGuiBlueprintScreen_Reworked.Closed += delegate(MyGuiScreenBase screen)
				{
					OnBlueprintScreen_Closed(screen, interactedEntity);
				};
				MyGuiSandbox.AddScreen(myGuiBlueprintScreen_Reworked);
			}
			else
			{
				MyGuiBlueprintScreen myGuiBlueprintScreen = new MyGuiBlueprintScreen(m_clipboard, allowCopyToClipboard: true, MyBlueprintAccessType.PROJECTOR);
				myGuiBlueprintScreen.Closed += delegate(MyGuiScreenBase screen)
				{
					OnBlueprintScreen_Closed(screen, interactedEntity);
				};
				MyGuiSandbox.AddScreen(myGuiBlueprintScreen);
			}
		}

		public bool SelectPrefab(string prefabName)
		{
			MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(prefabName);
			if (gridPrefab == null || gridPrefab.Length == 0)
			{
				return false;
			}
			m_clipboard.Deactivate();
			m_clipboard.SetGridFromBuilders(gridPrefab, Vector3.Zero, 0f);
			InitFromObjectBuilder(new List<MyObjectBuilder_CubeGrid>(gridPrefab));
			return true;
		}

		public Vector3 GetProjectionTranslationOffset()
		{
			return m_projectionOffset * m_clipboard.GridSize * Scale;
		}

		private void RequestRemoveProjection()
		{
			m_removeRequested = true;
			m_frameCount = 0;
		}

		private void RemoveProjection(bool keepProjection)
		{
			m_hiddenBlock = null;
			if (ProjectedGrid != null)
			{
				int num = 0;
				foreach (MyCubeGrid previewGrid in m_clipboard.PreviewGrids)
				{
					num += previewGrid.CubeBlocks.Count;
				}
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(base.BuiltBy);
				if (myIdentity != null)
				{
					int num2 = num;
					myIdentity.BlockLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, num2, base.CubeGrid, modifyBlockCount: false);
					base.CubeGrid.BlocksPCU -= num2;
				}
			}
			m_clipboard.Deactivate();
			if (!keepProjection)
			{
				m_clipboard.Clear();
				m_originalGridBuilders = null;
			}
			UpdateSounds();
			SetEmissiveStateWorking();
			m_statsDirty = true;
			UpdateText();
			RaisePropertiesChanged();
		}

		private void ResetRotation()
		{
			SetRotation(m_clipboard, -m_projectionRotation);
		}

		private void SetRotation(MyGridClipboard clipboard, Vector3I rotation)
		{
			clipboard.RotateAroundAxis(0, Math.Sign(rotation.X), newlyPressed: true, Math.Abs((float)rotation.X * (MathF.E * 449f / 777f)));
			clipboard.RotateAroundAxis(1, Math.Sign(rotation.Y), newlyPressed: true, Math.Abs((float)rotation.Y * (MathF.E * 449f / 777f)));
			clipboard.RotateAroundAxis(2, Math.Sign(rotation.Z), newlyPressed: true, Math.Abs((float)rotation.Z * (MathF.E * 449f / 777f)));
		}

		private void OnBlueprintScreen_Closed(MyGuiScreenBase source, MyEntity interactedEntity = null)
		{
			List<MyObjectBuilder_CubeGrid> copiedGrids = m_clipboard.CopiedGrids;
			InitFromObjectBuilder(copiedGrids, interactedEntity);
			ReopenTerminal(interactedEntity);
		}

		private void InitFromObjectBuilder(List<MyObjectBuilder_CubeGrid> gridsObs, MyEntity interactedEntity = null)
		{
			base.ResourceSink.Update();
			UpdateIsWorking();
			if (gridsObs.Count == 0 || !base.IsWorking)
			{
				RemoveProjection(keepProjection: false);
				if (interactedEntity != null)
				{
					ReopenTerminal(interactedEntity);
				}
				return;
			}
			if (!BlockDefinition.IgnoreSize && m_clipboard.GridSize != base.CubeGrid.GridSize)
			{
				RemoveProjection(keepProjection: false);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MySpaceTexts.NotificationProjectorGridSize), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
				{
					if (interactedEntity != null)
					{
						ReopenTerminal(interactedEntity);
					}
				}));
				return;
			}
			if (gridsObs.Count > 1 && AllowWelding)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MySpaceTexts.NotificationProjectorMultipleGrids)));
			}
			int largestGridIndex = -1;
			int num = -1;
			if (AllowWelding)
			{
				for (int i = 0; i < gridsObs.Count; i++)
				{
					int count = gridsObs[i].CubeBlocks.Count;
					if (count > num)
					{
						num = count;
						largestGridIndex = i;
					}
				}
			}
			List<MyObjectBuilder_CubeGrid> gridBuilders = new List<MyObjectBuilder_CubeGrid>();
			m_originalGridBuilders = null;
			Parallel.Start(delegate
			{
				if (largestGridIndex != -1)
				{
					gridBuilders.Add((MyObjectBuilder_CubeGrid)gridsObs[largestGridIndex].Clone());
				}
				else
				{
					foreach (MyObjectBuilder_CubeGrid gridsOb in gridsObs)
					{
						gridBuilders.Add((MyObjectBuilder_CubeGrid)gridsOb.Clone());
					}
				}
				foreach (MyObjectBuilder_CubeGrid gridsOb2 in gridsObs)
				{
					m_clipboard.ProcessCubeGrid(gridsOb2);
				}
				foreach (MyObjectBuilder_CubeGrid item in gridBuilders)
				{
					MyEntities.RemapObjectBuilder(item);
				}
			}, delegate
			{
				if (gridBuilders.Count > 0 && m_originalGridBuilders == null)
				{
					m_originalGridBuilders = gridBuilders;
					SendNewBlueprint(m_originalGridBuilders);
				}
			});
		}

		private void ReopenTerminal(MyEntity interactedEntity = null)
		{
			if (!MyGuiScreenTerminal.IsOpen)
			{
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalCharacter, interactedEntity ?? this);
			}
		}

		protected bool ScenarioSettingsEnabled()
		{
			if (!MySession.Static.Settings.ScenarioEditMode)
			{
				return MySession.Static.IsScenario;
			}
			return true;
		}

		protected bool CanEditInstantBuildingSettings()
		{
			if (CanEnableInstantBuilding())
			{
				return m_instantBuildingEnabled;
			}
			return false;
		}

		protected bool CanEnableInstantBuilding()
		{
			return MySession.Static.Settings.ScenarioEditMode;
		}

		protected bool CanSpawnProjection()
		{
			if (!m_instantBuildingEnabled)
			{
				return false;
			}
			if (ProjectedGrid == null)
			{
				return false;
			}
			int num = 0;
			foreach (MyCubeGrid previewGrid in m_clipboard.PreviewGrids)
			{
				num += previewGrid.CubeBlocks.Count;
			}
			if ((int)m_maxNumberOfBlocksPerProjection < 10000 && (int)m_maxNumberOfBlocksPerProjection < num)
			{
				return false;
			}
			if (m_projectionsRemaining == 0)
			{
				return false;
			}
			if (!ScenarioSettingsEnabled())
			{
				return false;
			}
			return true;
		}

		protected void TrySetInstantBuilding(bool v)
		{
			if (CanEnableInstantBuilding())
			{
				InstantBuildingEnabled = v;
			}
		}

		protected void TrySetGetOwnership(bool v)
		{
			if (CanEnableInstantBuilding())
			{
				GetOwnershipFromProjector = v;
			}
		}

		protected void TrySpawnProjection()
		{
			if (CanSpawnProjection())
			{
				SendSpawnProjection();
			}
		}

		protected void TryChangeMaxNumberOfBlocksPerProjection(float v)
		{
			if (CanEditInstantBuildingSettings())
			{
				MaxNumberOfBlocksPerProjection = (int)Math.Round(v);
			}
		}

		protected void TryChangeNumberOfProjections(float v)
		{
			if (CanEditInstantBuildingSettings())
			{
				MaxNumberOfProjections = (int)Math.Round(v);
			}
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			RaisePropertiesChanged();
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			CreateTextBox(isEditable, new StringBuilder(PanelComponent.Text.ToString()), isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBox);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 546)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		[Event(null, 557)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBox = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBox != null)
			{
				if (m_textBox.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(isPublic: true);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBox.Description.Text.Remove(100000, m_textBox.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBox.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBox);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					SendChangeDescriptionMessage(m_textBox.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text.Clear().Append((object)description);
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 648)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			PanelComponent.Text.Clear().Append((object)stringBuilder);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void ShowCube(MySlimBlock cubeBlock, bool canBuild)
		{
			if (canBuild)
			{
				SetTransparency(cubeBlock, 0.25f);
			}
			else
			{
				SetTransparency(cubeBlock, MyGridConstants.PROJECTOR_TRANSPARENCY);
			}
		}

		public void HideCube(MySlimBlock cubeBlock)
		{
			SetTransparency(cubeBlock, 1f);
		}

		protected virtual void SetTransparency(MySlimBlock cubeBlock, float transparency)
		{
			transparency = 0f - transparency;
			if (cubeBlock.Dithering != transparency || cubeBlock.CubeGrid.Render.Transparency != transparency)
			{
				cubeBlock.CubeGrid.Render.Transparency = transparency;
				cubeBlock.CubeGrid.Render.CastShadows = false;
				cubeBlock.Dithering = transparency;
				cubeBlock.UpdateVisual();
				MyCubeBlock fatBlock = cubeBlock.FatBlock;
				if (fatBlock != null)
				{
					fatBlock.Render.CastShadows = false;
					SetTransparencyForSubparts(fatBlock, transparency);
				}
				if (fatBlock != null && fatBlock.UseObjectsComponent != null && fatBlock.UseObjectsComponent.DetectorPhysics != null)
				{
					fatBlock.UseObjectsComponent.DetectorPhysics.Enabled = false;
				}
			}
		}

		private void SetTransparencyForSubparts(MyEntity renderEntity, float transparency)
		{
			renderEntity.Render.CastShadows = false;
			if (renderEntity.Subparts != null)
			{
				foreach (KeyValuePair<string, MyEntitySubpart> subpart in renderEntity.Subparts)
				{
					subpart.Value.Render.Transparency = transparency;
					subpart.Value.Render.CastShadows = false;
					subpart.Value.Render.RemoveRenderObjects();
					subpart.Value.Render.AddRenderObjects();
					SetTransparencyForSubparts(subpart.Value, transparency);
				}
			}
		}

		private void HideIntersectedBlock()
		{
			if ((bool)m_instantBuildingEnabled)
			{
				return;
			}
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return;
			}
			Vector3D translation = localCharacter.GetHeadMatrix(includeY: true).Translation;
			if (ProjectedGrid == null)
			{
				return;
			}
			Vector3I pos = ProjectedGrid.WorldToGridInteger(translation);
			MySlimBlock cubeBlock = ProjectedGrid.GetCubeBlock(pos);
			if (cubeBlock != null)
			{
				if (Math.Abs(cubeBlock.Dithering) < 1f && m_hiddenBlock != cubeBlock)
				{
					if (m_hiddenBlock != null)
					{
						ShowCube(m_hiddenBlock, CanBuild(m_hiddenBlock));
					}
					HideCube(cubeBlock);
					m_hiddenBlock = cubeBlock;
				}
			}
			else if (m_hiddenBlock != null)
			{
				ShowCube(m_hiddenBlock, CanBuild(m_hiddenBlock));
				m_hiddenBlock = null;
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, CalculateRequiredPowerInput);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			if (MyFakes.ENABLE_PROJECTOR_BLOCK)
			{
				MyObjectBuilder_ProjectorBase myObjectBuilder_ProjectorBase = (MyObjectBuilder_ProjectorBase)objectBuilder;
				List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
				if ((myObjectBuilder_ProjectorBase.ProjectedGrids == null || myObjectBuilder_ProjectorBase.ProjectedGrids.Count == 0) && myObjectBuilder_ProjectorBase.ProjectedGrid != null)
				{
					list.Add(myObjectBuilder_ProjectorBase.ProjectedGrid);
				}
				else
				{
					list = myObjectBuilder_ProjectorBase.ProjectedGrids;
				}
				if (list != null && list.Count > 0)
				{
					m_projectionOffset = Vector3I.Clamp(myObjectBuilder_ProjectorBase.ProjectionOffset, new Vector3I(-50), new Vector3I(50));
					int num = (int)(360f / (float)BlockDefinition.RotationAngleStepDeg);
					m_projectionRotation = Vector3I.Clamp(myObjectBuilder_ProjectorBase.ProjectionRotation, new Vector3I(-num), new Vector3I(num));
					m_projectionScale = myObjectBuilder_ProjectorBase.Scale;
					m_savedProjections = list;
					m_keepProjection.SetLocalValue(myObjectBuilder_ProjectorBase.KeepProjection);
				}
				m_showOnlyBuildable = myObjectBuilder_ProjectorBase.ShowOnlyBuildable;
				m_instantBuildingEnabled.SetLocalValue(myObjectBuilder_ProjectorBase.InstantBuildingEnabled);
				m_maxNumberOfProjections.SetLocalValue(MathHelper.Clamp(myObjectBuilder_ProjectorBase.MaxNumberOfProjections, 0, 1000));
				m_maxNumberOfBlocksPerProjection.SetLocalValue(MathHelper.Clamp(myObjectBuilder_ProjectorBase.MaxNumberOfBlocks, 0, 10000));
				m_getOwnershipFromProjector.SetLocalValue(myObjectBuilder_ProjectorBase.GetOwnershipFromProjector);
				m_projectionsRemaining = MathHelper.Clamp(myObjectBuilder_ProjectorBase.ProjectionsRemaining, 0, m_maxNumberOfProjections);
				base.IsWorkingChanged += MyProjector_IsWorkingChanged;
				myResourceSinkComponent.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
				base.ResourceSink.Update();
				m_statsDirty = true;
				UpdateText();
				base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
				SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
				base.EnabledChanged += OnEnabledChanged;
				if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count > 0)
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
					m_multiPanel = new MyMultiTextPanelComponent(this, BlockDefinition.ScreenAreas, myObjectBuilder_ProjectorBase.TextPanels);
					m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
				}
			}
		}

		private void InitializeClipboard()
		{
			m_clipboard.ResetGridOrientation();
			if (m_clipboard.IsActive || IsActivating)
			{
				return;
			}
			int num = 0;
			foreach (MyObjectBuilder_CubeGrid copiedGrid in m_clipboard.CopiedGrids)
			{
				num += copiedGrid.CubeBlocks.Count;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(base.BuiltBy);
			if (myIdentity != null)
			{
				int num2 = num;
				if (!MySession.Static.CheckLimitsAndNotify(base.BuiltBy, BlockDefinition.BlockPairName, num2))
				{
					return;
				}
				myIdentity.BlockLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, num2, base.CubeGrid, modifyBlockCount: false);
				base.CubeGrid.BlocksPCU += num2;
			}
			IsActivating = true;
			m_clipboard.Activate(delegate
			{
				if (m_clipboard.PreviewGrids.Count != 0)
				{
					foreach (MyCubeGrid previewGrid in m_clipboard.PreviewGrids)
					{
						previewGrid.Projector = this;
					}
				}
				m_forceUpdateProjection = true;
				m_shouldUpdateTexts = true;
				m_shouldResetBuildable = true;
				m_clipboard.ActuallyTestPlacement();
				SetRotation(m_clipboard, m_projectionRotation);
				base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
				IsActivating = false;
			});
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ProjectorBase myObjectBuilder_ProjectorBase = (MyObjectBuilder_ProjectorBase)base.GetObjectBuilderCubeBlock(copy);
			if (m_clipboard != null && m_clipboard.CopiedGrids != null && m_clipboard.CopiedGrids.Count > 0 && m_originalGridBuilders != null)
			{
				if (copy)
				{
					myObjectBuilder_ProjectorBase.ProjectedGrids = new List<MyObjectBuilder_CubeGrid>();
					foreach (MyObjectBuilder_CubeGrid originalGridBuilder in m_originalGridBuilders)
					{
						MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)originalGridBuilder.Clone();
						MyEntities.RemapObjectBuilder(myObjectBuilder_CubeGrid);
						myObjectBuilder_ProjectorBase.ProjectedGrids.Add(myObjectBuilder_CubeGrid);
					}
				}
				else
				{
					myObjectBuilder_ProjectorBase.ProjectedGrids = m_originalGridBuilders;
				}
				myObjectBuilder_ProjectorBase.ProjectionOffset = m_projectionOffset;
				myObjectBuilder_ProjectorBase.ProjectionRotation = m_projectionRotation;
				myObjectBuilder_ProjectorBase.KeepProjection = m_keepProjection;
				myObjectBuilder_ProjectorBase.Scale = m_projectionScale;
			}
			else if ((myObjectBuilder_ProjectorBase.ProjectedGrids == null || myObjectBuilder_ProjectorBase.ProjectedGrids.Count == 0) && m_savedProjections != null && m_savedProjections.Count > 0 && base.CubeGrid.Projector == null)
			{
				myObjectBuilder_ProjectorBase.ProjectedGrids = m_savedProjections;
				myObjectBuilder_ProjectorBase.ProjectionOffset = m_projectionOffset;
				myObjectBuilder_ProjectorBase.ProjectionRotation = m_projectionRotation;
				myObjectBuilder_ProjectorBase.KeepProjection = m_keepProjection;
			}
			else
			{
				myObjectBuilder_ProjectorBase.ProjectedGrids = null;
			}
			myObjectBuilder_ProjectorBase.ShowOnlyBuildable = m_showOnlyBuildable;
			myObjectBuilder_ProjectorBase.InstantBuildingEnabled = m_instantBuildingEnabled;
			myObjectBuilder_ProjectorBase.MaxNumberOfProjections = m_maxNumberOfProjections;
			myObjectBuilder_ProjectorBase.MaxNumberOfBlocks = m_maxNumberOfBlocksPerProjection;
			myObjectBuilder_ProjectorBase.ProjectionsRemaining = m_projectionsRemaining;
			myObjectBuilder_ProjectorBase.GetOwnershipFromProjector = m_getOwnershipFromProjector;
			myObjectBuilder_ProjectorBase.TextPanels = ((m_multiPanel != null) ? m_multiPanel.Serialize() : null);
			return myObjectBuilder_ProjectorBase;
		}

		private void UpdateStats()
		{
			m_totalBlocks = ProjectedGrid.CubeBlocks.Count;
			m_remainingArmorBlocks = 0;
			m_remainingBlocksPerType.Clear();
			foreach (MySlimBlock cubeBlock2 in ProjectedGrid.CubeBlocks)
			{
				Vector3 v = ProjectedGrid.GridIntegerToWorld(cubeBlock2.Position);
				Vector3I pos = base.CubeGrid.WorldToGridInteger(v);
				MySlimBlock cubeBlock = base.CubeGrid.GetCubeBlock(pos);
				if (cubeBlock == null || cubeBlock2.BlockDefinition.Id != cubeBlock.BlockDefinition.Id)
				{
					if (cubeBlock2.FatBlock == null)
					{
						m_remainingArmorBlocks++;
					}
					else if (!m_remainingBlocksPerType.ContainsKey(cubeBlock2.BlockDefinition))
					{
						m_remainingBlocksPerType.Add(cubeBlock2.BlockDefinition, 1);
					}
					else
					{
						m_remainingBlocksPerType[cubeBlock2.BlockDefinition]++;
					}
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			base.ResourceSink.Update();
			if (m_removeRequested)
			{
				m_frameCount++;
				if (m_frameCount > 10)
				{
					UpdateIsWorking();
					if (!base.IsWorking && IsProjecting())
					{
						RemoveProjection(keepProjection: true);
					}
					m_frameCount = 0;
					m_removeRequested = false;
				}
			}
			if (m_clipboard.IsActive)
			{
				m_clipboard.Update();
				if (m_shouldResetBuildable)
				{
					m_shouldResetBuildable = false;
					foreach (MySlimBlock cubeBlock in ProjectedGrid.CubeBlocks)
					{
						HideCube(cubeBlock);
					}
				}
				if (m_forceUpdateProjection || (m_shouldUpdateProjection && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdate > 2000))
				{
					UpdateProjection();
					m_shouldUpdateProjection = false;
					m_forceUpdateProjection = false;
					m_lastUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (m_clipboard.IsActive && (bool)m_instantBuildingEnabled)
			{
				m_clipboard.ActuallyTestPlacement();
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(CheckIsWorking());
			}
			if (AllowScaling && ProjectedGrid != null)
			{
				foreach (MyCubeGrid previewGrid in m_clipboard.PreviewGrids)
				{
					bool flag = IsInRange();
					if (previewGrid.InScene != flag)
					{
						if (flag)
						{
							MyEntities.Add(previewGrid);
						}
						else
						{
							MyEntities.Remove(previewGrid);
						}
					}
				}
			}
		}

		private void UpdateProjection()
		{
			if ((bool)m_instantBuildingEnabled)
			{
				if (ProjectedGrid != null)
				{
					foreach (MySlimBlock cubeBlock in ProjectedGrid.CubeBlocks)
					{
						ShowCube(cubeBlock, canBuild: true);
					}
					m_clipboard.HasPreviewBBox = true;
				}
			}
			else if (m_updateTask.IsComplete)
			{
				m_hiddenBlock = null;
				if (m_clipboard.PreviewGrids.Count != 0)
				{
					foreach (MyCubeGrid previewGrid in m_clipboard.PreviewGrids)
					{
						previewGrid.Render.Transparency = 0f;
					}
					m_updateTask = MyProjectorUpdateWork.Start(this);
				}
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			if (base.IsWorking)
			{
				if (IsProjecting())
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, Render.RenderObjectIDs[0]);
			}
			return false;
		}

		private void UpdateSounds()
		{
			UpdateIsWorking();
			if (!base.IsWorking)
			{
				return;
			}
			if (IsProjecting())
			{
				if (m_soundEmitter != null && m_soundEmitter.SoundId != BlockDefinition.PrimarySound.Arcade && m_soundEmitter.SoundId != BlockDefinition.PrimarySound.Realistic)
				{
					m_soundEmitter.StopSound(forced: false);
					m_soundEmitter.PlaySound(BlockDefinition.PrimarySound);
				}
			}
			else if (m_soundEmitter != null && m_soundEmitter.SoundId != BlockDefinition.IdleSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.IdleSound.Realistic)
			{
				m_soundEmitter.StopSound(forced: false);
				m_soundEmitter.PlaySound(BlockDefinition.IdleSound);
			}
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			if ((bool)m_instantBuildingEnabled)
			{
				UpdateBaseText(detailedInfo);
				if (m_clipboard.IsActive && ProjectedGrid != null)
				{
					if ((int)m_maxNumberOfBlocksPerProjection < 10000)
					{
						detailedInfo.Append("\n");
						detailedInfo.Append("Ship blocks: " + ProjectedGrid.BlocksCount + "/" + m_maxNumberOfBlocksPerProjection);
					}
					if ((int)m_maxNumberOfProjections < 1000)
					{
						detailedInfo.Append("\n");
						detailedInfo.Append("Projections remaining: " + m_projectionsRemaining + "/" + m_maxNumberOfProjections);
					}
				}
			}
			else
			{
				if (!m_statsDirty)
				{
					return;
				}
				if (m_clipboard.IsActive)
				{
					UpdateStats();
				}
				m_statsDirty = false;
				UpdateBaseText(detailedInfo);
				if (m_clipboard.IsActive && AllowWelding)
				{
					detailedInfo.Append("\n");
					if (m_buildableBlocksCount > 0)
					{
						detailedInfo.Append("\n");
					}
					else
					{
						detailedInfo.Append("WARNING! Projection out of bounds!\n");
					}
					detailedInfo.Append("Build progress: " + (m_totalBlocks - m_remainingBlocks) + "/" + m_totalBlocks);
					if (m_remainingArmorBlocks > 0 || m_remainingBlocksPerType.Count != 0)
					{
						detailedInfo.Append("\nBlocks remaining:\n");
						detailedInfo.Append("Armor blocks: " + m_remainingArmorBlocks);
						foreach (KeyValuePair<MyCubeBlockDefinition, int> item in m_remainingBlocksPerType)
						{
							detailedInfo.Append("\n");
							detailedInfo.Append(item.Key.DisplayNameText + ": " + item.Value);
						}
					}
					else
					{
						detailedInfo.Append("\nComplete!");
					}
				}
			}
		}

		private void UpdateText()
		{
			SetDetailedInfoDirty();
			if (!m_instantBuildingEnabled && m_statsDirty)
			{
				if (m_clipboard.IsActive)
				{
					UpdateStats();
				}
				RaisePropertiesChanged();
			}
		}

		private void UpdateBaseText(StringBuilder detailedInfo)
		{
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(BlockDefinition.RequiredPowerInput, detailedInfo);
		}

		private void ShowNotification(MyStringId textToDisplay)
		{
			MyHudNotification notification = new MyHudNotification(textToDisplay, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			MyHud.Notifications.Add(notification);
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
			if (m_clipboard.IsActive)
			{
				RemoveProjection(keepProjection: false);
			}
		}

		private void CubeGrid_OnGridSplit(MyCubeGrid grid1, MyCubeGrid grid2)
		{
			if (m_originalGridBuilders != null && Sync.IsServer && !base.MarkedForClose && !base.Closed && AllowWelding)
			{
				Remap();
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			if (m_originalGridBuilders != null && Sync.IsServer && AllowWelding)
			{
				Remap();
			}
		}

		private void Remap()
		{
			if (m_originalGridBuilders != null && m_originalGridBuilders.Count > 0 && Sync.IsServer)
			{
				foreach (MyObjectBuilder_CubeGrid originalGridBuilder in m_originalGridBuilders)
				{
					MyEntities.RemapObjectBuilder(originalGridBuilder);
				}
				SetNewBlueprint(m_originalGridBuilders);
			}
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && IsProjecting())
			{
				RequestRemoveProjection();
			}
			UpdateIsWorking();
			SetEmissiveStateWorking();
			UpdateScreen();
		}

		private float CalculateRequiredPowerInput()
		{
			return BlockDefinition.RequiredPowerInput;
		}

		private void MyProjector_IsWorkingChanged(MyCubeBlock obj)
		{
			if (!base.IsWorking && IsProjecting())
			{
				RequestRemoveProjection();
			}
			else
			{
				SetEmissiveStateWorking();
				if (base.IsWorking && !IsProjecting() && m_clipboard.HasCopiedGrids())
				{
					InitializeClipboard();
				}
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_multiPanel != null)
			{
				m_multiPanel.Reset();
			}
			if (base.ResourceSink != null)
			{
				UpdateScreen();
			}
			if (CheckIsWorking() && m_multiPanel != null)
			{
				Render.UpdateModelProperties();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (base.CubeGrid.Physics != null && m_savedProjections != null && m_savedProjections.Count > 0)
			{
				MyObjectBuilder_CubeGrid[] array = new MyObjectBuilder_CubeGrid[m_savedProjections.Count];
				for (int i = 0; i < m_savedProjections.Count; i++)
				{
					MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)m_savedProjections[i].Clone();
					MyEntities.RemapObjectBuilder(myObjectBuilder_CubeGrid);
					m_clipboard.ProcessCubeGrid(myObjectBuilder_CubeGrid);
					array[i] = myObjectBuilder_CubeGrid;
				}
				m_clipboard.SetGridFromBuilders(array, Vector3.Zero, 0f);
				m_originalGridBuilders = m_savedProjections;
				m_savedProjections = null;
				if (base.IsWorking)
				{
					InitializeClipboard();
				}
				RequestRemoveProjection();
			}
			UpdateSounds();
			SetEmissiveStateWorking();
			UpdateScreen();
		}

		private void previewGrid_OnBlockAdded(MySlimBlock obj)
		{
			m_shouldUpdateProjection = true;
			m_shouldUpdateTexts = true;
			if (m_originalGridBuilders != null && IsProjecting())
			{
				Vector3I a = ProjectedGrid.WorldToGridInteger(base.CubeGrid.GridIntegerToWorld(obj.Position));
				MyTerminalBlock myTerminalBlock = obj.FatBlock as MyTerminalBlock;
				if (myTerminalBlock != null)
				{
					foreach (MyObjectBuilder_CubeGrid originalGridBuilder in m_originalGridBuilders)
					{
						foreach (MyObjectBuilder_BlockGroup blockGroup in originalGridBuilder.BlockGroups)
						{
							foreach (Vector3I block in blockGroup.Blocks)
							{
								if (a == block)
								{
									MyBlockGroup myBlockGroup = new MyBlockGroup
									{
										Name = new StringBuilder(blockGroup.Name)
									};
									myBlockGroup.Blocks.Add(myTerminalBlock);
									base.CubeGrid.AddGroup(myBlockGroup);
								}
							}
						}
					}
					myTerminalBlock.CheckConnectionChanged += TerminalBlockOnCheckConnectionChanged;
				}
			}
		}

		private void TerminalBlockOnCheckConnectionChanged(MyCubeBlock myCubeBlock)
		{
			m_forceUpdateProjection = true;
			m_shouldUpdateTexts = true;
		}

		private void previewGrid_OnBlockRemoved(MySlimBlock obj)
		{
			m_shouldUpdateProjection = true;
			m_shouldUpdateTexts = true;
			if (obj != null && obj.FatBlock != null)
			{
				obj.FatBlock.CheckConnectionChanged -= TerminalBlockOnCheckConnectionChanged;
			}
		}

		[Event(null, 1418)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnSpawnProjection()
		{
			if (!CanSpawnProjection())
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] array = new MyObjectBuilder_CubeGrid[m_originalGridBuilders.Count];
			for (int i = 0; i < m_originalGridBuilders.Count; i++)
			{
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)m_originalGridBuilders[i].Clone();
				MyEntities.RemapObjectBuilder(myObjectBuilder_CubeGrid);
				if ((bool)m_getOwnershipFromProjector)
				{
					foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
					{
						cubeBlock.Owner = base.OwnerId;
						cubeBlock.ShareMode = base.IDModule.ShareMode;
					}
				}
				array[i] = myObjectBuilder_CubeGrid;
			}
			m_spawnClipboard.SetGridFromBuilders(array, Vector3.Zero, 0f);
			m_spawnClipboard.ResetGridOrientation();
			if (!m_spawnClipboard.IsActive)
			{
				m_spawnClipboard.Activate();
			}
			SetRotation(m_spawnClipboard, m_projectionRotation);
			m_spawnClipboard.Update();
			if (m_spawnClipboard.ActuallyTestPlacement() && m_spawnClipboard.PasteGrid())
			{
				OnConfirmSpawnProjection();
			}
			m_spawnClipboard.Deactivate();
			m_spawnClipboard.Clear();
		}

		[Event(null, 1460)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnConfirmSpawnProjection()
		{
			if ((int)m_maxNumberOfProjections < 1000)
			{
				m_projectionsRemaining--;
			}
			if (!m_keepProjection)
			{
				RemoveProjection(keepProjection: false);
			}
			UpdateText();
			RaisePropertiesChanged();
		}

		private void m_instantBuildingEnabled_ValueChanged(SyncBase obj)
		{
			m_shouldUpdateProjection = true;
			if ((bool)m_instantBuildingEnabled)
			{
				m_projectionsRemaining = m_maxNumberOfProjections;
			}
			RaisePropertiesChanged();
		}

		private void m_maxNumberOfProjections_ValueChanged(SyncBase obj)
		{
			m_projectionsRemaining = m_maxNumberOfProjections;
			RaisePropertiesChanged();
		}

		private void m_maxNumberOfBlocksPerProjection_ValueChanged(SyncBase obj)
		{
			RaisePropertiesChanged();
		}

		private void m_getOwnershipFromProjector_ValueChanged(SyncBase obj)
		{
			RaisePropertiesChanged();
		}

		private void OnEnabledChanged(MyTerminalBlock myTerminalBlock)
		{
			if (!base.Enabled)
			{
				RemoveProjection(keepProjection: true);
			}
			if (m_multiPanel != null && m_multiPanel.SurfaceCount > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null && m_multiPanel.SurfaceCount > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				m_multiPanel.AddToScene();
			}
			if (base.CubeGrid != null)
			{
				base.CubeGrid.OnBlockAdded += previewGrid_OnBlockAdded;
				base.CubeGrid.OnBlockRemoved += previewGrid_OnBlockRemoved;
				base.CubeGrid.OnGridSplit += CubeGrid_OnGridSplit;
				foreach (MyCubeBlock fatBlock in base.CubeGrid.GetFatBlocks())
				{
					if (fatBlock is Sandbox.ModAPI.IMyTerminalBlock)
					{
						fatBlock.CheckConnectionChanged += TerminalBlockOnCheckConnectionChanged;
					}
				}
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (base.CubeGrid != null)
			{
				base.CubeGrid.OnBlockAdded -= previewGrid_OnBlockAdded;
				base.CubeGrid.OnBlockRemoved -= previewGrid_OnBlockRemoved;
				base.CubeGrid.OnGridSplit -= CubeGrid_OnGridSplit;
				foreach (MyCubeBlock fatBlock in base.CubeGrid.GetFatBlocks())
				{
					if (fatBlock is Sandbox.ModAPI.IMyTerminalBlock)
					{
						fatBlock.CheckConnectionChanged -= TerminalBlockOnCheckConnectionChanged;
					}
				}
			}
		}

		public void UpdateScreen()
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateScreen(CheckIsWorking());
			}
		}

		public bool IsInRange()
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null)
			{
				return false;
			}
			return Vector3D.DistanceSquared(base.PositionComp.WorldVolume.Center, mainCamera.Position) < 2500.0;
		}

		private bool CanBuild(MySlimBlock cubeBlock)
		{
			return CanBuild(cubeBlock, checkHavokIntersections: false) == BuildCheckResult.OK;
		}

		public BuildCheckResult CanBuild(MySlimBlock projectedBlock, bool checkHavokIntersections)
		{
			if (!AllowWelding)
			{
				return BuildCheckResult.NotWeldable;
			}
			MyBlockOrientation orientation = projectedBlock.Orientation;
			orientation.GetQuaternion(out Quaternion result);
			Quaternion result2 = Quaternion.Identity;
			base.Orientation.GetQuaternion(out result2);
			result = Quaternion.Multiply(Quaternion.Multiply(result2, ProjectionRotationQuaternion), result);
			Vector3I vector3I = base.CubeGrid.WorldToGridInteger(projectedBlock.CubeGrid.GridIntegerToWorld(projectedBlock.Min));
			Vector3I vector3I2 = base.CubeGrid.WorldToGridInteger(projectedBlock.CubeGrid.GridIntegerToWorld(projectedBlock.Max));
			Vector3I position = base.CubeGrid.WorldToGridInteger(projectedBlock.CubeGrid.GridIntegerToWorld(projectedBlock.Position));
			Vector3I vector3I3 = new Vector3I(Math.Min(vector3I.X, vector3I2.X), Math.Min(vector3I.Y, vector3I2.Y), Math.Min(vector3I.Z, vector3I2.Z));
			Vector3I vector3I4 = new Vector3I(Math.Max(vector3I.X, vector3I2.X), Math.Max(vector3I.Y, vector3I2.Y), Math.Max(vector3I.Z, vector3I2.Z));
			vector3I = vector3I3;
			vector3I2 = vector3I4;
			if (!base.CubeGrid.CanAddCubes(vector3I, vector3I2))
			{
				return BuildCheckResult.IntersectedWithGrid;
			}
			MyGridPlacementSettings settings = default(MyGridPlacementSettings);
			settings.SnapMode = SnapMode.OneFreeAxis;
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = projectedBlock.BlockDefinition.GetBuildProgressModelMountPoints(1f);
			if (MyCubeGrid.CheckConnectivity(base.CubeGrid, projectedBlock.BlockDefinition, buildProgressModelMountPoints, ref result, ref position))
			{
				if (base.CubeGrid.GetCubeBlock(position) == null)
				{
					if (checkHavokIntersections)
					{
						if (MyCubeGrid.TestPlacementAreaCube(base.CubeGrid, ref settings, vector3I, vector3I2, orientation, projectedBlock.BlockDefinition, 0uL, base.CubeGrid))
						{
							return BuildCheckResult.OK;
						}
						return BuildCheckResult.IntersectedWithSomethingElse;
					}
					return BuildCheckResult.OK;
				}
				return BuildCheckResult.AlreadyBuilt;
			}
			return BuildCheckResult.NotConnected;
		}

		public void Build(MySlimBlock cubeBlock, long owner, long builder, bool requestInstant = true, long builtBy = 0L)
		{
			ulong steamId = MySession.Static.Players.TryGetSteamId(owner);
			if (AllowWelding && MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(cubeBlock.BlockDefinition, steamId))
			{
				MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.BuildInternal, cubeBlock.Position, owner, builder, requestInstant, builtBy);
			}
		}

		[Event(null, 1664)]
		[Reliable]
		[Server]
		private void BuildInternal(Vector3I cubeBlockPosition, long owner, long builder, bool requestInstant = true, long builtBy = 0L)
		{
			ulong num = MySession.Static.Players.TryGetSteamId(owner);
			MySlimBlock cubeBlock = ProjectedGrid.GetCubeBlock(cubeBlockPosition);
			if (cubeBlock == null || !AllowWelding || !MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(cubeBlock.BlockDefinition, num))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, kick: false);
				return;
			}
			Quaternion result = Quaternion.Identity;
			MyBlockOrientation orientation = cubeBlock.Orientation;
			Quaternion result2 = Quaternion.Identity;
			base.Orientation.GetQuaternion(out result2);
			orientation.GetQuaternion(out result);
			result = Quaternion.Multiply(ProjectionRotationQuaternion, result);
			result = Quaternion.Multiply(result2, result);
			MyCubeGrid cubeGrid = base.CubeGrid;
			MyCubeGrid cubeGrid2 = cubeBlock.CubeGrid;
			Vector3I vector3I = (cubeBlock.FatBlock != null) ? cubeBlock.FatBlock.Min : cubeBlock.Position;
			Vector3I gridCoords = (cubeBlock.FatBlock != null) ? cubeBlock.FatBlock.Max : cubeBlock.Position;
			Vector3I vector3I2 = cubeGrid.WorldToGridInteger(cubeGrid2.GridIntegerToWorld(vector3I));
			Vector3I vector3I3 = cubeGrid.WorldToGridInteger(cubeGrid2.GridIntegerToWorld(gridCoords));
			Vector3I center = cubeGrid.WorldToGridInteger(cubeGrid2.GridIntegerToWorld(cubeBlock.Position));
			MyCubeGrid.MyBlockLocation location = new MyCubeGrid.MyBlockLocation(min: new Vector3I(Math.Min(vector3I2.X, vector3I3.X), Math.Min(vector3I2.Y, vector3I3.Y), Math.Min(vector3I2.Z, vector3I3.Z)), max: new Vector3I(Math.Max(vector3I2.X, vector3I3.X), Math.Max(vector3I2.Y, vector3I3.Y), Math.Max(vector3I2.Z, vector3I3.Z)), blockDefinition: cubeBlock.BlockDefinition.Id, center: center, orientation: result, entityId: 0L, owner: owner);
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = null;
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (m_originalGridBuilders != null && m_originalGridBuilders.Count > 0) ? m_originalGridBuilders[0] : null;
			if (myObjectBuilder_CubeGrid != null)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock2 in myObjectBuilder_CubeGrid.CubeBlocks)
				{
					if ((Vector3I)cubeBlock2.Min == vector3I && cubeBlock2.GetId() == cubeBlock.BlockDefinition.Id)
					{
						myObjectBuilder_CubeBlock = (MyObjectBuilder_CubeBlock)cubeBlock2.Clone();
						myObjectBuilder_CubeBlock.SetupForProjector();
						if (MyDefinitionManagerBase.Static != null && myObjectBuilder_CubeBlock is MyObjectBuilder_BatteryBlock)
						{
							MyBatteryBlockDefinition myBatteryBlockDefinition = (MyBatteryBlockDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(myObjectBuilder_CubeBlock);
							((MyObjectBuilder_BatteryBlock)myObjectBuilder_CubeBlock).CurrentStoredPower = myBatteryBlockDefinition.InitialStoredPowerRatio * myBatteryBlockDefinition.MaxStoredPower;
						}
					}
				}
			}
			if (myObjectBuilder_CubeBlock == null)
			{
				myObjectBuilder_CubeBlock = cubeBlock.GetObjectBuilder();
				location.EntityId = MyEntityIdentifier.AllocateId();
			}
			myObjectBuilder_CubeBlock.ConstructionInventory = null;
			myObjectBuilder_CubeBlock.BuiltBy = builtBy;
			bool instantBuild = requestInstant && MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value);
			MyStringHash skinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(cubeBlock.SkinSubtypeId, num) ?? MyStringHash.NullOrEmpty;
			MyCubeGrid.MyBlockVisuals visuals = new MyCubeGrid.MyBlockVisuals(cubeBlock.ColorMaskHSV.PackHSVToUint(), skinId);
			cubeGrid.BuildBlockRequestInternal(visuals, location, myObjectBuilder_CubeBlock, builder, instantBuild, owner, MyEventContext.Current.IsLocallyInvoked ? num : MyEventContext.Current.Sender.Value);
			HideCube(cubeBlock);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 1748)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.RemoveItems(panelIndex, selection);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		[Event(null, 1759)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.SelectItems(panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 1770)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 1784)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		internal void SetNewBlueprint(List<MyObjectBuilder_CubeGrid> gridBuilders)
		{
			m_originalGridBuilders = gridBuilders;
			List<MyObjectBuilder_CubeGrid> originalGridBuilders = m_originalGridBuilders;
			m_clipboard.SetGridFromBuilders(originalGridBuilders.ToArray(), Vector3.Zero, 0f, deactivate: false);
			BoundingBox boundingBox = BoundingBox.CreateInvalid();
			MyCubeSize gridSize = MyCubeSize.Small;
			foreach (MyObjectBuilder_CubeGrid item in originalGridBuilders)
			{
				boundingBox = boundingBox.Include(item.CalculateBoundingBox());
				if (item.GridSizeEnum == MyCubeSize.Large)
				{
					gridSize = item.GridSizeEnum;
				}
			}
			if ((bool)m_instantBuildingEnabled)
			{
				ResetRotation();
				m_projectionOffset.Y = Math.Abs((int)(boundingBox.Min.Y / MyDefinitionManager.Static.GetCubeSize(gridSize))) + 2;
			}
			if (base.Enabled && base.IsWorking)
			{
				if (BlockDefinition.AllowScaling)
				{
					m_projectionScale = MathHelper.Clamp(base.CubeGrid.GridSize / boundingBox.Size.Max(), 0.02f, 1f);
				}
				InitializeClipboard();
			}
		}

		internal void SetNewOffset(Vector3I positionOffset, Vector3I rotationOffset, bool onlyCanBuildBlock)
		{
			m_clipboard.ResetGridOrientation();
			m_projectionOffset = positionOffset;
			m_projectionRotation = rotationOffset;
			m_showOnlyBuildable = onlyCanBuildBlock;
			SetRotation(m_clipboard, m_projectionRotation);
		}

		private void SendNewBlueprint(List<MyObjectBuilder_CubeGrid> projectedGrids)
		{
			SetNewBlueprint(projectedGrids);
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnNewBlueprintSuccess, projectedGrids);
		}

		[Event(null, 1846)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void OnNewBlueprintSuccess(List<MyObjectBuilder_CubeGrid> projectedGrids)
		{
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				return;
			}
			if (!MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value))
			{
				bool flag = false;
				foreach (MyObjectBuilder_CubeGrid projectedGrid in projectedGrids)
				{
					MyObjectBuilder_CubeGrid grid = projectedGrid;
					RemoveScriptsFromProjection(ref grid);
				}
				if (flag)
				{
					MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.ShowScriptRemoveMessage, MyEventContext.Current.Sender);
				}
			}
			SetNewBlueprint(projectedGrids);
		}

		private bool RemoveScriptsFromProjection(ref MyObjectBuilder_CubeGrid grid)
		{
			bool result = false;
			foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
			{
				MyObjectBuilder_MyProgrammableBlock myObjectBuilder_MyProgrammableBlock = cubeBlock as MyObjectBuilder_MyProgrammableBlock;
				if (myObjectBuilder_MyProgrammableBlock != null && myObjectBuilder_MyProgrammableBlock.Program != null)
				{
					myObjectBuilder_MyProgrammableBlock.Program = null;
					result = true;
				}
			}
			return result;
		}

		[Event(null, 1885)]
		[Reliable]
		[Client]
		private void ShowScriptRemoveMessage()
		{
			MyHud.Notifications.Add(new MyHudNotification(MySpaceTexts.Notification_BlueprintScriptRemoved, 5000, "Red"));
		}

		public void SendNewOffset(Vector3I positionOffset, Vector3I rotationOffset, float scale, bool showOnlyBuildable)
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnOffsetChangedSuccess, positionOffset, rotationOffset, scale, showOnlyBuildable);
		}

		[Event(null, 1897)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnOffsetChangedSuccess(Vector3I positionOffset, Vector3I rotationOffset, float scale, bool showOnlyBuildable)
		{
			m_projectionScale = scale;
			SetNewOffset(positionOffset, rotationOffset, showOnlyBuildable);
			m_shouldUpdateProjection = true;
		}

		public void SendRemoveProjection()
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnRemoveProjectionRequest);
		}

		[Event(null, 1910)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveProjectionRequest()
		{
			RemoveProjection(keepProjection: false);
		}

		private void SendSpawnProjection()
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnSpawnProjection);
		}

		private void SendConfirmSpawnProjection()
		{
			MyMultiplayer.RaiseEvent(this, (MyProjectorBase x) => x.OnConfirmSpawnProjection);
		}

		void Sandbox.ModAPI.IMyProjector.SetProjectedGrid(MyObjectBuilder_CubeGrid grid)
		{
			if (grid != null)
			{
				MyObjectBuilder_CubeGrid gridBuilder = null;
				m_originalGridBuilders = null;
				Parallel.Start(delegate
				{
					gridBuilder = (MyObjectBuilder_CubeGrid)grid.Clone();
					m_clipboard.ProcessCubeGrid(gridBuilder);
					MyEntities.RemapObjectBuilder(gridBuilder);
				}, delegate
				{
					if (gridBuilder != null && m_originalGridBuilders == null)
					{
						m_originalGridBuilders = new List<MyObjectBuilder_CubeGrid>(1);
						m_originalGridBuilders.Add(gridBuilder);
						SendNewBlueprint(m_originalGridBuilders);
					}
				});
			}
			else
			{
				SendRemoveProjection();
			}
		}

		BuildCheckResult Sandbox.ModAPI.IMyProjector.CanBuild(VRage.Game.ModAPI.IMySlimBlock projectedBlock, bool checkHavokIntersections)
		{
			return CanBuild((MySlimBlock)projectedBlock, checkHavokIntersections);
		}

		void Sandbox.ModAPI.IMyProjector.Build(VRage.Game.ModAPI.IMySlimBlock cubeBlock, long owner, long builder, bool requestInstant, long builtBy = 0L)
		{
			Build((MySlimBlock)cubeBlock, owner, builder, requestInstant, builtBy);
		}

		void Sandbox.ModAPI.Ingame.IMyProjector.UpdateOffsetAndRotation()
		{
			OnOffsetsChanged();
		}

		bool Sandbox.ModAPI.IMyProjector.LoadRandomBlueprint(string searchPattern)
		{
			bool result = false;
			string[] files = Directory.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data", "Blueprints"), searchPattern);
			if (files.Length != 0)
			{
				int num = MyRandom.Instance.Next() % files.Length;
				result = LoadBlueprint(files[num]);
			}
			return result;
		}

		bool Sandbox.ModAPI.IMyProjector.LoadBlueprint(string path)
		{
			return LoadBlueprint(path);
		}

		private bool LoadBlueprint(string path)
		{
			bool result = false;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyBlueprintUtils.LoadPrefab(path);
			if (myObjectBuilder_Definitions != null)
			{
				result = MyGuiBlueprintScreen.CopyBlueprintPrefabToClipboard(myObjectBuilder_Definitions, m_clipboard);
			}
			OnBlueprintScreen_Closed(null);
			return result;
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}
	}
}
