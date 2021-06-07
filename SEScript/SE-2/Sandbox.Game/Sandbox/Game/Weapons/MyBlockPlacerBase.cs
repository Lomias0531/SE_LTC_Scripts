using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI.Weapons;
using VRage.Audio;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	public abstract class MyBlockPlacerBase : MyEngineerToolBase, IMyBlockPlacerBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEngineerToolBase, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
	{
		public static MyHudNotificationBase MissingComponentNotification = new MyHudNotification(MyCommonTexts.NotificationMissingComponentToPlaceBlockFormat, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 1);

		protected int m_lastKeyPress;

		protected bool m_firstShot;

		protected bool m_closeAfterBuild;

		private MyHandItemDefinition m_definition;

		protected abstract MyBlockBuilderBase BlockBuilder
		{
			get;
		}

		protected MyBlockPlacerBase(MyHandItemDefinition definition)
			: base(500)
		{
			m_definition = definition;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			Init(objectBuilder, m_definition.PhysicalItemId);
			Init(null, null, null, null);
			base.Render.CastShadows = true;
			base.Render.NeedsResolveCastShadow = false;
			HasSecondaryEffect = false;
			HasPrimaryEffect = false;
			m_firstShot = true;
			if (base.PhysicalObject != null)
			{
				base.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)objectBuilder.Clone();
			}
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			bool result = base.CanShoot(action, shooter, out status);
			if (status == MyGunStatusEnum.Cooldown && action == MyShootActionEnum.PrimaryAction && m_firstShot)
			{
				status = MyGunStatusEnum.OK;
				result = true;
			}
			return result;
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (MySession.Static.CreativeMode)
			{
				return;
			}
			m_closeAfterBuild = false;
			base.Shoot(action, direction, null, gunAction);
			base.ShakeAmount = 0f;
			if (action != 0 || !m_firstShot)
			{
				return;
			}
			m_firstShot = false;
			m_lastKeyPress = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			MyCubeBlockDefinition currentBlockDefinition = MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition;
			if (currentBlockDefinition == null)
			{
				return;
			}
			if (!Owner.ControllerInfo.IsLocallyControlled())
			{
				MyCockpit myCockpit = Owner.IsUsing as MyCockpit;
				if (myCockpit == null || !myCockpit.ControllerInfo.IsLocallyControlled())
				{
					return;
				}
			}
			if (MyCubeBuilder.Static.CanStartConstruction(Owner))
			{
				MyCubeBuilder.Static.AddConstruction(Owner);
			}
			else if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				OnMissingComponents(currentBlockDefinition);
			}
		}

		public static void OnMissingComponents(MyCubeBlockDefinition definition)
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			(MyHud.Notifications.Get(MyNotificationSingletons.MissingComponent) as MyHudMissingComponentNotification).SetBlockDefinition(definition);
			MyHud.Notifications.Add(MyNotificationSingletons.MissingComponent);
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			base.EndShoot(action);
			m_firstShot = true;
			if (base.CharacterInventory != null)
			{
				MyCharacter myCharacter = base.CharacterInventory.Owner as MyCharacter;
				if (myCharacter != null && m_closeAfterBuild && (myCharacter.ControllerInfo == null || !myCharacter.ControllerInfo.IsRemotelyControlled()))
				{
					myCharacter.SwitchToWeapon((MyToolbarItemWeapon)null);
				}
			}
		}

		public override void OnControlReleased()
		{
			if (Owner != null && Owner.ControllerInfo.IsLocallyHumanControlled())
			{
				BlockBuilder.Deactivate();
				MySession.Static.GameFocusManager.Clear();
			}
			base.OnControlReleased();
		}

		public override void OnControlAcquired(MyCharacter owner)
		{
			base.OnControlAcquired(owner);
			if (Owner != null)
			{
				if (owner.UseNewAnimationSystem)
				{
					Owner.TriggerCharacterAnimationEvent("building", sync: false);
				}
				else
				{
					Owner.PlayCharacterAnimation("Building_pose", MyBlendOption.Immediate, MyFrameOption.Loop, 0.2f);
				}
			}
		}

		protected override void AddHudInfo()
		{
		}

		protected override void RemoveHudInfo()
		{
		}

		protected override void DrawHud()
		{
		}

		public new bool SupressShootAnimation()
		{
			return false;
		}

		public override void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public new bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public new bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return false;
		}
	}
}
