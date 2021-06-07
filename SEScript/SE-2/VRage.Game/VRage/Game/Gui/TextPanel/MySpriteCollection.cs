using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.GUI.TextPanel
{
	[Serializable]
	[ProtoContract]
	public struct MySpriteCollection
	{
		protected class VRage_Game_GUI_TextPanel_MySpriteCollection_003C_003ESprites_003C_003EAccessor : IMemberAccessor<MySpriteCollection, MySprite[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySpriteCollection owner, in MySprite[] value)
			{
				owner.Sprites = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySpriteCollection owner, out MySprite[] value)
			{
				value = owner.Sprites;
			}
		}

		private class VRage_Game_GUI_TextPanel_MySpriteCollection_003C_003EActor : IActivator, IActivator<MySpriteCollection>
		{
			private sealed override object CreateInstance()
			{
				return default(MySpriteCollection);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySpriteCollection CreateInstance()
			{
				return (MySpriteCollection)(object)default(MySpriteCollection);
			}

			MySpriteCollection IActivator<MySpriteCollection>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Nullable]
		public MySprite[] Sprites;

		public MySpriteCollection(MySprite[] sprites)
		{
			Sprites = sprites;
		}

		public static implicit operator MySerializableSpriteCollection(MySpriteCollection collection)
		{
			MySerializableSprite[] array = null;
			if (collection.Sprites != null && collection.Sprites.Length != 0)
			{
				array = new MySerializableSprite[collection.Sprites.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = collection.Sprites[i];
				}
			}
			return new MySerializableSpriteCollection(array, (array != null) ? array.Length : 0);
		}

		public static implicit operator MySpriteCollection(MySerializableSpriteCollection collection)
		{
			MySprite[] array = null;
			if (collection.Sprites != null && collection.Sprites.Length != 0)
			{
				array = new MySprite[collection.Sprites.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = collection.Sprites[i];
				}
			}
			return new MySpriteCollection(array);
		}
	}
}
