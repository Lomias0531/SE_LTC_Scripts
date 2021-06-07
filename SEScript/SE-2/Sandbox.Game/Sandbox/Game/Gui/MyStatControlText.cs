using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyStatControlText : MyStatControlBase
	{
		private readonly StringBuilder m_text;

		private readonly StringBuilder m_textTmp = new StringBuilder(128);

		private string m_precisionFormat;

		private int m_precision;

		private MyFont m_font;

		private MyStringHash m_fontHash;

		private readonly bool m_hasStat;

		private const string STAT_TAG = "{STAT}";

		public float Scale
		{
			get;
			set;
		}

		public Vector4 TextColorMask
		{
			get;
			set;
		}

		public MyGuiDrawAlignEnum TextAlign
		{
			get;
			set;
		}

		public string Font
		{
			get
			{
				return m_fontHash.String;
			}
			set
			{
				m_fontHash = MyStringHash.GetOrCompute(value);
				m_font = MyGuiManager.GetFont(m_fontHash);
			}
		}

		public static string SubstituteTexts(string text, string context = null)
		{
			return MyTexts.SubstituteTexts(text, context);
		}

		public MyStatControlText(MyStatControls parent, string text)
			: base(parent)
		{
			TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			Font = "Blue";
			Scale = 1f;
			TextColorMask = Vector4.One;
			string text2 = MyTexts.SubstituteTexts(text);
			m_text = new StringBuilder(text2);
			m_hasStat = text2.Contains("{STAT}");
		}

		public override void Draw(float transitionAlpha)
		{
			Vector4 sourceColorMask = TextColorMask;
			if (base.BlinkBehavior.Blink && base.BlinkBehavior.ColorMask.HasValue)
			{
				sourceColorMask = base.BlinkBehavior.ColorMask.Value;
			}
			sourceColorMask = MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, transitionAlpha);
			StringBuilder stringBuilder;
			if (m_hasStat)
			{
				m_textTmp.Clear();
				m_textTmp.Append((object)m_text);
				m_textTmp.Replace("{STAT}", base.StatString);
				stringBuilder = m_textTmp;
			}
			else
			{
				stringBuilder = m_text;
			}
			Vector2 size = m_font.MeasureString(stringBuilder, Scale);
			Vector2 screenCoord = MyUtils.GetCoordTopLeftFromAligned(base.Position, size, TextAlign) + base.Size / 2f;
			MyRenderProxy.DrawString((int)m_fontHash, screenCoord, sourceColorMask, stringBuilder.ToString(), Scale, size.X + 100f);
		}
	}
}
