using System.Xml;
using VRage.Utils;
using VRageRender.Animations;

namespace VRageRender.Utils
{
	public class MyConstPropertyTransparentMaterial : MyConstProperty<MyTransparentMaterial>
	{
		public override string ValueType => "String";

		public override string BaseValueType => "MyTransparentMaterial";

		public MyConstPropertyTransparentMaterial()
		{
		}

		public MyConstPropertyTransparentMaterial(string name)
			: base(name)
		{
		}

		protected override void Init()
		{
			base.Init();
		}

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyTransparentMaterial myConstPropertyTransparentMaterial = new MyConstPropertyTransparentMaterial(base.Name);
			Duplicate(myConstPropertyTransparentMaterial);
			return myConstPropertyTransparentMaterial;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(((MyTransparentMaterial)value).Id.String);
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = MyTransparentMaterials.GetMaterial(MyStringId.GetOrCompute((string)value));
		}
	}
}
