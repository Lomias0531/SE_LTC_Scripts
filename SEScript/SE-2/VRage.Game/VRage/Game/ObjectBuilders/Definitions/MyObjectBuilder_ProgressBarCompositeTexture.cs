using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	public class MyObjectBuilder_ProgressBarCompositeTexture : MyObjectBuilder_CompositeTexture
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003EProgressCenter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				owner.ProgressCenter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				value = owner.ProgressCenter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003EProgressLeft_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				owner.ProgressLeft = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				value = owner.ProgressLeft;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003EProgressRight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				owner.ProgressRight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				value = owner.ProgressRight;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003EProgressOverlay_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				owner.ProgressOverlay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				value = owner.ProgressOverlay;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ELeftTop_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ELeftTop_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ELeftCenter_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ELeftCenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ELeftBottom_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ELeftBottom_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ECenterTop_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ECenterTop_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ECenter_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ECenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ECenterBottom_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ECenterBottom_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ERightTop_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ERightTop_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ERightCenter_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ERightCenter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ERightBottom_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture_003C_003ERightBottom_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_CompositeTexture*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_CompositeTexture*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarCompositeTexture owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarCompositeTexture owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProgressBarCompositeTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProgressBarCompositeTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProgressBarCompositeTexture CreateInstance()
			{
				return new MyObjectBuilder_ProgressBarCompositeTexture();
			}

			MyObjectBuilder_ProgressBarCompositeTexture IActivator<MyObjectBuilder_ProgressBarCompositeTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ProgressCenter;

		public MyStringHash ProgressLeft;

		public MyStringHash ProgressRight;

		public MyStringHash ProgressOverlay;

		public override bool IsValid()
		{
			if (!base.IsValid() && !(ProgressCenter != MyStringHash.NullOrEmpty) && !(ProgressLeft != MyStringHash.NullOrEmpty) && !(ProgressRight != MyStringHash.NullOrEmpty))
			{
				return ProgressOverlay != MyStringHash.NullOrEmpty;
			}
			return true;
		}
	}
}
