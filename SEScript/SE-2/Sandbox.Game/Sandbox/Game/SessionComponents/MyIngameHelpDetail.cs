using System;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	public class MyIngameHelpDetail
	{
		public MyStringId TextEnum;

		public object[] Args;

		public Func<bool> FinishCondition;

		public bool Finished;
	}
}
