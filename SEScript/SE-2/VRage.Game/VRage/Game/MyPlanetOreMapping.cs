using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetOreMapping
	{
		protected class VRage_Game_MyPlanetOreMapping_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in byte value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out byte value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003EType_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003EStart_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in float value)
			{
				owner.Start = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out float value)
			{
				value = owner.Start;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003EDepth_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in float value)
			{
				owner.Depth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out float value)
			{
				value = owner.Depth;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003EColorShift_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, ColorDefinitionRGBA?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in ColorDefinitionRGBA? value)
			{
				owner.ColorShift = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out ColorDefinitionRGBA? value)
			{
				value = owner.ColorShift;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003Em_colorInfluence_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in float? value)
			{
				owner.m_colorInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out float? value)
			{
				value = owner.m_colorInfluence;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003ETargetColor_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in string value)
			{
				owner.TargetColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out string value)
			{
				value = owner.TargetColor;
			}
		}

		protected class VRage_Game_MyPlanetOreMapping_003C_003EColorInfluence_003C_003EAccessor : IMemberAccessor<MyPlanetOreMapping, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetOreMapping owner, in float value)
			{
				owner.ColorInfluence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetOreMapping owner, out float value)
			{
				value = owner.ColorInfluence;
			}
		}

		private class VRage_Game_MyPlanetOreMapping_003C_003EActor : IActivator, IActivator<MyPlanetOreMapping>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetOreMapping();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetOreMapping CreateInstance()
			{
				return new MyPlanetOreMapping();
			}

			MyPlanetOreMapping IActivator<MyPlanetOreMapping>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(18)]
		[XmlAttribute(AttributeName = "Value")]
		public byte Value;

		[ProtoMember(19)]
		[XmlAttribute(AttributeName = "Type")]
		public string Type;

		[ProtoMember(20)]
		[XmlAttribute(AttributeName = "Start")]
		public float Start = 5f;

		[ProtoMember(21)]
		[XmlAttribute(AttributeName = "Depth")]
		public float Depth = 10f;

		[ProtoMember(22)]
		[XmlIgnore]
		public ColorDefinitionRGBA? ColorShift;

		private float? m_colorInfluence;

		[ProtoMember(23)]
		[XmlAttribute("TargetColor")]
		public string TargetColor
		{
			get
			{
				if (!ColorShift.HasValue)
				{
					return null;
				}
				return ColorShift.Value.Hex;
			}
			set
			{
				ColorShift = new ColorDefinitionRGBA(value);
			}
		}

		[ProtoMember(24)]
		[XmlAttribute("ColorInfluence")]
		public float ColorInfluence
		{
			get
			{
				return m_colorInfluence ?? 0f;
			}
			set
			{
				m_colorInfluence = value;
			}
		}

		protected bool Equals(MyPlanetOreMapping other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((MyPlanetOreMapping)obj);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}
