using Sandbox.Definitions;
using Sandbox.Game.Localization;
using System.Globalization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.Models
{
	[MyContractModelDescriptor(typeof(MyObjectBuilder_ContractDeliver))]
	public class MyContractModelDeliver : MyContractModel
	{
		private static readonly MyDefinitionId m_definitionId = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Deliver");

		private double m_deliverDistance;

		public double DeliverDistance
		{
			get
			{
				return m_deliverDistance;
			}
			set
			{
				SetProperty(ref m_deliverDistance, value, "DeliverDistance");
			}
		}

		public string DeliverDistance_Formatted
		{
			get
			{
				if (DeliverDistance < 10000.0)
				{
					return DeliverDistance.ToString("F0", CultureInfo.InvariantCulture) + " m";
				}
				return (DeliverDistance / 1000.0).ToString("F1", CultureInfo.InvariantCulture) + " km";
			}
		}

		public override string Description => MyTexts.GetString($"ContractScreen_Deliver_Description_{base.Id % 3}");

		public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			MyObjectBuilder_ContractDeliver myObjectBuilder_ContractDeliver = ob as MyObjectBuilder_ContractDeliver;
			if (myObjectBuilder_ContractDeliver != null)
			{
				DeliverDistance = myObjectBuilder_ContractDeliver.DeliverDistance;
			}
			ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(m_definitionId.SubtypeName);
			base.Init(ob, showFactionIcons);
		}

		protected override string BuildNameWithId(long id)
		{
			return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Deliver_WithId), id);
		}

		protected override string BuildName()
		{
			return MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Deliver);
		}
	}
}
