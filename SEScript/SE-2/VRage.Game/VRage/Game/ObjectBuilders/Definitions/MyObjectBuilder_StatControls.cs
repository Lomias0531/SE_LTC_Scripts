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
	public class MyObjectBuilder_StatControls : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EApplyHudScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatControls owner, in bool value)
			{
				owner.ApplyHudScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatControls owner, out bool value)
			{
				value = owner.ApplyHudScale;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatControls owner, in Vector2 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatControls owner, out Vector2 value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EOriginAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatControls owner, in MyGuiDrawAlignEnum value)
			{
				owner.OriginAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatControls owner, out MyGuiDrawAlignEnum value)
			{
				value = owner.OriginAlign;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EVisibleCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatControls owner, in ConditionBase value)
			{
				owner.VisibleCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatControls owner, out ConditionBase value)
			{
				value = owner.VisibleCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EStatStyles_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, MyObjectBuilder_StatVisualStyle[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StatControls owner, in MyObjectBuilder_StatVisualStyle[] value)
			{
				owner.StatStyles = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StatControls owner, out MyObjectBuilder_StatVisualStyle[] value)
			{
				value = owner.StatStyles;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatControls owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatControls owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatControls owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatControls owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatControls owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatControls owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StatControls owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StatControls owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EActor : IActivator, IActivator<MyObjectBuilder_StatControls>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_StatControls();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_StatControls CreateInstance()
			{
				return new MyObjectBuilder_StatControls();
			}

			MyObjectBuilder_StatControls IActivator<MyObjectBuilder_StatControls>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool ApplyHudScale = true;

		public Vector2 Position;

		public MyGuiDrawAlignEnum OriginAlign;

		[XmlElement(typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase VisibleCondition;

		[XmlArrayItem("StatStyle", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_StatVisualStyle>))]
		public MyObjectBuilder_StatVisualStyle[] StatStyles;
	}
}
