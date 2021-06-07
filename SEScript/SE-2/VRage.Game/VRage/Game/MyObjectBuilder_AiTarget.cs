using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AiTarget : MyObjectBuilder_Base
	{
		[ProtoContract]
		public class UnreachableEntitiesData
		{
			protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003EUnreachableEntitiesData_003C_003EUnreachableEntityId_003C_003EAccessor : IMemberAccessor<UnreachableEntitiesData, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref UnreachableEntitiesData owner, in long value)
				{
					owner.UnreachableEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref UnreachableEntitiesData owner, out long value)
				{
					value = owner.UnreachableEntityId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003EUnreachableEntitiesData_003C_003ETimeout_003C_003EAccessor : IMemberAccessor<UnreachableEntitiesData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref UnreachableEntitiesData owner, in int value)
				{
					owner.Timeout = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref UnreachableEntitiesData owner, out int value)
				{
					value = owner.Timeout;
				}
			}

			private class VRage_Game_MyObjectBuilder_AiTarget_003C_003EUnreachableEntitiesData_003C_003EActor : IActivator, IActivator<UnreachableEntitiesData>
			{
				private sealed override object CreateInstance()
				{
					return new UnreachableEntitiesData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override UnreachableEntitiesData CreateInstance()
				{
					return new UnreachableEntitiesData();
				}

				UnreachableEntitiesData IActivator<UnreachableEntitiesData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long UnreachableEntityId;

			[ProtoMember(4)]
			public int Timeout;
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003ECurrentTarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, MyAiTargetEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AiTarget owner, in MyAiTargetEnum value)
			{
				owner.CurrentTarget = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AiTarget owner, out MyAiTargetEnum value)
			{
				value = owner.CurrentTarget;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AiTarget owner, in long? value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AiTarget owner, out long? value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003ECompoundId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, ushort?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AiTarget owner, in ushort? value)
			{
				owner.CompoundId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AiTarget owner, out ushort? value)
			{
				value = owner.CompoundId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003ETargetCube_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AiTarget owner, in Vector3I value)
			{
				owner.TargetCube = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AiTarget owner, out Vector3I value)
			{
				value = owner.TargetCube;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003ETargetPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AiTarget owner, in Vector3D value)
			{
				owner.TargetPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AiTarget owner, out Vector3D value)
			{
				value = owner.TargetPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003EUnreachableEntities_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AiTarget, List<UnreachableEntitiesData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AiTarget owner, in List<UnreachableEntitiesData> value)
			{
				owner.UnreachableEntities = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AiTarget owner, out List<UnreachableEntitiesData> value)
			{
				value = owner.UnreachableEntities;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AiTarget owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AiTarget owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AiTarget owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AiTarget owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AiTarget owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AiTarget owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AiTarget_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AiTarget, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AiTarget owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AiTarget owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AiTarget_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AiTarget>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AiTarget();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AiTarget CreateInstance()
			{
				return new MyObjectBuilder_AiTarget();
			}

			MyObjectBuilder_AiTarget IActivator<MyObjectBuilder_AiTarget>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public MyAiTargetEnum CurrentTarget;

		[ProtoMember(10)]
		public long? EntityId;

		[ProtoMember(13)]
		public ushort? CompoundId;

		[ProtoMember(16)]
		public Vector3I TargetCube = Vector3I.Zero;

		[ProtoMember(19)]
		public Vector3D TargetPosition = Vector3D.Zero;

		[ProtoMember(22)]
		public List<UnreachableEntitiesData> UnreachableEntities;
	}
}
