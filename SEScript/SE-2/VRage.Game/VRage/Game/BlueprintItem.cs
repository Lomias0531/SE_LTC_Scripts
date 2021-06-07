using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class BlueprintItem
	{
		protected class VRage_Game_BlueprintItem_003C_003EId_003C_003EAccessor : IMemberAccessor<BlueprintItem, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintItem owner, in SerializableDefinitionId value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintItem owner, out SerializableDefinitionId value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_BlueprintItem_003C_003EAmount_003C_003EAccessor : IMemberAccessor<BlueprintItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintItem owner, in string value)
			{
				owner.Amount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintItem owner, out string value)
			{
				value = owner.Amount;
			}
		}

		protected class VRage_Game_BlueprintItem_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<BlueprintItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintItem owner, in string value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintItem owner, out string value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_Game_BlueprintItem_003C_003ESubtypeId_003C_003EAccessor : IMemberAccessor<BlueprintItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintItem owner, in string value)
			{
				owner.SubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintItem owner, out string value)
			{
				value = owner.SubtypeId;
			}
		}

		private class VRage_Game_BlueprintItem_003C_003EActor : IActivator, IActivator<BlueprintItem>
		{
			private sealed override object CreateInstance()
			{
				return new BlueprintItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override BlueprintItem CreateInstance()
			{
				return new BlueprintItem();
			}

			BlueprintItem IActivator<BlueprintItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlIgnore]
		[ProtoMember(1)]
		public SerializableDefinitionId Id;

		/// <summary>
		/// Amount of item required or produced. For discrete objects this refers to
		/// pieces. For ingots and ore, this refers to volume in m^3.
		/// </summary>
		[XmlAttribute]
		[ProtoMember(4)]
		public string Amount;

		[XmlAttribute]
		public string TypeId
		{
			get
			{
				if (Id.TypeId.IsNull)
				{
					return "(null)";
				}
				return Id.TypeId.ToString();
			}
			set
			{
				Id.TypeId = MyObjectBuilderType.ParseBackwardsCompatible(value);
			}
		}

		[XmlAttribute]
		public string SubtypeId
		{
			get
			{
				return Id.SubtypeId;
			}
			set
			{
				Id.SubtypeId = value;
			}
		}
	}
}
