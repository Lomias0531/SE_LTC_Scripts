namespace VRage.Game.ObjectBuilders
{
	public static class GunObjectBuilderExtensions
	{
		public static void InitializeDeviceBase<T>(this IMyObjectBuilder_GunObject<T> gunObjectBuilder, MyObjectBuilder_DeviceBase newBuilder) where T : MyObjectBuilder_DeviceBase
		{
			if (!(newBuilder.TypeId != typeof(T)))
			{
				gunObjectBuilder.DeviceBase = newBuilder;
			}
		}

		public static T GetDevice<T>(this IMyObjectBuilder_GunObject<T> gunObjectBuilder) where T : MyObjectBuilder_DeviceBase
		{
			return gunObjectBuilder.DeviceBase as T;
		}
	}
}
