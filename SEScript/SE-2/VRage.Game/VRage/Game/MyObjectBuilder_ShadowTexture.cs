using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyObjectBuilder_ShadowTexture
	{
		protected class VRage_Game_MyObjectBuilder_ShadowTexture_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShadowTexture owner, in string value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShadowTexture owner, out string value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShadowTexture_003C_003EMinWidth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShadowTexture owner, in float value)
			{
				owner.MinWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShadowTexture owner, out float value)
			{
				value = owner.MinWidth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShadowTexture_003C_003EGrowFactorWidth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShadowTexture owner, in float value)
			{
				owner.GrowFactorWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShadowTexture owner, out float value)
			{
				value = owner.GrowFactorWidth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShadowTexture_003C_003EGrowFactorHeight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShadowTexture owner, in float value)
			{
				owner.GrowFactorHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShadowTexture owner, out float value)
			{
				value = owner.GrowFactorHeight;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ShadowTexture_003C_003EDefaultAlpha_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ShadowTexture owner, in float value)
			{
				owner.DefaultAlpha = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ShadowTexture owner, out float value)
			{
				value = owner.DefaultAlpha;
			}
		}

		private class VRage_Game_MyObjectBuilder_ShadowTexture_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ShadowTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ShadowTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ShadowTexture CreateInstance()
			{
				return new MyObjectBuilder_ShadowTexture();
			}

			MyObjectBuilder_ShadowTexture IActivator<MyObjectBuilder_ShadowTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public string Texture = "";

		[ProtoMember(7)]
		public float MinWidth;

		[ProtoMember(10)]
		public float GrowFactorWidth = 1f;

		[ProtoMember(13)]
		public float GrowFactorHeight = 1f;

		[ProtoMember(16)]
		public float DefaultAlpha = 1f;
	}
}
