using Sandbox.Definitions;
using Sandbox.Game.AI;
using Sandbox.Game.GameSystems;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	public class MyBotSpawningEnvironmentProxy : IMyEnvironmentModuleProxy
	{
		private MyEnvironmentSector m_sector;

		protected readonly MyRandom m_random = new MyRandom();

		protected List<int> m_items;

		protected Queue<int> m_spawnQueue;

		public long SectorId => m_sector.SectorId;

		public void Init(MyEnvironmentSector sector, List<int> items)
		{
			m_sector = sector;
			m_items = items;
			m_spawnQueue = new Queue<int>();
			foreach (int item in m_items)
			{
				m_spawnQueue.Enqueue(item);
			}
		}

		public void Close()
		{
			m_spawnQueue.Clear();
		}

		public void CommitLodChange(int lodBefore, int lodAfter)
		{
			if (lodAfter == 0)
			{
				MyEnvironmentBotSpawningSystem.Static.RegisterBotSpawningProxy(this);
			}
			else
			{
				MyEnvironmentBotSpawningSystem.Static.UnregisterBotSpawningProxy(this);
			}
		}

		public void CommitPhysicsChange(bool enabled)
		{
		}

		public void OnItemChange(int index, short newModel)
		{
		}

		public void OnItemChangeBatch(List<int> items, int offset, short newModel)
		{
		}

		public void HandleSyncEvent(int item, object data, bool fromClient)
		{
		}

		public void DebugDraw()
		{
		}

		public bool OnSpawnTick()
		{
			if (m_spawnQueue.Count == 0 || MyAIComponent.Static.GetAvailableUncontrolledBotsCount() < 1)
			{
				return false;
			}
			int count = m_spawnQueue.Count;
			int num = 0;
			while (num < count)
			{
				num++;
				int num2 = m_spawnQueue.Dequeue();
				m_spawnQueue.Enqueue(num2);
				if (m_sector.DataView.Items.Count >= num2)
				{
					ItemInfo itemInfo = m_sector.DataView.Items[num2];
					Vector3D v = m_sector.SectorCenter + itemInfo.Position;
					if (MyEnvironmentBotSpawningSystem.Static.IsHumanPlayerWithinRange(v))
					{
						m_sector.Owner.GetDefinition((ushort)itemInfo.DefinitionIndex, out MyRuntimeEnvironmentItemInfo def);
						MyDefinitionId subtypeId = new MyDefinitionId(typeof(MyObjectBuilder_BotCollectionDefinition), def.Subtype);
						MyBotCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyBotCollectionDefinition>(subtypeId);
						using (m_random.PushSeed(num2.GetHashCode()))
						{
							MyDefinitionId id = definition.Bots.Sample(m_random);
							MyAgentDefinition agentDefinition = MyDefinitionManager.Static.GetBotDefinition(id) as MyAgentDefinition;
							MyAIComponent.Static.SpawnNewBot(agentDefinition, itemInfo.Position + m_sector.SectorCenter, createdByPlayer: false);
						}
						return true;
					}
				}
			}
			return false;
		}
	}
}
