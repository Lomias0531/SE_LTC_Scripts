using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Localization
{
	internal class MyUtilKeyToStringLocalized : MyUtilKeyToString
	{
		private MyStringId m_name;

		public override string Name => MyTexts.GetString(m_name);

		public MyUtilKeyToStringLocalized(MyKeys key, MyStringId name)
			: base(key)
		{
			m_name = name;
		}
	}
}
