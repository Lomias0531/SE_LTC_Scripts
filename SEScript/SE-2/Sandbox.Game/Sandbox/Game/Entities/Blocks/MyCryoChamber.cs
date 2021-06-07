using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_CryoChamber))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyCryoChamber),
		typeof(Sandbox.ModAPI.Ingame.IMyCryoChamber)
	})]
	public class MyCryoChamber : MyCockpit, Sandbox.ModAPI.IMyCryoChamber, Sandbox.ModAPI.IMyCockpit, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, Sandbox.ModAPI.Ingame.IMyCockpit, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyCameraController, Sandbox.ModAPI.IMyTextSurfaceProvider, Sandbox.ModAPI.Ingame.IMyCryoChamber
	{
		protected class m_attachedPlayerId_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType attachedPlayerId;
				ISyncType result = attachedPlayerId = new Sync<MyPlayer.PlayerId?, SyncDirection.FromServer>(P_1, P_2);
				((MyCryoChamber)P_0).m_attachedPlayerId = (Sync<MyPlayer.PlayerId?, SyncDirection.FromServer>)attachedPlayerId;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyCryoChamber_003C_003EActor : IActivator, IActivator<MyCryoChamber>
		{
			private sealed override object CreateInstance()
			{
				return new MyCryoChamber();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCryoChamber CreateInstance()
			{
				return new MyCryoChamber();
			}

			MyCryoChamber IActivator<MyCryoChamber>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private string m_overlayTextureName = "Textures\\GUI\\Screens\\cryopod_interior.dds";

		private MyPlayer.PlayerId? m_currentPlayerId;

		private readonly Sync<MyPlayer.PlayerId?, SyncDirection.FromServer> m_attachedPlayerId;

		private bool m_retryAttachPilot;

		private bool m_pilotLights;

		private bool m_pilotJetpack;

		private bool m_pilotCameraInFP = true;

		public override bool IsInFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		private new MyCryoChamberDefinition BlockDefinition => (MyCryoChamberDefinition)base.BlockDefinition;

		public override MyToolbarType ToolbarType => MyToolbarType.None;

		public MyCryoChamber()
		{
			base.ControllerInfo.ControlAcquired += OnCryoChamberControlAcquired;
			m_attachedPlayerId.ValueChanged += delegate
			{
				AttachedPlayerChanged();
			};
			MinHeadLocalXAngle = -50f;
			MaxHeadLocalXAngle = 60f;
			MinHeadLocalYAngle = -30f;
			MaxHeadLocalYAngle = 30f;
		}

		protected override bool CanHaveHorizon()
		{
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			m_characterDummy = Matrix.Identity;
			base.Init(objectBuilder, cubeGrid);
			if (base.ResourceSink == null)
			{
				MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
				myResourceSinkComponent.Init(MyStringHash.GetOrCompute(BlockDefinition.ResourceSinkGroup), BlockDefinition.IdlePowerConsumption, CalculateRequiredPowerInput);
				myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
				base.ResourceSink = myResourceSinkComponent;
			}
			else
			{
				base.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, BlockDefinition.IdlePowerConsumption);
				base.ResourceSink.SetRequiredInputFuncByType(MyResourceDistributorComponent.ElectricityId, CalculateRequiredPowerInput);
				base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			}
			MyObjectBuilder_CryoChamber myObjectBuilder_CryoChamber = objectBuilder as MyObjectBuilder_CryoChamber;
			if (myObjectBuilder_CryoChamber.SteamId.HasValue && myObjectBuilder_CryoChamber.SerialId.HasValue)
			{
				m_currentPlayerId = new MyPlayer.PlayerId(myObjectBuilder_CryoChamber.SteamId.Value, myObjectBuilder_CryoChamber.SerialId.Value);
			}
			else
			{
				m_currentPlayerId = null;
			}
			string overlayTexture = BlockDefinition.OverlayTexture;
			if (!string.IsNullOrEmpty(overlayTexture))
			{
				m_overlayTextureName = overlayTexture;
			}
			base.HorizonIndicatorEnabled = false;
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private float CalculateRequiredPowerInput()
		{
			if (!base.IsFunctional)
			{
				return 0f;
			}
			return BlockDefinition.IdlePowerConsumption;
		}

		private void PowerDistributor_PowerStateChaged(MyResourceStateEnum newState)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdateIsWorking();
				}
			}, "MyCryoChamber::UpdateIsWorking");
		}

		private void Receiver_IsPoweredChanged()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdateIsWorking();
				}
			}, "MyCryoChamber::UpdateIsWorking");
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateIsWorking();
			CheckEmissiveState();
			if (Sync.IsServer && m_attachedPlayerId.Value != m_currentPlayerId)
			{
				m_attachedPlayerId.Value = m_currentPlayerId;
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CryoChamber myObjectBuilder_CryoChamber = (MyObjectBuilder_CryoChamber)base.GetObjectBuilderCubeBlock(copy);
			if (m_currentPlayerId.HasValue)
			{
				myObjectBuilder_CryoChamber.SteamId = m_currentPlayerId.Value.SteamId;
				myObjectBuilder_CryoChamber.SerialId = m_currentPlayerId.Value.SerialId;
			}
			return myObjectBuilder_CryoChamber;
		}

		protected override void PlacePilotInSeat(MyCharacter pilot)
		{
			m_pilotLights = pilot.LightEnabled;
			if (Sync.IsServer)
			{
				pilot.EnableLights(enable: false);
			}
			m_pilotCameraInFP = pilot.IsInFirstPersonView;
			MyCharacterJetpackComponent jetpackComp = pilot.JetpackComp;
			if (jetpackComp != null)
			{
				m_pilotJetpack = jetpackComp.TurnedOn;
				if (Sync.IsServer)
				{
					jetpackComp.TurnOnJetpack(newState: false);
				}
			}
			pilot.Sit(enableFirstPerson: true, MySession.Static.LocalCharacter == pilot, enableBag: false, BlockDefinition.CharacterAnimation);
			pilot.SuitBattery.ResourceSource.Enabled = true;
			pilot.PositionComp.SetWorldMatrix(m_characterDummy * base.WorldMatrix, this);
			CheckEmissiveState();
		}

		protected void OnCryoChamberControlAcquired(MyEntityController controller)
		{
			m_currentPlayerId = controller.Player.Id;
		}

		protected override void RemovePilotFromSeat(MyCharacter pilot)
		{
			if (pilot == MySession.Static.LocalCharacter)
			{
				MyHudCameraOverlay.Enabled = false;
				base.Render.Visible = true;
			}
			m_currentPlayerId = null;
			if (Sync.IsServer)
			{
				m_attachedPlayerId.Value = null;
				if (m_pilotLights)
				{
					pilot.EnableLights(enable: true);
				}
				if (m_pilotJetpack && pilot.JetpackComp != null)
				{
					pilot.JetpackComp.TurnOnJetpack(newState: true);
				}
			}
			pilot.IsInFirstPersonView = m_pilotCameraInFP;
			m_pilotLights = false;
			m_pilotJetpack = false;
			m_pilotCameraInFP = true;
			CheckEmissiveState();
		}

		public override void CheckEmissiveState(bool force = false)
		{
			if (base.IsWorking)
			{
				SetEmissiveStateWorking();
			}
			else if (base.IsFunctional)
			{
				SetEmissiveStateDisabled();
			}
			else
			{
				SetEmissiveStateDamaged();
			}
		}

		public override UseActionResult CanUse(UseActionEnum actionEnum, IMyControllableEntity user)
		{
			if (!base.IsFunctional)
			{
				return UseActionResult.CockpitDamaged;
			}
			if (!base.IsWorking)
			{
				return UseActionResult.Unpowered;
			}
			if (m_pilot != null)
			{
				return UseActionResult.UsedBySomeoneElse;
			}
			return base.CanUse(actionEnum, user);
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (MyFakes.ENABLE_OXYGEN_SOUNDS)
			{
				UpdateSound(Pilot != null && Pilot == MySession.Static.LocalCharacter);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			base.ResourceSink.Update();
			if (m_retryAttachPilot)
			{
				m_retryAttachPilot = false;
				AttachedPlayerChanged();
			}
		}

		private void SetOverlay()
		{
			if (IsLocalCharacterInside())
			{
				MyHudCameraOverlay.TextureName = m_overlayTextureName;
				MyHudCameraOverlay.Enabled = true;
				base.Render.Visible = false;
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			if (!base.IsWorking)
			{
				return false;
			}
			if (Pilot != null)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
			}
			if (base.OxygenFillLevel > 0f || MySession.Static.CreativeMode)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
		}

		protected override void OnInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					base.OnInputChanged(resourceTypeId, oldInput, sink);
				}
			}, "MyCryoChamber::OnInputChanged");
			CheckEmissiveState();
		}

		protected override void ComponentStack_IsFunctionalChanged()
		{
			MyCharacter pilot = m_pilot;
			MyEntityController controller = base.ControllerInfo.Controller;
			base.ComponentStack_IsFunctionalChanged();
			if (!base.IsFunctional && pilot != null && controller == null)
			{
				if (MySession.Static.CreativeMode)
				{
					pilot.Close();
				}
				else
				{
					pilot.DoDamage(1000f, MyDamageType.Destruction, updateSync: false, 0L);
				}
			}
		}

		public override void OnUnregisteredFromGridSystems()
		{
			MyCharacter pilot = m_pilot;
			MyEntityController controller = base.ControllerInfo.Controller;
			base.OnUnregisteredFromGridSystems();
			if (pilot != null && controller == null && MySession.Static.CreativeMode)
			{
				pilot.Close();
			}
			m_soundEmitter.StopSound(forced: true);
		}

		private bool IsLocalCharacterInside()
		{
			if (MySession.Static.LocalCharacter != null)
			{
				return MySession.Static.LocalCharacter == Pilot;
			}
			return false;
		}

		private void UpdateSound(bool isUsed)
		{
			if (!base.IsWorking)
			{
				return;
			}
			if (isUsed)
			{
				if (m_soundEmitter.SoundId != BlockDefinition.InsideSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.InsideSound.Realistic)
				{
					m_soundEmitter.Force2D = true;
					m_soundEmitter.Force3D = false;
					if (m_soundEmitter.SoundId == BlockDefinition.OutsideSound.Arcade || m_soundEmitter.SoundId != BlockDefinition.OutsideSound.Realistic)
					{
						m_soundEmitter.PlaySound(BlockDefinition.InsideSound, stopPrevious: true);
					}
					else
					{
						m_soundEmitter.PlaySound(BlockDefinition.InsideSound, stopPrevious: true, skipIntro: true);
					}
				}
			}
			else if (m_soundEmitter.SoundId != BlockDefinition.OutsideSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.OutsideSound.Realistic)
			{
				m_soundEmitter.Force2D = false;
				m_soundEmitter.Force3D = true;
				m_soundEmitter.PlaySound(BlockDefinition.OutsideSound, stopPrevious: true);
			}
		}

		public void CameraAttachedToChanged(IMyCameraController oldController, IMyCameraController newController)
		{
			if (oldController == this)
			{
				MyRenderProxy.UpdateRenderObjectVisibility(base.Render.RenderObjectIDs[0], visible: true, near: false);
			}
		}

		protected override void OnControlAcquired_UpdateCamera()
		{
			base.OnControlAcquired_UpdateCamera();
		}

		public override void UpdateCockpitModel()
		{
			base.UpdateCockpitModel();
		}

		public bool TryToControlPilot(MyPlayer player)
		{
			if (Pilot == null)
			{
				return false;
			}
			MyPlayer.PlayerId id = player.Id;
			MyPlayer.PlayerId? currentPlayerId = m_currentPlayerId;
			if (id != currentPlayerId)
			{
				return false;
			}
			if (m_attachedPlayerId.Value == m_currentPlayerId)
			{
				AttachedPlayerChanged();
			}
			else
			{
				m_attachedPlayerId.Value = m_currentPlayerId;
			}
			return true;
		}

		internal void OnPlayerLoaded()
		{
		}

		private void AttachedPlayerChanged()
		{
			if (!m_attachedPlayerId.Value.HasValue)
			{
				return;
			}
			MyPlayer.PlayerId id = new MyPlayer.PlayerId(m_attachedPlayerId.Value.Value.SteamId, m_attachedPlayerId.Value.Value.SerialId);
			MyPlayer playerById = Sync.Players.GetPlayerById(id);
			if (playerById != null)
			{
				if (Pilot != null)
				{
					if (playerById == MySession.Static.LocalHumanPlayer)
					{
						OnPlayerLoaded();
						if (MySession.Static.CameraController != this)
						{
							MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
						}
					}
					playerById.Controller.TakeControl(this);
					playerById.Identity.ChangeCharacter(Pilot);
				}
				else
				{
					_ = MySession.Static.LocalHumanPlayer;
				}
			}
			else
			{
				m_retryAttachPilot = true;
			}
		}
	}
}
