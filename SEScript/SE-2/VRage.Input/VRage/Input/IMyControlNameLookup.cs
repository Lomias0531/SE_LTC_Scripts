namespace VRage.Input
{
	public interface IMyControlNameLookup
	{
		string UnassignedText
		{
			get;
		}

		string GetKeyName(MyKeys key);

		string GetName(MyMouseButtonsEnum button);

		string GetName(MyJoystickButtonsEnum joystickButton);

		string GetName(MyJoystickAxesEnum joystickAxis);
	}
}
