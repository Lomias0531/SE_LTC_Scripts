using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public class MyFont
	{
		/// <summary>
		///  Info for each glyph in the font - where to find the glyph image and other properties
		/// </summary>
		protected class MyGlyphInfo
		{
			public ushort nBitmapID;

			public ushort pxLocX;

			public ushort pxLocY;

			public byte pxWidth;

			public byte pxHeight;

			public byte pxAdvanceWidth;

			public sbyte pxLeftSideBearing;
		}

		/// <summary>
		/// Info for each font bitmap
		/// </summary>
		protected struct MyBitmapInfo
		{
			public string strFilename;

			public int nX;

			public int nY;
		}

		protected struct KernPair
		{
			public char Left;

			public char Right;

			public KernPair(char l, char r)
			{
				Left = l;
				Right = r;
			}
		}

		protected class KernPairComparer : IComparer<KernPair>, IEqualityComparer<KernPair>
		{
			public int Compare(KernPair x, KernPair y)
			{
				if (x.Left != y.Left)
				{
					return x.Left.CompareTo(y.Left);
				}
				return x.Right.CompareTo(y.Right);
			}

			public bool Equals(KernPair x, KernPair y)
			{
				if (x.Left == y.Left)
				{
					return x.Right == y.Right;
				}
				return false;
			}

			public int GetHashCode(KernPair x)
			{
				return x.Left.GetHashCode() ^ x.Right.GetHashCode();
			}
		}

		/// <summary>
		/// Replacement character shown when we don't have something in our texture.
		/// Normally, this would be \uFFFD, but BMFontGen refuses to generate it, so I put its glyph at \u25A1 (empty square)
		/// </summary>
		protected const char REPLACEMENT_CHARACTER = '□';

		protected const char ELLIPSIS = '…';

		public const char NEW_LINE = '\n';

		private static readonly KernPairComparer m_kernPairComparer = new KernPairComparer();

		protected readonly Dictionary<int, MyBitmapInfo> m_bitmapInfoByID = new Dictionary<int, MyBitmapInfo>();

		protected readonly Dictionary<char, MyGlyphInfo> m_glyphInfoByChar = new Dictionary<char, MyGlyphInfo>();

		protected readonly Dictionary<KernPair, sbyte> m_kernByPair = new Dictionary<KernPair, sbyte>(m_kernPairComparer);

		protected readonly string m_fontDirectory;

		/// <summary>
		/// This is artificial spacing in between two characters (in pixels).
		/// Using it we can make spaces wider or narrower
		/// </summary>
		public int Spacing;

		/// <summary>
		/// Enable / disable kerning of adjacent character pairs.
		/// </summary>
		public bool KernEnabled = true;

		/// <summary>
		/// The depth at which to draw the font
		/// </summary>
		public float Depth;

		/// <summary>
		/// Distance from top of font to the baseline
		/// </summary>
		public int Baseline
		{
			get;
			private set;
		}

		/// <summary>
		/// Distance from top to bottom of the font
		/// </summary>
		public int LineHeight
		{
			get;
			private set;
		}

		/// <summary>
		/// Create a new font from the info in the specified font descriptor (XML) file
		/// </summary>
		public MyFont(string fontFilePath, int spacing = 1)
		{
			LogWriteLine("MyFont.Ctor - START");
			using (MyRenderProxy.Log?.IndentUsing(LoggingOptions.MISC_RENDER_ASSETS))
			{
				Spacing = spacing;
				LogWriteLine("Font filename: " + fontFilePath);
				string text = fontFilePath;
				if (!Path.IsPathRooted(fontFilePath))
				{
					text = Path.Combine(MyFileSystem.ContentPath, fontFilePath);
				}
				if (!MyFileSystem.FileExists(text))
				{
					throw new Exception($"Unable to find font path '{text}'.");
				}
				m_fontDirectory = Path.GetDirectoryName(text);
				LoadFontXML(text);
				LogWriteLine("FontFilePath: " + text);
				LogWriteLine("LineHeight: " + LineHeight);
				LogWriteLine("Baseline: " + Baseline);
				LogWriteLine("KernEnabled: " + KernEnabled);
			}
			LogWriteLine("MyFont.Ctor - END");
		}

		private void LogWriteLine(string message)
		{
			MyRenderProxy.Log?.WriteLine(message);
		}

		public Vector2 MeasureString(StringBuilder text, float scale)
		{
			scale *= 144f / 185f;
			float num = 0f;
			char chLeft = '\0';
			float num2 = 0f;
			int num3 = 1;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					num3++;
					num = 0f;
					chLeft = '\0';
				}
				else if (CanWriteOrReplace(ref c))
				{
					MyGlyphInfo myGlyphInfo = m_glyphInfoByChar[c];
					if (KernEnabled)
					{
						num += (float)CalcKern(chLeft, c);
						chLeft = c;
					}
					num += (float)(int)myGlyphInfo.pxAdvanceWidth;
					if (i < text.Length - 1)
					{
						num += (float)Spacing;
					}
					if (num > num2)
					{
						num2 = num;
					}
				}
			}
			return new Vector2(num2 * scale, (float)(num3 * LineHeight) * scale);
		}

		protected bool CanWriteOrReplace(ref char c)
		{
			if (!m_glyphInfoByChar.ContainsKey(c))
			{
				if (!CanUseReplacementCharacter(c))
				{
					return false;
				}
				c = '□';
			}
			return true;
		}

		public int ComputeCharsThatFit(StringBuilder text, float scale, float maxTextWidth)
		{
			scale *= 144f / 185f;
			maxTextWidth /= scale;
			float num = 0f;
			char chLeft = '\0';
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (CanWriteOrReplace(ref c))
				{
					MyGlyphInfo myGlyphInfo = m_glyphInfoByChar[c];
					if (KernEnabled)
					{
						num += (float)CalcKern(chLeft, c);
						chLeft = c;
					}
					num += (float)(int)myGlyphInfo.pxAdvanceWidth;
					if (i < text.Length - 1)
					{
						num += (float)Spacing;
					}
					if (num > maxTextWidth)
					{
						return i;
					}
				}
			}
			return text.Length;
		}

		protected float ComputeScaledAdvanceWithKern(char c, char cLast, float scale)
		{
			if (!CanWriteOrReplace(ref c))
			{
				return 0f;
			}
			if (!m_glyphInfoByChar.TryGetValue(c, out MyGlyphInfo value))
			{
				return 0f;
			}
			float num = 0f;
			if (KernEnabled)
			{
				int num2 = CalcKern(cLast, c);
				num += (float)num2 * scale;
			}
			return num + (float)(int)value.pxAdvanceWidth * scale;
		}

		protected bool CanUseReplacementCharacter(char c)
		{
			if (!char.IsWhiteSpace(c))
			{
				return !char.IsControl(c);
			}
			return false;
		}

		protected int CalcKern(char chLeft, char chRight)
		{
			sbyte value = 0;
			m_kernByPair.TryGetValue(new KernPair(chLeft, chRight), out value);
			return value;
		}

		private void LoadFontXML(string path)
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (Stream inStream = MyFileSystem.OpenRead(path))
			{
				xmlDocument.Load(inStream);
			}
			LoadFontXML(xmlDocument.ChildNodes);
		}

		/// <summary>
		/// Load the font data from an XML font descriptor file
		/// </summary>
		/// <param name="xnl">XML node list containing the entire font descriptor file</param>
		private void LoadFontXML(XmlNodeList xnl)
		{
			foreach (XmlNode item in xnl)
			{
				if (item.Name == "font")
				{
					Baseline = int.Parse(GetXMLAttribute(item, "base"));
					LineHeight = int.Parse(GetXMLAttribute(item, "height"));
					LoadFontXML_font(item.ChildNodes);
				}
			}
		}

		/// <summary>
		/// Load the data from the "font" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "font" node's children</param>
		private void LoadFontXML_font(XmlNodeList xnl)
		{
			foreach (XmlNode item in xnl)
			{
				if (item.Name == "bitmaps")
				{
					LoadFontXML_bitmaps(item.ChildNodes);
				}
				if (item.Name == "glyphs")
				{
					LoadFontXML_glyphs(item.ChildNodes);
				}
				if (item.Name == "kernpairs")
				{
					LoadFontXML_kernpairs(item.ChildNodes);
				}
			}
		}

		/// <summary>
		/// Load the data from the "bitmaps" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "bitmaps" node's children</param>
		private void LoadFontXML_bitmaps(XmlNodeList xnl)
		{
			foreach (XmlNode item in xnl)
			{
				if (item.Name == "bitmap")
				{
					string xMLAttribute = GetXMLAttribute(item, "id");
					string xMLAttribute2 = GetXMLAttribute(item, "name");
					string[] array = GetXMLAttribute(item, "size").Split(new char[1]
					{
						'x'
					});
					MyBitmapInfo value = default(MyBitmapInfo);
					value.strFilename = xMLAttribute2;
					value.nX = int.Parse(array[0]);
					value.nY = int.Parse(array[1]);
					m_bitmapInfoByID[int.Parse(xMLAttribute)] = value;
				}
			}
		}

		/// <summary>
		/// Load the data from the "glyphs" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "glyphs" node's children</param>
		private void LoadFontXML_glyphs(XmlNodeList xnl)
		{
			foreach (XmlNode item in xnl)
			{
				if (item.Name == "glyph")
				{
					string xMLAttribute = GetXMLAttribute(item, "ch");
					string xMLAttribute2 = GetXMLAttribute(item, "bm");
					string xMLAttribute3 = GetXMLAttribute(item, "loc");
					string xMLAttribute4 = GetXMLAttribute(item, "size");
					string xMLAttribute5 = GetXMLAttribute(item, "aw");
					string xMLAttribute6 = GetXMLAttribute(item, "lsb");
					if (xMLAttribute3 == "")
					{
						xMLAttribute3 = GetXMLAttribute(item, "origin");
					}
					string[] array = xMLAttribute3.Split(new char[1]
					{
						','
					});
					string[] array2 = xMLAttribute4.Split(new char[1]
					{
						'x'
					});
					MyGlyphInfo value = new MyGlyphInfo
					{
						nBitmapID = ushort.Parse(xMLAttribute2),
						pxLocX = ushort.Parse(array[0]),
						pxLocY = ushort.Parse(array[1]),
						pxWidth = byte.Parse(array2[0]),
						pxHeight = byte.Parse(array2[1]),
						pxAdvanceWidth = byte.Parse(xMLAttribute5),
						pxLeftSideBearing = sbyte.Parse(xMLAttribute6)
					};
					m_glyphInfoByChar[xMLAttribute[0]] = value;
				}
			}
		}

		/// <summary>
		/// Load the data from the "kernpairs" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "kernpairs" node's children</param>
		private void LoadFontXML_kernpairs(XmlNodeList xnl)
		{
			foreach (XmlNode item in xnl)
			{
				if (item.Name == "kernpair")
				{
					char l = GetXMLAttribute(item, "left")[0];
					char r = GetXMLAttribute(item, "right")[0];
					string xMLAttribute = GetXMLAttribute(item, "adjust");
					KernPair key = new KernPair(l, r);
					m_kernByPair[key] = sbyte.Parse(xMLAttribute);
				}
			}
		}

		/// <summary>
		/// Get the XML attribute value
		/// </summary>
		/// <param name="n">XML node</param>
		/// <param name="strAttr">Attribute name</param>
		/// <returns>Attribute value, or the empty string if the attribute doesn't exist</returns>
		private static string GetXMLAttribute(XmlNode n, string strAttr)
		{
			XmlAttribute xmlAttribute = n.Attributes.GetNamedItem(strAttr) as XmlAttribute;
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
			return "";
		}
	}
}
