using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyConstPropertyVector3 : MyConstProperty<Vector3>
	{
		public override string ValueType => "Vector3";

		public MyConstPropertyVector3()
		{
		}

		public MyConstPropertyVector3(string name)
			: base(name)
		{
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteElementString("X", ((Vector3)value).X.ToString());
			writer.WriteElementString("Y", ((Vector3)value).Y.ToString());
			writer.WriteElementString("Z", ((Vector3)value).Z.ToString());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyUtils.DeserializeValue(reader, out Vector3 value2);
			value = value2;
		}

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyVector3 myConstPropertyVector = new MyConstPropertyVector3(base.Name);
			Duplicate(myConstPropertyVector);
			return myConstPropertyVector;
		}

		public static implicit operator Vector3(MyConstPropertyVector3 f)
		{
			return f.GetValue<Vector3>();
		}
	}
}
