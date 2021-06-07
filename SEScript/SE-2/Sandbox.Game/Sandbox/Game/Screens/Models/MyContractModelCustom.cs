using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.Models
{
	[MyContractModelDescriptor(typeof(MyObjectBuilder_ContractCustom))]
	public class MyContractModelCustom : MyContractModel
	{
		private MyDefinitionId m_definitionId;

		private string m_name;

		private string m_description;

		public override string Description => m_description;

		public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			MyObjectBuilder_ContractCustom myObjectBuilder_ContractCustom;
			if ((myObjectBuilder_ContractCustom = (ob as MyObjectBuilder_ContractCustom)) != null)
			{
				m_definitionId = myObjectBuilder_ContractCustom.DefinitionId;
				m_name = myObjectBuilder_ContractCustom.ContractName;
				m_description = myObjectBuilder_ContractCustom.ContractDescription;
			}
			ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(m_definitionId.SubtypeName);
			base.Init(ob, showFactionIcons);
		}

		protected override string BuildNameWithId(long id)
		{
			return $"{m_name} {id}";
		}

		protected override string BuildName()
		{
			return m_name;
		}
	}
}
