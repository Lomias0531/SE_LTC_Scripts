namespace VRage.Network
{
	public interface ISyncComposer
	{
		ISyncType Compose(object instance, int id, ISerializerInfo serializeInfo);
	}
}
