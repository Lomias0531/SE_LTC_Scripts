using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyGuiCustomVisualStyle
	{
		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003ENormalTexture_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in string value)
			{
				owner.NormalTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out string value)
			{
				value = owner.NormalTexture;
			}
		}

		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003EHighlightTexture_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in string value)
			{
				owner.HighlightTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out string value)
			{
				value = owner.HighlightTexture;
			}
		}

		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in Vector2 value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out Vector2 value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003ENormalFont_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in string value)
			{
				owner.NormalFont = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out string value)
			{
				value = owner.NormalFont;
			}
		}

		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003EHighlightFont_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in string value)
			{
				owner.HighlightFont = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out string value)
			{
				value = owner.HighlightFont;
			}
		}

		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003EHorizontalPadding_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in float value)
			{
				owner.HorizontalPadding = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out float value)
			{
				value = owner.HorizontalPadding;
			}
		}

		protected class VRage_Game_MyGuiCustomVisualStyle_003C_003EVerticalPadding_003C_003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGuiCustomVisualStyle owner, in float value)
			{
				owner.VerticalPadding = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGuiCustomVisualStyle owner, out float value)
			{
				value = owner.VerticalPadding;
			}
		}

		private class VRage_Game_MyGuiCustomVisualStyle_003C_003EActor : IActivator, IActivator<MyGuiCustomVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return default(MyGuiCustomVisualStyle);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGuiCustomVisualStyle CreateInstance()
			{
				return (MyGuiCustomVisualStyle)(object)default(MyGuiCustomVisualStyle);
			}

			MyGuiCustomVisualStyle IActivator<MyGuiCustomVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string NormalTexture;

		[ProtoMember(4)]
		public string HighlightTexture;

		[ProtoMember(7)]
		public Vector2 Size;

		[ProtoMember(10)]
		public string NormalFont;

		[ProtoMember(13)]
		public string HighlightFont;

		[ProtoMember(16)]
		public float HorizontalPadding;

		[ProtoMember(19)]
		public float VerticalPadding;
	}
}
