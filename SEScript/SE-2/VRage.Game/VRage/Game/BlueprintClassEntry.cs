using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class BlueprintClassEntry
	{
		protected class VRage_Game_BlueprintClassEntry_003C_003EClass_003C_003EAccessor : IMemberAccessor<BlueprintClassEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintClassEntry owner, in string value)
			{
				owner.Class = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintClassEntry owner, out string value)
			{
				value = owner.Class;
			}
		}

		protected class VRage_Game_BlueprintClassEntry_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<BlueprintClassEntry, MyObjectBuilderType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintClassEntry owner, in MyObjectBuilderType value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintClassEntry owner, out MyObjectBuilderType value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_Game_BlueprintClassEntry_003C_003EBlueprintSubtypeId_003C_003EAccessor : IMemberAccessor<BlueprintClassEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintClassEntry owner, in string value)
			{
				owner.BlueprintSubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintClassEntry owner, out string value)
			{
				value = owner.BlueprintSubtypeId;
			}
		}

		protected class VRage_Game_BlueprintClassEntry_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<BlueprintClassEntry, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintClassEntry owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintClassEntry owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		protected class VRage_Game_BlueprintClassEntry_003C_003EBlueprintTypeId_003C_003EAccessor : IMemberAccessor<BlueprintClassEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BlueprintClassEntry owner, in string value)
			{
				owner.BlueprintTypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BlueprintClassEntry owner, out string value)
			{
				value = owner.BlueprintTypeId;
			}
		}

		private class VRage_Game_BlueprintClassEntry_003C_003EActor : IActivator, IActivator<BlueprintClassEntry>
		{
			private sealed override object CreateInstance()
			{
				return new BlueprintClassEntry();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override BlueprintClassEntry CreateInstance()
			{
				return new BlueprintClassEntry();
			}

			BlueprintClassEntry IActivator<BlueprintClassEntry>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public string Class;

		[XmlIgnore]
		public MyObjectBuilderType TypeId;

		[ProtoMember(7)]
		[XmlAttribute]
		public string BlueprintSubtypeId;

		[ProtoMember(10)]
		[DefaultValue(true)]
		public bool Enabled = true;

		[ProtoMember(4)]
		[XmlAttribute]
		public string BlueprintTypeId
		{
			get
			{
				if (!TypeId.IsNull)
				{
					return TypeId.ToString();
				}
				return "(null)";
			}
			set
			{
				TypeId = MyObjectBuilderType.ParseBackwardsCompatible(value);
			}
		}

		public override bool Equals(object other)
		{
			BlueprintClassEntry blueprintClassEntry = other as BlueprintClassEntry;
			if (blueprintClassEntry != null && blueprintClassEntry.Class.Equals(Class))
			{
				return blueprintClassEntry.BlueprintSubtypeId.Equals(BlueprintSubtypeId);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Class.GetHashCode() * 7607 + BlueprintSubtypeId.GetHashCode();
		}
	}
}
