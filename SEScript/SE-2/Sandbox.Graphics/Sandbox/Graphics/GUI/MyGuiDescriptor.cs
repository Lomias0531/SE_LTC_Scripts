using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiDescriptor : MyDescriptor
	{
		private const string LINE_SPLIT_SEPARATOR = " | ";

		private static readonly List<char> m_splitBuffer = new List<char>(16);

		public MyGuiDescriptor(MyStringId name, MyStringId? description = null)
			: base(name, description)
		{
		}

		private static void SplitStringBuilder(StringBuilder destination, StringBuilder source, string splitSeparator)
		{
			int length = source.Length;
			int length2 = splitSeparator.Length;
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				char c = source[i];
				if (c == splitSeparator[num])
				{
					num++;
					if (num == length2)
					{
						destination.AppendLine();
						m_splitBuffer.Clear();
						num = 0;
					}
					else
					{
						m_splitBuffer.Add(c);
					}
				}
				else
				{
					if (num > 0)
					{
						foreach (char item in m_splitBuffer)
						{
							destination.Append(item);
						}
						m_splitBuffer.Clear();
						num = 0;
					}
					destination.Append(c);
				}
			}
			foreach (char item2 in m_splitBuffer)
			{
				destination.Append(item2);
			}
			m_splitBuffer.Clear();
		}

		protected override void UpdateDirty()
		{
			m_name = MyTexts.Get(base.NameEnum);
			m_description.Clear();
			if (base.DescriptionEnum.HasValue)
			{
				SplitStringBuilder(m_description, MyTexts.Get(base.DescriptionEnum.Value), " | ");
			}
		}
	}
}
