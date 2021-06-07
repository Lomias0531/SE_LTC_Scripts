using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Components.Beacon;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks.SafeZone
{
	/// <summary>
	/// Handles operations on safezone created by beacons.
	/// </summary>
	[MyComponentBuilder(typeof(MyObjectBuilder_SafeZoneComponent), true)]
	public class MySafeZoneComponent : MyEntityComponentBase, IMyEventProxy, IMyEventOwner
	{
		protected sealed class OnSafezoneCreateRemove_003C_003ESystem_Boolean : ICallSite<MySafeZoneComponent, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZoneComponent @this, in bool turnOnSafeZone, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSafezoneCreateRemove(turnOnSafeZone);
			}
		}

		protected sealed class StartActivationCoundown_Client_003C_003E : ICallSite<MySafeZoneComponent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZoneComponent @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.StartActivationCoundown_Client();
			}
		}

		protected sealed class OnRadiusChanged_Server_003C_003E : ICallSite<MySafeZoneComponent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZoneComponent @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRadiusChanged_Server();
			}
		}

		protected sealed class SetUpkeepCoundown_Client_003C_003ESystem_Double : ICallSite<MySafeZoneComponent, double, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZoneComponent @this, in double minutes, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetUpkeepCoundown_Client(minutes);
			}
		}

		protected class m_safeZoneEntityId_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType safeZoneEntityId;
				ISyncType result = safeZoneEntityId = new Sync<long, SyncDirection.FromServer>(P_1, P_2);
				((MySafeZoneComponent)P_0).m_safeZoneEntityId = (Sync<long, SyncDirection.FromServer>)safeZoneEntityId;
				return result;
			}
		}

		private static MyDefinitionId DEFINITION_ZONECHIP = new MyDefinitionId(typeof(MyObjectBuilder_Component), "ZoneChip");

		private MySafeZoneBlock m_parentBlock;

		private Sync<long, SyncDirection.FromServer> m_safeZoneEntityId;

		private long m_safeZoneActivationTimeMS;

		private bool m_activating;

		private MyObjectBuilder_SafeZone m_obSafezoneWhenDisabled;

		private TimeSpan m_upkeepTime;

		private TimeSpan m_timeLeft;

		private bool m_processingActivation;

		/// <summary>
		/// Returns current safe zone associated to beacon.
		/// </summary>
		public long SafeZoneEntityId
		{
			get
			{
				return m_safeZoneEntityId;
			}
			private set
			{
				m_safeZoneEntityId.Value = value;
			}
		}

		/// <summary>
		/// True, if waiting for response of creation of safe zone from server.
		/// </summary>
		public bool WaitingResponse
		{
			get;
			private set;
		}

		public override string ComponentTypeDebugString => "MyBeaconSafeZoneManager";

		internal event Action SafeZoneChanged;

		internal void Init(MySafeZoneBlock parentBlock, long safeZoneId)
		{
			m_parentBlock = parentBlock;
			m_parentBlock.SyncType.Append(this);
			if (Sync.IsServer)
			{
				m_safeZoneEntityId.Value = safeZoneId;
			}
			m_safeZoneEntityId.ValueChanged += OnSafeZoneIdChanged;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_SafeZoneComponent myObjectBuilder_SafeZoneComponent = base.Serialize(copy) as MyObjectBuilder_SafeZoneComponent;
			myObjectBuilder_SafeZoneComponent.UpkeepTime = m_upkeepTime.TotalMilliseconds - (double)MySandboxGame.TotalGamePlayTimeInMilliseconds;
			myObjectBuilder_SafeZoneComponent.Activating = m_activating;
			myObjectBuilder_SafeZoneComponent.ActivationTime = m_safeZoneActivationTimeMS - MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_obSafezoneWhenDisabled != null)
			{
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = m_obSafezoneWhenDisabled.Clone() as MyObjectBuilder_SafeZone;
				myObjectBuilder_SafeZone.Factions = ((myObjectBuilder_SafeZone.Factions == null) ? Array.Empty<long>() : myObjectBuilder_SafeZone.Factions);
				myObjectBuilder_SafeZone.Players = ((myObjectBuilder_SafeZone.Players == null) ? Array.Empty<long>() : myObjectBuilder_SafeZone.Players);
				myObjectBuilder_SafeZone.Entities = ((myObjectBuilder_SafeZone.Entities == null) ? Array.Empty<long>() : myObjectBuilder_SafeZone.Entities);
				myObjectBuilder_SafeZoneComponent.SafeZoneOb = myObjectBuilder_SafeZone;
			}
			return myObjectBuilder_SafeZoneComponent;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			MyObjectBuilder_SafeZoneComponent myObjectBuilder_SafeZoneComponent = builder as MyObjectBuilder_SafeZoneComponent;
			m_upkeepTime = TimeSpan.FromMilliseconds((double)MySandboxGame.TotalGamePlayTimeInMilliseconds + myObjectBuilder_SafeZoneComponent.UpkeepTime);
			m_activating = myObjectBuilder_SafeZoneComponent.Activating;
			m_safeZoneActivationTimeMS = MySandboxGame.TotalGamePlayTimeInMilliseconds + myObjectBuilder_SafeZoneComponent.ActivationTime;
			m_timeLeft = m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			m_obSafezoneWhenDisabled = (myObjectBuilder_SafeZoneComponent.SafeZoneOb as MyObjectBuilder_SafeZone);
			if (m_obSafezoneWhenDisabled != null)
			{
				m_obSafezoneWhenDisabled.Factions = ((m_obSafezoneWhenDisabled.Factions == null) ? Array.Empty<long>() : m_obSafezoneWhenDisabled.Factions);
				m_obSafezoneWhenDisabled.Players = ((m_obSafezoneWhenDisabled.Players == null) ? Array.Empty<long>() : m_obSafezoneWhenDisabled.Players);
				m_obSafezoneWhenDisabled.Entities = ((m_obSafezoneWhenDisabled.Entities == null) ? Array.Empty<long>() : m_obSafezoneWhenDisabled.Entities);
				m_obSafezoneWhenDisabled.PositionAndOrientation = null;
			}
			if (m_activating || m_timeLeft > TimeSpan.Zero)
			{
				m_parentBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			base.Deserialize(builder);
		}

		public override bool IsSerialized()
		{
			return true;
		}

		private void OnSafeZoneIdChanged(SyncBase obj)
		{
			WaitingResponse = false;
			this.SafeZoneChanged?.Invoke();
		}

		internal void OnSafezoneCreateRemove_Request(bool turnOnSafeZone)
		{
			WaitingResponse = true;
			MyMultiplayer.RaiseEvent(this, (MySafeZoneComponent x) => x.OnSafezoneCreateRemove, turnOnSafeZone);
		}

		/// <summary>
		/// Server side method to enable/disable safe zone. Server will validate the request.
		/// </summary>
		/// <param name="turnOnSafeZone">Set True if safe zone should be enabled. Otherwise false</param>
		[Event(null, 148)]
		[Reliable]
		[Server]
		private void OnSafezoneCreateRemove(bool turnOnSafeZone)
		{
			if (turnOnSafeZone && SafeZoneEntityId == 0L)
			{
				float radius = GetRadius();
				if (m_obSafezoneWhenDisabled != null)
				{
					radius = m_obSafezoneWhenDisabled.Radius;
				}
				long num = MySessionComponentSafeZones.CreateSafeZone_ImplementationPlayer(m_parentBlock.EntityId, radius, activate: false, MyEventContext.Current.Sender.Value);
				OnSafezoneCreated(num);
				if (m_obSafezoneWhenDisabled != null)
				{
					m_obSafezoneWhenDisabled.EntityId = num;
					m_obSafezoneWhenDisabled.SafeZoneBlockId = m_parentBlock.EntityId;
					MySessionComponentSafeZones.UpdateSafeZone(m_obSafezoneWhenDisabled, sync: true);
				}
			}
			else
			{
				if (SafeZoneEntityId != 0L)
				{
					SaveSafeZoneSettings();
					MySessionComponentSafeZones.DeleteSafeZone_ImplementationPlayer(m_parentBlock.EntityId, SafeZoneEntityId, MyEventContext.Current.Sender.Value);
					m_timeLeft = m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
					m_activating = false;
					m_parentBlock.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
				SafeZoneEntityId = 0L;
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				WaitingResponse = false;
			}
			this.SafeZoneChanged?.Invoke();
		}

		/// <summary>
		/// Event triggered after server reponse on creation of safe zone.
		/// </summary>
		/// <param name="safeZoneEntId"></param>
		internal void OnSafezoneCreated(long safeZoneEntId)
		{
			if (safeZoneEntId == 0L)
			{
				SafeZoneEntityId = 0L;
				WaitingResponse = false;
				this.SafeZoneChanged?.Invoke();
				return;
			}
			SafeZoneEntityId = safeZoneEntId;
			WaitingResponse = false;
			m_processingActivation = true;
			MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity);
			if (!MySessionComponentSafeZones.IsSafeZoneColliding(safeZoneEntId, entity.WorldMatrix, entity.Shape, entity.Radius, entity.Size) && m_parentBlock.IsWorking && (m_timeLeft > TimeSpan.Zero || TryConsumeUpkeep()))
			{
				m_processingActivation = false;
				StartActivationCountdown();
			}
			m_processingActivation = false;
			this.SafeZoneChanged?.Invoke();
		}

		private void StartActivationCountdown()
		{
			m_safeZoneActivationTimeMS = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_parentBlock.Definition.SafeZoneActivationTimeS * 1000;
			m_activating = true;
			m_parentBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZoneComponent x) => x.StartActivationCoundown_Client);
			}
		}

		[Event(null, 236)]
		[Reliable]
		[Broadcast]
		private void StartActivationCoundown_Client()
		{
			StartActivationCountdown();
		}

		internal void SafeZoneRemove_Server()
		{
			if (Sync.IsServer)
			{
				if (MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
				{
					SaveSafeZoneSettings();
					entity.Close();
					m_timeLeft = m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
					m_activating = false;
					m_parentBlock.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
				SafeZoneEntityId = 0L;
			}
		}

		internal void SafeZoneCreate_Server()
		{
			if (Sync.IsServer && SafeZoneEntityId == 0L && m_timeLeft > TimeSpan.Zero)
			{
				float radius = GetRadius();
				if (m_obSafezoneWhenDisabled != null)
				{
					radius = m_obSafezoneWhenDisabled.Radius;
				}
				MyEntity myEntity = MySessionComponentSafeZones.CrateSafeZone(m_parentBlock.PositionComp.WorldMatrix, MySafeZoneShape.Sphere, MySafeZoneAccess.Whitelist, null, null, radius, enable: false, isVisible: true, Color.SkyBlue.ToVector3(), "", m_parentBlock.EntityId);
				OnSafezoneCreated(myEntity.EntityId);
				if (m_obSafezoneWhenDisabled != null)
				{
					m_obSafezoneWhenDisabled.EntityId = myEntity.EntityId;
					m_obSafezoneWhenDisabled.SafeZoneBlockId = m_parentBlock.EntityId;
					MySessionComponentSafeZones.UpdateSafeZone(m_obSafezoneWhenDisabled, sync: true);
				}
			}
		}

		private void SaveSafeZoneSettings()
		{
			if (MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				m_obSafezoneWhenDisabled = (entity.GetObjectBuilder() as MyObjectBuilder_SafeZone);
				m_obSafezoneWhenDisabled.ContainedEntities = Array.Empty<long>();
				m_obSafezoneWhenDisabled.Enabled = false;
				m_obSafezoneWhenDisabled.PositionAndOrientation = null;
			}
		}

		/// <summary>
		/// Gets current radius or default if safe zone not enabled.
		/// </summary>
		/// <returns></returns>
		internal float GetRadius()
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				return entity.Radius;
			}
			return m_parentBlock.Definition.DefaultSafeZoneRadius;
		}

		public bool IsSafeZoneInWorld()
		{
			if ((long)m_safeZoneEntityId <= 0)
			{
				return false;
			}
			return MyEntities.GetEntityById(m_safeZoneEntityId) != null;
		}

		public bool IsSafeZoneEnabled()
		{
			if ((long)m_safeZoneEntityId <= 0)
			{
				return false;
			}
			return (MyEntities.GetEntityById(m_safeZoneEntityId) as MySafeZone)?.Enabled ?? false;
		}

		/// <summary>
		/// Client side set radius of safe zone. Sends message to server which is validated
		/// </summary>
		/// <param name="radius">New radius</param>
		internal void SetRadius(float radius)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				if (MySessionComponentSafeZones.IsSafeZoneColliding(SafeZoneEntityId, entity.WorldMatrix, MySafeZoneShape.Sphere, radius))
				{
					m_parentBlock.RaisePropertiesChanged();
					return;
				}
				MySessionComponentSafeZones.RequestUpdateSafeZoneRadius_Player(m_parentBlock.EntityId, SafeZoneEntityId, radius);
				if (!m_parentBlock.IsWorking)
				{
					return;
				}
				MyMultiplayer.RaiseEvent(this, (MySafeZoneComponent x) => x.OnRadiusChanged_Server);
			}
			this.SafeZoneChanged?.Invoke();
		}

		[Event(null, 360)]
		[Reliable]
		[Server]
		private void OnRadiusChanged_Server()
		{
			if (m_parentBlock.IsWorking)
			{
				this.SafeZoneChanged?.Invoke();
				if (MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity) && !MySessionComponentSafeZones.IsSafeZoneColliding(SafeZoneEntityId, entity.WorldMatrix, entity.Shape, entity.Radius, entity.Size) && !entity.Enabled && !m_activating)
				{
					SetActivate_Server(activate: true);
				}
			}
		}

		internal void SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum sizeEnum, float newValue)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				Vector3 vector = Vector3.Zero;
				switch (sizeEnum)
				{
				case MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.X:
					vector = new Vector3(newValue, entity.Size.Y, entity.Size.Z);
					break;
				case MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Y:
					vector = new Vector3(entity.Size.X, newValue, entity.Size.Z);
					break;
				case MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Z:
					vector = new Vector3(entity.Size.X, entity.Size.Y, newValue);
					break;
				}
				if (MySessionComponentSafeZones.IsSafeZoneColliding(SafeZoneEntityId, entity.WorldMatrix, MySafeZoneShape.Sphere, 0f, vector))
				{
					m_parentBlock.RaisePropertiesChanged();
					return;
				}
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = entity.GetObjectBuilder() as MyObjectBuilder_SafeZone;
				myObjectBuilder_SafeZone.Size = vector;
				MySessionComponentSafeZones.RequestUpdateSafeZone_Player(m_parentBlock.EntityId, myObjectBuilder_SafeZone);
			}
			this.SafeZoneChanged?.Invoke();
		}

		internal Vector3 GetSize()
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				return entity.Size;
			}
			return new Vector3(m_parentBlock.Definition.DefaultSafeZoneRadius);
		}

		internal void SetColor(Color newColor)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = entity.GetObjectBuilder() as MyObjectBuilder_SafeZone;
				myObjectBuilder_SafeZone.ModelColor = newColor.ToVector3();
				MySessionComponentSafeZones.RequestUpdateSafeZone_Player(m_parentBlock.EntityId, myObjectBuilder_SafeZone);
			}
		}

		internal Color GetColor()
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				return entity.ModelColor;
			}
			return Color.SkyBlue;
		}

		/// <summary>
		/// Sets safe zone to be active. (Only on server)
		/// </summary>
		/// <param name="activate">True if to activate, otherwise false.</param>
		internal void SetActivate_Server(bool activate)
		{
			if (Sync.IsServer && !m_processingActivation && SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				m_processingActivation = true;
				if (m_timeLeft <= TimeSpan.Zero && activate && !m_activating && !entity.Enabled && TryConsumeUpkeep())
				{
					StartActivationCountdown();
					m_timeLeft = m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				}
				else if (activate && (m_activating || m_timeLeft > TimeSpan.Zero))
				{
					StartActivationCountdown();
				}
				else if (!activate)
				{
					UpdateSafeZoneEnabled(entity, activate);
					m_timeLeft = m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				}
				m_processingActivation = false;
			}
		}

		private void UpdateSafeZoneEnabled(MySafeZone safeZone, bool activate)
		{
			MyObjectBuilder_SafeZone obj = safeZone.GetObjectBuilder() as MyObjectBuilder_SafeZone;
			obj.Enabled = activate;
			MySessionComponentSafeZones.UpdateSafeZone(obj, sync: true);
			this.SafeZoneChanged?.Invoke();
		}

		/// <summary>
		/// Event triggered after safezone filter button pressed
		/// </summary>
		internal void OnSafeZoneFilterBtnPressed()
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				MyScreenManager.AddScreen(new MyGuiScreenSafeZoneFilter(new Vector2(0.5f, 0.5f), entity));
			}
		}

		/// <summary>
		/// Event triggered when one of checkboxes for safezone settings is pressed
		/// </summary>
		/// <param name="safeZoneAction">Safezone Setting to be changed</param>
		/// <param name="isChecked">Indication if it should be turned on of off</param>
		internal void OnSafeZoneSettingChanged(MySafeZoneAction safeZoneAction, bool isChecked)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = entity.GetObjectBuilder() as MyObjectBuilder_SafeZone;
				if (isChecked)
				{
					myObjectBuilder_SafeZone.AllowedActions |= safeZoneAction;
				}
				else
				{
					myObjectBuilder_SafeZone.AllowedActions &= ~safeZoneAction;
				}
				MySessionComponentSafeZones.RequestUpdateSafeZone_Player(m_parentBlock.EntityId, myObjectBuilder_SafeZone);
			}
		}

		/// <summary>
		/// Gets safezone setting state
		/// </summary>
		/// <param name="safeZoneAction">Setting to check for</param>
		/// <returns>True if safe zone setting/action is enabled/allower</returns>
		internal bool GetSafeZoneSetting(MySafeZoneAction safeZoneAction)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				return entity.AllowedActions.HasFlag(safeZoneAction);
			}
			return false;
		}

		/// <summary>
		/// Event triggered when shape of the safe zone is changed.
		/// </summary>
		/// <param name="newShape">New shape to set.</param>
		internal void OnSafeZoneShapeChanged(MySafeZoneShape newShape)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = entity.GetObjectBuilder() as MyObjectBuilder_SafeZone;
				myObjectBuilder_SafeZone.Shape = newShape;
				MySessionComponentSafeZones.RequestUpdateSafeZone_Player(m_parentBlock.EntityId, myObjectBuilder_SafeZone);
			}
			this.SafeZoneChanged?.Invoke();
		}

		/// <summary>
		/// Gets safezone current shape.
		/// </summary>
		/// <returns>Safe zone shape.</returns>
		internal long GetSafeZoneShape()
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				return (long)entity.Shape;
			}
			return 0L;
		}

		/// <summary>
		/// Gets current safezone texture.
		/// </summary>
		/// <returns></returns>
		internal long GetTexture()
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				return (int)entity.CurrentTexture;
			}
			return 0L;
		}

		/// <summary>
		/// Sets safe zone texture;
		/// </summary>
		/// <param name="texture"></param>
		internal void SetTexture(MyStringHash texture)
		{
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = entity.GetObjectBuilder() as MyObjectBuilder_SafeZone;
				myObjectBuilder_SafeZone.Texture = texture.String;
				MySessionComponentSafeZones.RequestUpdateSafeZone_Player(m_parentBlock.EntityId, myObjectBuilder_SafeZone);
			}
		}

		internal float GetPowerDrain()
		{
			float result = 1E-06f;
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity) && entity.Enabled)
			{
				float num = m_parentBlock.Definition.MaxSafeZonePowerDrainkW - m_parentBlock.Definition.MinSafeZonePowerDrainkW;
				float num2 = m_parentBlock.Definition.MaxSafeZoneRadius - m_parentBlock.Definition.MinSafeZoneRadius;
				if ((float)GetSafeZoneShape() == 0f)
				{
					result = (GetRadius() - m_parentBlock.Definition.MinSafeZoneRadius) / num2 * num / 1000f;
				}
				else
				{
					num2 = m_parentBlock.Definition.MaxSafeZoneRadius * 2f - m_parentBlock.Definition.MinSafeZoneRadius;
					Vector3 size = GetSize();
					float num3 = (size.X - m_parentBlock.Definition.MinSafeZoneRadius) / num2 / 3f;
					float num4 = (size.Y - m_parentBlock.Definition.MinSafeZoneRadius) / num2 / 3f;
					float num5 = (size.Z - m_parentBlock.Definition.MinSafeZoneRadius) / num2 / 3f;
					result = (num3 + num4 + num5) * num / 1000f;
				}
				return m_parentBlock.Definition.MinSafeZonePowerDrainkW / 1000f + result;
			}
			return result;
		}

		/// <summary>
		/// Updates safezone.
		/// </summary>
		/// <returns>True if safezone is activating.</returns>
		internal bool Update()
		{
			long num = m_safeZoneActivationTimeMS - MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out MySafeZone entity))
			{
				if (m_activating && num < 0)
				{
					m_activating = false;
					num = 0L;
					if (entity != null && Sync.IsServer)
					{
						if (m_timeLeft > TimeSpan.Zero)
						{
							m_upkeepTime = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + m_timeLeft;
							MyMultiplayer.RaiseEvent(this, (MySafeZoneComponent x) => x.SetUpkeepCoundown_Client, m_timeLeft.TotalMinutes);
							m_timeLeft = TimeSpan.Zero;
							UpdateSafeZoneEnabled(entity, activate: true);
						}
						else
						{
							UpdateSafeZoneEnabled(entity, activate: true);
						}
					}
				}
				else if (entity.Enabled && m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) < TimeSpan.Zero && Sync.IsServer)
				{
					m_processingActivation = true;
					if (!TryConsumeUpkeep())
					{
						SafeZoneRemove_Server();
					}
					m_processingActivation = false;
				}
				if (!m_activating && !entity.Enabled)
				{
					return m_upkeepTime > TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				}
				return true;
			}
			return m_activating;
		}

		/// <summary>
		/// Tries to consume zone chip for next time frame of the safe zone.
		/// </summary>
		/// <returns>If true, zone chip is consumed and function returns true. Otherwise false.</returns>
		private bool TryConsumeUpkeep()
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("Trying to consume zone chips on client. This is not legal");
				return false;
			}
			if (!MyFakes.ENABLE_ZONE_CHIP_REQ)
			{
				return true;
			}
			MyInventory inventory = m_parentBlock.GetInventory();
			if (inventory == null)
			{
				return false;
			}
			if ((int)inventory.GetItemAmount(DEFINITION_ZONECHIP) >= m_parentBlock.Definition.SafeZoneUpkeep)
			{
				inventory.RemoveItemsOfType((int)m_parentBlock.Definition.SafeZoneUpkeep, DEFINITION_ZONECHIP);
			}
			else
			{
				if ((int)m_parentBlock.CubeGrid.GridSystems.ConveyorSystem.PullItem(DEFINITION_ZONECHIP, (int)m_parentBlock.Definition.SafeZoneUpkeep, m_parentBlock, m_parentBlock.GetInventory(), remove: false, MyFakes.CONV_PULL_CACL_IMMIDIATLY_STORE_SAFEZONE) < m_parentBlock.Definition.SafeZoneUpkeep)
				{
					return false;
				}
				inventory.RemoveItemsOfType((int)m_parentBlock.Definition.SafeZoneUpkeep, DEFINITION_ZONECHIP);
			}
			m_upkeepTime = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes(m_parentBlock.Definition.SafeZoneUpkeepTimeM);
			MyMultiplayer.RaiseEvent(this, (Func<MySafeZoneComponent, Action<double>>)((MySafeZoneComponent x) => x.SetUpkeepCoundown_Client), (double)m_parentBlock.Definition.SafeZoneUpkeepTimeM, default(EndpointId));
			return true;
		}

		[Event(null, 743)]
		[Reliable]
		[Broadcast]
		private void SetUpkeepCoundown_Client(double minutes)
		{
			m_upkeepTime = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes(minutes);
			m_parentBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		/// <summary>
		/// Sets text used in Detailed info right panel of Terminal Display.
		/// </summary>
		/// <param name="sBuilderToSet">String Builder to set info on.</param>
		internal void SetTextInfo(StringBuilder sBuilderToSet)
		{
			sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Desc));
			if (WaitingResponse)
			{
				sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Initializing));
			}
			else if (SafeZoneEntityId == 0L)
			{
				sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Disabled));
			}
			else if (SafeZoneEntityId >= 0)
			{
				bool flag = true;
				if (m_parentBlock.IsWorking)
				{
					MySafeZone entity;
					if (m_activating)
					{
						long num = (m_safeZoneActivationTimeMS - MySandboxGame.TotalGamePlayTimeInMilliseconds) / 1000;
						if (num < 0)
						{
							num = 0L;
						}
						sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Initializing));
						sBuilderToSet.Append(" " + num);
						flag = false;
					}
					else if (SafeZoneEntityId != 0L && MyEntities.TryGetEntityById(SafeZoneEntityId, out entity))
					{
						if (entity.Enabled)
						{
							sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Enabled));
							if (MyFakes.ENABLE_ZONE_CHIP_REQ)
							{
								sBuilderToSet.AppendLine();
								StringBuilder otherStringBuilder = MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_NextUnkeepIn);
								sBuilderToSet.AppendStringBuilder(otherStringBuilder);
								TimeSpan t = m_upkeepTime - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
								if (t < TimeSpan.Zero)
								{
									t = TimeSpan.Zero;
								}
								sBuilderToSet.Append(t.ToString("hh\\:mm\\:ss"));
							}
							flag = false;
						}
						else if (MySessionComponentSafeZones.IsSafeZoneColliding(SafeZoneEntityId, entity.WorldMatrix, entity.Shape, entity.Radius, entity.Size))
						{
							sBuilderToSet.Append((object)MyTexts.Get(MySpaceTexts.SafeZoneBlock_Safezone_Collision));
							flag = false;
						}
					}
				}
				if (flag)
				{
					sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Inactive));
				}
			}
			MyInventory inventory = m_parentBlock.GetInventory();
			if (inventory != null)
			{
				sBuilderToSet.AppendLine();
				MyFixedPoint itemAmount = inventory.GetItemAmount(DEFINITION_ZONECHIP);
				StringBuilder otherStringBuilder2 = MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_ZoneChips);
				sBuilderToSet.AppendStringBuilder(otherStringBuilder2);
				sBuilderToSet.Append(itemAmount.ToString());
			}
		}
	}
}
