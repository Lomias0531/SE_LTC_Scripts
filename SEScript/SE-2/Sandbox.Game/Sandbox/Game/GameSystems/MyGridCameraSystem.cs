using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;

namespace Sandbox.Game.GameSystems
{
	public class MyGridCameraSystem
	{
		private MyCubeGrid m_grid;

		private readonly List<MyCameraBlock> m_cameras;

		private readonly List<MyCameraBlock> m_relayedCameras;

		private MyCameraBlock m_currentCamera;

		private bool m_ignoreNextInput;

		private static MyHudCameraOverlay m_cameraOverlay;

		public int CameraCount => m_cameras.Count;

		public MyCameraBlock CurrentCamera => m_currentCamera;

		public static IMyCameraController PreviousNonCameraBlockController
		{
			get;
			set;
		}

		public bool NeedsPerFrameUpdate => m_currentCamera != null;

		static MyGridCameraSystem()
		{
		}

		public MyGridCameraSystem(MyCubeGrid grid)
		{
			m_grid = grid;
			m_cameras = new List<MyCameraBlock>();
			m_relayedCameras = new List<MyCameraBlock>();
		}

		public void Register(MyCameraBlock camera)
		{
			m_cameras.Add(camera);
			m_grid.MarkForUpdate();
		}

		public void Unregister(MyCameraBlock camera)
		{
			if (camera == m_currentCamera)
			{
				ResetCamera();
			}
			m_cameras.Remove(camera);
			m_grid.MarkForUpdate();
		}

		public void CheckCurrentCameraStillValid()
		{
			if (m_currentCamera != null && !m_currentCamera.IsWorking)
			{
				ResetCamera();
			}
		}

		public void SetAsCurrent(MyCameraBlock newCamera)
		{
			if (m_currentCamera != newCamera)
			{
				if (newCamera.BlockDefinition.OverlayTexture != null)
				{
					MyHudCameraOverlay.TextureName = newCamera.BlockDefinition.OverlayTexture;
					MyHudCameraOverlay.Enabled = true;
				}
				else
				{
					MyHudCameraOverlay.Enabled = false;
				}
				string shipName = "";
				if (MyAntennaSystem.Static != null)
				{
					shipName = (MyAntennaSystem.Static.GetLogicalGroupRepresentative(m_grid).DisplayName ?? "");
				}
				string displayNameText = newCamera.DisplayNameText;
				MyHud.CameraInfo.Enable(shipName, displayNameText);
				m_currentCamera = newCamera;
				m_ignoreNextInput = true;
				MySessionComponentVoxelHand.Static.Enabled = false;
				MySession.Static.GameFocusManager.Clear();
				m_grid.MarkForUpdate();
			}
		}

