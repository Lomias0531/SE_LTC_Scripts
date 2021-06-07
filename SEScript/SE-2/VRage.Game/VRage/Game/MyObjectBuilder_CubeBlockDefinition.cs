using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CubeBlockDefinition : MyObjectBuilder_PhysicalModelDefinition
	{
		[ProtoContract]
		public class MountPoint
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003ESide_003C_003EAccessor : IMemberAccessor<MountPoint, BlockSideEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in BlockSideEnum value)
				{
					owner.Side = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out BlockSideEnum value)
				{
					value = owner.Side;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EStart_003C_003EAccessor : IMemberAccessor<MountPoint, SerializableVector2>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in SerializableVector2 value)
				{
					owner.Start = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out SerializableVector2 value)
				{
					value = owner.Start;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EEnd_003C_003EAccessor : IMemberAccessor<MountPoint, SerializableVector2>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in SerializableVector2 value)
				{
					owner.End = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out SerializableVector2 value)
				{
					value = owner.End;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EExclusionMask_003C_003EAccessor : IMemberAccessor<MountPoint, byte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in byte value)
				{
					owner.ExclusionMask = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out byte value)
				{
					value = owner.ExclusionMask;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EPropertiesMask_003C_003EAccessor : IMemberAccessor<MountPoint, byte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in byte value)
				{
					owner.PropertiesMask = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out byte value)
				{
					value = owner.PropertiesMask;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<MountPoint, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in bool value)
				{
					owner.Enabled = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out bool value)
				{
					value = owner.Enabled;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EDefault_003C_003EAccessor : IMemberAccessor<MountPoint, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in bool value)
				{
					owner.Default = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out bool value)
				{
					value = owner.Default;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EStartX_003C_003EAccessor : IMemberAccessor<MountPoint, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in float value)
				{
					owner.StartX = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out float value)
				{
					value = owner.StartX;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EStartY_003C_003EAccessor : IMemberAccessor<MountPoint, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in float value)
				{
					owner.StartY = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out float value)
				{
					value = owner.StartY;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EEndX_003C_003EAccessor : IMemberAccessor<MountPoint, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in float value)
				{
					owner.EndX = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out float value)
				{
					value = owner.EndX;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EEndY_003C_003EAccessor : IMemberAccessor<MountPoint, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MountPoint owner, in float value)
				{
					owner.EndY = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MountPoint owner, out float value)
				{
					value = owner.EndY;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoint_003C_003EActor : IActivator, IActivator<MountPoint>
			{
				private sealed override object CreateInstance()
				{
					return new MountPoint();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MountPoint CreateInstance()
				{
					return new MountPoint();
				}

				MountPoint IActivator<MountPoint>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(3)]
			public BlockSideEnum Side;

			[XmlIgnore]
			[ProtoMember(4)]
			public SerializableVector2 Start;

			[XmlIgnore]
			[ProtoMember(5)]
			public SerializableVector2 End;

			[XmlAttribute]
			[ProtoMember(6)]
			[DefaultValue(0)]
			public byte ExclusionMask;

			[XmlAttribute]
			[ProtoMember(7)]
			[DefaultValue(0)]
			public byte PropertiesMask;

			[XmlAttribute]
			[ProtoMember(8)]
			[DefaultValue(true)]
			public bool Enabled = true;

			[XmlAttribute]
			[ProtoMember(9)]
			[DefaultValue(false)]
			public bool Default;

			[XmlAttribute]
			public float StartX
			{
				get
				{
					return Start.X;
				}
				set
				{
					Start.X = value;
				}
			}

			[XmlAttribute]
			public float StartY
			{
				get
				{
					return Start.Y;
				}
				set
				{
					Start.Y = value;
				}
			}

			[XmlAttribute]
			public float EndX
			{
				get
				{
					return End.X;
				}
				set
				{
					End.X = value;
				}
			}

			[XmlAttribute]
			public float EndY
			{
				get
				{
					return End.Y;
				}
				set
				{
					End.Y = value;
				}
			}
		}

		[ProtoContract]
		public class CubeBlockComponent
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockComponent_003C_003EType_003C_003EAccessor : IMemberAccessor<CubeBlockComponent, MyObjectBuilderType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockComponent owner, in MyObjectBuilderType value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockComponent owner, out MyObjectBuilderType value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockComponent_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<CubeBlockComponent, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockComponent owner, in string value)
				{
					owner.Subtype = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockComponent owner, out string value)
				{
					value = owner.Subtype;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockComponent_003C_003ECount_003C_003EAccessor : IMemberAccessor<CubeBlockComponent, ushort>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockComponent owner, in ushort value)
				{
					owner.Count = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockComponent owner, out ushort value)
				{
					value = owner.Count;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockComponent_003C_003EDeconstructId_003C_003EAccessor : IMemberAccessor<CubeBlockComponent, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockComponent owner, in SerializableDefinitionId value)
				{
					owner.DeconstructId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockComponent owner, out SerializableDefinitionId value)
				{
					value = owner.DeconstructId;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockComponent_003C_003EActor : IActivator, IActivator<CubeBlockComponent>
			{
				private sealed override object CreateInstance()
				{
					return new CubeBlockComponent();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CubeBlockComponent CreateInstance()
				{
					return new CubeBlockComponent();
				}

				CubeBlockComponent IActivator<CubeBlockComponent>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlIgnore]
			public MyObjectBuilderType Type = typeof(MyObjectBuilder_Component);

			[XmlAttribute]
			[ProtoMember(10)]
			public string Subtype;

			[XmlAttribute]
			[ProtoMember(11)]
			public ushort Count;

			[ProtoMember(12)]
			public SerializableDefinitionId DeconstructId;
		}

		[ProtoContract]
		public class CriticalPart
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalPart_003C_003EType_003C_003EAccessor : IMemberAccessor<CriticalPart, MyObjectBuilderType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CriticalPart owner, in MyObjectBuilderType value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CriticalPart owner, out MyObjectBuilderType value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalPart_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<CriticalPart, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CriticalPart owner, in string value)
				{
					owner.Subtype = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CriticalPart owner, out string value)
				{
					value = owner.Subtype;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalPart_003C_003EIndex_003C_003EAccessor : IMemberAccessor<CriticalPart, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CriticalPart owner, in int value)
				{
					owner.Index = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CriticalPart owner, out int value)
				{
					value = owner.Index;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalPart_003C_003EActor : IActivator, IActivator<CriticalPart>
			{
				private sealed override object CreateInstance()
				{
					return new CriticalPart();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CriticalPart CreateInstance()
				{
					return new CriticalPart();
				}

				CriticalPart IActivator<CriticalPart>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlIgnore]
			public MyObjectBuilderType Type = typeof(MyObjectBuilder_Component);

			[XmlAttribute]
			[ProtoMember(13)]
			public string Subtype;

			[XmlAttribute]
			[ProtoMember(14)]
			public int Index;
		}

		[ProtoContract]
		public class Variant
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariant_003C_003EColor_003C_003EAccessor : IMemberAccessor<Variant, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Variant owner, in string value)
				{
					owner.Color = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Variant owner, out string value)
				{
					value = owner.Color;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariant_003C_003ESuffix_003C_003EAccessor : IMemberAccessor<Variant, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Variant owner, in string value)
				{
					owner.Suffix = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Variant owner, out string value)
				{
					value = owner.Suffix;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariant_003C_003EActor : IActivator, IActivator<Variant>
			{
				private sealed override object CreateInstance()
				{
					return new Variant();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Variant CreateInstance()
				{
					return new Variant();
				}

				Variant IActivator<Variant>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			/// <summary>
			/// Color is used to get Color(4 bytes) as well as
			/// MyStringId value for localization.
			/// </summary>
			[XmlAttribute]
			[ProtoMember(15)]
			public string Color;

			[XmlAttribute]
			[ProtoMember(16)]
			public string Suffix;
		}

		[ProtoContract]
		public class PatternDefinition
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPatternDefinition_003C_003ECubeTopology_003C_003EAccessor : IMemberAccessor<PatternDefinition, MyCubeTopology>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PatternDefinition owner, in MyCubeTopology value)
				{
					owner.CubeTopology = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PatternDefinition owner, out MyCubeTopology value)
				{
					value = owner.CubeTopology;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPatternDefinition_003C_003ESides_003C_003EAccessor : IMemberAccessor<PatternDefinition, Side[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PatternDefinition owner, in Side[] value)
				{
					owner.Sides = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PatternDefinition owner, out Side[] value)
				{
					value = owner.Sides;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPatternDefinition_003C_003EShowEdges_003C_003EAccessor : IMemberAccessor<PatternDefinition, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PatternDefinition owner, in bool value)
				{
					owner.ShowEdges = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PatternDefinition owner, out bool value)
				{
					value = owner.ShowEdges;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPatternDefinition_003C_003EActor : IActivator, IActivator<PatternDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new PatternDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override PatternDefinition CreateInstance()
				{
					return new PatternDefinition();
				}

				PatternDefinition IActivator<PatternDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(17)]
			public MyCubeTopology CubeTopology;

			[ProtoMember(18)]
			public Side[] Sides;

			[ProtoMember(19)]
			public bool ShowEdges;
		}

		[ProtoContract]
		public class Side
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EModel_003C_003EAccessor : IMemberAccessor<Side, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Side owner, in string value)
				{
					owner.Model = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Side owner, out string value)
				{
					value = owner.Model;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EPatternSize_003C_003EAccessor : IMemberAccessor<Side, SerializableVector2I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Side owner, in SerializableVector2I value)
				{
					owner.PatternSize = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Side owner, out SerializableVector2I value)
				{
					value = owner.PatternSize;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EScaleTileU_003C_003EAccessor : IMemberAccessor<Side, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Side owner, in int value)
				{
					owner.ScaleTileU = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Side owner, out int value)
				{
					value = owner.ScaleTileU;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EScaleTileV_003C_003EAccessor : IMemberAccessor<Side, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Side owner, in int value)
				{
					owner.ScaleTileV = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Side owner, out int value)
				{
					value = owner.ScaleTileV;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EPatternWidth_003C_003EAccessor : IMemberAccessor<Side, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Side owner, in int value)
				{
					owner.PatternWidth = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Side owner, out int value)
				{
					value = owner.PatternWidth;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EPatternHeight_003C_003EAccessor : IMemberAccessor<Side, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Side owner, in int value)
				{
					owner.PatternHeight = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Side owner, out int value)
				{
					value = owner.PatternHeight;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESide_003C_003EActor : IActivator, IActivator<Side>
			{
				private sealed override object CreateInstance()
				{
					return new Side();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Side CreateInstance()
				{
					return new Side();
				}

				Side IActivator<Side>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(20)]
			[ModdableContentFile("mwm")]
			public string Model;

			[XmlIgnore]
			[ProtoMember(21)]
			public SerializableVector2I PatternSize;

			[XmlAttribute]
			public int ScaleTileU = 1;

			[XmlAttribute]
			public int ScaleTileV = 1;

			[XmlAttribute]
			public int PatternWidth
			{
				get
				{
					return PatternSize.X;
				}
				set
				{
					PatternSize.X = value;
				}
			}

			[XmlAttribute]
			public int PatternHeight
			{
				get
				{
					return PatternSize.Y;
				}
				set
				{
					PatternSize.Y = value;
				}
			}
		}

		[ProtoContract]
		public class BuildProgressModel
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModel_003C_003EBuildPercentUpperBound_003C_003EAccessor : IMemberAccessor<BuildProgressModel, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildProgressModel owner, in float value)
				{
					owner.BuildPercentUpperBound = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildProgressModel owner, out float value)
				{
					value = owner.BuildPercentUpperBound;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModel_003C_003EFile_003C_003EAccessor : IMemberAccessor<BuildProgressModel, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildProgressModel owner, in string value)
				{
					owner.File = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildProgressModel owner, out string value)
				{
					value = owner.File;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModel_003C_003ERandomOrientation_003C_003EAccessor : IMemberAccessor<BuildProgressModel, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildProgressModel owner, in bool value)
				{
					owner.RandomOrientation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildProgressModel owner, out bool value)
				{
					value = owner.RandomOrientation;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModel_003C_003EMountPoints_003C_003EAccessor : IMemberAccessor<BuildProgressModel, MountPoint[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildProgressModel owner, in MountPoint[] value)
				{
					owner.MountPoints = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildProgressModel owner, out MountPoint[] value)
				{
					value = owner.MountPoints;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModel_003C_003EVisible_003C_003EAccessor : IMemberAccessor<BuildProgressModel, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildProgressModel owner, in bool value)
				{
					owner.Visible = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildProgressModel owner, out bool value)
				{
					value = owner.Visible;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModel_003C_003EActor : IActivator, IActivator<BuildProgressModel>
			{
				private sealed override object CreateInstance()
				{
					return new BuildProgressModel();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BuildProgressModel CreateInstance()
				{
					return new BuildProgressModel();
				}

				BuildProgressModel IActivator<BuildProgressModel>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(22)]
			public float BuildPercentUpperBound;

			[XmlAttribute]
			[ProtoMember(23)]
			[ModdableContentFile("mwm")]
			public string File;

			[XmlAttribute]
			[ProtoMember(24)]
			[DefaultValue(false)]
			public bool RandomOrientation;

			[ProtoMember(25)]
			[XmlArray("MountPointOverrides")]
			[XmlArrayItem("MountPoint")]
			[DefaultValue(null)]
			public MountPoint[] MountPoints;

			[XmlAttribute]
			[ProtoMember(26)]
			[DefaultValue(true)]
			public bool Visible = true;
		}

		[ProtoContract]
		public class MySubBlockDefinition
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMySubBlockDefinition_003C_003ESubBlock_003C_003EAccessor : IMemberAccessor<MySubBlockDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySubBlockDefinition owner, in string value)
				{
					owner.SubBlock = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySubBlockDefinition owner, out string value)
				{
					value = owner.SubBlock;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMySubBlockDefinition_003C_003EId_003C_003EAccessor : IMemberAccessor<MySubBlockDefinition, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySubBlockDefinition owner, in SerializableDefinitionId value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySubBlockDefinition owner, out SerializableDefinitionId value)
				{
					value = owner.Id;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMySubBlockDefinition_003C_003EActor : IActivator, IActivator<MySubBlockDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new MySubBlockDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MySubBlockDefinition CreateInstance()
				{
					return new MySubBlockDefinition();
				}

				MySubBlockDefinition IActivator<MySubBlockDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(27)]
			public string SubBlock;

			[ProtoMember(28)]
			public SerializableDefinitionId Id;
		}

		[ProtoContract]
		public class EntityComponentDefinition
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponentDefinition_003C_003EComponentType_003C_003EAccessor : IMemberAccessor<EntityComponentDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EntityComponentDefinition owner, in string value)
				{
					owner.ComponentType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EntityComponentDefinition owner, out string value)
				{
					value = owner.ComponentType;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponentDefinition_003C_003EBuilderType_003C_003EAccessor : IMemberAccessor<EntityComponentDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref EntityComponentDefinition owner, in string value)
				{
					owner.BuilderType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref EntityComponentDefinition owner, out string value)
				{
					value = owner.BuilderType;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponentDefinition_003C_003EActor : IActivator, IActivator<EntityComponentDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new EntityComponentDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override EntityComponentDefinition CreateInstance()
				{
					return new EntityComponentDefinition();
				}

				EntityComponentDefinition IActivator<EntityComponentDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(29)]
			public string ComponentType;

			[XmlAttribute]
			[ProtoMember(30)]
			public string BuilderType;
		}

		[ProtoContract]
		public class CubeBlockEffectBase
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffectBase_003C_003EName_003C_003EAccessor : IMemberAccessor<CubeBlockEffectBase, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffectBase owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffectBase owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffectBase_003C_003EParameterMin_003C_003EAccessor : IMemberAccessor<CubeBlockEffectBase, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffectBase owner, in float value)
				{
					owner.ParameterMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffectBase owner, out float value)
				{
					value = owner.ParameterMin;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffectBase_003C_003EParameterMax_003C_003EAccessor : IMemberAccessor<CubeBlockEffectBase, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffectBase owner, in float value)
				{
					owner.ParameterMax = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffectBase owner, out float value)
				{
					value = owner.ParameterMax;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffectBase_003C_003EParticleEffects_003C_003EAccessor : IMemberAccessor<CubeBlockEffectBase, CubeBlockEffect[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffectBase owner, in CubeBlockEffect[] value)
				{
					owner.ParticleEffects = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffectBase owner, out CubeBlockEffect[] value)
				{
					value = owner.ParticleEffects;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffectBase_003C_003EActor : IActivator, IActivator<CubeBlockEffectBase>
			{
				private sealed override object CreateInstance()
				{
					return new CubeBlockEffectBase();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CubeBlockEffectBase CreateInstance()
				{
					return new CubeBlockEffectBase();
				}

				CubeBlockEffectBase IActivator<CubeBlockEffectBase>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(31)]
			public string Name = "";

			[XmlAttribute]
			[ProtoMember(32)]
			public float ParameterMin = float.MinValue;

			[XmlAttribute]
			[ProtoMember(33)]
			public float ParameterMax = float.MaxValue;

			[XmlArrayItem("ParticleEffect")]
			[ProtoMember(34)]
			public CubeBlockEffect[] ParticleEffects;
		}

		[ProtoContract]
		public class CubeBlockEffect
		{
			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003EName_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003EOrigin_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in string value)
				{
					owner.Origin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out string value)
				{
					value = owner.Origin;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003EDelay_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in float value)
				{
					owner.Delay = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out float value)
				{
					value = owner.Delay;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003EDuration_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in float value)
				{
					owner.Duration = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out float value)
				{
					value = owner.Duration;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003ELoop_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in bool value)
				{
					owner.Loop = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out bool value)
				{
					value = owner.Loop;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003ESpawnTimeMin_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in float value)
				{
					owner.SpawnTimeMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out float value)
				{
					value = owner.SpawnTimeMin;
				}
			}

			protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003ESpawnTimeMax_003C_003EAccessor : IMemberAccessor<CubeBlockEffect, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CubeBlockEffect owner, in float value)
				{
					owner.SpawnTimeMax = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CubeBlockEffect owner, out float value)
				{
					value = owner.SpawnTimeMax;
				}
			}

			private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeBlockEffect_003C_003EActor : IActivator, IActivator<CubeBlockEffect>
			{
				private sealed override object CreateInstance()
				{
					return new CubeBlockEffect();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CubeBlockEffect CreateInstance()
				{
					return new CubeBlockEffect();
				}

				CubeBlockEffect IActivator<CubeBlockEffect>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(35)]
			public string Name = "";

			[XmlAttribute]
			[ProtoMember(36)]
			public string Origin = "";

			[XmlAttribute]
			[ProtoMember(37)]
			public float Delay;

			[XmlAttribute]
			[ProtoMember(38)]
			public float Duration;

			[XmlAttribute]
			[ProtoMember(39)]
			public bool Loop;

			[XmlAttribute]
			[ProtoMember(40)]
			public float SpawnTimeMin;

			[XmlAttribute]
			[ProtoMember(41)]
			public float SpawnTimeMax;
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVoxelPlacement_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, VoxelPlacementOverride?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in VoxelPlacementOverride? value)
			{
				owner.VoxelPlacement = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out VoxelPlacementOverride? value)
			{
				value = owner.VoxelPlacement;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESilenceableByShipSoundSystem_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.SilenceableByShipSoundSystem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.SilenceableByShipSoundSystem;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyCubeSize value)
			{
				owner.CubeSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyCubeSize value)
			{
				value = owner.CubeSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockTopology_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyBlockTopology>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyBlockTopology value)
			{
				owner.BlockTopology = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyBlockTopology value)
			{
				value = owner.BlockTopology;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in SerializableVector3I value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out SerializableVector3I value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModelOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in SerializableVector3 value)
			{
				owner.ModelOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out SerializableVector3 value)
			{
				value = owner.ModelOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECubeDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, PatternDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in PatternDefinition value)
			{
				owner.CubeDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out PatternDefinition value)
			{
				value = owner.CubeDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, CubeBlockComponent[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in CubeBlockComponent[] value)
			{
				owner.Components = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out CubeBlockComponent[] value)
			{
				value = owner.Components;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEffects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, CubeBlockEffectBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in CubeBlockEffectBase[] value)
			{
				owner.Effects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out CubeBlockEffectBase[] value)
			{
				value = owner.Effects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECriticalComponent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, CriticalPart>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in CriticalPart value)
			{
				owner.CriticalComponent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out CriticalPart value)
			{
				value = owner.CriticalComponent;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMountPoints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MountPoint[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MountPoint[] value)
			{
				owner.MountPoints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MountPoint[] value)
			{
				value = owner.MountPoints;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EVariants_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, Variant[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in Variant[] value)
			{
				owner.Variants = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out Variant[] value)
			{
				value = owner.Variants;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEntityComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, EntityComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in EntityComponentDefinition[] value)
			{
				owner.EntityComponents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out EntityComponentDefinition[] value)
			{
				value = owner.EntityComponents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicsOption_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyPhysicsOption>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyPhysicsOption value)
			{
				owner.PhysicsOption = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyPhysicsOption value)
			{
				value = owner.PhysicsOption;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressModels_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, List<BuildProgressModel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in List<BuildProgressModel> value)
			{
				owner.BuildProgressModels = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out List<BuildProgressModel> value)
			{
				value = owner.BuildProgressModels;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockPairName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.BlockPairName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.BlockPairName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECenter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in SerializableVector3I? value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out SerializableVector3I? value)
			{
				value = owner.Center;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringX_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MySymmetryAxisEnum value)
			{
				owner.MirroringX = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MySymmetryAxisEnum value)
			{
				value = owner.MirroringX;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringY_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MySymmetryAxisEnum value)
			{
				owner.MirroringY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MySymmetryAxisEnum value)
			{
				value = owner.MirroringY;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringZ_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySymmetryAxisEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MySymmetryAxisEnum value)
			{
				owner.MirroringZ = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MySymmetryAxisEnum value)
			{
				value = owner.MirroringZ;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDeformationRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value)
			{
				owner.DeformationRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value)
			{
				value = owner.DeformationRatio;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEdgeType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.EdgeType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.EdgeType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildTimeSeconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value)
			{
				owner.BuildTimeSeconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value)
			{
				value = owner.BuildTimeSeconds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisassembleRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value)
			{
				owner.DisassembleRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value)
			{
				value = owner.DisassembleRatio;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAutorotateMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyAutorotateMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyAutorotateMode value)
			{
				owner.AutorotateMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyAutorotateMode value)
			{
				value = owner.AutorotateMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirroringBlock_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.MirroringBlock = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.MirroringBlock;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseModelIntersection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.UseModelIntersection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.UseModelIntersection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPrimarySound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.PrimarySound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.PrimarySound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActionSound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.ActionSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.ActionSound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.BuildType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.BuildType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.BuildMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.BuildMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundTemplates_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string[] value)
			{
				owner.CompoundTemplates = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string[] value)
			{
				value = owner.CompoundTemplates;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECompoundEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.CompoundEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.CompoundEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubBlockDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MySubBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MySubBlockDefinition[] value)
			{
				owner.SubBlockDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MySubBlockDefinition[] value)
			{
				value = owner.SubBlockDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMultiBlock_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.MultiBlock = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.MultiBlock;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ENavigationDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.NavigationDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.NavigationDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGuiVisible_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.GuiVisible = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.GuiVisible;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBlockVariants_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in SerializableDefinitionId[] value)
			{
				owner.BlockVariants = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out SerializableDefinitionId[] value)
			{
				value = owner.BlockVariants;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDirection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyBlockDirection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyBlockDirection value)
			{
				owner.Direction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyBlockDirection value)
			{
				value = owner.Direction;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERotation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyBlockRotation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyBlockRotation value)
			{
				owner.Rotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyBlockRotation value)
			{
				value = owner.Rotation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in SerializableDefinitionId[] value)
			{
				owner.GeneratedBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out SerializableDefinitionId[] value)
			{
				value = owner.GeneratedBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneratedBlockType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.GeneratedBlockType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.GeneratedBlockType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMirrored_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.Mirrored = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.Mirrored;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value)
			{
				owner.DamageEffectId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value)
			{
				value = owner.DamageEffectId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffect_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.DestroyEffect = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.DestroyEffect;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroySound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.DestroySound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.DestroySound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESkeleton_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in List<BoneInfo> value)
			{
				owner.Skeleton = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out List<BoneInfo> value)
			{
				value = owner.Skeleton;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ERandomRotation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.RandomRotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.RandomRotation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsAirTight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool? value)
			{
				owner.IsAirTight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool? value)
			{
				value = owner.IsAirTight;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIsStandAlone_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.IsStandAlone = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.IsStandAlone;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EHasPhysics_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.HasPhysics = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.HasPhysics;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUseNeighbourOxygenRooms_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.UseNeighbourOxygenRooms = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.UseNeighbourOxygenRooms;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPoints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value)
			{
				owner.Points = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value)
			{
				value = owner.Points;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMaxIntegrity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value)
			{
				owner.MaxIntegrity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value)
			{
				value = owner.MaxIntegrity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EBuildProgressToPlaceGeneratedBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value)
			{
				owner.BuildProgressToPlaceGeneratedBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value)
			{
				value = owner.BuildProgressToPlaceGeneratedBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamagedSound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.DamagedSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.DamagedSound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ECreateFracturedPieces_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.CreateFracturedPieces = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.CreateFracturedPieces;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEmissiveColorPreset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.EmissiveColorPreset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.EmissiveColorPreset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EGeneralDamageMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value)
			{
				owner.GeneralDamageMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value)
			{
				value = owner.GeneralDamageMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDamageEffectName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				owner.DamageEffectName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				value = owner.DamageEffectName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EUsesDeformation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.UsesDeformation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.UsesDeformation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDestroyEffectOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, Vector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in Vector3? value)
			{
				owner.DestroyEffectOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out Vector3? value)
			{
				value = owner.DestroyEffectOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPCU_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in int value)
			{
				owner.PCU = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out int value)
			{
				value = owner.PCU;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPlaceDecals_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				owner.PlaceDecals = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				value = owner.PlaceDecals;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalModelDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalModelDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlockDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBlockDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBlockDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_CubeBlockDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CubeBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CubeBlockDefinition CreateInstance()
			{
				return new MyObjectBuilder_CubeBlockDefinition();
			}

			MyObjectBuilder_CubeBlockDefinition IActivator<MyObjectBuilder_CubeBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public VoxelPlacementOverride? VoxelPlacement;

		[ProtoMember(42)]
		[DefaultValue(false)]
		public bool SilenceableByShipSoundSystem;

		[ProtoMember(43)]
		public MyCubeSize CubeSize;

		[ProtoMember(44)]
		public MyBlockTopology BlockTopology;

		[ProtoMember(45)]
		public SerializableVector3I Size;

		[ProtoMember(46)]
		public SerializableVector3 ModelOffset;

		[ProtoMember(47)]
		public PatternDefinition CubeDefinition;

		[XmlArrayItem("Component")]
		[ProtoMember(48)]
		public CubeBlockComponent[] Components;

		[XmlArrayItem("Effect")]
		[ProtoMember(49)]
		public CubeBlockEffectBase[] Effects;

		[ProtoMember(50)]
		public CriticalPart CriticalComponent;

		[ProtoMember(51)]
		public MountPoint[] MountPoints;

		[ProtoMember(52)]
		public Variant[] Variants;

		[XmlArrayItem("Component")]
		[ProtoMember(53)]
		public EntityComponentDefinition[] EntityComponents;

		[ProtoMember(54)]
		[DefaultValue(MyPhysicsOption.Box)]
		public MyPhysicsOption PhysicsOption = MyPhysicsOption.Box;

		[XmlArrayItem("Model")]
		[ProtoMember(55)]
		[DefaultValue(null)]
		public List<BuildProgressModel> BuildProgressModels;

		[ProtoMember(56)]
		public string BlockPairName;

		[ProtoMember(57)]
		public SerializableVector3I? Center;

		[ProtoMember(58)]
		[DefaultValue(MySymmetryAxisEnum.None)]
		public MySymmetryAxisEnum MirroringX;

		[ProtoMember(59)]
		[DefaultValue(MySymmetryAxisEnum.None)]
		public MySymmetryAxisEnum MirroringY;

		[ProtoMember(60)]
		[DefaultValue(MySymmetryAxisEnum.None)]
		public MySymmetryAxisEnum MirroringZ;

		[ProtoMember(61)]
		[DefaultValue(1f)]
		public float DeformationRatio = 1f;

		[ProtoMember(62)]
		public string EdgeType;

		[ProtoMember(63)]
		[DefaultValue(10f)]
		public float BuildTimeSeconds = 10f;

		[ProtoMember(64)]
		[DefaultValue(1f)]
		public float DisassembleRatio = 1f;

		[ProtoMember(65)]
		public MyAutorotateMode AutorotateMode;

		[ProtoMember(66)]
		public string MirroringBlock;

		[ProtoMember(67)]
		public bool UseModelIntersection;

		[ProtoMember(68)]
		public string PrimarySound;

		[ProtoMember(69)]
		public string ActionSound;

		[ProtoMember(70)]
		[DefaultValue(null)]
		public string BuildType;

		[ProtoMember(71)]
		[DefaultValue(null)]
		public string BuildMaterial;

		[XmlArrayItem("Template")]
		[ProtoMember(72)]
		[DefaultValue(null)]
		public string[] CompoundTemplates;

		[ProtoMember(73)]
		[DefaultValue(true)]
		public bool CompoundEnabled = true;

		[XmlArrayItem("Definition")]
		[ProtoMember(74)]
		[DefaultValue(null)]
		public MySubBlockDefinition[] SubBlockDefinitions;

		[ProtoMember(75)]
		[DefaultValue(null)]
		public string MultiBlock;

		[ProtoMember(76)]
		[DefaultValue(null)]
		public string NavigationDefinition;

		[ProtoMember(77)]
		[DefaultValue(true)]
		public bool GuiVisible = true;

		[XmlArrayItem("BlockVariant")]
		[ProtoMember(78)]
		[DefaultValue(null)]
		public SerializableDefinitionId[] BlockVariants;

		[ProtoMember(79)]
		[DefaultValue(MyBlockDirection.Both)]
		public MyBlockDirection Direction = MyBlockDirection.Both;

		[ProtoMember(80)]
		[DefaultValue(MyBlockRotation.Both)]
		public MyBlockRotation Rotation = MyBlockRotation.Both;

		[XmlArrayItem("GeneratedBlock")]
		[ProtoMember(81)]
		[DefaultValue(null)]
		public SerializableDefinitionId[] GeneratedBlocks;

		[ProtoMember(82)]
		[DefaultValue(null)]
		public string GeneratedBlockType;

		[ProtoMember(83)]
		[DefaultValue(false)]
		public bool Mirrored;

		[ProtoMember(84, IsRequired = false)]
		[DefaultValue(0)]
		public int DamageEffectId;

		[ProtoMember(85)]
		[DefaultValue("")]
		public string DestroyEffect = "";

		[ProtoMember(86)]
		[DefaultValue("PoofExplosionCat1")]
		public string DestroySound = "PoofExplosionCat1";

		[ProtoMember(87)]
		[DefaultValue(null)]
		public List<BoneInfo> Skeleton;

		[ProtoMember(88)]
		[DefaultValue(false)]
		public bool RandomRotation;

		[ProtoMember(89)]
		[DefaultValue(null)]
		public bool? IsAirTight;

		[ProtoMember(90)]
		[DefaultValue(true)]
		public bool IsStandAlone = true;

		[ProtoMember(91)]
		[DefaultValue(true)]
		public bool HasPhysics = true;

		public bool UseNeighbourOxygenRooms;

		[ProtoMember(92)]
		[DefaultValue(1)]
		[Obsolete]
		public int Points;

		[ProtoMember(93)]
		[DefaultValue(0)]
		public int MaxIntegrity;

		[ProtoMember(94)]
		[DefaultValue(1)]
		public float BuildProgressToPlaceGeneratedBlocks = 1f;

		[ProtoMember(95)]
		[DefaultValue(null)]
		public string DamagedSound;

		[ProtoMember(96)]
		[DefaultValue(true)]
		public bool CreateFracturedPieces = true;

		[ProtoMember(97)]
		[DefaultValue(null)]
		public string EmissiveColorPreset;

		[ProtoMember(98)]
		[DefaultValue(1f)]
		public float GeneralDamageMultiplier = 1f;

		[ProtoMember(99, IsRequired = false)]
		[DefaultValue("")]
		public string DamageEffectName = "";

		[ProtoMember(100, IsRequired = false)]
		[DefaultValue(true)]
		public bool UsesDeformation = true;

		[ProtoMember(101, IsRequired = false)]
		[DefaultValue(null)]
		public Vector3? DestroyEffectOffset;

		[ProtoMember(102)]
		[DefaultValue(1)]
		public int PCU = 1;

		[ProtoMember(103, IsRequired = false)]
		[DefaultValue(true)]
		public bool PlaceDecals = true;

		public bool ShouldSerializeCenter()
		{
			return Center.HasValue;
		}
	}
}
