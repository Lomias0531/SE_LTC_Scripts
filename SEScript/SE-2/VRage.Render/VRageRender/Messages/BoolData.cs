using VRage.Render.Scene.Components;

namespace VRageRender.Messages
{
	public class BoolData : VolatileComponentData
	{
		public bool Bool;

		public static void Update<TComponent>(uint actorId, bool data) where TComponent : MyRenderDirectComponent
		{
			MyRenderProxy.UpdateRenderComponent(actorId, data, delegate(BoolData message, bool dataB)
			{
				message.Bool = dataB;
				message.SetComponent<TComponent>();
			});
		}

		public static implicit operator bool(BoolData data)
		{
			return data.Bool;
		}
	}
}
