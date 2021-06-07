using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PirateAntennaDefinition), null)]
	public class MyPirateAntennaDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPirateAntennaDefinition_003C_003EActor : IActivator, IActivator<MyPirateAntennaDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPirateAntennaDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPirateAntennaDefinition CreateInstance()
			{
				return new MyPirateAntennaDefinition();
			}

			MyPirateAntennaDefinition IActivator<MyPirateAntennaDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Name;

		public float SpawnDistance;

		public int SpawnTimeMs;

		public int FirstSpawnTimeMs;

		public int MaxDrones;

		public MyDiscreteSampler<MySpawnGroupDefinition> SpawnGroupSampler;

		private List<string> m_spawnGroups;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PirateAntennaDefinition myObjectBuilder_PirateAntennaDefinition = builder as MyObjectBuilder_PirateAntennaDefinition;
			Name = myObjectBuilder_PirateAntennaDefinition.Name;
			SpawnDistance = myObjectBuilder_PirateAntennaDefinition.SpawnDistance;
			SpawnTimeMs = myObjectBuilder_PirateAntennaDefinition.SpawnTimeMs;
			FirstSpawnTimeMs = myObjectBuilder_PirateAntennaDefinition.FirstSpawnTimeMs;
			MaxDrones = myObjectBuilder_PirateAntennaDefinition.MaxDrones;
			m_spawnGroups = new List<string>();
			string[] spawnGroups = myObjectBuilder_PirateAntennaDefinition.SpawnGroups;
			foreach (string item in spawnGroups)
			{
				m_spawnGroups.Add(item);
			}
		}

		public new void Postprocess()
		{
			List<MySpawnGroupDefinition> list = new List<MySpawnGroupDefinition>();
			List<float> list2 = new List<float>();
			foreach (string spawnGroup in m_spawnGroups)
			{
				MySpawnGroupDefinition definition = null;
				MyDefinitionId defId = new MyDefinitionId(typeof(MyObjectBuilder_SpawnGroupDefinition), spawnGroup);
				MyDefinitionManager.Static.TryGetDefinition(defId, out definition);
				if (definition != null)
				{
					list.Add(definition);
					list2.Add(definition.Frequency);
				}
			}
			m_spawnGroups = null;
			if (list2.Count != 0)
			{
				SpawnGroupSampler = new MyDiscreteSampler<MySpawnGroupDefinition>(list, list2);
			}
		}
	}
}
