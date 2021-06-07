#define VRAGE
using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Gui;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Cockpit))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyCockpit),
		typeof(Sandbox.ModAPI.Ingame.IMyCockpit)
	})]
	public class MyCockpit : MyShipController, IMyCameraController, IMyUsableEntity, Sandbox.ModAPI.IMyCockpit, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyCockpit, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, Sandbox.ModAPI.IMyTextSurfaceProvider, IMyConveyorEndpointBlock, IMyGasBlock, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner
	{
		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyCockpit, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyCockpit, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyCockpit, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyCockpit, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyCockpit, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyCockpit, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyCockpit, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class OnRequestRemovePilot_003C_003E : ICallSite<MyCockpit, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRequestRemovePilot();
			}
		}

		protected sealed class AttachPilotEvent_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64 : ICallSite<MyCockpit, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in UseActionEnum actionEnum, in long characterID, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AttachPilotEvent(actionEnum, characterID);
			}
		}

		protected sealed class SetAutopilot_Client_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_AutopilotBase : ICallSite<MyCockpit, MyObjectBuilder_AutopilotBase, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in MyObjectBuilder_AutopilotBase autopilot, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetAutopilot_Client(autopilot);
			}
		}

		protected sealed class NotifyClientPilotChanged_003C_003ESystem_Int64_0023System_Nullable_00601_003CVRageMath_Matrix_003E : ICallSite<MyCockpit, long, Matrix?, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCockpit @this, in long pilotEntityId, in Matrix? pilotRelativeWorld, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.NotifyClientPilotChanged(pilotEntityId, pilotRelativeWorld);
			}
		}

		protected class m_oxygenFillLevel_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType oxygenFillLevel;
				ISyncType result = oxygenFillLevel = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyCockpit)P_0).m_oxygenFillLevel = (Sync<float, SyncDirection.FromServer>)oxygenFillLevel;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyCockpit_003C_003EActor : IActivator, IActivator<MyCockpit>
		{
			private sealed override object CreateInstance()
			{
				return new MyCockpit();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCockpit CreateInstance()
			{
				return new MyCockpit();
			}

			MyCockpit IActivator<MyCockpit>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly float DEFAULT_FPS_CAMERA_X_ANGLE = -10f;

		public static float MAX_SHAKE_DAMAGE = 500f;

		public const double MAX_DRAW_DISTANCE = 200.0;

		private bool m_isLargeCockpit;

		private Vector3 m_playerHeadSpring;

		private Vector3 m_playerHeadShakeDir;

		protected float MinHeadLocalXAngle = -60f;

		protected float MaxHeadLocalXAngle = 70f;

		protected float MinHeadLocalYAngle = -90f;

		protected float MaxHeadLocalYAngle = 90f;

		private MatrixD m_cameraDummy = MatrixD.Identity;

		protected MatrixD m_characterDummy = MatrixD.Identity;

		protected MyCharacter m_pilot;

		private MyCharacter m_savedPilot;

		private long m_serverSidePilotId;

		private Matrix? m_pilotRelativeWorld;

		private MyAutopilotBase m_aiPilot;

		protected MyDefinitionId? m_pilotGunDefinition;

		private bool m_updateSink;

		private float m_headLocalXAngle;

		private float m_headLocalYAngle;

		private long m_lastGasInputUpdateTick;

		private string m_cockpitInteriorModel;

		private bool m_defferAttach;

		private bool m_playIdleSound;

		private float m_currentCameraShakePower;

		private bool? m_lastNearFlag;

		private int m_forcedFpsTimeoutMs;

		private const int m_forcedFpsTimeoutDefaultMs = 500;

		private float MIN_SHAKE_ACC = 1f;

		private float MAX_SHAKE_ACC = 10f;

		private float MAX_SHAKE = 0.5f;

		protected Action<MyEntity> m_pilotClosedHandler;

		private bool? m_pilotJetpackEnabledBackup;

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBox;

		private bool m_isInFirstPersonView = true;

		private bool m_wasCameraForced;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private readonly Sync<float, SyncDirection.FromServer> m_oxygenFillLevel;

		private bool m_retryAttachPilot;

		private bool m_pilotFirstPerson;

		private bool m_isTextPanelOpen;

		private readonly Vector3I[] m_neighbourPositions = new Vector3I[26]
		{
			new Vector3I(1, 0, 0),
			new Vector3I(-1, 0, 0),
			new Vector3I(0, 0, -1),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 1, 0),
			new Vector3I(0, -1, 0),
			new Vector3I(1, 1, 0),
			new Vector3I(-1, 1, 0),
			new Vector3I(1, -1, 0),
			new Vector3I(-1, -1, 0),
			new Vector3I(1, 1, -1),
			new Vector3I(-1, 1, -1),
			new Vector3I(1, -1, -1),
			new Vector3I(-1, -1, -1),
			new Vector3I(1, 0, -1),
			new Vector3I(-1, 0, -1),
			new Vector3I(0, 1, -1),
			new Vector3I(0, -1, -1),
			new Vector3I(1, 1, 1),
			new Vector3I(-1, 1, 1),
			new Vector3I(1, -1, 1),
			new Vector3I(-1, -1, 1),
			new Vector3I(1, 0, 1),
			new Vector3I(-1, 0, 1),
			new Vector3I(0, 1, 1),
			new Vector3I(0, -1, 1)
		};

		private static readonly MyDefinitionId[] m_forgetTheseWeapons = new MyDefinitionId[1]
		{
			new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer))
		};

		public MyAutopilotBase AiPilot => m_aiPilot;

		public bool PilotJetpackEnabledBackup
		{
			get
			{
				if (m_pilotJetpackEnabledBackup.HasValue)
				{
					return m_pilotJetpackEnabledBackup.Value;
				}
				return false;
			}
		}

		public virtual bool IsInFirstPersonView
		{
			get
			{
				return m_isInFirstPersonView;
			}
			set
			{
				bool isInFirstPersonView = m_isInFirstPersonView;
				m_isInFirstPersonView = value;
				if (MySession.Static != null && !MySession.Static.Enable3RdPersonView)
				{
					m_isInFirstPersonView = true;
				}
				if (m_isInFirstPersonView != isInFirstPersonView && !ForceFirstPersonCamera)
				{
					UpdateCameraAfterChange();
				}
			}
		}

		public override bool ForceFirstPersonCamera
		{
			get
			{
				if (base.ForceFirstPersonCamera || MyThirdPersonSpectator.Static.IsCameraForced())
				{
					return m_forcedFpsTimeoutMs <= 0;
				}
				return false;
			}
			set
			{
				if (value && !base.ForceFirstPersonCamera)
				{
					m_forcedFpsTimeoutMs = 500;
				}
				base.ForceFirstPersonCamera = value;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public float OxygenFillLevel
		{
			get
			{
				return m_oxygenFillLevel;
			}
			private set
			{
				m_oxygenFillLevel.Value = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyCockpit.OxygenFilledRatio => OxygenFillLevel;

		public float OxygenAmount
		{
			get
			{
				return OxygenFillLevel * BlockDefinition.OxygenCapacity;
			}
			set
			{
				if (BlockDefinition.OxygenCapacity != 0f)
				{
					ChangeGasFillLevel(MathHelper.Clamp(value / BlockDefinition.OxygenCapacity, 0f, 1f));
				}
				base.ResourceSink.Update();
			}
		}

		public bool CanPressurizeRoom => false;

		float Sandbox.ModAPI.Ingame.IMyCockpit.OxygenCapacity => BlockDefinition.OxygenCapacity;

		public float OxygenAmountMissing => (1f - OxygenFillLevel) * BlockDefinition.OxygenCapacity;

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

		public override float HeadLocalXAngle
		{
			get
			{
				return m_headLocalXAngle;
			}
			set
			{
				m_headLocalXAngle = value;
			}
		}

		public override float HeadLocalYAngle
		{
			get
			{
				return m_headLocalYAngle;
			}
			set
			{
				m_headLocalYAngle = value;
			}
		}

		public Vector3I[] NeighbourPositions => m_neighbourPositions;

		public new MyCockpitDefinition BlockDefinition => base.BlockDefinition as MyCockpitDefinition;

		public override MyCharacter Pilot
		{
			get
			{
				if (m_pilot == null && m_savedPilot != null)
				{
					return m_savedPilot;
				}
				return m_pilot;
			}
		}

		public MyEntity IsBeingUsedBy => m_pilot;

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return IsInFirstPersonView;
			}
			set
			{
				IsInFirstPersonView = value;
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				if (!ForceFirstPersonCamera)
				{
					return !MySession.Static.Settings.Enable3rdPersonView;
				}
				return true;
			}
			set
			{
				ForceFirstPersonCamera = value;
			}
		}

		bool IMyCameraController.AllowCubeBuilding => false;

		float Sandbox.ModAPI.IMyCockpit.OxygenFilledRatio
		{
			get
			{
				return OxygenFillLevel;
			}
			set
			{
				OxygenAmount = value * BlockDefinition.OxygenCapacity;
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

		public static event Action OnPilotAttached;

		public MyCockpit()
		{
			m_pilotClosedHandler = m_pilot_OnMarkForClose;
			base.ResourceSink = new MyResourceSinkComponent(2);
			base.Render = new MyRenderComponentCockpit(this);
			m_soundEmitter.EmitterMethods[1].Add(new Func<bool>(ShouldPlay2D));
		}

		private bool ShouldPlay2D()
		{
			if (MySession.Static.LocalCharacter != null)
			{
				return Pilot == MySession.Static.LocalCharacter;
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
			m_isLargeCockpit = (cubeBlockDefinition.CubeSize == MyCubeSize.Large);
			m_cockpitInteriorModel = BlockDefinition.InteriorModel;
			base.NeedsWorldMatrix = true;
			base.InvalidateOnMove = true;
			base.IsWorkingChanged += MyCockpit_IsWorkingChanged;
			if (m_cockpitInteriorModel == null)
			{
				base.NeedsWorldMatrix = false;
				base.InvalidateOnMove = false;
				base.Render = new MyRenderComponentCockpit(this);
			}
			base.Init(objectBuilder, cubeGrid);
			PostBaseInit();
			MyObjectBuilder_Cockpit myObjectBuilder_Cockpit = (MyObjectBuilder_Cockpit)objectBuilder;
			if (myObjectBuilder_Cockpit.Pilot != null)
			{
				m_pilotJetpackEnabledBackup = myObjectBuilder_Cockpit.PilotJetpackEnabled;
				MyCharacter myCharacter = null;
				if (MyEntities.TryGetEntityById(myObjectBuilder_Cockpit.Pilot.EntityId, out MyEntity entity))
				{
					myCharacter = (MyCharacter)entity;
					if (myCharacter.IsUsing is MyShipController && myCharacter.IsUsing != this)
					{
						myCharacter = null;
					}
				}
				else
				{
					myCharacter = (MyCharacter)MyEntities.CreateFromObjectBuilder(myObjectBuilder_Cockpit.Pilot, base.Render.FadeIn);
				}
				if (myCharacter != null)
				{
					m_savedPilot = myCharacter;
					m_defferAttach = true;
					m_singleWeaponMode = myObjectBuilder_Cockpit.UseSingleWeaponMode;
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					if (Sync.IsServer)
					{
						base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
					}
				}
				IsInFirstPersonView = myObjectBuilder_Cockpit.IsInFirstPersonView;
			}
			if (myObjectBuilder_Cockpit.Autopilot != null)
			{
				MyAutopilotBase autopilot = MyAutopilotFactory.CreateAutopilot(myObjectBuilder_Cockpit.Autopilot);
				autopilot.Init(myObjectBuilder_Cockpit.Autopilot);
				Action<MyEntity> delayedAttachAutopilot = null;
				delayedAttachAutopilot = delegate
				{
					AttachAutopilot(autopilot, updateSync: false);
					base.AddedToScene -= delayedAttachAutopilot;
				};
				base.AddedToScene += delayedAttachAutopilot;
			}
			m_pilotGunDefinition = myObjectBuilder_Cockpit.PilotGunDefinition;
			m_pilotRelativeWorld = (myObjectBuilder_Cockpit.PilotRelativeWorld.HasValue ? new Matrix?(myObjectBuilder_Cockpit.PilotRelativeWorld.Value.GetMatrix()) : null);
			if (m_pilotGunDefinition.HasValue && m_pilotGunDefinition.Value.TypeId == typeof(MyObjectBuilder_AutomaticRifle) && string.IsNullOrEmpty(m_pilotGunDefinition.Value.SubtypeName))
			{
				m_pilotGunDefinition = new MyDefinitionId(typeof(MyObjectBuilder_AutomaticRifle), "RifleGun");
			}
			if (!string.IsNullOrEmpty(m_cockpitInteriorModel))
			{
				if (MyModels.GetModelOnlyDummies(m_cockpitInteriorModel).Dummies.ContainsKey("head"))
				{
					m_headLocalPosition = MyModels.GetModelOnlyDummies(m_cockpitInteriorModel).Dummies["head"].Matrix.Translation;
				}
			}
			else if (MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies.ContainsKey("head"))
			{
				m_headLocalPosition = MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies["head"].Matrix.Translation;
			}
			AddDebugRenderComponent(new MyDebugRenderComponentCockpit(this));
			InitializeConveyorEndpoint();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			m_oxygenFillLevel.SetLocalValue(myObjectBuilder_Cockpit.OxygenLevel);
			List<MyResourceSinkInfo> list = new List<MyResourceSinkInfo>();
			MyResourceSinkInfo item = new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = 0f,
				RequiredInputFunc = CalculateRequiredPowerInput
			};
			list.Add(item);
			item = new MyResourceSinkInfo
			{
				ResourceTypeId = MyCharacterOxygenComponent.OxygenId,
				MaxRequiredInput = BlockDefinition.OxygenCapacity,
				RequiredInputFunc = ComputeRequiredGas
			};
			list.Add(item);
			List<MyResourceSinkInfo> sinkData = list;
			base.ResourceSink.Init(MyStringHash.GetOrCompute("Utility"), sinkData);
			base.ResourceSink.CurrentInputChanged += Sink_CurrentInputChanged;
			m_lastGasInputUpdateTick = MySession.Static.ElapsedGameTime.Ticks;
			MyInventory inventory = this.GetInventory();
			if (inventory == null && BlockDefinition.HasInventory)
			{
				Vector3 size = Vector3.One * 1f;
				inventory = new MyInventory(size.Volume, size, MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)inventory);
			}
			if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_multiPanel = new MyMultiTextPanelComponent(this, BlockDefinition.ScreenAreas, myObjectBuilder_Cockpit.TextPanels);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
		}

		private void MyCockpit_IsWorkingChanged(MyCubeBlock obj)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void CreateTerminalControls()
		{
			base.CreateTerminalControls();
			if (!MyTerminalControlFactory.AreControlsCreated<MyCockpit>())
			{
				MyMultiTextPanelComponent.CreateTerminalControls<MyCockpit>();
			}
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			}
			RaisePropertiesChanged();
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 422)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.RemoveItems(panelIndex, selection);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		[Event(null, 433)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.SelectItems(panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 444)]
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
				MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 458)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
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
			MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 481)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		[Event(null, 492)]
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
				MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 585)]
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

		protected virtual void PostBaseInit()
		{
			TryGetDummies();
		}

		private float CalculateRequiredPowerInput()
		{
			if (base.IsFunctional && BlockDefinition.EnableShipControl && base.CubeGrid.GridSystems.ResourceDistributor.ResourceState != MyResourceStateEnum.NoPower)
			{
				return 0.003f;
			}
			return 0f;
		}

		private float ComputeRequiredGas()
		{
			if (!base.IsWorking)
			{
				return 0f;
			}
			return Math.Min(OxygenAmountMissing * 60f / 100f, base.ResourceSink.MaxRequiredInputByType(MyCharacterOxygenComponent.OxygenId) * 0.1f);
		}

		protected override void ComponentStack_IsFunctionalChanged()
		{
			base.ComponentStack_IsFunctionalChanged();
			if (!base.IsFunctional)
			{
				if (m_pilot != null)
				{
					RemovePilot();
				}
				ChangeGasFillLevel(0f);
				base.ResourceSink.Update();
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Cockpit myObjectBuilder_Cockpit = (MyObjectBuilder_Cockpit)base.GetObjectBuilderCubeBlock(copy);
			if (m_pilot != null && m_pilot.Save)
			{
				myObjectBuilder_Cockpit.Pilot = (MyObjectBuilder_Character)m_pilot.GetObjectBuilder(copy);
			}
			else if (m_savedPilot != null && m_savedPilot.Save)
			{
				myObjectBuilder_Cockpit.Pilot = (MyObjectBuilder_Character)m_savedPilot.GetObjectBuilder(copy);
			}
			else
			{
				myObjectBuilder_Cockpit.Pilot = null;
			}
			myObjectBuilder_Cockpit.PilotJetpackEnabled = ((myObjectBuilder_Cockpit.Pilot != null) ? m_pilotJetpackEnabledBackup : null);
			myObjectBuilder_Cockpit.Autopilot = ((m_aiPilot != null) ? m_aiPilot.GetObjectBuilder() : null);
			myObjectBuilder_Cockpit.PilotGunDefinition = m_pilotGunDefinition;
			if (m_pilotRelativeWorld.HasValue)
			{
				myObjectBuilder_Cockpit.PilotRelativeWorld = new MyPositionAndOrientation(m_pilotRelativeWorld.Value);
			}
			else
			{
				myObjectBuilder_Cockpit.PilotRelativeWorld = null;
			}
			myObjectBuilder_Cockpit.IsInFirstPersonView = IsInFirstPersonView;
			myObjectBuilder_Cockpit.OxygenLevel = OxygenFillLevel;
			myObjectBuilder_Cockpit.TextPanels = ((m_multiPanel != null) ? m_multiPanel.Serialize() : null);
			return myObjectBuilder_Cockpit;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public override MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false)
		{
			MatrixD worldMatrix = base.PositionComp.WorldMatrix;
			float degrees = m_headLocalXAngle;
			float headLocalYAngle = m_headLocalYAngle;
			if (!includeX)
			{
				degrees = DEFAULT_FPS_CAMERA_X_ANGLE;
			}
			MatrixD matrix = MatrixD.CreateFromAxisAngle(Vector3D.Right, MathHelper.ToRadians(degrees));
			if (includeY)
			{
				matrix *= Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(headLocalYAngle));
			}
			matrix *= m_cameraDummy;
			worldMatrix = matrix * worldMatrix;
			Vector3D translation = worldMatrix.Translation;
			if (m_headLocalPosition != Vector3.Zero)
			{
				translation = Vector3D.Transform(m_headLocalPosition + m_playerHeadSpring, base.PositionComp.WorldMatrix);
			}
			else if (Pilot != null)
			{
				translation = Pilot.GetHeadMatrix(includeY, includeX, forceHeadAnim: true, forceHeadBone: true, preferLocalOverSync: true).Translation;
			}
			worldMatrix.Translation = translation;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
			{
				MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, 0.05f, Color.Yellow, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Cockpit camera", Color.Yellow, 0.5f, depthRead: false);
			}
			MatrixD result = worldMatrix;
			result.Translation = translation;
			return result;
		}

		public void Rotate(Vector2 rotationIndicator, float roll)
		{
			float num = MyInput.Static.GetMouseSensitivity() * 0.13f;
			if (rotationIndicator.X != 0f)
			{
				m_headLocalXAngle = MathHelper.Clamp(m_headLocalXAngle - rotationIndicator.X * num, MinHeadLocalXAngle, MaxHeadLocalXAngle);
			}
			if (rotationIndicator.Y != 0f)
			{
				bool isInFirstPersonView = IsInFirstPersonView;
				if (MinHeadLocalYAngle != 0f && isInFirstPersonView)
				{
					m_headLocalYAngle = MathHelper.Clamp(m_headLocalYAngle - rotationIndicator.Y * num, MinHeadLocalYAngle, MaxHeadLocalYAngle);
				}
				else
				{
					m_headLocalYAngle -= rotationIndicator.Y * num;
				}
			}
			if (!IsInFirstPersonView)
			{
				MyThirdPersonSpectator.Static.Rotate(rotationIndicator, roll);
			}
			rotationIndicator = Vector2.Zero;
		}

		public void RotateStopped()
		{
			MoveAndRotateStopped();
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
			MyHud.SetHudDefinition(BlockDefinition.HUD);
			UpdateCameraAfterChange();
		}

		public override MatrixD GetViewMatrix()
		{
			bool flag = !ForceFirstPersonCamera;
			if (!IsInFirstPersonView && flag)
			{
				return MyThirdPersonSpectator.Static.GetViewMatrix();
			}
			MatrixD matrix = GetHeadMatrix(IsInFirstPersonView || ForceFirstPersonCamera, IsInFirstPersonView || ForceFirstPersonCamera);
			MatrixD.Invert(ref matrix, out MatrixD result);
			return result;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_updateSink)
			{
				base.ResourceSink.Update();
				m_updateSink = false;
			}
			if (m_savedPilot != null && !base.MarkedForClose && !base.Closed && !m_savedPilot.MarkedForClose && !m_savedPilot.Closed)
			{
				if ((m_savedPilot.NeedsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
				{
					m_savedPilot.UpdateOnceBeforeFrame();
					m_savedPilot.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					MySession.Static.Players.UpdatePlayerControllers(base.EntityId);
					MySession.Static.Players.UpdatePlayerControllers(m_savedPilot.EntityId);
				}
				AttachPilot(m_savedPilot, storeOriginalPilotWorld: false, calledFromInit: true);
			}
			m_savedPilot = null;
			m_defferAttach = false;
			UpdateScreen();
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking() && base.ResourceSink.RequiredInput > 0f)
			{
				return base.ResourceSink.IsPowered;
			}
			return false;
		}

		public override void CheckEmissiveState(bool force = false)
		{
		}

		public void UpdateScreen()
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateScreen(CheckIsWorking());
			}
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
			if (CheckIsWorking())
			{
				(base.Render as MyRenderComponentCockpit).UpdateModelProperties();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_soundEmitter != null && m_soundEmitter.VolumeMultiplier < 1f)
			{
				m_soundEmitter.VolumeMultiplier = Math.Min(1f, m_soundEmitter.VolumeMultiplier + 0.005f);
			}
			if (m_forcedFpsTimeoutMs > 0)
			{
				m_forcedFpsTimeoutMs -= 16;
			}
			if (m_pilot != null && base.ControllerInfo.IsLocallyHumanControlled() && base.CubeGrid.Physics != null)
			{
				float num = base.CubeGrid.Physics.LinearAcceleration.Length();
				float num2 = base.CubeGrid.Physics.LinearVelocity.Length();
				if (num > 0f && num2 > 0f)
				{
					float num3 = MathHelper.Clamp((Vector3.Dot(Vector3.Normalize(base.CubeGrid.Physics.LinearVelocity), Vector3.Normalize(base.CubeGrid.Physics.LinearAcceleration)) * num - MIN_SHAKE_ACC) / (MAX_SHAKE_ACC - MIN_SHAKE_ACC), 0f, 1f);
					AddShake(MAX_SHAKE * num3);
				}
			}
			bool flag = !IsInFirstPersonView && ForceFirstPersonCamera;
			if (m_wasCameraForced != flag)
			{
				UpdateCameraAfterChange(resetHeadLocalAngle: false);
			}
			m_wasCameraForced = flag;
			if (MyDebugDrawSettings.DEBUG_DRAW_COCKPIT && m_pilotRelativeWorld.HasValue)
			{
				MatrixD matrixD = MatrixD.Multiply(m_pilotRelativeWorld.Value, base.WorldMatrix);
				if (m_lastPilot != null && m_lastPilot.Physics != null && m_lastPilot.Physics.CharacterProxy != null)
				{
					int shapeIndex = 0;
					HkShape collisionShape = m_lastPilot.Physics.CharacterProxy.GetCollisionShape();
					Vector3D translation = matrixD.Translation;
					Quaternion rotation = Quaternion.CreateFromRotationMatrix(matrixD);
					Vector3D vector3D = Vector3D.TransformNormal(m_lastPilot.Physics.Center, matrixD);
					translation += vector3D;
					matrixD.Translation += vector3D;
					MyPhysicsDebugDraw.DrawCollisionShape(collisionShape, matrixD, 1f, ref shapeIndex, "Pilot");
					List<HkBodyCollision> list = new List<HkBodyCollision>();
					MyPhysics.GetPenetrationsShape(collisionShape, ref translation, ref rotation, list, 18);
					foreach (HkBodyCollision item in list)
					{
						VRage.ModAPI.IMyEntity collisionEntity = item.GetCollisionEntity();
						if (collisionEntity != null && collisionEntity.Physics != null && !collisionEntity.Physics.IsPhantom)
						{
							MyRenderProxy.DebugDrawArrow3D(matrixD.Translation, collisionEntity.PositionComp.GetPosition(), Color.Lime, null, depthRead: false, 0.1, collisionEntity.DisplayName, 0.6f);
						}
					}
				}
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (m_soundEmitter != null)
			{
				if (hasPower && m_playIdleSound && (!m_soundEmitter.IsPlaying || (!m_soundEmitter.SoundPair.Equals(m_baseIdleSound) && !m_soundEmitter.SoundPair.Equals(GetInCockpitSound))) && !m_baseIdleSound.Equals(MySoundPair.Empty))
				{
					m_soundEmitter.VolumeMultiplier = 0f;
					m_soundEmitter.PlaySound(m_baseIdleSound, stopPrevious: true);
				}
				else if ((!hasPower || !base.IsWorking) && m_soundEmitter.IsPlaying && m_soundEmitter.SoundPair.Equals(m_baseIdleSound))
				{
					m_soundEmitter.StopSound(forced: true);
				}
			}
			if (base.GridResourceDistributor == null || GridGyroSystem == null || base.EntityThrustComponent == null)
			{
				return;
			}
			bool flag = false;
			MyEntityThrustComponent myEntityThrustComponent = base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null)
			{
				flag = myEntityThrustComponent.AutopilotEnabled;
			}
			bool flag2 = base.CubeGrid.GridSystems.ControlSystem.IsControlled || flag;
			if (Sync.IsServer)
			{
				if (!flag2 && m_aiPilot != null)
				{
					m_aiPilot.Update();
				}
				else if (flag2 && m_aiPilot != null && m_aiPilot.RemoveOnPlayerControl)
				{
					RemoveAutopilot();
				}
			}
			if (m_pilot != null && base.ControllerInfo.IsLocallyHumanControlled())
			{
				m_pilot.RadioReceiver.UpdateHud();
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (Sync.IsServer && m_pilot != null && OxygenFillLevel < 0.2f && base.CubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				RefillFromBottlesOnGrid();
			}
			base.ResourceSink.Update();
			if (Sync.IsServer)
			{
				float num = (float)(MySession.Static.ElapsedPlayTime.Ticks - m_lastGasInputUpdateTick) / 1E+07f;
				m_lastGasInputUpdateTick = MySession.Static.ElapsedPlayTime.Ticks;
				float num2 = base.ResourceSink.CurrentInputByType(MyCharacterOxygenComponent.OxygenId) * num;
				ChangeGasFillLevel(OxygenFillLevel + num2);
				if (BlockDefinition.IsPressurized)
				{
					float oxygenInPoint = MyOxygenProviderSystem.GetOxygenInPoint(base.CubeGrid.GridIntegerToWorld(base.Position));
					if (OxygenFillLevel < oxygenInPoint)
					{
						ChangeGasFillLevel(oxygenInPoint);
					}
				}
			}
			if (m_retryAttachPilot)
			{
				if (m_serverSidePilotId != 0L)
				{
					TryAttachPilot(m_serverSidePilotId);
				}
				else
				{
					m_retryAttachPilot = false;
				}
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(CheckIsWorking());
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Pilot == MySession.Static.LocalCharacter && base.CubeGrid.IsRespawnGrid)
			{
				MyIngameHelpPod1.StartingInPod = true;
			}
		}

		private void Sink_CurrentInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			OnInputChanged(resourceTypeId, oldInput, sink);
		}

		protected virtual void OnInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			UpdateIsWorking();
			if (resourceTypeId != MyCharacterOxygenComponent.OxygenId)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				return;
			}
			float num = (float)(MySession.Static.ElapsedPlayTime.Ticks - m_lastGasInputUpdateTick) / 1E+07f;
			m_lastGasInputUpdateTick = MySession.Static.ElapsedPlayTime.Ticks;
			float num2 = oldInput * num;
			ChangeGasFillLevel(OxygenFillLevel + num2);
			m_updateSink = true;
		}

		private void RefillFromBottlesOnGrid()
		{
			List<IMyConveyorEndpoint> list = new List<IMyConveyorEndpoint>();
			MyGridConveyorSystem.FindReachable(ConveyorEndpoint, list, (IMyConveyorEndpoint vertex) => vertex.CubeBlock != null && FriendlyWithBlock(vertex.CubeBlock) && vertex.CubeBlock.HasInventory);
			bool flag = false;
			foreach (IMyConveyorEndpoint item in list)
			{
				MyCubeBlock cubeBlock = item.CubeBlock;
				int inventoryCount = cubeBlock.InventoryCount;
				for (int i = 0; i < inventoryCount; i++)
				{
					MyInventory inventory = cubeBlock.GetInventory(i);
					foreach (MyPhysicalInventoryItem item2 in inventory.GetItems())
					{
						MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item2.Content as MyObjectBuilder_GasContainerObject;
						if (myObjectBuilder_GasContainerObject != null && myObjectBuilder_GasContainerObject.GasLevel != 0f)
						{
							MyOxygenContainerDefinition myOxygenContainerDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_GasContainerObject) as MyOxygenContainerDefinition;
							if (!(myOxygenContainerDefinition.StoredGasId != MyCharacterOxygenComponent.OxygenId))
							{
								float num = myObjectBuilder_GasContainerObject.GasLevel * myOxygenContainerDefinition.Capacity;
								float num2 = Math.Min(num, OxygenAmountMissing);
								if (num2 != 0f)
								{
									myObjectBuilder_GasContainerObject.GasLevel = (num - num2) / myOxygenContainerDefinition.Capacity;
									if (myObjectBuilder_GasContainerObject.GasLevel < 0f)
									{
										myObjectBuilder_GasContainerObject.GasLevel = 0f;
									}
									_ = myObjectBuilder_GasContainerObject.GasLevel;
									_ = 1f;
									inventory.UpdateGasAmount();
									flag = true;
									OxygenAmount += num2;
									if (OxygenFillLevel >= 1f)
									{
										ChangeGasFillLevel(1f);
										base.ResourceSink.Update();
										break;
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				MyHud.Notifications.Add(new MyHudNotification(MySpaceTexts.NotificationBottleRefill, 2500, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important));
			}
		}

		public override void ShowInventory()
		{
			MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, m_pilot, this);
		}

		public override void ShowTerminal()
		{
			if (base.CubeGrid.InScene)
			{
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, m_pilot, this);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (!MyEntities.CloseAllowed)
			{
				m_savedPilot = m_pilot;
				RemovePilot();
			}
			base.OnRemovedFromScene(source);
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_savedPilot != null || (m_multiPanel != null && m_multiPanel.SurfaceCount > 0))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene((BlockDefinition.InteriorModel != null) ? 1 : 0);
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
		}

		protected override void OnControlReleased(MyEntityController controller)
		{
			if (m_pilot == null || (m_pilot != null && !MySessionComponentReplay.Static.HasEntityReplayData(base.CubeGrid.EntityId)))
			{
				base.OnControlReleased(controller);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		private void m_pilot_OnMarkForClose(MyEntity obj)
		{
			if (m_pilot != null)
			{
				base.Hierarchy.RemoveChild(m_pilot);
				m_rechargeSocket.Unplug();
				m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = null;
				m_pilot = null;
			}
		}

		public void GiveControlToPilot()
		{
			MyCharacter myCharacter = m_pilot ?? m_savedPilot;
			if (myCharacter.ControllerInfo != null && myCharacter.ControllerInfo.Controller != null)
			{
				myCharacter.SwitchControl(this);
			}
		}

		public bool RemovePilot()
		{
			if (m_pilot == null)
			{
				return true;
			}
			if (Sync.IsServer && !base.CubeGrid.IsBlockTrasferInProgress)
			{
				m_serverSidePilotId = 0L;
				MyMultiplayer.RaiseEvent(this, (MyCockpit x) => NotifyClientPilotChanged, m_serverSidePilotId, m_pilotRelativeWorld);
			}
			MyAnalyticsHelper.ReportActivityEnd(m_pilot, "Cockpit");
			if (m_pilot.Physics == null)
			{
				m_pilot = null;
				return true;
			}
			StopLoopSound();
			m_pilot.OnMarkForClose -= m_pilotClosedHandler;
			if (MyVisualScriptLogicProvider.PlayerLeftCockpit != null)
			{
				MyVisualScriptLogicProvider.PlayerLeftCockpit(Name, m_pilot.GetPlayerIdentityId(), base.CubeGrid.Name);
			}
			base.Hierarchy.RemoveChild(m_pilot);
			base.NeedsWorldMatrix = (m_cockpitInteriorModel != null);
			base.InvalidateOnMove = base.NeedsWorldMatrix;
			if (m_pilot.IsDead)
			{
				if (base.ControllerInfo.Controller != null)
				{
					this.SwitchControl(m_pilot);
				}
				MyEntities.Add(m_pilot);
				m_pilot.WorldMatrix = base.WorldMatrix;
				m_pilotGunDefinition = null;
				m_rechargeSocket.Unplug();
				m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = null;
				if (m_pilot == MySession.Static.LocalCharacter)
				{
					MyLocalCache.LoadInventoryConfig(m_pilot, setModel: false);
				}
				m_pilot = null;
				return true;
			}
			bool flag = false;
			MatrixD worldMatrix = MatrixD.Identity;
			if (m_pilotRelativeWorld.HasValue)
			{
				Vector3D to = Vector3D.Transform(base.Position * base.CubeGrid.GridSize, base.CubeGrid.WorldMatrix);
				worldMatrix = MatrixD.Multiply(m_pilotRelativeWorld.Value, base.WorldMatrix);
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(worldMatrix.Translation, to, 15);
				if (hitInfo.HasValue)
				{
					VRage.ModAPI.IMyEntity hitEntity = hitInfo.Value.HkHitInfo.GetHitEntity();
					if (base.CubeGrid.Equals(hitEntity) && m_pilot.CanPlaceCharacter(ref worldMatrix))
					{
						flag = true;
					}
				}
				else if (m_pilot.CanPlaceCharacter(ref worldMatrix))
				{
					flag = true;
				}
			}
			Vector3D? vector3D = null;
			if (!flag)
			{
				vector3D = FindFreeNeighbourPosition();
				if (!vector3D.HasValue)
				{
					vector3D = base.PositionComp.GetPosition();
				}
			}
			RemovePilotFromSeat(m_pilot);
			EndShootAll();
			base.CubeGrid.GridSystems.RadioSystem.Unregister(m_pilot.RadioBroadcaster);
			base.CubeGrid.GridSystems.RadioSystem.Unregister(m_pilot.RadioReceiver);
			MyIdentity identity = m_pilot.GetIdentity();
			if (identity != null)
			{
				identity.FactionChanged -= OnCharacterFactionChanged;
			}
			if (base.CubeGrid.IsBlockTrasferInProgress)
			{
				MyCharacter pilot = m_pilot;
				m_pilot = null;
				if (base.ControllerInfo.Controller != null)
				{
					this.SwitchControl(pilot);
				}
			}
			else if (flag || vector3D.HasValue)
			{
				base.Hierarchy.RemoveChild(m_pilot);
				MatrixD worldMatrix2 = flag ? worldMatrix : MatrixD.CreateWorld(vector3D.Value - base.WorldMatrix.Up, base.WorldMatrix.Forward, base.WorldMatrix.Up);
				if (!MyEntities.CloseAllowed)
				{
					m_pilot.PositionComp.SetWorldMatrix(worldMatrix2, this);
				}
				MyEntities.Add(m_pilot);
				m_pilot.Physics.Enabled = true;
				m_rechargeSocket.Unplug();
				m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = null;
				m_pilot.Stand();
				if (m_pilotJetpackEnabledBackup.HasValue && m_pilot.JetpackComp != null)
				{
					m_pilot.JetpackComp.TurnOnJetpack(m_pilotJetpackEnabledBackup.Value);
				}
				if (base.Parent != null && base.Parent.Physics != null)
				{
					MyEntity entityById = MyEntities.GetEntityById(m_pilot.ClosestParentId);
					if (entityById != null && !Sync.IsServer)
					{
						m_pilot.Physics.LinearVelocity = entityById.Physics.LinearVelocity - base.Parent.Physics.LinearVelocity;
					}
					else
					{
						m_pilot.Physics.LinearVelocity = base.Parent.Physics.LinearVelocity;
					}
					if (base.Parent.Physics.LinearVelocity.LengthSquared() > 100f)
					{
						MyCharacterJetpackComponent jetpackComp = m_pilot.JetpackComp;
						if (jetpackComp != null)
						{
							jetpackComp.EnableDampeners(enable: true);
							jetpackComp.TurnOnJetpack(newState: true);
							m_pilot.RelativeDampeningEntity = base.CubeGrid;
							if (Sync.IsServer)
							{
								MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyPlayerCollection.SetDampeningEntityClient, m_pilot.EntityId, m_pilot.RelativeDampeningEntity.EntityId);
							}
						}
					}
				}
				MyCharacter pilot2 = m_pilot;
				m_pilot = null;
				if (base.ControllerInfo.Controller != null)
				{
					if (base.ControllerInfo.Controller.Player.IsLocalPlayer)
					{
						pilot2?.RadioReceiver.Clear();
					}
					this.SwitchControl(pilot2);
				}
				if (m_pilotGunDefinition.HasValue)
				{
					pilot2.SwitchToWeapon(m_pilotGunDefinition, sync: false);
				}
				else
				{
					pilot2.SwitchToWeapon(null, sync: false);
				}
				if (pilot2 == MySession.Static.LocalCharacter)
				{
					MyLocalCache.LoadInventoryConfig(pilot2, setModel: false);
				}
				if (MySession.Static.CameraController == this && pilot2 == MySession.Static.LocalCharacter)
				{
					_ = IsInFirstPersonView;
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, pilot2);
				}
				pilot2.IsInFirstPersonView = m_pilotFirstPerson;
				CheckEmissiveState();
				return true;
			}
			CheckEmissiveState();
			return false;
		}

		public void RequestRemovePilot()
		{
			MyMultiplayer.RaiseEvent(this, (MyCockpit x) => OnRequestRemovePilot);
		}

		[Event(null, 1425)]
		[Reliable]
		[Server(ValidationType.IgnoreDLC)]
		public void OnRequestRemovePilot()
		{
			if (MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				RemovePilot();
			}
		}

		protected virtual void RemovePilotFromSeat(MyCharacter pilot)
		{
			base.CubeGrid.UnregisterOccupiedBlock(this);
			base.CubeGrid.SetInventoryMassDirty();
		}

		public void AttachAutopilot(MyAutopilotBase newAutopilot, bool updateSync = true)
		{
			RemoveAutopilot(updateSync: false);
			m_aiPilot = newAutopilot;
			m_aiPilot.OnAttachedToShipController(this);
			if (updateSync && Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.SetAutopilot_Client, newAutopilot.GetObjectBuilder());
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		public void RemoveAutopilot(bool updateSync = true)
		{
			if (m_aiPilot != null)
			{
				m_aiPilot.OnRemovedFromCockpit();
				m_aiPilot = null;
				if (updateSync && Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent<MyCockpit, MyObjectBuilder_AutopilotBase>(this, (MyCockpit x) => x.SetAutopilot_Client, null);
				}
			}
			if (!Sync.IsServer && (base.ControllerInfo.Controller == null || !base.ControllerInfo.IsLocallyControlled()) && m_multiPanel == null)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
		}

		public void RemoveOriginalPilotPosition()
		{
			m_pilotRelativeWorld = null;
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
			UpdateNearFlag();
			if (m_enableFirstPerson)
			{
				UpdateCockpitModel();
			}
		}

		protected override void UpdateCameraAfterChange(bool resetHeadLocalAngle = true)
		{
			base.UpdateCameraAfterChange(resetHeadLocalAngle);
			if (resetHeadLocalAngle)
			{
				m_headLocalXAngle = 0f;
				m_headLocalYAngle = 0f;
			}
			UpdateNearFlag();
			if (m_enableFirstPerson)
			{
				UpdateCockpitModel();
			}
			else if (MySession.Static.IsCameraControlledObject() && MySession.Static.Settings.Enable3rdPersonView && Pilot != null && Pilot.ControllerInfo.IsLocallyControlled())
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator);
			}
		}

		private void UpdateNearFlag()
		{
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			TryGetDummies();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public virtual void UpdateCockpitModel()
		{
			if (m_cockpitInteriorModel == null)
			{
				return;
			}
			MyRenderComponentCockpit myRenderComponentCockpit = base.Render as MyRenderComponentCockpit;
			if (myRenderComponentCockpit != null && myRenderComponentCockpit.RenderObjectIDs.Length >= 2 && myRenderComponentCockpit.ExteriorRenderId != uint.MaxValue && myRenderComponentCockpit.InteriorRenderId != uint.MaxValue)
			{
				if (MySession.Static.CameraController == this && (IsInFirstPersonView || ForceFirstPersonCamera))
				{
					MyRenderProxy.UpdateRenderObjectVisibility(myRenderComponentCockpit.ExteriorRenderId, visible: false, near: false);
					MyRenderProxy.UpdateRenderObjectVisibility(myRenderComponentCockpit.InteriorRenderId, myRenderComponentCockpit.Visible, near: false);
				}
				else
				{
					MyRenderProxy.UpdateRenderObjectVisibility(myRenderComponentCockpit.ExteriorRenderId, myRenderComponentCockpit.Visible, near: false);
					MyRenderProxy.UpdateRenderObjectVisibility(myRenderComponentCockpit.InteriorRenderId, visible: false, near: false);
				}
			}
		}

		protected Vector3D? FindFreeNeighbourPosition()
		{
			int num = 512;
			int num2 = 1;
			while (num > 0)
			{
				Vector3I[] neighbourPositions = m_neighbourPositions;
				for (int i = 0; i < neighbourPositions.Length; i++)
				{
					Vector3I neighbourOffsetI = neighbourPositions[i] * num2;
					if (IsNeighbourPositionFree(neighbourOffsetI, out Vector3D translation))
					{
						return translation;
					}
				}
				num2++;
				num--;
			}
			return null;
		}

		public bool IsNeighbourPositionFree(Vector3I neighbourOffsetI, out Vector3D translation)
		{
			Vector3D value = 0.5f * base.PositionComp.LocalAABB.Size.X * (float)neighbourOffsetI.X * base.PositionComp.WorldMatrix.Right + 0.5f * base.PositionComp.LocalAABB.Size.Y * (float)neighbourOffsetI.Y * base.PositionComp.WorldMatrix.Up - 0.5f * base.PositionComp.LocalAABB.Size.Z * (float)neighbourOffsetI.Z * base.PositionComp.WorldMatrix.Forward;
			value += 0.9f * (float)neighbourOffsetI.X * base.PositionComp.WorldMatrix.Right + 0.9f * (float)neighbourOffsetI.Y * base.PositionComp.WorldMatrix.Up - 0.9f * (float)neighbourOffsetI.Z * base.PositionComp.WorldMatrix.Forward;
			MatrixD worldMatrix = MatrixD.CreateWorld(base.PositionComp.WorldMatrix.Translation + value, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up);
			translation = worldMatrix.Translation;
			return m_pilot.CanPlaceCharacter(ref worldMatrix, useCharacterCenter: true, checkCharacters: true);
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			if (!m_defferAttach && m_savedPilot != null)
			{
				AttachPilot(m_savedPilot, storeOriginalPilotWorld: false, calledFromInit: false, merged: true);
				m_savedPilot = null;
			}
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			if (m_pilot != null)
			{
				MyCharacter pilot = m_pilot;
				if (!MyEntities.CloseAllowed)
				{
					RemovePilot();
					pilot.DoDamage(1000f, MyDamageType.Destruction, updateSync: false, 0L);
				}
				else if (MySession.Static.CameraController == this)
				{
					MySession.Static.SetCameraController(MySession.Static.GetCameraControllerEnum(), m_pilot);
				}
			}
		}

		public void AttachPilot(MyCharacter pilot, bool storeOriginalPilotWorld = true, bool calledFromInit = false, bool merged = false)
		{
			if (!MyEntities.IsInsideWorld(pilot.PositionComp.GetPosition()) || !MyEntities.IsInsideWorld(base.PositionComp.GetPosition()))
			{
				return;
			}
			MyAnalyticsHelper.ReportActivityStart(pilot, "cockpit", "cockpit", string.Empty, string.Empty);
			long playerIdentityId = pilot.GetPlayerIdentityId();
			m_pilot = pilot;
			m_pilot.OnMarkForClose += m_pilotClosedHandler;
			m_pilot.IsUsing = this;
			m_pilot.ResetHeadRotation();
			bool num = !merged;
			if (num)
			{
				if (storeOriginalPilotWorld)
				{
					m_pilotRelativeWorld = MatrixD.Multiply(pilot.WorldMatrix, base.PositionComp.WorldMatrixNormalizedInv);
				}
				else if (!calledFromInit)
				{
					m_pilotRelativeWorld = null;
				}
			}
			if (pilot.InScene)
			{
				MyEntities.Remove(pilot);
			}
			m_pilot.Physics.Enabled = false;
			m_pilot.PositionComp.SetWorldMatrix(base.WorldMatrix, this, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: false, forceUpdateAllChildren: false, ignoreAssert: true);
			m_pilot.Physics.Clear();
			if (!base.Hierarchy.Children.Any((MyHierarchyComponentBase x) => x.Entity == m_pilot))
			{
				base.Hierarchy.AddChild(m_pilot, preserveWorldPos: true);
			}
			base.NeedsWorldMatrix = true;
			if (num)
			{
				if (m_pilot.CurrentWeapon is MyEntity && !m_forgetTheseWeapons.Contains(m_pilot.CurrentWeapon.DefinitionId))
				{
					m_pilotGunDefinition = m_pilot.CurrentWeapon.DefinitionId;
				}
				else
				{
					m_pilotGunDefinition = null;
				}
				m_pilotFirstPerson = pilot.IsInFirstPersonView;
			}
			PlacePilotInSeat(pilot);
			m_pilot.SuitBattery.ResourceSink.TemporaryConnectedEntity = this;
			m_rechargeSocket.PlugIn(m_pilot.SuitBattery.ResourceSink);
			if (pilot.ControllerInfo.Controller != null)
			{
				Sync.Players.SetPlayerToCockpit(pilot.ControllerInfo.Controller.Player, this);
			}
			if (!calledFromInit)
			{
				GiveControlToPilot();
				m_pilot.SwitchToWeapon(null, null, sync: false);
			}
			if (Sync.IsServer)
			{
				m_serverSidePilotId = m_pilot.EntityId;
				MyMultiplayer.RaiseEvent(this, (MyCockpit x) => NotifyClientPilotChanged, m_serverSidePilotId, m_pilotRelativeWorld);
			}
			if (num)
			{
				MyCharacterJetpackComponent jetpackComp = m_pilot.JetpackComp;
				if (jetpackComp != null && !calledFromInit)
				{
					m_pilotJetpackEnabledBackup = jetpackComp.TurnedOn;
				}
			}
			if (m_pilot.JetpackComp != null)
			{
				m_pilot.JetpackComp.TurnOnJetpack(newState: false);
			}
			m_lastPilot = pilot;
			if (GetInCockpitSound != MySoundPair.Empty && !calledFromInit && !merged)
			{
				PlayUseSound(getIn: true);
			}
			m_playIdleSound = true;
			if (pilot == MySession.Static.LocalCharacter && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
			{
				if (calledFromInit && (MySession.Static.CameraController == null || MySession.Static.CameraController == this))
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
				}
				else if (MySession.Static.CameraController == pilot && pilot == MySession.Static.LocalCharacter)
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
				}
			}
			string text = Name;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = base.EntityId.ToString();
			}
			string text2 = base.CubeGrid.Name;
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = base.CubeGrid.EntityId.ToString();
			}
			if (MyVisualScriptLogicProvider.PlayerEnteredCockpit != null && playerIdentityId != -1)
			{
				MyVisualScriptLogicProvider.PlayerEnteredCockpit(text, playerIdentityId, text2);
			}
			if (m_pilot == MySession.Static.LocalCharacter)
			{
				MyLocalCache.LoadInventoryConfig(pilot, setModel: false);
			}
			base.CubeGrid.GridSystems.RadioSystem.Register(m_pilot.RadioBroadcaster);
			base.CubeGrid.GridSystems.RadioSystem.Register(m_pilot.RadioReceiver);
			MyIdentity identity = pilot.GetIdentity();
			if (identity != null)
			{
				identity.FactionChanged += OnCharacterFactionChanged;
			}
			if (m_pilot == MySession.Static.LocalCharacter)
			{
				MyCockpit.OnPilotAttached.InvokeIfNotNull();
			}
		}

		protected virtual void PlacePilotInSeat(MyCharacter pilot)
		{
			bool playerIsPilot = MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Identity.Character == pilot;
			m_pilot.Sit(m_enableFirstPerson, playerIsPilot, enableBag: false, BlockDefinition.CharacterAnimation);
			pilot.PositionComp.SetWorldMatrix(m_characterDummy * base.WorldMatrix, this);
			base.CubeGrid.RegisterOccupiedBlock(this);
			base.CubeGrid.SetInventoryMassDirty();
		}

		public void AddShake(float shakePower)
		{
			m_currentCameraShakePower += shakePower;
			m_currentCameraShakePower = Math.Min(m_currentCameraShakePower, MySector.MainCamera.CameraShake.MaxShake);
		}

		private void ChangeGasFillLevel(float newFillLevel)
		{
			if (Sync.IsServer && OxygenFillLevel != newFillLevel)
			{
				OxygenFillLevel = newFillLevel;
				CheckEmissiveState();
			}
		}

		public override bool IsLargeShip()
		{
			return m_isLargeCockpit;
		}

		public override string CalculateCurrentModel(out Matrix orientation)
		{
			base.Orientation.GetMatrix(out orientation);
			if (base.Render.NearFlag)
			{
				if (!string.IsNullOrEmpty(m_cockpitInteriorModel))
				{
					return m_cockpitInteriorModel;
				}
				return BlockDefinition.Model;
			}
			return BlockDefinition.Model;
		}

		private void TryGetDummies()
		{
			if (base.Model != null)
			{
				base.Model.Dummies.TryGetValue("camera", out MyModelDummy value);
				if (value != null)
				{
					m_cameraDummy = MatrixD.Normalize(value.Matrix);
				}
				base.Model.Dummies.TryGetValue("character", out MyModelDummy value2);
				if (value2 != null)
				{
					m_characterDummy = MatrixD.Normalize(value2.Matrix);
				}
			}
		}

		public virtual UseActionResult CanUse(UseActionEnum actionEnum, IMyControllableEntity user)
		{
			if (user == null)
			{
				return UseActionResult.AccessDenied;
			}
			MyCharacter myCharacter;
			if ((myCharacter = (user as MyCharacter)) != null && myCharacter.IsDead)
			{
				return UseActionResult.AccessDenied;
			}
			if (m_pilot != null)
			{
				MyPlayer.PlayerId? savedPlayer = m_pilot.SavedPlayer;
				if (!savedPlayer.HasValue || MySession.Static.Players.TryGetPlayerById(savedPlayer.Value, out MyPlayer _))
				{
					return UseActionResult.UsedBySomeoneElse;
				}
			}
			if (base.MarkedForClose)
			{
				return UseActionResult.Closed;
			}
			if (!base.IsFunctional)
			{
				return UseActionResult.CockpitDamaged;
			}
			long controllingIdentityId = user.ControllerInfo.ControllingIdentityId;
			if (controllingIdentityId != 0L)
			{
				switch (HasPlayerAccessReason(controllingIdentityId))
				{
				case AccessRightsResult.MissingDLC:
					return UseActionResult.MissingDLC;
				case AccessRightsResult.Enemies:
				case AccessRightsResult.Other:
					return UseActionResult.AccessDenied;
				default:
					return UseActionResult.OK;
				}
			}
			return UseActionResult.AccessDenied;
		}

		protected override void UpdateSoundState()
		{
			base.UpdateSoundState();
		}

		protected override void StartLoopSound()
		{
			m_playIdleSound = true;
			if (m_soundEmitter != null && hasPower && !m_baseIdleSound.SoundId.IsNull)
			{
				m_soundEmitter.PlaySound(m_baseIdleSound, stopPrevious: true);
			}
		}

		protected override void StopLoopSound()
		{
			m_playIdleSound = false;
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		protected override bool IsCameraController()
		{
			return true;
		}

		protected override void OnControlAcquired_UpdateCamera()
		{
			base.CubeGrid.RaiseGridChanged();
			base.OnControlAcquired_UpdateCamera();
			m_currentCameraShakePower = 0f;
		}

		protected override void OnControlledEntity_Used()
		{
			_ = m_pilot;
			RemovePilot();
			base.OnControlledEntity_Used();
		}

		protected override void OnControlReleased_UpdateCamera()
		{
			base.OnControlReleased_UpdateCamera();
			m_currentCameraShakePower = 0f;
		}

		protected override void RemoveLocal()
		{
			if (MyCubeBuilder.Static.IsActivated)
			{
				MySession.Static.GameFocusManager.Clear();
			}
			base.RemoveLocal();
			RemovePilot();
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			CheckPilotRelation();
		}

		private void CheckPilotRelation()
		{
			if (m_pilot != null && Sync.IsServer && base.ControllerInfo.Controller != null)
			{
				MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(base.ControllerInfo.ControllingIdentityId);
				if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies || userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Neutral)
				{
					RaiseControlledEntityUsed();
				}
			}
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			List<MyHudEntityParams> hudParams = base.GetHudParams(allowBlink);
			long num = (MySession.Static.LocalHumanPlayer == null) ? 0 : MySession.Static.LocalHumanPlayer.Identity.IdentityId;
			bool num2 = base.ControllerInfo.ControllingIdentityId != num && Pilot != null;
			if (base.ShowOnHUD || base.IsBeingHacked)
			{
				hudParams[0].Text.AppendLine();
			}
			else
			{
				hudParams[0].Text.Clear();
			}
			if (num2 && Pilot != null)
			{
				hudParams[0].Text.Append((object)Pilot.UpdateCustomNameWithFaction());
			}
			if (!base.ShowOnHUD)
			{
				m_hudParams.Clear();
			}
			return hudParams;
		}

		protected override bool ShouldSit()
		{
			if (!m_isLargeCockpit)
			{
				return base.ShouldSit();
			}
			return true;
		}

		protected override bool CanBeMainCockpit()
		{
			return BlockDefinition.EnableShipControl;
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			if (!m_enableFirstPerson)
			{
				IsInFirstPersonView = false;
			}
			if (base.Closed && MySession.Static.LocalCharacter != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.LocalCharacter);
			}
			currentCamera.SetViewMatrix(GetViewMatrix());
			currentCamera.CameraSpring.Enabled = true;
			currentCamera.CameraSpring.SetCurrentCameraControllerVelocity((base.CubeGrid.Physics != null) ? base.CubeGrid.Physics.LinearVelocity : Vector3.Zero);
			if (m_currentCameraShakePower > 0f)
			{
				currentCamera.CameraShake.AddShake(m_currentCameraShakePower);
				m_currentCameraShakePower = 0f;
			}
			if (Pilot != null && Pilot.InScene && Pilot == MySession.Static.LocalCharacter)
			{
				Pilot.EnableHead(!IsInFirstPersonView && !ForceFirstPersonCamera);
			}
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			Rotate(rotationIndicator, rollIndicator);
		}

		void IMyCameraController.RotateStopped()
		{
			RotateStopped();
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			OnAssumeControl(previousCameraController);
			m_currentCameraShakePower = 0f;
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			OnReleaseControl(newCameraController);
			if (Pilot != null && Pilot.InScene)
			{
				Pilot.EnableHead(enabled: true);
			}
		}

		bool IMyCameraController.HandleUse()
		{
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		bool IMyGasBlock.IsWorking()
		{
			if (base.IsWorking)
			{
				return BlockDefinition.IsPressurized;
			}
			return false;
		}

		public void RequestUse(UseActionEnum actionEnum, MyCharacter user)
		{
			if (!user.IsDead)
			{
				UseActionResult useActionResult = UseActionResult.OK;
				if ((useActionResult = CanUse(actionEnum, user)) == UseActionResult.OK)
				{
					MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.AttachPilotEvent, actionEnum, user.EntityId);
				}
				else
				{
					AttachPilotEventFailed(useActionResult);
				}
			}
		}

		[Event(null, 2198)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public void AttachPilotEvent(UseActionEnum actionEnum, long characterID)
		{
			MyEntity entity;
			bool num = MyEntities.TryGetEntityById<MyEntity>(characterID, out entity, allowClosed: false);
			IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
			MyCharacter pilot = myControllableEntity as MyCharacter;
			if (num && this != null && ((IMyUsableEntity)this).CanUse(actionEnum, myControllableEntity) == UseActionResult.OK)
			{
				if (m_pilot != null)
				{
					RemovePilot();
				}
				AttachPilot(pilot);
			}
		}

		public void AttachPilotEventFailed(UseActionResult actionResult)
		{
			switch (actionResult)
			{
			case UseActionResult.UsedBySomeoneElse:
				MyHud.Notifications.Add(new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, 2500, "Red"));
				break;
			case UseActionResult.MissingDLC:
				MySession.Static.CheckDLCAndNotify(BlockDefinition);
				break;
			case UseActionResult.AccessDenied:
				MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				break;
			case UseActionResult.Unpowered:
				MyHud.Notifications.Add(new MyHudNotification(MySpaceTexts.BlockIsNotPowered, 2500, "Red"));
				break;
			case UseActionResult.CockpitDamaged:
			{
				MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.Notification_ControllableBlockIsDamaged, 2500, "Red");
				object[] textFormatArguments = new string[1]
				{
					base.DefinitionDisplayNameText
				};
				myHudNotification.SetTextFormatArguments(textFormatArguments);
				MyHud.Notifications.Add(myHudNotification);
				break;
			}
			}
		}

		private void TryAttachPilot(long pilotId)
		{
			m_retryAttachPilot = false;
			if ((m_pilot != null || (m_savedPilot != null && m_savedPilot.EntityId == pilotId)) && (m_pilot == null || m_pilot.EntityId == pilotId))
			{
				return;
			}
			m_savedPilot = null;
			RemovePilot();
			if (MyEntities.TryGetEntityById<MyEntity>(pilotId, out MyEntity entity, allowClosed: false))
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null && MyEntities.IsInsideWorld(myCharacter.PositionComp.GetPosition()))
				{
					AttachPilot(myCharacter, m_pilotRelativeWorld.HasValue);
				}
			}
			else
			{
				m_retryAttachPilot = true;
			}
		}

		public void ClearSavedpilot()
		{
			m_serverSidePilotId = 0L;
			m_savedPilot = null;
		}

		[Event(null, 2268)]
		[Reliable]
		[Broadcast]
		private void SetAutopilot_Client([Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_AutopilotBase autopilot)
		{
			if (autopilot == null)
			{
				RemoveAutopilot(updateSync: false);
			}
			else
			{
				AttachAutopilot(MyAutopilotFactory.CreateAutopilot(autopilot), updateSync: false);
			}
		}

		[Event(null, 2281)]
		[Reliable]
		[Broadcast]
		private void NotifyClientPilotChanged(long pilotEntityId, Matrix? pilotRelativeWorld)
		{
			m_serverSidePilotId = pilotEntityId;
			m_pilotRelativeWorld = pilotRelativeWorld;
			if (pilotEntityId != 0L)
			{
				TryAttachPilot(pilotEntityId);
			}
			else if (m_pilot != null)
			{
				RemovePilot();
			}
		}

		private void OnCharacterFactionChanged(MyFaction oldFaction, MyFaction newFaction)
		{
			CheckPilotRelation();
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		void Sandbox.ModAPI.IMyCockpit.AttachPilot(IMyCharacter pilot)
		{
			if (!pilot.IsDead)
			{
				UseActionEnum useActionEnum = UseActionEnum.Manipulate;
				IMyControllableEntity user = pilot as IMyControllableEntity;
				if (CanUse(useActionEnum, user) == UseActionResult.OK)
				{
					MyMultiplayer.RaiseEvent(this, (MyCockpit x) => x.AttachPilotEvent, useActionEnum, pilot.EntityId);
				}
			}
		}

		void Sandbox.ModAPI.IMyCockpit.RemovePilot()
		{
			RemoveLocal();
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
