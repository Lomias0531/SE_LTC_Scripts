namespace VRageRender.Messages
{
	public class VolatileComponentData : UpdateData
	{
		public VolatileComponentData()
			: base(null)
		{
		}

		public void SetComponent<T>()
		{
			base.ComponentType = typeof(T);
		}
	}
}
