using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageRender
{
	[ProtoContract]
	public struct MyDecalMaterialDesc
	{
		protected class VRageRender_MyDecalMaterialDesc_003C_003ENormalmapTexture_003C_003EAccessor : IMemberAccessor<MyDecalMaterialDesc, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDecalMaterialDesc owner, in string value)
			{
				owner.NormalmapTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDecalMaterialDesc owner, out string value)
			{
				value = owner.NormalmapTexture;
			}
		}

		protected class VRageRender_MyDecalMaterialDesc_003C_003EColorMetalTexture_003C_003EAccessor : IMemberAccessor<MyDecalMaterialDesc, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDecalMaterialDesc owner, in string value)
			{
				owner.ColorMetalTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDecalMaterialDesc owner, out string value)
			{
				value = owner.ColorMetalTexture;
			}
		}

		protected class VRageRender_MyDecalMaterialDesc_003C_003EAlphamaskTexture_003C_003EAccessor : IMemberAccessor<MyDecalMaterialDesc, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDecalMaterialDesc owner, in string value)
			{
				owner.AlphamaskTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDecalMaterialDesc owner, out string value)
			{
				value = owner.AlphamaskTexture;
			}
		}

		protected class VRageRender_MyDecalMaterialDesc_003C_003EExtensionsTexture_003C_003EAccessor : IMemberAccessor<MyDecalMaterialDesc, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDecalMaterialDesc owner, in string value)
			{
				owner.ExtensionsTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDecalMaterialDesc owner, out string value)
			{
				value = owner.ExtensionsTexture;
			}
		}

		[ProtoMember(1)]
		public string NormalmapTexture;

		[ProtoMember(4)]
		public string ColorMetalTexture;

		[ProtoMember(7)]
		public string AlphamaskTexture;

		[ProtoMember(10)]
		public string ExtensionsTexture;
	}
}
