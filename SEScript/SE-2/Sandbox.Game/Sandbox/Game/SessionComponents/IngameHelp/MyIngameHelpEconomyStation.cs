using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents.IngameHelp
{
	internal class MyIngameHelpEconomyStation : MyIngameHelpObjective
	{
		private MyGps m_stationGPS;

		public MyIngameHelpEconomyStation()
		{
			TitleEnum = MySpaceTexts.IngameHelp_EconomyStation_Title;
			RequiredIds = new string[2]
			{
				"IngameHelp_Intro",
				"IngameHelp_HUD"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_EconomyStation_Desc
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_EconomyStation_DetailDesc,
					FinishCondition = OnAtCoordinates
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			RequiredCondition = FindStationAround;
		}

		private bool OnAtCoordinates()
		{
			if ((MySession.Static.LocalHumanPlayer.GetPosition() - m_stationGPS.Coords).LengthSquared() < 10000.0)
			{
				MySession.Static.Gpss.SendDelete(MySession.Static.LocalPlayerId, m_stationGPS.Hash);
				return true;
			}
			return false;
		}

		private bool FindStationAround()
		{
			if (MySession.Static.LocalHumanPlayer == null || MySession.Static.LocalHumanPlayer.Character == null)
			{
				return false;
			}
			float num = 150000f;
			Vector3D position = MySession.Static.LocalHumanPlayer.GetPosition();
			BoundingBoxD area = new BoundingBoxD(position - num, position + num);
			List<MyObjectSeed> list = new List<MyObjectSeed>();
			MyProceduralWorldGenerator.Static.GetOverlapAllBoundingBox<MyStationCellGenerator>(area, list);
			MySafeZone mySafeZone = null;
			float num2 = float.PositiveInfinity;
			foreach (MyObjectSeed item in list)
			{
				MyStation myStation = item.UserData as MyStation;
				if (myStation != null && MyEntities.TryGetEntityById(myStation.SafeZoneEntityId, out MyEntity entity))
				{
					MySafeZone mySafeZone2 = entity as MySafeZone;
					if (mySafeZone2 != null && (entity.PositionComp.GetPosition() - MySession.Static.LocalHumanPlayer.Character.WorldMatrix.Translation).LengthSquared() < (double)num2)
					{
						mySafeZone = mySafeZone2;
					}
				}
			}
			if (mySafeZone != null)
			{
				m_stationGPS = new MyGps();
				m_stationGPS.Name = MyTexts.GetString(MySpaceTexts.IngameHelp_Economy_GPSName);
				m_stationGPS.Description = MyTexts.GetString(MySpaceTexts.IngameHelp_Economy_GPSDesc);
				m_stationGPS.Coords = mySafeZone.PositionComp.GetPosition();
				m_stationGPS.ShowOnHud = true;
				m_stationGPS.DiscardAt = null;
				m_stationGPS.IsLocal = true;
				m_stationGPS.IsObjective = true;
				m_stationGPS.UpdateHash();
				MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref m_stationGPS, 0L);
				return true;
			}
			return false;
		}
	}
}
