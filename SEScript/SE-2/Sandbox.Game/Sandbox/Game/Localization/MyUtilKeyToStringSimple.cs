using VRage.Input;

namespace Sandbox.Game.Localization
{
	internal class MyUtilKeyToStringSimple : MyUtilKeyToString
	{
		private string m_name;

		public override string Name => m_name;

		public MyUtilKeyToStringSimple(MyKeys key, string name)
			: base(key)
		{
			m_name = name;
		}
	}
}
