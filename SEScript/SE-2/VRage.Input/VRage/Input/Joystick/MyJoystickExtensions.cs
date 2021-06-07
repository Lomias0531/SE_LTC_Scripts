namespace VRage.Input.Joystick
{
	internal static class MyJoystickExtensions
	{
		public unsafe static bool IsPressed(this MyJoystickState state, int button)
		{
			return state.Buttons[button] > 0;
		}

		public static bool IsReleased(this MyJoystickState state, int button)
		{
			return !state.IsPressed(button);
		}
	}
}
