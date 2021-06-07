namespace VRage.Game.Components
{
	/// <summary>
	/// When creating a new aggregate component type, derive from this interface so that you can use extension methods
	/// AddComponent and RemoveComponent
	/// </summary>
	public interface IMyComponentAggregate
	{
		MyAggregateComponentList ChildList
		{
			get;
		}

		MyComponentContainer ContainerBase
		{
			get;
		}

		void AfterComponentAdd(MyComponentBase component);

		void BeforeComponentRemove(MyComponentBase component);
	}
}
