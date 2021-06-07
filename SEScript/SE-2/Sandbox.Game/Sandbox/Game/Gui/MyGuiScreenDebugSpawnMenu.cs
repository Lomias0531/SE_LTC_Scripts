using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal class MyGuiScreenDebugSpawnMenu : MyGuiScreenDebugBase
	{
		[Serializable]
		public struct SpawnAsteroidInfo
		{
			protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu_003C_003ESpawnAsteroidInfo_003C_003EAsteroid_003C_003EAccessor : IMemberAccessor<SpawnAsteroidInfo, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnAsteroidInfo owner, in string value)
				{
					owner.Asteroid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnAsteroidInfo owner, out string value)
				{
					value = owner.Asteroid;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu_003C_003ESpawnAsteroidInfo_003C_003ERandomSeed_003C_003EAccessor : IMemberAccessor<SpawnAsteroidInfo, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnAsteroidInfo owner, in int value)
				{
					owner.RandomSeed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnAsteroidInfo owner, out int value)
				{
					value = owner.RandomSeed;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu_003C_003ESpawnAsteroidInfo_003C_003EPosition_003C_003EAccessor : IMemberAccessor<SpawnAsteroidInfo, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnAsteroidInfo owner, in Vector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnAsteroidInfo owner, out Vector3D value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu_003C_003ESpawnAsteroidInfo_003C_003EIsProcedural_003C_003EAccessor : IMemberAccessor<SpawnAsteroidInfo, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnAsteroidInfo owner, in bool value)
				{
					owner.IsProcedural = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnAsteroidInfo owner, out bool value)
				{
					value = owner.IsProcedural;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu_003C_003ESpawnAsteroidInfo_003C_003EProceduralRadius_003C_003EAccessor : IMemberAccessor<SpawnAsteroidInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnAsteroidInfo owner, in float value)
				{
					owner.ProceduralRadius = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnAsteroidInfo owner, out float value)
				{
					value = owner.ProceduralRadius;
				}
			}

			[Serialize(MyObjectFlags.DefaultZero)]
			public string Asteroid;

			public int RandomSeed;

			public Vector3D Position;

			public bool IsProcedural;

			public float ProceduralRadius;
		}

		private delegate void CreateScreen(float space, float width);

		private struct Screen
		{
			public string Name;

			public CreateScreen Creator;
		}

		protected sealed class SpawnIntoContainer_Implementation_003C_003ESystem_Int64_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, SerializableDefinitionId, long, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long amount, in SerializableDefinitionId item, in long entityId, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				SpawnIntoContainer_Implementation(amount, item, entityId, playerId);
			}
		}

		protected sealed class SpawnAsteroid_003C_003ESandbox_Game_Gui_MyGuiScreenDebugSpawnMenu_003C_003ESpawnAsteroidInfo : ICallSite<IMyEventOwner, SpawnAsteroidInfo, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in SpawnAsteroidInfo asteroidInfo, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SpawnAsteroid(asteroidInfo);
			}
		}

		protected sealed class SpawnPlanet_Server_003C_003ESystem_String_0023System_Single_0023System_Int32_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, float, int, Vector3D, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string planetName, in float size, in int seed, in Vector3D pos, in DBNull arg5, in DBNull arg6)
			{
				SpawnPlanet_Server(planetName, size, seed, pos);
			}
		}

		protected sealed class SpawnPlanet_Client_003C_003ESystem_String_0023System_String_0023System_Single_0023System_Int32_0023VRageMath_Vector3D_0023System_Int64 : ICallSite<IMyEventOwner, string, string, float, int, Vector3D, long>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string planetName, in string storageNameBase, in float size, in int seed, in Vector3D pos, in long entityId)
			{
				SpawnPlanet_Client(planetName, storageNameBase, size, seed, pos, entityId);
			}
		}

		private static readonly Vector2 SCREEN_SIZE;

		private static readonly float HIDDEN_PART_RIGHT;

		private readonly Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		private MyGuiControlListbox m_asteroidListBox;

		private MyGuiControlListbox m_physicalObjectListbox;

		private static MyPhysicalItemDefinition m_lastSelectedPhysicalItemDefinition;

		private MyGuiControlListbox m_asteroidTypeListbox;

		private MyGuiControlListbox m_planetListbox;

		private string m_selectedCoreVoxelFile;

		private static string m_lastSelectedAsteroidName;

		private MyGuiControlTextbox m_amountTextbox;

		private MyGuiControlLabel m_errorLabel;

		private static long m_amount;

		private static int m_asteroidCounter;

		private static float m_procAsteroidSizeValue;

		private static string m_procAsteroidSeedValue;

		private MyGuiControlSlider m_procAsteroidSize;

		private MyGuiControlTextbox m_procAsteroidSeed;

		private static string m_selectedPlanetName;

		private MyVoxelBase m_currentVoxel;

		private static int m_selectedScreen;

		private Screen[] Screens;

		public static SpawnAsteroidInfo m_lastAsteroidInfo;

		private MyGuiControlSlider m_planetSizeSlider;

		private MyGuiControlTextbox m_procPlanetSeedValue;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugSpawnMenu";
		}

		static MyGuiScreenDebugSpawnMenu()
		{
			SCREEN_SIZE = new Vector2(0.4f, 1.2f);
			HIDDEN_PART_RIGHT = 0.04f;
			m_lastSelectedAsteroidName = null;
			m_amount = 1L;
			m_asteroidCounter = 0;
			m_procAsteroidSizeValue = 64f;
			m_procAsteroidSeedValue = "12345";
		}

		public MyGuiScreenDebugSpawnMenu()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			Screen[] array = new Screen[5];
			Screen screen = new Screen
			{
				Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Items),
				Creator = CreateObjectsSpawnMenu
			};
			array[0] = screen;
			screen = new Screen
			{
				Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Asteroids),
				Creator = CreateAsteroidsSpawnMenu
			};
			array[1] = screen;
			screen = new Screen
			{
				Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralAsteroids),
				Creator = CreateProceduralAsteroidsSpawnMenu
			};
			array[2] = screen;
			screen = new Screen
			{
				Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Planets),
				Creator = CreatePlanetsSpawnMenu
			};
			array[3] = screen;
			screen = new Screen
			{
				Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_EmptyVoxelMap),
				Creator = CreateEmptyVoxelMapSpawnMenu
			};
			array[4] = screen;
			Screens = array;
			RecreateControls(constructor: true);
		}

		private void CreateScreenSelector()
		{
			m_currentPosition.X += 0.018f;
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + m_controlPadding.Y - 0.071f;
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			bool num = localHumanPlayer != null && MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId);
			MyGuiControlCombobox combo = AddCombo();
			combo.AddItem(0L, MySpaceTexts.ScreenDebugSpawnMenu_Items);
			if (num)
			{
				combo.AddItem(1L, MySpaceTexts.ScreenDebugSpawnMenu_PredefinedAsteroids);
				combo.AddItem(2L, MySpaceTexts.ScreenDebugSpawnMenu_ProceduralAsteroids);
				if (MyFakes.ENABLE_PLANETS || MyInput.Static.ENABLE_DEVELOPER_KEYS)
				{
					combo.AddItem(3L, MySpaceTexts.ScreenDebugSpawnMenu_Planets);
				}
				combo.AddItem(4L, MySpaceTexts.ScreenDebugSpawnMenu_EmptyVoxelMap);
			}
			combo.SelectItemByKey(m_selectedScreen);
			combo.ItemSelected += delegate
			{
				m_selectedScreen = (int)combo.GetSelectedKey();
				RecreateControls(constructor: false);
			};
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			new Vector2(-0.05f, 0f);
			Vector2 vector = new Vector2(0.02f, 0.02f);
			float scale = 0.8f;
			float num = 0.01f;
			float usableWidth = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - vector.X * 2f;
			float num2 = (SCREEN_SIZE.Y - 1f) / 2f;
			m_currentPosition = -m_size.Value / 2f;
			m_currentPosition += vector;
			m_currentPosition.Y += num2;
			m_scale = scale;
			AddCaption(MyTexts.Get(MySpaceTexts.ScreenDebugSpawnMenu_Caption).ToString(), Color.White.ToVector4(), vector + new Vector2(0f - HIDDEN_PART_RIGHT, num2 - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.365f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + num;
			CreateMenu(num, usableWidth);
		}

		private void CreateMenu(float separatorSize, float usableWidth)
		{
			CreateScreenSelector();
			Screens[m_selectedScreen].Creator(separatorSize, usableWidth);
		}

		private void CreateAsteroidsSpawnMenu(float separatorSize, float usableWidth)
		{
			m_currentPosition.Y += 0.025f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Asteroid)
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			m_asteroidTypeListbox = AddListBox(0.2f);
			m_asteroidTypeListbox.MultiSelect = false;
			m_asteroidTypeListbox.VisibleRowsCount = 5;
			foreach (MyVoxelMapStorageDefinition item in MyDefinitionManager.Static.GetVoxelMapStorageDefinitions().OrderBy(delegate(MyVoxelMapStorageDefinition e)
			{
				MyStringHash subtypeId2 = e.Id.SubtypeId;
				return subtypeId2.ToString();
			}))
			{
				MyStringHash subtypeId = item.Id.SubtypeId;
				string text = subtypeId.ToString();
				m_asteroidTypeListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(text), text, null, text));
			}
			m_asteroidTypeListbox.ItemsSelected += OnAsteroidTypeListbox_ItemSelected;
			m_asteroidTypeListbox.ItemDoubleClicked += OnLoadAsteroid;
			if (m_asteroidTypeListbox.Items.Count > 0)
			{
				m_asteroidTypeListbox.SelectedItems.Add(m_asteroidTypeListbox.Items[0]);
			}
			OnAsteroidTypeListbox_ItemSelected(m_asteroidTypeListbox);
			m_currentPosition.Y -= 0.01f;
			MyGuiControlButton myGuiControlButton = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, OnLoadAsteroid);
			myGuiControlButton.PositionX += 0.002f;
			m_currentPosition.Y += 0.01f;
			MyGuiControlLabel control2 = new MyGuiControlLabel(m_currentPosition + new Vector2(0.002f, 0f), null, MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_AsteroidGenerationCanTakeLong), Color.Red.ToVector4(), 0.8f * m_scale, "Debug");
			Controls.Add(control2);
			m_currentPosition.Y += 0.03f;
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			bool flag = localHumanPlayer != null && MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId);
			if (!flag)
			{
				MyGuiControlLabel control3 = new MyGuiControlLabel(m_currentPosition + new Vector2(0.002f, 0f), null, MyTexts.GetString(MyCommonTexts.Warning_SpacemasterOrHigherRequired), Color.Red.ToVector4(), 0.8f * m_scale, "Debug");
				Controls.Add(control3);
			}
			m_currentPosition.Y += 0.03f;
			m_asteroidTypeListbox.Enabled = flag;
			myGuiControlButton.Enabled = flag;
		}

		private void CreateProceduralAsteroidsSpawnMenu(float separatorSize, float usableWidth)
		{
			m_currentPosition.Y += 0.025f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSize)
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			m_procAsteroidSize = new MyGuiControlSlider(m_currentPosition, 5f, 2000f, 0.285f, null, null, string.Empty, 2, 0.75f * m_scale, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_procAsteroidSize.DebugScale = m_sliderDebugScale;
			m_procAsteroidSize.ColorMask = Color.White.ToVector4();
			Controls.Add(m_procAsteroidSize);
			MyGuiControlLabel label = new MyGuiControlLabel(m_currentPosition + new Vector2(m_procAsteroidSize.Size.X - 0.003f, m_procAsteroidSize.Size.Y - 0.065f), null, string.Empty, Color.White.ToVector4(), 0.8f, "Debug");
			label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			Controls.Add(label);
			MyGuiControlSlider procAsteroidSize = m_procAsteroidSize;
			procAsteroidSize.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(procAsteroidSize.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				label.Text = MyValueFormatter.GetFormatedFloat(s.Value, 2) + "m";
				m_procAsteroidSizeValue = s.Value;
			});
			m_procAsteroidSize.Value = m_procAsteroidSizeValue;
			m_currentPosition.Y += m_procAsteroidSize.Size.Y;
			m_currentPosition.Y += separatorSize;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSeed)
			};
			Controls.Add(control2);
			m_currentPosition.Y += 0.03f;
			m_procAsteroidSeed = new MyGuiControlTextbox(m_currentPosition, m_procAsteroidSeedValue, 20, Color.White.ToVector4(), m_scale);
			m_procAsteroidSeed.TextChanged += delegate(MyGuiControlTextbox t)
			{
				m_procAsteroidSeedValue = t.Text;
			};
			m_procAsteroidSeed.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_procAsteroidSeed.Size = new Vector2(0.285f, 1f);
			Controls.Add(m_procAsteroidSeed);
			m_currentPosition.Y += m_procAsteroidSize.Size.Y + separatorSize;
			m_currentPosition.Y -= 0.015f;
			MyGuiControlButton myGuiControlButton = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_GenerateSeed, generateSeedButton_OnButtonClick);
			myGuiControlButton.PositionX += 0.002f;
			m_currentPosition.Y -= 0.01f;
			MyGuiControlButton myGuiControlButton2 = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, OnSpawnProceduralAsteroid);
			myGuiControlButton2.PositionX += 0.002f;
			m_currentPosition.Y += 0.01f;
			MyGuiControlLabel control3 = new MyGuiControlLabel(m_currentPosition + new Vector2(0.002f, 0f), null, MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_AsteroidGenerationCanTakeLong), Color.Red.ToVector4(), 0.8f * m_scale, "Debug");
			Controls.Add(control3);
			m_currentPosition.Y += 0.03f;
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			bool flag = localHumanPlayer != null && MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId);
			if (!flag)
			{
				MyGuiControlLabel control4 = new MyGuiControlLabel(m_currentPosition + new Vector2(0.002f, 0f), null, MyTexts.GetString(MyCommonTexts.Warning_SpacemasterOrHigherRequired), Color.Red.ToVector4(), 0.8f * m_scale, "Debug");
				Controls.Add(control4);
			}
			m_procAsteroidSize.Enabled = flag;
			m_procAsteroidSeed.Enabled = flag;
			myGuiControlButton.Enabled = flag;
			myGuiControlButton2.Enabled = flag;
		}

		private void CreateObjectsSpawnMenu(float separatorSize, float usableWidth)
		{
			m_currentPosition.Y += 0.025f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "Select Item :"
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			m_physicalObjectListbox = AddListBox(0.585f);
			m_physicalObjectListbox.MultiSelect = false;
			m_physicalObjectListbox.VisibleRowsCount = 17;
			foreach (MyPhysicalItemDefinition item2 in from MyPhysicalItemDefinition e in from e in MyDefinitionManager.Static.GetAllDefinitions()
					where e is MyPhysicalItemDefinition && e.Public
					select e
				orderby e.DisplayNameText
				select e)
			{
				if (item2.CanSpawnFromScreen)
				{
					string icon = (item2.Icons != null && item2.Icons.Length != 0) ? item2.Icons[0] : MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.DisplayNameText), item2.DisplayNameText, icon, item2);
					m_physicalObjectListbox.Items.Add(item);
					if (item2 == m_lastSelectedPhysicalItemDefinition)
					{
						m_physicalObjectListbox.SelectedItems.Add(item);
					}
				}
			}
			m_physicalObjectListbox.ItemsSelected += OnPhysicalObjectListbox_ItemSelected;
			m_physicalObjectListbox.ItemDoubleClicked += OnSpawnPhysicalObject;
			OnPhysicalObjectListbox_ItemSelected(m_physicalObjectListbox);
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ItemAmount)
			};
			Controls.Add(control2);
			m_currentPosition.Y += 0.03f;
			m_amountTextbox = new MyGuiControlTextbox(m_currentPosition, m_amount.ToString(), 6, null, m_scale, MyGuiControlTextboxType.DigitsOnly);
			m_amountTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_amountTextbox.TextChanged += OnAmountTextChanged;
			Controls.Add(m_amountTextbox);
			m_amountTextbox.Size = new Vector2(0.285f, 1f);
			m_currentPosition.Y -= 0.02f;
			m_currentPosition.Y += separatorSize + m_amountTextbox.Size.Y;
			m_errorLabel = AddLabel(MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_InvalidAmount), Color.Red.ToVector4(), 0.8f);
			m_errorLabel.PositionX += 0.282f;
			m_errorLabel.PositionY -= 0.04f;
			m_errorLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_errorLabel.Visible = false;
			m_currentPosition.Y += 0.01f;
			CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnObject, OnSpawnPhysicalObject).PositionX += 0.002f;
			MyTerminalBlock myTerminalBlock = null;
			bool enabled = false;
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Components.TryGet(out MyCharacterDetectorComponent component) && component.UseObject != null)
			{
				myTerminalBlock = (component.DetectedEntity as MyTerminalBlock);
			}
			string str = "-";
			if (myTerminalBlock != null && myTerminalBlock.HasInventory && myTerminalBlock.HasLocalPlayerAccess())
			{
				str = myTerminalBlock.CustomName.ToString();
				enabled = true;
			}
			m_currentPosition.Y -= 0.015f;
			CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnTargeted, OnSpawnIntoContainer, enabled).PositionX += 0.002f;
			AddLabel(MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_CurrentTarget) + str, Color.White.ToVector4(), m_scale);
		}

		private static float NormalizeLog(float f, float min, float max)
		{
			return MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0f, 1f);
		}

		private static float DenormalizeLog(float f, float min, float max)
		{
			return MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max);
		}

		private void UpdateLayerSlider(MyGuiControlSlider slider, float minValue, float maxValue)
		{
			slider.Value = MathHelper.Max(minValue, MathHelper.Min(slider.Value, maxValue));
			slider.MaxValue = maxValue;
			slider.MinValue = minValue;
		}

		private void OnAmountTextChanged(MyGuiControlTextbox textbox)
		{
			m_errorLabel.Visible = false;
		}

		private bool IsValidAmount()
		{
			if (long.TryParse(m_amountTextbox.Text, out m_amount))
			{
				if (m_amount < 1)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		private void OnAsteroidTypeListbox_ItemSelected(MyGuiControlListbox listbox)
		{
			if (listbox.SelectedItems.Count != 0)
			{
				m_lastSelectedAsteroidName = (listbox.SelectedItems[0].UserData as string);
				m_selectedCoreVoxelFile = m_lastSelectedAsteroidName;
			}
		}

		private void OnPlanetListbox_ItemSelected(MyGuiControlListbox listbox)
		{
			if (listbox.SelectedItems.Count != 0)
			{
				m_selectedPlanetName = (listbox.SelectedItems[0].UserData as string);
			}
		}

		private void OnPhysicalObjectListbox_ItemSelected(MyGuiControlListbox listbox)
		{
			if (listbox.SelectedItems.Count != 0)
			{
				m_lastSelectedPhysicalItemDefinition = (listbox.SelectedItems[0].UserData as MyPhysicalItemDefinition);
			}
		}

		private void ScreenAsteroids(object _)
		{
			MyGuiControlListbox.Item[] selectedItems = m_asteroidListBox.SelectedItems.ToArray();
			if (selectedItems.Length == 0)
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Error"), messageText: new StringBuilder("No asteroids selected"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
				return;
			}
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string path = MyPerGameSettings.GameNameSafe + "_AsteroidScreens";
			string folder = Path.Combine(folderPath, path);
			int state = 0;
			int pauseTimeout = 0;
			int asteroidIndex = 0;
			Action stateMachine = null;
			stateMachine = delegate
			{
				if (pauseTimeout > 0)
				{
					pauseTimeout--;
				}
				else
				{
					MyVoxelMapStorageDefinition myVoxelMapStorageDefinition = (MyVoxelMapStorageDefinition)selectedItems[asteroidIndex].UserData;
					switch (state)
					{
					case 0:
						SpawnVoxelPreview(myVoxelMapStorageDefinition.Id.SubtypeName);
						pauseTimeout = 100;
						break;
					case 1:
					{
						if (!MyClipboardComponent.Static.IsActive)
						{
							MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Done"), messageText: new StringBuilder("Screening interrupted"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
							return;
						}
						string pathToSave = Path.Combine(folder, myVoxelMapStorageDefinition.Id.SubtypeName + ".png");
						MyRenderProxy.TakeScreenshot(Vector2.One, pathToSave, debug: false, ignoreSprites: true, showNotification: false);
						pauseTimeout = 10;
						asteroidIndex++;
						if (asteroidIndex == selectedItems.Length)
						{
							MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Done"), messageText: new StringBuilder("All screens saved to\n" + folder), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
							return;
						}
						break;
					}
					}
					state++;
					if (state > 1)
					{
						state = 0;
					}
				}
				MySandboxGame.Static.Invoke(stateMachine, "Asteroid screening");
			};
			MySandboxGame.Static.Invoke(stateMachine, "Asteroid screening");
			CloseScreenNow();
		}

		private void SpawnFloatingObjectPreview()
		{
			if (m_lastSelectedPhysicalItemDefinition != null)
			{
				MyDefinitionId id = m_lastSelectedPhysicalItemDefinition.Id;
				MyFixedPoint amount = (MyFixedPoint)(decimal)m_amount;
				MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(id);
				if (myObjectBuilder_PhysicalObject is MyObjectBuilder_PhysicalGunObject || myObjectBuilder_PhysicalObject is MyObjectBuilder_OxygenContainerObject || myObjectBuilder_PhysicalObject is MyObjectBuilder_GasContainerObject)
				{
					amount = 1;
				}
				MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_FloatingObject>();
				myObjectBuilder_FloatingObject.PositionAndOrientation = MyPositionAndOrientation.Default;
				myObjectBuilder_FloatingObject.Item = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
				myObjectBuilder_FloatingObject.Item.Amount = amount;
				myObjectBuilder_FloatingObject.Item.PhysicalContent = myObjectBuilder_PhysicalObject;
				MyClipboardComponent.Static.ActivateFloatingObjectClipboard(myObjectBuilder_FloatingObject, Vector3D.Zero, 1f);
			}
		}

		private MyGuiControlButton CreateDebugButton(float usableWidth, MyStringId text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null)
		{
			m_currentPosition.Y += 0.01f;
			MyGuiControlButton myGuiControlButton = AddButton(MyTexts.Get(text), onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = m_scale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2((0f - HIDDEN_PART_RIGHT) / 2f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsNewKeyPressed(MyKeys.F10))
			{
				CloseScreen();
			}
		}

		private static Matrix GetPasteMatrix()
		{
			if (MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator))
			{
				return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
			}
			return MySector.MainCamera.WorldMatrix;
		}

		private void OnSpawnPhysicalObject(object obj)
		{
			if (!IsValidAmount())
			{
				m_errorLabel.Visible = true;
				return;
			}
			SpawnFloatingObjectPreview();
			CloseScreenNow();
		}

		private void OnSpawnIntoContainer(MyGuiControlButton myGuiControlButton)
		{
			MyCharacterDetectorComponent component;
			if (!IsValidAmount() || m_lastSelectedPhysicalItemDefinition == null)
			{
				m_errorLabel.Visible = true;
			}
			else if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Components.TryGet(out component))
			{
				MyTerminalBlock myTerminalBlock = component.DetectedEntity as MyTerminalBlock;
				if (myTerminalBlock != null && myTerminalBlock.HasInventory)
				{
					SerializableDefinitionId arg = m_lastSelectedPhysicalItemDefinition.Id;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SpawnIntoContainer_Implementation, m_amount, arg, myTerminalBlock.EntityId, MySession.Static.LocalPlayerId);
				}
			}
		}

		[Event(null, 789)]
		[Reliable]
		[Server]
		private static void SpawnIntoContainer_Implementation(long amount, SerializableDefinitionId item, long entityId, long playerId)
		{
			MyEntity entity;
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) && !MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MyEntities.TryGetEntityById(entityId, out entity) && entity.HasInventory && ((MyTerminalBlock)entity).HasPlayerAccess(playerId))
			{
				MyInventory inventory = entity.GetInventory();
				if (inventory.CheckConstraint(item))
				{
					MyFixedPoint amount2 = (MyFixedPoint)Math.Min(amount, (decimal)inventory.ComputeAmountThatFits(item));
					inventory.AddItems(amount2, (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(item));
				}
			}
		}

		private void OnLoadAsteroid(object obj)
		{
			SpawnVoxelPreview();
			CloseScreenNow();
		}

		private void OnSpawnProceduralAsteroid(MyGuiControlButton obj)
		{
			int proceduralAsteroidSeed = GetProceduralAsteroidSeed(m_procAsteroidSeed);
			SpawnProceduralAsteroid(proceduralAsteroidSeed, m_procAsteroidSize.Value);
			CloseScreenNow();
		}

		private void generateSeedButton_OnButtonClick(MyGuiControlButton sender)
		{
			m_procAsteroidSeed.Text = MyRandom.Instance.Next().ToString();
		}

		private int GetProceduralAsteroidSeed(MyGuiControlTextbox textbox)
		{
			int result = 12345;
			if (!int.TryParse(textbox.Text, out result))
			{
				string text = textbox.Text;
				byte[] array = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
				int num = 0;
				for (int i = 0; i < 4 && i < array.Length; i++)
				{
					result |= array[i] << num;
					num += 8;
				}
			}
			return result;
		}

		public static MyStorageBase CreateAsteroidStorage(string asteroid)
		{
			if (MySession.Static?.LowMemoryState ?? false)
			{
				MyHud.Notifications.Add(new MyHudNotification(MyCommonTexts.Performance_LowOnMemory));
				return null;
			}
			return MyStorageBase.CreateAsteroidStorage(asteroid);
		}

		public static MyObjectBuilder_VoxelMap CreateAsteroidObjectBuilder(string storageName)
		{
			MyObjectBuilder_VoxelMap myObjectBuilder_VoxelMap = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_VoxelMap>();
			myObjectBuilder_VoxelMap.StorageName = storageName;
			myObjectBuilder_VoxelMap.PersistentFlags = (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			myObjectBuilder_VoxelMap.PositionAndOrientation = MyPositionAndOrientation.Default;
			myObjectBuilder_VoxelMap.MutableStorage = false;
			return myObjectBuilder_VoxelMap;
		}

		private void SpawnVoxelPreview()
		{
			SpawnVoxelPreview(m_selectedCoreVoxelFile);
		}

		private void SpawnVoxelPreview(string storageNameBase)
		{
			string storageName = MakeStorageName(storageNameBase);
			MyStorageBase myStorageBase = CreateAsteroidStorage(storageNameBase);
			if (myStorageBase != null)
			{
				MyObjectBuilder_VoxelMap voxelMap = CreateAsteroidObjectBuilder(storageName);
				SpawnAsteroidInfo lastAsteroidInfo = default(SpawnAsteroidInfo);
				lastAsteroidInfo.Asteroid = storageNameBase;
				lastAsteroidInfo.Position = Vector3D.Zero;
				lastAsteroidInfo.IsProcedural = false;
				m_lastAsteroidInfo = lastAsteroidInfo;
				if (myStorageBase != null)
				{
					MyClipboardComponent.Static.ActivateVoxelClipboard(voxelMap, myStorageBase, MySector.MainCamera.ForwardVector, (myStorageBase.Size * 0.5f).Length());
				}
			}
		}

		public static MyStorageBase CreateProceduralAsteroidStorage(int seed, float radius)
		{
			return new MyOctreeStorage(MyCompositeShapeProvider.CreateAsteroidShape(seed, radius), MyVoxelCoordSystems.FindBestOctreeSize(radius));
		}

		private void SpawnProceduralAsteroid(int seed, float radius)
		{
			string storageName = MakeStorageName("ProcAsteroid-" + seed + "r" + radius);
			MyStorageBase myStorageBase = CreateProceduralAsteroidStorage(seed, radius);
			MyObjectBuilder_VoxelMap voxelMap = CreateAsteroidObjectBuilder(storageName);
			SpawnAsteroidInfo lastAsteroidInfo = default(SpawnAsteroidInfo);
			lastAsteroidInfo.Asteroid = null;
			lastAsteroidInfo.RandomSeed = seed;
			lastAsteroidInfo.Position = Vector3D.Zero;
			lastAsteroidInfo.IsProcedural = true;
			lastAsteroidInfo.ProceduralRadius = radius;
			m_lastAsteroidInfo = lastAsteroidInfo;
			MyClipboardComponent.Static.ActivateVoxelClipboard(voxelMap, myStorageBase, MySector.MainCamera.ForwardVector, (myStorageBase.Size * 0.5f).Length());
		}

		public static void RecreateAsteroidBeforePaste(float dragVectorLength)
		{
			int randomSeed = m_lastAsteroidInfo.RandomSeed;
			float proceduralRadius = m_lastAsteroidInfo.ProceduralRadius;
			string storageName = MakeStorageName("ProcAsteroid-" + randomSeed + "r" + proceduralRadius);
			MyStorageBase myStorageBase;
			if (m_lastAsteroidInfo.IsProcedural)
			{
				myStorageBase = CreateProceduralAsteroidStorage(randomSeed, proceduralRadius);
			}
			else
			{
				bool useStorageCache = MyStorageBase.UseStorageCache;
				MyStorageBase.UseStorageCache = false;
				try
				{
					if (m_lastAsteroidInfo.Asteroid != null)
					{
						myStorageBase = CreateAsteroidStorage(m_lastAsteroidInfo.Asteroid);
						if (myStorageBase == null)
						{
							return;
						}
					}
					else
					{
						int xyz = (int)m_procAsteroidSizeValue;
						myStorageBase = new MyOctreeStorage(null, new Vector3I(xyz));
					}
				}
				finally
				{
					MyStorageBase.UseStorageCache = useStorageCache;
				}
			}
			MyObjectBuilder_VoxelMap voxelMap = CreateAsteroidObjectBuilder(storageName);
			MyClipboardComponent.Static.ActivateVoxelClipboard(voxelMap, myStorageBase, MySector.MainCamera.ForwardVector, dragVectorLength);
		}

		public static string MakeStorageName(string storageNameBase)
		{
			string text = storageNameBase;
			int num = 0;
			bool flag;
			do
			{
				flag = false;
				foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
				{
					if (instance.StorageName == text)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					text = storageNameBase + "-" + num++;
				}
			}
			while (flag);
			return text;
		}

		private static void AddSeparator(MyGuiControlList list)
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.Size = new Vector2(1f, 0.01f);
			myGuiControlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlSeparatorList.AddHorizontal(Vector2.Zero, 1f);
			list.Controls.Add(myGuiControlSeparatorList);
		}

		private MyGuiControlLabel CreateSliderWithDescription(float usableWidth, float min, float max, string description, ref MyGuiControlSlider slider)
		{
			AddLabel(description, Vector4.One, m_scale);
			CreateSlider(usableWidth, min, max, ref slider);
			return AddLabel("", Vector4.One, m_scale);
		}

		private void CreateSlider(float usableWidth, float min, float max, ref MyGuiControlSlider slider)
		{
			slider = AddSlider(string.Empty, 5f, min, max);
			slider.Size = new Vector2(400f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, slider.Size.Y);
			slider.LabelDecimalPlaces = 4;
			slider.DebugScale = m_sliderDebugScale;
			slider.ColorMask = Color.White.ToVector4();
		}

		public static string GetAsteroidName()
		{
			return m_lastAsteroidInfo.Asteroid;
		}

		public static void SpawnAsteroid(Vector3D pos)
		{
			m_lastAsteroidInfo.Position = pos;
			if (MySession.Static.HasCreativeRights || MySession.Static.CreativeMode)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SpawnAsteroid, m_lastAsteroidInfo);
			}
		}

		[Event(null, 1044)]
		[Reliable]
		[Server]
		private static void SpawnAsteroid(SpawnAsteroidInfo asteroidInfo)
		{
			if (MyEventContext.Current.IsLocallyInvoked || MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				MyStorageBase myStorageBase;
				string storageName;
				if (!asteroidInfo.IsProcedural)
				{
					if (asteroidInfo.Asteroid == null)
					{
						int xyz = (int)asteroidInfo.ProceduralRadius;
						myStorageBase = new MyOctreeStorage(null, new Vector3I(xyz));
					}
					else
					{
						myStorageBase = CreateAsteroidStorage(asteroidInfo.Asteroid);
						if (myStorageBase == null)
						{
							return;
						}
					}
					storageName = MakeStorageName(asteroidInfo.Asteroid + "-" + asteroidInfo.RandomSeed);
				}
				else
				{
					using (MyRandom.Instance.PushSeed(asteroidInfo.RandomSeed))
					{
						storageName = MakeStorageName("ProcAsteroid-" + asteroidInfo.RandomSeed + "r" + asteroidInfo.ProceduralRadius);
						myStorageBase = CreateProceduralAsteroidStorage(asteroidInfo.RandomSeed, asteroidInfo.ProceduralRadius);
					}
				}
				MyVoxelMap myVoxelMap = new MyVoxelMap();
				myVoxelMap.CreatedByUser = true;
				myVoxelMap.Save = true;
				myVoxelMap.AsteroidName = asteroidInfo.Asteroid;
				myVoxelMap.Init(storageName, myStorageBase, asteroidInfo.Position - myStorageBase.Size * 0.5f);
				MyEntities.Add(myVoxelMap);
				MyEntities.RaiseEntityCreated(myVoxelMap);
			}
			else
			{
				((MyMultiplayerServerBase)MyMultiplayer.Static).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

		private void CreatePlanetsSpawnMenu(float separatorSize, float usableWidth)
		{
			m_currentPosition.Y += 0.025f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Asteroid)
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			m_planetListbox = AddListBox(0.5f);
			m_planetListbox.MultiSelect = false;
			m_planetListbox.VisibleRowsCount = 14;
			foreach (MyPlanetGeneratorDefinition item in MyDefinitionManager.Static.GetPlanetsGeneratorsDefinitions().OrderBy(delegate(MyPlanetGeneratorDefinition e)
			{
				MyStringHash subtypeId2 = e.Id.SubtypeId;
				return subtypeId2.ToString();
			}))
			{
				MyStringHash subtypeId = item.Id.SubtypeId;
				string text = subtypeId.ToString();
				m_planetListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(text), text, null, text));
			}
			m_planetListbox.ItemsSelected += OnPlanetListbox_ItemSelected;
			m_planetListbox.ItemDoubleClicked += OnCreatePlanetClicked;
			if (m_planetListbox.Items.Count > 0)
			{
				m_planetListbox.SelectedItems.Add(m_planetListbox.Items[0]);
			}
			OnPlanetListbox_ItemSelected(m_planetListbox);
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSize)
			};
			Controls.Add(control2);
			m_currentPosition.Y += 0.03f;
			float minValue = MyFakes.ENABLE_EXTENDED_PLANET_OPTIONS ? 100 : 19000;
			float maxValue = 120000f;
			m_planetSizeSlider = new MyGuiControlSlider(m_currentPosition, minValue, maxValue, 0.285f, null, null, string.Empty, 1, 0.8f, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
			m_planetSizeSlider.DebugScale = m_sliderDebugScale;
			m_planetSizeSlider.ColorMask = Color.White.ToVector4();
			Controls.Add(m_planetSizeSlider);
			MyGuiControlLabel label = new MyGuiControlLabel(m_currentPosition + new Vector2(m_planetSizeSlider.Size.X - 0.003f, m_planetSizeSlider.Size.Y - 0.065f), null, string.Empty, Color.White.ToVector4(), 0.8f, "Debug");
			label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			Controls.Add(label);
			m_currentPosition.Y += m_planetSizeSlider.Size.Y;
			m_currentPosition.Y += separatorSize;
			MyGuiControlSlider planetSizeSlider = m_planetSizeSlider;
			planetSizeSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(planetSizeSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				StringBuilder stringBuilder = new StringBuilder();
				MyValueFormatter.AppendDistanceInBestUnit(s.Value, stringBuilder);
				label.Text = stringBuilder.ToString();
				m_procAsteroidSizeValue = s.Value;
			});
			m_planetSizeSlider.Value = 8000f;
			MyGuiControlLabel control3 = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSeed)
			};
			Controls.Add(control3);
			m_currentPosition.Y += 0.03f;
			m_procPlanetSeedValue = new MyGuiControlTextbox(m_currentPosition, m_procAsteroidSeedValue, 20, Color.White.ToVector4(), m_scale);
			m_procPlanetSeedValue.TextChanged += delegate(MyGuiControlTextbox t)
			{
				m_procAsteroidSeedValue = t.Text;
			};
			m_procPlanetSeedValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_procPlanetSeedValue.Size = new Vector2(0.285f, 1f);
			Controls.Add(m_procPlanetSeedValue);
			m_currentPosition.Y += 0.043f;
			MyGuiControlButton myGuiControlButton = CreateDebugButton(0.285f, MySpaceTexts.ScreenDebugSpawnMenu_GenerateSeed, delegate
			{
				m_procPlanetSeedValue.Text = MyRandom.Instance.Next().ToString();
			});
			myGuiControlButton.PositionX += 0.002f;
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			bool flag = localHumanPlayer != null && MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId);
			if (!flag)
			{
				MyGuiControlLabel control4 = new MyGuiControlLabel(m_currentPosition + new Vector2(0.002f, 0.05f), null, MyTexts.GetString(MyCommonTexts.Warning_SpacemasterOrHigherRequired), Color.Red.ToVector4(), 0.8f * m_scale, "Debug");
				Controls.Add(control4);
			}
			m_currentPosition.Y -= 0.01f;
			MyGuiControlButton myGuiControlButton2 = CreateDebugButton(0.285f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, OnCreatePlanetClicked);
			myGuiControlButton2.PositionX += 0.002f;
			m_planetSizeSlider.Enabled = flag;
			m_procPlanetSeedValue.Enabled = flag;
			myGuiControlButton.Enabled = flag;
			m_planetListbox.Enabled = flag;
			myGuiControlButton2.Enabled = flag;
		}

		private void OnCreatePlanetClicked(object _)
		{
			int proceduralAsteroidSeed = GetProceduralAsteroidSeed(m_procPlanetSeedValue);
			CreatePlanet(proceduralAsteroidSeed, m_planetSizeSlider.Value);
			CloseScreenNow();
		}

		private void CreatePlanet(int seed, float size)
		{
			Vector3D positionMinCorner = MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * size * 3f - new Vector3D(size);
			MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(m_selectedPlanetName));
			if (!MyPlanets.Static.CanSpawnPlanet(definition, out string errorMessage))
			{
				MyHud.Notifications.Add(new MyHudNotification(MyStringId.GetOrCompute(errorMessage)));
				return;
			}
			MyPlanetStorageProvider myPlanetStorageProvider = new MyPlanetStorageProvider();
			myPlanetStorageProvider.Init(seed, definition, size / 2f);
			IMyStorage myStorage = new MyOctreeStorage(myPlanetStorageProvider, myPlanetStorageProvider.StorageSize);
			float num = myPlanetStorageProvider.Radius * definition.HillParams.Min;
			float num2 = myPlanetStorageProvider.Radius * definition.HillParams.Max;
			float radius = myPlanetStorageProvider.Radius;
			float maxRadius = radius + num2;
			float minRadius = radius + num;
			float num3 = (definition.AtmosphereSettings.HasValue && definition.AtmosphereSettings.Value.Scale > 1f) ? (1f + definition.AtmosphereSettings.Value.Scale) : 1.75f;
			num3 *= myPlanetStorageProvider.Radius;
			MyPlanet myPlanet = new MyPlanet();
			myPlanet.EntityId = MyRandom.Instance.NextLong();
			MyPlanetInitArguments arguments = default(MyPlanetInitArguments);
			arguments.StorageName = "test";
			arguments.Seed = seed;
			arguments.Storage = myStorage;
			arguments.PositionMinCorner = positionMinCorner;
			arguments.Radius = myPlanetStorageProvider.Radius;
			arguments.AtmosphereRadius = num3;
			arguments.MaxRadius = maxRadius;
			arguments.MinRadius = minRadius;
			arguments.HasAtmosphere = definition.HasAtmosphere;
			arguments.AtmosphereWavelengths = Vector3.Zero;
			arguments.GravityFalloff = definition.GravityFalloffPower;
			arguments.MarkAreaEmpty = true;
			arguments.AtmosphereSettings = (definition.AtmosphereSettings ?? MyAtmosphereSettings.Defaults());
			arguments.SurfaceGravity = definition.SurfaceGravity;
			arguments.AddGps = false;
			arguments.SpherizeWithDistance = true;
			arguments.Generator = definition;
			arguments.UserCreated = true;
			arguments.InitializeComponents = true;
			arguments.FadeIn = false;
			myPlanet.Init(arguments);
			SpawnAsteroidInfo lastAsteroidInfo = default(SpawnAsteroidInfo);
			lastAsteroidInfo.Asteroid = null;
			lastAsteroidInfo.RandomSeed = seed;
			lastAsteroidInfo.Position = Vector3D.Zero;
			lastAsteroidInfo.IsProcedural = true;
			lastAsteroidInfo.ProceduralRadius = size;
			m_lastAsteroidInfo = lastAsteroidInfo;
			MyClipboardComponent.Static.ActivateVoxelClipboard(myPlanet.GetObjectBuilder(), myStorage, MySector.MainCamera.ForwardVector, (myStorage.Size * 0.5f).Length());
			myPlanet.Close();
		}

		public static void SpawnPlanet(Vector3D pos)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SpawnPlanet_Server, m_selectedPlanetName, m_lastAsteroidInfo.ProceduralRadius, m_lastAsteroidInfo.RandomSeed, pos);
		}

		[Event(null, 1320)]
		[Reliable]
		[Server]
		private static void SpawnPlanet_Server(string planetName, float size, int seed, Vector3D pos)
		{
			if (MyEventContext.Current.IsLocallyInvoked || (MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value) && (MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode)))
			{
				string text = planetName + "-" + seed + "d" + size;
				MakeStorageName(text);
				long num = MyRandom.Instance.NextLong();
				MyWorldGenerator.AddPlanet(text, planetName, planetName, pos, seed, size, fadeIn: true, num, addGPS: false, userCreated: true);
				if (MySession.Static.RequiresDX < 11)
				{
					MySession.Static.RequiresDX = 11;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SpawnPlanet_Client, planetName, text, size, seed, pos, num);
			}
			else
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

		[Event(null, 1343)]
		[Reliable]
		[Broadcast]
		private static void SpawnPlanet_Client(string planetName, string storageNameBase, float size, int seed, Vector3D pos, long entityId)
		{
			MyWorldGenerator.AddPlanet(storageNameBase, planetName, planetName, pos, seed, size, fadeIn: true, entityId, addGPS: false, userCreated: true);
			if (MySession.Static.RequiresDX < 11)
			{
				MySession.Static.RequiresDX = 11;
			}
		}

		private void CreateEmptyVoxelMapSpawnMenu(float separatorSize, float usableWidth)
		{
			m_currentPosition.Y += 0.025f;
			MyGuiControlLabel label = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = "Voxel Size: "
			};
			Controls.Add(label);
			m_currentPosition.Y += 0.03f;
			MyGuiControlSlider myGuiControlSlider = null;
			myGuiControlSlider = new MyGuiControlSlider(m_currentPosition, 2f, 10f, 0.285f, null, null, string.Empty, 1, 0.8f, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
			myGuiControlSlider.DebugScale = m_sliderDebugScale;
			myGuiControlSlider.ColorMask = Color.White.ToVector4();
			Controls.Add(myGuiControlSlider);
			label = new MyGuiControlLabel(m_currentPosition + new Vector2(myGuiControlSlider.Size.X - 0.003f, myGuiControlSlider.Size.Y - 0.065f), null, string.Empty, Color.White.ToVector4(), 0.8f, "Debug");
			label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			Controls.Add(label);
			MyGuiControlSlider myGuiControlSlider2 = myGuiControlSlider;
			myGuiControlSlider2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider2.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				int num2 = 1 << (int)s.Value;
				label.Text = num2 + "m";
				m_procAsteroidSizeValue = num2;
			});
			myGuiControlSlider.Value = 5f;
			m_currentPosition.Y += myGuiControlSlider.Size.Y;
			m_currentPosition.Y += separatorSize;
			m_currentPosition.Y -= 0.01f;
			CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, delegate
			{
				int num = (int)m_procAsteroidSizeValue;
				MyStorageBase myStorageBase = new MyOctreeStorage(null, new Vector3I(num));
				MyObjectBuilder_VoxelMap voxelMap = CreateAsteroidObjectBuilder(MakeStorageName("MyEmptyVoxelMap"));
				m_lastAsteroidInfo.Asteroid = null;
				m_lastAsteroidInfo.ProceduralRadius = num;
				MyClipboardComponent.Static.ActivateVoxelClipboard(voxelMap, myStorageBase, MySector.MainCamera.ForwardVector, (myStorageBase.Size * 0.5f).Length());
				CloseScreenNow();
			});
		}
	}
}
