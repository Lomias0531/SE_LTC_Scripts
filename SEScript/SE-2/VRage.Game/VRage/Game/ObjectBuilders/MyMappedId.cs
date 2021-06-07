using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	public class MyMappedId
	{
		protected class VRage_Game_ObjectBuilders_MyMappedId_003C_003EGroup_003C_003EAccessor : IMemberAccessor<MyMappedId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMappedId owner, in string value)
			{
				owner.Group = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMappedId owner, out string value)
			{
				value = owner.Group;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyMappedId_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<MyMappedId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMappedId owner, in string value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMappedId owner, out string value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyMappedId_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<MyMappedId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMappedId owner, in string value)
			{
				owner.SubtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMappedId owner, out string value)
			{
				value = owner.SubtypeName;
			}
		}

		private class VRage_Game_ObjectBuilders_MyMappedId_003C_003EActor : IActivator, IActivator<MyMappedId>
		{
			private sealed override object CreateInstance()
			{
				return new MyMappedId();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMappedId CreateInstance()
			{
				return new MyMappedId();
			}

			MyMappedId IActivator<MyMappedId>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public string Group;

		[ProtoMember(4)]
		[XmlAttribute]
		public string TypeId;

		[ProtoMember(7)]
		[XmlAttribute]
		public string SubtypeName;

		[XmlIgnore]
		public MyStringHash GroupId => MyStringHash.GetOrCompute(Group);

		[XmlIgnore]
		public MyStringHash SubtypeId => MyStringHash.GetOrCompute(SubtypeName);
	}
}
