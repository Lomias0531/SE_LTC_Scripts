using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationLayer : MyObjectBuilder_Base
	{
		[ProtoContract]
		public enum MyLayerMode
		{
			private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EMyLayerMode_003C_003EActor : IActivator, IActivator<MyLayerMode>
			{
				private sealed override object CreateInstance()
				{
					return default(MyLayerMode);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyLayerMode CreateInstance()
				{
					//IL_000f: Expected I4, but got O
					return (MyLayerMode)(object)default(MyLayerMode);
				}

				MyLayerMode IActivator<MyLayerMode>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}
			,
			Replace,
			Add
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, MyLayerMode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in MyLayerMode value)
			{
				owner.Mode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out MyLayerMode value)
			{
				value = owner.Mode;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EStateMachine_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in string value)
			{
				owner.StateMachine = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out string value)
			{
				value = owner.StateMachine;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EInitialSMNode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in string value)
			{
				owner.InitialSMNode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out string value)
			{
				value = owner.InitialSMNode;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EBoneMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in string value)
			{
				owner.BoneMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out string value)
			{
				value = owner.BoneMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationLayer owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationLayer owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationLayer>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationLayer();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationLayer CreateInstance()
			{
				return new MyObjectBuilder_AnimationLayer();
			}

			MyObjectBuilder_AnimationLayer IActivator<MyObjectBuilder_AnimationLayer>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;

		[ProtoMember(4)]
		public MyLayerMode Mode;

		[ProtoMember(7)]
		public string StateMachine;

		[ProtoMember(10)]
		public string InitialSMNode;

		[ProtoMember(13)]
		public string BoneMask;
	}
}
