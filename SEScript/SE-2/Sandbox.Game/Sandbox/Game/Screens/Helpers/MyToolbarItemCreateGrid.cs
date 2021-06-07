using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemCreateGrid))]
	public class MyToolbarItemCreateGrid : MyToolbarItemDefinition
	{
		private static MyStringHash CreateSmallShip;

		private static MyStringHash CreateLargeShip;

		private static MyStringHash CreateStation;

		static MyToolbarItemCreateGrid()
		{
			CreateSmallShip = MyStringHash.GetOrCompute("CreateSmallShip");
			CreateLargeShip = MyStringHash.GetOrCompute("CreateLargeShip");
			CreateStation = MyStringHash.GetOrCompute("CreateStation");
		}

		public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
		{
			base.Init(objBuilder);
			base.WantsToBeSelected = false;
			base.WantsToBeActivated = true;
			base.ActivateOnClick = true;
			return true;
		}

		private void CreateGrid(MyCubeSize cubeSize, bool isStatic)
		{
			if (!MySandboxGame.IsPaused)
			{
				MySessionComponentVoxelHand.Static.Enabled = false;
				MyCubeBuilder.Static.StartStaticGridPlacement(cubeSize, isStatic);
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (localCharacter != null)
				{
					MyDefinitionId weaponDefinition = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
					localCharacter.SwitchToWeapon(weaponDefinition);
				}
			}
		}

		public override bool Activate()
		{
			if (Definition.Id.SubtypeId == CreateStation)
			{
				CreateGrid(MyCubeSize.Large, isStatic: true);
			}
			return false;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			if (type != 0)
			{
				return type == MyToolbarType.Spectator;
			}
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			return ChangeInfo.None;
		}
	}
}
