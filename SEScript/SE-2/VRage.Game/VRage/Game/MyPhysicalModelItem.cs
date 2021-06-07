using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyPhysicalModelItem
	{
		protected class VRage_Game_MyPhysicalModelItem_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<MyPhysicalModelItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalModelItem owner, in string value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalModelItem owner, out string value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_Game_MyPhysicalModelItem_003C_003ESubtypeId_003C_003EAccessor : IMemberAccessor<MyPhysicalModelItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalModelItem owner, in string value)
			{
				owner.SubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalModelItem owner, out string value)
			{
				value = owner.SubtypeId;
			}
		}

		protected class VRage_Game_MyPhysicalModelItem_003C_003EWeight_003C_003EAccessor : IMemberAccessor<MyPhysicalModelItem, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPhysicalModelItem owner, in float value)
			{
				owner.Weight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPhysicalModelItem owner, out float value)
			{
				value = owner.Weight;
			}
		}

		private class VRage_Game_MyPhysicalModelItem_003C_003EActor : IActivator, IActivator<MyPhysicalModelItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicalModelItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicalModelItem CreateInstance()
			{
				return new MyPhysicalModelItem();
			}

			MyPhysicalModelItem IActivator<MyPhysicalModelItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute(AttributeName = "TypeId")]
		public string TypeId;

		[ProtoMember(4)]
		[XmlAttribute(AttributeName = "SubtypeId")]
		public string SubtypeId;

		[ProtoMember(7)]
		[XmlAttribute(AttributeName = "Weight")]
		public float Weight = 1f;
	}
}
