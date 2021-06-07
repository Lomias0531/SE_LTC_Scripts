using System;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Game.GUI.TextPanel
{
	public struct MySpriteDrawFrame : IDisposable
	{
		private MyList<MySprite> m_sprites;

		private Action<MySpriteDrawFrame> m_submitFrameCallback;

		private bool m_isValid;

		public MySpriteDrawFrame(Action<MySpriteDrawFrame> submitFrameCallback)
		{
			m_sprites = PoolManager.Get<MyList<MySprite>>();
			m_submitFrameCallback = submitFrameCallback;
			m_isValid = (m_submitFrameCallback != null);
		}

		public void Add(MySprite sprite)
		{
			m_sprites.Add(sprite);
		}

		public void AddRange(IEnumerable<MySprite> sprites)
		{
			m_sprites.AddRange(sprites);
		}

		public MySpriteCollection ToCollection()
		{
			if (m_sprites.Count == 0)
			{
				return default(MySpriteCollection);
			}
			MySprite[] array = new MySprite[m_sprites.Count];
			for (int i = 0; i < m_sprites.Count; i++)
			{
				array[i] = m_sprites[i];
			}
			return new MySpriteCollection(array);
		}

		public void AddToList(List<MySprite> list)
		{
			list?.AddRange(m_sprites);
		}

		public void Dispose()
		{
			if (m_isValid)
			{
				m_isValid = false;
				if (m_submitFrameCallback != null)
				{
					m_submitFrameCallback(this);
				}
				m_sprites.ClearFast();
				PoolManager.Return(ref m_sprites);
			}
		}
	}
}
