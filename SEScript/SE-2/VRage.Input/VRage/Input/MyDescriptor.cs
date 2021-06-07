using System.Text;
using VRage.Utils;

namespace VRage.Input
{
	public abstract class MyDescriptor
	{
		private bool m_isDirty = true;

		protected StringBuilder m_name;

		protected StringBuilder m_description;

		private MyStringId? m_descriptionEnum;

		private MyStringId m_nameEnum;

		public MyStringId? DescriptionEnum
		{
			get
			{
				return m_descriptionEnum;
			}
			set
			{
				if (value != m_descriptionEnum)
				{
					m_descriptionEnum = value;
					m_isDirty = true;
				}
			}
		}

		public MyStringId NameEnum
		{
			get
			{
				return m_nameEnum;
			}
			set
			{
				if (value != m_nameEnum)
				{
					m_nameEnum = value;
					m_isDirty = true;
				}
			}
		}

		public StringBuilder Name
		{
			get
			{
				if (m_isDirty)
				{
					m_isDirty = false;
					UpdateDirty();
				}
				return m_name;
			}
		}

		public StringBuilder Description
		{
			get
			{
				if (m_isDirty)
				{
					m_isDirty = false;
					UpdateDirty();
				}
				return m_description;
			}
		}

		public MyDescriptor(MyStringId name, MyStringId? description = null)
		{
			m_nameEnum = name;
			DescriptionEnum = description;
		}

		protected abstract void UpdateDirty();
	}
}