		public void UpdateBeforeSimulation()
		{
			if (m_currentCamera == null)
			{
				return;
			}
			if (MySession.Static.CameraController != m_currentCamera)
			{
				if (!(MySession.Static.CameraController is MyCameraBlock))
				{
					DisableCameraEffects();
				}
				ResetCurrentCamera();
				return;
			}
			if (m_ignoreNextInput)
			{
				m_ignoreNextInput = false;
				return;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_LEFT) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				SetPrev();
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_RIGHT) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				SetNext();
			}
			if (MyInput.Static.DeltaMouseScrollWheelValue() != 0 && MyGuiScreenToolbarConfigBase.Static == null && !MyGuiScreenTerminal.IsOpen)
			{
				m_currentCamera.ChangeZoom(MyInput.Static.DeltaMouseScrollWheelValue());
			}
		}

		public void UpdateBeforeSimulation10()
		{
			if (m_currentCamera != null && !CameraIsInRangeAndPlayerHasAccess(m_currentCamera))
			{
				ResetCamera();
			}
		}

		public static bool CameraIsInRangeAndPlayerHasAccess(MyCameraBlock camera)
		{
			if (MySession.Static.ControlledEntity != null)
			{
				if (((IMyComponentOwner<MyIDModule>)camera).GetComponent(out MyIDModule component) && !camera.HasPlayerAccess(MySession.Static.LocalPlayerId) && component.Owner != 0L)
				{
					return false;
				}
				if (MySession.Static.ControlledEntity is MyCharacter)
				{
					return MyAntennaSystem.Static.CheckConnection(MySession.Static.LocalCharacter, camera.CubeGrid, MySession.Static.LocalHumanPlayer);
				}
				if (MySession.Static.ControlledEntity is MyShipController)
				{
					return MyAntennaSystem.Static.CheckConnection((MySession.Static.ControlledEntity as MyShipController).CubeGrid, camera.CubeGrid, MySession.Static.LocalHumanPlayer);
				}
				if (MySession.Static.ControlledEntity is MyCubeBlock)
				{
					return MyAntennaSystem.Static.CheckConnection((MySession.Static.ControlledEntity as MyCubeBlock).CubeGrid, camera.CubeGrid, MySession.Static.LocalHumanPlayer);
				}
			}
			return false;
		}

		public void ResetCamera()
		{
			ResetCurrentCamera();
			DisableCameraEffects();
			bool flag = false;
			if (PreviousNonCameraBlockController != null)
			{
				MyEntity myEntity = PreviousNonCameraBlockController as MyEntity;
				if (myEntity != null && !myEntity.Closed)
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, myEntity);
					PreviousNonCameraBlockController = null;
					flag = true;
				}
			}
			if (!flag && MySession.Static.LocalCharacter != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.LocalCharacter);
			}
		}

		private void DisableCameraEffects()
		{
			MyHudCameraOverlay.Enabled = false;
			MyHud.CameraInfo.Disable();
			MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
		}

		public void ResetCurrentCamera()
		{
			if (m_currentCamera != null)
			{
				m_currentCamera.OnExitView();
				m_currentCamera = null;
			}
		}

		private void SetNext()
		{
			UpdateRelayedCameras();
			MyCameraBlock next = GetNext(m_currentCamera);
			if (next != null)
			{
				SetCamera(next);
			}
		}

		private void SetPrev()
		{
			UpdateRelayedCameras();
			MyCameraBlock prev = GetPrev(m_currentCamera);
			if (prev != null)
			{
				SetCamera(prev);
			}
		}

		private void SetCamera(MyCameraBlock newCamera)
		{
			if (newCamera != m_currentCamera)
			{
				if (m_cameras.Contains(newCamera))
				{
					SetAsCurrent(newCamera);
					newCamera.SetView();
					return;
				}
				MyHudCameraOverlay.Enabled = false;
				MyHud.CameraInfo.Disable();
				ResetCurrentCamera();
				newCamera.RequestSetView();
			}
		}

		private void UpdateRelayedCameras()
		{
			List<MyAntennaSystem.BroadcasterInfo> list = MyAntennaSystem.Static.GetConnectedGridsInfo(m_grid).ToList();
			list.Sort((MyAntennaSystem.BroadcasterInfo b1, MyAntennaSystem.BroadcasterInfo b2) => b1.EntityId.CompareTo(b2.EntityId));
			m_relayedCameras.Clear();
			foreach (MyAntennaSystem.BroadcasterInfo item in list)
			{
				AddValidCamerasFromGridToRelayed(item.EntityId);
			}
			if (m_relayedCameras.Count == 0)
			{
				AddValidCamerasFromGridToRelayed(m_grid);
			}
		}

		private void AddValidCamerasFromGridToRelayed(long gridId)
		{
			MyEntities.TryGetEntityById(gridId, out MyCubeGrid entity);
			if (entity != null)
			{
				AddValidCamerasFromGridToRelayed(entity);
			}
		}

		private void AddValidCamerasFromGridToRelayed(MyCubeGrid grid)
		{
			foreach (MyTerminalBlock block in grid.GridSystems.TerminalSystem.Blocks)
			{
				MyCameraBlock myCameraBlock = block as MyCameraBlock;
				if (myCameraBlock != null && myCameraBlock.IsWorking && myCameraBlock.HasLocalPlayerAccess())
				{
					m_relayedCameras.Add(myCameraBlock);
				}
			}
		}

		private MyCameraBlock GetNext(MyCameraBlock current)
		{
			if (m_relayedCameras.Count == 1)
			{
				return current;
			}
			int num = m_relayedCameras.IndexOf(current);
			if (num == -1)
			{
				ResetCamera();
				return null;
			}
			return m_relayedCameras[(num + 1) % m_relayedCameras.Count];
		}

		private MyCameraBlock GetPrev(MyCameraBlock current)
		{
			if (m_relayedCameras.Count == 1)
			{
				return current;
			}
			int num = m_relayedCameras.IndexOf(current);
			if (num == -1)
			{
				ResetCamera();
				return null;
			}
			int num2 = num - 1;
			if (num2 < 0)
			{
				num2 = m_relayedCameras.Count - 1;
			}
			return m_relayedCameras[num2];
		}

		public void PrepareForDraw()
		{
			_ = m_currentCamera;
		}
	}
}
