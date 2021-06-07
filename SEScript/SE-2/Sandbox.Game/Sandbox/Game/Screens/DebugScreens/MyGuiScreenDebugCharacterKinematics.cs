using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Character kinematics")]
	internal class MyGuiScreenDebugCharacterKinematics : MyGuiScreenDebugBase
	{
		public bool updating;

		public MyRagdollMapper PlayerRagdollMapper => MySession.Static.LocalCharacter.Components.Get<MyCharacterRagdollComponent>()?.RagdollMapper;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCharacterKinematics";
		}

		public MyGuiScreenDebugCharacterKinematics()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Character kinematics debug draw", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCheckBox("Enable permanent IK/Ragdoll simulation ", null, MemberHelper.GetMember(() => MyFakes.ENABLE_PERMANENT_SIMULATIONS_COMPUTATION));
			AddCheckBox("Draw Ragdoll Rig Pose", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_ORIGINAL_RIG));
			AddCheckBox("Draw Bones Rig Pose", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_ORIGINAL_RIG));
			AddCheckBox("Draw Ragdoll Pose", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_POSE));
			AddCheckBox("Draw Bones", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_COMPUTED_BONES));
			AddCheckBox("Draw bones intended transforms", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_DESIRED));
			AddCheckBox("Draw Hip Ragdoll and Char. Position", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_HIPPOSITIONS));
			AddCheckBox("Enable Ragdoll", null, MemberHelper.GetMember(() => MyPerGameSettings.EnableRagdollModels));
			AddCheckBox("Enable Bones Translation", null, MemberHelper.GetMember(() => MyFakes.ENABLE_RAGDOLL_BONES_TRANSLATION));
			AddSlider("Animation weighting", 0f, 5f, null, MemberHelper.GetMember(() => MyFakes.RAGDOLL_ANIMATION_WEIGHTING));
			AddSlider("Ragdoll gravity multiplier", 0f, 50f, null, MemberHelper.GetMember(() => MyFakes.RAGDOLL_GRAVITY_MULTIPLIER));
			StringBuilder text = new StringBuilder("Kill Ragdoll");
			AddButton(text, killRagdollAction);
			StringBuilder text2 = new StringBuilder("Activate Ragdoll");
			AddButton(text2, activateRagdollAction);
			StringBuilder text3 = new StringBuilder("Switch to Dynamic / Keyframed");
			AddButton(text3, switchRagdoll);
		}

		private void switchRagdoll(MyGuiControlButton obj)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (PlayerRagdollMapper.IsActive)
			{
				if (localCharacter.Physics.Ragdoll.IsKeyframed)
				{
					localCharacter.Physics.Ragdoll.EnableConstraints();
					PlayerRagdollMapper.SetRagdollToDynamic();
				}
				else
				{
					localCharacter.Physics.Ragdoll.DisableConstraints();
					PlayerRagdollMapper.SetRagdollToKeyframed();
				}
			}
		}

		private void activateRagdollAction(MyGuiControlButton obj)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (PlayerRagdollMapper == null)
			{
				MyCharacterRagdollComponent myCharacterRagdollComponent = new MyCharacterRagdollComponent();
				localCharacter.Components.Add(myCharacterRagdollComponent);
				myCharacterRagdollComponent.InitRagdoll();
			}
			if (PlayerRagdollMapper.IsActive)
			{
				PlayerRagdollMapper.Deactivate();
			}
			localCharacter.Physics.SwitchToRagdollMode(deadMode: false);
			PlayerRagdollMapper.Activate();
			PlayerRagdollMapper.SetRagdollToKeyframed();
			localCharacter.Physics.Ragdoll.DisableConstraints();
		}

		private void killRagdollAction(MyGuiControlButton obj)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			MyFakes.CHARACTER_CAN_DIE_EVEN_IN_CREATIVE_MODE = true;
			localCharacter.DoDamage(1000000f, MyDamageType.Suicide, updateSync: true, 0L);
		}
	}
}
