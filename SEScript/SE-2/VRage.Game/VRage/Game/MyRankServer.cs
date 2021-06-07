using System.Xml.Serialization;

namespace VRage.Game
{
	public class MyRankServer
	{
		[XmlAttribute]
		public int Rank
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Address
		{
			get;
			set;
		}
	}
}
