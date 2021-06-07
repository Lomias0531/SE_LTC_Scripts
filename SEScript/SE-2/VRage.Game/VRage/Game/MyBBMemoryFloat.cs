using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("MyBBMemoryFloat")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBBMemoryFloat : MyBBMemoryValue
	{
		protected class VRage_Game_MyBBMemoryFloat_003C_003EFloatValue_003C_003EAccessor : IMemberAccessor<MyBBMemoryFloat, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryFloat owner, in float value)
			{
				owner.FloatValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryFloat owner, out float value)
			{
				value = owner.FloatValue;
			}
		}

		private class VRage_Game_MyBBMemoryFloat_003C_003EActor : IActivator, IActivator<MyBBMemoryFloat>
		{
			private sealed override object CreateInstance()
			{
				return new MyBBMemoryFloat();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBBMemoryFloat CreateInstance()
			{
				return new MyBBMemoryFloat();
			}

			MyBBMemoryFloat IActivator<MyBBMemoryFloat>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float FloatValue;
	}
}
