using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.GUI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_StatVisualStyle : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EStatId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyStringHash value)
			{
				owner.StatId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyStringHash value)
			{
				value = owner.StatId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in ConditionBase value)
			{
				owner.VisibleCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out ConditionBase value)
			{
				value = owner.VisibleCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in ConditionBase value)
			{
				owner.BlinkCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out ConditionBase value)
			{
				value = owner.BlinkCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESizePx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in Vector2 value)
			{
				owner.SizePx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out Vector2 value)
			{
				value = owner.SizePx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EOffsetPx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in Vector2 value)
			{
				owner.OffsetPx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out Vector2 value)
			{
				value = owner.OffsetPx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in uint? value)
			{
				owner.FadeInTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out uint? value)
			{
				value = owner.FadeInTimeMs;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in uint? value)
			{
				owner.FadeOutTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out uint? value)
			{
				value = owner.FadeOutTimeMs;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in uint? value)
			{
				owner.MaxOnScreenTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out uint? value)
			{
				value = owner.MaxOnScreenTimeMs;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlink_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyAlphaBlinkBehavior value)
			{
				owner.Blink = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyAlphaBlinkBehavior value)
			{
				value = owner.Blink;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ECategory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, VisualStyleCategory?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in VisualStyleCategory? value)
			{
				owner.Category = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out VisualStyleCategory? value)
			{
				value = owner.Category;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_StatVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_StatVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_StatVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_StatVisualStyle();
			}

			MyObjectBuilder_StatVisualStyle IActivator<MyObjectBuilder_StatVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash StatId;

		[XmlElement(typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase VisibleCondition;

		[XmlElement(typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase BlinkCondition;

		public Vector2 SizePx;

		public Vector2 OffsetPx;

		public uint? FadeInTimeMs;

		public uint? FadeOutTimeMs;

		public uint? MaxOnScreenTimeMs;

		public MyAlphaBlinkBehavior Blink;

		public VisualStyleCategory? Category;
	}
}
