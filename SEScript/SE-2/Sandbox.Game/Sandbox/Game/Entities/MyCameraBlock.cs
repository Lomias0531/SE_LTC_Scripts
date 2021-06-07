using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_CameraBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyCameraBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyCameraBlock)
	})]
	public class MyCameraBlock : MyFunctionalBlock, IMyCameraController, Sandbox.ModAPI.IMyCameraBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyCameraBlock
	{
		private struct RaycastInfo
		{
			public Vector3D Start;

			public Vector3D End;

			public Vector3D? Hit;

			public double Distance;
		}

		protected class m_syncFov_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType syncFov;
				ISyncType result = syncFov = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyCameraBlock)P_0).m_syncFov = (Sync<float, SyncDirection.BothWays>)syncFov;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyCameraBlock_003C_003EActor : IActivator, IActivator<MyCameraBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyCameraBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCameraBlock CreateInstance()
			{
				return new MyCameraBlock();
			}

			MyCameraBlock IActivator<MyCameraBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float MIN_FOV = 1E-05f;

		private const float MAX_FOV = MathF.PI * 179f / 180f;

		private int m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;

		private double m_availableScanRange;

		private float m_fov;

		private float m_targetFov;

		private RaycastInfo m_lastRay;

		private static MyHudNotification m_hudNotification;

		private bool m_requestActivateAfterLoad;

		private IMyCameraController m_previousCameraController;

		private readonly Sync<float, SyncDirection.BothWays> m_syncFov;

		private double m_angleLimitCosine;

		public new MyCameraBlockDefinition BlockDefinition => (MyCameraBlockDefinition)base.BlockDefinition;

		private double AvailableScanRange
		{
			get
			{
				if (base.IsWorking && EnableRaycast)
				{
					m_availableScanRange = Math.Min(double.MaxValue, m_availableScanRange + (double)((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) * BlockDefinition.RaycastTimeMultiplier));
					m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
				return m_availableScanRange;
			}
			set
			{
				m_availableScanRange = value;
			}
		}

		public bool EnableRaycast
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			private set;
		}

		public bool IsInFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool ForceFirstPersonCamera
		{
			get;
			set;
		}

		public bool EnableFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public MyEntity Entity => this;

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return IsInFirstPersonView;
			}
			set
			{
				IsInFirstPersonView = value;
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				return ForceFirstPersonCamera;
			}
			set
			{
				ForceFirstPersonCamera = value;
			}
		}

		bool IMyCameraController.AllowCubeBuilding => false;

		double Sandbox.ModAPI.Ingame.IMyCameraBlock.AvailableScanRange => AvailableScanRange;

		bool Sandbox.ModAPI.Ingame.IMyCameraBlock.EnableRaycast
		{
			get
			{
				return EnableRaycast;
			}
			set
			{
				if (EnableRaycast != value)
				{
					m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
				EnableRaycast = value;
				base.ResourceSink.Update();
			}
		}

		float Sandbox.ModAPI.Ingame.IMyCameraBlock.RaycastConeLimit => BlockDefinition.RaycastConeLimit;

		double Sandbox.ModAPI.Ingame.IMyCameraBlock.RaycastDistanceLimit => BlockDefinition.RaycastDistanceLimit;

		public MyCameraBlock()
		{
			CreateTerminalControls();
			m_syncFov.ValueChanged += delegate
			{
				OnSyncFov();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyCameraBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlButton<MyCameraBlock> obj = new MyTerminalControlButton<MyCameraBlock>("View", MySpaceTexts.BlockActionTitle_View, MySpaceTexts.Blank, delegate(MyCameraBlock b)
				{
					b.RequestSetView();
				})
				{
					Enabled = ((MyCameraBlock b) => b.CanUse()),
					SupportsMultipleBlocks = false
				};
				MyTerminalAction<MyCameraBlock> myTerminalAction = obj.EnableAction(MyTerminalActionIcons.TOGGLE);
				if (myTerminalAction != null)
				{
					myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>
					{
						MyToolbarType.ButtonPanel
					};
					myTerminalAction.ValidForGroups = false;
				}
				MyTerminalControlFactory.AddControl(obj);
				string controlButtonName = MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
				m_hudNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToExitCamera);
				m_hudNotification.SetTextFormatArguments(controlButtonName);
			}
		}

		public bool CanUse()
		{
			if (base.IsWorking)
			{
				return MyGridCameraSystem.CameraIsInRangeAndPlayerHasAccess(this);
			}
			return false;
		}

		public void RequestSetView()
		{
			if (base.IsWorking)
			{
				MyHud.Notifications.Remove(m_hudNotification);
				MyHud.Notifications.Add(m_hudNotification);
				base.CubeGrid.GridSystems.CameraSystem.SetAsCurrent(this);
				SetView();
				if (MyGuiScreenTerminal.IsOpen)
				{
					MyGuiScreenTerminal.Hide();
				}
			}
		}

		public void SetView()
		{
			MyCameraBlock myCameraBlock = MySession.Static.CameraController as MyCameraBlock;
			if (myCameraBlock != null)
			{
				myCameraBlock.IsActive = false;
			}
			MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
			SetFov(m_fov);
			IsActive = true;
			CheckEmissiveState();
		}

		private static void SetFov(float fov)
		{
			fov = MathHelper.Clamp(fov, 1E-05f, MathF.PI * 179f / 180f);
			MySector.MainCamera.FieldOfView = fov;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute(BlockDefinition.ResourceSinkGroup), BlockDefinition.RequiredPowerInput, CalculateRequiredPowerInput);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.RequiredInputChanged += Receiver_RequiredInputChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			myResourceSinkComponent.Update();
			MyObjectBuilder_CameraBlock myObjectBuilder_CameraBlock = objectBuilder as MyObjectBuilder_CameraBlock;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyCameraBlock_IsWorkingChanged;
			IsInFirstPersonView = true;
			if (myObjectBuilder_CameraBlock.IsActive)
			{
				m_requestActivateAfterLoad = true;
				myObjectBuilder_CameraBlock.IsActive = false;
			}
			OnChangeFov(myObjectBuilder_CameraBlock.Fov);
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void MyCameraBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			base.CubeGrid.GridSystems.CameraSystem.CheckCurrentCameraStillValid();
			if (m_requestActivateAfterLoad && base.IsWorking)
			{
				m_requestActivateAfterLoad = false;
				RequestSetView();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.GridSystems.CameraSystem.CurrentCamera == this)
			{
				m_fov = MathHelper.Lerp(m_fov, m_targetFov, 0.5f);
				SetFov(m_fov);
			}
			if (Math.Abs(m_fov - m_targetFov) < 0.01f && !MyDebugDrawSettings.ENABLE_DEBUG_DRAW && !base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && m_lastRay.Distance != 0.0)
			{
				DrawDebug();
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			base.ResourceSink.Update();
		}

		private void DrawDebug()
		{
			MyRenderProxy.DebugDrawLine3D(m_lastRay.Start, m_lastRay.End, Color.Orange, Color.Orange, depthRead: false);
			if (m_lastRay.Hit.HasValue)
			{
				MyRenderProxy.DebugDrawSphere(m_lastRay.Hit.Value, 1f, Color.Orange, 1f, depthRead: false);
			}
			double scaleFactor = m_lastRay.Distance / Math.Cos(MathHelper.ToRadians(BlockDefinition.RaycastConeLimit));
			Vector3D[] array = new Vector3D[4];
			Vector3D translation = base.WorldMatrix.Translation;
			Vector3D vector = base.WorldMatrix.Forward;
			float num = MathHelper.ToRadians(0f - BlockDefinition.RaycastConeLimit);
			float num2 = MathHelper.ToRadians(0f - BlockDefinition.RaycastConeLimit);
			MatrixD matrix = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Right, num);
			MatrixD matrix2 = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Down, num2);
			Vector3D.RotateAndScale(ref vector, ref matrix, out Vector3D result);
			Vector3D.RotateAndScale(ref result, ref matrix2, out Vector3D result2);
			array[0] = translation + result2 * scaleFactor;
			num = MathHelper.ToRadians(0f - BlockDefinition.RaycastConeLimit);
			num2 = MathHelper.ToRadians(BlockDefinition.RaycastConeLimit);
			matrix = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Right, num);
			matrix2 = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Down, num2);
			Vector3D.RotateAndScale(ref vector, ref matrix, out result);
			Vector3D.RotateAndScale(ref result, ref matrix2, out result2);
			array[1] = translation + result2 * scaleFactor;
			num = MathHelper.ToRadians(BlockDefinition.RaycastConeLimit);
			num2 = MathHelper.ToRadians(BlockDefinition.RaycastConeLimit);
			matrix = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Right, num);
			matrix2 = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Down, num2);
			Vector3D.RotateAndScale(ref vector, ref matrix, out result);
			Vector3D.RotateAndScale(ref result, ref matrix2, out result2);
			array[2] = translation + result2 * scaleFactor;
			num = MathHelper.ToRadians(BlockDefinition.RaycastConeLimit);
			num2 = MathHelper.ToRadians(0f - BlockDefinition.RaycastConeLimit);
			matrix = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Right, num);
			matrix2 = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Down, num2);
			Vector3D.RotateAndScale(ref vector, ref matrix, out result);
			Vector3D.RotateAndScale(ref result, ref matrix2, out result2);
			array[3] = translation + result2 * scaleFactor;
			MyRenderProxy.DebugDrawLine3D(translation, array[0], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(translation, array[1], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(translation, array[2], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(translation, array[3], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(array[0], array[1], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(array[1], array[2], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(array[2], array[3], Color.Blue, Color.Blue, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(array[3], array[0], Color.Blue, Color.Blue, depthRead: false);
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.ResourceSink.Update();
		}

		public void OnExitView()
		{
			IsActive = false;
			m_syncFov.Value = m_fov;
			CheckEmissiveState();
		}

		public override bool SetEmissiveStateWorking()
		{
			if (IsActive)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
			}
			return base.SetEmissiveStateWorking();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CameraBlock obj = (MyObjectBuilder_CameraBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.IsActive = IsActive;
			obj.Fov = m_fov;
			return obj;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private void Receiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
		}

		private float CalculateRequiredPowerInput()
		{
			if (!EnableRaycast)
			{
				return BlockDefinition.RequiredPowerInput;
			}
			return BlockDefinition.RequiredChargingInput;
		}

		public override MatrixD GetViewMatrix()
		{
			MatrixD matrix = base.WorldMatrix;
			matrix.Translation += base.WorldMatrix.Forward * 0.20000000298023224;
			MatrixD.Invert(ref matrix, out MatrixD result);
			return result;
		}

		public void Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
		}

		public void RotateStopped()
		{
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			currentCamera.SetViewMatrix(GetViewMatrix());
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			Rotate(rotationIndicator, rollIndicator);
		}

		void IMyCameraController.RotateStopped()
		{
			RotateStopped();
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			if (!(previousCameraController is MyCameraBlock))
			{
				MyGridCameraSystem.PreviousNonCameraBlockController = previousCameraController;
			}
			OnAssumeControl(previousCameraController);
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			OnReleaseControl(newCameraController);
		}

		bool IMyCameraController.HandleUse()
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
			base.CubeGrid.GridSystems.CameraSystem.ResetCamera();
			return true;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		internal void ChangeZoom(int deltaZoom)
		{
			if (deltaZoom > 0)
			{
				m_targetFov -= 0.15f;
				if (m_targetFov < BlockDefinition.MinFov)
				{
					m_targetFov = BlockDefinition.MinFov;
				}
			}
			else
			{
				m_targetFov += 0.15f;
				if (m_targetFov > BlockDefinition.MaxFov)
				{
					m_targetFov = BlockDefinition.MaxFov;
				}
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			SetFov(m_fov);
		}

		internal void OnChangeFov(float newFov)
		{
			m_fov = newFov;
			if (m_fov > BlockDefinition.MaxFov)
			{
				m_fov = BlockDefinition.MaxFov;
			}
			if (m_fov < BlockDefinition.MinFov)
			{
				m_fov = BlockDefinition.MinFov;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			m_targetFov = m_fov;
		}

		private void OnSyncFov()
		{
			if (!IsActive)
			{
				OnChangeFov(m_syncFov);
			}
		}

		public bool CheckAngleLimits(Vector3D directionNormalized)
		{
			if (m_angleLimitCosine == 0.0)
			{
				m_angleLimitCosine = Math.Cos(MathHelper.ToRadians(BlockDefinition.RaycastConeLimit));
			}
			Vector3D value = VectorProjection(directionNormalized, base.WorldMatrix.Forward);
			Vector3D value2 = VectorProjection(directionNormalized, base.WorldMatrix.Left);
			Vector3D value3 = VectorProjection(directionNormalized, base.WorldMatrix.Up);
			Vector3D vector3D = value + value2;
			Vector3D vector3D2 = value + value3;
			double num = vector3D.Dot(base.WorldMatrix.Forward);
			double num2 = vector3D2.Dot(base.WorldMatrix.Forward);
			if (!(num < m_angleLimitCosine))
			{
				return !(num2 < m_angleLimitCosine);
			}
			return false;
		}

		private Vector3D VectorProjection(Vector3D a, Vector3D b)
		{
			return a.Dot(b) / b.LengthSquared() * b;
		}

		MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyCameraBlock.Raycast(double distance, Vector3D targetDirection)
		{
			if (Vector3D.IsZero(targetDirection))
			{
				throw new ArgumentOutOfRangeException("targetDirection", "Direction cannot be 0,0,0");
			}
			targetDirection = Vector3D.TransformNormal(targetDirection, base.WorldMatrix);
			targetDirection.Normalize();
			if (CheckAngleLimits(targetDirection))
			{
				return Raycast(distance, targetDirection);
			}
			return default(MyDetectedEntityInfo);
		}

		MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyCameraBlock.Raycast(Vector3D targetPos)
		{
			Vector3D vector3D = Vector3D.Normalize(targetPos - base.WorldMatrix.Translation);
			if (CheckAngleLimits(vector3D))
			{
				return Raycast(Vector3D.Distance(targetPos, base.WorldMatrix.Translation), vector3D);
			}
			return default(MyDetectedEntityInfo);
		}

		MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyCameraBlock.Raycast(double distance, float pitch, float yaw)
		{
			if (pitch > BlockDefinition.RaycastConeLimit || yaw > BlockDefinition.RaycastConeLimit)
			{
				return default(MyDetectedEntityInfo);
			}
			pitch = MathHelper.ToRadians(pitch);
			yaw = MathHelper.ToRadians(yaw);
			Vector3D vector = base.WorldMatrix.Forward;
			MatrixD matrix = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Right, pitch);
			MatrixD matrix2 = MatrixD.CreateFromAxisAngle(base.WorldMatrix.Down, yaw);
			Vector3D.RotateAndScale(ref vector, ref matrix, out Vector3D result);
			Vector3D.RotateAndScale(ref result, ref matrix2, out Vector3D result2);
			return Raycast(distance, result2);
		}

		public MyDetectedEntityInfo Raycast(double distance, Vector3D direction)
		{
			if (Vector3D.IsZero(direction))
			{
				throw new ArgumentOutOfRangeException("direction", "Direction cannot be 0,0,0");
			}
			if (distance <= 0.0 || (BlockDefinition.RaycastDistanceLimit > -1.0 && distance > BlockDefinition.RaycastDistanceLimit))
			{
				return default(MyDetectedEntityInfo);
			}
			if (AvailableScanRange < distance || !CheckIsWorking())
			{
				return default(MyDetectedEntityInfo);
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			AvailableScanRange -= distance;
			Vector3D translation = base.WorldMatrix.Translation;
			Vector3D vector3D = translation + direction * distance;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(translation, vector3D, list);
			RaycastInfo lastRay;
			foreach (MyPhysics.HitInfo item in list)
			{
				MyEntity myEntity = (MyEntity)item.HkHitInfo.GetHitEntity();
				if (myEntity != this)
				{
					lastRay = new RaycastInfo
					{
						Distance = distance,
						Start = translation,
						End = vector3D,
						Hit = item.Position
					};
					m_lastRay = lastRay;
					return MyDetectedEntityInfoHelper.Create(myEntity, base.OwnerId, item.Position);
				}
			}
			LineD ray = new LineD(translation, vector3D);
			List<MyLineSegmentOverlapResult<MyVoxelBase>> list2 = new List<MyLineSegmentOverlapResult<MyVoxelBase>>();
			MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref ray, list2);
			foreach (MyLineSegmentOverlapResult<MyVoxelBase> item2 in list2)
			{
				MyPlanet myPlanet = item2.Element as MyPlanet;
				if (myPlanet != null)
				{
					double num = Vector3D.DistanceSquared(base.PositionComp.GetPosition(), myPlanet.PositionComp.GetPosition());
					MyGravityProviderComponent myGravityProviderComponent = myPlanet.Components.Get<MyGravityProviderComponent>();
					if (myGravityProviderComponent != null)
					{
						if (!myGravityProviderComponent.IsPositionInRange(translation) && num > (double)(myPlanet.MaximumRadius * myPlanet.MaximumRadius))
						{
							BoundingSphereD boundingSphereD = new BoundingSphereD(myPlanet.PositionComp.GetPosition(), myPlanet.MaximumRadius);
							RayD ray2 = new RayD(translation, direction);
							double? num2 = boundingSphereD.Intersects(ray2);
							if (num2.HasValue && !(distance < num2.Value))
							{
								Vector3D value = translation + direction * num2.Value;
								lastRay = new RaycastInfo
								{
									Distance = distance,
									Start = translation,
									End = vector3D,
									Hit = value
								};
								m_lastRay = lastRay;
								return MyDetectedEntityInfoHelper.Create(item2.Element, base.OwnerId, value);
							}
						}
						else if (myPlanet.RootVoxel.Storage != null)
						{
							Vector3 sizeInMetresHalf = myPlanet.SizeInMetresHalf;
							MatrixD worldMatrixInvScaled = myPlanet.PositionComp.WorldMatrixInvScaled;
							Line localLine = new Line(Vector3D.Transform(ray.From, worldMatrixInvScaled) + sizeInMetresHalf, Vector3D.Transform(ray.To, worldMatrixInvScaled) + sizeInMetresHalf);
							if (myPlanet.RootVoxel.Storage.GetGeometry().Intersect(ref localLine, out MyIntersectionResultLineTriangle result, IntersectionFlags.DIRECT_TRIANGLES))
							{
								Vector3D value2 = localLine.From + localLine.Direction * result.Distance + myPlanet.PositionLeftBottomCorner;
								lastRay = new RaycastInfo
								{
									Distance = distance,
									Start = translation,
									End = vector3D,
									Hit = value2
								};
								m_lastRay = lastRay;
								return MyDetectedEntityInfoHelper.Create(item2.Element, base.OwnerId, value2);
							}
						}
					}
				}
			}
			lastRay = (m_lastRay = new RaycastInfo
			{
				Distance = distance,
				Start = translation,
				End = vector3D,
				Hit = null
			});
			return default(MyDetectedEntityInfo);
		}

		bool Sandbox.ModAPI.Ingame.IMyCameraBlock.CanScan(double distance)
		{
			if (BlockDefinition.RaycastDistanceLimit == -1.0)
			{
				return distance <= AvailableScanRange;
			}
			if (distance <= AvailableScanRange)
			{
				return distance <= BlockDefinition.RaycastDistanceLimit;
			}
			return false;
		}

		bool Sandbox.ModAPI.Ingame.IMyCameraBlock.CanScan(double distance, Vector3D direction)
		{
			if (BlockDefinition.RaycastDistanceLimit == -1.0)
			{
				if (distance <= AvailableScanRange)
				{
					return CheckAngleLimits(Vector3D.Normalize(direction));
				}
				return false;
			}
			if (distance <= AvailableScanRange && distance <= BlockDefinition.RaycastDistanceLimit)
			{
				return CheckAngleLimits(Vector3D.Normalize(direction));
			}
			return false;
		}

		bool Sandbox.ModAPI.Ingame.IMyCameraBlock.CanScan(Vector3D target)
		{
			Vector3D directionNormalized = Vector3D.Normalize(target - base.WorldMatrix.Translation);
			double num = Vector3D.Distance(target, base.WorldMatrix.Translation);
			if (BlockDefinition.RaycastDistanceLimit == -1.0)
			{
				if (num <= AvailableScanRange)
				{
					return CheckAngleLimits(directionNormalized);
				}
				return false;
			}
			if (num <= AvailableScanRange && num <= BlockDefinition.RaycastDistanceLimit)
			{
				return CheckAngleLimits(directionNormalized);
			}
			return false;
		}

		int Sandbox.ModAPI.Ingame.IMyCameraBlock.TimeUntilScan(double distance)
		{
			return (int)Math.Max((distance - AvailableScanRange) / (double)BlockDefinition.RaycastTimeMultiplier, 0.0);
		}
	}
}
