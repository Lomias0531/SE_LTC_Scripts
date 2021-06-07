using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Systems;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	[MyScriptedSystem("DecayBlocks")]
	public class MyCubeBlockDecayScript : MyGroupScriptBase
	{
		private HashSet<MyStringHash> m_tmpSubtypes;

		public MyCubeBlockDecayScript()
		{
			m_tmpSubtypes = new HashSet<MyStringHash>(MyStringHash.Comparer);
		}

		public override void ProcessObjects(ListReader<MyDefinitionId> objects)
		{
			MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
			m_tmpSubtypes.Clear();
			foreach (MyDefinitionId item in objects)
			{
				m_tmpSubtypes.Add(item.SubtypeId);
			}
			foreach (MyEntity item2 in entities)
			{
				MyFloatingObject myFloatingObject = item2 as MyFloatingObject;
				if (myFloatingObject != null)
				{
					MyDefinitionId objectId = myFloatingObject.Item.Content.GetObjectId();
					if (m_tmpSubtypes.Contains(objectId.SubtypeId))
					{
						MyFloatingObjects.RemoveFloatingObject(myFloatingObject, sync: true);
					}
				}
			}
		}
	}
}
