using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI.Ingame
{
	public interface IMyInventoryItem
	{
		MyFixedPoint Amount
		{
			get;
			set;
		}

		float Scale
		{
			get;
			set;
		}

		MyObjectBuilder_Base Content
		{
			get;
			set;
		}

		uint ItemId
		{
			get;
			set;
		}
	}
}
