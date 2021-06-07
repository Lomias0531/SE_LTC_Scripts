using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment
{
	public interface IMyEnvironmentModule
	{
		void ProcessItems(Dictionary<short, MyLodEnvironmentItemSet> items, int changedLodMin, int changedLodMax);

		void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob);

		void Close();

		MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder();

		void OnItemEnable(int item, bool enable);

		void HandleSyncEvent(int logicalItem, object data, bool fromClient);

		void DebugDraw();
	}
}
