using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
	[StaticEventOwner]
	internal class MyTrashOtherTabContainer : MyTabContainer
	{
		protected sealed class RemoveFloating_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveFloating_Implementation();
			}
		}

		protected sealed class StopEntities_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StopEntities_Implementation();
			}
		}

		private MyGuiControlTextbox m_textboxOptimalGridCount;

		private MyGuiControlTextbox m_textboxCharacterRemovalTrash;

		private MyGuiControlTextbox m_textboxAfkTimeout;

		private MyGuiControlTextbox m_tbStopGridsPeriod;

		private MyGuiControlTextbox m_tbRemoveInactiveIdent;

		public MyTrashOtherTabContainer(MyGuiScreenBase parentScreen)
			: base(parentScreen)
		{
			base.Control.Size = new Vector2(base.Control.Size.X, 0.265f);
			Vector2 currentPosition = -base.Control.Size * 0.5f;
			Vector2? size = parentScreen.GetSize();
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_OptimalGridCount)
			};
			control.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_OptimalGridCount_Tooltip));
			base.Control.Controls.Add(control);
			m_textboxOptimalGridCount = AddTextbox(ref currentPosition, MySession.Static.Settings.OptimalGridCount.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxOptimalGridCount);
			m_textboxOptimalGridCount.Size = new Vector2(0.07f, m_textboxOptimalGridCount.Size.Y);
			m_textboxOptimalGridCount.PositionY = currentPosition.Y - 0.01f;
			m_textboxOptimalGridCount.PositionX = currentPosition.X + size.Value.X - m_textboxOptimalGridCount.Size.X - 0.045f;
			m_textboxOptimalGridCount.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_OptimalGridCount_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerCharacterRemoval)
			};
			control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerCharacterRemoval_Tooltip));
			base.Control.Controls.Add(control2);
			m_textboxCharacterRemovalTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.PlayerCharacterRemovalThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxCharacterRemovalTrash);
			m_textboxCharacterRemovalTrash.Size = new Vector2(0.07f, m_textboxCharacterRemovalTrash.Size.Y);
			m_textboxCharacterRemovalTrash.PositionX = currentPosition.X + size.Value.X - m_textboxCharacterRemovalTrash.Size.X - 0.045f;
			m_textboxCharacterRemovalTrash.PositionY = currentPosition.Y - 0.01f;
			m_textboxCharacterRemovalTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerCharacterRemoval_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control3 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_AFKTimeout)
			};
			control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_AFKTimeout_TTIP));
			base.Control.Controls.Add(control3);
			m_textboxAfkTimeout = AddTextbox(ref currentPosition, MySession.Static.Settings.AFKTimeountMin.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_textboxAfkTimeout);
			m_textboxAfkTimeout.Size = new Vector2(0.07f, m_textboxAfkTimeout.Size.Y);
			m_textboxAfkTimeout.PositionX = currentPosition.X + size.Value.X - m_textboxAfkTimeout.Size.X - 0.045f;
			m_textboxAfkTimeout.PositionY = currentPosition.Y - 0.01f;
			m_textboxAfkTimeout.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_AFKTimeout_TTIP));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control4 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_StopGridsPeriod)
			};
			control4.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_StopGridsPeriod_TTIP));
			base.Control.Controls.Add(control4);
			m_tbStopGridsPeriod = AddTextbox(ref currentPosition, MySession.Static.Settings.StopGridsPeriodMin.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_tbStopGridsPeriod);
			m_tbStopGridsPeriod.Size = new Vector2(0.07f, m_tbStopGridsPeriod.Size.Y);
			m_tbStopGridsPeriod.PositionX = currentPosition.X + size.Value.X - m_tbStopGridsPeriod.Size.X - 0.045f;
			m_tbStopGridsPeriod.PositionY = currentPosition.Y - 0.01f;
			m_tbStopGridsPeriod.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_StopGridsPeriod_TTIP));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control5 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_RemoveInactiveEnt)
			};
			control5.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_RemoveInactiveEnt_TTIP));
			base.Control.Controls.Add(control5);
			m_tbRemoveInactiveIdent = AddTextbox(ref currentPosition, MySession.Static.Settings.RemoveOldIdentitiesH.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false);
			base.Control.Controls.Add(m_tbRemoveInactiveIdent);
			m_tbRemoveInactiveIdent.Size = new Vector2(0.07f, m_tbRemoveInactiveIdent.Size.Y);
			m_tbRemoveInactiveIdent.PositionX = currentPosition.X + size.Value.X - m_tbRemoveInactiveIdent.Size.X - 0.045f;
			m_tbRemoveInactiveIdent.PositionY = currentPosition.Y - 0.01f;
			m_tbRemoveInactiveIdent.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_RemoveInactiveEnt_TTIP));
			currentPosition.Y += 0.045f;
			float num = 0.14f;
			Vector2 currentPosition2 = currentPosition + new Vector2(num * 0.5f, 0f);
			MyGuiControlButton control6 = CreateDebugButton(ref currentPosition2, num, MySpaceTexts.ScreenDebugAdminMenu_RemoveFloating, OnRemoveFloating, enabled: true, null, increaseSpacing: false, addToControls: false);
			base.Control.Controls.Add(control6);
			float num2 = 0.286f - 2f * num;
			currentPosition2 = currentPosition + new Vector2(num * 1.5f + num2, 0f);
			MyGuiControlButton control7 = CreateDebugButton(ref currentPosition2, num, MySpaceTexts.ScreenDebugAdminMenu_StopAll, OnStopEntities, enabled: true, null, increaseSpacing: false, addToControls: false);
			base.Control.Controls.Add(control7);
		}

		private void OnRemoveFloating(MyGuiControlButton obj)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveFloating_Implementation);
		}

		[Event(null, 153)]
		[Reliable]
		[Server]
		private static void RemoveFloating_Implementation()
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					if (entity is MyFloatingObject || entity is MyInventoryBagEntity)
					{
						entity.Close();
					}
				}
			}
		}

		private void OnStopEntities(MyGuiControlButton myGuiControlButton)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => StopEntities_Implementation);
		}

		[Event(null, 173)]
		[Server]
		[Reliable]
		private static void StopEntities_Implementation()
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					if (entity.Physics != null && !entity.Closed && !(entity is MyCharacter) && MySession.Static.Players.GetEntityController(entity) == null)
					{
						entity.Physics.ClearSpeed();
					}
				}
			}
		}

		internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings newSettings)
		{
			if (m_textboxCharacterRemovalTrash == null || m_textboxOptimalGridCount == null || m_textboxAfkTimeout == null || m_tbStopGridsPeriod == null || m_tbRemoveInactiveIdent == null)
			{
				return false;
			}
			int.TryParse(m_textboxCharacterRemovalTrash.Text, out int result);
			int.TryParse(m_textboxOptimalGridCount.Text, out int result2);
			int.TryParse(m_textboxAfkTimeout.Text, out int result3);
			int.TryParse(m_tbStopGridsPeriod.Text, out int result4);
			int.TryParse(m_tbRemoveInactiveIdent.Text, out int result5);
			int result6 = 0 | ((MySession.Static.Settings.PlayerCharacterRemovalThreshold != result) ? 1 : 0) | ((MySession.Static.Settings.OptimalGridCount != result2) ? 1 : 0) | ((MySession.Static.Settings.AFKTimeountMin != result3) ? 1 : 0) | ((MySession.Static.Settings.StopGridsPeriodMin != result4) ? 1 : 0) | ((MySession.Static.Settings.RemoveOldIdentitiesH != result5) ? 1 : 0);
			newSettings.characterRemovalThreshold = result;
			newSettings.gridCount = result2;
			newSettings.afkTimeout = result3;
			newSettings.stopGridsPeriod = result4;
			newSettings.removeOldIdentities = result5;
			return (byte)result6 != 0;
		}
	}
}
