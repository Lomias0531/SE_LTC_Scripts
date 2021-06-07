using System.Xml.Serialization;

namespace VRage.Game
{
	public struct MyVoxelMapModifierChange
	{
		[XmlAttribute(AttributeName = "From")]
		public string From;

		[XmlAttribute(AttributeName = "To")]
		public string To;
	}
}
