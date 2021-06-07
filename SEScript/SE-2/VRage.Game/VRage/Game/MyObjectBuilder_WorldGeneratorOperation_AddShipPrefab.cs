using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("AddShipPrefab")]
	public class MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003EPrefabFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in string value)
			{
				owner.PrefabFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out string value)
			{
				value = owner.PrefabFile;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003ETransform_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, MyPositionAndOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in MyPositionAndOrientation value)
			{
				owner.Transform = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out MyPositionAndOrientation value)
			{
				value = owner.Transform;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003EUseFirstGridOrigin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in bool value)
			{
				owner.UseFirstGridOrigin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out bool value)
			{
				value = owner.UseFirstGridOrigin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003ERandomRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in float value)
			{
				owner.RandomRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out float value)
			{
				value = owner.RandomRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab();
			}

			MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(136)]
		[XmlAttribute]
		public string PrefabFile;

		[ProtoMember(139)]
		public MyPositionAndOrientation Transform;

		[ProtoMember(142)]
		public bool UseFirstGridOrigin;

		[ProtoMember(145)]
		[XmlAttribute]
		public float RandomRadius;

		public bool ShouldSerializeRandomRadius()
		{
			return RandomRadius != 0f;
		}
	}
}
