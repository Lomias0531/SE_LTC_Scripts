using VRage.Render.Scene.Components;

namespace VRageRender.Messages
{
	public class FloatData : VolatileComponentData
	{
		public float Float;

		public static void Update<TComponent>(uint actorId, float data) where TComponent : MyRenderDirectComponent
		{
			MyRenderProxy.UpdateRenderComponent(actorId, data, delegate(FloatData message, float dataF)
			{
				message.Float = dataF;
				message.SetComponent<TComponent>();
			});
		}

		public static implicit operator float(FloatData data)
		{
			return data.Float;
		}
	}
}
