using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_BankingSystemDefinition), null)]
	public class MyBankingSystemDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MyBankingSystemDefinition_003C_003EActor : IActivator, IActivator<MyBankingSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBankingSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBankingSystemDefinition CreateInstance()
			{
				return new MyBankingSystemDefinition();
			}

			MyBankingSystemDefinition IActivator<MyBankingSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Currency full name.
		/// </summary>
		public MyStringId CurrencyFullName
		{
			get;
			private set;
		}

		/// <summary>
		/// Currency short name.
		/// </summary>
		public MyStringId CurrencyShortName
		{
			get;
			private set;
		}

		/// <summary>
		/// Starting balance when account is created.
		/// </summary>
		public long StartingBalance
		{
			get;
			private set;
		}

		/// <summary>
		/// Max account log entriee.
		/// </summary>
		public uint AccountLogLen
		{
			get;
			private set;
		}

		/// <summary>
		/// Definition id of physical item representation of the currency
		/// </summary>
		public MyDefinitionId PhysicalItemId
		{
			get;
			private set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BankingSystemDefinition myObjectBuilder_BankingSystemDefinition = builder as MyObjectBuilder_BankingSystemDefinition;
			CurrencyFullName = MyStringId.GetOrCompute(myObjectBuilder_BankingSystemDefinition.CurrencyFullName);
			CurrencyShortName = MyStringId.GetOrCompute(myObjectBuilder_BankingSystemDefinition.CurrencyShortName);
			StartingBalance = myObjectBuilder_BankingSystemDefinition.StartingBalance;
			AccountLogLen = myObjectBuilder_BankingSystemDefinition.AccountLogLen;
			PhysicalItemId = myObjectBuilder_BankingSystemDefinition.PhysicalItemId;
		}
	}
}
