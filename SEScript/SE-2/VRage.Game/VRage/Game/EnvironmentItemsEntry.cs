using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class EnvironmentItemsEntry
	{
		protected class VRage_Game_EnvironmentItemsEntry_003C_003EType_003C_003EAccessor : IMemberAccessor<EnvironmentItemsEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EnvironmentItemsEntry owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EnvironmentItemsEntry owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_EnvironmentItemsEntry_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<EnvironmentItemsEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EnvironmentItemsEntry owner, in string value)
			{
				owner.Subtype = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EnvironmentItemsEntry owner, out string value)
			{
				value = owner.Subtype;
			}
		}

		protected class VRage_Game_EnvironmentItemsEntry_003C_003EItemSubtype_003C_003EAccessor : IMemberAccessor<EnvironmentItemsEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EnvironmentItemsEntry owner, in string value)
			{
				owner.ItemSubtype = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EnvironmentItemsEntry owner, out string value)
			{
				value = owner.ItemSubtype;
			}
		}

		protected class VRage_Game_EnvironmentItemsEntry_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<EnvironmentItemsEntry, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EnvironmentItemsEntry owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EnvironmentItemsEntry owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		protected class VRage_Game_EnvironmentItemsEntry_003C_003EFrequency_003C_003EAccessor : IMemberAccessor<EnvironmentItemsEntry, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref EnvironmentItemsEntry owner, in float value)
			{
				owner.Frequency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref EnvironmentItemsEntry owner, out float value)
			{
				value = owner.Frequency;
			}
		}

		private class VRage_Game_EnvironmentItemsEntry_003C_003EActor : IActivator, IActivator<EnvironmentItemsEntry>
		{
			private sealed override object CreateInstance()
			{
				return new EnvironmentItemsEntry();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override EnvironmentItemsEntry CreateInstance()
			{
				return new EnvironmentItemsEntry();
			}

			EnvironmentItemsEntry IActivator<EnvironmentItemsEntry>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public string Type;

		[ProtoMember(4)]
		[XmlAttribute]
		public string Subtype;

		[ProtoMember(7)]
		[XmlAttribute]
		public string ItemSubtype;

		[ProtoMember(10)]
		[DefaultValue(true)]
		public bool Enabled = true;

		[ProtoMember(13)]
		[XmlAttribute]
		public float Frequency = 1f;

		public override bool Equals(object other)
		{
			EnvironmentItemsEntry environmentItemsEntry = other as EnvironmentItemsEntry;
			if (environmentItemsEntry != null && environmentItemsEntry.Type.Equals(Type) && environmentItemsEntry.Subtype.Equals(Subtype))
			{
				return environmentItemsEntry.ItemSubtype.Equals(ItemSubtype);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Type.GetHashCode() * 1572869) ^ (Subtype.GetHashCode() * 49157) ^ ItemSubtype.GetHashCode();
		}
	}
}
