using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_Gravity", "DisplayName_TSS_Gravity")]
	public class MyTSSGravity : MyTSSCommon
	{
		public static float ASPECT_RATIO = 3f;

		public static float DECORATION_RATIO = 0.25f;

		public static float TEXT_RATIO = 0.25f;

		private Vector2 m_innerSize;

		private Vector2 m_decorationSize;

		private float m_firstLine;

		private float m_secondLine;

		private StringBuilder m_sb = new StringBuilder();

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSGravity(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_innerSize = new Vector2(ASPECT_RATIO, 1f);
			MyTextSurfaceScriptBase.FitRect(size, ref m_innerSize);
			m_decorationSize = new Vector2(0.012f * m_innerSize.X, DECORATION_RATIO * m_innerSize.Y);
			m_sb.Clear();
			m_sb.Append((object)MyTexts.Get(MySpaceTexts.AGravity));
			m_sb.Append(": 00.00g");
			Vector2 vector = MyGuiManager.MeasureStringRaw("Monospace", m_sb, 1f);
			float val = TEXT_RATIO * m_innerSize.Y / vector.Y;
			m_fontScale = Math.Min(m_innerSize.X * 0.72f / vector.X, val);
			m_firstLine = m_halfSize.Y - m_decorationSize.Y * 0.55f;
			m_secondLine = m_halfSize.Y + m_decorationSize.Y * 0.55f;
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				AddBackground(frame, new Color(m_foregroundColor, 0.66f));
				if (m_block != null)
				{
					Vector3D position = m_block.GetPosition();
					float num = MyGravityProviderSystem.CalculateArtificialGravityInPoint(position, MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(position))).Length() / 9.81f;
					m_sb.Clear();
					m_sb.Append((object)MyTexts.Get(MySpaceTexts.AGravity));
					m_sb.AppendFormat(": {0:F2}g", num);
					Vector2 vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
					MySprite mySprite = new MySprite
					{
						Position = new Vector2(m_halfSize.X, m_firstLine - vector.Y * 0.5f),
						Size = new Vector2(m_innerSize.X, m_innerSize.Y),
						Type = SpriteType.TEXT,
						FontId = m_fontId,
						Alignment = TextAlignment.CENTER,
						Color = m_foregroundColor,
						RotationOrScale = m_fontScale,
						Data = m_sb.ToString()
					};
					MySprite sprite = mySprite;
					frame.Add(sprite);
					num = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).Length() / 9.81f;
					m_sb.Clear();
					m_sb.Append((object)MyTexts.Get(MySpaceTexts.PGravity));
					m_sb.AppendFormat(": {0:F2}g", num);
					vector = MyGuiManager.MeasureStringRaw(m_fontId, m_sb, m_fontScale);
					mySprite = default(MySprite);
					mySprite.Position = new Vector2(m_halfSize.X, m_secondLine - vector.Y * 0.5f);
					mySprite.Size = new Vector2(m_innerSize.X, m_innerSize.Y);
					mySprite.Type = SpriteType.TEXT;
					mySprite.FontId = m_fontId;
					mySprite.Alignment = TextAlignment.CENTER;
					mySprite.Color = m_foregroundColor;
					mySprite.RotationOrScale = m_fontScale;
					mySprite.Data = m_sb.ToString();
					MySprite sprite2 = mySprite;
					frame.Add(sprite2);
					float scale = m_innerSize.Y / 256f * 0.9f;
					AddBrackets(frame, new Vector2(64f, 256f), scale);
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
