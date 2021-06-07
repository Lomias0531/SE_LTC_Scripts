using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects
{
	[MyUseObject("wardrobe")]
	internal class MyUseObjectWardrobe : MyUseObjectBase
	{
		public readonly MyCubeBlock Block;

		public readonly Matrix LocalMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => (MatrixD)LocalMatrix * Block.WorldMatrix;

		public override MatrixD WorldMatrix => Block.WorldMatrix;

		public override uint RenderObjectID => Block.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions => PrimaryAction | SecondaryAction;

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.None;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectWardrobe(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			Block = (owner as MyCubeBlock);
			LocalMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (!Block.GetUserRelationToOwner(myCharacter.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
			{
				if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
			}
			else
			{
				if (actionEnum != UseActionEnum.Manipulate)
				{
					return;
				}
				MyMedicalRoom myMedicalRoom = Block as MyMedicalRoom;
				if (myMedicalRoom == null || !myMedicalRoom.IsWorking)
				{
					return;
				}
				if (!myMedicalRoom.SuitChangeAllowed)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					return;
				}
				MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.None;
				bool flag = myCharacter.Definition.Skeleton == "Humanoid";
				if (myMedicalRoom.CustomWardrobesEnabled)
				{
					if (MyGameService.IsActive && flag)
					{
						MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
						if (component != null)
						{
							component.EnableWindowPopups = false;
						}
						MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenLoadInventory(inGame: true, myMedicalRoom.CustomWardrobeNames));
						MyGuiScreenGamePlay.ActiveGameplayScreen.Closed += ActiveGameplayScreen_Closed;
						myMedicalRoom.UseWardrobe(myCharacter);
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenWardrobe(myCharacter, myMedicalRoom.CustomWardrobeNames));
					}
				}
				else if (MyGameService.IsActive && flag)
				{
					MySessionComponentContainerDropSystem component2 = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
					if (component2 != null)
					{
						component2.EnableWindowPopups = false;
					}
					MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenLoadInventory(inGame: true));
					MyGuiScreenGamePlay.ActiveGameplayScreen.Closed += ActiveGameplayScreen_Closed;
					myMedicalRoom.UseWardrobe(myCharacter);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenWardrobe(myCharacter));
				}
			}
		}

		private void ActiveGameplayScreen_Closed(MyGuiScreenBase source)
		{
			(Block as MyMedicalRoom)?.StopUsingWardrobe();
			MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
			if (component != null)
			{
				component.EnableWindowPopups = true;
			}
			if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
			{
				MyGuiScreenGamePlay.ActiveGameplayScreen.Closed -= ActiveGameplayScreen_Closed;
				MyGuiScreenGamePlay.ActiveGameplayScreen = null;
			}
			MyEntity myEntity;
			if (MySession.Static.LocalCharacter != null && (myEntity = (MySession.Static.LocalCharacter.CurrentWeapon as MyEntity)) != null)
			{
				MyAssetModifierComponent myAssetModifierComponent = myEntity.Components.Get<MyAssetModifierComponent>();
				if (myAssetModifierComponent != null)
				{
					MyLocalCache.LoadInventoryConfig(myEntity, myAssetModifierComponent);
				}
			}
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result = default(MyActionDescription);
			result.Text = MyCommonTexts.NotificationHintPressToUseWardrobe;
			result.FormatParams = new object[1]
			{
				string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]")
			};
			result.IsTextControlHint = true;
			result.JoystickText = MyCommonTexts.NotificationHintPressToUseWardrobe;
			result.JoystickFormatParams = new object[1]
			{
				MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE)
			};
			result.ShowForGamepad = true;
			return result;
		}

		public override bool HandleInput()
		{
			return false;
		}

		public override void OnSelectionLost()
		{
		}
	}
}
