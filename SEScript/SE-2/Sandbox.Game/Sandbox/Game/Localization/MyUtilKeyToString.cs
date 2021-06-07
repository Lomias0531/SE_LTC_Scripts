using VRage.Input;

namespace Sandbox.Game.Localization
{
	internal abstract class MyUtilKeyToString
	{
		public MyKeys Key;

		public abstract string Name
		{
			get;
		}

		public MyUtilKeyToString(MyKeys key)
		{
			Key = key;
		}
	}
}
