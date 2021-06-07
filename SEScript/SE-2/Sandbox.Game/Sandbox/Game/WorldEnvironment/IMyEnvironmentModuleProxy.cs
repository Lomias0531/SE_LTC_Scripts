using System.Collections.Generic;

namespace Sandbox.Game.WorldEnvironment
{
	public interface IMyEnvironmentModuleProxy
	{
		void Init(MyEnvironmentSector sector, List<int> items);

		void Close();

		void CommitLodChange(int lodBefore, int lodAfter);

		void CommitPhysicsChange(bool enabled);

		void OnItemChange(int index, short newModel);

		void OnItemChangeBatch(List<int> items, int offset, short newModel);

		void HandleSyncEvent(int item, object data, bool fromClient);

		void DebugDraw();
	}
}
