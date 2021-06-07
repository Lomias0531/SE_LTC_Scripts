using VRage.Utils;

namespace VRage.Game.Entity.UseObject
{
	public struct MyActionDescription
	{
		public MyStringId Text;

		public object[] FormatParams;

		public bool IsTextControlHint;

		public MyStringId? JoystickText;

		public object[] JoystickFormatParams;

		public bool ShowForGamepad;
	}
}
