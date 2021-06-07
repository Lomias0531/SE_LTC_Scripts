using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyConstPropertyVector4 : MyConstProperty<Vector4>
	{
		public override string ValueType => "Vector4";

		public MyConstPropertyVector4()
		{
		}

		public MyConstPropertyVector4(string name)
			: base(name)
		{
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteElementString("W", ((Vector4)value).W.ToString());
			writer.WriteElementString("X", ((Vector4)value).X.ToString());
			writer.WriteElementString("Y", ((Vector4)value).Y.ToString());
			writer.WriteElementString("Z", ((Vector4)value).Z.ToString());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyUtils.DeserializeValue(reader, out Vector4 value2);
			value = value2;
		}

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyVector4 myConstPropertyVector = new MyConstPropertyVector4(base.Name);
			Duplicate(myConstPropertyVector);
			return myConstPropertyVector;
		}

		public static implicit operator Vector4(MyConstPropertyVector4 f)
		{
			return f.GetValue<Vector4>();
		}
	}
}
