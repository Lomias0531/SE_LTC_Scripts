using System.Xml.Serialization;

namespace VRage
{
	public interface IMyXmlSerializable : IXmlSerializable
	{
		object Data
		{
			get;
		}
	}
}
