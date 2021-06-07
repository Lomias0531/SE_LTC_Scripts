using Sandbox.Game.Gui;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityEnergyEstimatedTimeRemaining : MyStatBase
	{
		private readonly StringBuilder m_stringBuilder = new StringBuilder();

		public MyStatControlledEntityEnergyEstimatedTimeRemaining()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_estimated_time_remaining_energy");
		}

		public override void Update()
		{
			base.CurrentValue = MyHud.ShipInfo.FuelRemainingTime;
		}

		public override string ToString()
		{
			m_stringBuilder.Clear();
			MyValueFormatter.AppendTimeInBestUnit(base.CurrentValue * 3600f, m_stringBuilder);
			return m_stringBuilder.ToString();
		}
	}
}
