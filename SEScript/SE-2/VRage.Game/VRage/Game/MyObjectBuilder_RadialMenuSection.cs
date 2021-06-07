using System.ComponentModel;
using System.Xml.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	public class MyObjectBuilder_RadialMenuSection
	{
		public MyStringId Label;

		[DefaultValue(null)]
		[XmlArrayItem("Item", typeof(MyAbstractXmlSerializer<MyObjectBuilder_RadialMenuItem>))]
		public MyObjectBuilder_RadialMenuItem[] Items;
	}
}
