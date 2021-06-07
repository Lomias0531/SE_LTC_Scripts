using System.Xml.Serialization;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyOxygenBlock
	{
		[XmlIgnore]
		public MyOxygenRoomLink RoomLink
		{
			get;
			set;
		}

		public float PreviousOxygenAmount
		{
			get;
			set;
		}

		public int OxygenChangeTime
		{
			get;
			set;
		}

		public MyOxygenRoom Room
		{
			get
			{
				if (RoomLink == null)
				{
					return null;
				}
				return RoomLink.Room;
			}
		}

		public MyOxygenBlock()
		{
		}

		public MyOxygenBlock(MyOxygenRoomLink roomPointer)
		{
			RoomLink = roomPointer;
		}

		internal float OxygenAmount()
		{
			if (Room == null)
			{
				return 0f;
			}
			float value = Room.IsAirtight ? (Room.OxygenAmount / (float)Room.BlockCount) : Room.EnvironmentOxygen;
			float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - OxygenChangeTime) / 1500f;
			if (num > 1f)
			{
				num = 1f;
			}
			return MathHelper.Lerp(PreviousOxygenAmount, value, num);
		}

		public float OxygenLevel(float gridSize)
		{
			return OxygenAmount() / (gridSize * gridSize * gridSize);
		}

		public override string ToString()
		{
			return "MyOxygenBlock - Oxygen: " + OxygenAmount() + "/" + PreviousOxygenAmount;
		}
	}
}
