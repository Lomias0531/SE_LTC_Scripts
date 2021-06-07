using System.Xml.Serialization;

namespace VRage.Game
{
	public class MyVoxelMapModifierOption
	{
		[XmlAttribute(AttributeName = "Chance")]
		public float Chance;

		[XmlElement("Change")]
		public MyVoxelMapModifierChange[] Changes;
	}
}
