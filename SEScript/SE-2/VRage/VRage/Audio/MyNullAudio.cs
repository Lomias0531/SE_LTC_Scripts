using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Utils;
using VRageMath;

namespace VRage.Audio
{
	public class MyNullAudio : IMyAudio
	{
		Dictionary<MyCueId, MySoundData>.ValueCollection IMyAudio.CueDefinitions => null;

		MySoundData IMyAudio.SoloCue
		{
			get;
			set;
		}

		bool IMyAudio.ApplyReverb
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		int IMyAudio.SampleRate => 0;

		float IMyAudio.VolumeMusic
		{
			get;
			set;
		}

		float IMyAudio.VolumeHud
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		float IMyAudio.VolumeGame
		{
			get;
			set;
		}

		float IMyAudio.VolumeVoiceChat
		{
			get;
			set;
		}

		bool IMyAudio.GameSoundIsPaused => true;

		bool IMyAudio.Mute
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		bool IMyAudio.MusicAllowed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyAudio.EnableVoiceChat
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyAudio.UseSameSoundLimiter
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyAudio.EnableReverb
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyAudio.EnableDoppler
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <inheritdoc />
		public bool CacheLoaded
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyAudio.UseVolumeLimiter
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool CanPlay => false;

		event Action<bool> IMyAudio.VoiceChatEnabled
		{
			add
			{
			}
			remove
			{
			}
		}

		void IMyAudio.SetReverbParameters(float diffusion, float roomSize)
		{
		}

		void IMyAudio.SetSameSoundLimiter()
		{
		}

		void IMyAudio.EnableMasterLimiter(bool e)
		{
		}

		void IMyAudio.ChangeGlobalVolume(float level, float time)
		{
		}

		bool IMyAudio.IsValidTransitionCategory(MyStringId transitionCategory, MyStringId musicCategory)
		{
			return false;
		}

		List<MyStringId> IMyAudio.GetCategories()
		{
			return null;
		}

		MySoundData IMyAudio.GetCue(MyCueId cue)
		{
			return null;
		}

		Dictionary<MyStringId, List<MyCueId>> IMyAudio.GetAllMusicCues()
		{
			return null;
		}

		void IMyAudio.PauseGameSounds()
		{
		}

		void IMyAudio.ResumeGameSounds()
		{
		}

		void IMyAudio.PlayMusic(MyMusicTrack? track, int priorityForRandom)
		{
		}

		IMySourceVoice IMyAudio.PlayMusicCue(MyCueId musicCue, bool overrideMusicAllowed)
		{
			return null;
		}

		void IMyAudio.StopMusic()
		{
		}

		void IMyAudio.MuteHud(bool mute)
		{
		}

		bool IMyAudio.HasAnyTransition()
		{
			return false;
		}

		void IMyAudio.LoadData(MyAudioInitParams initParams, ListReader<MySoundData> sounds, ListReader<MyAudioEffect> effects)
		{
		}

		void IMyAudio.UnloadData()
		{
		}

		void IMyAudio.ReloadData()
		{
		}

		void IMyAudio.ReloadData(ListReader<MySoundData> sounds, ListReader<MyAudioEffect> effects)
		{
		}

		void IMyAudio.Update(int stepSizeInMS, Vector3 listenerPosition, Vector3 listenerUp, Vector3 listenerFront, Vector3 listenerVelocity)
		{
		}

		float IMyAudio.SemitonesToFrequencyRatio(float semitones)
		{
			return 0f;
		}

		int IMyAudio.GetUpdating3DSoundsCount()
		{
			return 0;
		}

		int IMyAudio.GetSoundInstancesTotal2D()
		{
			return 0;
		}

		int IMyAudio.GetSoundInstancesTotal3D()
		{
			return 0;
		}

		void IMyAudio.StopUpdatingAll3DCues()
		{
		}

		bool IMyAudio.SourceIsCloseEnoughToPlaySound(Vector3 position, MyCueId cueId, float? customMaxDistance)
		{
			return false;
		}

		void IMyAudio.WriteDebugInfo(StringBuilder sb)
		{
		}

		bool IMyAudio.IsLoopable(MyCueId cueId)
		{
			return false;
		}

		bool IMyAudio.ApplyTransition(MyStringId transitionEnum, int priority, MyStringId? category, bool loop)
		{
			return false;
		}

		IMySourceVoice IMyAudio.PlaySound(MyCueId cue, IMy3DSoundEmitter source, MySoundDimensions type, bool skipIntro, bool skipToEnd)
		{
			return null;
		}

		IMySourceVoice IMyAudio.GetSound(MyCueId cue, IMy3DSoundEmitter source, MySoundDimensions type)
		{
			return null;
		}

		ListReader<IMy3DSoundEmitter> IMyAudio.Get3DSounds()
		{
			return null;
		}

		IMyAudioEffect IMyAudio.ApplyEffect(IMySourceVoice input, MyStringHash effect, MyCueId[] cueIds, float? duration, bool musicEffect)
		{
			return null;
		}

		IMySourceVoice IMyAudio.GetSound(IMy3DSoundEmitter source, int sampleRate, int channels, MySoundDimensions dimension)
		{
			return null;
		}

		public Vector3 GetListenerPosition()
		{
			return Vector3.Zero;
		}

		public void ClearSounds()
		{
		}

		public void EnumerateLastSounds(Action<StringBuilder, bool> a)
		{
		}

		public void DisposeCache()
		{
		}

		public void Preload(string soundFile)
		{
		}
	}
}
