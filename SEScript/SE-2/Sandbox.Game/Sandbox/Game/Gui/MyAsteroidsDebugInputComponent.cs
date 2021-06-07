using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	internal class MyAsteroidsDebugInputComponent : MyDebugComponent
	{
		private bool m_drawSeeds;

		private bool m_drawTrackedEntities;

		private bool m_drawAroundCamera;

		private bool m_drawRadius;

		private bool m_drawDistance;

		private bool m_drawCells;

		private List<MyCharacter> m_plys = new List<MyCharacter>();

		private float m_originalFarPlaneDisatance = -1f;

		private float m_debugFarPlaneDistance = 1000000f;

		private bool m_fakeFarPlaneDistance;

		private List<MyObjectSeed> m_tmpSeedsList = new List<MyObjectSeed>();

		private List<MyProceduralCell> m_tmpCellsList = new List<MyProceduralCell>();

		public MyAsteroidsDebugInputComponent()
		{
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Enable Meteor Debug Draw", delegate
			{
				MyDebugDrawSettings.DEBUG_DRAW_METEORITS_DIRECTIONS = true;
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Spawn meteor shower", delegate
			{
				MyMeteorShower.StartDebugWave(MySession.Static.LocalCharacter.WorldMatrix.Translation);
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Spawn small asteroid", delegate
			{
				Vector3 value = MySession.Static.LocalCharacter.WorldMatrix.Translation;
				Vector3 vector = MySession.Static.LocalCharacter.WorldMatrix.Forward;
				MyMeteor.SpawnRandom(value + vector * 2f, vector);
				return true;
			});
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Spawn crater", delegate
			{
				SpawnCrater();
				return true;
			});
		}

		private void SpawnCrater()
		{
			Vector3 vector = MySession.Static.LocalCharacter.WorldMatrix.Translation;
			Vector3 value = MySession.Static.LocalCharacter.WorldMatrix.Forward;
			MyPhysics.CastRay(vector, vector + value * 100f);
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			return base.HandleInput();
		}

		public override void Draw()
		{
			base.Draw();
			if (MySession.Static != null && MySector.MainCamera != null && MyProceduralWorldGenerator.Static != null)
			{
				if (m_drawAroundCamera)
				{
					MyProceduralWorldGenerator.Static.OverlapAllPlanetSeedsInSphere(new BoundingSphereD(MySector.MainCamera.Position, MySector.MainCamera.FarPlaneDistance * 2f), m_tmpSeedsList);
				}
				MyProceduralWorldGenerator.Static.GetAllExisting(m_tmpSeedsList);
				double num = 720000.0;
				foreach (MyObjectSeed tmpSeeds in m_tmpSeedsList)
				{
					if (m_drawSeeds)
					{
						Vector3D center = tmpSeeds.BoundingVolume.Center;
						MyRenderProxy.DebugDrawSphere(center, tmpSeeds.Size / 2f, (tmpSeeds.Params.Type == MyObjectSeedType.Asteroid) ? Color.Green : Color.Red);
						if (m_drawRadius)
						{
							MyRenderProxy.DebugDrawText3D(center, $"{tmpSeeds.Size:0}m", Color.Yellow, 0.8f, depthRead: true);
						}
						if (m_drawDistance)
						{
							double num2 = (center - MySector.MainCamera.Position).Length();
							MyRenderProxy.DebugDrawText3D(center, $"{num2 / 1000.0:0.0}km", Color.Lerp(Color.Green, Color.Red, (float)(num2 / num)), 0.8f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
						}
					}
				}
				m_tmpSeedsList.Clear();
				if (m_drawTrackedEntities)
				{
					foreach (KeyValuePair<MyEntity, MyEntityTracker> trackedEntity in MyProceduralWorldGenerator.Static.GetTrackedEntities())
					{
						MyRenderProxy.DebugDrawSphere(trackedEntity.Value.CurrentPosition, (float)trackedEntity.Value.BoundingVolume.Radius, Color.White);
					}
				}
				if (m_drawCells)
				{
					MyProceduralWorldGenerator.Static.GetAllExistingCells(m_tmpCellsList);
					foreach (MyProceduralCell tmpCells in m_tmpCellsList)
					{
						MyRenderProxy.DebugDrawAABB(tmpCells.BoundingVolume, Color.Blue);
					}
				}
				m_tmpCellsList.Clear();
				MyRenderProxy.DebugDrawSphere(Vector3D.Zero, 0f, Color.White, 0f, depthRead: false);
			}
		}

		public override string GetName()
		{
			return "Asteroids";
		}
	}
}
