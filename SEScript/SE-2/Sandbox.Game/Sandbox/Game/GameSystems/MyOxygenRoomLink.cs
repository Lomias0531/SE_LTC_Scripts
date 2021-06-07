namespace Sandbox.Game.GameSystems
{
	public class MyOxygenRoomLink
	{
		public MyOxygenRoom Room
		{
			get;
			set;
		}

		public MyOxygenRoomLink(MyOxygenRoom room)
		{
			SetRoom(room);
		}

		private void SetRoom(MyOxygenRoom room)
		{
			Room = room;
			Room.Link = this;
		}
	}
}
