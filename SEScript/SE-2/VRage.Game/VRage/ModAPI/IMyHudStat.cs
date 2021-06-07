using VRage.Utils;

namespace VRage.ModAPI
{
	public interface IMyHudStat
	{
		MyStringHash Id
		{
			get;
		}

		float CurrentValue
		{
			get;
		}

		float MaxValue
		{
			get;
		}

		float MinValue
		{
			get;
		}

		void Update();

		string GetValueString();
	}
}
