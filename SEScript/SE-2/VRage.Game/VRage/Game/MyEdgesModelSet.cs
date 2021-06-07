using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Data;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyEdgesModelSet
	{
		protected class VRage_Game_MyEdgesModelSet_003C_003EVertical_003C_003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEdgesModelSet owner, in string value)
			{
				owner.Vertical = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEdgesModelSet owner, out string value)
			{
				value = owner.Vertical;
			}
		}

		protected class VRage_Game_MyEdgesModelSet_003C_003EVerticalDiagonal_003C_003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEdgesModelSet owner, in string value)
			{
				owner.VerticalDiagonal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEdgesModelSet owner, out string value)
			{
				value = owner.VerticalDiagonal;
			}
		}

		protected class VRage_Game_MyEdgesModelSet_003C_003EHorisontal_003C_003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEdgesModelSet owner, in string value)
			{
				owner.Horisontal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEdgesModelSet owner, out string value)
			{
				value = owner.Horisontal;
			}
		}

		protected class VRage_Game_MyEdgesModelSet_003C_003EHorisontalDiagonal_003C_003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEdgesModelSet owner, in string value)
			{
				owner.HorisontalDiagonal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEdgesModelSet owner, out string value)
			{
				value = owner.HorisontalDiagonal;
			}
		}

		private class VRage_Game_MyEdgesModelSet_003C_003EActor : IActivator, IActivator<MyEdgesModelSet>
		{
			private sealed override object CreateInstance()
			{
				return new MyEdgesModelSet();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEdgesModelSet CreateInstance()
			{
				return new MyEdgesModelSet();
			}

			MyEdgesModelSet IActivator<MyEdgesModelSet>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[ModdableContentFile("mwm")]
		public string Vertical;

		[ProtoMember(4)]
		[ModdableContentFile("mwm")]
		public string VerticalDiagonal;

		[ProtoMember(7)]
		[ModdableContentFile("mwm")]
		public string Horisontal;

		[ProtoMember(10)]
		[ModdableContentFile("mwm")]
		public string HorisontalDiagonal;
	}
}
