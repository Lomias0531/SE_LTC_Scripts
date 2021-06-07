using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Utils;

namespace VRage.Game.Components
{
	public class MyComponentContainer
	{
		private readonly Dictionary<Type, MyComponentBase> m_components = new Dictionary<Type, MyComponentBase>();

		private static List<KeyValuePair<Type, MyComponentBase>> m_tmpComponents;

		[ThreadStatic]
		private static List<KeyValuePair<Type, MyComponentBase>> m_tmpSerializedComponents;

		public void Add<T>(T component) where T : MyComponentBase
		{
			Type typeFromHandle = typeof(T);
			Add(typeFromHandle, component);
		}

		public void Add(Type type, MyComponentBase component)
		{
			if (!typeof(MyComponentBase).IsAssignableFrom(type) || (component != null && !type.IsAssignableFrom(component.GetType())))
			{
				return;
			}
			Type componentType = MyComponentTypeFactory.GetComponentType(type);
			if (componentType != null)
			{
				_ = (componentType != type);
			}
			if (m_components.TryGetValue(type, out MyComponentBase value))
			{
				if (value is IMyComponentAggregate)
				{
					(value as IMyComponentAggregate).AddComponent(component);
					return;
				}
				if (component is IMyComponentAggregate)
				{
					Remove(type);
					(component as IMyComponentAggregate).AddComponent(value);
					m_components[type] = component;
					component.SetContainer(this);
					OnComponentAdded(type, component);
					return;
				}
			}
			Remove(type);
			if (component != null)
			{
				m_components[type] = component;
				component.SetContainer(this);
				OnComponentAdded(type, component);
			}
		}

		public void Remove<T>() where T : MyComponentBase
		{
			Type typeFromHandle = typeof(T);
			Remove(typeFromHandle);
		}

		public void Remove(Type t)
		{
			if (m_components.TryGetValue(t, out MyComponentBase value))
			{
				RemoveComponentInternal(t, value);
			}
		}

		private void RemoveComponentInternal(Type t, MyComponentBase c)
		{
			c.SetContainer(null);
			m_components.Remove(t);
			OnComponentRemoved(t, c);
		}

		public void Remove(Type t, MyComponentBase component)
		{
			MyComponentBase value = null;
			m_components.TryGetValue(t, out value);
			if (value != null)
			{
				IMyComponentAggregate myComponentAggregate = value as IMyComponentAggregate;
				if (myComponentAggregate == null)
				{
					RemoveComponentInternal(t, component);
				}
				else
				{
					myComponentAggregate.RemoveComponent(component);
				}
			}
		}

		public T Get<T>() where T : MyComponentBase
		{
			m_components.TryGetValue(typeof(T), out MyComponentBase value);
			return (T)value;
		}

		public bool TryGet<T>(out T component) where T : MyComponentBase
		{
			MyComponentBase value;
			bool result = m_components.TryGetValue(typeof(T), out value);
			component = (T)value;
			return result;
		}

		public bool TryGet(Type type, out MyComponentBase component)
		{
			return m_components.TryGetValue(type, out component);
		}

		public bool Has<T>() where T : MyComponentBase
		{
			return m_components.ContainsKey(typeof(T));
		}

		/// <summary>
		/// Returns if any component is assignable from type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Contains(Type type)
		{
			foreach (Type key in m_components.Keys)
			{
				if (type.IsAssignableFrom(key))
				{
					return true;
				}
			}
			return false;
		}

		public void Clear()
		{
			if (m_components.Count > 0)
			{
				MyUtils.ClearCollectionToken<List<KeyValuePair<Type, MyComponentBase>>, KeyValuePair<Type, MyComponentBase>> clearCollectionToken = MyUtils.ReuseCollection(ref m_tmpComponents);
				try
				{
					List<KeyValuePair<Type, MyComponentBase>> collection = clearCollectionToken.Collection;
					foreach (KeyValuePair<Type, MyComponentBase> component in m_components)
					{
						collection.Add(component);
						component.Value.SetContainer(null);
					}
					m_components.Clear();
					foreach (KeyValuePair<Type, MyComponentBase> item in collection)
					{
						OnComponentRemoved(item.Key, item.Value);
					}
				}
				finally
				{
					((IDisposable)clearCollectionToken).Dispose();
				}
			}
		}

		public void OnAddedToScene()
		{
			foreach (KeyValuePair<Type, MyComponentBase> component in m_components)
			{
				component.Value.OnAddedToScene();
			}
		}

		public void OnRemovedFromScene()
		{
			foreach (KeyValuePair<Type, MyComponentBase> component in m_components)
			{
				component.Value.OnRemovedFromScene();
			}
		}

		public virtual void Init(MyContainerDefinition definition)
		{
		}

		protected virtual void OnComponentAdded(Type t, MyComponentBase component)
		{
		}

		protected virtual void OnComponentRemoved(Type t, MyComponentBase component)
		{
		}

		public MyObjectBuilder_ComponentContainer Serialize(bool copy = false)
		{
			MyUtils.ClearRangeToken<KeyValuePair<Type, MyComponentBase>> clearRangeToken = MyUtils.ReuseCollectionNested(ref m_tmpSerializedComponents);
			try
			{
				foreach (KeyValuePair<Type, MyComponentBase> component in m_components)
				{
					if (component.Value.IsSerialized())
					{
						clearRangeToken.Add(component);
					}
				}
				if (clearRangeToken.Collection.Count == 0)
				{
					return null;
				}
				MyObjectBuilder_ComponentContainer myObjectBuilder_ComponentContainer = new MyObjectBuilder_ComponentContainer();
				foreach (KeyValuePair<Type, MyComponentBase> item in clearRangeToken)
				{
					MyObjectBuilder_ComponentBase myObjectBuilder_ComponentBase = item.Value.Serialize(copy);
					if (myObjectBuilder_ComponentBase != null)
					{
						myObjectBuilder_ComponentContainer.Components.Add(new MyObjectBuilder_ComponentContainer.ComponentData
						{
							TypeId = item.Key.Name,
							Component = myObjectBuilder_ComponentBase
						});
					}
				}
				return myObjectBuilder_ComponentContainer;
			}
			finally
			{
				((IDisposable)clearRangeToken).Dispose();
			}
		}

		public void Deserialize(MyObjectBuilder_ComponentContainer builder)
		{
			if (builder != null && builder.Components != null)
			{
				foreach (MyObjectBuilder_ComponentContainer.ComponentData component2 in builder.Components)
				{
					MyComponentBase component = null;
					Type createdInstanceType = MyComponentFactory.GetCreatedInstanceType(component2.Component.TypeId);
					Type type = MyComponentTypeFactory.GetType(component2.TypeId);
					Type componentType = MyComponentTypeFactory.GetComponentType(createdInstanceType);
					if (componentType != null)
					{
						type = componentType;
					}
					bool flag = TryGet(type, out component);
					if (flag && createdInstanceType != component.GetType() && createdInstanceType != typeof(MyHierarchyComponentBase))
					{
						flag = false;
					}
					if (!flag)
					{
						component = MyComponentFactory.CreateInstanceByTypeId(component2.Component.TypeId);
					}
					component.Deserialize(component2.Component);
					if (!flag)
					{
						Add(type, component);
					}
				}
			}
		}

		public Dictionary<Type, MyComponentBase>.ValueCollection.Enumerator GetEnumerator()
		{
			return m_components.Values.GetEnumerator();
		}

		public Dictionary<Type, MyComponentBase>.KeyCollection GetComponentTypes()
		{
			return m_components.Keys;
		}
	}
}
