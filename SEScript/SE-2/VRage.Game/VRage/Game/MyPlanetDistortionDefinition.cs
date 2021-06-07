using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetDistortionDefinition
	{
		protected class VRage_Game_MyPlanetDistortionDefinition_003C_003EType_003C_003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetDistortionDefinition owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetDistortionDefinition owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyPlanetDistortionDefinition_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetDistortionDefinition owner, in byte value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetDistortionDefinition owner, out byte value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_MyPlanetDistortionDefinition_003C_003EFrequency_003C_003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetDistortionDefinition owner, in float value)
			{
				owner.Frequency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetDistortionDefinition owner, out float value)
			{
				value = owner.Frequency;
			}
		}

		protected class VRage_Game_MyPlanetDistortionDefinition_003C_003EHeight_003C_003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetDistortionDefinition owner, in float value)
			{
				owner.Height = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetDistortionDefinition owner, out float value)
			{
				value = owner.Height;
			}
		}

		protected class VRage_Game_MyPlanetDistortionDefinition_003C_003ELayerCount_003C_003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetDistortionDefinition owner, in int value)
			{
				owner.LayerCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetDistortionDefinition owner, out int value)
			{
				value = owner.LayerCount;
			}
		}

		private class VRage_Game_MyPlanetDistortionDefinition_003C_003EActor : IActivator, IActivator<MyPlanetDistortionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetDistortionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetDistortionDefinition CreateInstance()
			{
				return new MyPlanetDistortionDefinition();
			}

			MyPlanetDistortionDefinition IActivator<MyPlanetDistortionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(38)]
		[XmlAttribute(AttributeName = "Type")]
		public string Type;

		[ProtoMember(39)]
		[XmlAttribute(AttributeName = "Value")]
		public byte Value;

		[ProtoMember(40)]
		[XmlAttribute(AttributeName = "Frequency")]
		public float Frequency = 1f;

		[ProtoMember(41)]
		[XmlAttribute(AttributeName = "Height")]
		public float Height = 1f;

		[ProtoMember(42)]
		[XmlAttribute(AttributeName = "LayerCount")]
		public int LayerCount = 1;
	}
}
