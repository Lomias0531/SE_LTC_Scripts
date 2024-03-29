using VRageMath;

namespace VRage.Audio
{
	public interface IMy3DSoundEmitter
	{
		MyCueId SoundId
		{
			get;
		}

		IMySourceVoice Sound
		{
			get;
			set;
		}

		Vector3D SourcePosition
		{
			get;
		}

		Vector3 Velocity
		{
			get;
		}

		float DopplerScaler
		{
			get;
		}

		float? CustomMaxDistance
		{
			get;
		}

		float? CustomVolume
		{
			get;
		}

		bool Realistic
		{
			get;
		}

		bool Force3D
		{
			get;
		}

		bool Plays2D
		{
			get;
		}

		int SourceChannels
		{
			get;
			set;
		}

		int LastPlayedWaveNumber
		{
			get;
			set;
		}
	}
}
