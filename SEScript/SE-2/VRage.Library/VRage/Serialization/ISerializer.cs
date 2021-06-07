namespace VRage.Serialization
{
	public interface ISerializer<T>
	{
		void Serialize(ByteStream destination, ref T data);

		void Deserialize(ByteStream source, out T data);
	}
}
