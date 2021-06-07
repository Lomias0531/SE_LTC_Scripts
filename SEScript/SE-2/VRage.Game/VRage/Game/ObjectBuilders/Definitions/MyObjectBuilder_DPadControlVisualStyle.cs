using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_DPadControlVisualStyle : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003ECenterPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in Vector2 value)
			{
				owner.CenterPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out Vector2 value)
			{
				value = owner.CenterPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003EOriginAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in MyGuiDrawAlignEnum value)
			{
				owner.OriginAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out MyGuiDrawAlignEnum value)
			{
				value = owner.OriginAlign;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in ConditionBase value)
			{
				owner.VisibleCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out ConditionBase value)
			{
				value = owner.VisibleCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DPadControlVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DPadControlVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DPadControlVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_DPadControlVisualStyle();
			}

			MyObjectBuilder_DPadControlVisualStyle IActivator<MyObjectBuilder_DPadControlVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector2 CenterPosition;

		public MyGuiDrawAlignEnum OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;

		[XmlElement(typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase VisibleCondition;

		public static MyObjectBuilder_DPadControlVisualStyle DefaultStyle()
		{
			MyObjectBuilder_DPadControlVisualStyle myObjectBuilder_DPadControlVisualStyle = new MyObjectBuilder_DPadControlVisualStyle();
			myObjectBuilder_DPadControlVisualStyle.CenterPosition = new Vector2(0.645f, 0.905f);
			myObjectBuilder_DPadControlVisualStyle.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			StatCondition statCondition = new StatCondition
			{
				StatId = MyStringHash.GetOrCompute("controller_mode"),
				Operator = StatConditionOperator.Equal,
				Value = 1f
			};
			StatCondition statCondition2 = new StatCondition
			{
				StatId = MyStringHash.GetOrCompute("hud_mode"),
				Operator = StatConditionOperator.Above,
				Value = 0f
			};
			Condition condition = new Condition();
			condition.Operator = StatLogicOperator.And;
			condition.Terms = new ConditionBase[2]
			{
				statCondition,
				statCondition2
			};
			Condition condition2 = (Condition)(myObjectBuilder_DPadControlVisualStyle.VisibleCondition = condition);
			return myObjectBuilder_DPadControlVisualStyle;
		}
	}
}
