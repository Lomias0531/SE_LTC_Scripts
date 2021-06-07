using System.Collections.Generic;
using System.Xml.Serialization;

namespace VRage.Game.News
{
	[XmlRoot(ElementName = "News")]
	public class MyNews
	{
		[XmlElement("Entry")]
		public List<MyNewsEntry> Entry;
	}
}
