using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ConveyorLine : MyObjectBuilder_Base
	{
		public enum LineType
		{
			DEFAULT_LINE,
			SMALL_LINE,
			LARGE_LINE
		}

		public enum LineConductivity
		{
			FULL,
			FORWARD,
			BACKWARD,
			NONE
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EStartPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in SerializableVector3I value)
			{
				owner.StartPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out SerializableVector3I value)
			{
				value = owner.StartPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EStartDirection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in Base6Directions.Direction value)
			{
				owner.StartDirection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out Base6Directions.Direction value)
			{
				value = owner.StartDirection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EEndPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in SerializableVector3I value)
			{
				owner.EndPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out SerializableVector3I value)
			{
				value = owner.EndPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EEndDirection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in Base6Directions.Direction value)
			{
				owner.EndDirection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out Base6Directions.Direction value)
			{
				value = owner.EndDirection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EPacketsForward_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, List<MyObjectBuilder_ConveyorPacket>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in List<MyObjectBuilder_ConveyorPacket> value)
			{
				owner.PacketsForward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out List<MyObjectBuilder_ConveyorPacket> value)
			{
				value = owner.PacketsForward;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EPacketsBackward_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, List<MyObjectBuilder_ConveyorPacket>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in List<MyObjectBuilder_ConveyorPacket> value)
			{
				owner.PacketsBackward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out List<MyObjectBuilder_ConveyorPacket> value)
			{
				value = owner.PacketsBackward;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003ESections_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, List<SerializableLineSectionInformation>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in List<SerializableLineSectionInformation> value)
			{
				owner.Sections = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out List<SerializableLineSectionInformation> value)
			{
				value = owner.Sections;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EConveyorLineType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, LineType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in LineType value)
			{
				owner.ConveyorLineType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out LineType value)
			{
				value = owner.ConveyorLineType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EConveyorLineConductivity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, LineConductivity>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in LineConductivity value)
			{
				owner.ConveyorLineConductivity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out LineConductivity value)
			{
				value = owner.ConveyorLineConductivity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ConveyorLine owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ConveyorLine owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ConveyorLine_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ConveyorLine>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ConveyorLine();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ConveyorLine CreateInstance()
			{
				return new MyObjectBuilder_ConveyorLine();
			}

			MyObjectBuilder_ConveyorLine IActivator<MyObjectBuilder_ConveyorLine>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableVector3I StartPosition;

		[ProtoMember(4)]
		public Base6Directions.Direction StartDirection;

		[ProtoMember(7)]
		public SerializableVector3I EndPosition;

		[ProtoMember(10)]
		public Base6Directions.Direction EndDirection;

		[ProtoMember(13)]
		public List<MyObjectBuilder_ConveyorPacket> PacketsForward = new List<MyObjectBuilder_ConveyorPacket>();

		[ProtoMember(16)]
		public List<MyObjectBuilder_ConveyorPacket> PacketsBackward = new List<MyObjectBuilder_ConveyorPacket>();

		[ProtoMember(19)]
		[DefaultValue(null)]
		[XmlArrayItem("Section")]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<SerializableLineSectionInformation> Sections;

		[ProtoMember(22)]
		[DefaultValue(LineType.DEFAULT_LINE)]
		public LineType ConveyorLineType;

		[ProtoMember(25)]
		[DefaultValue(LineConductivity.FULL)]
		public LineConductivity ConveyorLineConductivity;

		public bool ShouldSerializePacketsForward()
		{
			return PacketsForward.Count != 0;
		}

		public bool ShouldSerializePacketsBackward()
		{
			return PacketsBackward.Count != 0;
		}

		public bool ShouldSerializeSections()
		{
			return Sections != null;
		}

		public MyObjectBuilder_ConveyorLine()
		{
			Sections = new List<SerializableLineSectionInformation>();
		}
	}
}
