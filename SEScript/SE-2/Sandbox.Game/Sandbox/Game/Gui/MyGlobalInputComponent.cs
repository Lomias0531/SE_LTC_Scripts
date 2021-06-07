using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using System.Linq;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;

namespace Sandbox.Game.Gui
{
	internal class MyGlobalInputComponent : MyDebugComponent
	{
		public override string GetName()
		{
			return "Global";
		}

		public MyGlobalInputComponent()
		{
			AddShortcut(MyKeys.Space, newPress: true, control: true, shift: false, alt: false, () => "Teleport controlled object to camera position", delegate
			{
				if (MySession.Static.CameraController == MySpectator.Static)
				{
					MyMultiplayer.TeleportControlledEntity(MySpectator.Static.Position);
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Apply backward linear impulse x100", delegate
			{
				MyPhysicsComponentBase physics2 = MySession.Static.ControlledEntity.Entity.GetTopMostParent().Physics;
				if (physics2 != null && physics2.RigidBody != null)
				{
					physics2.RigidBody.ApplyLinearImpulse(MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward * physics2.Mass * -100.0);
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Apply linear impulse x100", delegate
			{
				MyPhysicsComponentBase physics = MySession.Static.ControlledEntity.Entity.GetTopMostParent().Physics;
				if (physics != null && physics.RigidBody != null)
				{
					physics.RigidBody.ApplyLinearImpulse(MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward * physics.Mass * 100.0);
				}
				return true;
			});
			AddShortcut(MyKeys.Z, newPress: true, control: true, shift: true, alt: false, () => "Save clipboard as prefab", delegate
			{
				MyClipboardComponent.Static.Clipboard.SaveClipboardAsPrefab();
				return true;
			});
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => (MySessionComponentReplay.Static == null || !MySessionComponentReplay.Static.IsReplaying) ? "Replay" : "Stop replaying", delegate
			{
				if (MySessionComponentReplay.Static != null)
				{
					if (!MySessionComponentReplay.Static.IsReplaying)
					{
						MySessionComponentReplay.Static.StartReplay();
					}
					else
					{
						MySessionComponentReplay.Static.StopReplay();
					}
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => (MySessionComponentReplay.Static == null || !MySessionComponentReplay.Static.IsRecording) ? "Record + Replay" : "Stop recording ", delegate
			{
				if (MySessionComponentReplay.Static != null)
				{
					if (!MySessionComponentReplay.Static.IsRecording)
					{
						MySessionComponentReplay.Static.StartRecording();
						MySessionComponentReplay.Static.StartReplay();
					}
					else
					{
						MySessionComponentReplay.Static.StopRecording();
						MySessionComponentReplay.Static.StopReplay();
					}
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Delete recordings", delegate
			{
				MySessionComponentReplay.Static.DeleteRecordings();
				return true;
			});
			AddShortcut(MyKeys.U, newPress: true, control: false, shift: false, alt: false, () => "Add character", delegate
			{
				MyCharacterInputComponent.SpawnCharacter();
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Toggle all handbrakes", delegate
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					foreach (IMyEntity item in entity.Hierarchy.Children.Select((MyHierarchyComponentBase x) => x.Entity))
					{
						MyCockpit myCockpit;
						if ((myCockpit = (item as MyCockpit)) != null)
						{
							myCockpit.SwitchHandbrake();
						}
					}
				}
				return true;
			});
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			if (base.HandleInput())
			{
				return true;
			}
			return false;
		}
	}
}
