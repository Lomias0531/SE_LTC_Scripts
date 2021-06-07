using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Definitions;
using VRage.Utils;

namespace VRage.Game
{
	public class MyDefinitionSet
	{
		public MyModContext Context;

		public readonly Dictionary<Type, Dictionary<MyStringHash, MyDefinitionBase>> Definitions = new Dictionary<Type, Dictionary<MyStringHash, MyDefinitionBase>>();

		/// Add a new definition to the set.
		///
		/// Crashes if existing.
		public void AddDefinition(MyDefinitionBase def)
		{
			if (!Definitions.TryGetValue(def.Id.TypeId, out Dictionary<MyStringHash, MyDefinitionBase> value))
			{
				value = new Dictionary<MyStringHash, MyDefinitionBase>();
				Definitions[def.Id.TypeId] = value;
			}
			value[def.Id.SubtypeId] = def;
		}

		/// Add or replace an existing definition.
		public bool AddOrRelaceDefinition(MyDefinitionBase def)
		{
			if (!Definitions.TryGetValue(def.Id.TypeId, out Dictionary<MyStringHash, MyDefinitionBase> value))
			{
				value = new Dictionary<MyStringHash, MyDefinitionBase>();
				Definitions[def.Id.TypeId] = value;
			}
			bool result = value.ContainsKey(def.Id.SubtypeId);
			value[def.Id.SubtypeId] = def;
			return result;
		}

		/// Remove a definition if on the set.
		public void RemoveDefinition(ref MyDefinitionId defId)
		{
			if (Definitions.TryGetValue(defId.TypeId, out Dictionary<MyStringHash, MyDefinitionBase> value))
			{
				value.Remove(defId.SubtypeId);
			}
		}

		/// Get all definitions of a given type.
		public IEnumerable<T> GetDefinitionsOfType<T>() where T : MyDefinitionBase
		{
			Type objectBuilderType = MyDefinitionManagerBase.GetObjectBuilderType(typeof(T));
			if (Definitions.TryGetValue(objectBuilderType, out Dictionary<MyStringHash, MyDefinitionBase> value))
			{
				return value.Values.Cast<T>();
			}
			return null;
		}

		/// Get all definitions of a given type.
		public IEnumerable<T> GetDefinitionsOfTypeAndSubtypes<T>() where T : MyDefinitionBase
		{
			HashSet<Type> subtypes = MyDefinitionManagerBase.Static.GetSubtypes<T>();
			Dictionary<MyStringHash, MyDefinitionBase> value = null;
			if (subtypes == null)
			{
				if (Definitions.TryGetValue(MyDefinitionManagerBase.GetObjectBuilderType(typeof(T)), out value))
				{
					return value.Values.Cast<T>();
				}
				return null;
			}
			return subtypes.SelectMany((Type x) => Definitions.GetOrEmpty(MyDefinitionManagerBase.GetObjectBuilderType(x)).Cast<T>());
		}

		public bool ContainsDefinition(MyDefinitionId id)
		{
			if (Definitions.TryGetValue(id.TypeId, out Dictionary<MyStringHash, MyDefinitionBase> value))
			{
				return value.ContainsKey(id.SubtypeId);
			}
			return false;
		}

		public T GetDefinition<T>(MyStringHash subtypeId) where T : MyDefinitionBase
		{
			MyDefinitionBase value = null;
			Type objectBuilderType = MyDefinitionManagerBase.GetObjectBuilderType(typeof(T));
			if (Definitions.TryGetValue(objectBuilderType, out Dictionary<MyStringHash, MyDefinitionBase> value2))
			{
				value2.TryGetValue(subtypeId, out value);
			}
			return (T)value;
		}

		public T GetDefinition<T>(MyDefinitionId id) where T : MyDefinitionBase
		{
			MyDefinitionBase value = null;
			if (Definitions.TryGetValue(id.TypeId, out Dictionary<MyStringHash, MyDefinitionBase> value2))
			{
				value2.TryGetValue(id.SubtypeId, out value);
			}
			return value as T;
		}

		/// Override the contents of this definition set with another.
		public virtual void OverrideBy(MyDefinitionSet definitionSet)
		{
			MyDefinitionPostprocessor.Bundle bundle = default(MyDefinitionPostprocessor.Bundle);
			bundle.Set = this;
			bundle.Context = Context;
			MyDefinitionPostprocessor.Bundle currentDefinitions = bundle;
			bundle = default(MyDefinitionPostprocessor.Bundle);
			bundle.Set = definitionSet;
			bundle.Context = definitionSet.Context;
			MyDefinitionPostprocessor.Bundle overrideBySet = bundle;
			foreach (KeyValuePair<Type, Dictionary<MyStringHash, MyDefinitionBase>> definition in definitionSet.Definitions)
			{
				if (!Definitions.TryGetValue(definition.Key, out Dictionary<MyStringHash, MyDefinitionBase> value))
				{
					value = new Dictionary<MyStringHash, MyDefinitionBase>();
					Definitions[definition.Key] = value;
				}
				MyDefinitionPostprocessor postProcessor = MyDefinitionManagerBase.GetPostProcessor(definition.Key);
				if (postProcessor == null)
				{
					postProcessor = MyDefinitionManagerBase.GetPostProcessor(MyDefinitionManagerBase.GetObjectBuilderType(definition.Value.First().Value.GetType()));
				}
				currentDefinitions.Definitions = value;
				overrideBySet.Definitions = definition.Value;
				postProcessor.OverrideBy(ref currentDefinitions, ref overrideBySet);
			}
		}

		public void Clear()
		{
			foreach (KeyValuePair<Type, Dictionary<MyStringHash, MyDefinitionBase>> definition in Definitions)
			{
				definition.Value.Clear();
			}
		}
	}
}
