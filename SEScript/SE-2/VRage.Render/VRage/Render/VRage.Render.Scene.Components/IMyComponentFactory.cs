using System;

namespace VRage.Render.Scene.Components
{
	public interface IMyComponentFactory
	{
		MyActorComponent Create(Type type);

		void Deallocate(MyActorComponent item);
	}
}
