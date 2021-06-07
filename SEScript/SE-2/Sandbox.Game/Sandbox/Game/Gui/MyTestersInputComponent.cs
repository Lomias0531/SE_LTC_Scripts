using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTestersInputComponent : MyDebugComponent
	{
		public MyTestersInputComponent()
		{
			AddShortcut(MyKeys.Back, newPress: true, control: true, shift: false, alt: false, () => "Freeze cube builder gizmo", delegate
			{
				MyCubeBuilder.Static.FreezeGizmo = !MyCubeBuilder.Static.FreezeGizmo;
				return true;
			});
			AddShortcut(MyKeys.NumPad0, newPress: false, control: false, shift: false, alt: false, () => "Add items to inventory (continuous)", delegate
			{
				AddItemsToInventory(0);
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Add items to inventory", delegate
			{
				AddItemsToInventory(1);
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Add components to inventory", delegate
			{
				AddItemsToInventory(2);
				return true;
			});
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Fill inventory with iron", FillInventoryWithIron);
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Add to inventory dialog...", delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenDialogInventoryCheat());
				return true;
			});
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Set container type", SetContainerType);
			AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Toggle debug draw", ToggleDebugDraw);
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Save the game", delegate
			{
				MyAsyncSaving.Start();
				return true;
			});
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "SpawnCargoShip", SpawnCargoShip);
		}

		private bool SpawnCargoShip()
		{
			MyNeutralShipSpawner.OnGlobalSpawnEvent(null);
			return true;
		}

		public override string GetName()
		{
			return "Testers";
		}

		public bool AddItems(MyInventory inventory, MyObjectBuilder_PhysicalObject obj, bool overrideCheck)
		{
			return AddItems(inventory, obj, overrideCheck, 1);
		}

		public bool AddItems(MyInventory inventory, MyObjectBuilder_PhysicalObject obj, bool overrideCheck, MyFixedPoint amount)
		{
			if (overrideCheck || !inventory.ContainItems(amount, obj))
			{
				if (inventory.CanItemsBeAdded(amount, obj.GetId()))
				{
					inventory.AddItems(amount, obj);
					return true;
				}
				return false;
			}
			return false;
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogInventoryCheat)
			{
				return false;
			}
			return base.HandleInput();
		}

		private static bool ToggleDebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
				MyDebugDrawSettings.DEBUG_DRAW_EVENTS = false;
			}
			else
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				MyDebugDrawSettings.DEBUG_DRAW_EVENTS = true;
			}
			return true;
		}

		private static bool SetContainerType()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return false;
			}
			Matrix matrix = localCharacter.GetHeadMatrix(includeY: true);
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(matrix.Translation, matrix.Translation + matrix.Forward * 100f, list);
			if (list.Count == 0)
			{
				return false;
			}
			MyPhysics.HitInfo hitInfo = list.FirstOrDefault();
			if (hitInfo.HkHitInfo.Body == null)
			{
				return false;
			}
			IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
			if (!(hitEntity is MyCargoContainer))
			{
				return false;
			}
			MyGuiSandbox.AddScreen(new MyGuiScreenDialogContainerType(hitEntity as MyCargoContainer));
			return true;
		}

		private static bool FillInventoryWithIron()
		{
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity != null && myEntity.HasInventory)
			{
				MyFixedPoint myFixedPoint = 20000;
				MyObjectBuilder_Ore myObjectBuilder_Ore = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Iron");
				MyInventory inventory = myEntity.GetInventory();
				myFixedPoint = inventory.ComputeAmountThatFits(myObjectBuilder_Ore.GetId());
				inventory.AddItems(myFixedPoint, myObjectBuilder_Ore);
			}
			return true;
		}

		private void AddItemsToInventory(int variant)
		{
			bool overrideCheck = variant != 0;
			bool flag = variant != 0;
			bool flag2 = variant == 2;
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity == null || !myEntity.HasInventory)
			{
				return;
			}
			MyInventory inventory = myEntity.GetInventory();
			if (!flag2)
			{
				MyObjectBuilder_AmmoMagazine myObjectBuilder_AmmoMagazine = new MyObjectBuilder_AmmoMagazine();
				myObjectBuilder_AmmoMagazine.SubtypeName = "NATO_5p56x45mm";
				myObjectBuilder_AmmoMagazine.ProjectilesCount = 50;
				AddItems(inventory, myObjectBuilder_AmmoMagazine, overrideCheck: false, 5);
				MyObjectBuilder_AmmoMagazine myObjectBuilder_AmmoMagazine2 = new MyObjectBuilder_AmmoMagazine();
				myObjectBuilder_AmmoMagazine2.SubtypeName = "NATO_25x184mm";
				myObjectBuilder_AmmoMagazine2.ProjectilesCount = 50;
				AddItems(inventory, myObjectBuilder_AmmoMagazine2, overrideCheck: false);
				MyObjectBuilder_AmmoMagazine myObjectBuilder_AmmoMagazine3 = new MyObjectBuilder_AmmoMagazine();
				myObjectBuilder_AmmoMagazine3.SubtypeName = "Missile200mm";
				myObjectBuilder_AmmoMagazine3.ProjectilesCount = 50;
				AddItems(inventory, myObjectBuilder_AmmoMagazine3, overrideCheck: false);
				AddItems(inventory, CreateGunContent("AutomaticRifleItem"), overrideCheck: false);
				AddItems(inventory, CreateGunContent("WelderItem"), overrideCheck: false);
				AddItems(inventory, CreateGunContent("AngleGrinderItem"), overrideCheck: false);
				AddItems(inventory, CreateGunContent("HandDrillItem"), overrideCheck: false);
			}
			foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
			{
				if ((!(allDefinition.Id.TypeId != typeof(MyObjectBuilder_Component)) || !(allDefinition.Id.TypeId != typeof(MyObjectBuilder_Ingot))) && (!flag2 || !(allDefinition.Id.TypeId != typeof(MyObjectBuilder_Component))) && (!flag2 || !(((MyComponentDefinition)allDefinition).Volume > 0.05f)))
				{
					MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(allDefinition.Id.TypeId);
					myObjectBuilder_PhysicalObject.SubtypeName = allDefinition.Id.SubtypeName;
					if (!AddItems(inventory, myObjectBuilder_PhysicalObject, overrideCheck, 1) && flag)
					{
						Matrix matrix = MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
						MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(1, myObjectBuilder_PhysicalObject), matrix.Translation + matrix.Forward * 0.2f, matrix.Forward, matrix.Up, MySession.Static.ControlledEntity.Entity.Physics);
					}
				}
			}
			if (flag2)
			{
				return;
			}
			MyDefinitionManager.Static.GetOreTypeNames(out string[] outNames);
			string[] array = outNames;
			for (int i = 0; i < array.Length; i++)
			{
				MyObjectBuilder_Ore myObjectBuilder_Ore = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(array[i]);
				if (!AddItems(inventory, myObjectBuilder_Ore, overrideCheck, 1) && flag)
				{
					Matrix matrix2 = MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
					MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(1, myObjectBuilder_Ore), matrix2.Translation + matrix2.Forward * 0.2f, matrix2.Forward, matrix2.Up, MySession.Static.ControlledEntity.Entity.Physics);
				}
			}
		}

		private MyObjectBuilder_PhysicalGunObject CreateGunContent(string subtypeName)
		{
			return (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), subtypeName));
		}
	}
}
