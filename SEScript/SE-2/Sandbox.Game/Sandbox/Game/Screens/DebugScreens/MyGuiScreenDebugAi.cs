using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System.Diagnostics;
using System.IO;
using VRage;
using VRage.Library.Utils;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "AI")]
	internal class MyGuiScreenDebugAi : MyGuiScreenDebugBase
	{
		private int m_ctr;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugAi";
		}

		public MyGuiScreenDebugAi()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Debug screen AI", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddButton("Test Chatbot", delegate
			{
				TestChatbot();
			});
			AddLabel("Options:", Color.OrangeRed.ToVector4(), 1f);
			AddCheckBox("Remove voxel navmesh cells", null, MemberHelper.GetMember(() => MyFakes.REMOVE_VOXEL_NAVMESH_CELLS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Debug draw bots", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BOTS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("    * Bot steering", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BOT_STEERING));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("    * Bot aiming", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BOT_AIMING));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("    * Bot navigation", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BOT_NAVIGATION));
			m_currentPosition.Y += 0.01f;
			AddLabel("Navmesh debug draw:", Color.OrangeRed.ToVector4(), 1f);
			AddCheckBox("Draw found path", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_FOUND_PATH));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Draw funnel path refining", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_FUNNEL));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Processed voxel navmesh cells", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_PROCESSED_VOXEL_CELLS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Prepared voxel navmesh cells", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_PREPARED_VOXEL_CELLS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Cells on paths", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_CELLS_ON_PATHS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Voxel navmesh connection helper", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_VOXEL_CONNECTION_HELPER));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("Draw navmesh links", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_LINKS));
			m_currentPosition.Y -= 0.005f;
			m_currentPosition.Y += 0.01f;
			AddLabel("Hierarchical pathfinding:", Color.OrangeRed.ToVector4(), 1f);
			AddCheckBox("Navmesh cell borders", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_CELL_BORDERS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("HPF (draw navmesh hierarchy)", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("    * (Lite version)", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY_LITE));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("    + Explored HL cells", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_EXPLORED_HL_CELLS));
			m_currentPosition.Y -= 0.005f;
			AddCheckBox("    + Fringe HL cells", null, MemberHelper.GetMember(() => MyFakes.DEBUG_DRAW_NAVMESH_FRINGE_HL_CELLS));
			m_currentPosition.Y -= 0.005f;
			m_currentPosition.Y += 0.01f;
			AddLabel("Winged-edge mesh debug draw:", Color.OrangeRed.ToVector4(), 1f);
			Vector2 currentPosition = m_currentPosition;
			AddCheckBox("    Lines", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.LINES) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.LINES) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.LINES));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			m_currentPosition = currentPosition + new Vector2(0.15f, 0f);
			AddCheckBox("    Lines Z-culled", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.LINES_DEPTH) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.LINES_DEPTH) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.LINES_DEPTH));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			m_currentPosition.X = currentPosition.X;
			currentPosition = m_currentPosition;
			AddCheckBox("    Edges", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.EDGES) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.EDGES) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.EDGES));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			m_currentPosition = currentPosition + new Vector2(0.15f, 0f);
			AddCheckBox("    Faces", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.FACES) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.FACES) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.FACES));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			m_currentPosition.X = currentPosition.X;
			currentPosition = m_currentPosition;
			AddCheckBox("    Vertices", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.VERTICES) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.VERTICES) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.VERTICES));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			m_currentPosition = currentPosition + new Vector2(0.15f, 0f);
			AddCheckBox("    Vertices detailed", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.VERTICES_DETAILED) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.VERTICES_DETAILED) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.VERTICES_DETAILED));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			m_currentPosition.X = currentPosition.X;
			currentPosition = m_currentPosition;
			AddCheckBox("    Normals", () => (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.NORMALS) != 0, delegate(bool b)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = (b ? (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.NORMALS) : (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.NORMALS));
			}, enabled: true, null, null, new Vector2(-0.15f, 0f));
			AddCheckBox("Animals", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ANIMALS));
			AddCheckBox("Spiders", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_FAUNA_COMPONENT));
			AddCheckBox("Switch Survival/Creative", () => MySession.Static.CreativeMode, delegate(bool b)
			{
				MySession.Static.Settings.GameMode = ((!b) ? MyGameModeEnum.Survival : MyGameModeEnum.Creative);
			});
		}

		private void TestChatbot()
		{
			StreamReader streamReader = new StreamReader("c:\\x\\stats.log");
			StreamWriter outF = new StreamWriter("c:\\x\\stats_resp.csv", append: false);
			m_ctr = 0;
			int num = 0;
			new MyTimeSpan(Stopwatch.GetTimestamp());
			while (!streamReader.EndOfStream)
			{
				string line = streamReader.ReadLine();
				MySession.Static.ChatBot?.FilterMessage("? " + line, delegate(string x)
				{
					outF.WriteLine("\"" + line + "\", \"" + x + "\"");
					m_ctr--;
				});
				MySandboxGame.Static.ProcessInvoke();
				m_ctr++;
				num++;
			}
			while (m_ctr != 0)
			{
				MySandboxGame.Static.ProcessInvoke();
			}
			streamReader.Close();
			outF.Close();
			new MyTimeSpan(Stopwatch.GetTimestamp());
		}
	}
}
