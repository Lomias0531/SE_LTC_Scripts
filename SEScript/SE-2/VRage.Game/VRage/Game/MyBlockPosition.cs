using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBlockPosition
	{
		protected class VRage_Game_MyBlockPosition_003C_003EName_003C_003EAccessor : IMemberAccessor<MyBlockPosition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBlockPosition owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBlockPosition owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyBlockPosition_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyBlockPosition, Vector2I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBlockPosition owner, in Vector2I value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBlockPosition owner, out Vector2I value)
			{
				value = owner.Position;
			}
		}

		private class VRage_Game_MyBlockPosition_003C_003EActor : IActivator, IActivator<MyBlockPosition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlockPosition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlockPosition CreateInstance()
			{
				return new MyBlockPosition();
			}

			MyBlockPosition IActivator<MyBlockPosition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;

		[ProtoMember(4)]
		public Vector2I Position;
	}
}
