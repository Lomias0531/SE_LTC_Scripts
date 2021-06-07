using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentType(typeof(MyModStorageComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_ModStorageComponent), true)]
	public class MyModStorageComponent : MyModStorageComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyModStorageComponent_003C_003EActor : IActivator, IActivator<MyModStorageComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyModStorageComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyModStorageComponent CreateInstance()
			{
				return new MyModStorageComponent();
			}

			MyModStorageComponent IActivator<MyModStorageComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private HashSet<Guid> m_cachedGuids = new HashSet<Guid>();

		public IReadOnlyDictionary<Guid, string> Storage => (IReadOnlyDictionary<Guid, string>)m_storageData;

		public override bool IsSerialized()
		{
			return m_storageData.Count > 0;
		}

		public override string GetValue(Guid guid)
		{
			return m_storageData[guid];
		}

		public override bool TryGetValue(Guid guid, out string value)
		{
			if (m_storageData.ContainsKey(guid))
			{
				value = m_storageData[guid];
				return true;
			}
			value = null;
			return false;
		}

		public override void SetValue(Guid guid, string value)
		{
			m_storageData[guid] = value;
		}

		public override bool RemoveValue(Guid guid)
		{
			return m_storageData.Remove(guid);
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_ModStorageComponent myObjectBuilder_ModStorageComponent = (MyObjectBuilder_ModStorageComponent)base.Serialize(copy);
			myObjectBuilder_ModStorageComponent.Storage = new SerializableDictionary<Guid, string>();
			foreach (MyModStorageComponentDefinition entityComponentDefinition in MyDefinitionManager.Static.GetEntityComponentDefinitions<MyModStorageComponentDefinition>())
			{
				Guid[] registeredStorageGuids = entityComponentDefinition.RegisteredStorageGuids;
				for (int i = 0; i < registeredStorageGuids.Length; i++)
				{
					Guid item = registeredStorageGuids[i];
					if (!m_cachedGuids.Add(item))
					{
						MyLog.Default.Log(MyLogSeverity.Warning, "Duplicate ModStorageComponent GUID: {0}, in {1}: {2}", item.ToString(), entityComponentDefinition.Context.ModId, entityComponentDefinition.Id.ToString());
					}
				}
			}
			foreach (Guid key in Storage.Keys)
			{
				if (m_cachedGuids.Contains(key))
				{
					myObjectBuilder_ModStorageComponent.Storage[key] = Storage[key];
				}
				else
				{
					MyLog.Default.Log(MyLogSeverity.Warning, "Not saving ModStorageComponent GUID: {0}, not claimed", key.ToString());
				}
			}
			m_cachedGuids.Clear();
			if (myObjectBuilder_ModStorageComponent.Storage.Dictionary.Count == 0)
			{
				return null;
			}
			return myObjectBuilder_ModStorageComponent;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			SerializableDictionary<Guid, string> storage = ((MyObjectBuilder_ModStorageComponent)builder).Storage;
			if (storage != null && storage.Dictionary != null)
			{
				m_storageData = new Dictionary<Guid, string>(storage.Dictionary);
			}
		}
	}
}
