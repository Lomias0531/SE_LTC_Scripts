using Sandbox.Definitions;
using Sandbox.Game.Localization;
using System.Globalization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.Models
{
	[MyContractModelDescriptor(typeof(MyObjectBuilder_ContractEscort))]
	public class MyContractModelEscort : MyContractModel
	{
		private static readonly MyDefinitionId m_definitionId = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Escort");

		private double m_pathLength;

		public double PathLength
		{
			get
			{
				return m_pathLength;
			}
			set
			{
				SetProperty(ref m_pathLength, value, "PathLength");
			}
		}

		public string PathLength_Formatted
		{
			get
			{
				if (PathLength < 10000.0)
				{
					return PathLength.ToString("F0", CultureInfo.InvariantCulture) + " m";
				}
				return (PathLength / 1000.0).ToString("F1", CultureInfo.InvariantCulture) + " km";
			}
		}

		public override string Description => MyTexts.GetString($"ContractScreen_Escort_Description_{base.Id % 4}");

		public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			MyObjectBuilder_ContractEscort myObjectBuilder_ContractEscort = ob as MyObjectBuilder_ContractEscort;
			if (myObjectBuilder_ContractEscort != null)
			{
				PathLength = myObjectBuilder_ContractEscort.PathLength;
			}
			ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(m_definitionId.SubtypeName);
			base.Init(ob, showFactionIcons);
		}

		protected override string BuildNameWithId(long id)
		{
			return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Escort_WithId), id);
		}

		protected override string BuildName()
		{
			return MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Escort);
		}
	}
}
