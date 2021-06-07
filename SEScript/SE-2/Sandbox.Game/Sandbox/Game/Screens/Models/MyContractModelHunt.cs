using Sandbox.Definitions;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.Models
{
	[MyContractModelDescriptor(typeof(MyObjectBuilder_ContractHunt))]
	public class MyContractModelHunt : MyContractModel
	{
		private static readonly MyDefinitionId m_definitionId = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Hunt");

		private long m_targetId;

		public long TargetId
		{
			get
			{
				return m_targetId;
			}
			set
			{
				SetProperty(ref m_targetId, value, "TargetId");
			}
		}

		public string TargetName_Formatted
		{
			get
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(TargetId);
				if (myIdentity == null)
				{
					return "Missing Identity";
				}
				return myIdentity.DisplayName + (myIdentity.IsDead ? " (Offline)" : string.Empty);
			}
		}

		public override string Description => MyTexts.GetString($"ContractScreen_Hunt_Description_{base.Id % 3}");

		public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			MyObjectBuilder_ContractHunt myObjectBuilder_ContractHunt = ob as MyObjectBuilder_ContractHunt;
			if (myObjectBuilder_ContractHunt != null)
			{
				TargetId = myObjectBuilder_ContractHunt.Target;
			}
			ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(m_definitionId.SubtypeName);
			base.Init(ob, showFactionIcons);
		}

		protected override string BuildNameWithId(long id)
		{
			return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Hunt_WithId), id);
		}

		protected override string BuildName()
		{
			return MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Hunt);
		}
	}
}
