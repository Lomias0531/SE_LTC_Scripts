using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MedicalRoom))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyMedicalRoom),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyMedicalRoom)
	})]
	public class MyMedicalRoom : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyMedicalRoom, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyMedicalRoom, IMyLifeSupportingBlock, IMyRechargeSocketOwner, IMyGasBlock, IMyConveyorEndpointBlock, IMySpawnBlock
	{
		protected sealed class SetSpawnTextEvent_003C_003ESystem_String : ICallSite<MyMedicalRoom, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMedicalRoom @this, in string text, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetSpawnTextEvent(text);
			}
		}

		protected sealed class UseWardrobeSync_003C_003ESystem_Int64 : ICallSite<MyMedicalRoom, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMedicalRoom @this, in long userId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseWardrobeSync(userId);
			}
		}

		protected sealed class StopUsingWardrobeSync_003C_003E : ICallSite<MyMedicalRoom, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMedicalRoom @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.StopUsingWardrobeSync();
			}
		}

		protected sealed class RequestSupport_003C_003ESystem_Int64 : ICallSite<MyMedicalRoom, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyMedicalRoom @this, in long userId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestSupport(userId);
			}
		}

		private static readonly string[] m_emissiveTextureNames = new string[2]
		{
			"Emissive2",
			"Emissive3"
		};

		private bool m_healingAllowed;

		private bool m_refuelAllowed;

		private bool m_suitChangeAllowed;

		private bool m_customWardrobesEnabled;

		private bool m_forceSuitChangeOnRespawn;

		private bool m_spawnWithoutOxygenEnabled;

		private HashSet<string> m_customWardrobeNames = new HashSet<string>();

		private string m_respawnSuitName;

		private MySoundPair m_idleSound;

		private MySoundPair m_progressSound;

		private MyCharacter m_wardrobeUser;

		private MatrixD m_wardrobeUserSpectatorMatrix;

		private byte m_wardrobeUserAwayCounter;

		private MyLight m_light;

		private readonly MyEntity3DSoundEmitter m_idleSoundEmitter;

		private MyMedicalRoomDefinition m_medicalRoomDefinition;

		private MyLifeSupportingComponent m_lifeSupportingComponent;

		protected bool m_takeSpawneeOwnership;

		protected bool m_setFactionToSpawnee;

		private MyResourceSinkComponent m_sinkComponent;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private long m_wardrobeUserId;

		private List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();

		public bool SetFactionToSpawnee => m_setFactionToSpawnee;

		public MyResourceSinkComponent SinkComp
		{
			get
			{
				return m_sinkComponent;
			}
			set
			{
				if (base.Components.Contains(typeof(MyResourceSinkComponent)))
				{
					base.Components.Remove<MyResourceSinkComponent>();
				}
				base.Components.Add(value);
				m_sinkComponent = value;
			}
		}

		private ulong SteamUserId
		{
			get;
			set;
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool CanPressurizeRoom => false;

		/// <summary>
		/// Disabling prevents healing characters.
		/// </summary>
		public bool HealingAllowed
		{
			get
			{
				return m_healingAllowed;
			}
			set
			{
				m_healingAllowed = value;
			}
		}

		/// <summary>
		/// Disabling prevents refueling suits.
		/// </summary>
		public bool RefuelAllowed
		{
			get
			{
				return m_refuelAllowed;
			}
			set
			{
				m_refuelAllowed = value;
			}
		}

		/// <summary>
		/// Disable to remove respawn component from medical room.
		/// </summary>
		public bool RespawnAllowed
		{
			get
			{
				return base.Components.Get<MyEntityRespawnComponentBase>() != null;
			}
			set
			{
				if (value)
				{
					if (base.Components.Get<MyEntityRespawnComponentBase>() == null)
					{
						base.Components.Add((MyEntityRespawnComponentBase)new MyRespawnComponent());
					}
				}
				else
				{
					base.Components.Remove<MyEntityRespawnComponentBase>();
				}
			}
		}

		/// <summary>
		/// The text displayed in the spawn menu
		/// </summary>
		public StringBuilder SpawnName
		{
			get;
			private set;
		}

		string IMySpawnBlock.SpawnName => SpawnName.ToString();

		/// <summary>
		/// Disable to prevent players from changing their suits.
		/// </summary>
		public bool SuitChangeAllowed
		{
			get
			{
				if (m_suitChangeAllowed)
				{
					return m_wardrobeUser == null;
				}
				return false;
			}
			set
			{
				m_suitChangeAllowed = value;
			}
		}

		/// <summary>
		/// If set to true CustomWardrobeNames are used instead of all definitions when instantiating WardrobeScreen.
		/// </summary>
		public bool CustomWardrobesEnabled
		{
			get
			{
				return m_customWardrobesEnabled;
			}
			set
			{
				m_customWardrobesEnabled = value;
			}
		}

		/// <summary>
		/// Used when CustomWardrobes are enabled.
		/// </summary>
		public HashSet<string> CustomWardrobeNames
		{
			get
			{
				return m_customWardrobeNames;
			}
			set
			{
				m_customWardrobeNames = value;
			}
		}

		/// <summary>
		/// Use when you want to force suit change on respawn. Wont turn to true if RespawnSuitName is null.
		/// </summary>
		public bool ForceSuitChangeOnRespawn
		{
			get
			{
				return m_forceSuitChangeOnRespawn;
			}
			set
			{
				if (value && m_respawnSuitName != null)
				{
					m_forceSuitChangeOnRespawn = value;
				}
			}
		}

		/// <summary>
		/// Name of suit into which would player be forced upon respawn.
		/// </summary>
		public string RespawnSuitName
		{
			get
			{
				return m_respawnSuitName;
			}
			set
			{
				m_respawnSuitName = value;
			}
		}

		/// <summary>
		/// Players wont be able to spawn in rooms that are not pressurised.
		/// </summary>
		public bool SpawnWithoutOxygenEnabled
		{
			get
			{
				return m_spawnWithoutOxygenEnabled;
			}
			set
			{
				m_spawnWithoutOxygenEnabled = value;
			}
		}

		public bool IsGridPreview
		{
			get
			{
				if (base.CubeGrid != null)
				{
					if (!base.CubeGrid.IsPreview)
					{
						return base.CubeGrid.Projector != null;
					}
					return true;
				}
				return false;
			}
		}

		MyLifeSupportingBlockType IMyLifeSupportingBlock.BlockType => MyLifeSupportingBlockType.MedicalRoom;

		MyRechargeSocket IMyRechargeSocketOwner.RechargeSocket => m_lifeSupportingComponent.RechargeSocket;

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		protected override bool CheckIsWorking()
		{
			if (SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private void SetSpawnName(StringBuilder text)
		{
			if (SpawnName.CompareUpdate(text))
			{
				MyMultiplayer.RaiseEvent(this, (MyMedicalRoom x) => x.SetSpawnTextEvent, text.ToString());
			}
		}

		[Event(null, 170)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		protected void SetSpawnTextEvent(string text)
		{
			SpawnName.CompareUpdate(text);
		}

		public MyMedicalRoom()
		{
			CreateTerminalControls();
			SpawnName = new StringBuilder();
			m_idleSoundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyMedicalRoom>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyMedicalRoom>("SpawnName", MySpaceTexts.MedicalRoom_SpawnNameLabel, MySpaceTexts.MedicalRoom_SpawnNameToolTip)
				{
					Getter = ((MyMedicalRoom x) => x.SpawnName),
					Setter = delegate(MyMedicalRoom x, StringBuilder v)
					{
						x.SetSpawnName(v);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlLabel<MyMedicalRoom> control = new MyTerminalControlLabel<MyMedicalRoom>(MySpaceTexts.TerminalScenarioSettingsLabel);
				MyTerminalControlCheckbox<MyMedicalRoom> control2 = new MyTerminalControlCheckbox<MyMedicalRoom>("TakeOwnership", MySpaceTexts.MedicalRoom_ownershipAssignmentLabel, MySpaceTexts.MedicalRoom_ownershipAssignmentTooltip)
				{
					Getter = ((MyMedicalRoom x) => x.m_takeSpawneeOwnership),
					Setter = delegate(MyMedicalRoom x, bool val)
					{
						x.m_takeSpawneeOwnership = val;
					},
					Enabled = ((MyMedicalRoom x) => MySession.Static.Settings.ScenarioEditMode)
				};
				MyTerminalControlFactory.AddControl(control);
				MyTerminalControlFactory.AddControl(control2);
				MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MyMedicalRoom>("SetFaction", MySpaceTexts.MedicalRoom_factionAssignmentLabel, MySpaceTexts.MedicalRoom_factionAssignmentTooltip)
				{
					Getter = ((MyMedicalRoom x) => x.m_setFactionToSpawnee),
					Setter = delegate(MyMedicalRoom x, bool val)
					{
						x.m_setFactionToSpawnee = val;
					},
					Enabled = ((MyMedicalRoom x) => MySession.Static.Settings.ScenarioEditMode)
				});
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			m_medicalRoomDefinition = (base.BlockDefinition as MyMedicalRoomDefinition);
			MyStringHash orCompute;
			if (m_medicalRoomDefinition != null)
			{
				m_idleSound = new MySoundPair(m_medicalRoomDefinition.IdleSound);
				m_progressSound = new MySoundPair(m_medicalRoomDefinition.ProgressSound);
				orCompute = MyStringHash.GetOrCompute(m_medicalRoomDefinition.ResourceSinkGroup);
			}
			else
			{
				m_idleSound = new MySoundPair("BlockMedical");
				m_progressSound = new MySoundPair("BlockMedicalProgress");
				orCompute = MyStringHash.GetOrCompute("Utility");
			}
			SinkComp = new MyResourceSinkComponent();
			SinkComp.Init(orCompute, 0.002f, () => (!base.Enabled || !base.IsFunctional) ? 0f : SinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			SinkComp.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.Init(objectBuilder, cubeGrid);
			m_lifeSupportingComponent = new MyLifeSupportingComponent(this, m_progressSound, "MedRoomHeal", 5f);
			base.Components.Add(m_lifeSupportingComponent);
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			MyObjectBuilder_MedicalRoom myObjectBuilder_MedicalRoom = objectBuilder as MyObjectBuilder_MedicalRoom;
			SpawnName.Clear();
			if (myObjectBuilder_MedicalRoom.SpawnName != null)
			{
				SpawnName.Append(myObjectBuilder_MedicalRoom.SpawnName);
			}
			SteamUserId = myObjectBuilder_MedicalRoom.SteamUserId;
			if (SteamUserId != 0L)
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(SteamUserId));
				if (playerById != null)
				{
					base.IDModule.Owner = playerById.Identity.IdentityId;
					base.IDModule.ShareMode = MyOwnershipShareModeEnum.Faction;
				}
			}
			SteamUserId = 0uL;
			m_takeSpawneeOwnership = myObjectBuilder_MedicalRoom.TakeOwnership;
			m_setFactionToSpawnee = myObjectBuilder_MedicalRoom.SetFaction;
			m_wardrobeUserId = myObjectBuilder_MedicalRoom.WardrobeUserId;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			InitializeConveyorEndpoint();
			SinkComp.Update();
			base.Components.Remove<MyRespawnComponent>();
			if (base.CubeGrid.CreatePhysics)
			{
				base.Components.Add((MyEntityRespawnComponentBase)new MyRespawnComponent());
			}
			m_healingAllowed = m_medicalRoomDefinition.HealingAllowed;
			m_refuelAllowed = m_medicalRoomDefinition.RefuelAllowed;
			m_suitChangeAllowed = m_medicalRoomDefinition.SuitChangeAllowed;
			m_customWardrobesEnabled = m_medicalRoomDefinition.CustomWardrobesEnabled;
			m_forceSuitChangeOnRespawn = m_medicalRoomDefinition.ForceSuitChangeOnRespawn;
			m_customWardrobeNames = m_medicalRoomDefinition.CustomWardrobeNames;
			m_respawnSuitName = m_medicalRoomDefinition.RespawnSuitName;
			m_spawnWithoutOxygenEnabled = m_medicalRoomDefinition.SpawnWithoutOxygenEnabled;
			RespawnAllowed = m_medicalRoomDefinition.RespawnAllowed;
			m_light = MyLights.AddLight();
			if (m_light != null)
			{
				m_light.Start(Color.White, 2f, "Med bay light");
				m_light.Falloff = 1.3f;
				m_light.LightOn = false;
				m_light.UpdateLight();
			}
			if (m_medicalRoomDefinition.ScreenAreas != null && m_medicalRoomDefinition.ScreenAreas.Count > 0)
			{
				m_panels = new List<MyTextPanelComponent>();
				for (int i = 0; i < m_medicalRoomDefinition.ScreenAreas.Count; i++)
				{
					MyTextPanelComponent myTextPanelComponent = new MyTextPanelComponent(i, this, m_medicalRoomDefinition.ScreenAreas[i].Name, m_medicalRoomDefinition.ScreenAreas[i].DisplayName, m_medicalRoomDefinition.ScreenAreas[i].TextureResolution);
					m_panels.Add(myTextPanelComponent);
					base.SyncType.Append(myTextPanelComponent);
					myTextPanelComponent.Init();
				}
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_panels.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.SetRender((MyRenderComponentScreenAreas)base.Render);
				((MyRenderComponentScreenAreas)base.Render).AddScreenArea(base.Render.RenderObjectIDs, panel.Name);
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			StopIdleSound();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateEmissivity();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStartWorking()
		{
			StartIdleSound();
			UpdateEmissivity();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			SinkComp.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateVisual();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_MedicalRoom myObjectBuilder_MedicalRoom = (MyObjectBuilder_MedicalRoom)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_MedicalRoom.SpawnName = SpawnName.ToString();
			myObjectBuilder_MedicalRoom.SteamUserId = SteamUserId;
			myObjectBuilder_MedicalRoom.IdleSound = m_idleSound.ToString();
			myObjectBuilder_MedicalRoom.ProgressSound = m_progressSound.ToString();
			myObjectBuilder_MedicalRoom.TakeOwnership = m_takeSpawneeOwnership;
			myObjectBuilder_MedicalRoom.SetFaction = m_setFactionToSpawnee;
			if (m_wardrobeUser != null)
			{
				myObjectBuilder_MedicalRoom.WardrobeUserId = m_wardrobeUser.EntityId;
			}
			return myObjectBuilder_MedicalRoom;
		}

		public override void UpdateSoundEmitters()
		{
			base.UpdateSoundEmitters();
			if (m_idleSoundEmitter != null)
			{
				m_idleSoundEmitter.Update();
			}
			m_lifeSupportingComponent.UpdateSoundEmitters();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_wardrobeUser != null && m_wardrobeUser == MySession.Static.LocalCharacter)
			{
				SetSpectatorCamera();
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (m_wardrobeUser != null)
			{
				if (m_wardrobeUser == MySession.Static.LocalCharacter)
				{
					double num = Math.Abs(Vector3D.Distance(m_wardrobeUser.PositionComp.GetPosition(), base.PositionComp.GetPosition()) - (double)m_medicalRoomDefinition.WardrobeCharacterOffsetLength);
					if (m_wardrobeUser.IsDead || num > 0.5)
					{
						m_wardrobeUserAwayCounter++;
						if (m_wardrobeUserAwayCounter > 6)
						{
							StopUsingWardrobe();
						}
					}
					else
					{
						m_wardrobeUserAwayCounter = 0;
					}
					if (!base.IsFunctional || !base.IsWorking)
					{
						StopUsingWardrobe();
						m_wardrobeUser = null;
					}
				}
				else if (Sync.IsServer && m_wardrobeUser.ControllerInfo.Controller == null)
				{
					MyMultiplayer.RaiseEvent(this, (MyMedicalRoom x) => x.StopUsingWardrobeSync);
				}
			}
			else if (m_wardrobeUserId != 0L)
			{
				MyEntities.TryGetEntityById(m_wardrobeUserId, out MyCharacter entity);
				if (entity != null)
				{
					m_wardrobeUser = entity;
					m_wardrobeUserId = 0L;
				}
			}
			m_lifeSupportingComponent.Update10();
		}

		public void UseWardrobe(MyCharacter user)
		{
			m_wardrobeUserSpectatorMatrix = MySpectatorCameraController.Static.GetViewMatrix();
			user.UpdateRotationsOverride = true;
			MyMultiplayer.RaiseEvent(this, (MyMedicalRoom x) => x.UseWardrobeSync, user.EntityId);
		}

		private void SetSpectatorCamera()
		{
			MatrixD worldMatrix = base.WorldMatrix;
			float num = 70f / MySector.MainCamera.FieldOfViewDegrees;
			worldMatrix.Translation += base.WorldMatrix.Right * (-1.1499999761581421 + m_medicalRoomDefinition.WardrobeCharacterOffset.X) + base.WorldMatrix.Up * (0.699999988079071 + m_medicalRoomDefinition.WardrobeCharacterOffset.Y) + base.WorldMatrix.Forward * (1.5 + m_medicalRoomDefinition.WardrobeCharacterOffset.Z) * num;
			worldMatrix.Left = base.WorldMatrix.Right;
			worldMatrix.Forward = base.WorldMatrix.Backward;
			if (m_light != null)
			{
				Vector3D position = base.WorldMatrix.Translation + base.WorldMatrix.Right * (-0.5 + m_medicalRoomDefinition.WardrobeCharacterOffset.X) + base.WorldMatrix.Up * (1.0 + m_medicalRoomDefinition.WardrobeCharacterOffset.Y) + base.WorldMatrix.Forward * (0.60000002384185791 + m_medicalRoomDefinition.WardrobeCharacterOffset.Z) * num;
				m_light.Position = position;
				m_light.UpdateLight();
			}
			MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, worldMatrix.Translation);
			MySpectatorCameraController.Static.SetTarget(worldMatrix.Translation + worldMatrix.Forward, worldMatrix.Up);
			if (!MySpectatorCameraController.Static.IsLightOn)
			{
				MySpectatorCameraController.Static.SwitchLight();
			}
		}

		[Event(null, 526)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void UseWardrobeSync(long userId)
		{
			MyEntities.TryGetEntityById(userId, out MyCharacter entity);
			if (entity == null)
			{
				return;
			}
			m_wardrobeUser = entity;
			MatrixD worldMatrix = base.WorldMatrix;
			if (base.Model.Dummies.ContainsKey("detector_wardrobe"))
			{
				worldMatrix = MatrixD.Multiply(MatrixD.Normalize(base.Model.Dummies["detector_wardrobe"].Matrix), base.WorldMatrix);
				worldMatrix.Translation -= base.WorldMatrix.Up * 0.98;
			}
			else
			{
				worldMatrix.Translation += base.WorldMatrix.Right * m_medicalRoomDefinition.WardrobeCharacterOffset.X + base.WorldMatrix.Up * m_medicalRoomDefinition.WardrobeCharacterOffset.Y + base.WorldMatrix.Forward * m_medicalRoomDefinition.WardrobeCharacterOffset.Z;
			}
			if (Sync.IsServer)
			{
				entity.PositionComp.SetWorldMatrix(worldMatrix);
			}
			if (entity == MySession.Static.LocalCharacter)
			{
				if (entity.JetpackRunning)
				{
					entity.SwitchJetpack();
				}
				entity.ForceDisablePrediction = true;
				entity.UpdateCharacterPhysics();
				entity.PositionComp.SetPosition(worldMatrix.Translation);
				entity.PositionComp.SetWorldMatrix(worldMatrix);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			if (m_light != null)
			{
				m_light.LightOn = true;
				m_light.UpdateLight();
			}
			UpdateEmissivity();
		}

		private void SetEmissive(Color color, int index, float emissivity = 1f)
		{
			if (base.Render.RenderObjectIDs[0] != uint.MaxValue && m_emissiveTextureNames.Length > index)
			{
				MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[index], color, emissivity);
			}
		}

		private void UpdateEmissivity()
		{
			if (base.IsFunctional)
			{
				if (base.IsWorking)
				{
					SetEmissive(Color.Green, 0);
					if (m_wardrobeUser != null)
					{
						SetEmissive(Color.Cyan, 0);
						SetEmissive(Color.White, 1);
					}
					else
					{
						SetEmissive(Color.Green, 0);
						SetEmissive(Color.White, 1, 0f);
					}
				}
				else
				{
					SetEmissive(Color.Red, 0);
					SetEmissive(Color.Red, 1);
				}
			}
			else
			{
				SetEmissive(Color.Black, 0, 0f);
				SetEmissive(Color.Black, 1, 0f);
			}
		}

		public void StopUsingWardrobe()
		{
			if (m_wardrobeUser != null)
			{
				m_wardrobeUser.UpdateRotationsOverride = true;
				m_wardrobeUserAwayCounter = 0;
				if (MyGuiScreenGamePlay.ActiveGameplayScreen is MyGuiScreenLoadInventory)
				{
					MyGuiScreenGamePlay.ActiveGameplayScreen.CloseScreen();
				}
				MySpectatorCameraController.Static.SetViewMatrix(m_wardrobeUserSpectatorMatrix);
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, m_wardrobeUser);
				MyMultiplayer.RaiseEvent(this, (MyMedicalRoom x) => x.StopUsingWardrobeSync);
				if (!base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		[Event(null, 634)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void StopUsingWardrobeSync()
		{
			if (m_wardrobeUser != null)
			{
				MyRespawnComponent myRespawnComponent = base.Components.Get<MyEntityRespawnComponentBase>() as MyRespawnComponent;
				MatrixD worldMatrix;
				if (myRespawnComponent != null)
				{
					worldMatrix = myRespawnComponent.GetSpawnPosition();
				}
				else
				{
					worldMatrix = base.WorldMatrix;
					worldMatrix.Translation += worldMatrix.Forward * base.BlockDefinition.Size.AbsMax();
				}
				if (Sync.IsServer)
				{
					m_wardrobeUser.PositionComp.SetWorldMatrix(worldMatrix);
				}
				else if (m_wardrobeUser == MySession.Static.LocalCharacter)
				{
					m_wardrobeUser.ForceDisablePrediction = false;
					m_wardrobeUser.UpdateCharacterPhysics();
					m_wardrobeUser.PositionComp.SetWorldMatrix(worldMatrix);
				}
			}
			m_wardrobeUser = null;
			if (m_light != null)
			{
				m_light.LightOn = false;
				m_light.UpdateLight();
			}
			UpdateEmissivity();
		}

		void IMyLifeSupportingBlock.ShowTerminal(MyCharacter user)
		{
			MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, this);
		}

		void IMyLifeSupportingBlock.BroadcastSupportRequest(MyCharacter user)
		{
			MyMultiplayer.RaiseEvent(this, (MyMedicalRoom x) => x.RequestSupport, user.EntityId);
		}

		[Event(null, 692)]
		[Reliable]
		[Server(ValidationType.Access)]
		[Broadcast]
		private void RequestSupport(long userId)
		{
			if (GetUserRelationToOwner(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value)).IsFriendly() || MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				MyEntities.TryGetEntityById(userId, out MyCharacter entity);
				if (entity != null)
				{
					m_lifeSupportingComponent.ProvideSupport(entity);
				}
			}
		}

		protected override void Closing()
		{
			StopIdleSound();
			MyLights.RemoveLight(m_light);
			base.Closing();
		}

		private void StopIdleSound()
		{
			m_idleSoundEmitter.StopSound(forced: false);
		}

		private void StartIdleSound()
		{
			m_idleSoundEmitter.PlaySound(m_idleSound, stopPrevious: true);
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SinkComp.Update();
		}

		bool IMyGasBlock.IsWorking()
		{
			return base.IsWorking;
		}

		public void TrySetFaction(MyPlayer player)
		{
			if (!MySession.Static.IsScenario || !m_setFactionToSpawnee || !Sync.IsServer || base.OwnerId == 0L)
			{
				return;
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(base.OwnerId);
			if (myFaction != null)
			{
				MyFactionCollection.SendJoinRequest(myFaction.FactionId, player.Identity.IdentityId);
				if (!myFaction.AutoAcceptMember)
				{
					MyFactionCollection.AcceptJoin(myFaction.FactionId, player.Identity.IdentityId);
				}
			}
		}

		public void TryTakeSpawneeOwnership(MyPlayer player)
		{
			if (MySession.Static.IsScenario && m_takeSpawneeOwnership && Sync.IsServer && base.OwnerId == 0L)
			{
				ChangeBlockOwnerRequest(player.Identity.IdentityId, MyOwnershipShareModeEnum.None);
			}
		}

		public void UpdateScreen()
		{
			if (!CheckIsWorking())
			{
				for (int i = 0; i < m_panels.Count; i++)
				{
					((MyRenderComponentScreenAreas)base.Render).ChangeTexture(i, m_panels[i].GetPathForID("Offline"));
				}
			}
			else
			{
				for (int j = 0; j < m_panels.Count; j++)
				{
					((MyRenderComponentScreenAreas)base.Render).ChangeTexture(j, null);
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
			UpdateEmissivity();
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
	}
}
