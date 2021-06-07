using Sandbox.Definitions;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.Models
{
	[MyContractModelDescriptor(typeof(MyObjectBuilder_ContractObtainAndDeliver))]
	public class MyContractModelObtainAndDeliver : MyContractModel
	{
		private static readonly MyDefinitionId m_definitionId = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "ObtainAndDeliver");

		public override string Description => MyTexts.GetString($"ContractScreen_ObtainDeliver_Description_{base.Id % 6}");

		public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(m_definitionId.SubtypeName);
			base.Init(ob, showFactionIcons);
		}

		protected override string BuildNameWithId(long id)
		{
			return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_ObtainAndDeliver_WithId), id);
		}

		protected override string BuildName()
		{
			return MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_ObtainAndDeliver);
		}
	}
}
