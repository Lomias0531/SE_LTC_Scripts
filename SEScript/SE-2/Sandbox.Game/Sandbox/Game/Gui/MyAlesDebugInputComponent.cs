using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal class MyAlesDebugInputComponent : MyDebugComponent
	{
		protected sealed class TravelToWaypoint_003C_003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D pos, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TravelToWaypoint(pos);
			}
		}

		private bool m_questlogOpened;

		private MyGuiScreenBase guiScreen;

		private static MyRandom random = new MyRandom();

		private MyRandom m_random;

		public override string GetName()
		{
			return "Ales";
		}

		public MyAlesDebugInputComponent()
		{
			m_random = new MyRandom();
			AddShortcut(MyKeys.U, newPress: true, control: false, shift: false, alt: false, () => "Reload particles", delegate
			{
				ReloadParticleDefinition();
				return true;
			});
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Teleport to gps", delegate
			{
				TravelToWaypointClient();
				return true;
			});
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Init questlog", delegate
			{
				ToggleQuestlog();
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Show/Hide QL", delegate
			{
				m_questlogOpened = !m_questlogOpened;
				MyHud.Questlog.Visible = m_questlogOpened;
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "QL: Prew page", () => true);
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "QL: Next page", () => true);
			int shortLine = 30;
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "QL: Add short line", delegate
			{
				MyHud.Questlog.AddDetail(RandomString(shortLine));
				return true;
			});
			int longLine = 60;
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "QL: Add long line", delegate
			{
				MyHud.Questlog.AddDetail(RandomString(longLine));
				return true;
			});
		}

		private void ToggleQuestlog()
		{
			MyHud.Questlog.QuestTitle = "Test Questlog title message";
			MyHud.Questlog.CleanDetails();
		}

		private void TravelToWaypointClient()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenDialogTeleportCheat());
		}

		[Event(null, 207)]
		[Reliable]
		[Server]
		public static void TravelToWaypoint(Vector3D pos)
		{
			MyMultiplayer.TeleportControlledEntity(pos);
		}

		private void ReloadParticleDefinition()
		{
			MyDefinitionManager.Static.ReloadParticles();
		}

		public static string RandomString(int length)
		{
			return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789           ", length)
				select s[random.Next(s.Length)]).ToArray()).Trim();
		}
	}
}
