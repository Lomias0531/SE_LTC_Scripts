using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("MyBBMemoryTarget")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBBMemoryTarget : MyBBMemoryValue
	{
		protected class VRage_Game_MyBBMemoryTarget_003C_003ETargetType_003C_003EAccessor : IMemberAccessor<MyBBMemoryTarget, MyAiTargetEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryTarget owner, in MyAiTargetEnum value)
			{
				owner.TargetType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryTarget owner, out MyAiTargetEnum value)
			{
				value = owner.TargetType;
			}
		}

		protected class VRage_Game_MyBBMemoryTarget_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyBBMemoryTarget, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryTarget owner, in long? value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryTarget owner, out long? value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_MyBBMemoryTarget_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyBBMemoryTarget, Vector3D?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryTarget owner, in Vector3D? value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryTarget owner, out Vector3D? value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyBBMemoryTarget_003C_003ETreeId_003C_003EAccessor : IMemberAccessor<MyBBMemoryTarget, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryTarget owner, in int? value)
			{
				owner.TreeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryTarget owner, out int? value)
			{
				value = owner.TreeId;
			}
		}

		protected class VRage_Game_MyBBMemoryTarget_003C_003ECompoundId_003C_003EAccessor : IMemberAccessor<MyBBMemoryTarget, ushort?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryTarget owner, in ushort? value)
			{
				owner.CompoundId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryTarget owner, out ushort? value)
			{
				value = owner.CompoundId;
			}
		}

		private class VRage_Game_MyBBMemoryTarget_003C_003EActor : IActivator, IActivator<MyBBMemoryTarget>
		{
			private sealed override object CreateInstance()
			{
				return new MyBBMemoryTarget();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBBMemoryTarget CreateInstance()
			{
				return new MyBBMemoryTarget();
			}

			MyBBMemoryTarget IActivator<MyBBMemoryTarget>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyAiTargetEnum TargetType;

		[ProtoMember(4)]
		public long? EntityId;

		[ProtoMember(7)]
		public Vector3D? Position;

		[ProtoMember(10)]
		public int? TreeId;

		[ProtoMember(13)]
		public ushort? CompoundId;

		public Vector3I BlockPosition => Vector3I.Round(Position.Value);

		public Vector3I VoxelPosition => Vector3I.Round(Position.Value);

		public static void UnsetTarget(ref MyBBMemoryTarget target)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = MyAiTargetEnum.NO_TARGET;
		}

		public static void SetTargetEntity(ref MyBBMemoryTarget target, MyAiTargetEnum targetType, long entityId, Vector3D? position = null)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = targetType;
			target.EntityId = entityId;
			target.TreeId = null;
			target.Position = position;
		}

		public static void SetTargetPosition(ref MyBBMemoryTarget target, Vector3D position)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = MyAiTargetEnum.POSITION;
			target.EntityId = null;
			target.TreeId = null;
			target.Position = position;
		}

		public static void SetTargetCube(ref MyBBMemoryTarget target, Vector3I blockPosition, long gridEntityId)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = MyAiTargetEnum.CUBE;
			target.EntityId = gridEntityId;
			target.TreeId = null;
			target.Position = new Vector3D(blockPosition);
		}

		public static void SetTargetVoxel(ref MyBBMemoryTarget target, Vector3I voxelPosition, long entityId)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = MyAiTargetEnum.VOXEL;
			target.EntityId = entityId;
			target.TreeId = null;
			target.Position = new Vector3D(voxelPosition);
		}

		public static void SetTargetTree(ref MyBBMemoryTarget target, Vector3D treePosition, long entityId, int treeId)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = MyAiTargetEnum.ENVIRONMENT_ITEM;
			target.EntityId = entityId;
			target.TreeId = treeId;
			target.Position = treePosition;
		}

		public static void SetTargetCompoundBlock(ref MyBBMemoryTarget target, Vector3I blockPosition, long entityId, ushort compoundId)
		{
			if (target == null)
			{
				target = new MyBBMemoryTarget();
			}
			target.TargetType = MyAiTargetEnum.COMPOUND_BLOCK;
			target.EntityId = entityId;
			target.CompoundId = compoundId;
			target.Position = blockPosition;
		}
	}
}
