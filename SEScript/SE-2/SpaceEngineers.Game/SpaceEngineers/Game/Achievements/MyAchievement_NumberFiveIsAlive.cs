using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.ModAPI;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_NumberFiveIsAlive : MySteamAchievementBase
	{
		public override bool NeedsUpdate => !MySession.Static.CreativeMode;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_NumberFiveIsAlive", null, 0f);
		}

		public override void SessionUpdate()
		{
			if (base.IsAchieved || MySession.Static.LocalCharacter == null)
			{
				return;
			}
			IMyEntity temporaryConnectedEntity = MySession.Static.LocalCharacter.SuitBattery.ResourceSink.TemporaryConnectedEntity;
			if ((double)MySession.Static.LocalCharacter.SuitEnergyLevel < 0.01 && temporaryConnectedEntity != null && temporaryConnectedEntity != MySession.Static.LocalCharacter)
			{
				MyMedicalRoom myMedicalRoom = temporaryConnectedEntity as MyMedicalRoom;
				MyCockpit myCockpit;
				if (myMedicalRoom != null && myMedicalRoom.IsWorking && myMedicalRoom.RefuelAllowed)
				{
					NotifyAchieved();
				}
				else if ((myCockpit = (temporaryConnectedEntity as MyCockpit)) != null && myCockpit.hasPower)
				{
					NotifyAchieved();
				}
			}
		}
	}
}
