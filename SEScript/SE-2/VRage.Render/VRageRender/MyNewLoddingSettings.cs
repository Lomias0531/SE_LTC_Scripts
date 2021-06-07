using System.Xml.Serialization;

namespace VRageRender
{
	public class MyNewLoddingSettings
	{
		public MyPassLoddingSetting GBuffer = MyPassLoddingSetting.Default;

		private MyPassLoddingSetting[] m_cascadeDepth = new MyPassLoddingSetting[0];

		public MyPassLoddingSetting SingleDepth = MyPassLoddingSetting.Default;

		public MyPassLoddingSetting Forward = MyPassLoddingSetting.Default;

		public MyGlobalLoddingSettings Global = MyGlobalLoddingSettings.Default;

		[XmlArrayItem("CascadeDepth")]
		public MyPassLoddingSetting[] CascadeDepths
		{
			get
			{
				return m_cascadeDepth;
			}
			set
			{
				if (m_cascadeDepth.Length != value.Length)
				{
					m_cascadeDepth = new MyPassLoddingSetting[value.Length];
				}
				value.CopyTo(m_cascadeDepth, 0);
			}
		}

		public void CopyFrom(MyNewLoddingSettings settings)
		{
			GBuffer = settings.GBuffer;
			CascadeDepths = settings.CascadeDepths;
			SingleDepth = settings.SingleDepth;
			Forward = settings.Forward;
			Global = settings.Global;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MyNewLoddingSettings))
			{
				return false;
			}
			MyNewLoddingSettings myNewLoddingSettings = (MyNewLoddingSettings)obj;
			if (GBuffer.Equals(myNewLoddingSettings.GBuffer))
			{
				return false;
			}
			if (!CascadeDepths.Equals(myNewLoddingSettings.CascadeDepths))
			{
				return false;
			}
			if (SingleDepth.Equals(myNewLoddingSettings.SingleDepth))
			{
				return false;
			}
			if (Forward.Equals(myNewLoddingSettings.Forward))
			{
				return false;
			}
			if (Global.Equals(myNewLoddingSettings.Global))
			{
				return false;
			}
			return true;
		}
	}
}
