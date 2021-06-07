using VRage.Library.Collections;

namespace VRage.Serialization
{
	public abstract class MySerializer
	{
		protected internal abstract void Clone(ref object value, MySerializeInfo info);

		protected internal abstract bool Equals(ref object a, ref object b, MySerializeInfo info);

		protected internal abstract void Read(BitStream stream, out object value, MySerializeInfo info);

		protected internal abstract void Write(BitStream stream, object value, MySerializeInfo info);

		public static T CreateAndRead<T>(BitStream stream, MySerializeInfo serializeInfo = null)
		{
			CreateAndRead(stream, out T value, serializeInfo);
			return value;
		}

		public static void CreateAndRead<T>(BitStream stream, out T value, MySerializeInfo serializeInfo = null)
		{
			MySerializationHelpers.CreateAndRead(stream, out value, MyFactory.GetSerializer<T>(), serializeInfo ?? MySerializeInfo.Default);
		}

		public static void Write<T>(BitStream stream, ref T value, MySerializeInfo serializeInfo = null)
		{
			MySerializationHelpers.Write(stream, ref value, MyFactory.GetSerializer<T>(), serializeInfo ?? MySerializeInfo.Default);
		}

		public static bool AnyNull(object a, object b)
		{
			if (a != null)
			{
				return b == null;
			}
			return true;
		}
	}
	public abstract class MySerializer<T> : MySerializer
	{
		public static bool IsValueType => typeof(T).IsValueType;

		public static bool IsClass => !IsValueType;

		/// <summary>
		/// In-place clone.
		/// Primitive and immutable types can implements this as empty method.
		/// Reference types must create new instance a fill it's members.
		/// </summary>
		public abstract void Clone(ref T value);

		/// <summary>
		/// Tests equality.
		/// </summary>
		public abstract bool Equals(ref T a, ref T b);

		public abstract void Read(BitStream stream, out T value, MySerializeInfo info);

		public abstract void Write(BitStream stream, ref T value, MySerializeInfo info);

		protected internal sealed override void Clone(ref object value, MySerializeInfo info)
		{
			T value2 = (T)value;
			Clone(ref value2);
			value = value2;
		}

		protected internal sealed override bool Equals(ref object a, ref object b, MySerializeInfo info)
		{
			T a2 = (T)a;
			T b2 = (T)b;
			return Equals(ref a2, ref b2);
		}

		protected internal sealed override void Read(BitStream stream, out object value, MySerializeInfo info)
		{
			Read(stream, out T value2, info);
			value = value2;
		}

		protected internal sealed override void Write(BitStream stream, object value, MySerializeInfo info)
		{
			T value2 = (T)value;
			Write(stream, ref value2, info);
		}
	}
}
