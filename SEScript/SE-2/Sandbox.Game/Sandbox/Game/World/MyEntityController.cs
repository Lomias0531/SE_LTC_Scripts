using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;

namespace Sandbox.Game.World
{
	public class MyEntityController : IMyEntityController
	{
		private Action<MyEntity> m_controlledEntityClosing;

		public Sandbox.Game.Entities.IMyControllableEntity ControlledEntity
		{
			get;
			protected set;
		}

		public MyPlayer Player
		{
			get;
			private set;
		}

		VRage.Game.ModAPI.Interfaces.IMyControllableEntity IMyEntityController.ControlledEntity => ControlledEntity;

		public event Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity> ControlledEntityChanged;

		event Action<VRage.Game.ModAPI.Interfaces.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity> IMyEntityController.ControlledEntityChanged
		{
			add
			{
				ControlledEntityChanged += GetDelegate(value);
			}
			remove
			{
				ControlledEntityChanged -= GetDelegate(value);
			}
		}

		public MyEntityController(MyPlayer parent)
		{
			Player = parent;
			m_controlledEntityClosing = ControlledEntity_OnClosing;
		}

		public void SaveCamera()
		{
			if (ControlledEntity != null && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				bool isLocalCharacter = ControlledEntity is MyCharacter && MySession.Static.LocalCharacter == ControlledEntity;
				if (!(ControlledEntity is MyCharacter) || MySession.Static.LocalCharacter == ControlledEntity)
				{
					MyEntityCameraSettings cameraEntitySettings = ControlledEntity.GetCameraEntitySettings();
					float headLocalXAngle = ControlledEntity.HeadLocalXAngle;
					float headLocalYAngle = ControlledEntity.HeadLocalYAngle;
					bool isFirstPerson = cameraEntitySettings?.IsFirstPerson ?? (MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator);
					MySession.Static.Cameras.SaveEntityCameraSettings(Player.Id, ControlledEntity.Entity.EntityId, isFirstPerson, MyThirdPersonSpectator.Static.GetViewerDistance(), isLocalCharacter, headLocalXAngle, headLocalYAngle);
				}
			}
		}

		public void TakeControl(Sandbox.Game.Entities.IMyControllableEntity entity)
		{
			if (ControlledEntity != entity && (entity == null || entity.ControllerInfo.Controller == null))
			{
				Sandbox.Game.Entities.IMyControllableEntity controlledEntity = ControlledEntity;
				SaveCamera();
				if (ControlledEntity != null)
				{
					ControlledEntity.Entity.OnClosing -= m_controlledEntityClosing;
					ControlledEntity.ControllerInfo.Controller = null;
				}
				ControlledEntity = entity;
				if (entity != null)
				{
					ControlledEntity.Entity.OnClosing += m_controlledEntityClosing;
					ControlledEntity.ControllerInfo.Controller = this;
					SetCamera();
				}
				if (controlledEntity != entity)
				{
					RaiseControlledEntityChanged(controlledEntity, entity);
				}
			}
		}

		private void RaiseControlledEntityChanged(Sandbox.Game.Entities.IMyControllableEntity old, Sandbox.Game.Entities.IMyControllableEntity entity)
		{
			this.ControlledEntityChanged?.Invoke(old, entity);
		}

		private void ControlledEntity_OnClosing(MyEntity entity)
		{
			if (ControlledEntity != null)
			{
				TakeControl(null);
			}
		}

		public void SetCamera()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && ControlledEntity.Entity is IMyCameraController)
			{
				MySession.Static.SetEntityCameraPosition(Player.Id, ControlledEntity.Entity);
			}
		}

		void IMyEntityController.TakeControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity entity)
		{
			if (entity is Sandbox.Game.Entities.IMyControllableEntity)
			{
				TakeControl(entity as Sandbox.Game.Entities.IMyControllableEntity);
			}
		}

		private Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity> GetDelegate(Action<VRage.Game.ModAPI.Interfaces.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity> value)
		{
			return (Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity>)Delegate.CreateDelegate(typeof(Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity>), value.Target, value.Method);
		}
	}
}
