namespace VRage.Input
{
	internal static class MyEnumsToStrings
	{
		public static string[] GuiInputDeviceEnum = new string[6]
		{
			"None",
			"Keyboard",
			"Mouse",
			"Joystick",
			"JoystickAxis",
			"KeyboardSecond"
		};

		public static string[] MouseButtonsEnum = new string[6]
		{
			"None",
			"Left",
			"Middle",
			"Right",
			"XButton1",
			"XButton2"
		};

		public static string[] JoystickButtonsEnum = new string[21]
		{
			"None",
			"JDLeft",
			"JDRight",
			"JDUp",
			"JDDown",
			"J01",
			"J02",
			"J03",
			"J04",
			"J05",
			"J06",
			"J07",
			"J08",
			"J09",
			"J10",
			"J11",
			"J12",
			"J13",
			"J14",
			"J15",
			"J16"
		};

		public static string[] JoystickAxesEnum = new string[17]
		{
			"None",
			"JXAxis+",
			"JXAxis-",
			"JYAxis+",
			"JYAxis-",
			"JZAxis+",
			"JZAxis-",
			"JXRotation+",
			"JXRotation-",
			"JYRotation+",
			"JYRotation-",
			"JZRotation+",
			"JZRotation-",
			"JSlider1+",
			"JSlider1-",
			"JSlider2+",
			"JSlider2-"
		};

		public static string[] ControlTypeEnum = new string[8]
		{
			"General",
			"Navigation",
			"Communications",
			"Weapons",
			"SpecialWeapons",
			"Systems1",
			"Systems2",
			"Editor"
		};
	}
}
