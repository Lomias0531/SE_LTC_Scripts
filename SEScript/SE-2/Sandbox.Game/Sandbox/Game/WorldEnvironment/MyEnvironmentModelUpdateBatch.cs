using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.WorldEnvironment
{
	public class MyEnvironmentModelUpdateBatch : IDisposable
	{
		private struct ModelList
		{
			public List<int> Items;

			public short Model;
		}

		private Dictionary<MyDefinitionId, ModelList> m_modelPerItemDefinition = new Dictionary<MyDefinitionId, ModelList>();

		private IMyEnvironmentOwner m_owner;

		private MyLogicalEnvironmentSectorBase m_sector;

		public MyEnvironmentModelUpdateBatch(MyLogicalEnvironmentSectorBase sector)
		{
			m_sector = sector;
			m_owner = m_sector.Owner;
		}

		public void Add(MyDefinitionId modelDef, int item)
		{
			if (!m_modelPerItemDefinition.TryGetValue(modelDef, out ModelList value))
			{
				value.Items = new List<int>();
				if (modelDef.TypeId.IsNull)
				{
					value.Model = -1;
				}
				else
				{
					MyPhysicalModelDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelDefinition>(modelDef);
					value.Model = (short)((definition != null) ? m_owner.GetModelId(definition) : (-1));
				}
				m_modelPerItemDefinition[modelDef] = value;
			}
			value.Items.Add(item);
		}

		public void Dispose()
		{
			Dispatch();
		}

		public void Dispatch()
		{
			foreach (ModelList value in m_modelPerItemDefinition.Values)
			{
				m_sector.UpdateItemModelBatch(value.Items, value.Model);
			}
		}
	}
}
