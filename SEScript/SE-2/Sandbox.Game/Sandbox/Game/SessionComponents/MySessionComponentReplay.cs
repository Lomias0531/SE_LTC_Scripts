using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof(MyObjectBuilder_SessionComponentReplay), null)]
	public class MySessionComponentReplay : MySessionComponentBase
	{
		public delegate void ActionRef<T>(ref T item);

		public static MySessionComponentReplay Static;

		private static Dictionary<long, Dictionary<int, PerFrameData>> m_recordedEntities = new Dictionary<long, Dictionary<int, PerFrameData>>();

		private ulong m_replayingStart = ulong.MaxValue;

		private ulong m_recordingStart = ulong.MaxValue;

		public bool HasRecordedData => m_recordedEntities.Count > 0;

		public bool IsRecording => m_recordingStart != ulong.MaxValue;

		public bool IsReplaying => m_replayingStart != ulong.MaxValue;

		public bool HasAnyData => m_recordedEntities.Count > 0;

		public MySessionComponentReplay()
		{
			Static = this;
		}

		public void DeleteRecordings()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings_Confirm), MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings), null, null, null, null, OnDeleteRecordingsClicked));
		}

		private void OnDeleteRecordingsClicked(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_recordedEntities.Clear();
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_SessionComponentReplay myObjectBuilder_SessionComponentReplay = sessionComponent as MyObjectBuilder_SessionComponentReplay;
			if (myObjectBuilder_SessionComponentReplay.EntityReplayData != null)
			{
				foreach (PerEntityData entityReplayDatum in myObjectBuilder_SessionComponentReplay.EntityReplayData)
				{
					Dictionary<int, PerFrameData> value = new Dictionary<int, PerFrameData>(entityReplayDatum.Data.Dictionary);
					if (!m_recordedEntities.ContainsKey(entityReplayDatum.EntityId))
					{
						m_recordedEntities.Add(entityReplayDatum.EntityId, value);
					}
				}
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentReplay myObjectBuilder_SessionComponentReplay = new MyObjectBuilder_SessionComponentReplay();
			if (m_recordedEntities.Count > 0)
			{
				myObjectBuilder_SessionComponentReplay.EntityReplayData = new MySerializableList<PerEntityData>();
				{
					foreach (KeyValuePair<long, Dictionary<int, PerFrameData>> recordedEntity in m_recordedEntities)
					{
						PerEntityData perEntityData = new PerEntityData();
						perEntityData.EntityId = recordedEntity.Key;
						perEntityData.Data = new SerializableDictionary<int, PerFrameData>();
						foreach (KeyValuePair<int, PerFrameData> item in recordedEntity.Value)
						{
							perEntityData.Data[item.Key] = item.Value;
						}
						myObjectBuilder_SessionComponentReplay.EntityReplayData.Add(perEntityData);
					}
					return myObjectBuilder_SessionComponentReplay;
				}
			}
			return myObjectBuilder_SessionComponentReplay;
		}

		public void StartRecording()
		{
			m_recordedEntities.Remove(MySession.Static.ControlledEntity.Entity.GetTopMostParent().EntityId);
			m_recordingStart = MySandboxGame.Static.SimulationFrameCounter;
		}

		public void StopRecording()
		{
			m_recordingStart = ulong.MaxValue;
		}

		public void StartReplay()
		{
			m_replayingStart = MySandboxGame.Static.SimulationFrameCounter;
		}

		public void StopReplay()
		{
			m_replayingStart = ulong.MaxValue;
		}

		public bool IsEntityBeingRecorded(long entityId)
		{
			if (IsRecording && MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null)
			{
				return MySession.Static.ControlledEntity.Entity.GetTopMostParent().EntityId == entityId;
			}
			return false;
		}

		public bool IsEntityBeingReplayed(long entityId)
		{
			if (IsReplaying && m_recordedEntities.ContainsKey(entityId))
			{
				return !IsEntityBeingRecorded(entityId);
			}
			return false;
		}

		public bool IsEntityBeingReplayed(long entityId, out PerFrameData perFrameData)
		{
			if (IsReplaying && IsEntityBeingReplayed(entityId) && m_recordedEntities.TryGetValue(entityId, out Dictionary<int, PerFrameData> value))
			{
				ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
				ulong replayingStart = m_replayingStart;
				int key = (int)(simulationFrameCounter - replayingStart);
				if (value.TryGetValue(key, out perFrameData))
				{
					return true;
				}
			}
			perFrameData = default(PerFrameData);
			return false;
		}

		public bool HasEntityReplayData(long entityId)
		{
			return m_recordedEntities.ContainsKey(entityId);
		}

		public void ProvideEntityRecordData(long entityId, PerFrameData data)
		{
			if (!m_recordedEntities.TryGetValue(entityId, out Dictionary<int, PerFrameData> value))
			{
				value = new Dictionary<int, PerFrameData>();
				m_recordedEntities[entityId] = value;
			}
			ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
			ulong recordingStart = m_recordingStart;
			int key = (int)(simulationFrameCounter - recordingStart);
			if (value.TryGetValue(key, out PerFrameData value2))
			{
				if (data.MovementData.HasValue)
				{
					value2.MovementData = data.MovementData;
				}
				if (data.SwitchWeaponData.HasValue)
				{
					SerializableDefinitionId? weaponDefinition = data.SwitchWeaponData.Value.WeaponDefinition;
					if (weaponDefinition.HasValue && !MyObjectBuilderType.IsValidTypeName(weaponDefinition?.TypeIdString))
					{
						data.SwitchWeaponData = default(SwitchWeaponData);
					}
					value2.SwitchWeaponData = data.SwitchWeaponData;
				}
				if (data.ShootData.HasValue)
				{
					value2.ShootData = data.ShootData;
				}
				if (data.AnimationData.HasValue)
				{
					if (value2.AnimationData.HasValue)
					{
						AnimationData animationData = default(AnimationData);
						animationData.Animation = value2.AnimationData.Value.Animation;
						animationData.Animation2 = data.AnimationData.Value.Animation;
						AnimationData value3 = animationData;
						value2.AnimationData = value3;
					}
					else
					{
						value2.AnimationData = data.AnimationData;
					}
				}
				if (data.ControlSwitchesData.HasValue)
				{
					value2.ControlSwitchesData = data.ControlSwitchesData;
				}
				if (data.UseData.HasValue)
				{
					value2.UseData = data.UseData;
				}
			}
			else
			{
				value2 = data;
			}
			value[key] = value2;
		}
	}
}
