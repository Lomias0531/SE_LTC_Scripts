using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Character properties")]
	internal class MyGuiScreenDebugCharacterProperties : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugCharacterProperties()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("System character properties", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			if (MySession.Static.LocalCharacter != null)
			{
				AddLabel("Front light", Color.Yellow.ToVector4(), 1.2f);
				AddSlider("Reflector Distance CONST", 1f, 500f, () => 35f, delegate
				{
				});
				AddSlider("Reflector Intensity CONST", 0f, 2f, () => MyCharacter.REFLECTOR_INTENSITY, delegate
				{
				});
				m_currentPosition.Y += 0.01f;
				AddLabel("Movement", Color.Yellow.ToVector4(), 1.2f);
				AddSlider("Acceleration", 0f, 100f, () => MyPerGameSettings.CharacterMovement.WalkAcceleration, delegate(float s)
				{
					MyPerGameSettings.CharacterMovement.WalkAcceleration = s;
				});
				AddSlider("Decceleration", 0f, 100f, () => MyPerGameSettings.CharacterMovement.WalkDecceleration, delegate(float s)
				{
					MyPerGameSettings.CharacterMovement.WalkDecceleration = s;
				});
				AddSlider("Sprint acceleration", 0f, 100f, () => MyPerGameSettings.CharacterMovement.SprintAcceleration, delegate(float s)
				{
					MyPerGameSettings.CharacterMovement.SprintAcceleration = s;
				});
				AddSlider("Sprint decceleration", 0f, 100f, () => MyPerGameSettings.CharacterMovement.SprintDecceleration, delegate(float s)
				{
					MyPerGameSettings.CharacterMovement.SprintDecceleration = s;
				});
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCharacterProperties";
		}
	}
}
