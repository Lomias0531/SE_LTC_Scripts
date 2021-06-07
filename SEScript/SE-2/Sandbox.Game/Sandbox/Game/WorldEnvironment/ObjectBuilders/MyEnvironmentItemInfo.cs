using System.Xml.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
	public class MyEnvironmentItemInfo
	{
		[XmlAttribute]
		public string Type;

		public MyStringHash Subtype;

		[XmlAttribute]
		public float Offset;

		[XmlAttribute]
		public float Density;

		[XmlAttribute("Subtype")]
		public string SubtypeText
		{
			get
			{
				return Subtype.ToString();
			}
			set
			{
				Subtype = MyStringHash.GetOrCompute(value);
			}
		}
	}
}
