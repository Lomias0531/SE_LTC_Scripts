using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRageMath;

namespace Sandbox.Gui
{
	public class MyWikiMarkupParser
	{
		private static Regex m_splitRegex = new Regex("\\[.*?\\]{1,2}");

		private static Regex m_markupRegex = new Regex("(?<=\\[)(?!\\[).*?(?=\\])");

		private static Regex m_digitsRegex = new Regex("\\d+");

		private static StringBuilder m_stringCache = new StringBuilder();

		public static void ParseText(string text, ref MyGuiControlMultilineText label)
		{
			try
			{
				string[] array = m_splitRegex.Split(text);
				MatchCollection matchCollection = m_splitRegex.Matches(text);
				for (int i = 0; i < matchCollection.Count || i < array.Length; i++)
				{
					if (i < array.Length)
					{
						label.AppendText(m_stringCache.Clear().Append(array[i]));
					}
					if (i < matchCollection.Count)
					{
						ParseMarkup(label, matchCollection[i].Value);
					}
				}
			}
			catch
			{
			}
		}

		private static void ParseMarkup(MyGuiControlMultilineText label, string markup)
		{
			Match match = m_markupRegex.Match(markup);
			if (Enumerable.Contains(match.Value, '|'))
			{
				string[] array = match.Value.Substring(5).Split(new char[1]
				{
					'|'
				});
				MatchCollection matchCollection = m_digitsRegex.Matches(array[1]);
				if (int.TryParse(matchCollection[0].Value, out int result) && int.TryParse(matchCollection[1].Value, out int result2))
				{
					label.AppendImage(array[0], MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(result, result2)), Vector4.One);
				}
			}
			else
			{
				label.AppendLink(match.Value.Substring(0, match.Value.IndexOf(' ')), match.Value.Substring(match.Value.IndexOf(' ') + 1));
			}
		}
	}
}
