using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	public abstract class MyTextSurfaceScriptBase : IMyTextSurfaceScript, IDisposable
	{
		public static readonly Color DEFAULT_BACKGROUND_COLOR = new Color(0, 88, 151);

		public static readonly Color DEFAULT_FONT_COLOR = new Color(179, 237, 255);

		protected IMyTextSurface m_surface;

		protected IMyCubeBlock m_block;

		protected Vector2 m_size;

		protected Vector2 m_halfSize;

		protected Vector2 m_scale;

		protected Color m_backgroundColor = DEFAULT_BACKGROUND_COLOR;

		protected Color m_foregroundColor = DEFAULT_FONT_COLOR;

		public IMyTextSurface Surface => m_surface;

		public IMyCubeBlock Block => m_block;

		public Color ForegroundColor => m_foregroundColor;

		public Color BackgroundColor => m_backgroundColor;

		public abstract ScriptUpdate NeedsUpdate
		{
			get;
		}

		protected MyTextSurfaceScriptBase(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
		{
			m_surface = surface;
			m_block = block;
			m_size = size;
			m_halfSize = size / 2f;
			m_scale = size / 512f;
		}

		public virtual void Run()
		{
		}

		public virtual void Dispose()
		{
			m_surface = null;
			m_block = null;
		}

		public static void FitRect(Vector2 texture, ref Vector2 rect)
		{
			float num = Math.Min(texture.X / rect.X, texture.Y / rect.Y);
			rect *= num;
		}
	}
}
