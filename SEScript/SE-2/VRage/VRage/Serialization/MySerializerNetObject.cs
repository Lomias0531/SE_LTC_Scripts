using System;
using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
	public static class MySerializerNetObject
	{
		public struct ResolverToken : IDisposable
		{
			private INetObjectResolver m_previousResolver;

			public ResolverToken(INetObjectResolver newResolver)
			{
				m_previousResolver = m_netObjectResolver;
				m_netObjectResolver = newResolver;
			}

			public void Dispose()
			{
				m_netObjectResolver = m_previousResolver;
				m_previousResolver = null;
			}
		}

		private static INetObjectResolver m_netObjectResolver;

		public static INetObjectResolver NetObjectResolver => m_netObjectResolver;

		public static ResolverToken Using(INetObjectResolver netObjectResolver)
		{
			return new ResolverToken(netObjectResolver);
		}
	}
	public class MySerializerNetObject<T> : MySerializer<T> where T : class, IMyNetObject
	{
		public override void Clone(ref T value)
		{
			throw new NotSupportedException();
		}

		public override bool Equals(ref T a, ref T b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out T value, MySerializeInfo info)
		{
			value = null;
			MySerializerNetObject.NetObjectResolver.Resolve(stream, ref value);
		}

		public override void Write(BitStream stream, ref T value, MySerializeInfo info)
		{
			MySerializerNetObject.NetObjectResolver.Resolve(stream, ref value);
		}
	}
}
