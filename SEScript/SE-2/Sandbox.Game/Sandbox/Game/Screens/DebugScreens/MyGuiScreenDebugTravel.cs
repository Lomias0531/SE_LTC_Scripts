using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Travel")]
	internal class MyGuiScreenDebugTravel : MyGuiScreenDebugBase
	{
		private static Dictionary<string, Vector3> s_travelPoints = new Dictionary<string, Vector3>
		{
			{
				"Mercury",
				new Vector3(-39f, 0f, 46f)
			},
			{
				"Venus",
				new Vector3(-2f, 0f, 108f)
			},
			{
				"Earth",
				new Vector3(101f, 0f, -111f)
			},
			{
				"Moon",
				new Vector3(101f, 0f, -111f) + new Vector3(-0.015f, 0f, -0.2f)
			},
			{
				"Mars",
				new Vector3(-182f, 0f, 114f)
			},
			{
				"Jupiter",
				new Vector3(-778f, 0f, 155.6f)
			},
			{
				"Saturn",
				new Vector3(1120f, 0f, -840f)
			},
			{
				"Uranus",
				new Vector3(-2700f, 0f, -1500f)
			},
			{
				"Zero",
				new Vector3(0f, 0f, 0f)
			},
			{
				"Billion",
				new Vector3(1000f)
			},
			{
				"BillionFlat0",
				new Vector3(999f, 1000f, 1000f)
			},
			{
				"BillionFlat1",
				new Vector3(1001f, 1000f, 1000f)
			}
		};

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDrawSettings";
		}

		public MyGuiScreenDebugTravel()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Travel", Color.Yellow.ToVector4());
			AddShareFocusHint();
			foreach (KeyValuePair<string, Vector3> travelPair in s_travelPoints)
			{
				AddButton(new StringBuilder(travelPair.Key), delegate
				{
					TravelTo(travelPair.Value);
				});
			}
			AddCheckBox("Testing jumpdrives", null, MemberHelper.GetMember(() => MyFakes.TESTING_JUMPDRIVE));
		}

		private void TravelTo(Vector3 positionInMilions)
		{
			MyMultiplayer.TeleportControlledEntity((Vector3D)positionInMilions * 1000000.0);
		}
	}
}
