using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTomasInputComponent : MyDebugComponent
	{
		private class DebugNewGridScreen : MyGuiScreenBase
		{
			private MyGuiControlCombobox m_sizeCombobox;

			private MyGuiControlCheckbox m_staticCheckbox;

			public override string GetFriendlyName()
			{
				return "DebugNewGridScreen";
			}

			public DebugNewGridScreen()
			{
				base.EnabledBackgroundFade = true;
				RecreateControls(constructor: true);
			}

			public override void RecreateControls(bool constructor)
			{
				base.RecreateControls(constructor);
				m_sizeCombobox = new MyGuiControlCombobox
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Position = Vector2.Zero
				};
				foreach (object enumValue in typeof(MyCubeSize).GetEnumValues())
				{
					m_sizeCombobox.AddItem((int)(MyCubeSize)enumValue, new StringBuilder(enumValue.ToString()));
				}
				m_sizeCombobox.SelectItemByKey(0L);
				m_staticCheckbox = new MyGuiControlCheckbox
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
					IsChecked = true
				};
				MyGuiControlLabel control = new MyGuiControlLabel
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
					Position = new Vector2(m_staticCheckbox.Size.X, 0f),
					Text = "Static grid"
				};
				MyGuiControlButton myGuiControlButton = new MyGuiControlButton
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
					Text = "Ok",
					Position = new Vector2(0f, 0.05f)
				};
				myGuiControlButton.ButtonClicked += okButton_ButtonClicked;
				Elements.Add(m_sizeCombobox);
				Elements.Add(m_staticCheckbox);
				Elements.Add(control);
				Elements.Add(myGuiControlButton);
			}

			private void okButton_ButtonClicked(MyGuiControlButton obj)
			{
				MyCubeBuilder.Static.StartStaticGridPlacement((MyCubeSize)m_sizeCombobox.GetSelectedKey(), m_staticCheckbox.IsChecked);
				CloseScreen();
			}
		}

		public static float USE_WHEEL_ANIMATION_SPEED = 1f;

		private long m_previousSpectatorGridId;

		public static string ClipboardText = string.Empty;

		public override string GetName()
		{
			return "Tomas";
		}

		public MyTomasInputComponent()
		{
			AddShortcut(MyKeys.Delete, newPress: true, control: true, shift: false, alt: false, () => "Delete all characters", delegate
			{
				foreach (MyCharacter item in MyEntities.GetEntities().OfType<MyCharacter>())
				{
					if (item == MySession.Static.ControlledEntity)
					{
						MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
					}
					item.Close();
				}
				foreach (MyCubeGrid item2 in MyEntities.GetEntities().OfType<MyCubeGrid>())
				{
					foreach (MySlimBlock block in item2.GetBlocks())
					{
						if (block.FatBlock is MyCockpit)
						{
							MyCockpit myCockpit = block.FatBlock as MyCockpit;
							if (myCockpit.Pilot != null)
							{
								myCockpit.Pilot.Close();
							}
						}
					}
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Spawn cargo ship or barbarians", delegate
			{
				MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
				if (eventById == null)
				{
					eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnBarbarians"));
				}
				if (eventById != null)
				{
					MyGlobalEvents.RemoveGlobalEvent(eventById);
					eventById.SetActivationTime(TimeSpan.FromSeconds(1.0));
					MyGlobalEvents.AddGlobalEvent(eventById);
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Spawn random meteor", delegate
			{
				MyMeteor.SpawnRandom(MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 20f + MySector.DirectionToSunNormalized * 1000f, -MySector.DirectionToSunNormalized);
				return true;
			});
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Switch control to next entity", delegate
			{
				if (MySession.Static.ControlledEntity != null)
				{
					MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
					if (cameraControllerEnum != MyCameraControllerEnum.Entity && cameraControllerEnum != MyCameraControllerEnum.ThirdPersonSpectator)
					{
						MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.ControlledEntity.Entity);
					}
					else
					{
						List<MyEntity> list = MyEntities.GetEntities().ToList();
						int num = list.IndexOf(MySession.Static.ControlledEntity.Entity);
						List<MyEntity> list2 = new List<MyEntity>();
						if (num + 1 < list.Count)
						{
							list2.AddRange(list.GetRange(num + 1, list.Count - num - 1));
						}
						if (num != -1)
						{
							list2.AddRange(list.GetRange(0, num + 1));
						}
						MyCharacter myCharacter = null;
						for (int i = 0; i < list2.Count; i++)
						{
							MyCharacter myCharacter2 = list2[i] as MyCharacter;
							if (myCharacter2 != null)
							{
								myCharacter = myCharacter2;
								break;
							}
						}
						if (myCharacter != null)
						{
							MySession.Static.LocalHumanPlayer.Controller.TakeControl(myCharacter);
						}
					}
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Use next ship", delegate
			{
				MyCharacterInputComponent.UseNextShip();
				return true;
			});
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Debug new grid screen", delegate
			{
				MyGuiSandbox.AddScreen(new DebugNewGridScreen());
				return true;
			});
			AddShortcut(MyKeys.N, newPress: true, control: false, shift: false, alt: false, () => "Refill all batteries", delegate
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					MyCubeGrid myCubeGrid3 = entity as MyCubeGrid;
					if (myCubeGrid3 != null)
					{
						foreach (MySlimBlock block2 in myCubeGrid3.GetBlocks())
						{
							MyBatteryBlock myBatteryBlock = block2.FatBlock as MyBatteryBlock;
							if (myBatteryBlock != null)
							{
								myBatteryBlock.CurrentStoredPower = myBatteryBlock.MaxStoredPower;
							}
						}
					}
				}
				return true;
			});
			AddShortcut(MyKeys.U, newPress: true, control: false, shift: false, alt: false, () => "Spawn new character", delegate
			{
				MyCharacterInputComponent.SpawnCharacter();
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Merge static grids", delegate
			{
				HashSet<MyCubeGrid> hashSet = new HashSet<MyCubeGrid>();
				bool flag;
				do
				{
					flag = false;
					foreach (MyEntity entity2 in MyEntities.GetEntities())
					{
						MyCubeGrid myCubeGrid = entity2 as MyCubeGrid;
						if (myCubeGrid != null && myCubeGrid.IsStatic && myCubeGrid.GridSizeEnum == MyCubeSize.Large)
						{
							if (hashSet.Contains(myCubeGrid))
							{
								continue;
							}
							foreach (MySlimBlock item3 in myCubeGrid.GetBlocks().ToList())
							{
								MyCubeGrid myCubeGrid2 = myCubeGrid.DetectMerge(item3);
								if (myCubeGrid2 != null)
								{
									flag = true;
									if (myCubeGrid2 != myCubeGrid)
									{
										break;
									}
								}
							}
							if (!flag)
							{
								hashSet.Add(myCubeGrid);
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				while (flag);
				return true;
			});
			AddShortcut(MyKeys.Add, newPress: true, control: false, shift: false, alt: false, () => "Increase wheel animation speed", delegate
			{
				USE_WHEEL_ANIMATION_SPEED += 0.05f;
				return true;
			});
			AddShortcut(MyKeys.Subtract, newPress: true, control: false, shift: false, alt: false, () => "Decrease wheel animation speed", delegate
			{
				USE_WHEEL_ANIMATION_SPEED -= 0.05f;
				return true;
			});
			AddShortcut(MyKeys.Divide, newPress: true, control: false, shift: false, alt: false, () => "Show model texture names", delegate
			{
				MyFakes.ENABLE_DEBUG_DRAW_TEXTURE_NAMES = !MyFakes.ENABLE_DEBUG_DRAW_TEXTURE_NAMES;
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Throw from spectator: " + MySessionComponentThrower.USE_SPECTATOR_FOR_THROW, delegate
			{
				MySessionComponentThrower.USE_SPECTATOR_FOR_THROW = !MySessionComponentThrower.USE_SPECTATOR_FOR_THROW;
				return true;
			});
			AddShortcut(MyKeys.F2, newPress: true, control: false, shift: false, alt: false, () => "Spectator to next small grid", () => SpectatorToNextGrid(MyCubeSize.Small));
			AddShortcut(MyKeys.F3, newPress: true, control: false, shift: false, alt: false, () => "Spectator to next large grid", () => SpectatorToNextGrid(MyCubeSize.Large));
			AddShortcut(MyKeys.Multiply, newPress: true, control: false, shift: false, alt: false, () => "Show model texture names", CopyAssetToClipboard);
		}

		private bool CopyAssetToClipboard()
		{
			if (!string.IsNullOrEmpty(ClipboardText))
			{
				MyVRage.Platform.Clipboard = ClipboardText;
			}
			return true;
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

		public bool SpectatorToNextGrid(MyCubeSize size)
		{
			MyCubeGrid myCubeGrid = null;
			MyCubeGrid myCubeGrid2 = null;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyCubeGrid myCubeGrid3 = entity as MyCubeGrid;
				if (myCubeGrid3 != null && myCubeGrid3.GridSizeEnum == size)
				{
					if (m_previousSpectatorGridId == 0L)
					{
						myCubeGrid = myCubeGrid3;
						break;
					}
					if (myCubeGrid2 != null)
					{
						myCubeGrid = myCubeGrid3;
						break;
					}
					if (myCubeGrid3.EntityId == m_previousSpectatorGridId)
					{
						myCubeGrid2 = myCubeGrid3;
					}
					if (myCubeGrid == null)
					{
						myCubeGrid = myCubeGrid3;
					}
				}
			}
			if (myCubeGrid == null)
			{
				return false;
			}
			BoundingSphere boundingSphere = myCubeGrid.PositionComp.WorldVolume;
			Vector3D value = Vector3D.Transform(Vector3D.Forward, MySpectator.Static.Orientation);
			MySpectator.Static.Position = myCubeGrid.PositionComp.GetPosition() - value * boundingSphere.Radius * 2.0;
			m_previousSpectatorGridId = myCubeGrid.EntityId;
			return true;
		}
	}
}
