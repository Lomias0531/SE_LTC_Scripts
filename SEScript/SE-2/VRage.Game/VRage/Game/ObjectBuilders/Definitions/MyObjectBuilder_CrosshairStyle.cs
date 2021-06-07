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
	public class MyObjectBuilder_CrosshairStyle : MyObjectBuilder_StatControls
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003EApplyHudScale_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EApplyHudScale_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_StatControls*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_StatControls*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003EPosition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatControls*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatControls*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref *(MyObjectBuilder_StatControls*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref *(MyObjectBuilder_StatControls*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatControls*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatControls*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003EStatStyles_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls_003C_003EStatStyles_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyObjectBuilder_StatVisualStyle[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyObjectBuilder_StatVisualStyle[] value)
			{
				Set(ref *(MyObjectBuilder_StatControls*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out MyObjectBuilder_StatVisualStyle[] value)
			{
				Get(ref *(MyObjectBuilder_StatControls*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CrosshairStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CrosshairStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CrosshairStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CrosshairStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CrosshairStyle CreateInstance()
			{
				return new MyObjectBuilder_CrosshairStyle();
			}

			MyObjectBuilder_CrosshairStyle IActivator<MyObjectBuilder_CrosshairStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
