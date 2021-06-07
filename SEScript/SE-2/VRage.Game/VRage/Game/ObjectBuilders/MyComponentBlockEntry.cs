using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	public class MyComponentBlockEntry
	{
		protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry_003C_003EType_003C_003EAccessor : IMemberAccessor<MyComponentBlockEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyComponentBlockEntry owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyComponentBlockEntry owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<MyComponentBlockEntry, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyComponentBlockEntry owner, in string value)
			{
				owner.Subtype = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyComponentBlockEntry owner, out string value)
			{
				value = owner.Subtype;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry_003C_003EMain_003C_003EAccessor : IMemberAccessor<MyComponentBlockEntry, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyComponentBlockEntry owner, in bool value)
			{
				owner.Main = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyComponentBlockEntry owner, out bool value)
			{
				value = owner.Main;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<MyComponentBlockEntry, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyComponentBlockEntry owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyComponentBlockEntry owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		private class VRage_Game_ObjectBuilders_MyComponentBlockEntry_003C_003EActor : IActivator, IActivator<MyComponentBlockEntry>
		{
			private sealed override object CreateInstance()
			{
				return new MyComponentBlockEntry();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyComponentBlockEntry CreateInstance()
			{
				return new MyComponentBlockEntry();
			}

			MyComponentBlockEntry IActivator<MyComponentBlockEntry>.CreateInstance()
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

		/// <summary>
		/// Whether the given block should be used when spawning the component which it contains
		/// </summary>
		[ProtoMember(7)]
		[XmlAttribute]
		public bool Main = true;

		[ProtoMember(10)]
		[DefaultValue(true)]
		[XmlAttribute]
		public bool Enabled = true;
	}
}
