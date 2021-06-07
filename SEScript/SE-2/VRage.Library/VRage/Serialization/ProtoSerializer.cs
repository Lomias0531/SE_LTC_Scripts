using ProtoBuf.Meta;

namespace VRage.Serialization
{
	public class ProtoSerializer<T> : ISerializer<T>
	{
		public readonly TypeModel Model;

		public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>();

		public ProtoSerializer(TypeModel model = null)
		{
			Model = (model ?? TypeModel.Default);
		}

		public void Serialize(ByteStream destination, ref T data)
		{
			Model.Serialize(destination, data);
		}

		public void Deserialize(ByteStream source, out T data)
		{
			data = (T)Model.Deserialize(source, null, typeof(T));
		}
	}
}
