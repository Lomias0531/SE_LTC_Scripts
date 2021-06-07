using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Data.Audio;
using VRage.Network;

namespace VRage
{
	[ProtoContract]
	[XmlType("Wave")]
	public sealed class MyAudioWave
	{
		protected class VRage_MyAudioWave_003C_003EType_003C_003EAccessor : IMemberAccessor<MyAudioWave, MySoundDimensions>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAudioWave owner, in MySoundDimensions value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAudioWave owner, out MySoundDimensions value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_MyAudioWave_003C_003EStart_003C_003EAccessor : IMemberAccessor<MyAudioWave, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAudioWave owner, in string value)
			{
				owner.Start = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAudioWave owner, out string value)
			{
				value = owner.Start;
			}
		}

		protected class VRage_MyAudioWave_003C_003ELoop_003C_003EAccessor : IMemberAccessor<MyAudioWave, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAudioWave owner, in string value)
			{
				owner.Loop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAudioWave owner, out string value)
			{
				value = owner.Loop;
			}
		}

		protected class VRage_MyAudioWave_003C_003EEnd_003C_003EAccessor : IMemberAccessor<MyAudioWave, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAudioWave owner, in string value)
			{
				owner.End = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAudioWave owner, out string value)
			{
				value = owner.End;
			}
		}

		[ProtoMember(10)]
		[XmlAttribute]
		public MySoundDimensions Type;

		[ProtoMember(13)]
		[DefaultValue("")]
		[ModdableContentFile(new string[]
		{
			"xwm",
			"wav"
		})]
		public string Start;

		[ProtoMember(16)]
		[DefaultValue("")]
		[ModdableContentFile(new string[]
		{
			"xwm",
			"wav"
		})]
		public string Loop;

		[ProtoMember(19)]
		[DefaultValue("")]
		[ModdableContentFile(new string[]
		{
			"xwm",
			"wav"
		})]
		public string End;
	}
}
