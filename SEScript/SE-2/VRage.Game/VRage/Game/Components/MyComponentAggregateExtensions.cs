using System.Collections.Generic;

namespace VRage.Game.Components
{
	public static class MyComponentAggregateExtensions
	{
		public static void AddComponent(this IMyComponentAggregate aggregate, MyComponentBase component)
		{
			if (component.ContainerBase != null)
			{
				component.OnBeforeRemovedFromContainer();
			}
			aggregate.ChildList.AddComponent(component);
			component.SetContainer(aggregate.ContainerBase);
			aggregate.AfterComponentAdd(component);
		}

		/// <summary>
		/// Adds to list but doesn't change ownership
		/// </summary>
		public static void AttachComponent(this IMyComponentAggregate aggregate, MyComponentBase component)
		{
			aggregate.ChildList.AddComponent(component);
		}

		public static bool RemoveComponent(this IMyComponentAggregate aggregate, MyComponentBase component)
		{
			int componentIndex = aggregate.ChildList.GetComponentIndex(component);
			if (componentIndex != -1)
			{
				aggregate.BeforeComponentRemove(component);
				component.SetContainer(null);
				aggregate.ChildList.RemoveComponentAt(componentIndex);
				return true;
			}
			foreach (MyComponentBase item in aggregate.ChildList.Reader)
			{
				IMyComponentAggregate myComponentAggregate = item as IMyComponentAggregate;
				if (myComponentAggregate != null && myComponentAggregate.RemoveComponent(component))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes from list, but doesn't change ownership
		/// </summary>
		public static void DetachComponent(this IMyComponentAggregate aggregate, MyComponentBase component)
		{
			int componentIndex = aggregate.ChildList.GetComponentIndex(component);
			if (componentIndex != -1)
			{
				aggregate.ChildList.RemoveComponentAt(componentIndex);
			}
		}

		public static void GetComponentsFlattened(this IMyComponentAggregate aggregate, List<MyComponentBase> output)
		{
			foreach (MyComponentBase item in aggregate.ChildList.Reader)
			{
				IMyComponentAggregate myComponentAggregate = item as IMyComponentAggregate;
				if (myComponentAggregate != null)
				{
					myComponentAggregate.GetComponentsFlattened(output);
				}
				else
				{
					output.Add(item);
				}
			}
		}
	}
}
