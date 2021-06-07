namespace VRage.Game.Components
{
	public interface IMyComponentBase
	{
		void SetContainer(IMyComponentContainer container);

		void OnAddedToContainer();

		void OnRemovedFromContainer();

		void OnAddedToScene();

		void OnRemovedFromScene();
	}
}
