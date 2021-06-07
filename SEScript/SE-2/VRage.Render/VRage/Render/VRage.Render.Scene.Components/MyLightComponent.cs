using VRageRender.Messages;

namespace VRage.Render.Scene.Components
{
	public abstract class MyLightComponent : MyActorComponent
	{
		protected UpdateRenderLightData m_data;

		protected UpdateRenderLightData m_originalData;

		public UpdateRenderLightData Data => m_originalData;

		public abstract void UpdateData(ref UpdateRenderLightData data);
	}
}
