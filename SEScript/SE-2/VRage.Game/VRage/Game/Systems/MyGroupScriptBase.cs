using VRage.Collections;

namespace VRage.Game.Systems
{
	public abstract class MyGroupScriptBase
	{
		public MyGroupScriptBase()
		{
		}

		public abstract void ProcessObjects(ListReader<MyDefinitionId> objects);
	}
}
