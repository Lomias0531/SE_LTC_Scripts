using EmptyKeys.UserInterface.Media.Imaging;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.Models
{
	[MyContractConditionModelDescriptor(typeof(MyObjectBuilder_ContractConditionCustom))]
	public class MyContractConditionCustomModel : MyContractConditionModel
	{
		public override void Init(MyObjectBuilder_ContractCondition ob)
		{
			base.Init(ob);
			_ = (ob is MyObjectBuilder_ContractConditionCustom);
		}

		protected override string BuildName(long id)
		{
			return string.Empty;
		}

		protected override BitmapImage BuildImage()
		{
			return new BitmapImage
			{
				TextureAsset = "Textures\\GUI\\Icons\\WeaponWelder_1.dds"
			};
		}
	}
}
