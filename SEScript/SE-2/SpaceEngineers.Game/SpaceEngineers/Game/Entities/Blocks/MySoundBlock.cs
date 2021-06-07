using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SoundBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMySoundBlock),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock)
	})]
	public class MySoundBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMySoundBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock
	{
		protected sealed class PlaySound_003C_003ESystem_Boolean : ICallSite<MySoundBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySoundBlock @this, in bool isLoopable, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PlaySound(isLoopable);
			}
		}

		protected sealed class StopSound_003C_003E : ICallSite<MySoundBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySoundBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.StopSound();
			}
		}

		protected class m_soundRadius_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType soundRadius;
				ISyncType result = soundRadius = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MySoundBlock)P_0).m_soundRadius = (Sync<float, SyncDirection.BothWays>)soundRadius;
				return result;
			}
		}

		protected class m_volume_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType volume;
				ISyncType result = volume = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MySoundBlock)P_0).m_volume = (Sync<float, SyncDirection.BothWays>)volume;
				return result;
			}
		}

		protected class m_cueIdString_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType cueIdString;
				ISyncType result = cueIdString = new Sync<string, SyncDirection.BothWays>(P_1, P_2);
				((MySoundBlock)P_0).m_cueIdString = (Sync<string, SyncDirection.BothWays>)cueIdString;
				return result;
			}
		}

		protected class m_loopPeriod_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType loopPeriod;
				ISyncType result = loopPeriod = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MySoundBlock)P_0).m_loopPeriod = (Sync<float, SyncDirection.BothWays>)loopPeriod;
				return result;
			}
		}

		private static StringBuilder m_helperSB = new StringBuilder();

		protected readonly Sync<float, SyncDirection.BothWays> m_soundRadius;

		protected readonly Sync<float, SyncDirection.BothWays> m_volume;

		protected readonly Sync<string, SyncDirection.BothWays> m_cueIdString;

		private readonly Sync<float, SyncDirection.BothWays> m_loopPeriod;

		protected new MyEntity3DSoundEmitter m_soundEmitter;

		private bool m_willStartSound;

		protected bool m_isPlaying;

		private bool m_isLooping;

		private long m_soundStartTime;

		private string m_playingSoundName;

		private static MyTerminalControlButton<MySoundBlock> m_playButton;

		private static MyTerminalControlButton<MySoundBlock> m_stopButton;

		private static MyTerminalControlSlider<MySoundBlock> m_loopableTimeSlider;

		private new MySoundBlockDefinition BlockDefinition => (MySoundBlockDefinition)base.BlockDefinition;

		public float Range
		{
			get
			{
				return m_soundRadius;
			}
			set
			{
				if ((float)m_soundRadius != value)
				{
					m_soundRadius.Value = value;
				}
			}
		}

		public float Volume
		{
			get
			{
				return m_volume;
			}
			set
			{
				if ((float)m_volume != value)
				{
					m_volume.Value = value;
				}
			}
		}

		public float LoopPeriod
		{
			get
			{
				return m_loopPeriod;
			}
			set
			{
				m_loopPeriod.Value = value;
			}
		}

		public bool IsLoopablePlaying
		{
			get
			{
				if (m_isPlaying)
				{
					return m_isLooping;
				}
				return false;
			}
		}

		public bool IsLoopPeriodUnderThreshold => (float)m_loopPeriod < (float)BlockDefinition.LoopUpdateThreshold;

		public bool IsSoundSelected => !string.IsNullOrEmpty(m_cueIdString);

		float SpaceEngineers.Game.ModAPI.IMySoundBlock.Volume
		{
			get
			{
				return Volume;
			}
			set
			{
				Volume = value;
			}
		}

		float SpaceEngineers.Game.ModAPI.IMySoundBlock.Range
		{
			get
			{
				return Range;
			}
			set
			{
				Range = value;
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Volume
		{
			get
			{
				return Volume;
			}
			set
			{
				Volume = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Range
		{
			get
			{
				return Range;
			}
			set
			{
				Range = MathHelper.Clamp(value, BlockDefinition.MinRange, BlockDefinition.MaxRange);
			}
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.IsSoundSelected => IsSoundSelected;

		float SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.LoopPeriod
		{
			get
			{
				return LoopPeriod;
			}
			set
			{
				LoopPeriod = value;
			}
		}

		string SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.SelectedSound
		{
			get
			{
				return m_cueIdString;
			}
			set
			{
				SelectSound(GetSoundQueue(value).ToString(), sync: true);
			}
		}

		public MySoundBlock()
		{
			CreateTerminalControls();
			m_volume.ValueChanged += delegate
			{
				VolumeChanged();
			};
			m_soundRadius.ValueChanged += delegate
			{
				RadiusChanged();
			};
			m_cueIdString.ValueChanged += delegate
			{
				SelectionChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySoundBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MySoundBlock> myTerminalControlSlider = new MyTerminalControlSlider<MySoundBlock>("VolumeSlider", MySpaceTexts.BlockPropertyTitle_SoundBlockVolume, MySpaceTexts.BlockPropertyDescription_SoundBlockVolume);
				myTerminalControlSlider.SetLimits(0f, 100f);
				myTerminalControlSlider.DefaultValue = 100f;
				myTerminalControlSlider.Getter = ((MySoundBlock x) => x.Volume * 100f);
				myTerminalControlSlider.Setter = delegate(MySoundBlock x, float v)
				{
					x.Volume = v * 0.01f;
				};
				myTerminalControlSlider.Writer = delegate(MySoundBlock x, StringBuilder result)
				{
					result.AppendInt32((int)((double)x.Volume * 100.0)).Append(" %");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MySoundBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MySoundBlock>("RangeSlider", MySpaceTexts.BlockPropertyTitle_SoundBlockRange, MySpaceTexts.BlockPropertyDescription_SoundBlockRange);
				myTerminalControlSlider2.SetLimits((MySoundBlock x) => x.BlockDefinition.MinRange, (MySoundBlock x) => x.BlockDefinition.MaxRange);
				myTerminalControlSlider2.DefaultValue = 50f;
				myTerminalControlSlider2.Getter = ((MySoundBlock x) => x.Range);
				myTerminalControlSlider2.Setter = delegate(MySoundBlock x, float v)
				{
					x.Range = v;
				};
				myTerminalControlSlider2.Writer = delegate(MySoundBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.Range).Append(" m");
				};
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				m_playButton = new MyTerminalControlButton<MySoundBlock>("PlaySound", MySpaceTexts.BlockPropertyTitle_SoundBlockPlay, MySpaceTexts.Blank, delegate(MySoundBlock x)
				{
					x.RequestPlaySound();
				});
				m_playButton.Enabled = ((MySoundBlock x) => x.IsSoundSelected);
				m_playButton.EnableAction();
				MyTerminalControlFactory.AddControl(m_playButton);
				m_stopButton = new MyTerminalControlButton<MySoundBlock>("StopSound", MySpaceTexts.BlockPropertyTitle_SoundBlockStop, MySpaceTexts.Blank, delegate(MySoundBlock x)
				{
					x.RequestStopSound();
				});
				m_stopButton.Enabled = ((MySoundBlock x) => x.IsSoundSelected);
				m_stopButton.EnableAction();
				MyTerminalControlFactory.AddControl(m_stopButton);
				m_loopableTimeSlider = new MyTerminalControlSlider<MySoundBlock>("LoopableSlider", MySpaceTexts.BlockPropertyTitle_SoundBlockLoopTime, MySpaceTexts.Blank);
				m_loopableTimeSlider.DefaultValue = 1f;
				m_loopableTimeSlider.Getter = ((MySoundBlock x) => x.LoopPeriod);
				m_loopableTimeSlider.Setter = delegate(MySoundBlock x, float f)
				{
					x.LoopPeriod = f;
				};
				m_loopableTimeSlider.Writer = delegate(MySoundBlock x, StringBuilder result)
				{
					MyValueFormatter.AppendTimeInBestUnit(x.LoopPeriod, result);
				};
				m_loopableTimeSlider.Enabled = ((MySoundBlock x) => x.IsSelectedSoundLoopable());
				m_loopableTimeSlider.Normalizer = ((MySoundBlock x, float f) => x.NormalizeLoopPeriod(f));
				m_loopableTimeSlider.Denormalizer = ((MySoundBlock x, float f) => x.DenormalizeLoopPeriod(f));
				m_loopableTimeSlider.EnableActions();
				MyTerminalControlFactory.AddControl(m_loopableTimeSlider);
				MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MySoundBlock>("SoundsList", MySpaceTexts.BlockPropertyTitle_SoundBlockSoundList, MySpaceTexts.Blank)
				{
					ListContent = delegate(MySoundBlock x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
					{
						x.FillListContent(list1, list2);
					},
					ItemSelected = delegate(MySoundBlock x, List<MyGuiControlListbox.Item> y)
					{
						x.SelectSound(y, sync: true);
					}
				});
			}
		}

		private void SelectionChanged()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_loopableTimeSlider.UpdateVisual();
				m_playButton.UpdateVisual();
				m_stopButton.UpdateVisual();
			}
		}

		private void RadiusChanged()
		{
			m_soundEmitter.CustomMaxDistance = m_soundRadius;
		}

		private void VolumeChanged()
		{
			m_soundEmitter.CustomVolume = m_volume;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MySoundBlockDefinition blockDefinition = BlockDefinition;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 0.0002f, UpdateRequiredPowerInput);
			myResourceSinkComponent.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: false, 0f);
			m_soundEmitter.Force3D = true;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_SoundBlock myObjectBuilder_SoundBlock = (MyObjectBuilder_SoundBlock)objectBuilder;
			m_isPlaying = myObjectBuilder_SoundBlock.IsPlaying;
			m_isLooping = myObjectBuilder_SoundBlock.IsLoopableSound;
			m_volume.SetLocalValue(MathHelper.Clamp(myObjectBuilder_SoundBlock.Volume, 0f, 1f));
			m_soundRadius.SetLocalValue(MathHelper.Clamp(myObjectBuilder_SoundBlock.Range, blockDefinition.MinRange, blockDefinition.MaxRange));
			m_loopPeriod.SetLocalValue(MathHelper.Clamp(myObjectBuilder_SoundBlock.LoopPeriod, 0f, blockDefinition.MaxLoopPeriod));
			if (myObjectBuilder_SoundBlock.IsPlaying)
			{
				m_willStartSound = true;
				m_playingSoundName = myObjectBuilder_SoundBlock.CueName;
				m_soundStartTime = Stopwatch.GetTimestamp() - (long)myObjectBuilder_SoundBlock.ElapsedSoundSeconds * Stopwatch.Frequency;
			}
			InitCue(myObjectBuilder_SoundBlock.CueName);
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
		}

		private void InitCue(string cueName)
		{
			if (string.IsNullOrEmpty(cueName))
			{
				m_cueIdString.SetLocalValue("");
				return;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_cueIdString.SetLocalValue(cueName);
				return;
			}
			MySoundPair mySoundPair = new MySoundPair(cueName);
			MySoundCategoryDefinition.SoundDescription soundDescription = null;
			foreach (MySoundCategoryDefinition soundCategoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
			{
				foreach (MySoundCategoryDefinition.SoundDescription sound in soundCategoryDefinition.Sounds)
				{
					if (mySoundPair.SoundId.ToString().EndsWith(sound.SoundId.ToString()))
					{
						soundDescription = sound;
					}
				}
			}
			if (soundDescription != null)
			{
				m_cueIdString.SetLocalValue(soundDescription.SoundId);
			}
			else
			{
				m_cueIdString.SetLocalValue("");
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SoundBlock myObjectBuilder_SoundBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_SoundBlock;
			myObjectBuilder_SoundBlock.Volume = Volume;
			myObjectBuilder_SoundBlock.Range = Range;
			myObjectBuilder_SoundBlock.LoopPeriod = LoopPeriod;
			myObjectBuilder_SoundBlock.IsLoopableSound = m_isLooping;
			myObjectBuilder_SoundBlock.ElapsedSoundSeconds = (float)((double)(Stopwatch.GetTimestamp() - m_soundStartTime) / (double)Stopwatch.Frequency);
			if (m_isPlaying && (m_soundEmitter.IsPlaying || (m_isLooping && LoopPeriod > myObjectBuilder_SoundBlock.ElapsedSoundSeconds)))
			{
				myObjectBuilder_SoundBlock.IsPlaying = true;
				myObjectBuilder_SoundBlock.CueName = m_playingSoundName;
			}
			else
			{
				myObjectBuilder_SoundBlock.IsPlaying = false;
				myObjectBuilder_SoundBlock.ElapsedSoundSeconds = 0f;
				myObjectBuilder_SoundBlock.CueName = m_cueIdString.Value;
			}
			return myObjectBuilder_SoundBlock;
		}

		public void RequestPlaySound()
		{
			MyMultiplayer.RaiseEvent(this, (MySoundBlock x) => x.PlaySound, IsSelectedSoundLoopable());
		}

		[Event(null, 319)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void PlaySound(bool isLoopable)
		{
			PlaySoundInternal(isLoopable);
		}

		protected void PlaySoundInternal(bool isLoopable)
		{
			if (!base.Enabled || !base.IsWorking)
			{
				return;
			}
			string value = m_cueIdString.Value;
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySoundPair cueId = new MySoundPair(value);
				if (Sync.IsServer)
				{
					RequestStopSound();
				}
				StopSound();
				if (isLoopable)
				{
					PlayLoopableSound(cueId);
				}
				else
				{
					PlaySingleSound(cueId);
				}
			}
			m_isPlaying = true;
			m_isLooping = isLoopable;
			m_playingSoundName = value;
			m_soundStartTime = Stopwatch.GetTimestamp();
		}

		private void PlayLoopableSound(MySoundPair cueId)
		{
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			m_soundEmitter.PlaySound(cueId, stopPrevious: true);
		}

		protected void PlaySingleSound(MySoundPair cueId)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_soundEmitter.PlaySingleSound(cueId, stopPrevious: true);
		}

		protected virtual void SelectSound(List<MyGuiControlListbox.Item> cuesId, bool sync)
		{
			SelectSound(cuesId[0].UserData.ToString(), sync);
		}

		public void SelectSound(string cueId, bool sync)
		{
			m_cueIdString.Value = cueId;
		}

		public void RequestStopSound()
		{
			MyMultiplayer.RaiseEvent(this, (MySoundBlock x) => x.StopSound);
			m_willStartSound = false;
		}

		[Event(null, 390)]
		[Reliable]
		[Server]
		[BroadcastExcept]
		public void StopSound()
		{
			StopSoundInternal(force: true);
		}

		protected void StopSoundInternal(bool force = false)
		{
			m_isPlaying = false;
			m_soundEmitter.StopSound(force);
			if (!base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
			base.DetailedInfo.Clear();
			RaisePropertiesChanged();
		}

		protected virtual void FillListContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			foreach (MySoundCategoryDefinition soundCategoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
			{
				if (soundCategoryDefinition.Public)
				{
					foreach (MySoundCategoryDefinition.SoundDescription sound in soundCategoryDefinition.Sounds)
					{
						m_helperSB.Clear().Append(sound.SoundText);
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(m_helperSB, null, null, sound.SoundId);
						listBoxContent.Add(item);
						if (sound.SoundId.Equals(m_cueIdString))
						{
							listBoxSelectedItems.Add(item);
						}
					}
				}
			}
		}

		private float NormalizeLoopPeriod(float value)
		{
			if (value <= 1f)
			{
				return 0f;
			}
			return MathHelper.InterpLogInv(value, 1f, BlockDefinition.MaxLoopPeriod);
		}

		private float DenormalizeLoopPeriod(float value)
		{
			if (value == 0f)
			{
				return 1f;
			}
			return MathHelper.InterpLog(value, 1f, BlockDefinition.MaxLoopPeriod);
		}

		public override void UpdateBeforeSimulation()
		{
			if (base.IsWorking)
			{
				base.UpdateBeforeSimulation();
				if (IsLoopablePlaying)
				{
					UpdateLoopableSoundEmitter();
				}
			}
		}

		public override void UpdateSoundEmitters()
		{
			if (base.IsWorking)
			{
				if (MyAudio.Static.CanPlay && !Sandbox.Engine.Platform.Game.IsDedicated && m_isPlaying && !m_soundEmitter.IsPlaying)
				{
					RequestPlaySound();
				}
				if (m_soundEmitter != null)
				{
					m_soundEmitter.Update();
				}
			}
		}

		/// !Never runs on DS!
		private void UpdateLoopableSoundEmitter()
		{
			double num = (double)(Stopwatch.GetTimestamp() - m_soundStartTime) / (double)Stopwatch.Frequency;
			if (num > (double)(float)m_loopPeriod)
			{
				StopSoundInternal(force: true);
				return;
			}
			base.DetailedInfo.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_LoopTimer));
			MyValueFormatter.AppendTimeInBestUnit(Math.Max(0f, (float)((double)(float)m_loopPeriod - num)), base.DetailedInfo);
			RaisePropertiesChanged();
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private float UpdateRequiredPowerInput()
		{
			if (base.Enabled && base.IsFunctional)
			{
				return 0.0002f;
			}
			return 0f;
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			m_soundEmitter.Sound?.Resume();
			if (!Sandbox.Engine.Platform.Game.IsDedicated && m_willStartSound && base.CubeGrid.Physics != null)
			{
				if (Sync.IsServer)
				{
					RequestStopSound();
				}
				else
				{
					StopSound();
				}
				MySoundPair cueId = new MySoundPair(m_playingSoundName);
				if (m_isLooping)
				{
					PlayLoopableSound(cueId);
				}
				else
				{
					PlaySingleSound(cueId);
				}
				m_willStartSound = false;
			}
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			m_soundEmitter.Sound?.Pause();
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
		}

		protected bool IsSelectedSoundLoopable()
		{
			return IsSoundLoopable(GetSoundQueue(m_cueIdString.Value));
		}

		private static bool IsSoundLoopable(MyCueId cueId)
		{
			return MyAudio.Static.GetCue(cueId)?.Loopable ?? false;
		}

		private MyCueId GetSoundQueue(string nameOrId)
		{
			MyCueId cueId = MySoundPair.GetCueId(nameOrId);
			if (cueId.IsNull)
			{
				foreach (MySoundCategoryDefinition soundCategoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
				{
					foreach (MySoundCategoryDefinition.SoundDescription sound in soundCategoryDefinition.Sounds)
					{
						if (nameOrId == sound.SoundText)
						{
							return MySoundPair.GetCueId(sound.SoundId);
						}
					}
				}
				return cueId;
			}
			return cueId;
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Play()
		{
			RequestPlaySound();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.Stop()
		{
			RequestStopSound();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMySoundBlock.GetSounds(List<string> list)
		{
			list.Clear();
			foreach (MySoundCategoryDefinition soundCategoryDefinition in MyDefinitionManager.Static.GetSoundCategoryDefinitions())
			{
				foreach (MySoundCategoryDefinition.SoundDescription sound in soundCategoryDefinition.Sounds)
				{
					list.Add(sound.SoundId);
				}
			}
		}
	}
}
