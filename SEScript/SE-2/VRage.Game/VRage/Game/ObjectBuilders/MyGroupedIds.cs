using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	public class MyGroupedIds
	{
		[ProtoContract]
		public struct GroupedId
		{
			protected class VRage_Game_ObjectBuilders_MyGroupedIds_003C_003EGroupedId_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<GroupedId, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GroupedId owner, in string value)
				{
					owner.TypeId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GroupedId owner, out string value)
				{
					value = owner.TypeId;
				}
			}

			protected class VRage_Game_ObjectBuilders_MyGroupedIds_003C_003EGroupedId_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<GroupedId, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GroupedId owner, in string value)
				{
					owner.SubtypeName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GroupedId owner, out string value)
				{
					value = owner.SubtypeName;
				}
			}

			private class VRage_Game_ObjectBuilders_MyGroupedIds_003C_003EGroupedId_003C_003EActor : IActivator, IActivator<GroupedId>
			{
				private sealed override object CreateInstance()
				{
					return default(GroupedId);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override GroupedId CreateInstance()
				{
					return (GroupedId)(object)default(GroupedId);
				}

				GroupedId IActivator<GroupedId>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[XmlAttribute]
			public string TypeId;

			[ProtoMember(4)]
			[XmlAttribute]
			public string SubtypeName;

			[XmlIgnore]
			public MyStringHash SubtypeId => MyStringHash.GetOrCompute(SubtypeName);
		}

		protected class VRage_Game_ObjectBuilders_MyGroupedIds_003C_003ETag_003C_003EAccessor : IMemberAccessor<MyGroupedIds, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGroupedIds owner, in string value)
			{
				owner.Tag = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGroupedIds owner, out string value)
			{
				value = owner.Tag;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyGroupedIds_003C_003EEntries_003C_003EAccessor : IMemberAccessor<MyGroupedIds, GroupedId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGroupedIds owner, in GroupedId[] value)
			{
				owner.Entries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGroupedIds owner, out GroupedId[] value)
			{
				value = owner.Entries;
			}
		}

		private class VRage_Game_ObjectBuilders_MyGroupedIds_003C_003EActor : IActivator, IActivator<MyGroupedIds>
		{
			private sealed override object CreateInstance()
			{
				return new MyGroupedIds();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGroupedIds CreateInstance()
			{
				return new MyGroupedIds();
			}

			MyGroupedIds IActivator<MyGroupedIds>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		[XmlAttribute]
		public string Tag;

		[ProtoMember(10)]
		[DefaultValue(null)]
		[XmlArrayItem("GroupEntry")]
		public GroupedId[] Entries;
	}
}
