using VRage.Render.Scene.Components;

namespace VRage.Render.Scene
{
	public interface IMyActorFactory
	{
		MyLightComponent CreateLight(string debugName);

		void Destroy(MyActor actor, bool fadeOut);
	}
}
