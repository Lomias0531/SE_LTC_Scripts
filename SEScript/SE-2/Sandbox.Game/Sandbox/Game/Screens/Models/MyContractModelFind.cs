using Sandbox.Definitions;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.Models
{
	[MyContractModelDescriptor(typeof(MyObjectBuilder_ContractFind))]
	public class MyContractModelFind : MyContractModel
	{
		private static readonly MyDefinitionId m_definitionId = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Find");

		private double m_gpsDistance;

		private float m_maxGpsOffset;

		public double GpsDistance
		{
			get
			{
				return m_gpsDistance;
			}
			set
			{
				SetProperty(ref m_gpsDistance, value, "GpsDistance");
			}
		}

		public float MaxGpsOffset
		{
			get
			{
				return m_maxGpsOffset;
			}
			set
			{
				SetProperty(ref m_maxGpsOffset, value, "MaxGpsOffset");
				RaisePropertyChanged("MaxGpsOffset_Formatted");
			}
		}

		public string MaxGpsOffset_Formatted
		{
			get
			{
				if (m_maxGpsOffset < 2000f)
				{
					return $"{m_maxGpsOffset} m";
				}
				return $"{m_maxGpsOffset / 1000f} km";
			}
		}

		public override string Description => MyTexts.GetString($"ContractScreen_Find_Description_{base.Id % 5}");

		public override void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
		{
			MyObjectBuilder_ContractFind myObjectBuilder_ContractFind = ob as MyObjectBuilder_ContractFind;
			if (myObjectBuilder_ContractFind != null)
			{
				GpsDistance = myObjectBuilder_ContractFind.GpsDistance;
				MaxGpsOffset = myObjectBuilder_ContractFind.MaxGpsOffset;
			}
			ContractTypeDefinition = MyDefinitionManager.Static.GetContractType(m_definitionId.SubtypeName);
			base.Init(ob, showFactionIcons);
		}

		protected override string BuildNameWithId(long id)
		{
			return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Find_WithId), id);
		}

		protected override string BuildName()
		{
			return MyTexts.GetString(MySpaceTexts.ContractScreen_Contract_Name_Find);
		}
	}
}
