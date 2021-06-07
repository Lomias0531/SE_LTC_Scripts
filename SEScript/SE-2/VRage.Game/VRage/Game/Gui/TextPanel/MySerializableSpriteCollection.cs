using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.GUI.TextPanel
{
	[Serializable]
	[ProtoContract]
	public struct MySerializableSpriteCollection
	{
		protected class VRage_Game_GUI_TextPanel_MySerializableSpriteCollection_003C_003ESprites_003C_003EAccessor : IMemberAccessor<MySerializableSpriteCollection, MySerializableSprite[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSpriteCollection owner, in MySerializableSprite[] value)
			{
				owner.Sprites = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSpriteCollection owner, out MySerializableSprite[] value)
			{
				value = owner.Sprites;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSpriteCollection_003C_003ELength_003C_003EAccessor : IMemberAccessor<MySerializableSpriteCollection, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSpriteCollection owner, in int value)
			{
				owner.Length = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSpriteCollection owner, out int value)
			{
				value = owner.Length;
			}
		}

		private class VRage_Game_GUI_TextPanel_MySerializableSpriteCollection_003C_003EActor : IActivator, IActivator<MySerializableSpriteCollection>
		{
			private sealed override object CreateInstance()
			{
				return default(MySerializableSpriteCollection);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySerializableSpriteCollection CreateInstance()
			{
				return (MySerializableSpriteCollection)(object)default(MySerializableSpriteCollection);
			}

			MySerializableSpriteCollection IActivator<MySerializableSpriteCollection>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Nullable]
		public MySerializableSprite[] Sprites;

		[ProtoMember(4)]
		public int Length;

		public MySerializableSpriteCollection(MySerializableSprite[] sprites, int length)
		{
			Sprites = sprites;
			Length = length;
		}
	}
}
