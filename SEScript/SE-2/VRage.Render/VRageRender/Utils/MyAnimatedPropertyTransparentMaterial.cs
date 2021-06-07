using System.Xml;
using VRage.Utils;
using VRageRender.Animations;

namespace VRageRender.Utils
{
	public class MyAnimatedPropertyTransparentMaterial : MyAnimatedProperty<MyTransparentMaterial>
	{
		public override string ValueType => "String";

		public override string BaseValueType => "MyTransparentMaterial";

		public MyAnimatedPropertyTransparentMaterial()
		{
		}

		public MyAnimatedPropertyTransparentMaterial(string name)
			: this(name, null)
		{
		}

		public MyAnimatedPropertyTransparentMaterial(string name, InterpolatorDelegate interpolator)
			: base(name, interpolateAfterEnd: false, interpolator)
		{
		}

		protected override void Init()
		{
			Interpolator = MyTransparentMaterialInterpolator.Switch;
			base.Init();
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyTransparentMaterial myAnimatedPropertyTransparentMaterial = new MyAnimatedPropertyTransparentMaterial(base.Name);
			Duplicate(myAnimatedPropertyTransparentMaterial);
			return myAnimatedPropertyTransparentMaterial;
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

		protected override bool EqualsValues(object value1, object value2)
		{
			return ((MyTransparentMaterial)value1).Id.String == ((MyTransparentMaterial)value2).Id.String;
		}
	}
}
